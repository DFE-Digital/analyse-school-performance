using ASP.Application.UseCases.Downloads.GetAvailableLADownloads;
using ASP.Core.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace ASP.Api.Functions
{
    public class GetAvailableLADownloads : ApiFunction
    {
        private readonly ILogger _logger;
        private readonly IGetAvailableLADownloads _useCase;
        private readonly ErrorHandlingOptions _options;

        public GetAvailableLADownloads(ILoggerFactory loggerFactory, IGetAvailableLADownloads useCase, IOptions<ErrorHandlingOptions> options)
        {
            _logger = loggerFactory.CreateLogger<GetAvailableLADownloads>();
            _useCase = useCase;
            _options = (options ?? throw new ArgumentNullException(nameof(options)))
                .Value;
        }

        [Function("GetAvailableLADownloads")]
        public override async Task<ActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get")]
            HttpRequest request,
            CancellationToken cancellationToken
        )
        {
            _logger.LogInformation(request.Method + " " + request.Path + request.QueryString);

            var result =
                from _ in request.ValidateHttpMethod([HttpMethods.Get])
                from laCode in request.ValidateParameter("laCode", p => p.IsRequired().IsDigits().HasLength(3))
                from year in request.ValidateParameter("year", p => p.IsOptional().HasLength(4).IsNumeric())
                from response in _useCase.HandleRequest(new GetAvailableLADownloadsRequest(laCode, year))
                select response;

            return await result.ToApiResultAsync(_options, cancellationToken);
        }
    }
}