using ASP.Core;
using ASP.Core.Results;
using ASP.Test.Core;
using Newtonsoft.Json;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Infrastructure;

namespace ASP.Test.SpecFlow;

[Binding]
public partial class LocalAuthorityStepDefinitions
{
    private readonly ScenarioContext _scenarioContext;
    private readonly IDocumentDatabase _database;
    private readonly ISpecFlowOutputHelper _outputHelper;

    public LocalAuthorityStepDefinitions(ScenarioContext scenarioContext, IDocumentDatabase database,
        ISpecFlowOutputHelper outputHelper)
    {
        _scenarioContext = scenarioContext;
        _database = database;
        _outputHelper = outputHelper;
    }
    
    [Given(@"no Local Authorities exist")]
    public void GivenNoLocalAuthorityExists()
    {
    }
    
    [Given(@"Local Authority ""([^""]+)"" exists:")]
    public async Task GivenLocalAuthorityExistsMultiline(string id, string data)
    {
        await SetUpLocalAuthority(id, data).Switch(
            _ => { },
            e => AssertWithMessage.Fail(e.ToString()));
    }
    
    [Given(@"([0-9]+) Local Authorities exist with properties:")]
    public async Task GivenLocalAuthorityExistsWithProperties(int noOfLocalAuthorities, Table properties)
    {
        for (var n = 1; n <= noOfLocalAuthorities; n++)
        {
            Dictionary<string, object> data = new();

            if (properties.Rows.Any())
            {
                var row = properties.Rows.First();

                foreach (var (key, value) in row)
                {
                    data[key] = StringReplacementPatterns.ReplaceNInPropertyValue(value, n);
                }
            }

            var id = (data.TryGetValue("code", out var codeValue) ? codeValue : null)?.ToString() ?? n.ToString();

            await SetUpLocalAuthority(id, data).Switch(
                _ => { },
                e => AssertWithMessage.Fail(e.ToString()));
        }
    }
    protected Task<Result<Done>> SetUpLocalAuthority(string id, string data)
    {
        var dataDict = JsonConvert.DeserializeObject<Dictionary<string, object>>(data);

        return SetUpLocalAuthority(id, dataDict!);
    }
    
    protected async Task<Result<Done>> SetUpLocalAuthority(string id, Dictionary<string, object> data)
    {
        var document = await _database.GetAsync<Dictionary<string, object>>("local-authorities", id, id)
            .GetValueOrDefault(new Dictionary<string, object>());

        foreach (var d in data)
        {
            document[d.Key] = d.Value;
        }
        document["id"] = id;
        document["code"] = id;

        return await _database.UpsertAsync("local-authorities", id, id, document);
    }
    
}