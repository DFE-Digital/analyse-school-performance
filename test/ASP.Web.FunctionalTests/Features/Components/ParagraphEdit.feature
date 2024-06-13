Feature: Paragraph component (edit)

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
	When I edit the component on the page
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
	When I edit the component on the page
	Then there should be no errors
	And the component should exist

@Javascript:disabled
Scenario: When IsLarge property is missing, component should not error and Is Large should default to false
	Given a content template contains the component:
		"""
		{
			"ViewId": "Paragraph",
			"ViewContent": {
			}
		}
		"""
	When I edit the component on the page
	Then there should be no errors
	And the component should exist
	And the component field labelled "Is Large" should be unchecked

@Javascript:disabled
Scenario Outline: When IsLarge property is invalid, component should not error and Is Large should default to false
	Given a content template contains the component:
		"""
		{
			"ViewId": "Paragraph",
			"ViewContent": {
				"IsLarge": <Value>
			}
		}
		"""
	When I edit the component on the page
	Then there should be no errors
	And the component should exist
	And the component field labelled "Is Large" should be unchecked
Examples:
	| Value                  |
	| null                   |
	| 123                    |
	| 1.2                    |
	| [1,2,3]                |
	| { "property": "value"} |
	| ""                     |
	| " "                    |
	| "xxx"                  |

@Javascript:disabled
Scenario Outline: Is Large field should be populated from the IsLarge property
	Given a content template contains the component:
		"""
		{
			"ViewId": "Paragraph",
			"ViewContent": {
				"IsLarge": <IsLarge>
			}
		}
		"""
	When I edit the component on the page
	Then the component field labelled "Is Large" should be <State>
Examples:
	| IsLarge | State     |
	| true    | checked   |
	| false   | unchecked |
	| "true"  | checked   |
	| "false" | unchecked |

@Javascript:disabled
Scenario: Updating Is Large field should update IsLarge property on template
	Given a content template contains the component:
		"""
		{
			"ViewId": "Paragraph",
			"ViewContent": {
				"IsLarge": false
			}
		}
		"""
	And I edit the component on the page
	When I update the component field labelled "Is Large" to be checked
	And I save the component
	Then I should get a 200 response
	And the component template should have property "IsLarge" equal to true

@Javascript:disabled
Scenario: When Text property is null, component should not error and should handle the value appropriately
	Given a content template contains the component:
		"""
		{
			"ViewId": "Paragraph",
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
			"ViewId": "Paragraph",
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
			"ViewId": "Paragraph",
			"ViewContent": {
				"Text": "Test paragraph text"
			}
		}
		"""
	When I edit the component on the page
	Then the component field labelled "Text" should have the value "Test paragraph text"

@Javascript:disabled
Scenario: Updating Text field should update Text property on template
	Given a content template contains the component:
		"""
		{
			"ViewId": "Paragraph",
			"ViewContent": {
				"Text": "Test paragraph text"
			}
		}
		"""
	And I edit the component on the page
	When I update the component field labelled "Text" to have the value "Updated paragraph"
	And I save the component
	Then I should get a 200 response
	And the component template should have property "Text" equal to "Updated paragraph"

@Javascript:disabled
Scenario: Updating Text field with a new line should update Text property on template
	Given a content template contains the component:
		"""
		{
			"ViewId": "Paragraph",
			"ViewContent": {
				"Text": "Test paragraph text"
			}
		}
		"""
	And I edit the component on the page
	When I update the component field labelled "Text" to have the value:
	"""
	Updated
	paragraph
	"""
	And I save the component
	Then I should get a 200 response
	And the component template should have property "Text" equal to:
	"""
	"Updated
	paragraph"
	"""