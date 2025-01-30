namespace ASP.Infrastructure.Repositories.Establishments.DAO.Mapper;

public static class AdmissionsPolicyMapper
{
    public static Domain.Establishments.AdmissionsPolicy? MapToDomainEntityAdmissionsPolicy(this AdmissionsPolicyDAO? admissionsPolicy)
    {
        if (admissionsPolicy == null) return null;  // Return null directly instead of an empty object

        return new Domain.Establishments.AdmissionsPolicy(
            admissionsPolicy.Code,
            admissionsPolicy.Name
        );
    }

    public static AdmissionsPolicyDAO? MapToAdmissionsPolicyDAO(this Domain.Establishments.AdmissionsPolicy? admissionsPolicy)
    {
        if (admissionsPolicy == null) return null;  // Return null directly instead of an empty object

        return new AdmissionsPolicyDAO(
            admissionsPolicy.Code,
            admissionsPolicy.Name
        );
    }
}