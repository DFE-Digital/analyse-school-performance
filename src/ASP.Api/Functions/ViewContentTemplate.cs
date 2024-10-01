using ASP.Application.UseCases.ContentTemplates.ViewContentTemplate;
using ASP.Core.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

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
