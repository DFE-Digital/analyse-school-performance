using ASP.Infrastructure.DAO.Establishment;

namespace ASP.Infrastructure.Mapper.Establishment;

public static class EstablishmentTypeMapper
{
    public static Core.Establishments.EstablishmentType? MapToDomainEntityEstablishmentType(this EstablishmentType? establishmentType)
    {
        if (establishmentType == null) return null;  // Return null directly instead of an empty object
        
        return new Core.Establishments.EstablishmentType()
        {
            Code = establishmentType.Code,
            Name = establishmentType.Name
        };
    }
    
    public static EstablishmentType? MapToEstablishmentTypeDAO(this Core.Establishments.EstablishmentType? establishmentType)
    {
        if (establishmentType == null) return null;  // Return null directly instead of an empty object
        return new EstablishmentType(establishmentType.Code, establishmentType.Name);
    }
}