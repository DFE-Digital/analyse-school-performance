using ASP.Application.UseCases.LocalAuthorities.GetLocalAuthority;
using ASP.Core.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace ASP.Api.Functions;

public class GetLocalAuthority : ApiFunction
{
    private readonly ILogger _logger;
    private readonly IGetLocalAuthority _useCase;
    private readonly ErrorHandlingOptions _options;

    public GetLocalAuthority(ILoggerFactory loggerFactory,
        IGetLocalAuthority useCase,
        IOptions<ErrorHandlingOptions> options)
    {
        _logger = loggerFactory.CreateLogger<GetLocalAuthority>();
        _useCase = useCase;
        _options = (options ?? throw new ArgumentNullException(nameof(options)))
            .Value;
    }

    [Function("GetLocalAuthority")]
    public override async Task<ApiResult> Run(
        [HttpTrigger(AuthorizationLevel.Function, "get", "post")]
        HttpRequest request,
        CancellationToken cancellationToken
    )
    {
        _logger.LogInformation(request.Method + " " + request.Path + request.QueryString);

        var result =
            from _ in request.ValidateHttpMethod([HttpMethods.Get])
            from laCode in request.ValidateParameter("code", p => p.IsRequired().IsDigits().HasLength(3))
            from response in _useCase.HandleRequest(new GetLocalAuthorityRequest(laCode))
            select response;

        return await result.ToApiResultAsync(_options, cancellationToken);
    }
}