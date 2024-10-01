using ASP.Application.UseCases.Establishments.GetAllEstablishments;
using ASP.Core.Results;
using ASP.Core.Scoping;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace ASP.Api.Functions;

public class GetAllEstablishments : ApiFunction
{
    private readonly ILogger<GetAllEstablishments> _logger;
    private readonly IGetAllEstablishments _useCase;
    private readonly ApiResultConverter _resultConverter;

    public GetAllEstablishments(
        ILogger<GetAllEstablishments> logger,
        IGetAllEstablishments useCase,
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

    [Function("GetAllEstablishments")]
    public override async Task<ActionResult> Run(
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

        return await _resultConverter.ConvertToApiResultAsync(result, cancellationToken);
    }
}