using ASP.Application.UseCases.Establishments.GetEstablishmentDetails;
using ASP.Core.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace ASP.Api.Functions;

public class GetEstablishmentDetails : ApiFunction
{
    private readonly ILogger<GetEstablishmentDetails> _logger;
    private readonly IGetEstablishmentDetails _useCase;
    private readonly ApiResultConverter _resultConverter;

    public GetEstablishmentDetails(
        ILogger<GetEstablishmentDetails> logger,
        IGetEstablishmentDetails useCase,
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

    [Function("GetEstablishmentDetails")]
    public override async Task<ActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Function, "get", "post")] 
      	HttpRequest request,
        CancellationToken cancellationToken
	)
    {
        _logger.LogInformation(request.Method + " " + request.Path + request.QueryString);

        var result =
            from _ in request.ValidateHttpMethod([HttpMethods.Get])
            from urn in request.ValidateParameter("urn", p => p.IsRequired().IsDigits().HasLength(6))
            from response in _useCase.HandleRequest(new GetEstablishmentDetailsRequest(urn))
            select response;

        return await _resultConverter.ConvertToApiResultAsync(result, cancellationToken);
    }
}