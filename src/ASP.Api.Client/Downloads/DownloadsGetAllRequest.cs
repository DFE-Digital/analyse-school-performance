namespace ASP.Api.Client.Downloads;

public record DownloadsGetAllRequest(DownloadsScopeType ScopeType, string ScopeIdentifier, int? Year);