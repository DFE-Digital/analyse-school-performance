Feature: Heading component
The Heading component provides the ability to add h2 and h3 tags to content templates.

For more information please view the [technical specification for this component](https://dev.azure.com/dfe-ssp/s192-Analyse-School-Performance%20%28ASP%29/_wiki/wikis/s192-Analyse-School-Performance-%28ASP%29.wiki/14389/Heading).

@Javascript:disabled
Scenario: When ViewContent property is missing, component should not error
	Given a content template contains the component:
		"""
		{
			"ViewId": "Heading"
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
			"ViewId": "Heading",
			"ViewContent": null
		}
		"""
	When I view the component on the page
	Then there should be no errors
	And the component should exist

#@Javascript:disabled
#Scenario: When ViewContent property is not an object, component should not error and heading type should default to h2
#	Given a content template contains the component:
#		"""
#		{
#			"ViewId": "Heading",
#			"ViewContent": []
#		}
#		"""
#	When I view the component on the page
#	Then there should be no errors
#	And the component should exist

@Javascript:disabled
Scenario: When HeadingType property is missing, component should not error and heading type should default to h2
	Given a content template contains the component:
		"""
		{
			"ViewId": "Heading",
			"ViewContent": {
			}
		}
		"""
	When I view the component on the page
	Then there should be no errors
	And the component should exist
	And the component outer element should have the tag name "h2"

@Javascript:disabled
Scenario Outline: When HeadingType property is not "h2" or "h3", component should not error and default to "h2"
	Given a content template contains the component:
		"""
		{
			"ViewId": "Heading",
			"ViewContent": {
				"HeadingType": <Value>
			}
		}
		"""
	When I view the component on the page
	Then there should be no errors
	And the component should exist
	And the component outer element should have the tag name "h2"
	And the component outer element should have the class "govuk-heading-l"
Examples:
	| Value                  |
	| null                   |
	| 123                    |
	| 1.0                    |
	| true                   |
	| [1,2,3]                |
	| { "property": "value"} |
	| ""                     |
	| " "                    |
	| "h1"                   |
	| "h4"                   |
	| "xxx"                  |

@Javascript:disabled
Scenario Outline: HeadingType property should determine outer element tag type and class
	Given a content template contains the component:
		"""
		{
			"ViewId": "Heading",
			"ViewContent": {
				"HeadingType": <HeadingType>
			}
		}
		"""
	When I view the component on the page
	Then the component outer element should have the tag name "<Tag>"
	And the component outer element should have the class "<Class>"
Examples:
	| HeadingType | Tag | Class           |
	| "h2"        | h2  | govuk-heading-l |
	| "h3"        | h3  | govuk-heading-m |

@Javascript:disabled
Scenario: When Text property is missing, heading text should default to "Heading text"
	Given a content template contains the component:
		"""
		{
			"ViewId": "Heading",
			"ViewContent": {
				"HeadingType": "h2"
			}
		}
		"""
	When I view the component on the page
	Then the component should have the text content "Heading text"

@Javascript:disabled
Scenario Outline: When Text property is invalid, component should not error and should handle the value appropriately
	Given a content template contains the component:
		"""
		{
			"ViewId": "Heading",
			"ViewContent": {
				"HeadingType": "h2",
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
	| null                   | Heading text         |
	| ""                     | Heading text         |
	| " "                    | Heading text         |
	| 123                    | 123                  |
	| 1.0                    | 1.0                  |
	| true                   | true                 |
	| [1,2,3]                | [1,2,3]              |
	| { "property": "value"} | {"property":"value"} |

@Javascript:disabled
Scenario: Heading text should be populated from Text property
	Given a content template contains the component:
		"""
		{
			"ViewId": "Heading",
			"ViewContent": {
				"HeadingType": "h2",
				"Text": "This is a test"
			}
		}
		"""
	When I view the component on the page
	Then the component should have the text content "This is a test"

@Javascript:disabled
Scenario: When Caption property is missing, heading caption should not be displayed
	Given a content template contains the component:
		"""
		{
			"ViewId": "Heading",
			"ViewContent": {
				"HeadingType": "h2",
				"Text": "This is a test"
			}
		}
		"""
	When I view the component on the page
	Then the element "span[class^="govuk-caption-"]" within the component should not exist

@Javascript:disabled
Scenario Outline: When Caption property is invalid, component should not error and should handle the value appropriately
	Given a content template contains the component:
		"""
		{
			"ViewId": "Heading",
			"ViewContent": {
				"Caption": <Value>
			}
		}
		"""
	When I view the component on the page
	Then there should be no errors
	And the component should exist
	And the element "span[class^="govuk-caption-"]" within the component should not exist
Examples:
	| Value                  |
	| null                   |
	| ""                     |
	| " "                    |
	| 123                    |
	| 1.0                    |
	| true                   |
	| [1,2,3]                |
	| { "property": "value"} |

@Javascript:disabled
Scenario: Heading caption should be populated from Caption property
	Given a content template contains the component:
		"""
		{
			"ViewId": "Heading",
			"ViewContent": {
				"HeadingType": "h2",
				"Caption": "Test caption",
				"Text": "This is a test",
			}
		}
		"""
	When I view the component on the page
	Then the component should have the outer HTML:
		"""
		<h2 class="govuk-heading-l">
			<span class="govuk-caption-l">Test caption</span>
			This is a test
		</h2>
		"""

@Javascript:disabled
Scenario Outline: Caption class should be determined by HeadingType property
	Given a content template contains the component:
		"""
		{
			"ViewId": "Heading",
			"ViewContent": {
				"HeadingType": <HeadingType>,
				"Caption": "Test caption"
			}
		}
		"""
	When I view the component on the page
	Then the element "span" within the component should have the class "<Class>"
Examples:
	| HeadingType | Class           |
	| "h2"        | govuk-caption-l |
	| "h3"        | govuk-caption-m |

@Javascript:disabled
Scenario: When LinkUrl property is missing, heading link should not be displayed
	Given a content template contains the component:
		"""
		{
			"ViewId": "Heading",
			"ViewContent": {
				"HeadingType": "h2",
				"Text": "This is a test"
			}
		}
		"""
	When I view the component on the page
	Then the component should have the outer HTML:
		"""
		<h2 class="govuk-heading-l">
			This is a test
		</h2>
		"""

@Javascript:disabled
Scenario Outline: When LinkUrl property is invalid, component should not error and should handle the value appropriately
	Given a content template contains the component:
		"""
		{
			"ViewId": "Heading",
			"ViewContent": {
				"HeadingType": "h2",
				"Text": "This is a test",
				"LinkUrl": <Value>
			}
		}
		"""
	When I view the component on the page
	Then there should be no errors
	And the component should exist
	And the element "a" within the component should not exist
Examples:
	| Value                  |
	| null                   |
	| ""                     |
	| " "                    |
	| 123                    |
	| 1.0                    |
	| true                   |
	| [1,2,3]                |
	| { "property": "value"} |

@Javascript:disabled
Scenario: Heading link should be populated from LinkUrl property
	Given a content template contains the component:
		"""
		{
			"ViewId": "Heading",
			"ViewContent": {
				"HeadingType": "h2",
				"Text": "This is a test",
				"LinkUrl": "http://google.com"
			}
		}
		"""
	When I view the component on the page
	Then the component should have the outer HTML:
		"""
		<h2 class="govuk-heading-l">
			<a href="http://google.com" class="govuk-link" target="_blank">
				This is a test
			</a>
		</h2>
		"""