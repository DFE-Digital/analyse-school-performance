using ASP.Application.UseCases.LocalAuthorities.LocalAuthoritySearchSuggestions;
using ASP.Core.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace ASP.Api.Functions;

public class LocalAuthoritySearchSuggestions : ApiFunction
{
    private readonly ILogger<LocalAuthoritySearchSuggestions> _logger;
    private readonly ILocalAuthoritySearchSuggestions _useCase;
    private readonly ApiResultConverter _resultConverter;

    public LocalAuthoritySearchSuggestions(
        ILogger<LocalAuthoritySearchSuggestions> logger,
        ILocalAuthoritySearchSuggestions useCase,
        ApiResultConverter resultConverter)
    {
        _logger = logger
                  ?? throw new ArgumentNullException(nameof(logger));

        _useCase = useCase
                   ?? throw new ArgumentNullException(nameof(useCase));

        _resultConverter = resultConverter
                           ?? throw new ArgumentNullException(nameof(resultConverter));
    }

    [Function("LocalAuthoritySearchSuggestions")]
    public override async Task<ActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Function, "get", "post")] HttpRequest request,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation(request.Method + " " + request.Path + request.QueryString);

        var result =
            from _ in request.ValidateHttpMethod([HttpMethods.Get])
            from searchTerm in request.ValidateParameter("searchTerm", p => p.IsRequired())
            from maxSuggestions in request.ValidateParameter("maxSuggestions", p => p.IsOptional().IsNumeric())
            from response in _useCase.HandleRequest(new LocalAuthoritySearchSuggestionsRequest(
                searchTerm,
                maxSuggestions
            ))
            select response;

        return await _resultConverter.ConvertToApiResultAsync(result, cancellationToken);
    }
}