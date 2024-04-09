using ASP.Application.UseCases.UpdateContentTemplate;
using ASP.Application.UseCases.ViewContentTemplate;
using ASP.Web.Enums;
using ASP.Web.Constants;
using ASP.Web.Extensions;
using ASP.Web.Models;
using ASP.Core.Results;
using Microsoft.AspNetCore.Mvc;
using ASP.Web.Services;

namespace ASP.Web.Controllers
{
    [Route("help")]
    public class HelpController : Controller
    {
        private readonly IViewContentTemplateUseCase _viewContentUseCase;
        private readonly IUpdateContentTemplateUseCase _updateContentUseCase;
        private readonly ICookieProvider _cookieProvider;
        public HelpController(IViewContentTemplateUseCase viewContentUseCase,
            IUpdateContentTemplateUseCase updateContentUseCase, ICookieProvider cookieProvider)
        {
            _viewContentUseCase = viewContentUseCase ??
                throw new ArgumentNullException(nameof(viewContentUseCase));
            _updateContentUseCase = updateContentUseCase ??
                throw new ArgumentNullException(nameof(updateContentUseCase));
            _cookieProvider = cookieProvider ?? 
                throw new ArgumentNullException(nameof(cookieProvider));
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
                .Map(t => ContentTemplateEditModel.FromTemplate(contentId, t))
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

        [HttpPost("accept-terms-of-use")]
        public IActionResult AcceptTermsOfUse()
        {
            _cookieProvider.SetCookie(CookieKeys.AcceptedTermsOfUse, TermsOfUse.Accepted.ToString());

            // Read the "ref-url" query string parameter, this is set from the terms of use action filter
            string referer = HttpContext.Request.Query["ref-url"].ToString();

            return Redirect(string.IsNullOrEmpty(referer) ? "/" : referer);
        }
    }
}