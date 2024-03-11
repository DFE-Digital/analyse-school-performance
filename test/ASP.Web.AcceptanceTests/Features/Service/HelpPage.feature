Feature: Help page

Scenario: Help should not have a top-level page 
	When I navigate to /help
	Then I should get a 404 response

Scenario: Page should not be found if template doesn't exist 
	Given no page content exists
	When I navigate to /help/test
	Then I should get a 404 response

Scenario: Page should be visible if template exists
	Given page content "help-test" exists:
		"""
		{
		}
		"""
	When I navigate to /help/test
	Then I should get a 200 response

Scenario: Page should not error if ViewContent is null
	Given page content "help-test" exists:
		"""
		{
			"ViewContent": null
		}
		"""
	When I navigate to /help/test
	Then I should get a 200 response

Scenario: Page should not error if Views is null
	Given page content "help-test" exists:
		"""
		{
			"Views": null
		}
		"""
	When I navigate to /help/test
	Then I should get a 200 response

Scenario: Edit button should link to edit page
	Given page content "help-test" exists:
		"""
		{
		}
		"""
	When I navigate to /help/test
	Then I should get a 200 response
	And the element "#app-content-edit" should have the text content "Edit this page"
	And the anchor "#app-content-edit" should be an internal link to "/help/test/edit"