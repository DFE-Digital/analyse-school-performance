using ASP.Application.UseCases.UpdateContentPage;
using ASP.Core.Helpers;
using ASP.Web.Models;
using DfE.Data.ComponentLibrary.CleanArchitecture.CleanArchitecture.Application.UseCase;
using DfE.Data.DynamicPageTemplates.Core.Application.UseCases;
using DfE.Data.DynamicPageTemplates.Web;
using DfE.Data.DynamicPageTemplates.Web.DynamicPages.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace ASP.Web.Controllers
{
    [Route("[controller]")]
    public class HelpController : DynamicPageController
    {
        private readonly IUseCase<DynamicPageTemplateRequest, DynamicPageTemplateResponse> _getContentUseCase;
        private readonly IUpdateContentPageUseCase _updateContentUseCase;

        public HelpController(IUseCase<DynamicPageTemplateRequest, DynamicPageTemplateResponse> useCase, 
            IUpdateContentPageUseCase updateContentUseCase)
        {
            _updateContentUseCase = updateContentUseCase ??
                throw new ArgumentNullException(nameof(updateContentUseCase));
            _getContentUseCase = useCase ??
                throw new ArgumentNullException(nameof(useCase));
        }


        [HttpGet("{contentId}")]
        public async Task<IActionResult> ViewContentPage(string contentId)
        {
            string pageName = $"help-{contentId}".ToLower();
            DynamicPageTemplateRequest request = new(pageName);
            DynamicPageTemplateResponse response = await _getContentUseCase.HandleRequest(request);

            DynamicPageTemplateModel pageTemplate = JsonHelper
                .Deserialize<DynamicPageTemplateModel>(response.DynamicPageTemplate.ToString());

            return View(new ViewContentPageModel
            {
                ContentId = contentId,
                DynamicPageTemplate = pageTemplate
            });
        }



        [HttpGet("{contentId}/edit")]
        public async Task<IActionResult> EditContentPage(string contentId)
        {
            string pageName = $"help-{contentId}".ToLower();
            DynamicPageTemplateRequest request = new(pageName);
            DynamicPageTemplateResponse response = await _getContentUseCase.HandleRequest(request);

            DynamicPageTemplateModel pageTemplate = JsonHelper
                .Deserialize<DynamicPageTemplateModel>(response.DynamicPageTemplate.ToString());

            return View(new EditContentPageModel
            {
                Id = pageName,
                PageTitle = pageTemplate.PageTitle,
                Views = pageTemplate.Views.Select(v => new EditViewComponentModel
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
                JsonValue = JsonHelper.Serialize(model)
            };

            UpdateContentPageResponse test = await _updateContentUseCase.HandleRequest(request);

            return RedirectToAction(nameof(ViewContentPage), new { contentId });
        }
    }
}