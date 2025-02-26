namespace ASP.Domain.Repositories.Schools
{
    public class HeadTeacherDao
    {
        public string Title { get; }
        public string FirstName { get; }
        public string LastName { get; }
        public string PreferredJobTitle { get; }

        public HeadTeacherDao(string title, string firstName, string lastName, string preferredJobTitle)
        {
            Title = title;
            FirstName = firstName;
            LastName = lastName;
            PreferredJobTitle = preferredJobTitle;
        }
    }
}
