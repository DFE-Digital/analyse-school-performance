using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using AngleSharp.Html.Parser;
using ASP.AcceptanceTests.Drivers;
using ASP.Core.Helpers;
using ASP.Test.Core;
using System.Net;
using TechTalk.SpecFlow.Infrastructure;
using Xunit.Sdk;

namespace ASP.AcceptanceTests.StepDefinitions
{
    [Binding]
    public partial class ComponentStepDefinitions
    {
        private readonly AspWebContext _web;
        private readonly ISpecFlowOutputHelper _outputHelper;

        public ComponentStepDefinitions(AspWebContext web, ISpecFlowOutputHelper outputHelper)
        {
            _web = web;
            _outputHelper = outputHelper;
        }

        [Given(@"I view the component on the page")]
        [When(@"I view the component on the page")]
        public async Task IViewTheComponentOnThePage()
        {
            await _web.GetAsync("/component-test/view");
        }

        [Given(@"I edit the component on the page")]
        [When(@"I edit the component on the page")]
        public async Task IEditTheComponentOnThePage()
        {
            await _web.GetAsync("/component-test/edit");
        }

        [When(@"I save the component")]
        public async Task WhenISaveTheComponent()
        {
            var element = _web.LastResponse.QuerySelector("#form");
            AssertWithMessage.NotNull(element, @$"Could not find an element with the selector ""#form"".");
            var form = Assert.IsAssignableFrom<IHtmlFormElement>(element);

            await _web.SubmitFormAsync(form);
        }

        [When(@"I update the component field labelled ""(.+)"" to have the value ""(.*)""")]
        public void WhenIUpdateTheComponentFieldLabelledToHaveTheValue(string labelText, string value)
        {
            var component = ComponentShouldExist();

            var label = component!.QuerySelectorAll(":scope label").FirstOrDefault(l => l.TextContent.Trim() == labelText.Trim());
            AssertWithMessage.NotNull(label, @$"Could not find a label with the text ""{labelText}"".");

            var field = component!.QuerySelector($":scope #{label?.Attributes["for"]?.Value}");
            AssertWithMessage.NotNull(field, @$"Could not find the associated input for the label ""{labelText}"" (""for"" attribute missing or incorrect).");

            var _ = field switch {
                IHtmlSelectElement select => select.Value = value,
                IHtmlInputElement input => input.Value = value,
                IHtmlTextAreaElement textarea => textarea.Value = value,
                _ => throw new XunitException($"Could not set the value of element of type {field!.GetType().Name}.")
            };
        }

        [When(@"I update the component field labelled ""(.+)"" to have the value:")]
        public void WhenIUpdateTheComponentFieldLabelledToHaveTheValueMultiline(string labelText, string value)
        {
            var component = ComponentShouldExist();

            var label = component!.QuerySelectorAll(":scope label").FirstOrDefault(l => l.TextContent.Trim() == labelText.Trim());
            AssertWithMessage.NotNull(label, @$"Could not find a label with the text ""{labelText}"".");

            var field = component!.QuerySelector($":scope #{label?.Attributes["for"]?.Value}");
            AssertWithMessage.NotNull(field, @$"Could not find the associated input for the label ""{labelText}"" (""for"" attribute missing or incorrect).");

            var _ = field switch {
                IHtmlSelectElement select => select.Value = value,
                IHtmlInputElement input => input.Value = value,
                IHtmlTextAreaElement textarea => textarea.Value = value,
                _ => throw new XunitException($"Could not set the value of element of type {field!.GetType().Name}.")
            };
        }

        [When(@"I update the component field labelled ""(.+)"" to be (checked|unchecked)")]
        public void WhenIUpdateTheComponentFieldLabelledToBe(string labelText, string state)
        {
            var component = ComponentShouldExist();

            var label = component!.QuerySelectorAll(":scope label").FirstOrDefault(l => l.TextContent.Trim() == labelText.Trim());
            AssertWithMessage.NotNull(label, @$"Could not find a label with the text ""{labelText}"".");

            var field = component!.QuerySelector($":scope #{label?.Attributes["for"]?.Value}");
            AssertWithMessage.NotNull(field, @$"Could not find the associated input for the label ""{labelText}"" (""for"" attribute missing or incorrect).");

            var _ = field switch {
                IHtmlInputElement input => input.IsChecked = state == "checked",
                _ => throw new XunitException($"Could not set the checked state of element of type {field!.GetType().Name}.")
            };
        }

