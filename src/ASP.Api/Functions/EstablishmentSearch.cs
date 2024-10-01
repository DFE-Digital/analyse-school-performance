using ASP.Application.UseCases.Establishments.EstablishmentSearch;
using ASP.Core.Results;
using ASP.Core.Scoping;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace ASP.Api.Functions;

public class EstablishmentSearch : ApiFunction
{
    private readonly ILogger<EstablishmentSearch> _logger;
    private readonly IEstablishmentSearch _useCase;
    private readonly ApiResultConverter _resultConverter;

    public EstablishmentSearch(
        ILogger<EstablishmentSearch> logger,
        IEstablishmentSearch useCase,
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

    [Function("EstablishmentSearch")]
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

        return await _resultConverter.ConvertToApiResultAsync(result, cancellationToken);
    }
}