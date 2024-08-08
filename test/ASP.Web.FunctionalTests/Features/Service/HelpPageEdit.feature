Feature: Help page (edit)


@Javascript:disabled
Scenario: A admin user should be able to access the 'edit' page.
	Given published Content Template "help-test" exists:
	"""
	{
	}
	"""
	And I am a Super Admin user
    When I navigate to /help/test/edit
    Then I should get a 200 response

@Javascript:disabled
Scenario: Page should not be found if Content Template doesn't exist 
	Given no Content Templates exist
	And I am a Super Admin user
	When I navigate to /help/test/edit
	Then I should get a 404 response

@Javascript:disabled
Scenario: Page should not be found if Content Template doesn't exist (even if revision does exist)
	Given published Content Template with id "revision1" and contentId "help-test" exists:
	"""
	{
		"PageTitle": "Test title"
	}
	"""
	And I am a Super Admin user
	When I navigate to /help/test/edit?revision=revision1
	Then I should get a 404 response

@Javascript:disabled
Scenario: Page should not be found if Content Template exists but is unpublished
	Given unpublished Content Template with id "help-test" and contentId "help-test" exists:
	"""
	{
	  "PageTitle": "Test title"
	}
	"""
	And I am a Super Admin user
	When I navigate to /help/test/edit
	Then I should get a 404 response

@Javascript:disabled
Scenario: Page should not be found if revision doesn't exist
	Given published Content Template with id "help-test" and contentId "help-test" exists:
	"""
	{
		"PageTitle": "Test title"
	}
	"""
	And I am a Super Admin user
	When I navigate to /help/test/edit?revision=revision1
	Then I should get a 404 response

@Javascript:disabled
Scenario: Page should be visible if Content Template exists and is published
	Given published Content Template "help-test" exists:
	"""
	{
		"PageTitle": "Test title"
	}
	"""
	And I am a Super Admin user
	When I navigate to /help/test/edit
	Then I should get a 200 response
	And the textbox "[data-testid="content-edit-page-title"]" should have the value "Test title"

@Javascript:disabled
Scenario: Page should be visible if unpublished Content Template exists and revision is specified
	Given unpublished Content Template with id "help-test" and contentId "help-test" exists:
	"""
	{
	  "PageTitle": "Test title"
	}
	"""
	And I am a Super Admin user
	When I navigate to /help/test/edit?revision=help-test
	Then I should get a 200 response
	And the textbox "[data-testid="content-edit-page-title"]" should have the value "Test title"

@Javascript:disabled
Scenario: Page should be visible if revision exists but is unpublished
	Given published Content Template with id "help-test" and contentId "help-test" exists:
	"""
	{
	  "PageTitle": "Test title"
	}
	"""
	Given unpublished Content Template with id "revision1" and contentId "help-test" exists:
	"""
	{
		"PageTitle": "Test title (revised)"
	}
	"""
	And I am a Super Admin user
	When I navigate to /help/test/edit?revision=revision1
	Then I should get a 200 response
	And the textbox "[data-testid="content-edit-page-title"]" should have the value "Test title (revised)"

@Javascript:disabled
Scenario: Page should display published revision
	Given unpublished Content Template with id "help-test" and contentId "help-test" exists:
	"""
	{
	  "PageTitle": "Test title"
	}
	"""
	And published Content Template with id "revision1" and contentId "help-test" exists:
	"""
	{
	  "PageTitle": "Test title (revised)"
	}
	"""
	And I am a Super Admin user
	When I navigate to /help/test/edit
	Then I should get a 200 response
	And the textbox "[data-testid="content-edit-page-title"]" should have the value "Test title (revised)"

@Javascript:disabled
Scenario: Page should not error if ViewContent is null
	Given published Content Template "help-test" exists:
	"""
	{
		"ViewContent": null
	}
	"""
	And I am a Super Admin user
	When I navigate to /help/test/edit
	Then I should get a 200 response

@Javascript:disabled
Scenario: Page should not error if Views is null
	Given published Content Template "help-test" exists:
	"""
	{
		"Views": null
	}
	"""
	And I am a Super Admin user
	When I navigate to /help/test/edit
	Then I should get a 200 response

