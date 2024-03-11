using ASP.Application.UseCases.UpdateContentPage;
using ASP.Application.UseCases.ViewContentPage;
using ASP.Web.Extensions;
using ASP.Web.Models;
using ErrorOr;
using Microsoft.AspNetCore.Mvc;

namespace ASP.Web.Controllers
{
    [Route("component-test")]
    public class ComponentTestController : Controller
    {
        private const string TEST_COMPONENT_TEMPLATE_ID = "test-component";

        private readonly IViewContentPageUseCase _viewContentUseCase;
        private readonly IUpdateContentPageUseCase _updateContentUseCase;

        public ComponentTestController(IViewContentPageUseCase viewContentUseCase,
            IUpdateContentPageUseCase updateContentUseCase)
        {
            _viewContentUseCase = viewContentUseCase ??
                throw new ArgumentNullException(nameof(viewContentUseCase));
            _updateContentUseCase = updateContentUseCase ??
                throw new ArgumentNullException(nameof(updateContentUseCase));
        }

        [HttpGet("view")]
        public new async Task<IActionResult> View()
        {
            return await _viewContentUseCase.HandleRequest(new ViewContentPageRequest(TEST_COMPONENT_TEMPLATE_ID))
                .Then(t => ContentTemplateViewModel.FromTemplate(TEST_COMPONENT_TEMPLATE_ID, t))
                .ToActionResult(View);
        }

        [HttpGet("edit")]
        public async Task<IActionResult> Edit()
        {
            return await _viewContentUseCase.HandleRequest(new ViewContentPageRequest(TEST_COMPONENT_TEMPLATE_ID))
                .Then(t => ContentTemplateEditModel.FromTemplate(TEST_COMPONENT_TEMPLATE_ID, t))
                .ToActionResult(View);
        }

        [HttpPost("edit")]
        public async Task<IActionResult> Edit(ContentTemplateEditModel model)
        {         
            return await model.ToTemplate()
                .ThenAsync(v => _updateContentUseCase.HandleRequest(new UpdateContentPageRequest(TEST_COMPONENT_TEMPLATE_ID, v)))
                .ToActionResult(_ => RedirectToAction(nameof(View)));
        }
    }
}