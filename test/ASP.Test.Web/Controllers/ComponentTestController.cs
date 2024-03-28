using ASP.Application.UseCases.UpdateContentTemplate;
using ASP.Application.UseCases.ViewContentTemplate;
using ASP.Web.Extensions;
using ASP.Web.Models;
using ErrorOr;
using Microsoft.AspNetCore.Mvc;

namespace ASP.Web.Controllers
{
    // Test controller for components which is added as an Application Part to the test assembly:
    //   services.AddMvc()
    //     .AddApplicationPart(typeof(ComponentTestController).Assembly)
    //     .AddControllersAsServices();
    // 
    // This allows components to be tested independently of any controllers defined in the web application, so
    // keeping application and test code separate
    [Route("component-test")]
    public class ComponentTestController : Controller
    {
        private const string TEST_COMPONENT_TEMPLATE_ID = "test-component";

        private readonly IViewContentTemplateUseCase _viewContentUseCase;
        private readonly IUpdateContentTemplateUseCase _updateContentUseCase;

        public ComponentTestController(IViewContentTemplateUseCase viewContentUseCase,
            IUpdateContentTemplateUseCase updateContentUseCase)
        {
            _viewContentUseCase = viewContentUseCase ??
                throw new ArgumentNullException(nameof(viewContentUseCase));
            _updateContentUseCase = updateContentUseCase ??
                throw new ArgumentNullException(nameof(updateContentUseCase));
        }

        [HttpGet("view")]
        public new async Task<IActionResult> View()
        {
            return await _viewContentUseCase.HandleRequest(new ViewContentTemplateRequest(TEST_COMPONENT_TEMPLATE_ID))
                .Then(t => ContentTemplateViewModel.FromTemplate(TEST_COMPONENT_TEMPLATE_ID, t))
                .ToActionResult(View);
        }

        [HttpGet("edit")]
        public async Task<IActionResult> Edit()
        {
            return await _viewContentUseCase.HandleRequest(new ViewContentTemplateRequest(TEST_COMPONENT_TEMPLATE_ID))
                .Then(t => ContentTemplateEditModel.FromTemplate(TEST_COMPONENT_TEMPLATE_ID, t))
                .ToActionResult(View);
        }

        [HttpPost("edit")]
        public async Task<IActionResult> Edit(ContentTemplateEditModel model)
        {         
            return await model.ToTemplate()
                .ThenAsync(v => _updateContentUseCase.HandleRequest(new UpdateContentTemplateRequest(TEST_COMPONENT_TEMPLATE_ID, v)))
                .ToActionResult(_ => RedirectToAction(nameof(View)));
        }
    }
}