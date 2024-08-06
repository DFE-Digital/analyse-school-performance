namespace ASP.Infrastructure.Establishments.DAO.Mapper;

public static class EstablishmentTypeMapper
{
    public static Core.Establishments.EstablishmentType? MapToDomainEntityEstablishmentType(this EstablishmentTypeDAO? establishmentType)
    {
        if (establishmentType == null) return null;  // Return null directly instead of an empty object

        return new Core.Establishments.EstablishmentType(
            establishmentType.Code,
            establishmentType.Name
        );
    }

    public static EstablishmentTypeDAO? MapToEstablishmentTypeDAO(this Core.Establishments.EstablishmentType? establishmentType)
    {
        if (establishmentType == null) return null;  // Return null directly instead of an empty object

        return new EstablishmentTypeDAO(
            establishmentType.Code, 
            establishmentType.Name
        );
    }
}