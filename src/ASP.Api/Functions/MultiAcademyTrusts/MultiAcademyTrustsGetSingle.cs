using ASP.Core.Results;
using ASP.Domain.MultiAcademyTrusts.UseCases.GetMultiAcademyTrust;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System.Net;

namespace ASP.Api.Functions.MultiAcademyTrusts;

public class MultiAcademyTrustsGetSingle : ApiFunction
{
    private readonly ILogger<MultiAcademyTrustsGetSingle> _logger;
    private readonly IGetMultiAcademyTrust _useCase;
    private readonly ApiResultConverter _resultConverter;

    public MultiAcademyTrustsGetSingle(
        ILogger<MultiAcademyTrustsGetSingle> logger,
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

    [OpenApiOperation(operationId: "MultiAcademyTrustsGetSingle", tags: ["Multi-Academy Trusts"], 
        Description = "Retrieves details for a specific multi-academy trust based on a given UID.")]
    [OpenApiParameter(name: "uid", In = ParameterLocation.Path, Required = true, 
        Description = "The UID of the multi-academy trust (numeric).")]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(Client.LookupValueWithUid), 
        Description = "Details of the multi-academy trust for the given UID.")]
    [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.BadRequest, 
        Description = "Bad request: Invalid UID parameter.")]
    [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.NotFound, 
        Description = "Not found: No multi-academy trust found for the given UID.")]
    [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.MethodNotAllowed, 
        Description = "Method not allowed: The HTTP method POST is not allowed.")]
    
    [Function("MultiAcademyTrustsGetSingle")]
    public override async Task<ActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Function, "get", "put", "post", "delete", Route = "multi-academy-trusts/{uid:int}")]
        HttpRequest request,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation(request.Method + " " + request.Path + request.QueryString);

        var result =
            from _ in request.ValidateHttpMethod([HttpMethods.Get])
            from uid in request.ValidatePathParameter("uid", p => p.IsRequired().IsDigits())
            from response in _useCase.HandleRequest(new GetMultiAcademyTrustRequest(uid))
            select response;

        return await _resultConverter.ConvertToApiResultAsync(result, cancellationToken);
    }
}