namespace ASP.Domain.Establishments
{
    public class EstablishmentType
    {
        public string Code { get; }
        public string Name { get; }

        public EstablishmentType(string code, string name)
        {
            Code = code;
            Name = name;
        }
    }
}
