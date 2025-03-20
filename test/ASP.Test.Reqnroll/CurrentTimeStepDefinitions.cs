using ASP.Core.Time;
using Reqnroll;

namespace ASP.Test.Reqnroll;

[Binding]
public class CurrentTimeStepDefinitions
{
    private readonly CurrentTimeProvider _timeProvider;

    public CurrentTimeStepDefinitions(CurrentTimeProvider timeProvider)
    {
        _timeProvider = timeProvider;
    }

    [Given(@"^the current time is (\d{4}/\d{2}/\d{2} \d{2}:\d{2}:\d{2})")]
    public void GivenTheCurrentTimeIs(string dateTime)
    {
        _timeProvider.Override = DateTime.Parse(dateTime);
    }
}