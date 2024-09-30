using ASP.Application.UseCases.Downloads.DTO;
using ASP.Core.Optionality;
using ASP.Core.Results;

namespace ASP.Application.UseCases.Downloads.GetAvailableLADownloads
{
    public class GetAvailableLADownloads : IGetAvailableLADownloads
    {
        public Task<Result<GetAvailableLADownloadsResponse>> HandleRequest(GetAvailableLADownloadsRequest request)
        {
            return GetAvailableLaDownloadsResponse(request.LaCode, request.Year)
                .MapError(error => error is NotFoundError ? Error.NotFound(error.Message) : error).Map(Task.FromResult);
        }

        private Result<GetAvailableLADownloadsResponse> GetAvailableLaDownloadsResponse(string laCode, Optional<int> year)
        {
            var downloads = new List<DownloadDto>
            {
                new() { Id = $"kts-{laCode}-phonics-la-2022-final-pupil", Label = "Phonics LA pupil data", Source = "Key to success", Year = 2022, DatasetType = "Phonics", Version = "Final" },
                new() { Id = $"kts-{laCode}-phonics-la-2023-final-pupil", Label = "Phonics LA pupil data", Source = "Key to success", Year = 2023, DatasetType = "Phonics", Version = "Final" },
                new() { Id = $"kts-{laCode}-phonics-la-2024-provisional-pupil", Label = "Phonics LA pupil data", Source = "Key to success", Year = 2024, DatasetType = "Phonics", Version = "Provisional" },
                new() { Id = $"asp-{laCode}-phonics-la-2024-revised-pupil", Label = "Phonics LA pupil data", Source = "Analyse school performance", Year = 2024, DatasetType = "Phonics", Version = "Revised" },
                new() { Id = $"kts-{laCode}-ks2-la-2022-final", Label = "Key stage 2 LA data", Source = "Key to success", Year = 2022, DatasetType = "Key stage 2", Version = "Final" },
                new() { Id = $"kts-{laCode}-ks2-la-2023-final", Label = "Key stage 2 LA data", Source = "Key to success", Year = 2023, DatasetType = "Key stage 2", Version = "Final" },
                new() { Id = $"kts-{laCode}-ks2-la-2024-revised", Label = "Key stage 2 LA data", Source = "Key to success", Year = 2024, DatasetType = "Key stage 2", Version = "Revised" },
                new() { Id = $"asp-{laCode}-ks2-la-2022-provisional", Label = "Key stage 2 LA data", Source = "Analyse school performance", Year = 2022, DatasetType = "Key stage 2", Version = "Provisional" },
                new() { Id = $"asp-{laCode}-ks2-la-2023-provisional", Label = "Key stage 2 LA data", Source = "Analyse school performance", Year = 2023, DatasetType = "Key stage 2", Version = "Provisional" },
                new() { Id = $"kts-{laCode}-ks4-la-2022-final-pupil", Label = "Key stage 4 LA pupil data", Source = "Key to success", Year = 2022, DatasetType = "Key stage 4", Version = "Final" },
                new() { Id = $"kts-{laCode}-ks4-la-2023-revised-pupil", Label = "Key stage 4 LA pupil data", Source = "Key to success", Year = 2023, DatasetType = "Key stage 4", Version = "Revised" },
                new() { Id = $"asp-{laCode}-ks4-la-2022-final-pupil", Label = "Key stage 4 LA pupil data", Source = "Analyse school performance", Year = 2022, DatasetType = "Key stage 4", Version = "Final" },
                new() { Id = $"asp-{laCode}-ks4-la-2023-final-pupil", Label = "Key stage 4 LA pupil data", Source = "Analyse school performance", Year = 2023, DatasetType = "Key stage 4", Version = "Final" },
                new() { Id = $"asp-{laCode}-ks4-la-2024-provisional-pupil", Label = "Key stage 4 LA pupil data", Source = "Analyse school performance", Year = 2024, DatasetType = "Key stage 4", Version = "Provisional" }
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

            if (hasNotFoundError) return Error.NotFound($@"there are no downloads available for Local Authority ""{laCode}"" for the given year.");
            
            return new GetAvailableLADownloadsResponse(laCode, downloads, yearValue);
        }
    }
}
