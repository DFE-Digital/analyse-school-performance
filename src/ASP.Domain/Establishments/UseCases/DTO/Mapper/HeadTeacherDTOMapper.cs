namespace ASP.Domain.Establishments.UseCases.DTO.Mapper;

public static class HeadTeacherDTOMapper
{
    public static HeadTeacherDTO? MapToHeadTeacherDTO(this HeadTeacher? headTeacher)
    {
        if (headTeacher == null) return null;  // Return null directly instead of an empty object
        return new HeadTeacherDTO() {
            FirstName = headTeacher.FirstName,
            LastName = headTeacher.LastName,
            PreferredJobTitle = headTeacher.PreferredJobTitle,
            Title = headTeacher.Title
        };
    }
}