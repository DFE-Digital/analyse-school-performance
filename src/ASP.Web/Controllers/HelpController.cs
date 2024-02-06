using ASP.Application.UseCases.UpdateContentPage;
using ASP.Application.UseCases.ViewContentPage;
using ASP.Core.Helpers;
using ASP.Web.Models;
using DfE.Data.DynamicPageTemplates.Web;
using DfE.Data.DynamicPageTemplates.Web.DynamicPages.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace ASP.Web.Controllers
{
    [Route("[controller]")]
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

        [HttpGet("{contentId}")]
        public async Task<IActionResult> ViewContentPage(string contentId)
        {
            string pageName = $"help-{contentId}".ToLower();
            ViewContentPageRequest request = new(pageName);
            ViewContentPageResponse response = await _viewContentUseCase.HandleRequest(request);

            return View(new ViewContentPageModel
            {
                ContentId = contentId,
                PageContentTemplate = response.PageContentTemplate
            });
        }

        [HttpGet("{contentId}/edit")]
        public async Task<IActionResult> EditContentPage(string contentId)
        {
            string pageName = $"help-{contentId}".ToLower();
            ViewContentPageRequest request = new(pageName);
            ViewContentPageResponse response = await _viewContentUseCase.HandleRequest(request);

            return View(new EditContentPageModel
            {
                Id = pageName,
                PageTitle = response.PageContentTemplate.PageTitle,
                Views = response.PageContentTemplate.Views.Select(v => new EditViewComponentModel
                {
                    ViewId = v.ViewId,
                    ViewContent = ((IDictionary<string, object>)v.ViewContent).ToDictionary(c => c.Key, c => c.Value?.ToString() ?? "")
                }).ToList()
            });
        }

        [HttpPost("{contentId}/edit")]
        public async Task<IActionResult> EditContentPage(string contentId, EditContentPageModel model)
        {
            UpdateContentPageRequest request = new()
            {
                PageContentId = model.Id,
                JsonValue = JsonHelper.SerializeIndented(model)
            };

            UpdateContentPageResponse test = await _updateContentUseCase.HandleRequest(request);

            return RedirectToAction(nameof(ViewContentPage), new { contentId });
        }
    }
}