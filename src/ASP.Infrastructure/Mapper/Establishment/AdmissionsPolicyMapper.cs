using ASP.Infrastructure.DAO.Establishment;

namespace ASP.Infrastructure.Mapper.Establishment;

public static class AdmissionsPolicyMapper
{
    public static Core.Establishments.AdmissionsPolicy MapToDomainEntityAdmissionsPolicy(this AdmissionsPolicy? admissionsPolicy)
    {
        if (admissionsPolicy == null) return new Core.Establishments.AdmissionsPolicy();
        
        return new Core.Establishments.AdmissionsPolicy()
        {
           Code = admissionsPolicy.Code,
           Name = admissionsPolicy.Name
        };
    }
    
    public static AdmissionsPolicy MapToAdmissionsPolicyDAO(this Core.Establishments.AdmissionsPolicy admissionsPolicy)
    {
        return new AdmissionsPolicy(admissionsPolicy.Code, admissionsPolicy.Name);
    }
}