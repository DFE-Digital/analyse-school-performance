using ASP.Core.DTO.Downloads;
using ASP.Core.Results;
using Newtonsoft.Json;

namespace ASP.Application.UseCases.Downloads.LaDownloads
{
    public class GetLaDownloadsUseCase: IGetLaDownloadsUseCase
    {
        public Task<Result<LaDownloadsDetailsDto>> HandleRequest(GetLaDownloadsUseCaseRequest request)
        {
            var jsonStr = """
                   {
                    "LaCode": 301,
                    "Downloads": [
                        {
                            "Id": "kts-phonics-la-pupil-2022-final",
                            "Name": "Phonics LA pupil data",
                            "DownloadSource": "Key to success",
                            "Year": "2022",
                            "DatasetType": "Phonics",
                            "ReleaseVersion": "Final"
                        },
                        {
                            "Id": "kts-phonics-la-pupil-2023-final",
                            "Name": "Phonics LA pupil data",
                            "DownloadSource": "Key to success",
                            "Year": "2023",
                            "DatasetType": "Phonics",
                            "ReleaseVersion": "Final"
                        },
                        {
                            "Id": "kts-phonics-la-pupil-2024-provisional",
                            "Name": "Phonics LA pupil data",
                            "DownloadSource": "Key to success",
                            "Year": "2024",
                            "DatasetType": "Phonics",
                            "ReleaseVersion": "Provisional"
                        },
                        {
                            "Id": "asp-phonics-la-pupil-2024-revised",
                            "Name": "Phonics LA pupil data",
                            "DownloadSource": "Analyse school performance",
                            "Year": "2024",
                            "DatasetType": "Phonics",
                            "ReleaseVersion": "Revised"
                        },
                        {
                            "Id": "kts-ks2-la-2022-final",
                            "Name": "Key stage 2 LA data",
                            "DownloadSource": "Key to success",
                            "Year": "2022",
                            "DatasetType": "Key stage 2",
                            "ReleaseVersion": "Final"
                        },
                        {
                            "Id": "kts-ks2-la-2023-final",
                            "Name": "Key stage 2 LA data",
                            "DownloadSource": "Key to success",
                            "Year": "2023",
                            "DatasetType": "Key stage 2",
                            "ReleaseVersion": "Final"
                        },
                        {
                            "Id": "kts-ks2-la-2024-revised",
                            "Name": "Key stage 2 LA data",
                            "DownloadSource": "Key to success",
                            "Year": "2024",
                            "DatasetType": "Key stage 2",
                            "ReleaseVersion": "Revised"
                        },
                        {
                            "Id": "asp-ks2-la-2022-provisional",
                            "Name": "Key stage 2 LA data",
                            "DownloadSource": "Analyse school performance",
                            "Year": "2022",
                            "DatasetType": "Key stage 2",
                            "ReleaseVersion": "Provisional"
                        },
                        {
                            "Id": "asp-ks2-la-2023-provisional",
                            "Name": "Key stage 2 LA data",
                            "DownloadSource": "Analyse school performance",
                            "Year": "2023",
                            "DatasetType": "Key stage 2",
                            "ReleaseVersion": "Provisional"
                        },
                        {
                            "Id": "kts-ks4-la-pupil-2022-final",
                            "Name": "Key stage 4 LA pupil data",
                            "DownloadSource": "Key to success",
                            "Year": "2022",
                            "DatasetType": "Key stage 4",
                            "ReleaseVersion": "Final"
                        },
                        {
                            "Id": "kts-ks4-la-pupil-2023-revised",
                            "Name": "Key stage 4 LA pupil data",
                            "DownloadSource": "Key to success",
                            "Year": "2023",
                            "DatasetType": "Key stage 4",
                            "ReleaseVersion": "Revised"
                        },
                        {
                            "Id": "asp-ks4-la-pupil-2022-final",
                            "Name": "Key stage 4 LA pupil data",
                            "DownloadSource": "Analyse school performance",
                            "Year": "2022",
                            "DatasetType": "Key stage 4",
                            "ReleaseVersion": "Final"
                        },
                        {
                            "Id": "asp-ks4-la-pupil-2023-final",
                            "Name": "Key stage 4 LA pupil data",
                            "DownloadSource": "Analyse school performance",
                            "Year": "2023",
                            "DatasetType": "Key stage 4",
                            "ReleaseVersion": "Final"
                        },
                        {
                            "Id": "asp-ks4-la-pupil-2024-provisional",
                            "Name": "Key stage 4 LA pupil data",
                            "DownloadSource": "Analyse school performance",
                            "Year": "2024",
                            "DatasetType": "Key stage 4",
                            "ReleaseVersion": "Provisional"
                        }
                    ]
                }
                """;
            var result = JsonConvert.DeserializeObject<LaDownloadsDetailsDto>(jsonStr);
            result.LaCode = request.LaCode;
            return Task.FromResult(Result.Success(result));
        }
    }
}
