using ASP.Api.Client.Downloads;

namespace ASP.Api.Functions.Downloads;

public static class DomainMappingExtensions
{
    public static DownloadsGetAllResponse ForApiClient(this ASP.Domain.DataDownloads.UseCases.GetAvailableDownloads.GetAvailableDownloadsResponse response)
    {
        return new Client.Downloads.DownloadsGetAllResponse(
            response.Downloads.Select(d => new Client.Downloads.Download {
                DatasetType = d.DatasetType.ToFriendlyName(),
                Id = d.Id,
                Label = d.Label,
                Source = d.Source.ToFriendlyName(),
                Version = d.Version?.FriendlyName,
                Year = d.Year
            })
            .ToList(),
            response.AvailableDates.Select(y => new Client.Downloads.AcademicYear(y.Year, y.Description)).ToList());
    }
}
