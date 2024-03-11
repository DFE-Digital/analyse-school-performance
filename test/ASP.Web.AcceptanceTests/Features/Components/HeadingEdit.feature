Feature: Heading component (edit)

![Heading edit](https://dfe-ssp.visualstudio.com/eb62f5e3-e9f9-48e4-b1ad-1299fcc97149/_apis/git/repositories/64c84fc6-b50c-4733-b588-1cf324205824/Items?path=/.attachments/image-853dafb3-92ed-4f94-95f3-847e6160fee6.png&download=false&resolveLfs=true&%24format=octetStream&api-version=5.0-preview.1&sanitize=true&versionDescriptor.version=wikiMaster)

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

Scenario: When HeadingType property is not "h2" or "h3", component should not error and Heading Type should default to "h2"
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

Scenario: Heading Type field should be populated from the HeadingType property
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
	And the component template property "HeadingType" should be equal to "h3"

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

Scenario: When Caption property is a JSON value, component should not error and should handle the value appropriately
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
	And the component field labelled "Caption" should have the JSON value <TextValue>
Examples:
	| Value                  | TextValue              |
	| 123                    | "123"                  |
	| 1.2                    | "1.2"                  |
	| true                   | "true"                 |
	| [1,2,3]                | "[1,2,3]"              |
	| { "property": "value"} | "{"property":"value"}" |

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
	And the component template property "Caption" should be equal to "Updated caption"

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
	And the component template property "Text" should be equal to "Updated heading"

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

Scenario: When LinkUrl property is a JSON value, component should not error and should handle the value appropriately
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
	And the component field labelled "Link URL" should have the JSON value <TextValue>
Examples:
	| Value                  | TextValue              |
	| 123                    | "123"                  |
	| 1.2                    | "1.2"                  |
	| true                   | "true"                 |
	| [1,2,3]                | "[1,2,3]"              |
	| { "property": "value"} | "{"property":"value"}" |

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
	And the component template property "LinkUrl" should be equal to "https://www.gov.uk"