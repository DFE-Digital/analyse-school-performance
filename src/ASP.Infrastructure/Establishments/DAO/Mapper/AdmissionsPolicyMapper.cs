namespace ASP.Infrastructure.Establishments.DAO.Mapper;

public static class AdmissionsPolicyMapper
{
    public static Core.Establishments.AdmissionsPolicy? MapToDomainEntityAdmissionsPolicy(this AdmissionsPolicyDAO? admissionsPolicy)
    {
        if (admissionsPolicy == null) return null;  // Return null directly instead of an empty object

        return new Core.Establishments.AdmissionsPolicy()
        {
            Code = admissionsPolicy.Code,
            Name = admissionsPolicy.Name
        };
    }

    public static AdmissionsPolicyDAO? MapToAdmissionsPolicyDAO(this Core.Establishments.AdmissionsPolicy? admissionsPolicy)
    {
        if (admissionsPolicy == null) return null;  // Return null directly instead of an empty object

        return new AdmissionsPolicyDAO(admissionsPolicy.Code, admissionsPolicy.Name);
    }
}