using ASP.Core.Results;
using ASP.Domain.Templating;
using ASP.Domain.Templating.UseCases.UpdateContentTemplate;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System.Net;

namespace ASP.Api.Functions.ContentTemplates;

public class ContentTemplatesUpdateSingle : ApiFunction
{
    private readonly ILogger<ContentTemplatesUpdateSingle> _logger;
    private readonly IUpdateContentTemplate _useCase;
    private readonly ApiResultConverter _resultConverter;

    public ContentTemplatesUpdateSingle(
        ILogger<ContentTemplatesUpdateSingle> logger,
        IUpdateContentTemplate useCase,
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

    [OpenApiOperation(operationId: "ContentTemplatesUpdateSingle", tags: ["Content Templates"], 
        Description = "Updates a specific content template based on a given ID.")]
    [OpenApiParameter(name: "id", In = ParameterLocation.Path, Required = true, 
        Description = "The unique identifier of the content template.")]
    [OpenApiParameter(name: "revision", In = ParameterLocation.Query, Required = false, 
        Description = "The revision identifier of the content template.")]
    [OpenApiRequestBody(contentType: "application/json", bodyType: typeof(ContentTemplate), Required = true, 
        Description = "The content template details to update.")]
    [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.OK, 
        Description = "Updated content template details.")]
    [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.BadRequest, 
        Description = "Bad request: Missing or invalid parameters.")]
    [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.NotFound, 
        Description = "Not found: Content template not found for the given ID and revision.")]
    [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.MethodNotAllowed, 
        Description = "Method not allowed: The HTTP method GET is not allowed.")]

    [Function("ContentTemplatesUpdateSingle")]
    public override async Task<ActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Function, "post", Route = "content-templates/{id}")]
        HttpRequest request,
        CancellationToken cancellationToken
    )
    {
        _logger.LogInformation(request.Method + " " + request.Path + request.QueryString);

        var result =
            from _ in request.ValidateHttpMethod([HttpMethods.Post])
            from id in request.ValidatePathParameter("id", p => p.IsRequired())
            from revision in request.ValidateQueryStringParameter("revision", p => p.IsOptional().IsNotEmpty())
            from contentTemplate in request.ValidateBodyAsync<ContentTemplate>()
            from response in _useCase.HandleRequest(new UpdateContentTemplateRequest(id, revision, contentTemplate))
            select response;

        return await _resultConverter.ConvertToApiResultAsync(result, cancellationToken);
    }
}