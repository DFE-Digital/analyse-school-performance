using Microsoft.AspNetCore.Razor.TagHelpers;

namespace ASP.Web.Accordion
{
    [HtmlTargetElement("asp-accordion")]
    public class AccordionTagHelper : TagHelper
    {
        public string Title { get; set; }
        public string TitleDataTestId { get; set; }
        public string BodyDataTestId { get; set; }
        public override async Task ProcessAsync(TagHelperContext context,
                                                    TagHelperOutput output)
        {
            var content = await output.GetChildContentAsync();
            output.Content.SetHtmlContent(
               $$"""
                <div class="govuk-accordion__section">
                    <div class="govuk-accordion__section-header">
                        <h2 class="govuk-accordion__section-heading" id="{{BodyDataTestId}}">
                            <span class="govuk-accordion__section-button" data-testid="accordion-default-heading-{{TitleDataTestId}}">{{Title}}</span>
                        </h2>
                    </div>
                    <div class="govuk-accordion__section-content" id="{{BodyDataTestId}}" data-testid="{{BodyDataTestId}}" aria-describedby="{{BodyDataTestId}}">{{content.GetContent()}}</div>
                </div>
                """
               );
        }
    }
}
