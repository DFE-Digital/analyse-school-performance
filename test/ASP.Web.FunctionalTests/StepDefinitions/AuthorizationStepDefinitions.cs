using ASP.Core.Authorization;
using ASP.Web.FunctionalTests.Drivers;

namespace ASP.Web.FunctionalTests.StepDefinitions
{
    [Binding]
    public class AuthorizationStepDefinitions
    {
        private const string ScopedRoles = "LA Named|LA Unnamed|MAT Named|MAT Unnamed|MAT Governor|School Named|School Unnamed|School Governor|Diocese Named|Diocese Unnamed";
        private const string UnscopedRoles = "DfE Named|DfE Unnamed|Super Admin|Ofsted Unnamed";
        private const string AllRoles = $"{UnscopedRoles}|{ScopedRoles}";

        private readonly AspWebContext _web;

        public AuthorizationStepDefinitions(AspWebContext web)
        {
            _web = web;
        }

        [BeforeScenario(Order = 0)]
        public void ClearRoles()
        {
            _web.TestClaimsProvider.ClearClaims();
        }

        [Given($@"I am a logged-in user")]
        public void GivenIAmALoggedInUser()
        {
            _web.TestClaimsProvider.ClearClaims();
            _web.TestClaimsProvider.SetRole(Role.SchoolUnnamed);
        }

        [Given(@"^I am a logged-in user called ""(.+) (.+)""")]
        public void GivenIAmALoggedInUserCalled(string firstName, string lastName)
        {
            _web.TestClaimsProvider.ClearClaims();
            _web.TestClaimsProvider.SetRole(Role.SchoolUnnamed);
            _web.TestClaimsProvider.SetName(firstName, lastName);
        }

        [Given($@"^I am an? ({AllRoles}) user")]
        public void GivenIAmAUserWithTheRole(string roleName)
        {
            _web.TestClaimsProvider.ClearClaims();
            var role = Role.FromName(roleName);

            if(role is null)
            {
                throw new ArgumentException($"Unknown role: {roleName}");
            }

            _web.TestClaimsProvider.SetRole(role);
        }

        [Given($@"^I am an? ({AllRoles}) user called ""(.+) (.+)""")]
        public void GivenIAmAUserWithTheRoleCalled(string roleName, string firstName, string lastName)
        {
            _web.TestClaimsProvider.ClearClaims();
            var role = Role.FromName(roleName);

            if (role is null)
            {
                throw new ArgumentException($"Unknown role: {roleName}");
            }

            _web.TestClaimsProvider.SetRole(role);
            _web.TestClaimsProvider.SetName(firstName, lastName);
        }

        [Given($@"^I am an? ({ScopedRoles}) user for (Local Authority|Multi-Academy Trust|Diocese|Establishment) ([^""]+)")]
        public void GivenIAmAUserWithTheRoleFor(string roleName, string identifierType, string identifierValue)
        {
            var role = Role.FromName(roleName);

            if (role is null)
            {
                throw new ArgumentException($"Unknown role: {roleName}");
            }

            switch (roleName)
            {
                case "LA Named":
                case "LA Unnamed":
                    SetupLAUser(role, identifierValue);
                    break;
                case "MAT Named":
                case "MAT Unnamed":
                case "MAT Governor":
                    SetupMatUser(role, identifierValue);
                    break;
                case "School Named":
                case "School Unnamed":
                case "School Governor":
                    SetupEstablishmentUser(role, identifierValue);
                    break;
                case "Diocese Named":
                case "Diocese Unnamed":
                    SetupDioceseUser(role, identifierValue);
                    break;
                default:
                    break;
            }
        }

        private void SetupLAUser(Role role, string laCode)
        {
            _web.TestClaimsProvider.ClearClaims();
            _web.TestClaimsProvider.SetRole(role);
            _web.TestClaimsProvider.SetCategory("002", "Local Authority");
            _web.TestClaimsProvider.SetEstablishmentNumber(laCode);
        }

        private void SetupDioceseUser(Role role, string dioceseName)
        {
            _web.TestClaimsProvider.ClearClaims();
            _web.TestClaimsProvider.SetRole(role);
            _web.TestClaimsProvider.SetCategory("008", "Other Stakeholders");
            _web.TestClaimsProvider.SetOrganisationName(dioceseName);
        }

        private void SetupMatUser(Role role, string matId)
        {
            _web.TestClaimsProvider.ClearClaims();
            _web.TestClaimsProvider.SetRole(role);
            _web.TestClaimsProvider.SetUid(matId);
        }
        
        private void SetupEstablishmentUser(Role role, string urn)
        {
            _web.TestClaimsProvider.ClearClaims();
            _web.TestClaimsProvider.SetRole(role);
            _web.TestClaimsProvider.SetUrn(urn);
        }
    }
}