Feature: Heading component (edit)
The Heading component provides the ability to add h2 and h3 tags to content templates.

For more information please view the [technical specification for this component](https://dev.azure.com/dfe-ssp/s192-Analyse-School-Performance%20%28ASP%29/_wiki/wikis/s192-Analyse-School-Performance-%28ASP%29.wiki/14389/Heading?anchor=editing).

@Javascript:disabled
Scenario: When ViewContent property is missing, component should not error
	Given a content template contains the component:
		"""
		{
			"ViewId": "Heading"
		}
		"""
	When I edit the component on the page
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
	When I edit the component on the page
	Then there should be no errors
	And the component should exist

@Javascript:disabled
Scenario: When HeadingType property is missing, component should not error and Heading Type should default to h2
	Given a content template contains the component:
		"""
		{
			"ViewId": "Heading",
			"ViewContent": {
			}
		}
		"""
	When I edit the component on the page
	Then there should be no errors
	And the component should exist
	And the component field labelled "Heading Type" should have the value "h2"

@Javascript:disabled
Scenario Outline: When HeadingType property is not "h2" or "h3", component should not error and Heading Type should default to "h2"
	Given a content template contains the component:
		"""
		{
			"ViewId": "Heading",
			"ViewContent": {
				"HeadingType": <Value>
			}
		}
		"""
	When I edit the component on the page
	Then there should be no errors
	And the component should exist
	And the component field labelled "Heading Type" should have the value "h2"
Examples:
	| Value                  |
	| null                   |
	| 123                    |
	| 1.2                    |
	| true                   |
	| [1,2,3]                |
	| { "property": "value"} |
	| ""                     |
	| " "                    |
	| "h1"                   |
	| "h4"                   |
	| "xxx"                  |

@Javascript:disabled
Scenario Outline: Heading Type field should be populated from the HeadingType property
	Given a content template contains the component:
		"""
		{
			"ViewId": "Heading",
			"ViewContent": {
				"HeadingType": <HeadingType>
			}
		}
		"""
	When I edit the component on the page
	Then the component field labelled "Heading Type" should have the value <HeadingType>
Examples:
	| HeadingType |
	| "h2"        |
	| "h3"        |

@Javascript:disabled
Scenario: Updating Heading Type field should update HeadingType property on template
	Given a content template contains the component:
		"""
		{
			"ViewId": "Heading",
			"ViewContent": {
				"HeadingType": "h2"
			}
		}
		"""
	And I edit the component on the page
	When I update the component field labelled "Heading Type" to have the value "h3"
	And I save the component
	Then I should get a 200 response
	And the component template should have property "HeadingType" equal to "h3"

@Javascript:disabled
Scenario: When Caption property is null, component should not error and should handle the value appropriately
	Given a content template contains the component:
		"""
		{
			"ViewId": "Heading",
			"ViewContent": {
				"Caption": null
			}
		}
		"""
	When I edit the component on the page
	Then there should be no errors
	And the component should exist
	And the component field labelled "Caption" should have the value ""

@Javascript:disabled
Scenario Outline: When Caption property is a JSON value, component should not error and should handle the value appropriately
	Given a content template contains the component:
		"""
		{
			"ViewId": "Heading",
			"ViewContent": {
				"Caption": <Value>
			}
		}
		"""
	When I edit the component on the page
	Then there should be no errors
	And the component should exist
	And the component field labelled "Caption" should match the JSON string <TextValue>
Examples:
	| Value                  | TextValue              |
	| 123                    | "123"                  |
	| 1.2                    | "1.2"                  |
	| true                   | "true"                 |
	| [1,2,3]                | "[1,2,3]"              |
	| { "property": "value"} | "{"property":"value"}" |

@Javascript:disabled
Scenario: Caption field should be populated from Caption property
	Given a content template contains the component:
		"""
		{
			"ViewId": "Heading",
			"ViewContent": {
				"Caption": "Test caption"
			}
		}
		"""
	When I edit the component on the page
	Then the component field labelled "Caption" should have the value "Test caption"

@Javascript:disabled
Scenario: Updating Caption field should update Caption property on template
	Given a content template contains the component:
		"""
		{
			"ViewId": "Heading",
			"ViewContent": {
				"Caption": "Test caption"
			}
		}
		"""
	And I edit the component on the page
	When I update the component field labelled "Caption" to have the value "Updated caption"
	And I save the component
	Then I should get a 200 response
	And the component template should have property "Caption" equal to "Updated caption"

@Javascript:disabled
Scenario: When Text property is null, component should not error and should handle the value appropriately
	Given a content template contains the component:
		"""
		{
			"ViewId": "Heading",
			"ViewContent": {
				"Text": null
			}
		}
		"""
	When I edit the component on the page
	Then there should be no errors
	And the component should exist
	And the component field labelled "Text" should have the value ""

@Javascript:disabled
Scenario Outline: When Text property is a JSON value, component should not error and should handle the value appropriately
	Given a content template contains the component:
		"""
		{
			"ViewId": "Heading",
			"ViewContent": {
				"Text": <Value>
			}
		}
		"""
	When I edit the component on the page
	Then there should be no errors
	And the component should exist
	And the component field labelled "Text" should match the JSON string <TextValue>
Examples:
	| Value                  | TextValue              |
	| 123                    | "123"                  |
	| 1.2                    | "1.2"                  |
	| true                   | "true"                 |
	| [1,2,3]                | "[1,2,3]"              |
	| { "property": "value"} | "{"property":"value"}" |

@Javascript:disabled
Scenario: Text field should be populated from Text property
	Given a content template contains the component:
		"""
		{
			"ViewId": "Heading",
			"ViewContent": {
				"Text": "Test heading text"
			}
		}
		"""
	When I edit the component on the page
	Then the component field labelled "Text" should have the value "Test heading text"

@Javascript:disabled
Scenario: Updating Text field should update Text property on template
	Given a content template contains the component:
		"""
		{
			"ViewId": "Heading",
			"ViewContent": {
				"Text": "Test heading text"
			}
		}
		"""
	And I edit the component on the page
	When I update the component field labelled "Text" to have the value "Updated heading"
	And I save the component
	Then I should get a 200 response
	And the component template should have property "Text" equal to "Updated heading"

@Javascript:disabled
Scenario: When LinkUrl property is null, component should not error and should handle the value appropriately
	Given a content template contains the component:
		"""
		{
			"ViewId": "Heading",
			"ViewContent": {
				"LinkUrl": null
			}
		}
		"""
	When I edit the component on the page
	Then there should be no errors
	And the component should exist
	And the component field labelled "Link URL" should have the value ""

@Javascript:disabled
Scenario Outline: When LinkUrl property is a JSON value, component should not error and should handle the value appropriately
	Given a content template contains the component:
		"""
		{
			"ViewId": "Heading",
			"ViewContent": {
				"LinkUrl": <Value>
			}
		}
		"""
	When I edit the component on the page
	Then there should be no errors
	And the component should exist
	And the component field labelled "Link URL" should match the JSON string <TextValue>
Examples:
	| Value                  | TextValue              |
	| 123                    | "123"                  |
	| 1.2                    | "1.2"                  |
	| true                   | "true"                 |
	| [1,2,3]                | "[1,2,3]"              |
	| { "property": "value"} | "{"property":"value"}" |

@Javascript:disabled
Scenario: Link URL field should be populated from LinkUrl property
	Given a content template contains the component:
		"""
		{
			"ViewId": "Heading",
			"ViewContent": {
				"LinkUrl": "https://google.com"
			}
		}
		"""
	When I edit the component on the page
	Then the component field labelled "Link URL" should have the value "https://google.com"

@Javascript:disabled
Scenario: Updating Link URL field should update LinkUrl property on template
	Given a content template contains the component:
		"""
		{
			"ViewId": "Heading",
			"ViewContent": {
				"Caption": "https://google.com"
			}
		}
		"""
	And I edit the component on the page
	When I update the component field labelled "Link URL" to have the value "https://www.gov.uk"
	And I save the component
	Then I should get a 200 response
	And the component template should have property "LinkUrl" equal to "https://www.gov.uk"