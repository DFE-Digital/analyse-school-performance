namespace ASP.Infrastructure.Establishments.DAO.Mapper;

public static class ReligiousDenominationMapper
{
    public static Core.Establishments.ReligiousDenomination? MapToDomainEntityReligiousDenomination(this ReligiousDenominationDAO? religiousDenomination)
    {
        if (religiousDenomination == null) return null;  // Return null directly instead of an empty object

        return new Core.Establishments.ReligiousDenomination(
            religiousDenomination.Code,
            religiousDenomination.Name
        );
    }

    public static ReligiousDenominationDAO? MapToReligiousDenominationDAO(this Core.Establishments.ReligiousDenomination? religiousDenomination)
    {
        if (religiousDenomination == null) return null;  // Return null directly instead of an empty object

        return new ReligiousDenominationDAO(
            religiousDenomination.Code, 
            religiousDenomination.Name
        );
    }
}