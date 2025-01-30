namespace ASP.Domain.Establishments
{
    public class Gender
    {
        public string Code { get; }
        public string Name { get; }

        public Gender(string code, string name)
        {
            Code = code;
            Name = name;
        }
    }
}
