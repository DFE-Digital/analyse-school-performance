using ASP.Application.UseCases.Downloads.DTO;
using ASP.Core.Results;

namespace ASP.Application.UseCases.Downloads.GetAvailableSchoolDownloads
{
    public class GetAvailableSchoolDownloads : IGetAvailableSchoolDownloads
    {
        public Task<Result<GetAvailableSchoolDownloadsResponse>> HandleRequest(GetAvailableSchoolDownloadsRequest request)
        {
            GetAvailableSchoolDownloadsResponse stubResponse = new(request.Urn, GetDownloads(request.Urn));
            return Task.FromResult(Result.Success(stubResponse));
        }

        private List<DownloadDto> GetDownloads(string urn)
        {
            return new List<DownloadDto>
            {
                new() { Id = $"kts-{urn}-phonics-2022-final-pupil", Name = "Phonics pupil data", DownloadSource = "Key to success", Year = "2022", DatasetType = "Phonics", ReleaseVersion = "Final" },
                new() { Id = $"kts-{urn}-phonics-2023-final-pupil", Name = "Phonics pupil data", DownloadSource = "Key to success", Year = "2023", DatasetType = "Phonics", ReleaseVersion = "Final" },
                new() { Id = $"kts-{urn}-phonics-2024-provisional-pupil", Name = "Phonics pupil data", DownloadSource = "Key to success", Year = "2024", DatasetType = "Phonics", ReleaseVersion = "Provisional" },
                new() { Id = $"asp-{urn}-phonics-2024-revised-pupil", Name = "Phonics pupil data", DownloadSource = "Analyse school performance", Year = "2024", DatasetType = "Phonics", ReleaseVersion = "Revised" },
                new() { Id = $"kts-{urn}-ks2-2022-final-school", Name = "Key stage 2 school data", DownloadSource = "Key to success", Year = "2022", DatasetType = "Key stage 2", ReleaseVersion = "Final" },
                new() { Id = $"kts-{urn}-ks2-2023-final-school", Name = "Key stage 2 school data", DownloadSource = "Key to success", Year = "2023", DatasetType = "Key stage 2", ReleaseVersion = "Final" },
                new() { Id = $"kts-{urn}-ks2-2024-revised-school", Name = "Key stage 2 school data", DownloadSource = "Key to success", Year = "2024", DatasetType = "Key stage 2", ReleaseVersion = "Revised" },
                new() { Id = $"asp-{urn}-ks2-2022-provisional-school", Name = "Key stage 2 school data", DownloadSource = "Analyse school performance", Year = "2022", DatasetType = "Key stage 2", ReleaseVersion = "Provisional" },
                new() { Id = $"asp-{urn}-ks2-2023-provisional-school", Name = "Key stage 2 school data", DownloadSource = "Analyse school performance", Year = "2023", DatasetType = "Key stage 2", ReleaseVersion = "Provisional" },
                new() { Id = $"kts-{urn}-ks4-2022-final-pupil", Name = "Key stage 4 pupil data", DownloadSource = "Key to success", Year = "2022", DatasetType = "Key stage 4", ReleaseVersion = "Final" },
                new() { Id = $"kts-{urn}-ks4-2023-revised-pupil", Name = "Key stage 4 pupil data", DownloadSource = "Key to success", Year = "2023", DatasetType = "Key stage 4", ReleaseVersion = "Revised" },
                new() { Id = $"asp-{urn}-ks4-2022-final-pupil", Name = "Key stage 4 pupil data", DownloadSource = "Analyse school performance", Year = "2022", DatasetType = "Key stage 4", ReleaseVersion = "Final" },
                new() { Id = $"asp-{urn}-ks4-2023-final-pupil", Name = "Key stage 4 pupil data", DownloadSource = "Analyse school performance", Year = "2023", DatasetType = "Key stage 4", ReleaseVersion = "Final" },
                new() { Id = $"asp-{urn}-ks4-2024-provisional-pupil", Name = "Key stage 4 pupil data", DownloadSource = "Analyse school performance", Year = "2024", DatasetType = "Key stage 4", ReleaseVersion = "Provisional" }
            };
        }
    }
}
