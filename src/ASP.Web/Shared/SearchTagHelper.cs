using ASP.Web.Features.Search;
using ASP.Web.Shared.Components.SearchResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace ASP.Web.Shared
{
    [HtmlTargetElement("asp-search", Attributes = "model")]
    public class SearchTagHelper : TagHelper
    {
        private readonly IViewComponentHelper _viewComponentHelper;

        public SearchTagHelper(IViewComponentHelper viewComponentHelper)
        {
            _viewComponentHelper = viewComponentHelper;
        }

        [HtmlAttributeNotBound]
        [ViewContext]
        public ViewContext? ViewContext { get; set; }

        [HtmlAttributeName("model")]
        public SearchViewModel? Model { get; set; }

        public string? SearchFormWidth { get; set; }

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            ((IViewContextAware)_viewComponentHelper).Contextualize(ViewContext);
            var childContent = await output.GetChildContentAsync();

            var content = await _viewComponentHelper.InvokeAsync(typeof(SearchViewComponent), new { Search = Model, ChildContent = childContent.GetContent(), SearchFormWidth });
            output.TagMode = TagMode.StartTagAndEndTag;

            output.SuppressOutput();
            output.PostElement.SetHtmlContent(content);
        }
    }
}
