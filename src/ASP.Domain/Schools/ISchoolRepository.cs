using ASP.Core.Pagination;
using ASP.Core.Results;
using ASP.Core.Optionality;
using ASP.Domain.Schools.LinkedSchools;
using ASP.Domain.Schools.Details;
using ASP.Domain.Schools.Search;
using ASP.Domain.Schools.Access;

namespace ASP.Domain.Schools
{
    public interface ISchoolRepository
    {
        Task<Result<School>> Get(
            SchoolUrn urn,
            CancellationToken cancellationToken = default);

        Task<Result<SchoolWithEstablishmentDetails>> GetWithEstablishmentDetails(
            SchoolUrn urn,
            CancellationToken cancellationToken = default);

        Task<Result<SchoolWithLinks>> GetWithLinkedSchools(
            SchoolUrn urn,
            CancellationToken cancellationToken = default);

        Task<Result<ResultsPage<School>>> GetAll(
            Optional<SchoolAccessScope> scope,
            int page,
            int resultsPerPage,
            CancellationToken cancellationToken = default);

        Task<Result<ResultsPage<School>>> Search(
            ISearchCriteria criteria,
            Optional<SchoolAccessScope> scope,
            int page,
            int resultsPerPage,
            CancellationToken cancellationToken = default);

        Task<Result<List<School>>> SearchSuggestions(
            ISearchCriteria criteria,
            Optional<SchoolAccessScope> scope,
            int maxSuggestions,
            CancellationToken cancellationToken = default);
    }
}
