using ASP.Core;
using TechTalk.SpecFlow.Infrastructure;

namespace ASP.AcceptanceTests.StepDefinitions
{
    [Binding]
    public partial class ComponentTemplateStepDefinitions : Test.Acceptance.Core.PageContentStepDefinitions
    {
        public ComponentTemplateStepDefinitions(IDocumentDatabase database, ISpecFlowOutputHelper outputHelper)
            : base(database, outputHelper)
        {
        }

        [Given(@"a content template contains the component:")]
        public async Task AContentTemplateContainsTheComponentMultiline(string data)
        {
            await GivenPageContentExistsMultiline("test-component",
                $$"""
                {
                  "Views": [
                    {{data}}
                  ]
                }
                """);
        }

        [Then(@"the component template property ""([^""]+)"" should be equal to (.+)")]
        public async Task TheComponentTemplatePropertyShouldBeEqualTo(string propertyPath, string propertyValue)
        {
            await ThenPageContentPropertyShouldBeEqualTo("test-component", $"Views[0].ViewContent.{propertyPath}", propertyValue);
        }

        [Then(@"the component template property ""([^""]+)"" should be equal to:")]
        public async Task TheComponentTemplateShouldHavePropertyEqualToMultiline(string propertyPath, string propertyValue)
        {
            await ThenPageContentPropertyShouldBeEqualToMultiline("test-component", $"Views[0].ViewContent.{propertyPath}", propertyValue);
        }

        [Then(@"the component template property ""([^""]+)"" should match:")]
        public async Task TheComponentTemplatePropertyShouldMatchMultiline(string propertyPath, string propertyValue)
        {
            await ThenPageContentPropertyShouldMatchMultiline("test-component", $"Views[0].ViewContent.{propertyPath}", propertyValue);
        }

        [Then(@"the component template should match:")]
        public async Task TheComponentTemplateShouldMatchMultiline(string properties)
        {
            await ThenPageContentPropertyShouldMatchMultiline("test-component", $"Views[0]", properties);
        }

        [Then(@"the component template should be equal to:")]
        public async Task TheComponentTemplateShouldBeEqualToMultiline(string properties)
        {
            await ThenPageContentPropertyShouldBeEqualToMultiline("test-component", $"Views[0]", properties);
        }
    }
}
