using ASP.Application.UseCases.ContentTemplates.UpdateContentTemplate;
using ASP.Core.Results;
using ASP.Core.Templating;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace ASP.Api.Functions;

public class UpdateContentTemplate : ApiFunction
{
    private readonly ILogger<UpdateContentTemplate> _logger;
    private readonly IUpdateContentTemplate _useCase;
    private readonly ApiResultConverter _resultConverter;

    public UpdateContentTemplate(
        ILogger<UpdateContentTemplate> logger,
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

    [Function("UpdateContentTemplate")]
    public override async Task<ActionResult> Run(
    	[HttpTrigger(AuthorizationLevel.Function, "get", "post")] 
    	HttpRequest request, 
    	CancellationToken cancellationToken
	)
    {
        _logger.LogInformation(request.Method + " " + request.Path + request.QueryString);

        var result =
            from _ in request.ValidateHttpMethod([HttpMethods.Post])
            from id in request.ValidateParameter("id", p => p.IsRequired())
            from revision in request.ValidateParameter("revision", p => p.IsOptional())
            from contentTemplate in request.ValidateBodyAsync<ContentTemplate>()
            from response in _useCase.HandleRequest(new UpdateContentTemplateRequest(id, revision, contentTemplate))
            select response;

        return await _resultConverter.ConvertToApiResultAsync(result, cancellationToken);
    }
}