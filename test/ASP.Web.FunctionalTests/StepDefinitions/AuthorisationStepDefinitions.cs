using ASP.Core.Authorisation;
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
        
        [Given($@"I am an? (DfE Named|DfE Unnamed|Super Admin|Ofsted|LA Named|LA Unnamed|MAT Named|MAT Unnamed|School Named|School Unnamed|Diocese Named|Diocese Unnamed|MAT Governor|School Governor) user")]
        public void GivenIAmAUserWithTheRole(string role)
        {
            switch (role)
            {
                case "DfE Named":
                    _web.TestClaimsProvider.SetRole(Roles.DfeNamed);
                    break;
                case "DfE Unnamed":
                    _web.TestClaimsProvider.SetRole(Roles.DfeUnnamed);
                    break;
                case "Super Admin":
                    _web.TestClaimsProvider.SetRole(Roles.SuperUser);
                    break;
                case "Ofsted":
                    _web.TestClaimsProvider.SetRole(Roles.OfstedUnnamed);
                    break;
                case "LA Named":
                    _web.TestClaimsProvider.SetRole(Roles.LaNamed);
                    break;
                case "LA Unnamed":
                    _web.TestClaimsProvider.SetRole(Roles.LaUnnamed);
                    break;
                case "MAT Named":
                    _web.TestClaimsProvider.SetRole(Roles.MatNamed);
                    break;
                case "MAT Unnamed":
                    _web.TestClaimsProvider.SetRole(Roles.MatUnnamed);
                    break;
                case "School Named":
                    _web.TestClaimsProvider.SetRole(Roles.SchoolNamed);
                    break;
                case "School Unnamed":
                    _web.TestClaimsProvider.SetRole(Roles.SchoolUnnamed);
                    break;
                case "Diocese Named":
                    _web.TestClaimsProvider.SetRole(Roles.DioceseNamed);
                    break;
                case "Diocese Unnamed":
                    _web.TestClaimsProvider.SetRole(Roles.DioceseUnnamed);
                    break;
                case "MAT Governor":
                    _web.TestClaimsProvider.SetRole(Roles.MatGovernor);
                    break;
                case "School Governor":
                    _web.TestClaimsProvider.SetRole(Roles.SchoolGovernor);
                    break;
                default:
                    throw new ArgumentException($"Unknown role: {role}");
            }
        }

        [Given($@"I am an? (LA Named|LA Unnamed|MAT Named|MAT Unnamed|MAT Governor|School Named|School Unnamed|School Governor|Diocese Named|Diocese Unnamed) user for (Local Authority|Multi-Academy Trust|Diocese|Establishment) ""(.+)""")]
        public void GivenIAmAUserWithTheRoleFor(string role, string identifierType, string identifierValue)
        {
            switch (role)
            {
                case "LA Named":
                    SetupLAUser(Roles.LaNamed, identifierValue);
                    break;
                case "LA Unnamed":
                    SetupLAUser(Roles.LaUnnamed, identifierValue);
                    break;
                case "MAT Named":
                    SetupMatUser(Roles.MatNamed, identifierValue);
                    break;
                case "MAT Unnamed":
                    SetupMatUser(Roles.MatUnnamed, identifierValue);
                    break;
                case "MAT Governor":
                    SetupMatUser(Roles.MatGovernor, identifierValue);
                    break;
                case "School Named":
                    SetupEstablishmentUser(Roles.SchoolNamed, identifierValue);
                    break;
                case "School Unnamed":
                    SetupEstablishmentUser(Roles.SchoolUnnamed, identifierValue);
                    break;
                case "School Governor":
                    SetupEstablishmentUser(Roles.SchoolGovernor, identifierValue);
                    break;
                case "Diocese Named":
                    SetupDioceseUser(Roles.DioceseNamed, identifierValue);
                    break;
                case "Diocese Unnamed":
                    SetupDioceseUser(Roles.DioceseUnnamed, identifierValue);
                    break;
                default:
                    throw new ArgumentException($"Unknown role: {role}");
            }
        }

        private void SetupLAUser(string role, string laCode)
        {
            _web.TestClaimsProvider.SetCategory("002", "Local Authority");
            _web.TestClaimsProvider.SetEstablishmentNumber(laCode);
            _web.TestClaimsProvider.SetRole(role);
        }

        private void SetupDioceseUser(string role, string dioceseName)
        {
            _web.TestClaimsProvider.SetCategory("008", "Other Stakeholders");
            _web.TestClaimsProvider.SetOrganisationName(dioceseName);
            _web.TestClaimsProvider.SetRole(role);
        }

        private void SetupMatUser(string role, string matId)
        {
            _web.TestClaimsProvider.SetUid(matId);
            _web.TestClaimsProvider.SetRole(role);
        }
        
        private void SetupEstablishmentUser(string role, string urn)
        {
            _web.TestClaimsProvider.SetUrn(urn);
            _web.TestClaimsProvider.SetRole(role);
        }
    }
}