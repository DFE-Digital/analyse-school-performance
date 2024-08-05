namespace ASP.Infrastructure.Establishments.DAO;

public class SearchSuggestionsResultDAO
{
    public string Urn { get; }
    public string Name { get; }
    public AddressDAO? Address { get; }
    public string? Laestab { get; }
    public bool IsDeleted { get; }
    public bool IsVisible { get; }

    public SearchSuggestionsResultDAO(string urn,
        string name,
        AddressDAO? address,
        string? laestab,
        bool isDeleted,
        bool isVisible)
    {
        Urn = urn;
        Name = name;
        Address = address;
        Laestab = laestab;
        IsDeleted = isDeleted;
        IsVisible = isVisible;
    }
}