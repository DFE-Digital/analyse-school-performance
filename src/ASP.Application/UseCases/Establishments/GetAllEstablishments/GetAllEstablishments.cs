using ASP.Application.UseCases.Establishments.DTO;
using ASP.Application.UseCases.Establishments.DTO.Mapper;
using ASP.Core;
using ASP.Core.Establishments;
using ASP.Core.LocalAuthorities;
using ASP.Core.MultiAcademyTrusts;
using ASP.Core.Results;
using ASP.Core.Scoping;

namespace ASP.Application.UseCases.Establishments.GetAllEstablishments;

public class GetAllEstablishments : IGetAllEstablishments
{
    private readonly IEstablishmentRepository _repository;
    private readonly ILocalAuthorityRepository _localAuthorityRepository;
    private readonly IMultiAcademyTrustRepository _multiAcademyTrustRepository;

    public GetAllEstablishments(IEstablishmentRepository establishmentRepository, ILocalAuthorityRepository localAuthorityRepository,
        IMultiAcademyTrustRepository multiAcademyTrustRepository)
    {
        _repository = establishmentRepository ?? throw new ArgumentNullException(nameof(establishmentRepository));
        _localAuthorityRepository = localAuthorityRepository ?? throw new ArgumentNullException(nameof(localAuthorityRepository));
        _multiAcademyTrustRepository = multiAcademyTrustRepository ?? throw new ArgumentNullException(nameof(multiAcademyTrustRepository));
    }

    public Task<Result<ScopedResultsPage<EstablishmentListingDTO>>> HandleRequest(
        GetAllEstablishmentsRequest request)
    {
        var page = request.Page.GetValueOrDefault(1);
        var resultsPerPage = request.ResultsPerPage.GetValueOrDefault(Constants.SearchResultPageSize);
        var scopeIdentifier = request.ScopeIdentifier.GetValueOrDefault("");

        return 
            from scope in Scope.Validate(request.ScopeType, scopeIdentifier, _localAuthorityRepository, _multiAcademyTrustRepository)
            from results in _repository.GetAllEstablishments(scope, page, resultsPerPage)
            select results.Map(r => r.MapToEstablishmentListingDTO());
    }
}