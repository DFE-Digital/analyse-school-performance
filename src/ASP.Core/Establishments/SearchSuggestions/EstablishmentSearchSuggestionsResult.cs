namespace ASP.Core.Establishments.SearchSuggestions;

public class EstablishmentSearchSuggestionsResult
{
    public string Urn { get; }
    public string Name { get; }
    public Address? Address { get; }
    public string? Laestab { get; }
    public bool IsDeleted { get; }
    public bool IsVisible { get; }

    public EstablishmentSearchSuggestionsResult(string urn, string name, Address? address, string? laestab, bool isDeleted, bool isVisible)
    {
        Urn = urn;
        Name = name;
        Address = address;
        Laestab = laestab;
        IsDeleted = isDeleted;
        IsVisible = isVisible;
    }

    public override bool Equals(object? obj)
    {
        return obj is EstablishmentSearchSuggestionsResult dao &&
               Urn == dao.Urn;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Urn);
    }
}