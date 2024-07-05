using ASP.Core.Establishments;

namespace ASP.Core.Search.Suggestions;

public class EstablishmentSearchSuggestionsResult
{
    public string Urn { get; set; }
    public string Name { get; set; }
    public Address? Address { get; set; }
    public string? Laestab { get; set; }
    public bool IsDeleted { get; set; }
    public bool IsVisible { get; set; }
}