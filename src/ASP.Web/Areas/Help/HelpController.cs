using ASP.Application.UseCases.ContentPage.UpdateContentTemplate;
using ASP.Application.UseCases.ContentPage.ViewContentTemplate;
using ASP.Application.UseCases.ViewContentTemplate;
using ASP.Core.Results;
using Microsoft.AspNetCore.Mvc;
using ASP.Web.Core.Templating;

namespace ASP.Web.Areas.Help
{
    [Area("Help")]
    [Route("help")]
    public class HelpController : Controller
    {
        private readonly IViewContentTemplateUseCase _viewContentUseCase;
        private readonly IUpdateContentTemplateUseCase _updateContentUseCase;
        private readonly ITemplateComponentEditModelFactory _editModelFactory;

        public HelpController(IViewContentTemplateUseCase viewContentUseCase,
            IUpdateContentTemplateUseCase updateContentUseCase, ITemplateComponentEditModelFactory editModelFactory)
        {
            _viewContentUseCase = viewContentUseCase ??
                throw new ArgumentNullException(nameof(viewContentUseCase));
            _updateContentUseCase = updateContentUseCase ??
                throw new ArgumentNullException(nameof(updateContentUseCase));
            _editModelFactory = editModelFactory;
        }

        [HttpGet("{contentId}", Name = "app-route-help-view")]
        public async Task<IActionResult> ViewPage(string contentId)
        {
            string templateId = $"help-{contentId}".ToLower();
            ViewContentTemplateRequest request = new(templateId);

            return await _viewContentUseCase.HandleRequest(request)
                .Map(t => ContentTemplateViewModel.FromTemplate(contentId, t))
                .ToActionResult(View);
        }

        [HttpGet("{contentId}/edit", Name = "app-route-help-edit")]
        public async Task<IActionResult> EditPage(string contentId)
        {
            string templateId = $"help-{contentId}".ToLower();
            ViewContentTemplateRequest request = new(templateId);

            return await _viewContentUseCase.HandleRequest(request)
                .Map(t => ContentTemplateEditModel.FromTemplate(contentId, t, _editModelFactory))
                .ToActionResult(View);
        }

        [HttpPost("{contentId}/edit")]
        public async Task<IActionResult> EditPage(string contentId, ContentTemplateEditModel model)
        {
            string templateId = $"help-{contentId}".ToLower();

            return await model.ToTemplate()
                .ThenAsync(t => _updateContentUseCase.HandleRequest(new(templateId, t)))
                .ToActionResult(_ => RedirectToAction(nameof(ViewPage), new { contentId }));
        }
    }
}