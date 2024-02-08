using ASP.Application.UseCases.UpdateContentPage;
using ASP.Application.UseCases.ViewContentPage;
using ASP.Core.Helpers;
using ASP.Web.Extensions;
using ASP.Web.Models;
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

        [HttpGet("{contentId}", Name = "asp-content-view")]
        public async Task<IActionResult> ViewContentPage(string contentId)
        {
            string pageName = $"help-{contentId}".ToLower();
            ViewContentPageRequest request = new(pageName);

            return await _viewContentUseCase.HandleRequest(request)
                .ToActionResult(response => View(new ViewContentPageModel {
                    ContentId = contentId,
                    PageContentTemplate = response.PageContentTemplate
                }));
        }

        [HttpGet("{contentId}/edit", Name = "asp-content-edit")]
        public async Task<IActionResult> EditContentPage(string contentId)
        {
            string pageName = $"help-{contentId}".ToLower();
            ViewContentPageRequest request = new(pageName);

            return await _viewContentUseCase.HandleRequest(request)
                .ToActionResult(response => View(new EditContentPageModel {
                    ContentId = contentId,
                    Id = pageName,
                    PageTitle = response.PageContentTemplate.PageTitle,
                    Views = response.PageContentTemplate.Views.Select(v => new EditViewComponentModel {
                        ViewId = v.ViewId,
                        ViewContent = ((IDictionary<string, object>)v.ViewContent).ToDictionary(c => c.Key, c => c.Value?.ToString() ?? "")
                    }).ToList()
                }));
        }

        [HttpPost("{contentId}/edit")]
        public async Task<IActionResult> EditContentPage(string contentId, EditContentPageModel model)
        {
            UpdateContentPageRequest request = new() {
                PageContentId = model.Id,
                JsonValue = JsonHelper.SerializeIndented(model)
            };

            return await _updateContentUseCase.HandleRequest(request)
                .ToActionResult(response => RedirectToAction(nameof(ViewContentPage), new { contentId }));
        }
    }
}