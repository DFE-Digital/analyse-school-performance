namespace ASP.Infrastructure.Repositories.Establishments.DAO.Mapper;

public static class ReligiousDenominationMapper
{
    public static Domain.Establishments.ReligiousDenomination? MapToDomainEntityReligiousDenomination(this ReligiousDenominationDAO? religiousDenomination)
    {
        if (religiousDenomination == null) return null;  // Return null directly instead of an empty object

        return new Domain.Establishments.ReligiousDenomination(
            religiousDenomination.Code,
            religiousDenomination.Name
        );
    }

    public static ReligiousDenominationDAO? MapToReligiousDenominationDAO(this Domain.Establishments.ReligiousDenomination? religiousDenomination)
    {
        if (religiousDenomination == null) return null;  // Return null directly instead of an empty object

        return new ReligiousDenominationDAO(
            religiousDenomination.Code,
            religiousDenomination.Name
        );
    }
}