namespace ASP.Web.Models
{
    public class EditContentPageModel
    {
        public string Id { get; set; }
        public string PageTitle { get; set; }
        public List<EditViewComponentModel> Views { get; set; }
    }
}
