using ASP.Core.Results;

namespace ASP.Domain.Establishments.GetLinkedEstablishments;

public class LinkedEstablishmentsService : ILinkedEstablishmentsService
{
    private readonly IEstablishmentRepository _establishmentRepository;
    private readonly ILinkDescriptionGenerator _linkDescriptionGenerator;

    public LinkedEstablishmentsService(IEstablishmentRepository establishmentRepository,
        ILinkDescriptionGenerator linkDescriptionGenerator)
    {
        _establishmentRepository = establishmentRepository;
        _linkDescriptionGenerator = linkDescriptionGenerator;
    }

    public async Task<Result<LinkedEstablishmentsResponse>> GetLinkedEstablishments(string urn)
    {
        return await _establishmentRepository.GetEstablishmentDetails(urn)
            .Then(async establishment =>
            {
                var links = establishment.Links ?? new List<Link>();
                
                return await GetLinkedEstablishmentDetails(links)
                    .Map(linkedEstablishments =>
                    {
                        var validLinks = links
                            .Where(l => linkedEstablishments.ContainsKey(l.LinkedUrn))
                            .ToList();

                        return new LinkedEstablishmentsResponse(
                            urn,
                            establishment.Name,
                            validLinks.Select(l => l.LinkedUrn).Distinct().ToList(),
                            CreateLinkResponses(establishment, linkedEstablishments, validLinks));
                    });
            });
    }

    private async Task<Result<Dictionary<string, LinkedEstablishment>>> GetLinkedEstablishmentDetails(
        List<Link> links)
    {
        try
        {
            var linkedEstablishments = new Dictionary<string, LinkedEstablishment>();
            var distinctUrns = links.Select(l => l.LinkedUrn).Distinct();

            foreach (var linkedUrn in distinctUrns)
            {
                await _establishmentRepository.GetEstablishmentDetails(linkedUrn)
                    .Then(establishment =>
                    {
                        linkedEstablishments[linkedUrn] = new LinkedEstablishment(
                            linkedUrn,
                            establishment.Name);
                        return Result.Success(linkedEstablishments);
                    });
            }

            return Result.Success(linkedEstablishments);
        }
        catch (Exception ex)
        {
            return Result.Error<Dictionary<string, LinkedEstablishment>>(
                new UnexpectedError($"Error getting linked establishment details: {ex.Message}", ex.StackTrace));
        }
    }

    private List<LinkResponse> CreateLinkResponses(
        EstablishmentDetails establishment,
        Dictionary<string, LinkedEstablishment> linkedEstablishments,
        List<Link> validLinks)
    {
        return validLinks
            .GroupBy(l => new { l.EstablishedDate, TypeCode = l.LinkType?.Code })
            .Select(g =>
            {
                var establishments = g
                    .Select(l => linkedEstablishments[l.LinkedUrn])
                    .OrderBy(e => e.Urn)
                    .ToList();

                return new LinkResponse(
                    g.Key.EstablishedDate,
                    g.First().LinkType,
                    establishments,
                    _linkDescriptionGenerator.GenerateDescription(
                        establishment,
                        g.Key.EstablishedDate,
                        g.Key.TypeCode ?? string.Empty,
                        establishments));
            })
            .OrderBy(l => l.Date)
            .ToList();
    }
}