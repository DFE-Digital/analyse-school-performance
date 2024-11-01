using ASP.Core.Results;
using Microsoft.AspNetCore.Mvc;
using ASP.Web.Core.Templating;
using ASP.Application;
using ASP.Application.UseCases.ContentTemplates.ViewContentTemplate;
using Microsoft.AspNetCore.Authorization;
using ASP.Web.Features.Authorization;
using ASP.Core.Optionality;
using ASP.Web.Extensions;

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
        public Task<IActionResult> ViewPage(string contentId, string? revision)
        {
            string templateId = $"help-{contentId}".ToLower();
            ViewContentTemplateRequest request = new(templateId, Optional.FromNullable(revision));

            var result =
                from template in _api.ViewContentTemplate(request)
                select ContentTemplateViewModel.FromTemplate(contentId, revision, template);

            return result.ToActionResult(View, _hostEnvironment);
        }


        [Authorize(Policy = Policy.AccessToEditPages)]
        [HttpGet("{contentId}/edit", Name = "app-route-help-edit")]
        public Task<IActionResult> EditPage(string contentId, string? revision)
        {
            string templateId = $"help-{contentId}".ToLower();
            ViewContentTemplateRequest request = new(templateId, Optional.FromNullable(revision));

            var result =
                from template in _api.ViewContentTemplate(request)
                select ContentTemplateEditModel.FromTemplate(contentId, revision, template, _editModelFactory);
                
            return result.ToActionResult(View, _hostEnvironment);
        }

        [Authorize(Policy = Policy.AccessToEditPages)]
        [HttpPost("{contentId}/edit")]
        public Task<IActionResult> EditPage(string contentId, string revision, ContentTemplateEditModel model)
        {
            string templateId = $"help-{contentId}".ToLower();

            var result =
                from template in model.ToTemplate()
                from done in _api.UpdateContentTemplate(new(templateId, Optional.FromNullable(revision), template))
                select done;

            return result.ToActionResult(_ => RedirectToAction(nameof(ViewPage), new { contentId, revision }), _hostEnvironment);
        }
    }
}