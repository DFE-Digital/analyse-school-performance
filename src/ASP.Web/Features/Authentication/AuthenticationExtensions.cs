using System.Security.Claims;
using ASP.Core.Authorization;
using ASP.Infrastructure.Dsi;
using ASP.Infrastructure.Dsi.DsiApiClient;
using ASP.Infrastructure.Dsi.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;

namespace ASP.Web.Features.Authentication
{
    public static class AuthenticationExtensions
    {
        public static IServiceCollection ConfigureDsiAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            var dsiConfiguration = configuration.GetSection(DsiConstants.DsiSection);

            var overallSessionTimeout = TimeSpan.FromMinutes(double.Parse(dsiConfiguration[DsiConstants.SessionTimeout] ?? "20"));

            services
            .Configure<MvcOptions>(options =>
            {
                options.Filters.Add(typeof(UserDetailsActionFilter));
            })
            .AddAuthentication(options =>
            {
                options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
            })
            .AddCookie(options =>
            {
                //If a cookie name is not provided it will default to .ASPNetCore.Cookies
                options.Cookie.Name = DsiConstants.DsiCookieName;

                //The 'DsiCookie' will contain the authentication information for the logged in user.
                //This authentication information is held in the cookie and will be invalidated if there is no user activity and the 'overallSessionTimeout' is reached.
                //Invalidating the authentication information in the cookie does not remove the cookie from the browser - see the 'OnTokenValidated' section below
                options.ExpireTimeSpan = overallSessionTimeout;

                //If 'SlidingExpiration' is set to true then a new cookie is issue if a request is received (user activity on the service) more than halfway
                //through the 'overallSessionTimeout' value.
                //This is to improve the user experience by preventing users having to sign in everytime the 'overallSessionTimeout'
                //is reached
                options.SlidingExpiration = true;

                //This is the high level Access not allowed check
                //If you try to access a controller action that's protected with a Policy that you do not
                //have access rights to then you will be redirected to the 'accessdenied' page
                options.AccessDeniedPath = new PathString(DsiConstants.AccessDeniedRoute);
            })
            //Various settings used to authenticate using OAuth 2.0 and OpenId connect.
            .AddOpenIdConnect(options =>
            {
                options.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.MetadataAddress = dsiConfiguration[DsiConstants.DsiMetadataAddress];
                options.ClientId = dsiConfiguration[DsiConstants.DsiOidcClientId];
                options.ClientSecret = dsiConfiguration[DsiConstants.DsiOidcClientSecret];
                options.ResponseType = OpenIdConnectResponseType.Code;
                options.RequireHttpsMetadata = true;
                options.GetClaimsFromUserInfoEndpoint = true;
                // Make sure DSI cookies adhere to the Content-security policy (fixed browser errorrs)
                options.NonceCookie.SecurePolicy = CookieSecurePolicy.Always;
                options.CorrelationCookie.SecurePolicy = CookieSecurePolicy.Always;
                options.Scope.Clear();
                options.Scope.Add(DsiConstants.DsiScopeOpenId);
                options.Scope.Add(DsiConstants.DsiScopeEmail);
                options.Scope.Add(DsiConstants.DsiScopeProfile);
                options.Scope.Add(DsiConstants.DsiScopeOrganisation);

                //Save the authentication information in the cookie.
                //This information is known as the authentication ticket
                options.SaveTokens = true;

                //This 'CallbackPath' does not require a controller route. 
                //Requests to this path are handled automatically by the OIDC middleware.
                //The path value does need to match the one that's requested in the 'Redirect URL' section of the
                //DSI Service Configuration for the ASP2
                options.CallbackPath = dsiConfiguration[DsiConstants.DsiCallbackPath];

                //This 'SignedOutCallbackPath' does not require a controller route. 
                //Requests to this path are handler automatically by the OIDC middleware.
                //The path value does need to match the one that's requested in the 'Logout redirect URL' section of the
                //DSI Service Configuration for the ASP2
                options.SignedOutCallbackPath = dsiConfiguration[DsiConstants.DsiSignedOutCallbackPath];

                //The URI users are redirected to once they sign out
                options.SignedOutRedirectUri = dsiConfiguration[DsiConstants.DsiSignedOutRedirectUri] ?? "";

                //Taken from GIAP and left unchanged.
                //Probably worth investigating what properties of the id_token we should/need to be validating.
                //'ProtocolValidator' states that the id_token should match the specification
                //defined at 'https://openid.net/specs/openid-connect-core-1_0.html#IDTokenValidation'
                options.ProtocolValidator = new OpenIdConnectProtocolValidator
                {
                    RequireSub = true,
                    RequireStateValidation = false,
                    NonceLifetime = TimeSpan.FromMinutes(60)
                };

                options.Events = new OpenIdConnectEvents
                {
                    OnMessageReceived = context =>
                    {
                        var isSpuriousAuthCbRequest =
                            context.Request.Path == options.CallbackPath &&
                            context.Request.Method == "GET" &&
                            !context.Request.Query.ContainsKey("code");

                        if (isSpuriousAuthCbRequest)
                        {
                            context.HandleResponse();
                            context.Response.Redirect("/error/accessdenied/");
                        }

                        return Task.CompletedTask;
                    },


                    //Should ideally redirect to an exception page
                    //Not implemented yet
                    OnRemoteFailure = context =>
                    {
                        context.HandleResponse();
                        return Task.FromException(context.Failure!);
                    },

                    //Once the id_token is validated in the above steps..
                    //A claims Principle is created using the information about the user that's contained in the id_token
                    //such as user organisation, userid.
                    //The user organisation and userid is then used to make a call to the DSI public API
                    //This API call will return Role information for the user.
                    //This Role information is then used to create a set of Claims that the service can perform checks against.
                    OnTokenValidated = async context =>
                    {
                        if (context.Principal?.Identity?.IsAuthenticated == true)
                        {
                            //These two properties refer to cookie behaviour.
                            //If 'IsPersistent = true' and an 'ExpiresUtc' is set then the cookie will be removed from the
                            //browser once 'ExpiresUtc' is reached
                            context.Properties = new()
                            {
                                IsPersistent = true,
                                ExpiresUtc = DateTime.UtcNow.Add(overallSessionTimeout)
                            };

                            var principal = context.Principal;

                            var organisation = principal.GetOrganisation();
                            if (organisation == null)
                            {
                                // Just return here, we don't want to throw an exception if the user doesn't have an organisation
                                // as they won't then be able to sign out and select a different user (at least on Dev as it will show
                                // the Developer Exception page which doesn't have a sign out link)
                                return;
                            }
                            else
                            {
                                var authenticatedUserInfo = new AuthenticatedUserInfo() {
                                    UserId = principal.GetUserId()
                                };

                                var dsiPublicApiClient = context.HttpContext.RequestServices.GetService<IDsiApiClient>();

                                //userAccess contains the Role information needed to construct a set of Claims for use in the service
                                var userAccessResult = await dsiPublicApiClient!.GetUserAccess(dsiConfiguration[DsiConstants.DsiServiceId]!, organisation.Id!, authenticatedUserInfo.UserId);

                                var userAccess = userAccessResult.GetValueOrDefault(new UserAccess());
                                if (!userAccess.Roles.Any())
                                {
                                    // Just return here, we don't want to throw an exception if the user doesn't have an organisation
                                    // as they won't then be able to sign out and select a different user (at least on Dev as it will show
                                    // the Developer Exception page which doesn't have a sign out link)
                                    return;
                                }
                                else
                                {
                                    //A Role is Claim with Type Role
                                    //A user may have more than one Role
                                    //Individual Claims would need to be added for each one.
                                    var claims = new List<Claim> {
                                        new Claim(ClaimTypes.Role, userAccess.Roles.First().Code)
                                    };

                                    //Add user name claims
                                    claims.AddRange(principal.FindAll(c => c.Type == ClaimTypes.GivenName || c.Type == ClaimTypes.Surname));

                                    if (organisation.UniqueReferenceNumber != null)
                                    {
                                        claims.Add(new Claim(CustomClaimTypes.UniqueReferenceNumber, organisation.UniqueReferenceNumber));
                                    }

                                    if (organisation.EstablishmentNumber != null)
                                    {
                                        claims.Add(new Claim(CustomClaimTypes.EstablishmentNumber, organisation.EstablishmentNumber));
                                    }

                                    //Create a new ClaimsPrincipal containing the Claims of the logged in user taken from the API
                                    //This overrides the Principal that is created from the id_token that's sent as part of the authentication process.
                                    //The original Claim information in that Principal may need to be retained.
                                    context.Principal = new ClaimsPrincipal(new ClaimsIdentity(claims, DsiConstants.AuthenticationMethod));
                                }
                            }
                        }
                    }
                };
            });

            return services;
        }
    }
}
