using ASP.Core.Results;
using ASP.Core.Text;
using ASP.Infrastructure.DocumentDatabase;
using Newtonsoft.Json;
using Reqnroll;
using Xunit;

namespace ASP.Test.Reqnroll
{
    [Binding]
    public partial class ContentTemplateStepDefinitions
    {
        private readonly ScenarioContext _scenarioContext;
        private readonly IDocumentDatabase _database;
        private readonly IReqnrollOutputHelper _outputHelper;

        public ContentTemplateStepDefinitions(IDocumentDatabase database, IReqnrollOutputHelper outputHelper, ScenarioContext scenarioContext)
        {
            _database = database;
            _outputHelper = outputHelper;
            _scenarioContext = scenarioContext;
        }

        [Given(@"no Content Templates exist")]
        public void GivenNoContentTemplateExists()
        {
        }

        [Given(@"(?:published )?Content Template ""([^""]+)"" exists:")]
        public async Task GivenContentTemplateExistsMultiline(string id, string data)
        {
            await SetUpPublishedContentTemplate(id, id, data)
                .OnError(e => Assert.Fail(e.ToString()));
        }

        [Given(@"(?:published )?Content Template with id ""([^""]+)"" and contentId ""([^""]+)"" exists:")]
        public async Task GivenContentTemplateWithIdAndContentIdExistsMultiline(string id, string contentId, string data)
        {
            await SetUpPublishedContentTemplate(id, contentId, data)
                .OnError(e => Assert.Fail(e.ToString()));

        }

        [Given(@"unpublished Content Template ""([^""]+)"" exists:")]
        public async Task GivenUnpublishedContentTemplateExistsMultiline(string id, string data)
        {
            await SetUpUnpublishedContentTemplate(id, id, data)
                .OnError(e => Assert.Fail(e.ToString()));
        }

        [Given(@"unpublished Content Template with id ""([^""]+)"" and contentId ""([^""]+)"" exists:")]
        public async Task GivenUnpublishedContentTemplateWithIdAndContentIdExistsMultiline(string id, string contentId, string data)
        {
            await SetUpUnpublishedContentTemplate(id, contentId, data)
                .OnError(e => Assert.Fail(e.ToString()));
        }

        [Then(@"Content Template <(.+)> should have property ""([^""]+)"" equal to (.+)")]
        public async Task ThenContentTemplateWithIdFromVariableShouldHavePropertyEqualTo(string idVariableName, string propertyPath, string propertyValue)
        {
            var id = ResolveVariable(idVariableName);
            await ThenContentTemplateShouldHavePropertyEqualTo(id, propertyPath, propertyValue);
        }

        [Then(@"Content Template ""([^""]+)"" should have property ""([^""]+)"" equal to (.+)")]
        public async Task ThenContentTemplateShouldHavePropertyEqualTo(string id, string propertyPath, string propertyValue)
        {
            await GetContentTemplate(id, id).Switch(
                contentTemplate => Assert.ObjectHasPropertyIdenticalTo(propertyPath, propertyValue, contentTemplate),
                e => Assert.Fail(e.ToString())
            );
        }

        [Then(@"Content Template with id <(.+)> and contentId ""([^""]+)"" should have property ""([^""]+)"" equal to (.+)")]
        public async Task ThenContentTemplateWithIdFromVariableAndContentIdShouldHavePropertyEqualTo(string idVariableName, string contentId, string propertyPath, string propertyValue)
        {
            var id = ResolveVariable(idVariableName);
            await ThenContentTemplateWithIdAndContentIdShouldHavePropertyEqualTo(id, contentId, propertyPath, propertyValue);
        }

        [Then(@"Content Template with id ""([^""]+)"" and contentId <(.+)> should have property ""([^""]+)"" equal to (.+)")]
        public async Task ThenContentTemplateWithIdAndContentIdFromVariableShouldHavePropertyEqualTo(string id, string contentIdVariableName, string propertyPath, string propertyValue)
        {
            var contentId = ResolveVariable(contentIdVariableName);
            await ThenContentTemplateWithIdAndContentIdShouldHavePropertyEqualTo(id, contentId, propertyPath, propertyValue);
        }

        [Then(@"Content Template with id <(.+)> and contentId <(.+)> should have property ""([^""]+)"" equal to (.+)")]
        public async Task ThenContentTemplateWithIdFromVariableAndContentIdFromVariableShouldHavePropertyEqualTo(string idVariableName, string contentIdVariableName, string propertyPath, string propertyValue)
        {
            var id = ResolveVariable(idVariableName);
            var contentId = ResolveVariable(contentIdVariableName);
            await ThenContentTemplateWithIdAndContentIdShouldHavePropertyEqualTo(id, contentId, propertyPath, propertyValue);
        }

