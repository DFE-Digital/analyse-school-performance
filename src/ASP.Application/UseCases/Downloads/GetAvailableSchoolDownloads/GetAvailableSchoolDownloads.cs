using ASP.Application.UseCases.Downloads.DTO;
using ASP.Core.Optionality;
using ASP.Core.Results;

namespace ASP.Application.UseCases.Downloads.GetAvailableSchoolDownloads
{
    public class GetAvailableSchoolDownloads : IGetAvailableSchoolDownloads
    {
        public Task<Result<GetAvailableSchoolDownloadsResponse>> HandleRequest(GetAvailableSchoolDownloadsRequest request)
        {
            return GetAvailableSchoolDownloadsResponse(request.Urn, request.Year)
                .MapError(error => error is NotFoundError ? Error.NotFound(error.Message) : error).Map(Task.FromResult);
        }

        private Result<GetAvailableSchoolDownloadsResponse> GetAvailableSchoolDownloadsResponse(string urn, Optional<int> year)
        {
            var downloads = new List<DownloadDto>
            {
                new() { Id = $"kts-{urn}-phonics-2022-final-pupil", Label = "Phonics pupil data", Source = "Key to success", Year = 2022, DatasetType = "Phonics", Version = "Final" },
                new() { Id = $"kts-{urn}-phonics-2023-final-pupil", Label = "Phonics pupil data", Source = "Key to success", Year = 2023, DatasetType = "Phonics", Version = "Final" },
                new() { Id = $"kts-{urn}-phonics-2024-provisional-pupil", Label = "Phonics pupil data", Source = "Key to success", Year = 2024, DatasetType = "Phonics", Version = "Provisional" },
                new() { Id = $"asp-{urn}-phonics-2024-revised-pupil", Label = "Phonics pupil data", Source = "Analyse school performance", Year = 2024, DatasetType = "Phonics", Version = "Revised" },
                new() { Id = $"kts-{urn}-ks2-2022-final-school", Label = "Key stage 2 school data", Source = "Key to success", Year = 2022, DatasetType = "Key stage 2", Version = "Final" },
                new() { Id = $"kts-{urn}-ks2-2023-final-school", Label = "Key stage 2 school data", Source = "Key to success", Year = 2023, DatasetType = "Key stage 2", Version = "Final" },
                new() { Id = $"kts-{urn}-ks2-2024-revised-school", Label = "Key stage 2 school data", Source = "Key to success", Year = 2024, DatasetType = "Key stage 2", Version = "Revised" },
                new() { Id = $"asp-{urn}-ks2-2022-provisional-school", Label = "Key stage 2 school data", Source = "Analyse school performance", Year = 2022, DatasetType = "Key stage 2", Version = "Provisional" },
                new() { Id = $"asp-{urn}-ks2-2023-provisional-school", Label = "Key stage 2 school data", Source = "Analyse school performance", Year = 2023, DatasetType = "Key stage 2", Version = "Provisional" },
                new() { Id = $"kts-{urn}-ks4-2022-final-pupil", Label = "Key stage 4 pupil data", Source = "Key to success", Year = 2022, DatasetType = "Key stage 4", Version = "Final" },
                new() { Id = $"kts-{urn}-ks4-2023-revised-pupil", Label = "Key stage 4 pupil data", Source = "Key to success", Year = 2023, DatasetType = "Key stage 4", Version = "Revised" },
                new() { Id = $"asp-{urn}-ks4-2022-final-pupil", Label = "Key stage 4 pupil data", Source = "Analyse school performance", Year = 2022, DatasetType = "Key stage 4", Version = "Final" },
                new() { Id = $"asp-{urn}-ks4-2023-final-pupil", Label = "Key stage 4 pupil data", Source = "Analyse school performance", Year = 2023, DatasetType = "Key stage 4", Version = "Final" },
                new() { Id = $"asp-{urn}-ks4-2024-provisional-pupil", Label = "Key stage 4 pupil data", Source = "Analyse school performance", Year = 2024, DatasetType = "Key stage 4", Version = "Provisional" }
            };

            bool hasNotFoundError = false;
            int? yearValue = null;

            year.IfSome(value =>
            {
                yearValue = value;
                
                downloads = downloads.FindAll(x => x.Year == value);
                    
                if (!downloads.Any())
                {
                    hasNotFoundError = true;
                }
            });
            
            if (hasNotFoundError) return Error.NotFound($@"there are no downloads available for Establishment ""{urn}"" for the given year.");
            
            return new GetAvailableSchoolDownloadsResponse(urn, downloads, yearValue);
        }
    }
}
