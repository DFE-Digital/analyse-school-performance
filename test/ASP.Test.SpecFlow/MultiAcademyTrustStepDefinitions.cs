using ASP.Core;
using ASP.Core.Results;
using ASP.Infrastructure.DocumentDatabase;
using Newtonsoft.Json;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Infrastructure;
using Xunit;

namespace ASP.Test.SpecFlow;

[Binding]
public partial class MultiAcademyTrustStepDefinitions
{
    private readonly ScenarioContext _scenarioContext;
    private readonly IDocumentDatabase _database;
    private readonly ISpecFlowOutputHelper _outputHelper;

    public MultiAcademyTrustStepDefinitions(ScenarioContext scenarioContext, IDocumentDatabase database,
        ISpecFlowOutputHelper outputHelper)
    {
        _scenarioContext = scenarioContext;
        _database = database;
        _outputHelper = outputHelper;
    }
    
    [Given(@"no multi-academy trusts exist")]
    public void GivenNoMultiAcademyTrustExists()
    {
    }

    [Given(@"multi-academy trust ([^\(\)]+) \(([0-9]+)\) exists")]
    public async Task GivenEstablishmentExists(string name, string uid)
    {
        await SetUpMultiAcademyTrust(uid, @$"{{""name"":""{name}""}}").Switch(
            _ => { },
            e => Assert.Fail(e.ToString()));
    }

    protected Task<Result<Done>> SetUpMultiAcademyTrust(string id, string data)
    {
        var dataDict = JsonConvert.DeserializeObject<Dictionary<string, object>>(data);

        return SetUpMultiAcademyTrust(id, dataDict!);
    }
    
    protected async Task<Result<Done>> SetUpMultiAcademyTrust(string id, Dictionary<string, object> data)
    {
        var document = await _database.GetAsync<Dictionary<string, object>>("multi-academy-trusts", id, id)
            .GetValueOrDefault(new Dictionary<string, object>());

        foreach (var d in data)
        {
            document[d.Key] = d.Value;
        }
        document["id"] = id;

        return await _database.UpsertAsync("multi-academy-trusts", id, id, document);
    }
    
}