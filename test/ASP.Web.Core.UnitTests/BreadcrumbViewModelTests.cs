using ASP.Web.Core.BreadcrumbTrail;

namespace ASP.Web.Core.UnitTests
{
    public class BreadcrumbViewModelTests
    {
        [Fact]
        public void Constructor_WhenNullOrWhitespace_Throws()
        {
            Assert.Throws<ArgumentNullException>(() => new BreadcrumbTrailViewModel(null!));
        }
        

        [Fact]
        public void AddBreadcrumb_WhenTitleNullOrWhitespace_Throws()
        {
            var viewModel = new BreadcrumbTrailViewModel();

            Assert.Throws<ArgumentNullException>(() => viewModel.AddBreadcrumb(null!, "href"));
            Assert.Throws<ArgumentException>(() => viewModel.AddBreadcrumb("", "href"));
            Assert.Throws<ArgumentException>(() => viewModel.AddBreadcrumb("   ", "href"));
        }

        [Fact]
        public void AddBreadcrumb_WhenUrlNullOrWhitespace_Throws()
        {
            var viewModel = new BreadcrumbTrailViewModel();

            Assert.Throws<ArgumentNullException>(() => viewModel.AddBreadcrumb("title", null!));
            Assert.Throws<ArgumentException>(() => viewModel.AddBreadcrumb("title", ""));
            Assert.Throws<ArgumentException>(() => viewModel.AddBreadcrumb("title", "   "));
        }

        [Fact]
        public void AddBreadcrumb_AddsBreadcrumbItem()
        {
            var viewModel = new BreadcrumbTrailViewModel();
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
            var viewModel = new BreadcrumbTrailViewModel();
            var breadcrumbTitle = "Home";
            var breadcrumbUrl = "/home";

            var result = viewModel.AddBreadcrumb(breadcrumbTitle, breadcrumbUrl);

            Assert.Same(viewModel, result);
        }

        [Fact]
        public void AddBreadcrumb_AddsMultipleBreadcrumbItems()
        {
            var viewModel = new BreadcrumbTrailViewModel();

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
            var viewModel = new BreadcrumbTrailViewModel();

            List<BreadcrumbItem> breadcrumbs = viewModel.Breadcrumbs;

            Assert.Empty(breadcrumbs);
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
