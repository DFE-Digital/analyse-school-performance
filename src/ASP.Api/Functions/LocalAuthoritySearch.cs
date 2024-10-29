using ASP.Application.UseCases.LocalAuthorities.LocalAuthoritySearch;
using ASP.Core.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace ASP.Api.Functions;

public class LocalAuthoritySearch : ApiFunction
{
    private readonly ILogger<LocalAuthoritySearch> _logger;
    private readonly ILocalAuthoritySearch _useCase;
    private readonly ApiResultConverter _resultConverter;

    public LocalAuthoritySearch(
        ILogger<LocalAuthoritySearch> logger,
        ILocalAuthoritySearch useCase,
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

    [Function("LocalAuthoritySearch")]
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
            from page in request.ValidateParameter("page", p => p.IsOptional().IsNumeric())
            from resultsPerPage in request.ValidateParameter("resultsPerPage", p => p.IsOptional().IsNumeric())
            from response in _useCase.HandleRequest(new LocalAuthoritySearchRequest(
                searchTerm,
                page,
                resultsPerPage
            ))
            select response;

        return await _resultConverter.ConvertToApiResultAsync(result, cancellationToken);
    }
}