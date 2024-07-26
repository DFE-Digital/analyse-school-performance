using ASP.Application.UseCases.LocalAuthority;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace ASP.Api.Functions;

public class GetLocalAuthority : ApiFunction
{
    private readonly ILogger _logger;
    private readonly IGetLocalAuthorityUseCase _useCase;
    private readonly ErrorHandlingOptions _options;

    public GetLocalAuthority(ILoggerFactory loggerFactory,
        IGetLocalAuthorityUseCase useCase,
        IOptions<ErrorHandlingOptions> options)
    {
        _logger = loggerFactory.CreateLogger<GetLocalAuthority>();
        _useCase = useCase;
        _options = (options ?? throw new ArgumentNullException(nameof(options)))
            .Value;
    }
    
    [Function("GetLocalAuthority")]
    public override async Task<ApiResult> Run([HttpTrigger(AuthorizationLevel.Function, "get", "post")] HttpRequest req, CancellationToken cancellationToken)
    {
        _logger.LogInformation(req.Method + " " + req.Path + req.QueryString);

        return await RequestValidation.RequiredHttpMethod(req, [HttpMethods.Get])
            .Then(_ => RequestValidation.RequiredParameter(req, "code")
                .Then(code => _useCase.HandleRequest(new GetLocalAuthorityRequest(code))))
            .ToApiResultAsync(_options, cancellationToken);
    }
}