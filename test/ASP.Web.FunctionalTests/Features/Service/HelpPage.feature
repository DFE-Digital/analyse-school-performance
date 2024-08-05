Feature: Help page

@Javascript:disabled
Scenario: Help should not have a top-level page 
	When I navigate to /help
	Then I should get a 404 response

@Javascript:disabled
Scenario: Page should not be found if Content Template doesn't exist 
	Given no Content Templates exist
	When I navigate to /help/test
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
	When I navigate to /help/test
	Then I should get a 404 response

@Javascript:disabled
Scenario: Page should not be found if revision doesn't exist
	Given published Content Template with id "help-test" and contentId "help-test" exists:
	"""
	{
	  "PageTitle": "Test title"
	}
	"""
	When I navigate to /help/test?revision=revision1
	Then I should get a 404 response

@Javascript:disabled
Scenario: Page should be visible if Content Template exists and is published
	Given published Content Template "help-test" exists:
	"""
	{
		"PageTitle": "Test title"
	}
	"""
	When I navigate to /help/test
	Then I should get a 200 response
	And the element "[data-testid="content-page-title"]" should have the text content "Test title"

@Javascript:disabled
Scenario: Page should be visible if unpublished Content Template exists and revision is specified
	Given unpublished Content Template with id "help-test" and contentId "help-test" exists:
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
	When I navigate to /help/test?revision=revision1
	Then I should get a 200 response
	And the element "[data-testid="content-page-title"]" should have the text content "Test title (revised)"

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
	When I navigate to /help/test
	Then I should get a 200 response
	And the element "[data-testid="content-page-title"]" should have the text content "Test title (revised)"

@Javascript:disabled
Scenario: Page should not error if ViewContent is null
	Given published Content Template "help-test" exists:
	"""
	{
		"ViewContent": null
	}
	"""
	When I navigate to /help/test
	Then I should get a 200 response

@Javascript:disabled
Scenario: Page should not error if Views is null
	Given published Content Template "help-test" exists:
	"""
	{
		"Views": null
	}
	"""
	When I navigate to /help/test
	Then I should get a 200 response

@Javascript:disabled
Scenario: Page should show a breadcrumb trail
	Given published Content Template "help-test" exists:
	"""
	{   
		"PageTitle" : "Current page", 
		"Views": null
	}
	"""
	When I navigate to /help/test
	Then I should get a 200 response
	And the element "[data-testid="breadcrumb-home"]" should have the href "/"
    And the element "[data-testid="breadcrumb-current-page"]" should have the text content "Current page"


@Javascript:disabled
Scenario: Edit button should show when user has admin role
	Given published Content Template "help-test" exists:
	"""
	{
	}
	"""
	And I am a <Role> user
	When I navigate to /help/test
	Then I should get a 200 response
	And the element "[data-testid="content-edit"]" should have the text content "Edit this page"
	And the element "[data-testid="content-edit"]" should have the href "/help/test/edit"
Examples: 
	| Role        |
	| Super Admin |


@Javascript:disabled
Scenario: Edit button should not show when user does not have admin role
	Given published Content Template "help-test" exists:
	"""
	{
	}
	"""
	And I am a <Role> user
    When I navigate to /
	Then the element "[data-testid="content-edit"]" should not exist
Examples: 
	| Role            |
	| DfE Named       |
	| DfE Unnamed     |
	| Super Admin     |
	| Ofsted          |
	| MAT Named       |
	| MAT Unnamed     |
	| School Named    |
	| School Unnamed  |
	| Diocese Named   |
	| Diocese Unnamed |
	| MAT Governor    |
	| School Governor |

