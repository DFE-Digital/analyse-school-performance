namespace ASP.Infrastructure.Establishments.DAO.Mapper;

public static class HeadTeacherMapper
{
    public static Core.Establishments.HeadTeacher? MapToDomainEntityHeadTeacher(this HeadTeacherDAO? headTeacher)
    {
        if (headTeacher == null) return null;  // Return null directly instead of an empty object

        return new Core.Establishments.HeadTeacher(
            headTeacher.Title,
            headTeacher.FirstName,
            headTeacher.LastName,
            headTeacher.PreferredJobTitle
        );
    }

    public static HeadTeacherDAO? MapToHeadTeacherDAO(this Core.Establishments.HeadTeacher? headTeacher)
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