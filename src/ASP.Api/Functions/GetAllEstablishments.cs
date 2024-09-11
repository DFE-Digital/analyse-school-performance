using ASP.Application.UseCases.Establishments.GetAllEstablishments;
using ASP.Core.Scope;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace ASP.Api.Functions;

public class GetAllEstablishments : ApiFunction
{
    private readonly ILogger _logger;
    private readonly IGetAllEstablishments _useCase;
    private readonly ErrorHandlingOptions _options;

    public GetAllEstablishments(
        ILoggerFactory loggerFactory,
        IGetAllEstablishments useCase,
        IOptions<ErrorHandlingOptions> options
    )
    {
        _logger = loggerFactory.CreateLogger<GetEstablishmentDetails>();
        _useCase = useCase;
        _options = (options ?? throw new ArgumentNullException(nameof(options)))
            .Value;
    }
    
    [Function("GetAllEstablishments")]
    public override async Task<ApiResult> Run(
        [HttpTrigger(AuthorizationLevel.Function, "get", "post")]
        HttpRequest req,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation(req.Method + " " + req.Path + req.QueryString);

        return await RequestValidation.RequiredHttpMethod(req, [HttpMethods.Get])
                .Then(_ => RequestValidation.RequiredParameter<ScopeType>(req, "scope")
                    .Then(scope => RequestValidation
                        .RequiredParameterIf(req, "scopeIdentifier", scope == ScopeType.All)
                        .Then(scopeIdentifier => RequestValidation.OptionalParameter(req, "page")
                            .Then(page => RequestValidation.NumericParameter(page, "page"))
                            .Then(page => RequestValidation.OptionalParameter(req, "resultsPerPage")
                                .Then(resultsPerPage =>
                                    RequestValidation.NumericParameter(resultsPerPage, "resultsPerPage"))
                                .Then(resultsPerPage => _useCase.HandleRequest(
                                    new GetAllEstablishmentsRequest(scope,
                                        scopeIdentifier, page, resultsPerPage)))))))
            .ToApiResultAsync(_options, cancellationToken);
    }
}