@Javascript:disabled
Scenario: Cancel button should link to view page
	Given published Content Template "help-test" exists:
	"""
	{
	}
	"""
	And I am a Super Admin user
	When I navigate to /help/test/edit
	Then I should get a 200 response
	And the element "[data-testid="content-edit-cancel"]" should have the text content "Cancel"
	And the element "[data-testid="content-edit-cancel"]" should have the href "/help/test/"

@Javascript:disabled
Scenario: Editing an unpublished Content Template using revision parameter should update Content Template
	Given unpublished Content Template with id "help-test" and contentId "help-test" exists:
	"""
	{
		"PageTitle": "Test title"
	}
	"""
	And I am a Super Admin user
	When I navigate to /help/test/edit?revision=help-test
	And I update the textbox "[data-testid="content-edit-page-title"]" to have the value "Updated test title"
	And I click the button "[data-testid="content-edit-save"]"
	Then I should get a 200 response
	And Content Template with id "help-test" and contentId "help-test" should have property "PageTitle" equal to "Updated test title"

@Javascript:disabled
Scenario: Editing an unpublished revision should update Content Template
	Given published Content Template with id "help-test" and contentId "help-test" exists:
	"""
	{
		"PageTitle": "Test title"
	}
	"""
	And unpublished Content Template with id "revision1" and contentId "help-test" exists:
	"""
	{
		"PageTitle": "Test title (revised)"
	}
	"""
	And I am a Super Admin user
	When I navigate to /help/test/edit?revision=revision1
	And I update the textbox "[data-testid="content-edit-page-title"]" to have the value "Updated test title (revised)"
	And I click the button "[data-testid="content-edit-save"]"
	Then I should get a 200 response
	And Content Template with id "help-test" and contentId "help-test" should have property "PageTitle" equal to "Test title"
	And Content Template with id "revision1" and contentId "help-test" should have property "PageTitle" equal to "Updated test title (revised)"

@Javascript:disabled
Scenario: Editing a published Content Template should create a new revision
	Given published Content Template "help-test" exists:
	"""
	{
		"PageTitle": "Test title",
	}
	"""
	And I am a Super Admin user
	When I navigate to /help/test/edit
	And I remember the value of hidden input "[data-testid="update-revision"]" as <NEW-REVISION-ID>
	And I update the textbox "[data-testid="content-edit-page-title"]" to have the value "Updated test title"
	And I click the button "[data-testid="content-edit-save"]"
	Then I should get a 200 response
	And Content Template with id "help-test" and contentId "help-test" should have property "PageTitle" equal to "Test title"
	And Content Template with id <NEW-REVISION-ID> and contentId "help-test" should have property "PageTitle" equal to "Updated test title"

@Javascript:disabled
Scenario: Editing a published revision should create a new revision
	Given published Content Template with id "help-test" and contentId "help-test" exists:
	"""
	{
		"PageTitle": "Test title"
	}
	"""
	And published Content Template with id "revision1" and contentId "help-test" exists:
	"""
	{
		"PageTitle": "Test title (revised)"
	}
	"""
	And I am a Super Admin user
	When I navigate to /help/test/edit?revision=revision1
	And I remember the value of hidden input "[data-testid="update-revision"]" as <NEW-REVISION-ID>
	And I update the textbox "[data-testid="content-edit-page-title"]" to have the value "Updated test title (revised)"
	And I click the button "[data-testid="content-edit-save"]"
	Then I should get a 200 response
	And Content Template with id "help-test" and contentId "help-test" should have property "PageTitle" equal to "Test title"
	And Content Template with id "revision1" and contentId "help-test" should have property "PageTitle" equal to "Test title (revised)"
	And Content Template with id <NEW-REVISION-ID> and contentId "help-test" should have property "PageTitle" equal to "Updated test title (revised)"

		
@Javascript:disabled
Scenario: A non-admin user should not be able to access the 'edit' page. Instead, they should see a 403 Access Denied page.
        And I am a <Role> user
        When I navigate to /help/test/edit
        Then I should get a 403 response
        And the page title should be "Access denied | Analyse school performance"
        And the element "h1.govuk-heading-l" should have the text content "Access denied"
Examples: 
	| Role            |
	| DfE Named       |
	| DfE Unnamed     |
	| Ofsted          |
	| MAT Named       |
	| MAT Unnamed     |
	| School Named    |
	| School Unnamed  |
	| Diocese Named   |
	| Diocese Unnamed |
	| MAT Governor    |
	| School Governor |