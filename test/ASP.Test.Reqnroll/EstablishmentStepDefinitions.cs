using ASP.Core.Results;
using ASP.Infrastructure.DocumentDatabase;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Reqnroll;
using Xunit;

namespace ASP.Test.Reqnroll;

[Binding]
public partial class EstablishmentStepDefinitions
{
    private readonly ScenarioContext _scenarioContext;
    private readonly IDocumentDatabase _database;
    private readonly IReqnrollOutputHelper _outputHelper;

    public EstablishmentStepDefinitions(ScenarioContext scenarioContext, IDocumentDatabase database,
        IReqnrollOutputHelper outputHelper)
    {
        _scenarioContext = scenarioContext;
        _database = database;
        _outputHelper = outputHelper;
    }


    [Given(@"no establishments exist")]
    public void GivenNoEstablishmentsExists()
    {
    }

    [Given(@"establishment ([^\(\)]+) \(([0-9]+)\) exists")]
    [Given(@"visible establishment ([^\(\)]+) \(([0-9]+)\) exists")]
    public async Task GivenEstablishmentExists(string name, string urn)
    {
        await SetUpEstablishment(urn, @$"{{""name"":""{name}""}}").Switch(
            _ => { },
            e => Assert.Fail(e.ToString()));
    }

    [Given(@"non-visible establishment ([^\(\)]+) \(([0-9]+)\) exists")]
    public async Task GivenNonVisibleEstablishmentExists(string name, string urn)
    {
        await SetUpEstablishment(urn, @$"{{""name"":""{name}""}}", false).Switch(
            _ => { },
            e => Assert.Fail(e.ToString()));
    }

    [Given(@"deleted establishment ([^\(\)]+) \(([0-9]+)\) exists")]
    public async Task GivenDeletedEstablishmentExists(string name, string urn)
    {
        await SetUpEstablishment(urn, @$"{{""name"":""{name}""}}", true, true).Switch(
            _ => { },
            e => Assert.Fail(e.ToString()));
    }

    [Given(@"establishment ([^\(\)]+) \(([0-9]+)\) exists with LAESTAB code (.+)")]
    [Given(@"visible establishment ([^\(\)]+) \(([0-9]+)\) exists with LAESTAB code (.+)")]
    public async Task GivenEstablishmentExistsWithLaestabCode(string name, string urn, string laestabCode)
    {
        await SetUpEstablishment(urn, @$"{{""name"":""{name}"", ""laestab"": ""{laestabCode}""}}").Switch(
            _ => { },
            e => Assert.Fail(e.ToString()));
    }

    [Given(@"establishment ([^\(\)]+) \(([0-9]+)\) exists in local authority ([0-9]+)")]
    [Given(@"visible establishment ([^\(\)]+) \(([0-9]+)\) exists in local authority ([0-9]+)")]
    public async Task GivenEstablishmentExistsInLocalAuthority(string name, string urn, string laCode)
    {
        await SetUpEstablishment(urn, @$"{{""name"":""{name}"", ""localAuthority"":{{""code"":""{laCode}""}}}}").Switch(
            _ => { },
            e => Assert.Fail(e.ToString()));
    }

    [Given(@"establishment ([^\(\)]+) \(([0-9]+)\) exists in local authority ([^\(\)]+) \(([0-9]+)\)")]
    [Given(@"visible establishment ([^\(\)]+) \(([0-9]+)\) exists in local authority ([^\(\)]+) \(([0-9]+)\)")]
    public async Task GivenEstablishmentExistsInLocalAuthorityNameAndCode(string name, string urn, string laName, string laCode)
    {
        await SetUpEstablishment(urn, @$"{{""name"":""{name}"", ""localAuthority"":{{""code"":""{laCode}"", ""name"":""{laName}""}}}}").Switch(
            _ => { },
            e => Assert.Fail(e.ToString()));
    }

    [Given(@"establishment ([^\(\)]+) \(([0-9]+)\) exists in multi-academy trust ([0-9]+)")]
    [Given(@"visible establishment ([^\(\)]+) \(([0-9]+)\) exists in multi-academy trust ([0-9]+)")]
    public async Task GivenEstablishmentExistsInMultiAcademyTrust(string name, string urn, string matUid)
    {
        await SetUpEstablishment(urn, @$"{{""name"":""{name}"", ""multiAcademyTrust"":{{""uid"":""{matUid}""}}}}").Switch(
            _ => { },
            e => Assert.Fail(e.ToString()));
    }

    [Given(@"establishment ([^\(\)]+) \(([0-9]+)\) exists in diocese ([^\:\(\)]+)")]
    [Given(@"visible establishment ([^\(\)]+) \(([0-9]+)\) exists in diocese ([^\:\(\)]+)")]
    public async Task GivenEstablishmentExistsInDiocese(string name, string urn, string dioceseName)
    {
        await SetUpEstablishment(urn, @$"{{""name"":""{name}"", ""diocese"":{{""name"":""{dioceseName}""}}}}").Switch(
            _ => { },
            e => Assert.Fail(e.ToString()));
    }

