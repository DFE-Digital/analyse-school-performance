Feature: List component


Scenario: List content should display html correctly with list item only
	Given page content "help-test" exists:
		"""
		{
			"Views": [
				{
					"ViewId": "List",
					"ViewContent": {
						"Id": "test"
					},
					"ChildViews": [
						{
							"ViewId": "ListItem",
							"ViewContent": {
								"Text": "List item"
							}
						}
					]
					
				}
			]
		}
		"""
	When I navigate to /help/test
	Then I should get a 200 response
	Then the element "#test" should have the following markup:
		"""
		<ul id="test" class="govuk-list govuk-list--bullet">
			<li>List item</li>
		</ul>
		"""



Scenario: List content should display html correctly with bold markdown
	Given page content "help-test" exists:
		"""
		{
			"Views": [
				{
					"ViewId": "List",
					"ViewContent": {
						"Id": "test"
					},
					"ChildViews": [
						{
							"ViewId": "ListItem",
							"ViewContent": {
								"Text": "<input>"
							}
						}
					]
					
				}
			]
		}
		"""
	When I navigate to /help/test
	Then I should get a 200 response
	Then the element "#test" should have the following markup:
		"""
		<ul id="test" class="govuk-list govuk-list--bullet">
			<li><expected></li>
		</ul>
		"""

Examples:
	| input             | expected                      |
	| Expected **bold** | Expected<strong>bold</strong> |
	| Expected __bold__ | Expected<strong>bold</strong> |


Scenario: List content should display html correctly with italic markdown
	Given page content "help-test" exists:
		"""
		{
			"Views": [
				{
					"ViewId": "List",
					"ViewContent": {
						"Id": "test"
					},
					"ChildViews": [
						{
							"ViewId": "ListItem",
							"ViewContent": {
								"Text": "<input>"
							}
						}
					]
					
				}
			]
		}
		"""
	When I navigate to /help/test
	Then I should get a 200 response
	Then the element "#test" should have the following markup:
		"""
		<ul id="test" class="govuk-list govuk-list--bullet">
			<li><expected></li>
		</ul>
		"""

Examples:
	| input             | expected                |
	| Expected *italic* | Expected<em>italic</em> |
	| Expected _italic_ | Expected<em>italic</em> |


Scenario: List content should display html correctly with link markdown
	Given page content "help-test" exists:
		"""
		{
			"Views": [
				{
					"ViewId": "List",
					"ViewContent": {
						"Id": "test"
					},
					"ChildViews": [
						{
							"ViewId": "ListItem",
							"ViewContent": {
								"Text": "<input>"
							}
						}
					]
					
				}
			]
		}
		"""
	When I navigate to /help/test
	Then I should get a 200 response
	Then the element "#test" should have the following markup:
		"""
		<ul id="test" class="govuk-list govuk-list--bullet">
			<li><expected></li>
		</ul>
		"""

Examples:
	| input                                 | expected                                                     |
	| [markdown link](https://google.co.uk) | <a href="https://google.co.uk">markdown link</a>             |
	| **[Google](https://www.google.com)**  | <strong><a href="https://www.google.com">Google</a></strong> |
	| *[Google](https://www.google.com)*    | <em><a href="https://www.google.com">Google</a></em>         |



Scenario: List html should be correctly escaped
	Given page content "help-test" exists:
		"""
		{
			"Views": [
				{
					"ViewId": "List",
					"ViewContent": {
						"Id": "test"
					},
					"ChildViews": [
						{
							"ViewId": "ListItem",
							"ViewContent": {
								"Text": "<input>"
							}
						}
					]
					
				}
			]
		}
		"""
	When I navigate to /help/test
	Then I should get a 200 response
	Then the element "#test" should have the following markup:
		"""
		<ul id="test" class="govuk-list govuk-list--bullet">
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
	Given page content "help-test" exists:
		"""
		{
			"Views": [
				{
					"ViewId": "List",
					"ViewContent": {
						"Id": "test"
					},
					"ChildViews": [
						{
							"ViewId": "ListItem",
							"ViewContent": {
								"Text": "List item"
							},
							"ChildViews": [
							{
								"ViewId": "ListItem",
								"ViewContent": {
									"Text": "Nested item"
									}
								}
							]
						},
					]
				}
			]
		}
		"""
	When I navigate to /help/test
	Then I should get a 200 response
	Then the element "#test" should have the following markup:
		"""
		<ul id="test" class="govuk-list govuk-list--bullet">
			<li>
				List item
				<ul>
					<li>Nested item</li>
				</ul>
			</li>
		</ul>
		"""


Scenario: List content should not display html with empty list
	Given page content "help-test" exists:
		"""
		{
			"Views": [
				{
					"ViewId": "List",
					"ViewContent": {
						"Id": "test"
					},
					"ChildViews": [
						
					]
					
				}
			]
		}
		"""
	When I navigate to /help/test
	Then I should get a 200 response
	Then the element "#test" should not exist