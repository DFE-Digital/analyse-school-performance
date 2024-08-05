using ASP.Core.Results;
using Microsoft.AspNetCore.Mvc;
using ASP.Web.Core.Templating;
using ASP.Application;
using ASP.Application.UseCases.ContentTemplates.ViewContentTemplate;
using Microsoft.AspNetCore.Authorization;
using ASP.Web.Features.Authorisation;

namespace ASP.Web.Areas.Help
{
    [Area("Help")]
    [Route("help")]
    public class HelpController : Controller
    {
        private readonly IAspApiClient _api;
        private readonly ITemplateComponentEditModelFactory _editModelFactory;
        private readonly IHostEnvironment _hostEnvironment;

        public HelpController(
            IAspApiClient api, 
            ITemplateComponentEditModelFactory editModelFactory, 
            IHostEnvironment hostEnvironment
        )
        {
            _api = api ?? throw new ArgumentNullException(nameof(api));
            _editModelFactory = editModelFactory ?? throw new ArgumentNullException(nameof(editModelFactory));
            _hostEnvironment = hostEnvironment ?? throw new ArgumentNullException(nameof(hostEnvironment));
        }

        [HttpGet("{contentId}", Name = "app-route-help-view")]
        public async Task<IActionResult> ViewPage(string contentId, string? revision)
        {
            string templateId = $"help-{contentId}".ToLower();
            ViewContentTemplateRequest request = new(templateId, revision);

            return await _api.ViewContentTemplate(request)
                .Map(t => ContentTemplateViewModel.FromTemplate(contentId, revision, t))
                .ToActionResult(View, _hostEnvironment);
        }


        [Authorize(Policy = Policy.AccessToEditPages)]
        [HttpGet("{contentId}/edit", Name = "app-route-help-edit")]
        public async Task<IActionResult> EditPage(string contentId, string? revision)
        {
            string templateId = $"help-{contentId}".ToLower();
            ViewContentTemplateRequest request = new(templateId, revision);

            return await _api.ViewContentTemplate(request)
                .Map(t => ContentTemplateEditModel.FromTemplate(contentId, revision, t, _editModelFactory))
                .ToActionResult(View, _hostEnvironment);
        }

        [Authorize(Policy = Policy.AccessToEditPages)]
        [HttpPost("{contentId}/edit")]
        public async Task<IActionResult> EditPage(string contentId, string revision, ContentTemplateEditModel model)
        {
            string templateId = $"help-{contentId}".ToLower();

            return await model.ToTemplate()
                .Then(t => _api.UpdateContentTemplate(new(templateId, revision, t)))
                .ToActionResult(_ => RedirectToAction(nameof(ViewPage), new { contentId, revision }), _hostEnvironment);
        }
    }
}