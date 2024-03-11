using ASP.Application.UseCases.UpdateContentPage;
using ASP.Application.UseCases.ViewContentPage;
using ASP.Core.Helpers;
using ASP.Core.PageContent;
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
        private readonly IViewContentPageUseCase _viewContentUseCase;
        private readonly IUpdateContentPageUseCase _updateContentUseCase;

        public HelpController(IViewContentPageUseCase viewContentUseCase,
            IUpdateContentPageUseCase updateContentUseCase)
        {
            _viewContentUseCase = viewContentUseCase ??
                throw new ArgumentNullException(nameof(viewContentUseCase));
            _updateContentUseCase = updateContentUseCase ??
                throw new ArgumentNullException(nameof(updateContentUseCase));
        }

        [HttpGet("{contentId}", Name = "app-content-view")]
        public async Task<IActionResult> ViewContentPage(string contentId)
        {
            string templateId = $"help-{contentId}".ToLower();
            ViewContentPageRequest request = new(templateId);

            return await _viewContentUseCase.HandleRequest(request)
                .Then(t => ContentTemplateViewModel.FromTemplate(contentId, t))
                .ToActionResult(View);
        }

        [HttpGet("{contentId}/edit", Name = "app-content-edit")]
        public async Task<IActionResult> EditContentPage(string contentId)
        {
            string templateId = $"help-{contentId}".ToLower();
            ViewContentPageRequest request = new(templateId);

            return await _viewContentUseCase.HandleRequest(request)
                .Then(t => ContentTemplateEditModel.FromTemplate(contentId, t))
                .ToActionResult(View);
        }

        [HttpPost("{contentId}/edit")]
        public async Task<IActionResult> EditContentPage(string contentId, ContentTemplateEditModel model)
        {
            string templateId = $"help-{contentId}".ToLower();

            return await model.ToTemplate()
                .ThenAsync(t => _updateContentUseCase.HandleRequest(new UpdateContentPageRequest(templateId, t)))
                .ToActionResult(_ => RedirectToAction(nameof(ViewContentPage), new { contentId }));
        }
    }
}