using ASP.Api.Client.ContentTemplates;
using ASP.Core.Results;
using ASP.Domain.Templating.UseCases.GetAllContentTemplates;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Extensions.Logging;
using System.Net;

namespace ASP.Api.Functions.ContentTemplates;

public class ContentTemplatesGetAll : ApiFunction
{
    private readonly ILogger<ContentTemplatesGetAll> _logger;
    private readonly IGetAllContentTemplates _useCase;
    private readonly ApiResultConverter _resultConverter;

    public ContentTemplatesGetAll(
        ILogger<ContentTemplatesGetAll> logger,
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

    [OpenApiOperation(operationId: "ContentTemplatesGetAll", tags: ["Content Templates"], 
        Description = "Retrieves all published content templates.")]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(List<ContentTemplate>), 
        Description = "A list of all published content templates.")]
    [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.NotFound, 
        Description = "Not found: Could not find any published content templates.")]
    [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.BadRequest, 
        Description = "Bad request: Missing or invalid parameters.")]
    [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.MethodNotAllowed, 
        Description = "Method not allowed: The HTTP method POST/PUT/DELETE is not allowed.")]

    [Function("ContentTemplatesGetAll")]
    public override async Task<ActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Function, "get", "put", "post", "delete", Route = "content-templates")]
        HttpRequest request,
        CancellationToken cancellationToken
    )
    {
        _logger.LogInformation($"{request.Method} {request.Path + request.QueryString}");

        var result =
            from _ in request.ValidateHttpMethod([HttpMethods.Get])
            from response in _useCase.HandleRequest()
            select response.ForApiClient();

        return await _resultConverter.ConvertToApiResultAsync(result, cancellationToken);
    }
}
