using ASP.Core.Time;

namespace ASP.Api.FunctionalTests.StepDefinitions;

[Binding]
public class CurrentTimeStepDefinitions : Test.SpecFlow.CurrentTimeStepDefinitions
{
    public CurrentTimeStepDefinitions(CurrentTimeProvider timeProvider)
        : base(timeProvider)
    {
    }
}
