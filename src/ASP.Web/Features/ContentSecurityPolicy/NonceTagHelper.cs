using Microsoft.AspNetCore.Razor.TagHelpers;

namespace ASP.Web.Features.ContentSecurityPolicy
{
    [HtmlTargetElement("script", Attributes = "add-nonce")]
    [HtmlTargetElement("style", Attributes = "add-nonce")]
    [HtmlTargetElement("link", Attributes = "add-nonce")]
    public class NonceTagHelper : TagHelper
    {
        private readonly INonceService _nonceService;
        [HtmlAttributeName("add-nonce")]
        public bool AddNonce { get; set; }

        public NonceTagHelper(INonceService nonceService)
        {
            _nonceService = nonceService;
        }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if (AddNonce)
            {
                output.Attributes.Add("nonce", _nonceService.GetNonce());
            }
        }
    }
}
