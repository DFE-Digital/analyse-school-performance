using ASP.Application.UseCases.BlobStorageDemoFileDownload;
using ASP.Application.UseCases.BlobStorageDemoZipFileDownload;
using ASP.Application.UseCases.ContentTemplates.UpdateContentTemplate;
using ASP.Application.UseCases.ContentTemplates.ViewContentTemplate;
using ASP.Application.UseCases.Downloads.DownloadAsZip;
using ASP.Application.UseCases.Downloads.GetAvailableSchoolDownloads;
using ASP.Application.UseCases.Establishments.DTO;
using ASP.Application.UseCases.Establishments.EstablishmentSearch;
using ASP.Application.UseCases.Establishments.EstablishmentSearchSuggestions;
using ASP.Application.UseCases.Establishments.GetAllEstablishments;
using ASP.Application.UseCases.Establishments.GetEstablishmentDetails;
using ASP.Application.UseCases.LocalAuthorities.GetAllLocalAuthorities;
using ASP.Application.UseCases.LocalAuthorities.GetLocalAuthority;
using ASP.Application.UseCases.MultiAcademyTrusts.GetMultiAcademyTrust;
using LA = ASP.Application.UseCases.LocalAuthorities.DTO;
using MAT = ASP.Application.UseCases.MultiAcademyTrusts.DTO;
using ASP.Core.Establishments.Search;
using ASP.Core.Establishments.SearchSuggestions;
using ASP.Core.Results;
using ASP.Core.Scoping;
using ASP.Core.Templating;
using ASP.Core.Utilities;

namespace ASP.Application
{
    public interface IAspApiClient
    {
        Task<Result<ContentTemplate>> ViewContentTemplate(ViewContentTemplateRequest request);
        Task<Result<Done>> UpdateContentTemplate(UpdateContentTemplateRequest request);
        Task<Result<List<ContentTemplate>>> GetAllContentTemplates();
        Task<Result<EstablishmentDetailsDTO>> GetEstablishmentDetails(GetEstablishmentDetailsRequest request);
        Task<Result<GetAvailableSchoolDownloadsResponse>> GetAvailableSchoolDownloads(GetAvailableSchoolDownloadsRequest request);
        Task<Result<FileStreamResponse>> DownloadAsZipFile(DownloadAsZipFileRequest request);
        Task<Result<ScopedSearchResultsPage<EstablishmentListingDTO>>> EstablishmentSearch(EstablishmentSearchRequest request);
        Task<Result<SearchSuggestionsResult<EstablishmentSuggestionDTO>>> EstablishmentSearchSuggestions(EstablishmentSearchSuggestionsRequest request);
        Task<Result<LA.LocalAuthorityDTO>> GetLocalAuthority(GetLocalAuthorityRequest request);
        Task<Result<MAT.MultiAcademyTrustDTO>> GetMultiAcademyTrust(GetMultiAcademyTrustRequest request);
        Task<Result<ScopedResultsPage<EstablishmentListingDTO>>> GetAllEstablishments(GetAllEstablishmentsRequest request);
        Task<Result<ResultsPage<LA.LocalAuthorityDTO>>> GetAllLocalAuthorities(GetAllLocalAuthoritiesRequest request);
        Task<Result<FileStreamResponse>> BlobStorageDemoFileDownload(BlobStorageDemoFileDownloadRequest request);
        Task<Result<FileStreamResponse>> BlobStorageDemoZipFileDownload(BlobStorageDemoZipFileDownloadRequest request);
    }
}
