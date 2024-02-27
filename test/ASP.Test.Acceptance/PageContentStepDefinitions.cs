using AngleSharp.Dom;
using ASP.Core.Helpers;
using ASP.Core.PageContent;
using ASP.Core.PageContent.Repository;
using ASP.Test.Core;
using ErrorOr;
using Newtonsoft.Json;
using System.Text.RegularExpressions;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Infrastructure;
using Xunit;

namespace ASP.Test.Acceptance.Core
{
    [Binding]
    public partial class PageContentStepDefinitions
    {
        private readonly IPageContentRepository _repository;
        private readonly ISpecFlowOutputHelper _outputHelper;

        public PageContentStepDefinitions(IPageContentRepository repository, ISpecFlowOutputHelper outputHelper)
        {
            _repository = repository;
            _outputHelper = outputHelper;
        }

        [BeforeScenario]
        public async Task ClearDownPageContent()
        {
            await _repository.DeleteAll();
        }

        [Given(@"no page content exists")]
        public void GivenNoPageContentExists()
        {
        }

        [Given(@"page content ""([^""]*)"" exists:")]
        public async Task GivenPageContentExists(string id, string data)
        {
            await SetUpPageContent(id, data).SwitchFirst(
                data =>
                {
                },
                e => AssertWithMessage.Failed(@$"Could not update page content with id ""{id}"": {e.Description}"));
        }

        [Then(@"page content ""([^""]*)"" should have property ""([^""]*)"" set to ""([^""]*)""")]
        public void ThenPageContentShouldHavePropertySetTo(string id, string propertyPath, string propertyValue)
        {
            GetPageContent(id).SwitchFirst(
                data =>
                {
                    var value = GetPropertyPathValue(propertyPath, data);

                    Assert.Equal(propertyValue, value);
                },
                e => AssertWithMessage.Failed(@$"Could not find page content with id ""{id}"": {e.Description}")
            );
        }

        [Then(@"page content ""([^""]*)"" should have property ""([^""]*)"" set to:")]
        public void ThenPageContentShouldHavePropertySetToMultiline(string id, string propertyPath, string propertyValue)
        {
            GetPageContent(id).SwitchFirst(
                data =>
                {
                    string? expectedSerializedPropertyValue = null;
                    JsonHelper.Deserialize<object>(propertyValue).SwitchFirst(
                        v => expectedSerializedPropertyValue = JsonHelper.Serialize(v),
                        e => AssertWithMessage.Failed(e.Description)
                    );

                    var value = GetPropertyPathValue(propertyPath, data);

                    var serializedPropertyValue = JsonHelper.Serialize(value);
                    Assert.Equal(expectedSerializedPropertyValue, serializedPropertyValue);
                },
                e => AssertWithMessage.Failed(@$"Could not find page content with id ""{id}"": {e.Description}")
            );
        }

        private async Task<ErrorOr<string>> GetPageContent(string id)
        {
            return await _repository.Get(id).Then(JsonConvert.SerializeObject);
        }

        private async Task<ErrorOr<Updated>> SetUpPageContent(string id, string data)
        {
            var document = await _repository.Get(id)
                .Match(v => JsonConvert.SerializeObject(v), _ => $$"""
                {
                    "id": "{{id}}",
                    "contentId": "{{id}}",
                    "published": true,
                    "PageTitle": null,
                    "PageContent": {},
                    "Views": []
                }
            """);

            var dataDict = JsonConvert.DeserializeObject<Dictionary<string, object>>(data);
            var docDict = JsonConvert.DeserializeObject<Dictionary<string, object>>(document);
            foreach (var d in dataDict)
            {
                docDict[d.Key] = d.Value;
            }

            var template = JsonConvert.DeserializeObject<PageContentTemplate>(JsonConvert.SerializeObject(docDict));

            return await _repository.Update(template);
        }

        private object GetPropertyPathValue(string propertyPath, string data)
        {
            var parts = propertyPath.Split('.');
            var propertyName = parts[0];
            var regex = new Regex(@"(.*)\[(\d+)\]");
            var match = regex.Match(propertyName);
            if (match.Success)
            {
                var arrayProperty = match.Groups[1].Value;
                var arrayIndex = int.Parse(match.Groups[2].Value);

                Dictionary<string, object>? dict = null;
                JsonHelper.Deserialize<Dictionary<string, object>>(data).SwitchFirst(
                    v => dict = v,
                    e => AssertWithMessage.Failed(e.Description)
                );
                AssertWithMessage.True(dict!.ContainsKey(arrayProperty), $@"Page content object does not contain property ""{arrayProperty}"":\n{data}");
                var arrayValue = dict![arrayProperty];
                object[]? array = null;
                JsonHelper.Deserialize<object[]>(JsonHelper.Serialize(arrayValue)).SwitchFirst(
                    v => array = v,
                    e => AssertWithMessage.Failed(e.Description)
                );
                if (array!.Length <= arrayIndex)
                {
                    AssertWithMessage.Failed($@"Array index {arrayIndex} does not exist on array ""{arrayProperty}"":\n{JsonHelper.Serialize(array!)}");
                }
                var value = array![arrayIndex];
                var restOfPath = string.Join(".", parts.Skip(1));
                if (restOfPath.Length > 0)
                {
                    return GetPropertyPathValue(restOfPath, JsonHelper.Serialize(value));
                }
                return value;
            }
            else
            {
                Dictionary<string, object>? dict = null;
                JsonHelper.Deserialize<Dictionary<string, object>>(data).SwitchFirst(
                    v => dict = v,
                    e => AssertWithMessage.Failed(e.Description)
                );
                AssertWithMessage.True(dict!.ContainsKey(propertyName), $@"Page content object does not contain property ""{propertyName}"":\n{data}");
                var value = dict![propertyName];
                var restOfPath = string.Join(".", parts.Skip(1));
                if (restOfPath.Length > 0)
                {
                    return GetPropertyPathValue(restOfPath, JsonHelper.Serialize(value));
                }
                return value;
            }
        }
    }
}
