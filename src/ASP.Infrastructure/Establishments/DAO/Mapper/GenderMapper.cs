namespace ASP.Infrastructure.Establishments.DAO.Mapper;

public static class GenderMapper
{
    public static Core.Establishments.Gender? MapToDomainEntityGender(this GenderDAO? gender)
    {
        if (gender == null) return null;  // Return null directly instead of an empty object

        return new Core.Establishments.Gender()
        {
            Code = gender.Code,
            Name = gender.Name
        };
    }

    public static GenderDAO? MapToGenderDAO(this Core.Establishments.Gender? gender)
    {
        if (gender == null) return null;  // Return null directly instead of an empty object
        return new GenderDAO(gender.Code, gender.Name);
    }
}