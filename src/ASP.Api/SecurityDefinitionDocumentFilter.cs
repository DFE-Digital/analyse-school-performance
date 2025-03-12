using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Abstractions;
using Microsoft.OpenApi.Models;

namespace ASP.Api;

public class SecurityDefinitionDocumentFilter : IDocumentFilter
{
    public void Apply(IHttpRequestDataObject req, OpenApiDocument document)
    {
        if (document.Components == null)
            document.Components = new OpenApiComponents();

        document.Components.SecuritySchemes = new Dictionary<string, OpenApiSecurityScheme>
        {
            {
                "function_key", new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.ApiKey,
                    Name = "x-functions-key",
                    In = ParameterLocation.Header,
                    Description = "Function key authentication",
                    Scheme = "ApiKeyScheme"
                }
            }
        };

        document.SecurityRequirements = new List<OpenApiSecurityRequirement>
        {
            new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "function_key"
                        },
                        In = ParameterLocation.Header
                    },
                    new List<string>()
                }
            }
        };
    }
}