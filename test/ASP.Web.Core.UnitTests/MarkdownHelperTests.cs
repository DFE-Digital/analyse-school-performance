using ASP.Web.Core.Templating;
using Microsoft.AspNetCore.Html;

namespace ASP.Web.Core.UnitTests
{
    public class MarkdownHelperTests
    {
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void ConvertInlineMarkdown_WhenEmptyOrNull_ReturnsEmptyString(string input)
        {
            TestRequestHostProvider requestHostProvider = new("asp.gov.uk");
            AttributeHelper attributeHelper = new(requestHostProvider);
            MarkdownHelper markdownHelper = new(attributeHelper);

            HtmlString result = markdownHelper.ConvertInlineMarkdown(input);

            Assert.Equal("", result.ToString());
        }


        [Theory]
        [InlineData("**bold**", "<strong>bold</strong>")]
        [InlineData("__bold__", "<strong>bold</strong>")]
        public void ConvertInlineMarkdown_WhenBoldText_ConvertsToStrongTag(string input, string expected)
        {
            TestRequestHostProvider requestHostProvider = new("asp.gov.uk");
            AttributeHelper attributeHelper = new(requestHostProvider);
            MarkdownHelper markdownHelper = new(attributeHelper);

            HtmlString result = markdownHelper.ConvertInlineMarkdown(input);

            Assert.Equal(expected, result.ToString());
        }

        [Theory]
        [InlineData("*italic*", "<em>italic</em>")]
        [InlineData("_italic_", "<em>italic</em>")]
        public void ConvertInlineMarkdown_WhenItalicText_ConvertsToEmTag(string input, string expected)
        {
            TestRequestHostProvider requestHostProvider = new("asp.gov.uk");
            AttributeHelper attributeHelper = new(requestHostProvider);
            MarkdownHelper markdownHelper = new(attributeHelper);

            HtmlString result = markdownHelper.ConvertInlineMarkdown(input);

            Assert.Equal(expected, result.ToString());
        }

        [Theory]
        [InlineData("* text *", "* text *")]
        [InlineData("_ text _", "_ text _")]
        public void ConvertInlineMarkdown_WhenSpaceBetweenTextAndItalicMarker_DoesNotConvertToEmTag(string input, string expected)
        {
            TestRequestHostProvider requestHostProvider = new("asp.gov.uk");
            AttributeHelper attributeHelper = new(requestHostProvider);
            MarkdownHelper markdownHelper = new(attributeHelper);

            HtmlString result = markdownHelper.ConvertInlineMarkdown(input);

            Assert.Equal(expected, result.ToString());
        }

        [Theory]
        [InlineData("** text **", "** text **")]
        [InlineData("__ text __", "__ text __")]
        public void ConvertInlineMarkdown_WhenSpaceBetweenTextAndBoldMarker_DoesNotConvertToStrongTag(string input, string expected)
        {
            TestRequestHostProvider requestHostProvider = new("asp.gov.uk");
            AttributeHelper attributeHelper = new(requestHostProvider);
            MarkdownHelper markdownHelper = new(attributeHelper);

            HtmlString result = markdownHelper.ConvertInlineMarkdown(input);

            Assert.Equal(expected, result.ToString());
        }


        [Theory]
        [InlineData("**this is bold* text**", "<strong>this is bold* text</strong>")]
        [InlineData("**this is bold_ text**", "<strong>this is bold_ text</strong>")]
        [InlineData("*this is italic_ text*", "<em>this is italic_ text</em>")]
        [InlineData("_this is italic* text_", "<em>this is italic* text</em>")]
        [InlineData("__this is bold* text__", "<strong>this is bold* text</strong>")]
        [InlineData("__this is bold_ text__", "<strong>this is bold_ text</strong>")]
        [InlineData("**this is bold__ text**", "<strong>this is bold__ text</strong>")]
        [InlineData("__this is bold** text__", "<strong>this is bold** text</strong>")]
        [InlineData("*this is italic** text*", "<em>this is italic** text</em>")]
        [InlineData("*this is italic__ text*", "<em>this is italic__ text</em>")]
        [InlineData("_this is italic** text_", "<em>this is italic** text</em>")]
        [InlineData("_this is italic__ text_", "<em>this is italic__ text</em>")]
        [InlineData("**this is bold** text**", "<strong>this is bold</strong> text**")]
        [InlineData("__this is bold__ text__", "<strong>this is bold</strong> text__")]
        [InlineData("*this is italic* text*", "<em>this is italic</em> text*")]
        [InlineData("_this is italic_ text_", "<em>this is italic</em> text_")]
        public void ConvertInlineMarkdown_WhenUnbalancedMarker_DoesNotWrap(string input, string expected)
        {
            TestRequestHostProvider requestHostProvider = new("asp.gov.uk");
            AttributeHelper attributeHelper = new(requestHostProvider);
            MarkdownHelper markdownHelper = new(attributeHelper);

            HtmlString result = markdownHelper.ConvertInlineMarkdown(input);

            Assert.Equal(expected, result.ToString());
        }

