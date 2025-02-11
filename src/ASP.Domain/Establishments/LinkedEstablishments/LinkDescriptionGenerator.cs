namespace ASP.Domain.Establishments.LinkedEstablishments;

public class LinkDescriptionGenerator : ILinkDescriptionGenerator
{
    public string GenerateDescription(
        EstablishmentDetails establishment,
        DateTime? establishedDate,
        string linkTypeCode,
        List<LinkedEstablishment> linkedEstablishments)
    {
        var name = establishment.Name;
        var establishmentsText = FormatEstablishmentsList(linkedEstablishments);

        return linkTypeCode switch
        {
            "1"    => FormatDateMessage($"{name} was previously {establishmentsText}", "up until", establishedDate),
            "1F"   => FormatDateMessage($"{name} was created as the result of a split from {establishmentsText}", "on", establishedDate),
            "1I"   => establishedDate.HasValue
                        ? FormatDateMessage($"{name} was previously {establishmentsText}, which closed", "on", establishedDate)
                        : $"{name} was previously {establishmentsText}.",
            "1L"   => FormatDateMessage($"{name} was merged with {establishmentsText}", "on", establishedDate),
            "2" or "2A" => FormatDateMessage($"{name} became {establishmentsText}", "on", establishedDate),
            "2F"  => GenerateSuccessorSplitDescription(establishment, name, establishmentsText, establishedDate, linkedEstablishments.Count),
            "2K"  => FormatDateMessage($"{name} was amalgamated into {establishmentsText}", "on", establishedDate),
            "2O" or "2P" or "2Q" or "6" => FormatDateMessage($"{name} was merged with {establishmentsText}", "on", establishedDate),
            "6.1" => GeneratePredecessorAmalgamatedDescription(establishment, name, establishmentsText, establishedDate, linkedEstablishments.Count),
            "6.2" => FormatDateMessage($"{name} was amalgamated into {establishmentsText}", "on", establishedDate),
            _     => FormatDateMessage($"{name} was linked to {establishmentsText}", "on", establishedDate)
        };
    }

    /// <summary>
    /// Returns a formatted string that appends the connector and a formatted date if a date exists.
    /// Otherwise it returns the base message with a trailing period.
    /// </summary>
    private static string FormatDateMessage(string baseMessage, string connector, DateTime? date)
    {
        return date.HasValue
            ? $"{baseMessage} {connector} {date.Value:d MMMM yyyy}."
            : $"{baseMessage}.";
    }

    private static string GenerateSuccessorSplitDescription(
        EstablishmentDetails establishment,
        string name,
        string establishmentsText,
        DateTime? establishedDate,
        int establishmentCount)
    {
        if (establishmentCount == 1)
        {
            return FormatDateMessage($"{establishmentsText} was split off from {name}", "on", establishedDate);
        }

        bool isClosedOnSplitDate = establishedDate.HasValue &&
                                   establishment.CloseDate.HasValue &&
                                   establishment.CloseDate.Value.Date == establishedDate.Value.Date;

        if (isClosedOnSplitDate)
        {
            return FormatDateMessage($"{name} was split into {establishmentsText}", "on", establishedDate);
        }

        return FormatDateMessage($"{establishmentsText} were split off from {name}", "on", establishedDate);
    }

    private static string GeneratePredecessorAmalgamatedDescription(
        EstablishmentDetails establishment,
        string name,
        string establishmentsText,
        DateTime? establishedDate,
        int establishmentCount)
    {
        if (establishmentCount == 1)
        {
            return FormatDateMessage($"{name} was amalgamated with {establishmentsText}", "on", establishedDate);
        }

        bool isCreatedOnAmalgamationDate = establishedDate.HasValue &&
                                           establishment.OpenDate.HasValue &&
                                           establishment.OpenDate.Value.Date >= establishedDate.Value.Date;

        if (isCreatedOnAmalgamationDate)
        {
            return FormatDateMessage($"{name} was created as an amalgamation of {establishmentsText}", "on", establishedDate);
        }

        return FormatDateMessage($"{name} was amalgamated with {establishmentsText}", "on", establishedDate);
    }

    private static string FormatEstablishmentsList(List<LinkedEstablishment> establishments)
    {
        if (!establishments.Any())
            return string.Empty;

        if (establishments.Count == 1)
            return FormatEstablishmentLink(establishments[0]);

        var allButLast = establishments.Take(establishments.Count - 1)
                                       .Select(FormatEstablishmentLink);
        var last = FormatEstablishmentLink(establishments[^1]);

        return $"{string.Join(", ", allButLast)} and {last}";
    }

    private static string FormatEstablishmentLink(LinkedEstablishment establishment)
    {
        return $"[{establishment.Name}]({establishment.Urn})";
    }
}