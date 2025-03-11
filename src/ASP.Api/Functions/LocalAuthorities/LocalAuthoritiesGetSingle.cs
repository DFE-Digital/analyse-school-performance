using ASP.Core.Results;
using ASP.Domain.LocalAuthorities.UseCases.GetLocalAuthority;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System.Net;

namespace ASP.Api.Functions.LocalAuthorities;

public class LocalAuthoritiesGetSingle : ApiFunction
{
    private readonly ILogger<LocalAuthoritiesGetSingle> _logger;
    private readonly IGetLocalAuthority _useCase;
    private readonly ApiResultConverter _resultConverter;

    public LocalAuthoritiesGetSingle(
        ILogger<LocalAuthoritiesGetSingle> logger,
        IGetLocalAuthority useCase,
        ApiResultConverter resultConverter
    )
    {
        _logger = logger
            ?? throw new ArgumentNullException(nameof(logger));

        _useCase = useCase
            ?? throw new ArgumentNullException(nameof(useCase));

        _resultConverter = resultConverter
            ?? throw new ArgumentNullException(nameof(resultConverter));
    }

    [OpenApiOperation(operationId: "LocalAuthoritiesGetSingle", tags: ["Local Authorities"], 
        Description = "Retrieves details for a specific local authority based on a given code.")]
    [OpenApiParameter(name: "code", In = ParameterLocation.Path, Required = true, 
        Description = "The local authority code (3 digits).")]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(Client.LookupValueWithCode), 
        Description = "Details of the local authority for the specified code.")]
    [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.BadRequest, 
        Description = "Bad request: Invalid local authority code parameter.")]
    [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.NotFound, 
        Description = "Not found: No local authority found for the specified code.")]
    [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.MethodNotAllowed, 
        Description = "Method not allowed: The HTTP method POST is not allowed.")]

    [Function("LocalAuthoritiesGetSingle")]
    public override async Task<ActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Function, "get", "put", "post", "delete", Route = "local-authorities/{code:int}")]
        HttpRequest request,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation(request.Method + " " + request.Path + request.QueryString);

        var result =
            from _ in request.ValidateHttpMethod([HttpMethods.Get])
            from laCode in request.ValidatePathParameter("code", p => p.IsRequired().IsDigits().HasLength(3))
            from response in _useCase.HandleRequest(new GetLocalAuthorityRequest(laCode))
            select response.ForApiClient();

        return await _resultConverter.ConvertToApiResultAsync(result, cancellationToken);
    }
}