        [Then(@"there should be no errors")]
        public void ThenThereShouldBeNoErrors()
        {
            if (_web.LastResponse.StatusCode != HttpStatusCode.OK)
            {
                _outputHelper.WriteLine(_web.LastResponse.Body.Html());
            }

            Assert.Equal(HttpStatusCode.OK, _web.LastResponse.StatusCode);
        }

        [Then(@"the component should exist")]
        public void ThenTheComponentShouldExist()
        {
            ComponentShouldExist();
        }

        [Then(@"the component should not exist")]
        public void ThenTheComponentShouldNotExist()
        {
            ComponentShouldNotExist();
        }

        [Then(@"the component should have the following markup:")]
        public void ThenTheComponentShouldHaveTheFollowingMarkup(string expectedMarkup)
        {
            var component = ComponentShouldExist();

            AssertHtml.Equivalent(expectedMarkup, component!, _outputHelper.WriteLine);
        }

        [Then(@"the component should have the text content ""(.*)""")]
        public void ThenTheComponentShouldHaveTheTextContent(string textContent)
        {
            var component = ComponentShouldExist();

            Assert.Equal(textContent.Trim(), component!.TextContent.Trim());
        }

        [Then(@"the component should have the inner HTML ""(.*)""")]
        public void ThenTheComponentShouldHaveTheInnerHtml(string expectedHtml)
        {
            var component = ComponentShouldExist();

            AssertHtml.Equivalent(expectedHtml, component!.InnerHtml, _outputHelper.WriteLine);
        }

        [Then(@"the component outer element should have the tag name ""([^""]*)""")]
        public void ThenTheComponentOuterElementShouldHaveTheTagName(string expectedTagName)
        {
            var component = ComponentShouldExist();

            Assert.Equal(expectedTagName, component!.TagName.ToLower());
        }

        [Then(@"the component outer element should have the class ""([^""]*)""")]
        public void ThenTheComponentOuterElementShouldHaveTheClass(string expectedClass)
        {
            var component = ComponentShouldExist();

            Assert.Equal(expectedClass, component!.ClassName);
        }

        [Then(@"the element ""(.+)"" within the component should exist")]
        public void ThenTheElementWithinTheComponentShouldExist(string selector)
        {
            ComponentElementShouldExist(selector);
        }

        [Then(@"the element ""(.+)"" within the component should not exist")]
        public void ThenTheElementWithinTheComponentShouldNotExist(string selector)
        {
            ComponentElementShouldNotExist(selector);
        }

        [Then(@"the element ""(.+)"" within the component should have the tag name ""([^""]+)""")]
        public void ThenTheElementWithinTheComponentShouldHaveTheTagName(string selector, string expectedTagName)
        {
            var element = ComponentElementShouldExist(selector);

            Assert.Equal(expectedTagName, element!.TagName.ToLower());
        }

        [Then(@"the element ""(.+)"" within the component should have the class ""(.*)""")]
        public void ThenTheElementWithinTheComponentShouldHaveTheClass(string selector, string expectedClass)
        {
            var element = ComponentElementShouldExist(selector);

            Assert.Equal(expectedClass, element!.ClassName);
        }

        [Then(@"the element ""(.+)"" within the component should have the text content ""(.*)""")]
        public void ThenTheElementWithinTheComponentShouldHaveTheTextContent(string selector, string textContent)
        {
            var element = ComponentElementShouldExist(selector);

            Assert.Equal(textContent.Trim(), element!.TextContent.Trim());
        }

        [Then(@"the component field labelled ""(.+)"" should have the value ""(.*)""")]
        public void ThenTheComponentFieldLabelledShouldHaveTheValue(string labelText, string expectedValue)
        {
            var component = ComponentShouldExist();

            var label = component!.QuerySelectorAll(":scope label").FirstOrDefault(l => l.TextContent.Trim() == labelText.Trim());
            AssertWithMessage.NotNull(label, @$"Could not find a label with the text ""{labelText}"".");

            var field = component!.QuerySelector($":scope #{label?.Attributes["for"]?.Value}");
            AssertWithMessage.NotNull(field, @$"Could not find the associated input for the label ""{labelText}"" (""for"" attribute missing or incorrect).");

            var value = field! switch {
                IHtmlSelectElement select => select.Value,
                IHtmlInputElement input => input.Value,
                IHtmlTextAreaElement textArea => textArea.Value,
                _ => throw new XunitException($"Could not find the value of element of type {field!.GetType().Name}.")
            };

            Assert.Equal(expectedValue, value);
        }