        [Theory]
        [InlineData("[Google](https://www.google.com)", "<a href=\"https://www.google.com\" class=\"govuk-link\" target=\"_blank\">Google</a>")]
        [InlineData("**[Google](https://www.google.com)**", "<strong><a href=\"https://www.google.com\" class=\"govuk-link\" target=\"_blank\">Google</a></strong>")]
        public void ConvertInlineMarkdown_WhenLink_ConvertsToAnchorTag(string input, string expected)
        {

            TestRequestHostProvider requestHostProvider = new("asp.gov.uk");
            AttributeHelper attributeHelper = new(requestHostProvider);
            MarkdownHelper markdownHelper = new(attributeHelper);

            HtmlString result = markdownHelper.ConvertInlineMarkdown(input);

            Assert.Equal(expected, result.ToString());
        }

        [Theory]
        [InlineData("[Google](https://www.google.com){target=\"_blank\"}", "<a href=\"https://www.google.com\" class=\"govuk-link\" target=&quot;_blank&quot;>Google</a>")]
        public void ConvertInlineMarkdown_WhenLinkHasTargetAttribute_ConvertsToAnchorTagAndSetsTarget(string input, string expected)
        {
            TestRequestHostProvider requestHostProvider = new("asp.gov.uk");
            AttributeHelper attributeHelper = new(requestHostProvider);
            MarkdownHelper markdownHelper = new(attributeHelper);

            HtmlString result = markdownHelper.ConvertInlineMarkdown(input);

            Assert.Equal(expected, result.ToString());
        }

        [Theory]
        [InlineData("**_bold italic_**", "<strong><em>bold italic</em></strong>")]
        [InlineData("___bold italic___", "<strong><em>bold italic</em></strong>")]
        [InlineData("***bold italic***", "<strong><em>bold italic</em></strong>")]
        [InlineData("_**italic bold**_", "<em><strong>italic bold</strong></em>")]
        [InlineData("**__double bold__**", "<strong><strong>double bold</strong></strong>")]
        [InlineData("__**double bold**__", "<strong><strong>double bold</strong></strong>")]
        [InlineData("****double bold****", "<strong><strong>double bold</strong></strong>")]
        [InlineData("____double bold____", "<strong><strong>double bold</strong></strong>")]
        [InlineData("*_double italic_*", "<em><em>double italic</em></em>")]
        [InlineData("_*double italic*_", "<em><em>double italic</em></em>")]
        [InlineData("****_double bold italic_****", "<strong><strong><em>double bold italic</em></strong></strong>")]
        [InlineData("_____double bold italic_____", "<strong><strong><em>double bold italic</em></strong></strong>")]
        [InlineData("**___double bold italic___**", "<strong><strong><em>double bold italic</em></strong></strong>")]
        [InlineData("__**_double bold italic_**__", "<strong><strong><em>double bold italic</em></strong></strong>")]
        [InlineData("*_**double italic bold**_*", "<em><em><strong>double italic bold</strong></em></em>")]
        [InlineData("_*__double italic bold__*_", "<em><em><strong>double italic bold</strong></em></em>")]
        public void ConvertInlineMarkdown_WhenBoldOrItalicText_ReturnsCorrectHtml(string input, string expected)
        {
            TestRequestHostProvider requestHostProvider = new("asp.gov.uk");
            AttributeHelper attributeHelper = new(requestHostProvider);
            MarkdownHelper markdownHelper = new(attributeHelper);

            HtmlString result = markdownHelper.ConvertInlineMarkdown(input);

            Assert.Equal(expected, result.ToString());
        }

        [Theory]
        [InlineData("<script>alert('Hello');</script>", "&lt;script&gt;alert(&#39;Hello&#39;);&lt;/script&gt;")]
        [InlineData("<div>Some <strong>bold</strong> text</div>", "&lt;div&gt;Some &lt;strong&gt;bold&lt;/strong&gt; text&lt;/div&gt;")]
        [InlineData("<a href=\"https://example.com\">Link</a>", "&lt;a href=&quot;https://example.com&quot;&gt;Link&lt;/a&gt;")]
        [InlineData("<img src=\"image.jpg\" alt=\"Image\">", "&lt;img src=&quot;image.jpg&quot; alt=&quot;Image&quot;&gt;")]
        public void ConvertInlineMarkdown_WhenInputContainsHtmlCharacters_EscapesHtml(string input, string expected)
        {
            TestRequestHostProvider requestHostProvider = new("asp.gov.uk");
            AttributeHelper attributeHelper = new(requestHostProvider);
            MarkdownHelper markdownHelper = new(attributeHelper);

            HtmlString result = markdownHelper.ConvertInlineMarkdown(input);

            Assert.Equal(expected, result.ToString());
        }

        [Fact]
        public void ConvertInlineMarkdown_WhenBoldTextStretchesOverNewLines_ConvertsToStrongTag()
        {
            var input =
            """
                **this is

                bold text
                over multiple

                lines**
            """;

            TestRequestHostProvider requestHostProvider = new("asp.gov.uk");
            AttributeHelper attributeHelper = new(requestHostProvider);
            MarkdownHelper markdownHelper = new(attributeHelper);

            HtmlString result = markdownHelper.ConvertInlineMarkdown(input);

            var output =
            """
                <strong>this is

                bold text
                over multiple

                lines</strong>
            """;
            Assert.Equal(output, result.ToString());
        }

