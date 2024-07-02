using ASP.Core;
using ASP.Core.Results;
using ASP.Test.Core;
using Newtonsoft.Json;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Infrastructure;

namespace ASP.Test.SpecFlow;

[Binding]
public partial class EstablishmentStepDefinitions
{
    private readonly ScenarioContext _scenarioContext;
    private readonly IDocumentDatabase _database;
    private readonly ISpecFlowOutputHelper _outputHelper;

    public EstablishmentStepDefinitions(ScenarioContext scenarioContext, IDocumentDatabase database,
        ISpecFlowOutputHelper outputHelper)
    {
        _scenarioContext = scenarioContext;
        _database = database;
        _outputHelper = outputHelper;
    }


    [Given(@"no establishments exist")]
    public void GivenNoEstablishmentsExists()
    {
    }

    [Given(@"establishment ""([^""]+)"" exists:")]
    [Given(@"visible establishment ""([^""]+)"" exists:")]
    public async Task GivenVisibleEstablishmentExistsMultiline(string id, string data)
    {
        await SetUpVisibleEstablishment(id, data).Switch(
            _ => { },
            e => AssertWithMessage.Fail(e.ToString()));
    }

    [Given(@"non visible establishment ""([^""]+)"" exists:")]
    public async Task GivenEstablishmentExistsMultiline(string id, string data)
    {
        await SetUpNonVisibleEstablishment(id, data).Switch(
            _ => { },
            e => AssertWithMessage.Fail(e.ToString()));
    }

    protected async Task<Result<Done>> SetUpVisibleEstablishment(string id, string data)
    {
        var document = await _database.GetAsync<Dictionary<string, object>>("establishments", id, id)
            .GetValueOrDefault(new Dictionary<string, object>());

        var dataDict = JsonConvert.DeserializeObject<Dictionary<string, object>>(data);

        foreach (var d in dataDict)
        {
            document[d.Key] = d.Value;
        }

        document["isVisible"] = true;

        return await _database.UpsertAsync("establishments", id, id, document);
    }

    protected async Task<Result<Done>> SetUpNonVisibleEstablishment(string id, string data)
    {
        var document = await _database.GetAsync<Dictionary<string, object>>("establishments", id, id)
            .GetValueOrDefault(new Dictionary<string, object>());

        var dataDict = JsonConvert.DeserializeObject<Dictionary<string, object>>(data);

        foreach (var d in dataDict)
        {
            document[d.Key] = d.Value;
        }

        document["isVisible"] = false;

        return await _database.UpsertAsync("establishments", id, id, document);
    }
}