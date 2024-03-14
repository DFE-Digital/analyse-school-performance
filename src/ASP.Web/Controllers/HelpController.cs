using ASP.Application.UseCases.UpdateContentTemplate;
using ASP.Application.UseCases.ViewContentTemplate;
using ASP.Core.Helpers;
using ASP.Core.Templating;
using ASP.Web.Extensions;
using ASP.Web.Filters;
using ASP.Web.Models;
using ErrorOr;
using Microsoft.AspNetCore.Mvc;

namespace ASP.Web.Controllers
{
    [Route("help")]
    [ServiceFilter<CheckCookies>]
    public class HelpController : Controller
    {
        private readonly IViewContentTemplateUseCase _viewContentUseCase;
        private readonly IUpdateContentTemplateUseCase _updateContentUseCase;

        public HelpController(IViewContentTemplateUseCase viewContentUseCase,
            IUpdateContentTemplateUseCase updateContentUseCase)
        {
            _viewContentUseCase = viewContentUseCase ??
                throw new ArgumentNullException(nameof(viewContentUseCase));
            _updateContentUseCase = updateContentUseCase ??
                throw new ArgumentNullException(nameof(updateContentUseCase));
        }

        [HttpGet("{contentId}", Name = "app-route-help-view")]
        public async Task<IActionResult> ViewPage(string contentId)
        {
            string templateId = $"help-{contentId}".ToLower();
            ViewContentTemplateRequest request = new(templateId);

            return await _viewContentUseCase.HandleRequest(request)
                .Then(t => ContentTemplateViewModel.FromTemplate(contentId, t))
                .ToActionResult(View);
        }

        [HttpGet("{contentId}/edit", Name = "app-route-help-edit")]
        public async Task<IActionResult> EditPage(string contentId)
        {
            string templateId = $"help-{contentId}".ToLower();
            ViewContentTemplateRequest request = new(templateId);

            return await _viewContentUseCase.HandleRequest(request)
                .Then(t => ContentTemplateEditModel.FromTemplate(contentId, t))
                .ToActionResult(View);
        }

        [HttpPost("{contentId}/edit")]
        public async Task<IActionResult> EditContentPage(string contentId, ContentTemplateEditModel model)
        {
            string templateId = $"help-{contentId}".ToLower();

            return await model.ToTemplate()
                .ThenAsync(t => _updateContentUseCase.HandleRequest(new UpdateContentTemplateRequest(templateId, t)))
                .ToActionResult(_ => RedirectToAction(nameof(ViewPage), new { contentId }));
        }
    }
}