namespace ASP.Domain.Repositories.Establishments.DAO.Mapper;

public static class EstablishmentTypeMapper
{
    public static Domain.Establishments.EstablishmentType? MapToDomainEntityEstablishmentType(this EstablishmentTypeDAO? establishmentType)
    {
        if (establishmentType == null) return null;  // Return null directly instead of an empty object

        return new Domain.Establishments.EstablishmentType(
            establishmentType.Code,
            establishmentType.Name
        );
    }

    public static EstablishmentTypeDAO? MapToEstablishmentTypeDAO(this Domain.Establishments.EstablishmentType? establishmentType)
    {
        if (establishmentType == null) return null;  // Return null directly instead of an empty object

        return new EstablishmentTypeDAO(
            establishmentType.Code,
            establishmentType.Name
        );
    }
}