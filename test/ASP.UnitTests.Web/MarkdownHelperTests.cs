using ASP.Web.Helpers;
using Microsoft.AspNetCore.Html;

namespace ASP.UnitTest.Web
{
    public class MarkdownHelperTests
    {
        [Fact]
        public void ConvertInlineMarkdown_WhenEmpty_ReturnsEmptyString()
        {
            // Arrange
            string input = "";

            // Act
            HtmlString result = MarkdownHelper.ConvertInlineMarkdown(input);

            // Assert
            Assert.Equal("", result.ToString());
        }

        [Fact]
        public void ConvertInlineMarkdown_WhenNull_ReturnsEmptyString()
        {
            // Arrange
            string? input = null;

            // Act
            HtmlString result = MarkdownHelper.ConvertInlineMarkdown(input);

            // Assert
            Assert.Equal("", result.ToString());
        }


        [Theory]
        [InlineData("**bold**", "<strong>bold</strong>")]
        [InlineData("__bold__", "<strong>bold</strong>")]
        public void ConvertInlineMarkdown_WhenBoldText_ReturnsStrongTag(string input, string expected)
        {
            // Act
            HtmlString result = MarkdownHelper.ConvertInlineMarkdown(input);

            // Assert
            Assert.Equal(expected, result.ToString());
        }

        [Theory]
        [InlineData("*italic*", "<em>italic</em>")]
        [InlineData("_italic_", "<em>italic</em>")]
        public void ConvertInlineMarkdown_WhenItalicText_ReturnsEmTag(string input, string expected)
        {
            // Act
            HtmlString result = MarkdownHelper.ConvertInlineMarkdown(input);

            // Assert
            Assert.Equal(expected, result.ToString());
        }

        [Theory]
        [InlineData("[Google](https://www.google.com)", "<a href=\"https://www.google.com\">Google</a>")]
        [InlineData("**[Google](https://www.google.com)**", "<strong><a href=\"https://www.google.com\">Google</a></strong>")]
        public void ConvertInlineMarkdown_WhenLink_ReturnsAnchorTag(string input, string expected)
        {
            // Act
            HtmlString result = MarkdownHelper.ConvertInlineMarkdown(input);

            // Assert
            Assert.Equal(expected, result.ToString());
        }

        [Theory]
        [InlineData("**_bold italic_**", "<strong><em>bold italic</em></strong>")]
        [InlineData("___bold italic___", "<strong><em>bold italic</em></strong>")]
        [InlineData("_**italic bold**_", "<em><strong>italic bold</strong></em>")]
        public void ConvertInlineMarkdown_WhenBoldOrItalicText_ReturnsCorrectHtml(string input, string expected)
        {
            // Act
            HtmlString result = MarkdownHelper.ConvertInlineMarkdown(input);

            // Assert
            Assert.Equal(expected, result.ToString());
        }


        [Theory]
        [InlineData("<script>alert('Hello');</script>", "&lt;script&gt;alert(&#39;Hello&#39;);&lt;/script&gt;")]
        [InlineData("<div>Some <strong>bold</strong> text</div>", "&lt;div&gt;Some &lt;strong&gt;bold&lt;/strong&gt; text&lt;/div&gt;")]
        [InlineData("<a href=\"https://example.com\">Link</a>", "&lt;a href=&quot;https://example.com&quot;&gt;Link&lt;/a&gt;")]
        [InlineData("<img src=\"image.jpg\" alt=\"Image\">", "&lt;img src=&quot;image.jpg&quot; alt=&quot;Image&quot;&gt;")]
        public void ConvertInlineMarkdown_WhenInputContainsHtmlCharacters_EscapesHtml(string input, string expected)
        {
            // Act
            HtmlString result = MarkdownHelper.ConvertInlineMarkdown(input);

            // Assert
            Assert.Equal(expected, result.ToString());
        }
    }
}