using ASP.Api.Client.ContentTemplates;
using ASP.Api.Client.Downloads;
using ASP.Api.Client.LocalAuthorities;
using ASP.Api.Client.MultiAcademyTrusts;
using ASP.Api.Client.Schools;
using ASP.Core.Network;
using ASP.Core.Pagination;
using ASP.Core.Results;

namespace ASP.Api.Client;

public interface IAspApiClient
{
    Task<Result<List<ContentTemplate>>> ContentTemplatesGetAll(ContentTemplatesGetAllRequest request);
    Task<Result<ContentTemplate>> ContentTemplatesGetSingle(ContentTemplatesGetSingleRequest request);
    Task<Result<Done>> ContentTemplatesUpdateSingle(ContentTemplatesUpdateSingleRequest request);

    Task<Result<DownloadsGetAllResponse>> DownloadsGetAll(DownloadsGetAllRequest request);
    Task<Result<FileStreamResponse>> DownloadsGetPackage(DownloadsGetPackageRequest request);

    Task<Result<SchoolDetails>> SchoolsGetSingle(SchoolsGetSingleRequest request);
    Task<Result<ResultsPage<SchoolListing>>> SchoolsGetAll(SchoolsGetAllRequest request);
    Task<Result<List<SchoolSuggestion>>> SchoolsGetSearchSuggestions(SchoolsGetSearchSuggestionsRequest request);
    Task<Result<SchoolsGetAccessResponse>> SchoolsGetAccess(SchoolsGetAccessRequest request);
    Task<Result<List<SchoolLink>>> SchoolsGetLinkedSchools(SchoolsGetLinkedSchoolsRequest request);

    Task<Result<LookupValueWithCode>> LocalAuthoritiesGetSingle(LocalAuthoritiesGetSingleRequest request);
    Task<Result<ResultsPage<LookupValueWithCode>>> LocalAuthoritiesGetAll(LocalAuthoritiesGetAllRequest request);
    Task<Result<List<LookupValueWithCode>>> LocalAuthoritiesGetSearchSuggestions(LocalAuthoritiesGetSearchSuggestionsRequest request); 
    
    Task<Result<LookupValueWithUid>> MultiAcademyTrustsGetSingle(MultiAcademyTrustsGetSingleRequest request);
}
