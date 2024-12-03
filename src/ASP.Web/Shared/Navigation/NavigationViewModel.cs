namespace ASP.Web.Shared.Navigation;

public class NavigationViewModel
{
    public List<NavigationItemViewModel> NavigationItems { get; set; }

    public NavigationViewModel(List<NavigationItemViewModel> navigationItems)
    {
        NavigationItems = navigationItems;
    }
}
