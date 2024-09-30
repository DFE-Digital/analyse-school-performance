using ASP.Web.Core.BreadcrumbTrail;

namespace ASP.Web.Areas.School.ViewModels;

public class SchoolPageViewModel
{
    public string Controller { get; set; }
    public string Title { get; }
    public string SchoolName { get; set; }
    public string SchoolUrn { get; set; }
    public PathString RequestPath { get; }
    public PathString BasePath { get; }
    public BreadcrumbTrailViewModel Breadcrumbs { get; }

    public SchoolPageViewModel(
        string controller,
        string title, 
        string schoolName,
        string schoolUrn,
        PathString requestPath, 
        PathString basePath, 
        BreadcrumbTrailViewModel breadcrumbs
    )
    {
        Controller = controller;
        Title = title;
        SchoolName = schoolName;
        SchoolUrn = schoolUrn;
        RequestPath = requestPath;
        BasePath = basePath;
        Breadcrumbs = breadcrumbs;
    }
}
