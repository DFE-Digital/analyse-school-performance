using Microsoft.AspNetCore.Razor.TagHelpers;

namespace ASP.Web.Areas.Shared.ErrorSummary
{ 

    [HtmlTargetElement("asp-error-summary")]
    public class ErrorSummaryTagHelper : TagHelper
    {
        public override async Task ProcessAsync(TagHelperContext context,
                                                    TagHelperOutput output)
        {
            var content = await output.GetChildContentAsync();
            output.Content.SetHtmlContent(
               $$"""
                <div class="govuk-error-summary" data-module="govuk-error-summary">
                    <div role="alert">
                        <h2 class="govuk-error-summary__title">
                            There is a problem
                        </h2>
                        <div class="govuk-error-summary__body">
                            <ul class="govuk-list govuk-error-summary__list">
                                {{content.GetContent()}}
                            </ul>
                        </div>
                    </div>
                </div>
                """
               );
        }
    }
}