    [Given(@"non-visible establishment ([^\(\)]+) \(([0-9]+)\) exists in local authority ([0-9]+)")]
    public async Task GivenNonVisibleEstablishmentExistsInLocalAuthority(string name, string urn, string laCode)
    {
        await SetUpEstablishment(urn, @$"{{""name"":""{name}"", ""localAuthority"":{{""code"":""{laCode}""}}}}", false).Switch(
            _ => { },
            e => Assert.Fail(e.ToString()));
    }

    [Given(@"non-visible establishment ([^\(\)]+) \(([0-9]+)\) exists in multi-academy trust ([0-9]+)")]
    public async Task GivenNonVisibleEstablishmentExistsInMultiAcademyTrust(string name, string urn, string matUid)
    {
        await SetUpEstablishment(urn, @$"{{""name"":""{name}"", ""multiAcademyTrust"":{{""uid"":""{matUid}""}}}}", false).Switch(
            _ => { },
            e => Assert.Fail(e.ToString()));
    }

    [Given(@"non-visible establishment ([^\(\)]+) \(([0-9]+)\) exists in diocese ([^\:\(\)]+)")]
    public async Task GivenNonVisibleEstablishmentExistsInDiocese(string name, string urn, string dioceseName)
    {
        await SetUpEstablishment(urn, @$"{{""name"":""{name}"", ""diocese"":{{""name"":""{dioceseName}""}}}}", false).Switch(
            _ => { },
            e => Assert.Fail(e.ToString()));
    }

    [Given(@"deleted establishment ([^\(\)]+) \(([0-9]+)\) exists in local authority ([0-9]+)")]
    public async Task GivenDeletedEstablishmentExistsInLocalAuthority(string name, string urn, string laCode)
    {
        await SetUpEstablishment(urn, @$"{{""name"":""{name}"", ""localAuthority"":{{""code"":""{laCode}""}}}}", true, true).Switch(
            _ => { },
            e => Assert.Fail(e.ToString()));
    }

    [Given(@"deleted establishment ([^\(\)]+) \(([0-9]+)\) exists in multi-academy trust ([0-9]+)")]
    public async Task GivenDeletedEstablishmentExistsInMultiAcademyTrust(string name, string urn, string matUid)
    {
        await SetUpEstablishment(urn, @$"{{""name"":""{name}"", ""multiAcademyTrust"":{{""uid"":""{matUid}""}}}}", true, true).Switch(
            _ => { },
            e => Assert.Fail(e.ToString()));
    }

    [Given(@"deleted establishment ([^\(\)]+) \(([0-9]+)\) exists in diocese ([^\:\(\)]+)")]
    public async Task GivenDeletedEstablishmentExistsInDiocese(string name, string urn, string dioceseName)
    {
        await SetUpEstablishment(urn, @$"{{""name"":""{name}"", ""diocese"":{{""name"":""{dioceseName}""}}}}", true, true).Switch(
            _ => { },
            e => Assert.Fail(e.ToString()));
    }

    [Given(@"establishment ([^\(\)]+) \(([0-9]+)\) exists with properties:")]
    [Given(@"visible establishment ([^\(\)]+) \(([0-9]+)\) exists with properties:")]
    public async Task GivenVisibleEstablishmentExistsMultiline(string name, string urn, string properties)
    {
        var firstBrace = properties.IndexOf('{');
        var lastBrace = properties.LastIndexOf('}');
        var data = properties.Substring(0, firstBrace) + properties.Substring(firstBrace + 1, lastBrace - firstBrace - 1) + properties.Substring(lastBrace + 1);
        await SetUpEstablishment(urn, @$"{{""name"":""{name}"", {data}}}", true)
            .OnError(e => Assert.Fail(e.ToString()));
    }

    [Given(@"non-visible establishment ([^\(\)]+) \(([0-9]+)\) exists with properties:")]
    public async Task GivenNonVisibleEstablishmentExistsMultiline(string name, string urn, string properties)
    {
        var firstBrace = properties.IndexOf('{');
        var lastBrace = properties.LastIndexOf('}');
        var data = properties.Substring(0, firstBrace) + properties.Substring(firstBrace + 1, lastBrace - firstBrace - 1) + properties.Substring(lastBrace + 1);
        await SetUpEstablishment(urn, @$"{{""name"":""{name}"", {data}}}", false)
            .OnError(e => Assert.Fail(e.ToString()));
    }

    [Given(@"deleted establishment ([^\(\)]+) \(([0-9]+)\) exists with properties:")]
    public async Task GivenDeletedEstablishmentExistsMultiline(string name, string urn, string properties)
    {
        var firstBrace = properties.IndexOf('{');
        var lastBrace = properties.LastIndexOf('}');
        var data = properties.Substring(0, firstBrace) + properties.Substring(firstBrace + 1, lastBrace - firstBrace - 1) + properties.Substring(lastBrace + 1);
        await SetUpEstablishment(urn, @$"{{""name"":""{name}"", {data}}}", false, true)
            .OnError(e => Assert.Fail(e.ToString()));
    }

