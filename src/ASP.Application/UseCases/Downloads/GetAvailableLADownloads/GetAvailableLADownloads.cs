using ASP.Application.UseCases.Downloads.DTO;
using ASP.Core.Results;

namespace ASP.Application.UseCases.Downloads.GetAvailableLADownloads
{
    public class GetAvailableLADownloads : IGetAvailableLADownloads
    {
        public Task<Result<GetAvailableLADownloadsResponse>> HandleRequest(GetAvailableLADownloadsRequest request)
        {
            GetAvailableLADownloadsResponse stubResponse = new(request.LaCode, GetDownloads(request.LaCode));
            return Task.FromResult(Result.Success(stubResponse));
        }

        private List<DownloadDto> GetDownloads(string laCode)
        {
            return new List<DownloadDto>
            {
                new() { Id = $"kts-{laCode}-phonics-la-2022-final-pupil", Name = "Phonics LA pupil data", DownloadSource = "Key to success", Year = "2022", DatasetType = "Phonics", ReleaseVersion = "Final" },
                new() { Id = $"kts-{laCode}-phonics-la-2023-final-pupil", Name = "Phonics LA pupil data", DownloadSource = "Key to success", Year = "2023", DatasetType = "Phonics", ReleaseVersion = "Final" },
                new() { Id = $"kts-{laCode}-phonics-la-2024-provisional-pupil", Name = "Phonics LA pupil data", DownloadSource = "Key to success", Year = "2024", DatasetType = "Phonics", ReleaseVersion = "Provisional" },
                new() { Id = $"asp-{laCode}-phonics-la-2024-revised-pupil", Name = "Phonics LA pupil data", DownloadSource = "Analyse school performance", Year = "2024", DatasetType = "Phonics", ReleaseVersion = "Revised" },
                new() { Id = $"kts-{laCode}-ks2-la-2022-final", Name = "Key stage 2 LA data", DownloadSource = "Key to success", Year = "2022", DatasetType = "Key stage 2", ReleaseVersion = "Final" },
                new() { Id = $"kts-{laCode}-ks2-la-2023-final", Name = "Key stage 2 LA data", DownloadSource = "Key to success", Year = "2023", DatasetType = "Key stage 2", ReleaseVersion = "Final" },
                new() { Id = $"kts-{laCode}-ks2-la-2024-revised", Name = "Key stage 2 LA data", DownloadSource = "Key to success", Year = "2024", DatasetType = "Key stage 2", ReleaseVersion = "Revised" },
                new() { Id = $"asp-{laCode}-ks2-la-2022-provisional", Name = "Key stage 2 LA data", DownloadSource = "Analyse school performance", Year = "2022", DatasetType = "Key stage 2", ReleaseVersion = "Provisional" },
                new() { Id = $"asp-{laCode}-ks2-la-2023-provisional", Name = "Key stage 2 LA data", DownloadSource = "Analyse school performance", Year = "2023", DatasetType = "Key stage 2", ReleaseVersion = "Provisional" },
                new() { Id = $"kts-{laCode}-ks4-la-2022-final-pupil", Name = "Key stage 4 LA pupil data", DownloadSource = "Key to success", Year = "2022", DatasetType = "Key stage 4", ReleaseVersion = "Final" },
                new() { Id = $"kts-{laCode}-ks4-la-2023-revised-pupil", Name = "Key stage 4 LA pupil data", DownloadSource = "Key to success", Year = "2023", DatasetType = "Key stage 4", ReleaseVersion = "Revised" },
                new() { Id = $"asp-{laCode}-ks4-la-2022-final-pupil", Name = "Key stage 4 LA pupil data", DownloadSource = "Analyse school performance", Year = "2022", DatasetType = "Key stage 4", ReleaseVersion = "Final" },
                new() { Id = $"asp-{laCode}-ks4-la-2023-final-pupil", Name = "Key stage 4 LA pupil data", DownloadSource = "Analyse school performance", Year = "2023", DatasetType = "Key stage 4", ReleaseVersion = "Final" },
                new() { Id = $"asp-{laCode}-ks4-la-2024-provisional-pupil", Name = "Key stage 4 LA pupil data", DownloadSource = "Analyse school performance", Year = "2024", DatasetType = "Key stage 4", ReleaseVersion = "Provisional" }
            };
        }
    }
}