        [Then(@"Content Template with id ""([^""]+)"" and contentId ""([^""]+)"" should have property ""([^""]+)"" equal to (.+)")]
        public async Task ThenContentTemplateWithIdAndContentIdShouldHavePropertyEqualTo(string id, string contentId, string propertyPath, string propertyValue)
        {
            await GetContentTemplate(id, contentId).Switch(
                contentTemplate => Assert.ObjectHasPropertyIdenticalTo(propertyPath, propertyValue, contentTemplate),
                e => Assert.Fail(e.ToString())
            );
        }

        [Then(@"Content Template <(.+)> should have property ""([^""]+)"" equal to:")]
        public async Task ThenContentTemplateWithIdFromVariableShouldHavePropertyEqualToMultiline(string idVariableName, string propertyPath, string propertyValue)
        {
            var id = ResolveVariable(idVariableName);
            await ThenContentTemplateShouldHavePropertyEqualToMultiline(id, propertyPath, propertyValue);
        }

        [Then(@"Content Template ""([^""]+)"" should have property ""([^""]+)"" equal to:")]
        public async Task ThenContentTemplateShouldHavePropertyEqualToMultiline(string id, string propertyPath, string propertyValue)
        {
            await GetContentTemplate(id, id).Switch(
                contentTemplate => Assert.ObjectHasPropertyIdenticalTo(propertyPath, propertyValue, contentTemplate),
                e => Assert.Fail(e.ToString())
            );
        }

        [Then(@"Content Template with id <(.+)> and contentId ""([^""]+)"" should have property ""([^""]+)"" equal to:")]
        public async Task ThenContentTemplateWithIdFromVariableAndContentIdShouldHavePropertyEqualToMultiline(string idVariableName, string contentId, string propertyPath, string propertyValue)
        {
            var id = ResolveVariable(idVariableName);
            await ThenContentTemplateWithIdAndContentIdShouldHavePropertyEqualToMultiline(id, contentId, propertyPath, propertyValue);
        }

        [Then(@"Content Template with id ""([^""]+)"" and contentId <(.+)> should have property ""([^""]+)"" equal to:")]
        public async Task ThenContentTemplateWithIdAndContentIdFromVariableShouldHavePropertyEqualToMultiline(string id, string contentIdVariableName, string propertyPath, string propertyValue)
        {
            var contentId = ResolveVariable(contentIdVariableName);
            await ThenContentTemplateWithIdAndContentIdShouldHavePropertyEqualToMultiline(id, contentId, propertyPath, propertyValue);
        }

        [Then(@"Content Template with id <(.+)> and contentId <(.+)> should have property ""([^""]+)"" equal to:")]
        public async Task ThenContentTemplateWithIdFromVariableAndContentIdFromVariableShouldHavePropertyEqualToMultiline(string idVariableName, string contentIdVariableName, string propertyPath, string propertyValue)
        {
            var id = ResolveVariable(idVariableName);
            var contentId = ResolveVariable(contentIdVariableName);
            await ThenContentTemplateWithIdAndContentIdShouldHavePropertyEqualToMultiline(id, contentId, propertyPath, propertyValue);
        }

        [Then(@"Content Template with id ""([^""]+)"" and contentId ""([^""]+)"" should have property ""([^""]+)"" equal to:")]
        public async Task ThenContentTemplateWithIdAndContentIdShouldHavePropertyEqualToMultiline(string id, string contentId, string propertyPath, string propertyValue)
        {
            await GetContentTemplate(id, contentId).Switch(
                contentTemplate => Assert.ObjectHasPropertyIdenticalTo(propertyPath, propertyValue, contentTemplate),
                e => Assert.Fail(e.ToString())
            );
        }

        [Then(@"Content Template <(.+)> should have property ""([^""]+)"" matching:")]
        public async Task ThenContentTemplateWithIdFromVariableShouldHavePropertyMatchingMultiline(string idVariableName, string propertyPath, string expected)
        {
            var id = ResolveVariable(idVariableName);
            await ThenContentTemplateShouldHavePropertyMatchingMultiline(id, propertyPath, expected);
        }

