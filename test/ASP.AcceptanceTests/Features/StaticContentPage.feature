Feature: Static content page

Scenario: Page title
	Given page content "help-test" exists:
		"""
		{
			"PageTitle": "Test",
		}
		"""
	When I navigate to /help/test
	Then I should get a 200 response
	And the page title should be "Test | Analyse school performance"

Scenario: Page content
	Given page content "help-test" exists:
		"""
		{
			"Views": [
				{
					"ViewId": "Heading",
					"ViewContent": {
						"Id": "test",
						"Text": "Test heading text",
					}
				}
			]
		}
		"""
	When I navigate to /help/test
	Then I should get a 200 response
	And the HTML element with selector "#test" should have the text content "Test heading text"