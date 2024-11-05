using ASP.Application.UseCases.ContentPage.GetAllContentTemplates;
using ASP.Core.Results;
using ASP.Core.Templating;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Extensions.Logging;
using System.Net;

namespace ASP.Api.Functions;

public class GetAllContentTemplates : ApiFunction
{
    private readonly ILogger<GetAllContentTemplates> _logger;
    private readonly IGetAllContentTemplates _useCase;
    private readonly ApiResultConverter _resultConverter;

    public GetAllContentTemplates(
        ILogger<GetAllContentTemplates> logger,
        IGetAllContentTemplates useCase,
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

    [Function("GetAllContentTemplates")]
    [OpenApiOperation(operationId: "GetAllContentTemplates", tags: ["Content Templates"], Description = "Retrieves all available content templates.")]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(List<ContentTemplate>), Description = "A list of all content templates.")]
    [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.NotFound, Description = "Not found: Could not find any published Content Templates.")]
    [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.BadRequest, Description = "Bad request: Missing or invalid parameters.")]
    [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.MethodNotAllowed, Description = "Method not allowed: The HTTP method POST is not allowed.")]
    public override async Task<ActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Function, "get")]
        HttpRequest request,
        CancellationToken cancellationToken
    )
    {
        _logger.LogInformation($"{request.Method} {request.Path + request.QueryString}");

        var result =
            from _ in request.ValidateHttpMethod([HttpMethods.Get])
            from response in _useCase.HandleRequest()
            select response;

        return await _resultConverter.ConvertToApiResultAsync(result, cancellationToken);
    }
}
