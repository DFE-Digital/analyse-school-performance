using AngleSharp.Dom;
using ASP.Core.Helpers;
using ASP.Core.Templating;
using ASP.Core.Templating.Repository;
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
        private readonly IContentTemplateRepository _repository;
        private readonly ISpecFlowOutputHelper _outputHelper;

        public PageContentStepDefinitions(IContentTemplateRepository repository, ISpecFlowOutputHelper outputHelper)
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

        [Given(@"page content ""([^""]+)"" exists:")]
        public async Task GivenPageContentExistsMultiline(string id, string data)
        {
            await SetUpPageContent(id, data).SwitchFirst(
                data =>
                {
                },
                e => AssertWithMessage.Fail(@$"Could not update page content with id ""{id}"": {e.Description}"));
        }

        [Then(@"page content ""([^""]+)"" property ""([^""]+)"" should be equal to (.+)")]
        public async Task ThenPageContentPropertyShouldBeEqualTo(string id, string propertyPath, string propertyValue)
        {
            await GetPageContent(id).SwitchFirst(
                data =>
                {
                    var value = GetPropertyPathValue(propertyPath, data);
                    Assert.Equal(propertyValue, value);
                },
                e => AssertWithMessage.Fail(@$"Could not find page content with id ""{id}"": {e.Description}")
            );
        }

        [Then(@"page content ""([^""]+)"" property ""([^""]+)"" should be equal to:")]
        public async Task ThenPageContentPropertyShouldBeEqualToMultiline(string id, string propertyPath, string propertyValue)
        {
            await GetPageContent(id).SwitchFirst(
                pageContent => JsonHelper.Deserialize<object>(propertyValue).SwitchFirst(
                    expected =>
                    {
                        var expectedSerializedPropertyValue = JsonHelper.Serialize(expected);
                        var actualSerializedPropertyValue = GetPropertyPathValue(propertyPath, pageContent);

                        Assert.Equal(expectedSerializedPropertyValue, actualSerializedPropertyValue);
                    },
                    e => AssertWithMessage.Fail(e.Description)
                ),
                e => AssertWithMessage.Fail(@$"Could not find page content with id ""{id}"": {e.Description}")
            );
        }

        [Then(@"page content ""([^""]+)"" property ""([^""]+)"" should match:")]
        public async Task ThenPageContentPropertyShouldMatchMultiline(string id, string propertyPath, string expected)
        {
            await GetPageContent(id).SwitchFirst(
                pageContent =>
                {
                    var actual = GetPropertyPathValue(propertyPath, pageContent);

                    MatchProperties(expected, actual);
                },
                e => AssertWithMessage.Fail(@$"Could not find page content with id ""{id}"": {e.Description}")
            );
        }

        [Then(@"page content ""([^""]+)"" should match:")]
        public async Task ThenPageContentShouldMatchMultiline(string id, string expected)
        {
            await GetPageContent(id).SwitchFirst(
                actual => MatchProperties(expected, actual),
                e => AssertWithMessage.Fail(@$"Could not find page content with id ""{id}"": {e.Description}")
            );
        }

        [Then(@"page content ""([^""]*)"" should be equal to:")]
        public async Task ThenPageContentShouldBeEqualToMultiline(string id, string properties)
        {
            await GetPageContent(id).SwitchFirst(
                actual => 
                {
                    var expected = JsonHelper.Serialize(JsonHelper.Deserialize<object>(properties));
                    Assert.Equal(expected, actual);
                },
                e => AssertWithMessage.Fail(@$"Could not find page content with id ""{id}"": {e.Description}")
            );
        }

        protected async Task<ErrorOr<string>> GetPageContent(string id)
        {
            return await _repository.Get(id).Then(JsonHelper.Serialize);
        }

        protected async Task<ErrorOr<Updated>> SetUpPageContent(string id, string data)
        {
            var document = await _repository.Get(id)
                .Match(v => JsonConvert.SerializeObject(v), _ => "{}");

            var dataDict = JsonConvert.DeserializeObject<Dictionary<string, object>>(data);
            var docDict = JsonConvert.DeserializeObject<Dictionary<string, object>>(document);

            foreach (var d in dataDict)
            {
                docDict[d.Key] = d.Value;
            }

            var template = JsonConvert.DeserializeObject<ContentTemplate>(JsonConvert.SerializeObject(docDict));

            return await _repository.Update(id, template);
        }

        protected string GetPropertyPathValue(string propertyPath, string data)
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
                    e => AssertWithMessage.Fail(e.Description)
                );
                AssertWithMessage.True(dict!.ContainsKey(arrayProperty), $@"Page content object does not contain property ""{arrayProperty}"":\n{data}");
                var arrayValue = dict![arrayProperty];
                object[]? array = null;
                JsonHelper.Deserialize<object[]>(JsonHelper.Serialize(arrayValue)).SwitchFirst(
                    v => array = v,
                    e => AssertWithMessage.Fail(e.Description)
                );
                if (array!.Length <= arrayIndex)
                {
                    AssertWithMessage.Fail($@"Array index {arrayIndex} does not exist on array ""{arrayProperty}"":\n{JsonHelper.Serialize(array!)}");
                }
                var value = JsonHelper.Serialize(array![arrayIndex]);
                var restOfPath = string.Join(".", parts.Skip(1));
                if (restOfPath.Length > 0)
                {
                    return GetPropertyPathValue(restOfPath, value);
                }
                return value;
            }
            else
            {
                Dictionary<string, object>? dict = null;
                JsonHelper.Deserialize<Dictionary<string, object>>(data).SwitchFirst(
                    v => dict = v,
                    e => AssertWithMessage.Fail(e.Description)
                );
                AssertWithMessage.True(dict!.ContainsKey(propertyName), $@"Page content object does not contain property ""{propertyName}"":\n{data}");
                var value = JsonHelper.Serialize(dict![propertyName]);
                var restOfPath = string.Join(".", parts.Skip(1));
                if (restOfPath.Length > 0)
                {
                    return GetPropertyPathValue(restOfPath, value);
                }
                return value;
            }
        }

        private void MatchProperties(string expected, string actual)
        {
            PruneTree(expected, actual, serializedActual =>
            {
                JsonHelper.Deserialize<object>(expected).SwitchFirst(
                    ex =>
                    {
                        var serializedExpected = JsonHelper.Serialize(ex);
                        Assert.Equal(serializedExpected, serializedActual);
                    },
                    e =>
                    {
                        AssertWithMessage.Fail(e.Description);
                    }
                );
            });
        }

        private void PruneTree(string expected, string actual, Action<string> doSomething)
        {
            JsonHelper.Deserialize<Dictionary<string, object>>(actual).SwitchFirst(
                actualDict =>
                {
                    JsonHelper.Deserialize<Dictionary<string, object>>(expected).SwitchFirst(
                        expectedDict =>
                        {
                            var resultDict = new Dictionary<string, object>();
                            foreach (var kvp in expectedDict)
                            {
                                PruneTree(JsonHelper.Serialize(kvp.Value), JsonHelper.Serialize(actualDict[kvp.Key]), r =>
                                {
                                    JsonHelper.Deserialize<object>(r).SwitchFirst(
                                        v =>
                                        {
                                            resultDict[kvp.Key] = v;
                                        },
                                        e =>
                                        {
                                            AssertWithMessage.Fail(e.Description);
                                        }
                                    );
                                });
                            }

                            var result = JsonHelper.Serialize(resultDict);

                            doSomething(result);
                        },
                        e =>
                        {
                            AssertWithMessage.Fail(e.Description);
                        }
                    );
                },
                e =>
                {
                    JsonHelper.Deserialize<List<object>>(actual).SwitchFirst(
                        actualList =>
                        {
                            JsonHelper.Deserialize<List<object>>(expected).SwitchFirst(
                                expectedList =>
                                {
                                    var resultList = new List<object>();
                                    for (var i = 0; i < Math.Min(expectedList.Count, actualList.Count); i++)
                                    {
                                        PruneTree(JsonHelper.Serialize(expectedList[i]), JsonHelper.Serialize(actualList[i]), r =>
                                        {
                                            JsonHelper.Deserialize<object>(r).SwitchFirst(
                                                v =>
                                                {
                                                    resultList.Add(v);
                                                },
                                                e =>
                                                {
                                                    AssertWithMessage.Fail(e.Description);
                                                }
                                            );
                                        });
                                    }

                                    var result = JsonHelper.Serialize(resultList);

                                    doSomething(result);
                                },
                                e =>
                                {
                                    AssertWithMessage.Fail(e.Description);
                                }
                            );
                        },
                        e =>
                        {
                            doSomething(actual);
                        }
                    );
                }
            );
        }
    }
}
