using ASP.Core;
using ASP.Core.Results;
using ASP.Infrastructure.DocumentDatabase;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Infrastructure;
using Xunit;

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


    [Given(@"no Establishments exist")]
    public void GivenNoEstablishmentsExists()
    {
    }

    [Given(@"Establishment ""([^""]+)"" exists:")]
    [Given(@"visible Establishment ""([^""]+)"" exists:")]
    public async Task GivenVisibleEstablishmentExistsMultiline(string id, string data)
    {
        await SetUpEstablishment(id, data, true)
            .OnError(e => Assert.Fail(e.ToString()));
    }

    [Given(@"non-visible Establishment ""([^""]+)"" exists:")]
    public async Task GivenEstablishmentExistsMultiline(string id, string data)
    {
        await SetUpEstablishment(id, data, false)
            .OnError(e => Assert.Fail(e.ToString()));
    }

    [Given(@"deleted Establishment ""([^""]+)"" exists:")]
    public async Task GivenDeletedEstablishmentExistsMultiline(string id, string data)
    {
        await SetUpEstablishment(id, data, false, true)
            .OnError(e => Assert.Fail(e.ToString()));
    }

    [Given(@"([0-9]+) Establishments exist with properties:")]
    public async Task GivenEstablishmentsExistWithProperties(int noOfEstablishments, Table properties)
    {
        for (var n = 1; n <= noOfEstablishments; n++)
        {
            Dictionary<string, object> data = new();

            if (properties.Rows.Any())
            {
                var row = properties.Rows.First();

                foreach (var (key, value) in row)
                {
                    var processedValue = StringReplacementPatterns.ReplaceNInPropertyValue(value, n);

                    // Check if the value is a JSON-like structure (object or array)
                    if (IsJsonStructure(processedValue))
                    {
                        // Parse the JSON-like string into a JToken (can be JObject or JArray)
                        data[key] = JToken.Parse(processedValue);
                    }
                    else
                    {
                        data[key] = processedValue;
                    }
                }
            }

            var id = (data.TryGetValue("urn", out var urnValue) ? urnValue : null)?.ToString() ?? n.ToString();

            await SetUpEstablishment(id, data).Switch(
                _ => { },
                e => Assert.Fail(e.ToString()));
        }
    }

    protected Task<Result<Done>> SetUpEstablishment(string id, string data, bool isVisible = true,
        bool isDeleted = false)
    {
        var dataDict = JsonConvert.DeserializeObject<Dictionary<string, object>>(data);

        return SetUpEstablishment(id, dataDict!, isVisible, isDeleted);
    }

    protected async Task<Result<Done>> SetUpEstablishment(string id, Dictionary<string, object> data,
        bool isVisible = true, bool isDeleted = false)
    {
        var document = await _database.GetAsync<Dictionary<string, object>>("establishments", id, id)
            .GetValueOrDefault(new Dictionary<string, object>());

        foreach (var d in data)
        {
            document[d.Key] = d.Value;
        }

        document["id"] = id;
        document["urn"] = id;
        document["isVisible"] = isVisible;
        document["isDeleted"] = isDeleted;

        return await _database.UpsertAsync("establishments", id, id, document);
    }
    
    private bool IsJsonStructure(string value)
    {
        value = value.Trim();
        return (value.StartsWith('{') && value.EndsWith('}')) || // JSON object
               (value.StartsWith('[') && value.EndsWith(']'));   // JSON array
    }
}