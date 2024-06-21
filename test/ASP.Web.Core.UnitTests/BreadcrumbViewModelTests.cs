using ASP.Web.Core.Templating;

namespace ASP.Web.Core.UnitTests
{
    public class BreadcrumbViewModelTests
    {
        [Fact]
        public void Constructor_SetsCurrentPageTitle()
        {
            var expectedTitle = "Home";
            
            var viewModel = new BreadcrumbViewModel(expectedTitle);

            Assert.Equal(expectedTitle, viewModel.CurrentPageTitle);
        }

        [Fact]
        public void Constructor_WhenNull_SetsCurrentPageTitleToEmptyString()
        {
            string? title = null;

            var viewModel = new BreadcrumbViewModel(title);

            Assert.Equal(string.Empty, viewModel.CurrentPageTitle);
        }

        [Fact]
        public void AddBreadcrumb_AddsBreadcrumbItem()
        {
            var viewModel = new BreadcrumbViewModel("Page");
            var breadcrumbTitle = "Home";
            var breadcrumbUrl = "/home";

            viewModel.AddBreadcrumb(breadcrumbTitle, breadcrumbUrl);

            Assert.Single(viewModel.Breadcrumbs);
            Assert.Equal(breadcrumbTitle, viewModel.Breadcrumbs[0].Title);
            Assert.Equal(breadcrumbUrl, viewModel.Breadcrumbs[0].Url);
        }

        [Fact]
        public void AddBreadcrumb_ReturnsSameInstance()
        {
            var viewModel = new BreadcrumbViewModel("Page");
            var breadcrumbTitle = "Home";
            var breadcrumbUrl = "/home";

            var result = viewModel.AddBreadcrumb(breadcrumbTitle, breadcrumbUrl);

            Assert.Same(viewModel, result);
        }

        [Fact]
        public void AddBreadcrumb_AddsMultipleBreadcrumbItems()
        {
            var viewModel = new BreadcrumbViewModel("Page");

            viewModel.AddBreadcrumb("Home", "/home")
                     .AddBreadcrumb("About", "/about");

            Assert.Equal(2, viewModel.Breadcrumbs.Count);
            Assert.Equal("Home", viewModel.Breadcrumbs[0].Title);
            Assert.Equal("/home", viewModel.Breadcrumbs[0].Url);
            Assert.Equal("About", viewModel.Breadcrumbs[1].Title);
            Assert.Equal("/about", viewModel.Breadcrumbs[1].Url);
        }

        [Fact]
        public void Breadcrumbs_InitialState_IsEmpty()
        {
            var viewModel = new BreadcrumbViewModel("Page");

            List<BreadcrumbItem> breadcrumbs = viewModel.Breadcrumbs;

            Assert.Empty(breadcrumbs);
        }

        [Fact]
        public void AddBreadcrumb_HandlesEmptyTitleAndUrl()
        {
            var viewModel = new BreadcrumbViewModel("Page");

            viewModel.AddBreadcrumb(string.Empty, string.Empty);

            Assert.Single(viewModel.Breadcrumbs);
            Assert.Equal(string.Empty, viewModel.Breadcrumbs[0].Title);
            Assert.Equal(string.Empty, viewModel.Breadcrumbs[0].Url);
        }

        [Fact]
        public void BreadcrumbItem_Constructor_SetsProperties()
        {
            var title = "Home";
            var url = "/home";

            var breadcrumbItem = new BreadcrumbItem(title, url);

            Assert.Equal(title, breadcrumbItem.Title);
            Assert.Equal(url, breadcrumbItem.Url);
        }

        [Theory]
        [InlineData("Home Page", "home-page")]
        [InlineData("Home  Page", "home--page")]
        public void BreadcrumbItem_TitleIdProperty_GeneratesCorrectId(string inputTitle, string expectedTitle)
        {
            // Arrange
            var title = inputTitle;
            var expectedId = expectedTitle;

            // Act
            var breadcrumbItem = new BreadcrumbItem(title, "/test");

            // Assert
            Assert.Equal(expectedId, breadcrumbItem.TitleId);
        }
    }
}
