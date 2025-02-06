namespace ASP.Domain.Repositories.Establishments.DAO.Mapper;

public static class HeadTeacherMapper
{
    public static Domain.Establishments.HeadTeacher? MapToDomainEntityHeadTeacher(this HeadTeacherDAO? headTeacher)
    {
        if (headTeacher == null) return null;  // Return null directly instead of an empty object

        return new Domain.Establishments.HeadTeacher(
            headTeacher.Title,
            headTeacher.FirstName,
            headTeacher.LastName,
            headTeacher.PreferredJobTitle
        );
    }

    public static HeadTeacherDAO? MapToHeadTeacherDAO(this Domain.Establishments.HeadTeacher? headTeacher)
    {
        if (headTeacher == null) return null;  // Return null directly instead of an empty object

        return new HeadTeacherDAO(
            headTeacher.Title,
            headTeacher.FirstName,
            headTeacher.LastName,
            headTeacher.PreferredJobTitle
        );
    }
}