using ASP.AcceptanceTests.Drivers;

namespace ASP.AcceptanceTests.StepDefinitions
{
    [Binding]
    public class AuthorisationStepDefinitions
    {
        private readonly AspWebContext _web;

        public AuthorisationStepDefinitions(AspWebContext web)
        {
            _web = web;
        }

        [BeforeScenario(Order = 0)]
        public void ClearRoles()
        {
            _web.TestRoleProvider.ClearRoles();
        }

        [Given(@"I am a user with (.+) roles")]
        public void UserAccess(string roles)
        {
            var splitRoles = roles.Split(new string[] { "and" }, StringSplitOptions.TrimEntries);

            _web.TestRoleProvider.SetRoles(splitRoles);
        }
    }
}