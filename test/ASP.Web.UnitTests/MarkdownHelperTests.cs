using ASP.Web.Helpers;
using Microsoft.AspNetCore.Html;
using System;

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
        [InlineData("* text *", "* text *")]
        [InlineData("_ text _", "_ text _")]
        public void ConvertInlineMarkdown_WhenSpaceBetweenTextAndItalicMarker_DoesNotWrapInEmTag(string input, string expected)
        {
            // Act
            HtmlString result = MarkdownHelper.ConvertInlineMarkdown(input);

            // Assert
            Assert.Equal(expected, result.ToString());
        }

        [Theory]
        [InlineData("** text **", "** text **")]
        [InlineData("__ text __", "__ text __")]
        public void ConvertInlineMarkdown_WhenSpaceBetweenTextAndBoldMarker_DoesNotWrapInStrongTag(string input, string expected)
        {
            // Act
            HtmlString result = MarkdownHelper.ConvertInlineMarkdown(input);

            // Assert
            Assert.Equal(expected, result.ToString());
        }


        [Theory]
        [InlineData("**this is bold* text**", "<strong>this is bold* text</strong>")]
        [InlineData("**this is bold_ text**", "<strong>this is bold_ text</strong>")]
        [InlineData("*this is italic* text*", "<em>this is italic* text</em>")]
        [InlineData("*this is italic_ text*", "<em>this is italic_ text</em>")]
        [InlineData("__this is bold* text__", "<strong>this is bold* text</strong>")]
        [InlineData("__this is bold_ text__", "<strong>this is bold_ text</strong>")]
        [InlineData("_this is italic* text_", "<em>this is italic* text</em>")]
        [InlineData("_this is italic_ text_", "<em>this is italic_ text</em>")]
        [InlineData("**this is bold** text**", "<strong>this is bold** text</strong>")]
        [InlineData("**this is bold__ text**", "<strong>this is bold__ text</strong>")]
        [InlineData("*this is italic** text*", "<em>this is italic** text</em>")]
        [InlineData("*this is italic__ text*", "<em>this is italic__ text</em>")]
        [InlineData("__this is bold** text__", "<strong>this is bold** text</strong>")]
        [InlineData("__this is bold__ text__", "<strong>this is bold__ text</strong>")]
        [InlineData("_this is italic** text_", "<em>this is italic** text</em>")]
        [InlineData("_this is italic__ text_", "<em>this is italic__ text</em>")]
        public void ConvertInlineMarkdown_WhenUnbalancedMarker_DoesNotWrap(string input, string expected)
        {
            // Act
            HtmlString result = MarkdownHelper.ConvertInlineMarkdown(input);

            // Assert
            Assert.Equal(expected, result.ToString());
        }

        [Theory]
        [InlineData("[Google](https://www.google.com)", "<a href=\"https://www.google.com\" class=\"govuk-link\">Google</a>")]
        [InlineData("**[Google](https://www.google.com)**", "<strong><a href=\"https://www.google.com\" class=\"govuk-link\">Google</a></strong>")]
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