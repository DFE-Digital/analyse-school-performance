using ASP.Domain.MultiAcademyTrusts.UseCases.GetMultiAcademyTrust;
using ASP.Core.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System.Net;

namespace ASP.Api.Functions.MultiAcademyTrusts;

public class GetMultiAcademyTrust : ApiFunction
{
    private readonly ILogger<GetMultiAcademyTrust> _logger;
    private readonly IGetMultiAcademyTrust _useCase;
    private readonly ApiResultConverter _resultConverter;

    public GetMultiAcademyTrust(
        ILogger<GetMultiAcademyTrust> logger,
        IGetMultiAcademyTrust useCase,
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

    [Function("GetMultiAcademyTrust")]
    [OpenApiOperation(operationId: "GetMultiAcademyTrust", tags: ["Multi Academy Trust"], Description = "Retrieves details for a specific Multi Academy Trust based on the provided ID.")]
    [OpenApiParameter(name: "uid", In = ParameterLocation.Path, Required = true, Description = "The UID of the Multi Academy Trust (numeric).")]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(Client.LookupValueWithId), Description = "Details of the Multi Academy Trust for the specified ID.")]
    [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.BadRequest, Description = "Bad request: Invalid UID parameter.")]
    [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.NotFound, Description = "Not found: No Multi Academy Trust found for the specified UID.")]
    [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.MethodNotAllowed, Description = "Method not allowed: The HTTP method POST is not allowed.")]
    public override async Task<ActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Function, "get", "post", "put", "delete", Route = "multi-academy-trusts/{uid:int}")]
        HttpRequest request,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation(request.Method + " " + request.Path + request.QueryString);

        var result =
            from _ in request.ValidateHttpMethod([HttpMethods.Get])
            from uid in request.ValidateRouteParameter("uid", p => p.IsRequired().IsDigits())
            from response in _useCase.HandleRequest(new GetMultiAcademyTrustRequest(uid))
            select response;

        return await _resultConverter.ConvertToApiResultAsync(result, cancellationToken);
    }
}