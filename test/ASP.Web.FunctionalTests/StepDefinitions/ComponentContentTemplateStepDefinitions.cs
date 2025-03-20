using ASP.Infrastructure.DocumentDatabase;
using ASP.Test.Reqnroll;
using ASP.Test.Web.Areas.ComponentTest;

namespace ASP.Web.FunctionalTests.StepDefinitions
{
    [Binding]
    public partial class ComponentContentTemplateStepDefinitions : ContentTemplateStepDefinitions
    {
        public ComponentContentTemplateStepDefinitions(IDocumentDatabase database, IReqnrollOutputHelper outputHelper, ScenarioContext scenarioContext)
            : base(database, outputHelper, scenarioContext)
        {
        }

        [Given(@"a content template contains the component:")]
        public async Task AContentTemplateContainsTheComponentMultiline(string data)
        {
            await SetUpUnpublishedContentTemplate(ComponentTestController.TEST_COMPONENT_TEMPLATE_ID, ComponentTestController.TEST_COMPONENT_TEMPLATE_ID,
                $$"""
                {
                  "Views": [
                    {{data}}
                  ]
                }
                """);
        }

        [Then(@"the component template should have property ""([^""]+)"" equal to (.+)")]
        public async Task TheComponentTemplateShouldHavePropertyEqualTo(string propertyPath, string propertyValue)
        {
            await ThenContentTemplateWithIdAndContentIdShouldHavePropertyEqualTo(ComponentTestController.TEST_COMPONENT_TEMPLATE_ID, ComponentTestController.TEST_COMPONENT_TEMPLATE_ID, $"Views[0].ViewContent.{propertyPath}", propertyValue);
        }

        [Then(@"the component template should have property ""([^""]+)"" equal to:")]
        public async Task TheComponentTemplateShouldHavePropertyEqualToMultiline(string propertyPath, string propertyValue)
        {
            await ThenContentTemplateWithIdAndContentIdShouldHavePropertyEqualToMultiline(ComponentTestController.TEST_COMPONENT_TEMPLATE_ID, ComponentTestController.TEST_COMPONENT_TEMPLATE_ID, $"Views[0].ViewContent.{propertyPath}", propertyValue);
        }

        [Then(@"the component template should have property ""([^""]+)"" matching:")]
        public async Task TheComponentTemplateShouldHavePropertyMatchingMultiline(string propertyPath, string propertyValue)
        {
            await ThenContentTemplateWithIdAndContentIdShouldHavePropertyMatchingMultiline(ComponentTestController.TEST_COMPONENT_TEMPLATE_ID, ComponentTestController.TEST_COMPONENT_TEMPLATE_ID, $"Views[0].ViewContent.{propertyPath}", propertyValue);
        }

        [Then(@"the component template should match:")]
        public async Task TheComponentTemplateShouldMatchMultiline(string properties)
        {
            await ThenContentTemplateWithIdAndContentIdShouldHavePropertyMatchingMultiline(ComponentTestController.TEST_COMPONENT_TEMPLATE_ID, ComponentTestController.TEST_COMPONENT_TEMPLATE_ID, $"Views[0]", properties);
        }

        [Then(@"the component template should be equal to:")]
        public async Task TheComponentTemplateShouldBeEqualToMultiline(string properties)
        {
            await ThenContentTemplateWithIdAndContentIdShouldHavePropertyEqualToMultiline(ComponentTestController.TEST_COMPONENT_TEMPLATE_ID, ComponentTestController.TEST_COMPONENT_TEMPLATE_ID, $"Views[0]", properties);
        }
    }
}
