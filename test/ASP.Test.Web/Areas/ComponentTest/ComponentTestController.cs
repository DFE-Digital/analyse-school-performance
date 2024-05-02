using ASP.Application.UseCases.ContentPage.UpdateContentTemplate;
using ASP.Application.UseCases.ContentPage.ViewContentTemplate;
using ASP.Core.Results;
using ASP.Web;
using ASP.Web.Core.Templating;
using Microsoft.AspNetCore.Mvc;

namespace ASP.Test.Web.Areas.ComponentTest
{
    // Test controller for components which is added as an Application Part to the test assembly:
    //   services.AddMvc()
    //     .AddApplicationPart(typeof(ComponentTestController).Assembly)
    //     .AddControllersAsServices();
    // 
    // This allows components to be tested independently of any controllers defined in the web application, so
    // keeping application and test code separate
    [Area("ComponentTest")]
    [Route("component-test")]
    public class ComponentTestController : Controller
    {
        private const string TEST_COMPONENT_TEMPLATE_ID = "test-component";

        private readonly IViewContentTemplateUseCase _viewContentUseCase;
        private readonly IUpdateContentTemplateUseCase _updateContentUseCase;
        private readonly ITemplateComponentEditModelFactory _editModelFactory;

        public ComponentTestController(IViewContentTemplateUseCase viewContentUseCase,
            IUpdateContentTemplateUseCase updateContentUseCase, ITemplateComponentEditModelFactory editModelFactory)
        {
            _viewContentUseCase = viewContentUseCase ??
                throw new ArgumentNullException(nameof(viewContentUseCase));
            _updateContentUseCase = updateContentUseCase ??
                throw new ArgumentNullException(nameof(updateContentUseCase));
            _editModelFactory = editModelFactory;
        }

        [HttpGet("view")]
        public new async Task<IActionResult> View()
        {
            return await _viewContentUseCase.HandleRequest(new ViewContentTemplateRequest(TEST_COMPONENT_TEMPLATE_ID))
                .Map(t => ContentTemplateViewModel.FromTemplate(TEST_COMPONENT_TEMPLATE_ID, t))
                .ToActionResult(View);
        }

        [HttpGet("edit")]
        public async Task<IActionResult> Edit()
        {
            return await _viewContentUseCase.HandleRequest(new ViewContentTemplateRequest(TEST_COMPONENT_TEMPLATE_ID))
                .Map(t => ContentTemplateEditModel.FromTemplate(TEST_COMPONENT_TEMPLATE_ID, t, _editModelFactory))
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