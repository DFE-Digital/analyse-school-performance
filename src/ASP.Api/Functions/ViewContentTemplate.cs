using ASP.Application.UseCases.ContentTemplates.ViewContentTemplate;
using ASP.Core.Results;
using ASP.Core.Templating;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System.Net;

namespace ASP.Api.Functions;

public class ViewContentTemplate : ApiFunction
{
    private readonly ILogger<ViewContentTemplate> _logger;
    private readonly IViewContentTemplate _useCase;
    private readonly ApiResultConverter _resultConverter;

    public ViewContentTemplate(
        ILogger<ViewContentTemplate> logger,
        IViewContentTemplate useCase,
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

    [Function("ViewContentTemplate")]
    [OpenApiOperation(operationId: "ViewContentTemplate", tags: ["Content Templates"], Description = "Retrieves details for a specific content template based on the provided ID.")]
    [OpenApiParameter(name: "id", In = ParameterLocation.Query, Required = true, Description = "The unique identifier of the content template.")]
    [OpenApiParameter(name: "revision", In = ParameterLocation.Query, Required = false, Description = "The revision identifier of the content template.")]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(ContentTemplate), Description = "Details of the specified content template.")]
    [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.BadRequest, Description = "Bad request: Missing or invalid parameters.")]
    [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.NotFound, Description = "Not found: Content template not found for the given ID and revision.")]
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
            from id in request.ValidateParameter("id", p => p.IsRequired())
            from revision in request.ValidateParameter("revision", p => p.IsOptional())
            from response in _useCase.HandleRequest(new ViewContentTemplateRequest(id, revision))
            select response;

        return await _resultConverter.ConvertToApiResultAsync(result, cancellationToken);
    }
}