    [Given(@"establishment ([^\(\)]+) \(([0-9]+)\) exists in local authority ([0-9]+) with properties:")]
    [Given(@"visible establishment ([^\(\)]+) \(([0-9]+)\) exists in local authority ([0-9]+) with properties:")]
    public async Task GivenEstablishmentExistsInLocalAuthorityMultiline(string name, string urn, string laCode, string properties)
    {
        var firstBrace = properties.IndexOf('{');
        var lastBrace = properties.LastIndexOf('}');
        var data = properties.Substring(0, firstBrace) + properties.Substring(firstBrace + 1, lastBrace - firstBrace - 1) + properties.Substring(lastBrace + 1);
        await SetUpEstablishment(urn, @$"{{""name"":""{name}"", ""localAuthority"":{{""code"":""{laCode}""}}, {data}}}").Switch(
            _ => { },
            e => Assert.Fail(e.ToString()));
    }

    [Given(@"establishment ([^\(\)]+) \(([0-9]+)\) exists in local authority ([^\(\)]+) \(([0-9]+)\) with properties:")]
    [Given(@"visible establishment ([^\(\)]+) \(([0-9]+)\) exists in local authority ([^\(\)]+) \(([0-9]+)\) with properties:")]
    public async Task GivenEstablishmentExistsInLocalAuthorityNameAndCodeMultiline(string name, string urn, string laName, string laCode, string properties)
    {
        var firstBrace = properties.IndexOf('{');
        var lastBrace = properties.LastIndexOf('}');
        var data = properties.Substring(0, firstBrace) + properties.Substring(firstBrace + 1, lastBrace - firstBrace - 1) + properties.Substring(lastBrace + 1);
        await SetUpEstablishment(urn, @$"{{""name"":""{name}"", ""localAuthority"":{{""code"":""{laCode}"", ""name"":""{laName}""}}, {data}}}").Switch(
            _ => { },
            e => Assert.Fail(e.ToString()));
    }

    [Given(@"establishment ([^\(\)]+) \(([0-9]+)\) exists in multi-academy trust ([0-9]+) with properties:")]
    [Given(@"visible establishment ([^\(\)]+) \(([0-9]+)\) exists in multi-academy trust ([0-9]+) with properties:")]
    public async Task GivenEstablishmentExistsInMultiAcademyTrustMultiline(string name, string urn, string matUid, string properties)
    {
        var firstBrace = properties.IndexOf('{');
        var lastBrace = properties.LastIndexOf('}');
        var data = properties.Substring(0, firstBrace) + properties.Substring(firstBrace + 1, lastBrace - firstBrace - 1) + properties.Substring(lastBrace + 1);
        await SetUpEstablishment(urn, @$"{{""name"":""{name}"", ""multiAcademyTrust"":{{""uid"":""{matUid}""}}, {data}}}").Switch(
            _ => { },
            e => Assert.Fail(e.ToString()));
    }

    [Given(@"establishment ([^\(\)]+) \(([0-9]+)\) exists in diocese ([^\:\(\)]+) with properties:")]
    [Given(@"visible establishment ([^\(\)]+) \(([0-9]+)\) exists in diocese ([^\:\(\)]+) with properties:")]
    public async Task GivenEstablishmentExistsInDioceseMultiline(string name, string urn, string dioceseName, string properties)
    {
        var firstBrace = properties.IndexOf('{');
        var lastBrace = properties.LastIndexOf('}');
        var data = properties.Substring(0, firstBrace) + properties.Substring(firstBrace + 1, lastBrace - firstBrace - 1) + properties.Substring(lastBrace + 1);
        await SetUpEstablishment(urn, @$"{{""name"":""{name}"", ""diocese"":{{""name"":""{dioceseName}""}}, {data}}}").Switch(
            _ => { },
            e => Assert.Fail(e.ToString()));
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

    protected Task<Result<Done>> SetUpEstablishment(string urn, string data, bool isVisible = true,
        bool isDeleted = false)
    {
        var dataDict = JsonConvert.DeserializeObject<Dictionary<string, object>>(data);

        return SetUpEstablishment(urn, dataDict!, isVisible, isDeleted);
    }

    protected async Task<Result<Done>> SetUpEstablishment(string urn, Dictionary<string, object> data,
        bool isVisible = true, bool isDeleted = false)
    {
        var document = await _database.GetAsync<Dictionary<string, object>>("establishments", urn, urn)
            .GetValueOrDefault(new Dictionary<string, object>());

        foreach (var d in data)
        {
            document[d.Key] = d.Value;
        }

        document["id"] = urn;
        document["urn"] = urn;
        document["isVisible"] = isVisible;
        document["isDeleted"] = isDeleted;

        return await _database.UpsertAsync("establishments", urn, urn, document);
    }
    
    private bool IsJsonStructure(string value)
    {
        value = value.Trim();
        return (value.StartsWith('{') && value.EndsWith('}')) || // JSON object
               (value.StartsWith('[') && value.EndsWith(']'));   // JSON array
    }
}