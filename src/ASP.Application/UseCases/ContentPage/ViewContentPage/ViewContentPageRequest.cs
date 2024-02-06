namespace ASP.Application.UseCases.ViewContentPage
{
    public class ViewContentPageRequest
    {
        public string PageContentId { get; set; }

        public ViewContentPageRequest(string pageContentId)
        {
            PageContentId = pageContentId;
        }
    }
}
