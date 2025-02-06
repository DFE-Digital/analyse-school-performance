namespace ASP.Domain.Establishments.LinkedEstablishments;

public class LinkedEstablishment
{
    public string Urn { get; }
    public string Name { get; }

    public LinkedEstablishment(string urn, string name)
    {
        Urn = urn;
        Name = name;
    }
}