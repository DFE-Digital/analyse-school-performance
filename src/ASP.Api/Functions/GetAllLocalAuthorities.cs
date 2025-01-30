using ASP.Domain.LocalAuthorities.UseCases.DTO;
using ASP.Domain.LocalAuthorities.UseCases.GetAllLocalAuthorities;
using ASP.Core.Pagination;
using ASP.Core.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System.Net;

namespace ASP.Api.Functions;

public class GetAllLocalAuthorities : ApiFunction
{
    private readonly ILogger<GetAllLocalAuthorities> _logger;
    private readonly IGetAllLocalAuthorities _useCase;
    private readonly ApiResultConverter _resultConverter;

    public GetAllLocalAuthorities(
        ILogger<GetAllLocalAuthorities> logger,
        IGetAllLocalAuthorities useCase,
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

    [Function("GetAllLocalAuthorities")]
    [OpenApiOperation(operationId: "GetAllLocalAuthorities", tags: ["Local Authorities"], Description = "Retrieves a paginated list of all local authorities.")]
    [OpenApiParameter(name: "page", In = ParameterLocation.Query, Required = false, Description = "The page number for pagination.")]
    [OpenApiParameter(name: "resultsPerPage", In = ParameterLocation.Query, Required = false, Description = "The number of results to return per page.")]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(ResultsPage<LocalAuthorityDTO>), Description = "A paginated list of local authorities.")]
    [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.NotFound, Description = "Not found: Could not find any local authorities.")]
    [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.MethodNotAllowed, Description = "Method not allowed: The HTTP method POST is not allowed.")]
    public override async Task<ActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Function, "get", "post")]
        HttpRequest request,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation(request.Method + " " + request.Path + request.QueryString);

        var result =
            from _ in request.ValidateHttpMethod([HttpMethods.Get])
            from page in request.ValidateParameter("page", p => p.IsOptional().IsNumeric())
            from resultsPerPage in request.ValidateParameter("resultsPerPage", p => p.IsOptional().IsNumeric())
            from response in _useCase.HandleRequest(new GetAllLocalAuthoritiesRequest(page, resultsPerPage))
            select response;

        return await _resultConverter.ConvertToApiResultAsync(result, cancellationToken);
    }
}