Feature: Help page (edit)

@Javascript:disabled
Scenario: Page should not be found if content template doesn't exist 
	Given no content template exists
	When I navigate to /help/test/edit
	Then I should get a 404 response

@Javascript:disabled
Scenario: Page should not be found if content template doesn't exist (even if revision does exist)
	Given published content template with id "revision1" and contentId "help-test" exists:
	"""
	{
		"PageTitle": "Test title"
	}
	"""
	When I navigate to /help/test/edit?revision=revision1
	Then I should get a 404 response

@Javascript:disabled
Scenario: Page should not be found if content template exists but is unpublished
	Given unpublished content template with id "help-test" and contentId "help-test" exists:
	"""
	{
	  "PageTitle": "Test title"
	}
	"""
	When I navigate to /help/test/edit
	Then I should get a 404 response

@Javascript:disabled
Scenario: Page should not be found if revision doesn't exist
	Given published content template with id "help-test" and contentId "help-test" exists:
	"""
	{
		"PageTitle": "Test title"
	}
	"""
	When I navigate to /help/test/edit?revision=revision1
	Then I should get a 404 response

@Javascript:disabled
Scenario: Page should be visible if content template exists and is published
	Given published content template "help-test" exists:
	"""
	{
		"PageTitle": "Test title"
	}
	"""
	When I navigate to /help/test/edit
	Then I should get a 200 response
	And the textbox "[data-testid="content-edit-page-title"]" should have the value "Test title"

@Javascript:disabled
Scenario: Page should be visible if unpublished content template exists and revision is specified
	Given unpublished content template with id "help-test" and contentId "help-test" exists:
	"""
	{
	  "PageTitle": "Test title"
	}
	"""
	When I navigate to /help/test/edit?revision=help-test
	Then I should get a 200 response
	And the textbox "[data-testid="content-edit-page-title"]" should have the value "Test title"

@Javascript:disabled
Scenario: Page should be visible if revision exists but is unpublished
	Given published content template with id "help-test" and contentId "help-test" exists:
	"""
	{
	  "PageTitle": "Test title"
	}
	"""
	Given unpublished content template with id "revision1" and contentId "help-test" exists:
	"""
	{
		"PageTitle": "Test title (revised)"
	}
	"""
	When I navigate to /help/test/edit?revision=revision1
	Then I should get a 200 response
	And the textbox "[data-testid="content-edit-page-title"]" should have the value "Test title (revised)"

@Javascript:disabled
Scenario: Page should display published revision
	Given unpublished content template with id "help-test" and contentId "help-test" exists:
	"""
	{
	  "PageTitle": "Test title"
	}
	"""
	And published content template with id "revision1" and contentId "help-test" exists:
	"""
	{
	  "PageTitle": "Test title (revised)"
	}
	"""
	When I navigate to /help/test/edit
	Then I should get a 200 response
	And the textbox "[data-testid="content-edit-page-title"]" should have the value "Test title (revised)"

@Javascript:disabled
Scenario: Page should not error if ViewContent is null
	Given published content template "help-test" exists:
	"""
	{
		"ViewContent": null
	}
	"""
	When I navigate to /help/test/edit
	Then I should get a 200 response

@Javascript:disabled
Scenario: Page should not error if Views is null
	Given published content template "help-test" exists:
	"""
	{
		"Views": null
	}
	"""
	When I navigate to /help/test/edit
	Then I should get a 200 response

@Javascript:disabled
Scenario: Cancel button should link to view page
	Given published content template "help-test" exists:
	"""
	{
	}
	"""
	When I navigate to /help/test/edit
	Then I should get a 200 response
	And the element "[data-testid="content-edit-cancel"]" should have the text content "Cancel"
	And the element "[data-testid="content-edit-cancel"]" should have the href "/help/test"

@Javascript:disabled
Scenario: Editing an unpublished content template using revision parameter should update content template
	Given unpublished content template with id "help-test" and contentId "help-test" exists:
	"""
	{
		"PageTitle": "Test title"
	}
	"""
	When I navigate to /help/test/edit?revision=help-test
	And I update the textbox "[data-testid="content-edit-page-title"]" to have the value "Updated test title"
	And I click the button "[data-testid="content-edit-save"]"
	Then I should get a 200 response
	And content template with id "help-test" and contentId "help-test" should have property "PageTitle" equal to "Updated test title"

@Javascript:disabled
Scenario: Editing an unpublished revision should update content template
	Given published content template with id "help-test" and contentId "help-test" exists:
	"""
	{
		"PageTitle": "Test title"
	}
	"""
	And unpublished content template with id "revision1" and contentId "help-test" exists:
	"""
	{
		"PageTitle": "Test title (revised)"
	}
	"""
	When I navigate to /help/test/edit?revision=revision1
	And I update the textbox "[data-testid="content-edit-page-title"]" to have the value "Updated test title (revised)"
	And I click the button "[data-testid="content-edit-save"]"
	Then I should get a 200 response
	And content template with id "help-test" and contentId "help-test" should have property "PageTitle" equal to "Test title"
	And content template with id "revision1" and contentId "help-test" should have property "PageTitle" equal to "Updated test title (revised)"

@Javascript:disabled
Scenario: Editing a published content template should create a new revision
	Given published content template "help-test" exists:
	"""
	{
		"PageTitle": "Test title",
	}
	"""
	When I navigate to /help/test/edit
	And I remember the value of hidden input "[data-testid="update-revision"]" as <NEW-REVISION-ID>
	And I update the textbox "[data-testid="content-edit-page-title"]" to have the value "Updated test title"
	And I click the button "[data-testid="content-edit-save"]"
	Then I should get a 200 response
	And content template with id "help-test" and contentId "help-test" should have property "PageTitle" equal to "Test title"
	And content template with id <NEW-REVISION-ID> and contentId "help-test" should have property "PageTitle" equal to "Updated test title"

@Javascript:disabled
Scenario: Editing a published revision should create a new revision
	Given published content template with id "help-test" and contentId "help-test" exists:
	"""
	{
		"PageTitle": "Test title"
	}
	"""
	And published content template with id "revision1" and contentId "help-test" exists:
	"""
	{
		"PageTitle": "Test title (revised)"
	}
	"""
	When I navigate to /help/test/edit?revision=revision1
	And I remember the value of hidden input "[data-testid="update-revision"]" as <NEW-REVISION-ID>
	And I update the textbox "[data-testid="content-edit-page-title"]" to have the value "Updated test title (revised)"
	And I click the button "[data-testid="content-edit-save"]"
	Then I should get a 200 response
	And content template with id "help-test" and contentId "help-test" should have property "PageTitle" equal to "Test title"
	And content template with id "revision1" and contentId "help-test" should have property "PageTitle" equal to "Test title (revised)"
	And content template with id <NEW-REVISION-ID> and contentId "help-test" should have property "PageTitle" equal to "Updated test title (revised)"