using ASP.Web.FunctionalTests.Drivers;

namespace ASP.Web.FunctionalTests.StepDefinitions
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
            _web.TestClaimsProvider.ClearClaims();
        }
        
        [Given(@"I am a user with (.+) roles")]
        public void GivenIAmAUserWithTheRoles(string roles)
        {
            var splitRoles = roles.Split(new string[] { "and" }, StringSplitOptions.TrimEntries);

            foreach (var role in splitRoles)
            {
                _web.TestClaimsProvider.SetRole(role);
            }
        }
        
        [Given(@"I am a user with the (.+) role")]
        public void GivenIAmAUserWithTheRole(string role)
        {
            _web.TestClaimsProvider.SetRole(role);
        }
        
        [Given(@"I am a user with the (.+) role for (EstablishmentNumber|Uid|Urn) (.+)")]
        public void GivenIAmAUserWithTheRoleFor(string role, string identifierType, string identifierValue)
        {
            switch (identifierType)
            {
                case "EstablishmentNumber":
                    SetupUserWithEstablishmentNumber(role, identifierValue);
                    break;
                case "Uid":
                    SetupUserWithUid(role, identifierValue);
                    break;
                case "Urn":
                    SetupUserWithUrn(role, identifierValue);
                    break;
                default:
                    throw new ArgumentException($"Unknown identifier type: {identifierType}");
            }
        }
        
        private void SetupUserWithEstablishmentNumber(string role, string establishmentNumber)
        {
            _web.TestClaimsProvider.SetCategory("002", "Local Authority");
            _web.TestClaimsProvider.SetEstablishmentNumber(establishmentNumber);
            _web.TestClaimsProvider.SetRole(role);
        }
        
        private void SetupUserWithUid(string role, string uid)
        {
            _web.TestClaimsProvider.SetUid(uid);
            _web.TestClaimsProvider.SetRole(role);
        }
        
        private void SetupUserWithUrn(string role, string urn)
        {
            _web.TestClaimsProvider.SetUrn(urn);
            _web.TestClaimsProvider.SetRole(role);
        }
    }
}