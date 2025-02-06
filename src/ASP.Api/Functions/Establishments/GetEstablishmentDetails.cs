using ASP.Core.Results;
using ASP.Domain.Establishments.UseCases.GetEstablishmentDetails;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System.Net;

namespace ASP.Api.Functions.Establishments;

public class GetEstablishmentDetails : ApiFunction
{
    private readonly ILogger<GetEstablishmentDetails> _logger;
    private readonly IGetEstablishmentDetails _useCase;
    private readonly ApiResultConverter _resultConverter;

    public GetEstablishmentDetails(
        ILogger<GetEstablishmentDetails> logger,
        IGetEstablishmentDetails useCase,
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

    [Function("GetEstablishmentDetails")]
    [OpenApiOperation(operationId: "GetEstablishmentDetails", tags: ["Establishments"], Description = "Retrieves details for a specific establishment based on URN.")]
    [OpenApiParameter(name: "urn", In = ParameterLocation.Query, Required = true, Description = "The URN of the establishment.")]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(Client.Establishments.EstablishmentDetails), Description = "Details of the establishment for the specified URN.")]
    [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.BadRequest, Description = "Bad request: Invalid URN parameter.")]
    [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.NotFound, Description = "Not found: No establishment found for the specified URN.")]
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
            from response in _useCase.HandleRequest(new GetEstablishmentDetailsRequest(urn))
            select response.ForApiClient();

        return await _resultConverter.ConvertToApiResultAsync(result, cancellationToken);
    }
}