using ASP.Application.UseCases.LocalAuthorities.GetLocalAuthority;
using ASP.Core.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace ASP.Api.Functions;

public class GetLocalAuthority : ApiFunction
{
    private readonly ILogger<GetLocalAuthority> _logger;
    private readonly IGetLocalAuthority _useCase;
    private readonly ApiResultConverter _resultConverter;

    public GetLocalAuthority(
        ILogger<GetLocalAuthority> logger,
        IGetLocalAuthority useCase,
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

    [Function("GetLocalAuthority")]
    public override async Task<ActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Function, "get", "post")]
        HttpRequest request,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation(request.Method + " " + request.Path + request.QueryString);

        var result =
            from _ in request.ValidateHttpMethod([HttpMethods.Get])
            from laCode in request.ValidateParameter("code", p => p.IsRequired().IsDigits().HasLength(3))
            from response in _useCase.HandleRequest(new GetLocalAuthorityRequest(laCode))
            select response;

        return await _resultConverter.ConvertToApiResultAsync(result, cancellationToken);
    }
}