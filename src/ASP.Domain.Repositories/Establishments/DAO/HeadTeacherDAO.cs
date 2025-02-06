namespace ASP.Domain.Repositories.Establishments.DAO
{
    public class HeadTeacherDAO
    {
        public string Title { get; }
        public string FirstName { get; }
        public string LastName { get; }
        public string PreferredJobTitle { get; }

        public HeadTeacherDAO(string title, string firstName, string lastName, string preferredJobTitle)
        {
            Title = title;
            FirstName = firstName;
            LastName = lastName;
            PreferredJobTitle = preferredJobTitle;
        }
    }
}
