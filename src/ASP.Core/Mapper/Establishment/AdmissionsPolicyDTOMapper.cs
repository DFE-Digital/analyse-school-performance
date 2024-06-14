using ASP.Core.DTO.Establishment;
using ASP.Core.Establishments;

namespace ASP.Core.Mapper.Establishment;

public static class AdmissionsPolicyDTOMapper
{
    public static AdmissionsPolicyDTO MapToAdmissionsPolicyDTO(this AdmissionsPolicy? admissionsPolicy)
    {
        if (admissionsPolicy == null) return new AdmissionsPolicyDTO();
        
        return new AdmissionsPolicyDTO()
        {
            Code = admissionsPolicy.Code,
            Name = admissionsPolicy.Name
        };
    }
}