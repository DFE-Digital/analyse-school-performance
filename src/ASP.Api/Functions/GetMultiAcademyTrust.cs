using ASP.Application.UseCases.MultiAcademyTrusts.GetMultiAcademyTrust;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace ASP.Api.Functions;

public class GetMultiAcademyTrust : ApiFunction
{
    private readonly ILogger _logger;
    private readonly IGetMultiAcademyTrust _useCase;
    private readonly ErrorHandlingOptions _options;

    public GetMultiAcademyTrust(ILoggerFactory loggerFactory,
        IGetMultiAcademyTrust useCase,
        IOptions<ErrorHandlingOptions> options)
    {
        _logger = loggerFactory.CreateLogger<GetMultiAcademyTrust>();
        _useCase = useCase;
        _options = (options ?? throw new ArgumentNullException(nameof(options)))
            .Value;
    }
    
    [Function("GetMultiAcademyTrust")]
    public override async Task<ApiResult> Run([HttpTrigger(AuthorizationLevel.Function, "get", "post")] HttpRequest req,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation(req.Method + " " + req.Path + req.QueryString);

        return await RequestValidation.RequiredHttpMethod(req, [HttpMethods.Get])
            .Then(_ => RequestValidation.RequiredParameter(req, "id")
                .Then(id => _useCase.HandleRequest(new GetMultiAcademyTrustRequest(id))))
            .ToApiResultAsync(_options, cancellationToken);
    }
}