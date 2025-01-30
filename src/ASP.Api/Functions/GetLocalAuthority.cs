using ASP.Domain.LocalAuthorities.UseCases.DTO;
using ASP.Domain.LocalAuthorities.UseCases.GetLocalAuthority;
using ASP.Core.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System.Net;

namespace ASP.Api.Functions;

public class GetLocalAuthority : ApiFunction
{
    private readonly ILogger<GetLocalAuthority> _logger;
    private readonly IGetLocalAuthority _useCase;
    private readonly ApiResultConverter _resultConverter;

    public GetLocalAuthority(
        ILogger<GetLocalAuthority> logger,
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

    [Function("GetLocalAuthority")]
    [OpenApiOperation(operationId: "GetLocalAuthority", tags: ["Local Authorities"], Description = "Retrieves details for a specific local authority based on the provided code.")]
    [OpenApiParameter(name: "code", In = ParameterLocation.Query, Required = true, Description = "The local authority code (3 digits).")]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(LocalAuthorityDTO), Description = "Details of the local authority for the specified code.")]
    [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.BadRequest, Description = "Bad request: Invalid local authority code parameter.")]
    [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.NotFound, Description = "Not found: No local authority found for the specified code.")]
    [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.MethodNotAllowed, Description = "Method not allowed: The HTTP method POST is not allowed.")]
    public override async Task<ActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Function, "get", "post")]
        HttpRequest request,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation(request.Method + " " + request.Path + request.QueryString);

        var result =
            from _ in request.ValidateHttpMethod([HttpMethods.Get])
            from laCode in request.ValidateParameter("code", p => p.IsRequired().IsDigits().HasLength(3))
            from response in _useCase.HandleRequest(new GetLocalAuthorityRequest(laCode))
            select response;

        return await _resultConverter.ConvertToApiResultAsync(result, cancellationToken);
    }
}