using ASP.Application.UseCases.ContentPage.UpdateContentTemplate;
using ASP.Application.UseCases.ContentPage.ViewContentTemplate;
using ASP.Core.Results;
using Microsoft.AspNetCore.Mvc;
using ASP.Web.Core.Templating;
using Microsoft.Azure.Cosmos.Linq;

namespace ASP.Web.Areas.Help
{
    [Area("Help")]
    [Route("help")]
    public class HelpController : Controller
    {
        private readonly IViewContentTemplateUseCase _viewContentUseCase;
        private readonly IUpdateContentTemplateUseCase _updateContentUseCase;
        private readonly ITemplateComponentEditModelFactory _editModelFactory;
        private readonly IHostEnvironment _hostEnvironment;

        public HelpController(IViewContentTemplateUseCase viewContentUseCase,
            IUpdateContentTemplateUseCase updateContentUseCase, ITemplateComponentEditModelFactory editModelFactory, 
            IHostEnvironment hostEnvironment)
        {
            _viewContentUseCase = viewContentUseCase ??
                throw new ArgumentNullException(nameof(viewContentUseCase));
            _updateContentUseCase = updateContentUseCase ??
                throw new ArgumentNullException(nameof(updateContentUseCase));
            _editModelFactory = editModelFactory;
            _hostEnvironment = hostEnvironment;
        }

        [HttpGet("{contentId}", Name = "app-route-help-view")]
        public async Task<IActionResult> ViewPage(string contentId, string? revision)
        {
            string templateId = $"help-{contentId}".ToLower();
            ViewContentTemplateRequest request = new(templateId, revision);

            return await _viewContentUseCase.HandleRequest(request)
                .Map(t => ContentTemplateViewModel.FromTemplate(contentId, revision, t))
                .ToActionResult(View, _hostEnvironment);
        }

        [HttpGet("{contentId}/edit", Name = "app-route-help-edit")]
        public async Task<IActionResult> EditPage(string contentId, string? revision)
        {
            string templateId = $"help-{contentId}".ToLower();
            ViewContentTemplateRequest request = new(templateId, revision);

            return await _viewContentUseCase.HandleRequest(request)
                .Map(t => ContentTemplateEditModel.FromTemplate(contentId, revision, t, _editModelFactory))
                .ToActionResult(View, _hostEnvironment);
        }

        [HttpPost("{contentId}/edit")]
        public async Task<IActionResult> EditPage(string contentId, string revision, ContentTemplateEditModel model)
        {
            string templateId = $"help-{contentId}".ToLower();

            return await model.ToTemplate()
                .Then(t => _updateContentUseCase.HandleRequest(new(templateId, revision, t)))
                .ToActionResult(_ => RedirectToAction(nameof(ViewPage), new { contentId, revision }), _hostEnvironment);
        }
    }
}