Feature: Help content

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

Scenario: Cancel button should link to view page
	Given page content "help-test" exists:
		"""
		{
		}
		"""
	When I navigate to /help/test/edit
	Then I should get a 200 response
	And the element "#app-content-edit-cancel" should have the text content "Cancel"
	And the anchor "#app-content-edit-cancel" should be an internal link to "/help/test"

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

Scenario: Changing page title should update template
	Given page content "help-test" exists:
		"""
		{
			"PageTitle": "Test title",
		}
		"""
	And I navigate to /help/test/edit
	When I update the textbox "#app-content-edit-page-title" to have the value "Updated title"
	And I submit the form "#app-content-edit-form"
	Then I should get a 200 response
	And page content "help-test" should have property "PageTitle" set to "Updated title"
