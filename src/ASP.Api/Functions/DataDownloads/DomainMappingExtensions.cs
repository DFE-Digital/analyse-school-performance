using ASP.Domain.DataDownloads.UseCases.GetAvailableDownloads;

namespace ASP.Api.Functions.DataDownloads;

public static class DomainMappingExtensions
{
    public static Client.DataDownloads.GetAvailableDownloadsResponse ForApiClient(this GetAvailableDownloadsResponse response)
    {
        return new Client.DataDownloads.GetAvailableDownloadsResponse(
            response.Downloads.Select(d => new Client.DataDownloads.Download {
                DatasetType = d.DatasetType.ToFriendlyName(),
                Id = d.Id,
                Label = d.Label,
                Source = d.Source.ToFriendlyName(),
                Version = d.Version?.FriendlyName,
                Year = d.Year
            })
            .ToList(),
            response.Year,
            response.AvailableDates.Select(y => new Client.DataDownloads.AcademicYear(y.Year, y.Description)).ToList());
    }
}
