using ASP.Application.UseCases.ContentPage.GetAllContentTemplates;
using ASP.Core.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

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