        [Then(@"the component field labelled ""(.+)"" should be (checked|unchecked)")]
        public void ThenTheComponentFieldLabelledShouldBeUnchecked(string labelText, string state)
        {
            var component = ComponentShouldExist();

            var label = component!.QuerySelectorAll(":scope label").FirstOrDefault(l => l.TextContent.Trim() == labelText.Trim());
            AssertWithMessage.NotNull(label, @$"Could not find a label with the text ""{labelText}"".");

            var field = component!.QuerySelector($":scope #{label?.Attributes["for"]?.Value}");
            AssertWithMessage.NotNull(field, @$"Could not find the associated input for the label ""{labelText}"" (""for"" attribute missing or incorrect).");

            var isChecked = field! switch {
                IHtmlInputElement input => input.IsChecked,
                _ => throw new XunitException($"Could not find the checked state of element of type {field!.GetType().Name}.")
            };

            Assert.Equal(state == "checked", isChecked);
        }

        [Then(@"the component field labelled ""(.+)"" should match the JSON string ""(.*)""")]
        public void ThenTheComponentFieldLabelledShouldMatchTheJSONString(string labelText, string expectedValue)
        {
            var component = ComponentShouldExist();

            var label = component!.QuerySelectorAll(":scope label").FirstOrDefault(l => l.TextContent.Trim() == labelText.Trim());
            AssertWithMessage.NotNull(label, @$"Could not find a label with the text ""{labelText}"".");

            var field = component!.QuerySelector($":scope #{label?.Attributes["for"]?.Value}");
            AssertWithMessage.NotNull(field, @$"Could not find the associated input for the label ""{labelText}"" (""for"" attribute missing or incorrect).");

            var actual = field! switch {
                IHtmlSelectElement select => select.Value,
                IHtmlInputElement input => input.Value,
                IHtmlTextAreaElement textArea => textArea.Value,
                _ => throw new XunitException($"Could not find the value of element of type {field!.GetType().Name}.")
            };

            var normalizedExpected = JsonHelper.Serialize(JsonHelper.Deserialize<object>(expectedValue));
            var normalizedActual = JsonHelper.Serialize(JsonHelper.Deserialize<object>(actual!));
            Assert.Equal(normalizedExpected, normalizedActual);
        }

        [Then(@"the input element of the component field labelled ""(.+)"" should match the selector ""(.+)""")]
        public void ThenTheInputElementOfTheComponentFieldLabelledShouldMatchTheSelector(string labelText, string selector)
        {
            var component = ComponentShouldExist();

            var label = component!.QuerySelectorAll(":scope label").FirstOrDefault(l => l.TextContent.Trim() == labelText.Trim());
            AssertWithMessage.NotNull(label, @$"Could not find a label with the text ""{labelText}"".");

            var element = component!.QuerySelector($":scope #{label?.Attributes["for"]?.Value}");
            AssertWithMessage.NotNull(element, @$"Could not find the associated input element for the label ""{labelText}"" (""for"" attribute missing or incorrect).");

            AssertWithMessage.True(element!.Matches(selector), @$"The associated input element for the label ""{labelText}"" did not match ""{selector}"".");
        }

        private IElement ComponentShouldExist()
        {
            ThenThereShouldBeNoErrors();
            var testWrapper = _web.LastResponse.QuerySelector("#test");
            AssertWithMessage.NotNull(testWrapper, @$"Could not find an element with the selector ""#test"".");

            var component = testWrapper!.QuerySelector(":scope > *");
            AssertWithMessage.NotNull(component, @$"Could not find the component's outer element on the page.");

            return component!;
        }

        private void ComponentShouldNotExist()
        {
            ThenThereShouldBeNoErrors();
            var testWrapper = _web.LastResponse.QuerySelector("#test");
            AssertWithMessage.NotNull(testWrapper, @$"Could not find an element with the selector ""#test"".");

            var component = testWrapper!.QuerySelector(":scope > *");
            AssertWithMessage.Null(component, @$"Found the component's outer element on the page.");
        }

        private IElement ComponentElementShouldExist(string selector)
        {
            var component = ComponentShouldExist();

            var element = component!.QuerySelector($":scope {selector}");
            AssertWithMessage.NotNull(element, @$"Could not find an element within the component with the selector ""{selector}"".");

            return element!;
        }

        private void ComponentElementShouldNotExist(string selector)
        {
            var component = ComponentShouldExist();

            var element = component!.QuerySelector($":scope {selector}");
            AssertWithMessage.Null(element, @$"Found an element within the component with the selector ""{selector}"".");
        }
    }
}
