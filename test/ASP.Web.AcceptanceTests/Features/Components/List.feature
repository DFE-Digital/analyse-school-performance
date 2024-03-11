Feature: List component

Scenario: When ViewContent property is missing, component should not error
	Given a content template contains the component:
		"""
		{
			"ViewId": "List",
			"ChildViews": [
				{
					"ViewId": "ListItem",
					"ViewContent": {
						"Text": "List item"
					}
				}
			]
		}
		"""
	When I view the component on the page
	Then there should be no errors
	And the component should exist

Scenario: When ViewContent property is null, component should not error
	Given a content template contains the component:
		"""
		{
			"ViewId": "List",
			"ViewContent": null,
			"ChildViews": [
				{
					"ViewId": "ListItem",
					"ViewContent": {
						"Text": "List item"
					}
				}
			]
		}
		"""
	When I view the component on the page
	Then there should be no errors
	And the component should exist

Scenario: When ChildViews property is missing, component should not error and component should not be displayed
	Given a content template contains the component:
		"""
		{
			"ViewId": "List",
			"ViewContent": {}
		}
		"""
	When I view the component on the page
	Then there should be no errors
	And the component should not exist

Scenario: When ChildViews property is null or empty, component should not error and component should not be displayed
	Given a content template contains the component:
		"""
		{
			"ViewId": "List",
			"ViewContent": {},
			"ChildViews": <Value>
		}
		"""
	When I view the component on the page
	Then there should be no errors
	And the component should not exist
Examples:
	| Value |
	| null  |
	| []    |

Scenario: List content should display html correctly with single list item
	Given a content template contains the component:
		"""
		{
			"ViewId": "List",
			"ChildViews": [
				{
					"ViewId": "ListItem",
					"ViewContent": {
						"Text": "List item"
					}
				}
			]
		}
		"""
	When I view the component on the page
	Then there should be no errors
	And the component should have the following markup:
		"""
		<ul class="govuk-list govuk-list--bullet">
			<li>List item</li>
		</ul>
		"""

Scenario: List content should display html correctly with bold markdown
	Given a content template contains the component:
		"""
		{
			"ViewId": "List",
			"ChildViews": [
				{
					"ViewId": "ListItem",
					"ViewContent": {
						"Text": "<input>"
					}
				}
			]
		}
		"""
	When I view the component on the page
	Then there should be no errors
	And the component should have the following markup:
		"""
		<ul class="govuk-list govuk-list--bullet">
			<li><expected></li>
		</ul>
		"""

Examples:
	| input             | expected                      |
	| Expected **bold** | Expected<strong>bold</strong> |
	| Expected __bold__ | Expected<strong>bold</strong> |


Scenario: List content should display html correctly with italic markdown
	Given a content template contains the component:
		"""
		{
			"ViewId": "List",
			"ChildViews": [
				{
					"ViewId": "ListItem",
					"ViewContent": {
						"Text": "<input>"
					}
				}
			]
		}
		"""
	When I view the component on the page
	Then there should be no errors
	And the component should have the following markup:
		"""
		<ul class="govuk-list govuk-list--bullet">
			<li><expected></li>
		</ul>
		"""

Examples:
	| input             | expected                |
	| Expected *italic* | Expected<em>italic</em> |
	| Expected _italic_ | Expected<em>italic</em> |


Scenario: List content should display html correctly with link markdown
	Given a content template contains the component:
		"""
		{
			"ViewId": "List",
			"ChildViews": [
				{
					"ViewId": "ListItem",
					"ViewContent": {
						"Text": "<input>"
					}
				}
			]
		}
		"""
	When I view the component on the page
	Then there should be no errors
	And the component should have the following markup:
		"""
		<ul class="govuk-list govuk-list--bullet">
			<li><expected></li>
		</ul>
		"""

Examples:
	| input                                 | expected                                                                        |
	| [markdown link](https://google.co.uk) | <a href="https://google.co.uk" class="govuk-link">markdown link</a>             |
	| **[Google](https://www.google.com)**  | <strong><a href="https://www.google.com" class="govuk-link">Google</a></strong> |
	| *[Google](https://www.google.com)*    | <em><a href="https://www.google.com" class="govuk-link">Google</a></em>         |



Scenario: List html should be correctly escaped
	Given a content template contains the component:
		"""
		{
			"ViewId": "List",
			"ChildViews": [
				{
					"ViewId": "ListItem",
					"ViewContent": {
						"Text": "<input>"
					}
				}
			]
		}
		"""
	When I view the component on the page
	Then there should be no errors
	And the component should have the following markup:
		"""
		<ul class="govuk-list govuk-list--bullet">
			<li><expected></li>
		</ul>
		"""
Examples:
	| input                                      | expected                                                           |
	| <script>alert('Hello');</script>           | &lt;script&gt;alert(&#39;Hello&#39;);&lt;/script&gt;               |
	| <div>Some <strong>bold</strong> text</div> | &lt;div&gt;Some &lt;strong&gt;bold&lt;/strong&gt; text&lt;/div&gt; |
	| <a href=\\"https://example.com\\">Link</a> | &lt;a href=&quot;https://example.com&quot;&gt;Link&lt;/a&gt;       |
	| <img src=\\"image.jpg\\" alt=\\"Image\\">  | &lt;img src=&quot;image.jpg&quot; alt=&quot;Image&quot;&gt;        |

Scenario: List content should display html correctly with nested list items
	Given a content template contains the component:
		"""
		{
			"ViewId": "List",
			"ChildViews": [
				{
					"ViewId": "ListItem",
					"ViewContent": {
						"Text": "List item"
					},
					"ChildViews": [
						{
							"ViewId": "List",
							"ChildViews": [
								{
									"ViewId": "ListItem",
									"ViewContent": {
										"Text": "Nested item"
									}
								}
							]
						}
					]
				}
			]
		}
		"""
	When I view the component on the page
	Then there should be no errors
	And the component should have the following markup:
		"""
		<ul class="govuk-list govuk-list--bullet">
			<li>
				List item
				<ul class="govuk-list govuk-list--bullet">
					<li>Nested item</li>
				</ul>
			</li>
		</ul>
		"""