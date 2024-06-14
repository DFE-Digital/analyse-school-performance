using ASP.Infrastructure.DAO.Establishment;

namespace ASP.Infrastructure.Mapper.Establishment;

public static class EstablishmentTypeMapper
{
    public static Core.Establishments.EstablishmentType MapToDomainEntityEstablishmentType(this EstablishmentType? establishmentType)
    {
        if (establishmentType == null) return new Core.Establishments.EstablishmentType();
        
        return new Core.Establishments.EstablishmentType()
        {
            Code = establishmentType.Code,
            Name = establishmentType.Name
        };
    }
    
    public static EstablishmentType MapToEstablishmentTypeDAO(this Core.Establishments.EstablishmentType establishmentType)
    {
        return new EstablishmentType(establishmentType.Code, establishmentType.Name);
    }
}