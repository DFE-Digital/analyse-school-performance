using ASP.Application.UseCases.Establishments.DTO;
using ASP.Application.UseCases.Establishments.DTO.Mapper;
using ASP.Core;
using ASP.Core.Establishments;
using ASP.Core.LocalAuthorities;
using ASP.Core.MultiAcademyTrusts;
using ASP.Core.Results;
using ASP.Core.Scope;

namespace ASP.Application.UseCases.Establishments.GetAllEstablishments;

public class GetAllEstablishments : IGetAllEstablishments
{
    private readonly IEstablishmentRepository _repository;
    private readonly ILocalAuthorityRepository _localAuthorityRepository;
    private readonly IMultiAcademyTrustRepository _multiAcademyTrustRepository;

    public GetAllEstablishments(IEstablishmentRepository establishmentRepository, ILocalAuthorityRepository localAuthorityRepository,
        IMultiAcademyTrustRepository multiAcademyTrustRepository)
    {
        _repository = establishmentRepository ??
                      throw new ArgumentNullException(nameof(establishmentRepository));
        _localAuthorityRepository = localAuthorityRepository ?? throw new ArgumentNullException(nameof(localAuthorityRepository));
        _multiAcademyTrustRepository = multiAcademyTrustRepository ?? throw new ArgumentNullException(nameof(multiAcademyTrustRepository));
    }

    public async Task<Result<ScopedResultsPage<EstablishmentListingDTO>>> HandleRequest(
        GetAllEstablishmentsRequest request)
    {
        var page = request.Page ?? 1;
        var resultsPerPage = request.ResultsPerPage ?? Constants.SearchResultPageSize;
        
        var scope = new Scope(request.ScopeType, request.ScopeIdentifier);

        return await scope
            .Validate(_localAuthorityRepository, _multiAcademyTrustRepository)
            .Then(async x => await _repository.GetAllEstablishments(scope, page, resultsPerPage)
            ).Map(results => results.Map(r => r.MapToEstablishmentListingDTO()));
    }
}