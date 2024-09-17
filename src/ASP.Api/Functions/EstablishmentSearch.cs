using ASP.Application.UseCases.Establishments.EstablishmentSearch;
using ASP.Core.Results;
using ASP.Core.Scoping;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace ASP.Api.Functions
{
    public class EstablishmentSearch : ApiFunction
    {
        private readonly ILogger _logger;
        private readonly IEstablishmentSearch _useCase;
        private readonly ErrorHandlingOptions _options;

        public EstablishmentSearch(
            ILoggerFactory loggerFactory,
            IEstablishmentSearch useCase,
            IOptions<ErrorHandlingOptions> options
        )
        {
            _logger = loggerFactory.CreateLogger<EstablishmentSearch>();
            _useCase = useCase ?? throw new ArgumentNullException(nameof(useCase));
            _options = (options ?? throw new ArgumentNullException(nameof(options)))
                .Value;
        }

        [Function("EstablishmentSearch")]
        public override async Task<ApiResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post")]
            HttpRequest request,
            CancellationToken cancellationToken
        )
        {
            _logger.LogInformation(request.Method + " " + request.Path + request.QueryString);

            var result =
                from _ in request.ValidateHttpMethod([HttpMethods.Get])
                from searchTerm in request.ValidateParameter("searchTerm", p => p.IsRequired())
                from scope in request.ValidateParameter("scope", p => p.IsRequired().IsEnum<ScopeType>())
                from scopeIdentifier in request.ValidateParameter("scopeIdentifier", p => p.IsRequiredIf(scope != ScopeType.All))
                from page in request.ValidateParameter("page", p => p.IsOptional().IsNumeric())
                from resultsPerPage in request.ValidateParameter("resultsPerPage", p => p.IsOptional().IsNumeric())
                from response in _useCase.HandleRequest(new EstablishmentSearchRequest(
                    searchTerm,
                    scope,
                    scopeIdentifier,
                    page,
                    resultsPerPage
                ))
                select response;

            return await result.ToApiResultAsync(_options, cancellationToken);
        }
    }
}