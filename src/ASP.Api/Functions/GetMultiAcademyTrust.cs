using ASP.Application.UseCases.MultiAcademyTrusts.GetMultiAcademyTrust;
using ASP.Core.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace ASP.Api.Functions;

public class GetMultiAcademyTrust : ApiFunction
{
    private readonly ILogger<GetMultiAcademyTrust> _logger;
    private readonly IGetMultiAcademyTrust _useCase;
    private readonly ApiResultConverter _resultConverter;

    public GetMultiAcademyTrust(
        ILogger<GetMultiAcademyTrust> logger,
        IGetMultiAcademyTrust useCase,
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

    [Function("GetMultiAcademyTrust")]
    public override async Task<ActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Function, "get", "post")]
        HttpRequest request,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation(request.Method + " " + request.Path + request.QueryString);

        var result =
            from _ in request.ValidateHttpMethod([HttpMethods.Get])
            from uid in request.ValidateParameter("id", p => p.IsRequired().IsDigits())
            from response in _useCase.HandleRequest(new GetMultiAcademyTrustRequest(uid))
            select response;

        return await _resultConverter.ConvertToApiResultAsync(result, cancellationToken);
    }
}