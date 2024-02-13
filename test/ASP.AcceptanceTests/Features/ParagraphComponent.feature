Feature: Paragraph component

Scenario: Page content should be populated from template
	Given page content "help-test" exists:
		"""
		{
			"Views": [
				{
					"ViewId": "Paragraph",
					"ViewContent": {
						"Id": "test",
						"Text": "Test paragraph text",
					}
				}
			]
		}
		"""
	When I navigate to /help/test
	Then I should get a 200 response
	And the element "#test" should have the text content "Test paragraph text"


Scenario: Paragraph content html should be correct for null text
	Given page content "help-test" exists:
		"""
		{
			"Views": [
				{
					"ViewId": "Paragraph",
					"ViewContent": {
						"Id": "test",
						"Text": null,
					}
				}
			]
		}
		"""
	When I navigate to /help/test
	Then the element "#test" should have the following markup:
		"""
		<p id="test" class="govuk-body"></p>
		"""


Scenario: Paragraph content html should be correct for empty text
	Given page content "help-test" exists:
		"""
		{
			"Views": [
				{
					"ViewId": "Paragraph",
					"ViewContent": {
						"Id": "test",
						"Text": "",
					}
				}
			]
		}
		"""
	When I navigate to /help/test
	Then the element "#test" should have the following markup:
		"""
		<p id="test" class="govuk-body"></p>
		"""


Scenario: paragraph content should escape html
	Given page content "help-test" exists:
		"""
		{
			"Views": [
				{
					"ViewId": "Paragraph",
					"ViewContent": {
						"Id": "test",
						"Text": "<script>alert('test')</script>",
					}
				}
			]
		}
		"""
	When I navigate to /help/test
	Then I should get a 200 response
	And the element "#test" should have the text content "<script>alert('test')</script>"


Scenario: paragraph content should escape embedded html within markdown
	Given page content "help-test" exists:
		"""
		{
			"Views": [
				{
					"ViewId": "Paragraph",
					"ViewContent": {
						"Id": "test",
						"Text": "**bold <script>alert('test')</script>**",
					}
				}
			]
		}
		"""
	When I navigate to /help/test
	Then I should get a 200 response
	And the element "#test" should have the text content "bold <script>alert('test')</script>"



Scenario: Paragraph html should be correct when using bold markdown
	Given page content "help-test" exists:
		"""
		{
			"Views": [
				{
					"ViewId": "Paragraph",
					"ViewContent": {
						"Id": "test",
						"Text": "<input>",
					}
				}
			]
		}
		"""
	When I navigate to /help/test
	Then the element "#test" should have the following markup:
		"""
		<p id="test" class="govuk-body">
			<expected>
		</p>
		"""

Examples:
	| input             | expected                      |
	| Expected **bold** | Expected<strong>bold</strong> |
	| Expected __bold__ | Expected<strong>bold</strong> |


Scenario: Paragraph html should be correct when using italic markdown
	Given page content "help-test" exists:
		"""
		{
			"Views": [
				{
					"ViewId": "Paragraph",
					"ViewContent": {
						"Id": "test",
						"Text": "<input>",
					}
				}
			]
		}
		"""
	When I navigate to /help/test
	Then the element "#test" should have the following markup:
		"""
		<p id="test" class="govuk-body">
			<expected>
		</p>
		"""

Examples:
	| input             | expected                |
	| Expected *italic* | Expected<em>italic</em> |
	| Expected _italic_ | Expected<em>italic</em> |


Scenario: Paragraph html should be correct when using links markdown
	Given page content "help-test" exists:
		"""
		{
			"Views": [
				{
					"ViewId": "Paragraph",
					"ViewContent": {
						"Id": "test",
						"Text": "<input>",
					}
				}
			]
		}
		"""
	When I navigate to /help/test
	Then the element "#test" should have the following markup:
		"""
		<p id="test" class="govuk-body">
			<expected>
		</p>
		"""

Examples:
	| input                                 | expected                                                     |
	| [markdown link](https://google.co.uk) | <a href="https://google.co.uk">markdown link</a>             |
	| **[Google](https://www.google.com)**  | <strong><a href="https://www.google.com">Google</a></strong> |



Scenario: Paragraph html should be correctly escaped
	Given page content "help-test" exists:
		"""
		{
			"Views": [
				{
					"ViewId": "Paragraph",
					"ViewContent": {
						"Id": "test",
						"Text": "<input>",
					}
				}
			]
		}
		"""
	When I navigate to /help/test
	Then the element "#test" should have the following markup:
		"""
		<p id="test" class="govuk-body">
			<expected>
		</p>
		"""

Examples:
	| input                                      | expected                                                           |
	| <script>alert('Hello');</script>           | &lt;script&gt;alert(&#39;Hello&#39;);&lt;/script&gt;               |
	| <div>Some <strong>bold</strong> text</div> | &lt;div&gt;Some &lt;strong&gt;bold&lt;/strong&gt; text&lt;/div&gt; |
	| <a href=\\"https://example.com\\">Link</a> | &lt;a href=&quot;https://example.com&quot;&gt;Link&lt;/a&gt;       |
	| <img src=\\"image.jpg\\" alt=\\"Image\\">  | &lt;img src=&quot;image.jpg&quot; alt=&quot;Image&quot;&gt;        |