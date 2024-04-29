using ASP.Core.Establishments;
using ASP.Core.Templating;
using TechTalk.SpecFlow.Infrastructure;

namespace ASP.Api.AcceptanceTests.StepDefinitions
{
    [Binding]
    public partial class PageContentStepDefinitions : Test.Acceptance.Core.PageContentStepDefinitions
    {
        public PageContentStepDefinitions(IContentTemplateRepository repository, 
            IEstablishmentRepository establishmentRepository, 
            ISpecFlowOutputHelper outputHelper)
            : base(repository, establishmentRepository, outputHelper)
        {
        }
    }
}
