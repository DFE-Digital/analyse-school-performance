namespace ASP.Api.Client.Downloads;

public record DownloadsGetAllRequest(DownloadsScopeType ScopeType, string ScopeId, int? Year);