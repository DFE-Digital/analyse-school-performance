namespace ASP.Api.Client.DataDownloads;

public record GetAvailableDownloadsResponse(List<Download> Downloads, int? Year, List<AcademicYear> AvailableDates);