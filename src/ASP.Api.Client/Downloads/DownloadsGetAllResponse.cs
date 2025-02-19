namespace ASP.Api.Client.Downloads;

public record DownloadsGetAllResponse(
    List<Download> Downloads,
    List<AcademicYear> AvailableDates);