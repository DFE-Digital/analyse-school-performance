using System.Net;
using ASP.Core.Results;
using ASP.Domain.Establishments.UseCases.DTO;
using ASP.Domain.Establishments.UseCases.GetLinkedEstablishments;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;

namespace ASP.Api.Functions;

public class GetLinkedEstablishments : ApiFunction
{
    
    private readonly ILogger<GetLinkedEstablishments> _logger;
    private readonly IGetLinkedEstablishments _useCase;
    private readonly ApiResultConverter _resultConverter;

    public GetLinkedEstablishments(
        ILogger<GetLinkedEstablishments> logger,
        IGetLinkedEstablishments useCase,
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
    
    [Function("GetLinkedEstablishments")]
    [OpenApiOperation(operationId: "GetLinkedEstablishments", tags: ["Establishments"], Description = "Retrieves linked establishments for a specific establishment based on URN.")]
    [OpenApiParameter(name: "urn", In = ParameterLocation.Query, Required = true, Description = "The URN of the establishment.")]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(LinkedEstablishmentsResponseDTO), Description = "Details of the linked establishments for the specified URN.")]
    [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.BadRequest, Description = "Bad request: Invalid URN parameter.")]
    [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.NotFound, Description = "Not found: Could not find Establishment with URN {urn}.")]
    [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.MethodNotAllowed, Description = "Method not allowed: The HTTP method POST is not allowed.")]
    public override async Task<ActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Function, "get", "post")]
        HttpRequest request,
        CancellationToken cancellationToken
    )
    {
        _logger.LogInformation(request.Method + " " + request.Path + request.QueryString);

        var result =
            from _ in request.ValidateHttpMethod([HttpMethods.Get])
            from urn in request.ValidateParameter("urn", p => p.IsRequired().IsDigits().HasLength(6))
            from response in _useCase.HandleRequest(new GetLinkedEstablishmentsRequest(urn))
            select response;

        return await _resultConverter.ConvertToApiResultAsync(result, cancellationToken);
    }
}