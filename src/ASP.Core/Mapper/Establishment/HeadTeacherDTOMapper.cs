using ASP.Core.DTO.Establishment;
using ASP.Core.Establishments;

namespace ASP.Core.Mapper.Establishment;

public static class HeadTeacherDTOMapper
{
    public static HeadTeacherDTO MapToHeadTeacherDTO(this HeadTeacher? headTeacher)
    {
        if (headTeacher == null) return new HeadTeacherDTO();
        return new HeadTeacherDTO()
        {
           FirstName = headTeacher.FirstName,
           LastName = headTeacher.LastName,
           PreferredJobTitle = headTeacher.PreferredJobTitle,
           Title = headTeacher.Title
        };
    }
}