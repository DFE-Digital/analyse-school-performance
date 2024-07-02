using ASP.Infrastructure.DAO.Establishment;

namespace ASP.Infrastructure.Mapper.Establishment;

public static class ReligiousDenominationMapper
{
    public static Core.Establishments.ReligiousDenomination? MapToDomainEntityReligiousDenomination(this ReligiousDenomination? religiousDenomination)
    {
        if (religiousDenomination == null) return null;  // Return null directly instead of an empty object
        
        return new Core.Establishments.ReligiousDenomination()
        {
           Code = religiousDenomination.Code,
           Name = religiousDenomination.Name
        };
    }
    
    public static ReligiousDenomination? MapToReligiousDenominationDAO(this Core.Establishments.ReligiousDenomination? religiousDenomination)
    {
        if (religiousDenomination == null) return null;  // Return null directly instead of an empty object

        return new ReligiousDenomination(religiousDenomination.Code, religiousDenomination.Name);
    }
}