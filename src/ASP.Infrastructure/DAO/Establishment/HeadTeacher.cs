namespace ASP.Infrastructure.DAO.Establishment
{
    public class HeadTeacher
    {
        public string Title { get; }
        public string FirstName { get; }
        public string LastName { get; }
        public string PreferredJobTitle { get; }

        public HeadTeacher(string title, string firstName, string lastName, string preferredJobTitle)
        {
            Title = title;
            FirstName = firstName;
            LastName = lastName;
            PreferredJobTitle = preferredJobTitle;
        }
    }
}
