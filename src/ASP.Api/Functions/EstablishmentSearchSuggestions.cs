using ASP.Application.UseCases.Establishments.EstablishmentSearchSuggestions;
using ASP.Core.Results;
using ASP.Core.Scoping;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace ASP.Api.Functions
{
    public class EstablishmentSearchSuggestions : ApiFunction
    {
        private readonly ILogger _logger;
        private readonly IEstablishmentSearchSuggestions _useCase;
        private readonly ErrorHandlingOptions _options;

        public EstablishmentSearchSuggestions(
            ILoggerFactory loggerFactory,
            IEstablishmentSearchSuggestions useCase,
            IOptions<ErrorHandlingOptions> options
        )
        {
            _logger = loggerFactory.CreateLogger<EstablishmentSearchSuggestions>();
            _useCase = useCase;
            _options = (options ?? throw new ArgumentNullException(nameof(options)))
                .Value;
        }

        [Function("EstablishmentSearchSuggestions")]
        public override async Task<ActionResult> Run(
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
                from maxSuggestions in request.ValidateParameter("maxSuggestions", p => p.IsOptional().IsNumeric())
                from response in _useCase.HandleRequest(new EstablishmentSearchSuggestionsRequest(
                    searchTerm,
                    scope,
                    scopeIdentifier,
                    maxSuggestions
                ))
                select response;

            return await result.ToApiResultAsync(_options, cancellationToken);
        }
    }
}