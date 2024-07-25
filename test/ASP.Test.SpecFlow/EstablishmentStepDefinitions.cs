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
    private static readonly List<(Regex, string)> _stringRegexPattern = [
        
        (new Regex(@"^(?<Alpha>[\s\S]*)\((?<number>[0-9]+) \+ (?<replace>n)\)", RegexOptions.Compiled), "{0}"), // (1000 + n) or School (1000 + n)
        (new Regex(@"^(?<Alpha>[\s\S]*) (?<replace>n)$", RegexOptions.Compiled), " {0}"), // School n
        (new Regex(@"^(?<Alpha>[\s\S]+) (?<replace>n) (?<Beta>[\s\S]+)$", RegexOptions.Compiled), " {0} "), //School n Test
        (new Regex(@"^(?<Alpha>[\s\S]+) \((?<number>[0-9]+) \+ (?<replace>n)\) (?<beta>[\s\S]+)$", RegexOptions.Compiled), " {0} "), //School (1000 + n) Test
        (new Regex(@"^(?<replace>n) (?<Alpha>[\s\S]+)$", RegexOptions.Compiled), "{0} "), // n School
        (new Regex(@"^(?<replace>n)$", RegexOptions.Compiled), "{0}") // n
        
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
        await SetUpEstablishment(id, data, true)
            .OnError(e => AssertWithMessage.Fail(e.ToString()));
    }

    [Given(@"non visible establishment ""([^""]+)"" exists:")]
    public async Task GivenEstablishmentExistsMultiline(string id, string data)
    {
        await SetUpEstablishment(id, data, false)
            .OnError(e => AssertWithMessage.Fail(e.ToString()));
    }

    [Given(@"deleted establishment ""([^""]+)"" exists:")]
    public async Task GivenDeletedEstablishmentExistsMultiline(string id, string data)
    {
        await SetUpEstablishment(id, data, false, true)
            .OnError(e => AssertWithMessage.Fail(e.ToString()));
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
        foreach (var (regex, replacement) in _stringRegexPattern)
        {
            value = regex.Replace(value, match =>
            {
                var construct = "";
                for(var inc=1; inc <= match.Groups.Count-1; inc++)
                {
                    if (match.Groups[inc].Name == "number" && match.Groups[inc+1].Name == "replace")
                    {
                        if (int.TryParse(match.Groups[inc].Value, out int basenumber1))
                        {
                            construct = construct + (basenumber1+n);
                        }
                        inc++;
                    }
                    else if (match.Groups[inc].Name == "replace")
                    {
                       construct = construct + string.Format(replacement, n);    
                    }
                    else if(match.Groups[inc].Name == "Alpha" || match.Groups[inc].Name == "Beta")
                    {
                        construct = construct + match.Groups[inc].Value;
                    }
                    
                }
                return construct;
            });
        }
        return value;
    }

    protected Task<Result<Done>> SetUpEstablishment(string id, string data, bool isVisible = true, bool isDeleted = false)
    {
        var dataDict = JsonConvert.DeserializeObject<Dictionary<string, object>>(data);

        return SetUpEstablishment(id, dataDict!, isVisible, isDeleted);
    }

    protected async Task<Result<Done>> SetUpEstablishment(string id, Dictionary<string, object> data, bool isVisible = true, bool isDeleted = false)
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
}