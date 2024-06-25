namespace ASP.Application.UseCases.Establishments.EstablishmentSearch;

public class EstablishmentSearchUseCaseRequest
{
    public EstablishmentSearchUseCaseRequest(string searchTerm, int page)
    {
        SearchTerm = searchTerm;
        Page = page;
    }

    public string SearchTerm { get; set; }
    public int Page { get; set; }
}