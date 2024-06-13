Feature: Help page

@Javascript:disabled
Scenario: Help should not have a top-level page 
	When I navigate to /help
	Then I should get a 404 response

@Javascript:disabled
Scenario: Page should not be found if content template doesn't exist 
	Given no content template exists
	When I navigate to /help/test
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
	When I navigate to /help/test
	Then I should get a 404 response

@Javascript:disabled
Scenario: Page should not be found if revision doesn't exist
	Given published content template with id "help-test" and contentId "help-test" exists:
	"""
	{
	  "PageTitle": "Test title"
	}
	"""
	When I navigate to /help/test?revision=revision1
	Then I should get a 404 response

@Javascript:disabled
Scenario: Page should be visible if content template exists and is published
	Given published content template "help-test" exists:
	"""
	{
		"PageTitle": "Test title"
	}
	"""
	When I navigate to /help/test
	Then I should get a 200 response
	And the element "[data-testid="content-page-title"]" should have the text content "Test title"

@Javascript:disabled
Scenario: Page should be visible if unpublished content template exists and revision is specified
	Given unpublished content template with id "help-test" and contentId "help-test" exists:
	"""
	{
	  "PageTitle": "Test title"
	}
	"""
	When I navigate to /help/test?revision=help-test
	Then I should get a 200 response
	And the element "[data-testid="content-page-title"]" should have the text content "Test title"

@Javascript:disabled
Scenario: Page should be visible if revision exists but is unpublished
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
	When I navigate to /help/test?revision=revision1
	Then I should get a 200 response
	And the element "[data-testid="content-page-title"]" should have the text content "Test title (revised)"

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
	When I navigate to /help/test
	Then I should get a 200 response
	And the element "[data-testid="content-page-title"]" should have the text content "Test title (revised)"

@Javascript:disabled
Scenario: Page should not error if ViewContent is null
	Given published content template "help-test" exists:
	"""
	{
		"ViewContent": null
	}
	"""
	When I navigate to /help/test
	Then I should get a 200 response

@Javascript:disabled
Scenario: Page should not error if Views is null
	Given published content template "help-test" exists:
	"""
	{
		"Views": null
	}
	"""
	When I navigate to /help/test
	Then I should get a 200 response

@Javascript:disabled
Scenario: Page should show a breadcrumb trail
	Given published content template "help-test" exists:
	"""
	{   
		"PageTitle" : "Current page", 
		"Views": null
	}
	"""
	When I navigate to /help/test
	Then I should get a 200 response
	And the element "#app-breadcrumb-home" should have the href "/"
    And the element "#app-breadcrumb-current-page" should have the text content "Current page"

@Javascript:disabled
Scenario: Edit button should link to edit page
	Given published content template "help-test" exists:
	"""
	{
	}
	"""
	When I navigate to /help/test
	Then I should get a 200 response
	And the element "[data-testid="content-edit"]" should have the text content "Edit this page"
	And the element "[data-testid="content-edit"]" should have the href "/help/test/edit"