        [Then(@"Content Template ""([^""]+)"" should have property ""([^""]+)"" matching:")]
        public async Task ThenContentTemplateShouldHavePropertyMatchingMultiline(string id, string propertyPath, string expected)
        {
            await GetContentTemplate(id, id).Switch(
                contentTemplate => Assert.ObjectHasPropertyMatching(propertyPath, expected, contentTemplate),
                e => Assert.Fail(e.ToString())
            );
        }

        [Then(@"Content Template with id <(.+)> and contentId ""([^""]+)"" should have property ""([^""]+)"" matching:")]
        public async Task ThenContentTemplateWithIdFromVariableAndContentIdShouldHavePropertyMatchingMultiline(string idVariableName, string contentId, string propertyPath, string expected)
        {
            var id = ResolveVariable(idVariableName);
            await ThenContentTemplateWithIdAndContentIdShouldHavePropertyMatchingMultiline(id, contentId, propertyPath, expected);
        }

        [Then(@"Content Template with id ""([^""]+)"" and contentId <(.+)> should have property ""([^""]+)"" matching:")]
        public async Task ThenContentTemplateWithIdAndContentIdFromVariableShouldHavePropertyMatchingMultiline(string id, string contentIdVariableName, string propertyPath, string expected)
        {
            var contentId = ResolveVariable(contentIdVariableName);
            await ThenContentTemplateWithIdAndContentIdShouldHavePropertyMatchingMultiline(id, contentId, propertyPath, expected);
        }

        [Then(@"Content Template with id <(.+)> and contentId <(.+)> should have property ""([^""]+)"" matching:")]
        public async Task ThenContentTemplateWithIdFromVariableAndContentIdFromVariableShouldHavePropertyMatchingMultiline(string idVariableName, string contentIdVariableName, string propertyPath, string expected)
        {
            var id = ResolveVariable(idVariableName);
            var contentId = ResolveVariable(contentIdVariableName);
            await ThenContentTemplateWithIdAndContentIdShouldHavePropertyMatchingMultiline(id, contentId, propertyPath, expected);
        }

        [Then(@"Content Template with id ""([^""]+)"" and contentId ""([^""]+)"" should have property ""([^""]+)"" matching:")]
        public async Task ThenContentTemplateWithIdAndContentIdShouldHavePropertyMatchingMultiline(string id, string contentId, string propertyPath, string expected)
        {
            await GetContentTemplate(id, contentId).Switch(
                contentTemplate => Assert.ObjectHasPropertyMatching(propertyPath, expected, contentTemplate),
                e => Assert.Fail(e.ToString())
            );
        }

        [Then(@"Content Template <(.+)> should match:")]
        public async Task ThenContentTemplateWithIdFromVariableShouldMatchMultiline(string idVariableName, string expected)
        {
            var id = ResolveVariable(idVariableName);
            await ThenContentTemplateShouldMatchMultiline(id, expected);
        }
        
        [Then(@"Content Template ""([^""]+)"" should match:")]
        public async Task ThenContentTemplateShouldMatchMultiline(string id, string expected)
        {
            await GetContentTemplate(id, id).Switch(
                actual => Assert.ObjectMatchesProperties(expected, actual),
                e => Assert.Fail(e.ToString())
            );
        }

        [Then(@"Content Template with id <(.+)> and contentId ""([^""]+)"" should match:")]
        public async Task ThenContentTemplateWithIdFromVariableAndContentIdShouldMatchMultiline(string idVariableName, string contentId, string expected)
        {
            var id = ResolveVariable(idVariableName);
            await ThenContentTemplateWithIdAndContentIdShouldMatchMultiline(id, contentId, expected);
        }

        [Then(@"Content Template with id ""([^""]+)"" and contentId <(.+)> should match:")]
        public async Task ThenContentTemplateWithIdAndContentIdFromVariableShouldMatchMultiline(string id, string contentIdVariableName, string expected)
        {
            var contentId = ResolveVariable(contentIdVariableName);
            await ThenContentTemplateWithIdAndContentIdShouldMatchMultiline(id, contentId, expected);
        }

        [Then(@"Content Template with id <(.+)> and contentId <(.+)> should match:")]
        public async Task ThenContentTemplateWithIdFromVariableAndContentIdFromVariableShouldMatchMultiline(string idVariableName, string contentIdVariableName, string expected)
        {
            var id = ResolveVariable(idVariableName);
            var contentId = ResolveVariable(contentIdVariableName);
            await ThenContentTemplateWithIdAndContentIdShouldMatchMultiline(id, contentId, expected);
        }

