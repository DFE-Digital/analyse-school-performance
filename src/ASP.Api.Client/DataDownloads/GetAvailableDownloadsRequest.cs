namespace ASP.Api.Client.DataDownloads;

public record GetAvailableDownloadsRequest(DataDownloadsScopeType ScopeType, string ScopeIdentifier, int? Year);