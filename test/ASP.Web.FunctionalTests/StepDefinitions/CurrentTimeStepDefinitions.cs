using ASP.Core.Time;

namespace ASP.Web.FunctionalTests.StepDefinitions;

[Binding]
public class CurrentTimeStepDefinitions : Test.Reqnroll.CurrentTimeStepDefinitions
{
    public CurrentTimeStepDefinitions(CurrentTimeProvider timeProvider)
        : base(timeProvider)
    {
    }
}
