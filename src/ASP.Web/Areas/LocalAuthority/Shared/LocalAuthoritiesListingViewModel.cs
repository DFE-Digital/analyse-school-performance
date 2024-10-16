using ASP.Application.UseCases.LocalAuthorities.DTO;

namespace ASP.Web.Areas.LocalAuthority.Shared;

public class LocalAuthoritiesListingViewModel
{
    public IEnumerable<LocalAuthorityDTO> LocalAuthorities { get; }

    public LocalAuthoritiesListingViewModel(IEnumerable<LocalAuthorityDTO> localAuthorities)
    {
        LocalAuthorities = localAuthorities;
    }
}