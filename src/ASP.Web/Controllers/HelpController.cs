using ASP.Application.UseCases.UpdateContentPage;
using ASP.Application.UseCases.ViewContentPage;
using ASP.Core.Helpers;
using ASP.Core.PageContent;
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

        [HttpGet("{contentId}", Name = "app-content-view")]
        public async Task<IActionResult> ViewContentPage(string contentId)
        {
            string pageName = $"help-{contentId}".ToLower();
            ViewContentPageRequest request = new(pageName);

            return await _viewContentUseCase.HandleRequest(request)
                .ToActionResult(template => View(new ViewContentPageModel {
                    ContentId = contentId,
                    PageContentTemplate = template
                }));
        }

        [HttpGet("{contentId}/edit", Name = "app-content-edit")]
        public async Task<IActionResult> EditContentPage(string contentId)
        {
            string pageName = $"help-{contentId}".ToLower();
            ViewContentPageRequest request = new(pageName);

            return await _viewContentUseCase.HandleRequest(request)
                .ToActionResult(template => View(new EditContentPageModel {
                    ContentId = contentId,
                    Id = pageName,
                    PageTitle = template.PageTitle,
                    Views = template.Views.Select(v => new EditViewComponentModel {
                        ViewId = v.ViewId,
                        ViewContent = ((IDictionary<string, object>)v.ViewContent).ToDictionary(c => c.Key, c => c.Value?.ToString() ?? "")
                    }).ToList()
                }));
        }

        [HttpPost("{contentId}/edit")]
        public async Task<IActionResult> EditContentPage(string contentId, EditContentPageModel model)
        {
            var pageContent = JsonHelper.DeserializeIgnoringMissingMembers<PageContentTemplate>(JsonHelper.SerializeIndented(model));

            return await pageContent.Match(async v =>
            {
                UpdateContentPageRequest request = new(model.Id, v);

                return await _updateContentUseCase.HandleRequest(request)
                    .ToActionResult(response => RedirectToAction(nameof(ViewContentPage), new { contentId }));
            },
            e => {
                return Task.FromResult((IActionResult) new StatusCodeResult(500));
            });
        }
    }
}