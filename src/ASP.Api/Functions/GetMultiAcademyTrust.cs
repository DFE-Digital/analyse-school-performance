using ASP.Application.UseCases.MultiAcademyTrusts.GetMultiAcademyTrust;
using ASP.Core.Results;
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
    public override async Task<ApiResult> Run(
        [HttpTrigger(AuthorizationLevel.Function, "get", "post")]
        HttpRequest request,
        CancellationToken cancellationToken
    )
    {
        _logger.LogInformation(request.Method + " " + request.Path + request.QueryString);

        var result =
            from _ in request.ValidateHttpMethod([HttpMethods.Get])
            from uid in request.ValidateParameter("id", p => p.IsRequired().IsDigits())
            from response in _useCase.HandleRequest(new GetMultiAcademyTrustRequest(uid))
            select response;

        return await result.ToApiResultAsync(_options, cancellationToken);
    }
}