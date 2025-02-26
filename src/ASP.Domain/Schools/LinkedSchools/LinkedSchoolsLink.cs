namespace ASP.Domain.Schools.LinkedSchools;

public class LinkedSchoolsLink
{
    public string ParentName { get; }
    public DateTime? ParentOpenDate { get; }
    public DateTime? ParentCloseDate { get; }
    public DateTime? Date { get; }
    public LinkType? LinkType { get; }
    public List<School> LinkedSchools { get; }

    public LinkedSchoolsLink(string parentName, DateTime? parentOpenDate, DateTime? parentCloseDate, DateTime? date, LinkType? linkType, List<School> linkedSchools)
    {
        ParentName = parentName;
        ParentOpenDate = parentOpenDate;
        ParentCloseDate = parentCloseDate;
        Date = date;
        LinkType = linkType;
        LinkedSchools = linkedSchools;
    }

    public string Description
    {
        get
        {
            var schoolsList = FormatSchoolsList();

            return LinkType?.Code switch {
                "1" => FormatDateMessage($"{ParentName} was previously {schoolsList}", "up until", Date),
                "1F" => FormatDateMessage($"{ParentName} was created as the result of a split from {schoolsList}", "on", Date),
                "1I" => Date.HasValue
                            ? FormatDateMessage($"{ParentName} was previously {schoolsList}, which closed", "on", Date)
                            : $"{ParentName} was previously {schoolsList}.",
                "1L" => FormatDateMessage($"{ParentName} was merged with {schoolsList}", "on", Date),
                "2" or "2A" => FormatDateMessage($"{ParentName} became {schoolsList}", "on", Date),
                "2F" => GenerateSuccessorSplitDescription(schoolsList),
                "2K" => FormatDateMessage($"{ParentName} was amalgamated into {schoolsList}", "on", Date),
                "2O" or "2P" or "2Q" or "6" => FormatDateMessage($"{ParentName} was merged with {schoolsList}", "on", Date),
                "6.1" => GeneratePredecessorAmalgamatedDescription(schoolsList),
                "6.2" => FormatDateMessage($"{ParentName} was amalgamated into {schoolsList}", "on", Date),
                _ => FormatDateMessage($"{ParentName} was linked to {schoolsList}", "on", Date)
            };
        }
    }

    /// <summary>
    /// Returns a formatted string that appends the connector and a formatted date if a date exists.
    /// Otherwise it returns the base message with a trailing period.
    /// </summary>
    private string FormatDateMessage(string baseMessage, string connector, DateTime? date)
    {
        return date.HasValue
            ? $"{baseMessage} {connector} {date.Value:d MMMM yyyy}."
            : $"{baseMessage}.";
    }

    private string GenerateSuccessorSplitDescription(string schoolsList)
    {
        if (LinkedSchools.Count == 1)
        {
            return FormatDateMessage($"{schoolsList} was split off from {ParentName}", "on", Date);
        }

        bool isClosedOnSplitDate = Date.HasValue &&
                                   ParentCloseDate.HasValue &&
                                   ParentCloseDate.Value.Date == Date.Value.Date;

        if (isClosedOnSplitDate)
        {
            return FormatDateMessage($"{ParentName} was split into {schoolsList}", "on", Date);
        }

        return FormatDateMessage($"{schoolsList} were split off from {ParentName}", "on", Date);
    }

    private string GeneratePredecessorAmalgamatedDescription(string schoolsList)
    {
        if (LinkedSchools.Count == 1)
        {
            return FormatDateMessage($"{ParentName} was amalgamated with {schoolsList}", "on", Date);
        }

        bool isCreatedOnAmalgamationDate = Date.HasValue &&
                                           ParentOpenDate.HasValue &&
                                           ParentOpenDate.Value.Date >= Date.Value.Date;

        if (isCreatedOnAmalgamationDate)
        {
            return FormatDateMessage($"{ParentName} was created as an amalgamation of {schoolsList}", "on", Date);
        }

        return FormatDateMessage($"{ParentName} was amalgamated with {schoolsList}", "on", Date);
    }

    private string FormatSchoolsList()
    {
        if (!LinkedSchools.Any())
            return string.Empty;

        if (LinkedSchools.Count == 1)
            return FormatSchoolLink(LinkedSchools[0]);

        var allButLast = LinkedSchools
            .Take(LinkedSchools.Count - 1)
            .Select(FormatSchoolLink);
        
        var last = FormatSchoolLink(LinkedSchools[^1]);

        return $"{string.Join(", ", allButLast)} and {last}";
    }

    private string FormatSchoolLink(School school)
    {
        return $"[{school.Name}]({school.Urn.Value})";
    }
}