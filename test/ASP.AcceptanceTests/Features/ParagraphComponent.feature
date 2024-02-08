Feature: Paragraph component

Scenario: Page content should be populated from template
	Given page content "help-test" exists:
		"""
		{
			"Views": [
				{
					"ViewId": "Paragraph_Body",
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