using ASP.Api.Client.ContentTemplates;
using ASP.Core.Results;
using ASP.Domain.Templating.UseCases.GetContentTemplate;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System.Net;

namespace ASP.Api.Functions.ContentTemplates;

public class ContentTemplatesGetSingle : ApiFunction
{
    private readonly ILogger<ContentTemplatesGetSingle> _logger;
    private readonly IGetContentTemplate _useCase;
    private readonly ApiResultConverter _resultConverter;

    public ContentTemplatesGetSingle(
        ILogger<ContentTemplatesGetSingle> logger,
        IGetContentTemplate useCase,
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

    [OpenApiOperation(operationId: "ContentTemplatesGetSingle", tags: ["Content Templates"], 
        Description = "Retrieves details for a specific content template based on a given ID.")]
    [OpenApiParameter(name: "id", In = ParameterLocation.Path, Required = true, 
        Description = "The unique identifier of the content template.")]
    [OpenApiParameter(name: "revision", In = ParameterLocation.Query, Required = false, 
        Description = "The revision identifier of the content template.")]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(ContentTemplate), 
        Description = "Details of the specified content template.")]
    [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.BadRequest, 
        Description = "Bad request: Missing or invalid parameters.")]
    [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.NotFound, 
        Description = "Not found: Content template not found for the given ID and revision.")]
    [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.MethodNotAllowed, 
        Description = "Method not allowed: The HTTP method PUT/POST/DELETE is not allowed.")]

    [Function("ContentTemplatesGetSingle")]
    public override async Task<ActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Function, "get", "put", "delete", Route = "content-templates/{id}")]
        HttpRequest request,
        CancellationToken cancellationToken
    )
    {
        _logger.LogInformation(request.Method + " " + request.Path + request.QueryString);

        var result =
            from _ in request.ValidateHttpMethod([HttpMethods.Get])
            from id in request.ValidatePathParameter("id", p => p.IsRequired())
            from revision in request.ValidateQueryStringParameter("revision", p => p.IsOptional().IsNotEmpty())
            from response in _useCase.HandleRequest(new GetContentTemplateRequest(id, revision))
            select response.ForApiClient();

        return await _resultConverter.ConvertToApiResultAsync(result, cancellationToken);
    }
}
