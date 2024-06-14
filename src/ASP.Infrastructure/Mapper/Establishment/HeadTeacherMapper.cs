using ASP.Infrastructure.DAO.Establishment;

namespace ASP.Infrastructure.Mapper.Establishment;

public static class HeadTeacherMapper
{
    public static Core.Establishments.HeadTeacher MapToDomainEntityHeadTeacher(this HeadTeacher? headTeacher)
    {
        if (headTeacher == null) return new Core.Establishments.HeadTeacher();
        
        return new Core.Establishments.HeadTeacher()
        {
            FirstName = headTeacher.FirstName,
            LastName = headTeacher.LastName,
            PreferredJobTitle = headTeacher.PreferredJobTitle,
            Title = headTeacher.Title
        };
    }
    
    public static HeadTeacher MapToHeadTeacherDAO(this Core.Establishments.HeadTeacher headTeacher)
    {
        return new HeadTeacher(headTeacher.Title,headTeacher.FirstName, headTeacher.LastName, headTeacher.PreferredJobTitle);
    }
}