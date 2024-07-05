namespace ASP.Infrastructure.DAO.Establishment;

public class SearchSuggestionsResultDAO
{
    public string Urn { get; }
    public string Name { get; }
    public Address? Address { get; }
    public string? Laestab { get; }
    public bool IsDeleted { get; }
    public bool IsVisible { get; }

    public SearchSuggestionsResultDAO(string urn,
        string name,
        Address? address,
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