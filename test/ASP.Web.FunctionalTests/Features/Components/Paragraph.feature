Feature: Paragraph component

The Paragraph component provides the ability to add paragraphs to content templates.

For more information please view the [technical specification for this component](https://dev.azure.com/dfe-ssp/s192-Analyse-School-Performance%20%28ASP%29/_wiki/wikis/s192-Analyse-School-Performance-%28ASP%29.wiki/14454/Paragraph).

@Javascript:disabled
Scenario: When ViewContent property is missing, component should not error
	Given a content template contains the component:
		"""
		{
			"ViewId": "Paragraph"
		}
		"""
	When I view the component on the page
	Then there should be no errors
	And the component should exist

@Javascript:disabled
Scenario: When ViewContent property is null, component should not error
	Given a content template contains the component:
		"""
		{
			"ViewId": "Paragraph",
			"ViewContent": null
		}
		"""
	When I view the component on the page
	Then there should be no errors
	And the component should exist

@Javascript:disabled
Scenario: When IsLarge property is missing, component should not error and should default to normal size
	Given a content template contains the component:
		"""
		{
			"ViewId": "Paragraph",
			"ViewContent": {
				"Text": "Test paragraph text"
			}
		}
		"""
	When I view the component on the page
	Then there should be no errors
	And the component should exist
	And the component outer element should have the tag name "p"
	And the component outer element should have the class "govuk-body"
	And the component should have the text content "Test paragraph text"

@Javascript:disabled
Scenario Outline: When IsLarge property is invalid, component should not error and should default to normal size
	Given a content template contains the component:
		"""
		{
			"ViewId": "Paragraph",
			"ViewContent": {
				"IsLarge": <Value>,
				"Text": "Test paragraph text"
			}
		}
		"""
	When I view the component on the page
	Then there should be no errors
	And the component should exist
	And the component outer element should have the class "govuk-body"

Examples:
	| Value                  |
	| null                   |
	| ""                     |
	| " "                    |
	| 123                    |
	| 1.0                    |
	| [1,2,3]                |
	| { "property": "value"} |

@Javascript:disabled
Scenario Outline: Paragraph size should be determined from IsLarge property
	Given a content template contains the component:
		"""
		{
			"ViewId": "Paragraph",
			"ViewContent": {
				"IsLarge": <Value>,
				"Text": "Test paragraph text",
			}
		}
		"""
	When I view the component on the page
	Then the component outer element should have the class "<Class>"

Examples:
	| Value   | Class        |
	| true    | govuk-body-l |
	| "true"  | govuk-body-l |
	| false   | govuk-body   |
	| "false" | govuk-body   |

@Javascript:disabled
Scenario: When Text property is missing, component should not error and should handle the value appropriately
	Given a content template contains the component:
		"""
		{
			"ViewId": "Paragraph",
			"ViewContent": {
			}
		}
		"""
	When I view the component on the page
	Then there should be no errors
	And the component should exist
	And the component should have the text content "Paragraph text"

@Javascript:disabled
Scenario Outline: When Text property is invalid, component should not error and should handle the value appropriately
	Given a content template contains the component:
		"""
		{
			"ViewId": "Paragraph",
			"ViewContent": {
				"Text": <Value>
			}
		}
		"""
	When I view the component on the page
	Then there should be no errors
	And the component should exist
	And the component should have the text content "<Text>"
Examples:
	| Value                  | Text                 |
	| null                   | Paragraph text       |
	| ""                     | Paragraph text       |
	| " "                    | Paragraph text       |
	| 123                    | 123                  |
	| 1.0                    | 1.0                  |
	| true                   | true                 |
	| [1,2,3]                | [1,2,3]              |
	| { "property": "value"} | {"property":"value"} |

@Javascript:disabled
Scenario: Paragraph text should be populated from Text property
	Given a content template contains the component:
		"""
		{
			"ViewId": "Paragraph",
			"ViewContent": {
				"Text": "Test paragraph text",
			}
		}
		"""
	When I view the component on the page
	Then the component should have the text content "Test paragraph text"

@Javascript:disabled
Scenario: When Text property contains a new line this should be ignored when generating HTML
	Given a content template contains the component:
		"""
		{
			"ViewId": "Paragraph",
			"ViewContent": {
				"Text": "Test paragraph \n text",
			}
		}
		"""
	When I view the component on the page
	Then the component should have the inner HTML "Test paragraph text"

@Javascript:disabled
Scenario Outline: When Text property contains bold, italic, or link markdown it should be converted to <strong>, <em>, and <a> tags
	Given a content template contains the component:
		"""
		{
			"ViewId": "Paragraph",
			"ViewContent": {
				"Text": "<Input>",
			}
		}
		"""
	When I view the component on the page
	Then the component should have the inner HTML "<Expected>"
Examples:
	| Input                                | Expected                                                                                        |
	| **bold**                             | <strong>bold</strong>                                                                           |
	| __bold__                             | <strong>bold</strong>                                                                           |
	| *italic*                             | <em>italic</em>                                                                                 |
	| _italic_                             | <em>italic</em>                                                                                 |
	| **__double bold__**                  | <strong><strong>double bold</strong></strong>                                                   |
	| __**double bold**__                  | <strong><strong>double bold</strong></strong>                                                   |
	| *_double italic_*                    | <em><em>double italic</em></em>                                                                 |
	| _*double italic*_                    | <em><em>double italic</strong></em>                                                             |
	| **_italic in bold_**                 | <strong><em>italic in bold</em></strong>                                                        |
	| ___italic in bold___                 | <strong><em>italic in bold</em></strong>                                                        |
	| __*italic in bold*__                 | <strong><em>italic in bold</em></strong>                                                        |
	| ***italic in bold***                 | <strong><em>italic in bold</em></strong>                                                        |
	| _**bold in italic**_                 | <em><strong>bold in italic</strong></em>                                                        |
	| *__bold in italic__*                 | <em><strong>bold in italic</strong></em>                                                        |
	| [markdown link](https://google.com)  | <a href="https://google.com" class="govuk-link" target="_blank">markdown link</a>               |
	| **[Google](https://www.google.com)** | <strong><a href="https://www.google.com" class="govuk-link" target="_blank">Google</a></strong> |
	| *[Google](https://www.google.com)*   | <em><a href="https://www.google.com" class="govuk-link" target="_blank">Google</a></em>         |
	| __[Google](https://www.google.com)__ | <strong><a href="https://www.google.com" class="govuk-link" target="_blank">Google</a></strong> |
	| _[Google](https://www.google.com)_   | <em><a href="https://www.google.com" class="govuk-link" target="_blank">Google</a></em>         |
	| [**Google**](https://www.google.com) | <a href="https://www.google.com" class="govuk-link" target="_blank"><strong>Google</strong></a> |
	| [*Google*](https://www.google.com)   | <a href="https://www.google.com" class="govuk-link" target="_blank"><em>Google</em></a>         |
	| [__Google__](https://www.google.com) | <a href="https://www.google.com" class="govuk-link" target="_blank"><strong>Google</strong></a> |
	| [_Google_](https://www.google.com)   | <a href="https://www.google.com" class="govuk-link" target="_blank"><em>Google</em></a>         |

@Javascript:disabled
Scenario Outline: When Text property contains HTML content, HTML content should be escaped on the page
	Given a content template contains the component:
		"""
		{
			"ViewId": "Paragraph",
			"ViewContent": {
				"Text": "<Input>",
			}
		}
		"""
	When I view the component on the page
	Then the component should have the inner HTML "<Expected>"

Examples:
	| Input                                      | Expected                                                           |
	| <script>alert('Hello');</script>           | &lt;script&gt;alert(&#39;Hello&#39;);&lt;/script&gt;               |
	| <div>Some <strong>bold</strong> text</div> | &lt;div&gt;Some &lt;strong&gt;bold&lt;/strong&gt; text&lt;/div&gt; |
	| <a href=\\"https://example.com\\">Link</a> | &lt;a href="https://example.com"&gt;Link&lt;/a&gt;                 |
	| <img src=\\"image.jpg\\" alt=\\"Image\\">  | &lt;img src=&quot;image.jpg&quot; alt=&quot;Image&quot;&gt;        |

@Javascript:disabled
Scenario: When Text property contains HTML content within markdown, HTML content should be escaped on the page
	Given a content template contains the component:
		"""
		{
			"ViewId": "Paragraph",
			"ViewContent": {
				"Text": "**bold <script>alert('test')</script>**",
			}
		}
		"""
	When I view the component on the page
	Then the component should have the inner HTML "<strong>bold &lt;script&gt;alert('test')&lt;/script&gt;</strong>"