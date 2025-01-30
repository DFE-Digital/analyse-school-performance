using ASP.Domain.LocalAuthorities.UseCases.DTO;
using ASP.Domain.LocalAuthorities.UseCases.LocalAuthoritySearchSuggestions;
using ASP.Core.Results;
using ASP.Domain.LocalAuthorities.LocalAuthoritySearchSuggestions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System.Net;

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
    [OpenApiOperation(operationId: "LocalAuthoritySearchSuggestions", tags: ["Local Authorities"], Description = "Provides suggestions for local authorities based on a search term.")]
    [OpenApiParameter(name: "searchTerm", In = ParameterLocation.Query, Required = true, Type = typeof(string), Description = "The term to search for local authorities.")]
    [OpenApiParameter(name: "maxSuggestions", In = ParameterLocation.Query, Required = false, Type = typeof(int), Description = "The maximum number of suggestions to return.")]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(LocalAuthoritySearchSuggestionsResult<LocalAuthorityDTO>), Description = "A list of suggested local authorities.")]
    [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.BadRequest, Description = "Bad request: Missing or invalid parameters.")]
    [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.NotFound, Description = "Not found: No suggestions found for the given search term.")]
    [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.MethodNotAllowed, Description = "Method not allowed: The HTTP method POST is not allowed.")]
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