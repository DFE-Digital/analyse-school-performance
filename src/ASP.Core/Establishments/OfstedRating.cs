namespace ASP.Core.Establishments
{
    public class OfstedRating
    {
        public string Code { get; }
        public string Name { get; }

        public OfstedRating(string code, string name)
        {
            Code = code;
            Name = name;
        }
    }
}
