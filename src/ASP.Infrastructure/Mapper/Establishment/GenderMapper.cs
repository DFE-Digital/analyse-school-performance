using ASP.Infrastructure.DAO.Establishment;

namespace ASP.Infrastructure.Mapper.Establishment;

public static class GenderMapper
{
    public static Core.Establishments.Gender MapToDomainEntityGender(this Gender? gender)
    {
        if (gender == null) return new Core.Establishments.Gender();
        
        return new Core.Establishments.Gender()
        {
           Code = gender.Code,
           Name = gender.Name
        };
    }
    
    public static Gender MapToGenderDAO(this Core.Establishments.Gender gender)
    {
        return new Gender(gender.Code, gender.Name);
    }
}