        [Then(@"Content Template with id ""([^""]+)"" and contentId ""([^""]+)"" should match:")]
        public async Task ThenContentTemplateWithIdAndContentIdShouldMatchMultiline(string id, string contentId, string expected)
        {
            await GetContentTemplate(id, contentId).Switch(
                actual => Assert.ObjectMatchesProperties(expected, actual),
                e => Assert.Fail(e.ToString())
            );
        }

        [Then(@"Content Template <(.+)> should be equal to:")]
        public async Task ThenContentTemplateWithIdFromVariableShouldBeEqualToMultiline(string idVariableName, string properties)
        {
            var id = ResolveVariable(idVariableName);
            await ThenContentTemplateShouldBeEqualToMultiline(id, properties);
        }

        [Then(@"Content Template ""([^""]+)"" should be equal to:")]
        public async Task ThenContentTemplateShouldBeEqualToMultiline(string id, string properties)
        {
            await GetContentTemplate(id, id).Switch(
                actual => Assert.ObjectsAreIdentical(properties, actual),
                e => Assert.Fail(e.ToString())
            );
        }

        [Then(@"Content Template with id <(.+)> and contentId ""([^""]+)"" should be equal to:")]
        public async Task ThenContentTemplateWithIdFromVariableAndContentIdShouldBeEqualToMultiline(string idVariableName, string contentId, string properties)
        {
            var id = ResolveVariable(idVariableName);
            await ThenContentTemplateWithIdAndContentIdShouldBeEqualToMultiline(id, contentId, properties);
        }

        [Then(@"Content Template with id ""([^""]+)"" and contentId <(.+)> should be equal to:")]
        public async Task ThenContentTemplateWithIdAndContentIdFromVariableShouldBeEqualToMultiline(string id, string contentIdVariableName, string properties)
        {
            var contentId = ResolveVariable(contentIdVariableName);
            await ThenContentTemplateWithIdAndContentIdShouldBeEqualToMultiline(id, contentId, properties);
        }

        [Then(@"Content Template with id <(.+)> and contentId <(.+)> should be equal to:")]
        public async Task ThenContentTemplateWithIdFromVariableAndContentIdFromVariableShouldBeEqualToMultiline(string idVariableName, string contentIdVariableName, string properties)
        {
            var id = ResolveVariable(idVariableName);
            var contentId = ResolveVariable(contentIdVariableName);
            await ThenContentTemplateWithIdAndContentIdShouldBeEqualToMultiline(id, contentId, properties);
        }

        [Then(@"Content Template with id ""([^""]+)"" and contentId ""([^""]+)"" should be equal to:")]
        public async Task ThenContentTemplateWithIdAndContentIdShouldBeEqualToMultiline(string id, string contentId, string properties)
        {
            await GetContentTemplate(id, contentId).Switch(
                actual => Assert.ObjectsAreIdentical(properties, actual),
                e => Assert.Fail(e.ToString())
            );
        }

        protected async Task<Result<string>> GetContentTemplate(string id, string contentId)
        {
            return await _database.GetAsync<Dictionary<string, object>>("content", id, contentId)
                .Map(JsonHelper.Serialize);
        }

        protected async Task<Result<Done>> SetUpPublishedContentTemplate(string id, string contentId, string data)
        {
            var document = await _database.GetAsync<Dictionary<string, object>>("content", id, contentId)
            .GetValueOrDefault(new Dictionary<string, object>());

            var dataDict = JsonConvert.DeserializeObject<Dictionary<string, object>>(data);

            foreach (var d in dataDict)
            {
                document[d.Key] = d.Value;
            }
            document["id"] = id;
            document["contentId"] = contentId;
            document["isPublished"] = true;

            return await _database.UpsertAsync("content", id, contentId, document);
        }

        protected async Task<Result<Done>> SetUpUnpublishedContentTemplate(string id, string contentId, string data)
        {
            var document = await _database.GetAsync<Dictionary<string, object>>("content", id, contentId)
            .GetValueOrDefault(new Dictionary<string, object>());

            var dataDict = JsonConvert.DeserializeObject<Dictionary<string, object>>(data);

            foreach (var d in dataDict)
            {
                document[d.Key] = d.Value;
            }
            document["id"] = id;
            document["contentId"] = contentId;
            document["isPublished"] = false;

            return await _database.UpsertAsync("content", id, contentId, document);
        }

        private string ResolveVariable(string variableName)
        {
            if (!_scenarioContext.ContainsKey(variableName))
            {
                Assert.Fail($@"Variable ""{variableName}"" was not set. Did you forget a ""When I remember ... "" step? ;)");
            }

            return (string)_scenarioContext[variableName];
        }
    }
}