        [Fact]
        public void ConvertInlineMarkdown_WhenItalicTextStretchesOverNewLines_ConvertsToEmTag()
        {
            var input =
            """
                _this is

                bold text
                over multiple

                lines_
            """;

            TestRequestHostProvider requestHostProvider = new("asp.gov.uk");
            AttributeHelper attributeHelper = new(requestHostProvider);
            MarkdownHelper markdownHelper = new(attributeHelper);

            HtmlString result = markdownHelper.ConvertInlineMarkdown(input);

            var output =
            """
                <em>this is

                bold text
                over multiple

                lines</em>
            """;
            Assert.Equal(output, result.ToString());
        }

        [Theory]
        [InlineData(
            "The ASP website is operated by the **Department for Education** ('**DfE**', '**we**' or '**us**'). These terms of use apply to all and authorised users of the ASP website ('**you**').",
            "The ASP website is operated by the <strong>Department for Education</strong> (&#39;<strong>DfE</strong>&#39;, &#39;<strong>we</strong>&#39; or &#39;<strong>us</strong>&#39;). These terms of use apply to all and authorised users of the ASP website (&#39;<strong>you</strong>&#39;)."
        )]
        [InlineData(
            "The ASP website is operated by the *Department for Education* ('*DfE*', '*we*' or '*us*'). These terms of use apply to all and authorised users of the ASP website ('*you*').",
            "The ASP website is operated by the <em>Department for Education</em> (&#39;<em>DfE</em>&#39;, &#39;<em>we</em>&#39; or &#39;<em>us</em>&#39;). These terms of use apply to all and authorised users of the ASP website (&#39;<em>you</em>&#39;)."
        )]
        public void ConvertInlineMarkdown_RegressionTests(string input, string expected)
        {
            TestRequestHostProvider requestHostProvider = new("asp.gov.uk");
            AttributeHelper attributeHelper = new(requestHostProvider);
            MarkdownHelper markdownHelper = new(attributeHelper);

            HtmlString result = markdownHelper.ConvertInlineMarkdown(input);

            Assert.Equal(expected, result.ToString());
        }

        [Theory]
        [InlineData("*", "*")]
        [InlineData("**", "**")]
        [InlineData("***", "***")]
        [InlineData("****", "****")]
        [InlineData("*****", "*****")]
        [InlineData("* *", "* *")]
        [InlineData("** **", "** **")]
        [InlineData("*** ***", "*** ***")]
        [InlineData("**** ****", "**** ****")]
        [InlineData("***** *****", "***** *****")]
        [InlineData("*a*", "<em>a</em>")]
        [InlineData("**a**", "<strong>a</strong>")]
        [InlineData("***a***", "<strong><em>a</em></strong>")]
        [InlineData("****a****", "<strong><strong>a</strong></strong>")]
        [InlineData("*****a*****", "<strong><strong><em>a</em></strong></strong>")]
        public void ConvertInlineMarkdown_MinimumLengths(string input, string expected)
        {
            TestRequestHostProvider requestHostProvider = new("asp.gov.uk");
            AttributeHelper attributeHelper = new(requestHostProvider);
            MarkdownHelper markdownHelper = new(attributeHelper);

            HtmlString result = markdownHelper.ConvertInlineMarkdown(input);

            Assert.Equal(expected, result.ToString());
        }

        [Theory]
        [InlineData("\\_escaped text\\_", "_escaped text_")]
        [InlineData("\\*escaped text\\*", "*escaped text*")]
        [InlineData("\\\\_escaped text\\\\_", @"\_escaped text\_")]
        [InlineData("\\\\*escaped text\\\\*", @"\*escaped text\*")]
        [InlineData("\\\\\\_escaped text\\\\\\_", @"\\_escaped text\\_")]
        [InlineData("\\\\\\*escaped text\\\\\\*", @"\\*escaped text\\*")]
        [InlineData("\\_\\_escaped text\\_\\_", "__escaped text__")]
        [InlineData("\\*\\*escaped text\\*\\*", "**escaped text**")]
        [InlineData("\\\\_\\\\_escaped text\\\\_\\\\_", @"\_\_escaped text\_\_")]
        [InlineData("\\\\*\\\\*escaped text\\\\*\\\\*", @"\*\*escaped text\*\*")]
        public void ConvertInlineMarkdown_WhenTextContainsEscapedCharacters_EscapesCorrectly(string input, string expected)
        {
            TestRequestHostProvider requestHostProvider = new("asp.gov.uk");
            AttributeHelper attributeHelper = new(requestHostProvider);
            MarkdownHelper markdownHelper = new(attributeHelper);

            HtmlString result = markdownHelper.ConvertInlineMarkdown(input);

            Assert.Equal(expected, result.ToString());
        }
    }
}