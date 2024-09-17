using ASP.Application.UseCases.Establishments.GetAllEstablishments;
using ASP.Core.Results;
using ASP.Core.Scoping;
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
        HttpRequest request,
        CancellationToken cancellationToken
    )
    {
        _logger.LogInformation(request.Method + " " + request.Path + request.QueryString);

        var result =
            from _ in request.ValidateHttpMethod([HttpMethods.Get])
            from scope in request.ValidateParameter("scope", p => p.IsRequired().IsEnum<ScopeType>())
            from scopeIdentifier in request.ValidateParameter("scopeIdentifier", p => p.IsRequiredIf(scope != ScopeType.All))
            from page in request.ValidateParameter("page", p => p.IsOptional().IsNumeric())
            from resultsPerPage in request.ValidateParameter("resultsPerPage", p => p.IsOptional().IsNumeric())
            from response in _useCase.HandleRequest(new GetAllEstablishmentsRequest(
                scope,
                scopeIdentifier,
                page,
                resultsPerPage
            ))
            select response;

        return await result.ToApiResultAsync(_options, cancellationToken);
    }
}