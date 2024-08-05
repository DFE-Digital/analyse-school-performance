using ASP.Core.Establishments;

namespace ASP.Application.UseCases.Establishments.DTO.Mapper;

public static class AdmissionsPolicyDTOMapper
{
    public static AdmissionsPolicyDTO? MapToAdmissionsPolicyDTO(this AdmissionsPolicy? admissionsPolicy)
    {
        if (admissionsPolicy == null) return null;  // Return null directly instead of an empty object

        return new AdmissionsPolicyDTO()
        {
            Code = admissionsPolicy.Code,
            Name = admissionsPolicy.Name
        };
    }
}