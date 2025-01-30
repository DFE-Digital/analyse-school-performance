using ASP.Application;
using ASP.Core.Optionality;
using ASP.Core.Results;
using ASP.Domain.Templating.UseCases.UpdateContentTemplate;
using ASP.Domain.Templating.UseCases.ViewContentTemplate;
using ASP.Web.Core.Templating;
using ASP.Web.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;

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
    [AllowAnonymous]
    public class ComponentTestController : Controller
    {
        public const string TEST_COMPONENT_TEMPLATE_ID = "test-component";

        private readonly IAspApiClient _api;
        private readonly ITemplateComponentEditModelFactory _editModelFactory;
        private readonly IHostEnvironment _hostEnvironment;

        public ComponentTestController(
            IAspApiClient api,
            ITemplateComponentEditModelFactory editModelFactory, 
            IHostEnvironment hostEnvironment
        )
        {
            _api = api ?? throw new ArgumentNullException(nameof(api));
            _editModelFactory = editModelFactory ?? throw new ArgumentNullException(nameof(editModelFactory));
            _hostEnvironment = hostEnvironment ?? throw new ArgumentNullException(nameof(hostEnvironment));
        }

        [HttpGet("view")]
        public new Task<IActionResult> View()
        {
            var model =
                from template in _api.ViewContentTemplate(new ViewContentTemplateRequest(TEST_COMPONENT_TEMPLATE_ID, Optional.FromNullable(TEST_COMPONENT_TEMPLATE_ID)))
                select ContentTemplateViewModel.FromTemplate(TEST_COMPONENT_TEMPLATE_ID, TEST_COMPONENT_TEMPLATE_ID, template);

            return model
                .ToActionResult(View, _hostEnvironment);
        }

        [HttpGet("edit")]
        public Task<IActionResult> Edit()
        {
            var model =
                from template in _api.ViewContentTemplate(new ViewContentTemplateRequest(TEST_COMPONENT_TEMPLATE_ID, Optional.FromNullable(TEST_COMPONENT_TEMPLATE_ID)))
                select ContentTemplateEditModel.FromTemplate(TEST_COMPONENT_TEMPLATE_ID, TEST_COMPONENT_TEMPLATE_ID, template, _editModelFactory);

            return model
                .ToActionResult(View, _hostEnvironment);
        }

        [HttpPost("edit")]
        public Task<IActionResult> Edit(ContentTemplateEditModel model)
        {
            var result =
                from template in model.ToTemplate()
                from done in _api.UpdateContentTemplate(new UpdateContentTemplateRequest(TEST_COMPONENT_TEMPLATE_ID, Optional.FromNullable(TEST_COMPONENT_TEMPLATE_ID), template))
                select done;

            return result
                .ToActionResult(_ => RedirectToAction(nameof(View)), _hostEnvironment);
        }
    }
}