using ASP.Web.Helpers;

namespace ASP.Web.UnitTests
{
    public class AttributeHelperTests
    {
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void SetTargetAttribute_NullOrEmptyUrl_ReturnsSelf(string input)
        {
            TestRequestHostProvider requestHostProvider = new("asp.gov.uk");
            AttributeHelper attributeHelper = new(requestHostProvider);

            string result = attributeHelper.SetTargetAttribute(input);

            Assert.Equal("_self", result);
        }


        [Theory]
        [InlineData("http://example.com")]
        [InlineData("https://example.com")]
        public void SetTargetAttribute_ExternalUrl_ReturnsBlank(string url)
        {
            TestRequestHostProvider requestHostProvider = new("asp.gov.uk");
            AttributeHelper attributeHelper = new(requestHostProvider);

            string result = attributeHelper.SetTargetAttribute(url);

            Assert.Equal("_blank", result);
        }


        [Theory]
        [InlineData("/")]
        [InlineData("/help/privacy")]
        [InlineData("help/privacy")]
        [InlineData("http://asp.gov.uk/help/privacy")]
        [InlineData("https://asp.gov.uk/help/privacy")]
        public void SetTargetAttribute_InternalUrl_ReturnsSelf(string url)
        {
            TestRequestHostProvider requestHostProvider = new("asp.gov.uk");
            AttributeHelper attributeHelper = new(requestHostProvider);

            string result = attributeHelper.SetTargetAttribute(url);

            Assert.Equal("_self", result);
        }


        [Theory]
        [InlineData("true")]
        [InlineData("123456789")]
        [InlineData("<script>alert('Hello');</script>")]
        public void SetTargetAttribute_InvalidUrl_ReturnsSelf(string url)
        {
            TestRequestHostProvider requestHostProvider = new("asp.gov.uk");
            AttributeHelper attributeHelper = new(requestHostProvider);

            string result = attributeHelper.SetTargetAttribute(url);

            Assert.Equal("_self", result);
        }
    }
}
