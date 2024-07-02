using ASP.Infrastructure.DAO.Establishment;

namespace ASP.Infrastructure.Mapper.Establishment;

public static class HeadTeacherMapper
{
    public static Core.Establishments.HeadTeacher? MapToDomainEntityHeadTeacher(this HeadTeacher? headTeacher)
    {
        if (headTeacher == null) return null;  // Return null directly instead of an empty object
        
        return new Core.Establishments.HeadTeacher()
        {
            FirstName = headTeacher.FirstName,
            LastName = headTeacher.LastName,
            PreferredJobTitle = headTeacher.PreferredJobTitle,
            Title = headTeacher.Title
        };
    }
    
    public static HeadTeacher? MapToHeadTeacherDAO(this Core.Establishments.HeadTeacher? headTeacher)
    {
        if (headTeacher == null) return null;  // Return null directly instead of an empty object
        
        return new HeadTeacher(headTeacher.Title,headTeacher.FirstName, headTeacher.LastName, headTeacher.PreferredJobTitle);
    }
}