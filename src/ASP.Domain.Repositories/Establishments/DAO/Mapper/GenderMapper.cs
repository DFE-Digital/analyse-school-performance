namespace ASP.Domain.Repositories.Establishments.DAO.Mapper;

public static class GenderMapper
{
    public static Domain.Establishments.Gender? MapToDomainEntityGender(this GenderDAO? gender)
    {
        if (gender == null) return null;  // Return null directly instead of an empty object

        return new Domain.Establishments.Gender(
            gender.Code,
            gender.Name
        );
    }

    public static GenderDAO? MapToGenderDAO(this Domain.Establishments.Gender? gender)
    {
        if (gender == null) return null;  // Return null directly instead of an empty object

        return new GenderDAO(
            gender.Code,
            gender.Name
        );
    }
}