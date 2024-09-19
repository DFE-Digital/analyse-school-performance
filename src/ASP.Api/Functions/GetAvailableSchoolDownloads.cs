using ASP.Application.UseCases.Downloads.GetAvailableSchoolDownloads;
using ASP.Core.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace ASP.Api.Functions
{
    public class GetAvailableSchoolDownloads : ApiFunction
    {
        private readonly ILogger _logger;
        private readonly IGetAvailableSchoolDownloads _useCase;
        private readonly ErrorHandlingOptions _options;

        public GetAvailableSchoolDownloads(
            ILoggerFactory loggerFactory,
            IGetAvailableSchoolDownloads useCase,
            IOptions<ErrorHandlingOptions> options)
        {
            _logger = loggerFactory.CreateLogger<GetAvailableSchoolDownloads>();
            _useCase = useCase;
            _options = (options ?? throw new ArgumentNullException(nameof(options)))
                .Value;
        }

        [Function("GetAvailableSchoolDownloads")]
        public override async Task<ActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post")]
            HttpRequest request,
            CancellationToken cancellationToken)
        {
            _logger.LogInformation($"{request.Method} {request.Path + request.QueryString}");

            var result =
                from _ in request.ValidateHttpMethod([HttpMethods.Get])
                from urn in request.ValidateParameter("urn", p => p.IsRequired().IsDigits().HasLength(6))
                from response in _useCase.HandleRequest(new GetAvailableSchoolDownloadsRequest(urn))
                select response;

            return await result.ToApiResultAsync(_options, cancellationToken);
        }
    }
}
