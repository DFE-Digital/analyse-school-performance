using ASP.Core;
using ASP.Core.Results;
using ASP.Test.Core;
using Newtonsoft.Json;
using System.Text.RegularExpressions;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Infrastructure;

namespace ASP.Test.SpecFlow;

[Binding]
public partial class EstablishmentStepDefinitions
{
    private static readonly Regex _sumContainingN = new Regex(@"^\(([0-9]+) \+ n\)$", RegexOptions.Compiled);
    private static readonly List<(Regex, string)> _stringContainingN = [
        (new Regex(@"^(n) ", RegexOptions.Compiled), "{0} "),
        (new Regex(@" (n) ", RegexOptions.Compiled), " {0} "),
        (new Regex(@" (n)$", RegexOptions.Compiled), " {0}")
    ];

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
        await SetUpEstablishment(id, data, true).Switch(
            _ => { },
            e => AssertWithMessage.Fail(e.ToString()));
    }

    [Given(@"non visible establishment ""([^""]+)"" exists:")]
    public async Task GivenEstablishmentExistsMultiline(string id, string data)
    {
        await SetUpEstablishment(id, data, false).Switch(
            _ => { },
            e => AssertWithMessage.Fail(e.ToString()));
    }

    [Given(@"([0-9]+) establishments exist with properties:")]
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
                    data[key] = ReplaceNInPropertyValue(value, n);
                }
            }

            var id = (data.TryGetValue("urn", out var urnValue) ? urnValue : null)?.ToString() ?? n.ToString();

            await SetUpEstablishment(id, data).Switch(
                _ =>
                {
                },
                e => AssertWithMessage.Fail(e.ToString()));
        }
    }

    private object ReplaceNInPropertyValue(string value, int n)
    {
        var numericMatch = _sumContainingN.Match(value);
        if (numericMatch.Success && int.TryParse(numericMatch.Groups[1].Value, out int baseNumber))
        {
            return (baseNumber + n).ToString();
        }

        foreach (var (regex, replacement) in _stringContainingN)
        {
            value = regex.Replace(value, string.Format(replacement, n));
        }

        return value;
    }

    protected Task<Result<Done>> SetUpEstablishment(string id, string data, bool isVisible = true)
    {
        var dataDict = JsonConvert.DeserializeObject<Dictionary<string, object>>(data);

        return SetUpEstablishment(id, dataDict!, isVisible);
    }

    protected async Task<Result<Done>> SetUpEstablishment(string id, Dictionary<string, object> data, bool isVisible = true)
    {
        var document = await _database.GetAsync<Dictionary<string, object>>("establishments", id, id)
            .GetValueOrDefault(new Dictionary<string, object>());

        foreach (var d in data)
        {
            document[d.Key] = d.Value;
        }
        document["id"] = id;
        document["isVisible"] = isVisible;

        return await _database.UpsertAsync("establishments", id, id, document);
    }
}