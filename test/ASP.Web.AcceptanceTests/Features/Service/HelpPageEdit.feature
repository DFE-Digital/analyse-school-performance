Feature: Help page (edit)

@Javascript:disabled
Scenario: Page should not be found if template doesn't exist 
	Given no page content exists
	When I navigate to /help/test/edit
	Then I should get a 404 response

@Javascript:disabled
Scenario: Page should be visible if template exists
	Given page content "help-test" exists:
		"""
		{
		}
		"""
	When I navigate to /help/test/edit
	Then I should get a 200 response
@Javascript:disabled
Scenario: Page should not error if ViewContent is null
	Given page content "help-test" exists:
		"""
		{
			"ViewContent": null
		}
		"""
	When I navigate to /help/test/edit
	Then I should get a 200 response

@Javascript:disabled
Scenario: Page should not error if Views is null
	Given page content "help-test" exists:
		"""
		{
			"Views": null
		}
		"""
	When I navigate to /help/test/edit
	Then I should get a 200 response

@Javascript:disabled
Scenario: Cancel button should link to view page
	Given page content "help-test" exists:
		"""
		{
		}
		"""
	When I navigate to /help/test/edit
	Then I should get a 200 response
	And the element "#app-content-edit-cancel" should have the text content "Cancel"
	And the element "#app-content-edit-cancel" should be an internal link to "/help/test"

@Javascript:disabled
Scenario: Page title should be editable
	Given page content "help-test" exists:
		"""
		{
			"PageTitle": "Test title",
		}
		"""
	When I navigate to /help/test/edit
	Then I should get a 200 response
	And the textbox "#app-content-edit-page-title" should have the value "Test title"

@Javascript:disabled
Scenario: Changing page title should update template
	Given page content "help-test" exists:
		"""
		{
			"PageTitle": "Test title",
		}
		"""
	And I navigate to /help/test/edit
	When I update the textbox "#app-content-edit-page-title" to have the value "Updated title"
	And I click the button "#app-content-edit-save"
	Then I should get a 200 response
	And page content "help-test" property "PageTitle" should be equal to "Updated title"