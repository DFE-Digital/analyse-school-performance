using ASP.Core.DTO.Downloads;
using ASP.Core.Results;

namespace ASP.Application.UseCases.Downloads.GetAvailableSchoolDownloads
{
    public class GetAvailableSchoolDownloadsUseCase : IGetAvailableSchoolDownloadsUseCase
    {
        public Task<Result<GetAvailableSchoolDownloadsResponse>> HandleRequest(GetAvailableSchoolDownloadsRequest request)
        {
            GetAvailableSchoolDownloadsResponse stubResponse = new(request.Urn, GetDownloadItems(request.Urn));
            return Task.FromResult(Result.Success(stubResponse));
        }


        private List<DownloadsDetailsDto> GetDownloadItems(string urn)
        {
            return new List<DownloadsDetailsDto>
            {
                new DownloadsDetailsDto { Id = $"kts-phonics-pupil-{urn}-2022-final", Name = "Phonics pupil data", DownloadSource = "Key to success", Year = "2022", DatasetType = "Phonics", ReleaseVersion = "Final" },
                new DownloadsDetailsDto { Id = $"kts-phonics-pupil-{urn}-2023-final", Name = "Phonics pupil data", DownloadSource = "Key to success", Year = "2023", DatasetType = "Phonics", ReleaseVersion = "Final" },
                new DownloadsDetailsDto { Id = $"kts-phonics-pupil-{urn}-2024-provisional", Name = "Phonics pupil data", DownloadSource = "Key to success", Year = "2024", DatasetType = "Phonics", ReleaseVersion = "Provisional" },
                new DownloadsDetailsDto { Id = $"asp-phonics-pupil-{urn}-2024-revised", Name = "Phonics pupil data", DownloadSource = "Analyse school performance", Year = "2024", DatasetType = "Phonics", ReleaseVersion = "Revised" },
                new DownloadsDetailsDto { Id = $"kts-ks2-school-{urn}-2022-final", Name = "Key stage 2 school data", DownloadSource = "Key to success", Year = "2022", DatasetType = "Key stage 2", ReleaseVersion = "Final" },
                new DownloadsDetailsDto { Id = $"kts-ks2-school-{urn}-2023-final", Name = "Key stage 2 school data", DownloadSource = "Key to success", Year = "2023", DatasetType = "Key stage 2", ReleaseVersion = "Final" },
                new DownloadsDetailsDto { Id = $"kts-ks2-school-{urn}-2024-revised", Name = "Key stage 2 school data", DownloadSource = "Key to success", Year = "2024", DatasetType = "Key stage 2", ReleaseVersion = "Revised" },
                new DownloadsDetailsDto { Id = $"asp-ks2-school-{urn}-2022-provisional", Name = "Key stage 2 school data", DownloadSource = "Analyse school performance", Year = "2022", DatasetType = "Key stage 2", ReleaseVersion = "Provisional" },
                new DownloadsDetailsDto { Id = $"asp-ks2-school-{urn}-2023-provisional", Name = "Key stage 2 school data", DownloadSource = "Analyse school performance", Year = "2023", DatasetType = "Key stage 2", ReleaseVersion = "Provisional" },
                new DownloadsDetailsDto { Id = $"kts-ks4-pupil-{urn}-2022-final", Name = "Key stage 4 pupil data", DownloadSource = "Key to success", Year = "2022", DatasetType = "Key stage 4", ReleaseVersion = "Final" },
                new DownloadsDetailsDto { Id = $"kts-ks4-pupil-{urn}-2023-revised", Name = "Key stage 4 pupil data", DownloadSource = "Key to success", Year = "2023", DatasetType = "Key stage 4", ReleaseVersion = "Revised" },
                new DownloadsDetailsDto { Id = $"asp-ks4-pupil-{urn}-2022-final", Name = "Key stage 4 pupil data", DownloadSource = "Analyse school performance", Year = "2022", DatasetType = "Key stage 4", ReleaseVersion = "Final" },
                new DownloadsDetailsDto { Id = $"asp-ks4-pupil-{urn}-2023-final", Name = "Key stage 4 pupil data", DownloadSource = "Analyse school performance", Year = "2023", DatasetType = "Key stage 4", ReleaseVersion = "Final" },
                new DownloadsDetailsDto { Id = $"asp-ks4-pupil-{urn}-2024-provisional", Name = "Key stage 4 pupil data", DownloadSource = "Analyse school performance", Year = "2024", DatasetType = "Key stage 4", ReleaseVersion = "Provisional" }
            };
        }
    }
}
