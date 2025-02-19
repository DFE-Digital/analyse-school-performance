Feature: ContentTemplatesUpdateSingle


Scenario: Should return NotFound (404) response if id parameter is missing
	Given no Content Templates exist
	When I send a POST request to /api/content-templates//
	Then I should get a 404 response
	And the response should be the message "Not found: Function not found for path: /api/content-templates//"


Scenario: Should return BadRequest (400) response if revision parameter is empty string 
	Given no Content Templates exist
	When I send a POST request to /api/content-templates/xyz?revision=
	Then I should get a 400 response
	And the response should be the message "Bad request: The query parameter "revision" should not be empty."


Scenario: Should return BadRequest (400) response if revision parameter is duplicated
	Given no Content Templates exist
	When I send a POST request to /api/content-templates/xyz?revision=1&revision=2
	Then I should get a 400 response
	And the response should be the message "Bad request: The query parameter "revision" is duplicated."


Scenario: Should return BadRequest (400) response if request body is missing 
	Given no Content Templates exist
	When I send a POST request to /api/content-templates/test-content
	Then I should get a 400 response
	And the response should be the message "Bad request: The request body is missing."


Scenario: Should return BadRequest (400) response if request body is not an object 
	Given no Content Templates exist
	When I send a POST request to /api/content-templates/test-content with content:
	"""
	Hello
	"""
	Then I should get a 400 response
	And the response should be the message "Bad request: The request body is not a JSON object."


Scenario: Should return NotFound (404) when updating a revision of a Content Template that doesn't exist
	Given published Content Template with id "revision1" and contentId "test-content" exists:
	"""
	{
		"PageTitle": "Test title"
	}
	"""
	When I send a POST request to /api/content-templates/test-content?revision=revision1 with content:
	"""
	{
		"PageTitle": "Updated test title"
	}
	"""
	Then I should get a 404 response
	And the response should be the message "Not found: Could not find Content Template "test-content"."


Scenario: Should return Forbidden (403) when updating a published Content Template
	Given published Content Template with id "test-content" and contentId "test-content" exists:
	"""
	{
		"PageTitle": "Test title"
	}
	"""
	When I send a POST request to /api/content-templates/test-content with content:
	"""
	{
		"PageTitle": "Updated title",
	}
	"""
	Then I should get a 403 response
	And the response should be the message "Not allowed: Only unpublished Content Template revisions can be updated."


Scenario: Should return Forbidden (403) when updating a published revision
	Given published Content Template with id "test-content" and contentId "test-content" exists:
	"""
	{
		"PageTitle": "Test title"
	}
	"""
	And published Content Template with id "revision1" and contentId "test-content" exists:
	"""
	{
		"PageTitle": "Test title (revised)"
	}
	"""
	When I send a POST request to /api/content-templates/test-content?revision=revision1 with content:
	"""
	{
		"PageTitle": "Updated title",
	}
	"""
	Then I should get a 403 response
	And the response should be the message "Not allowed: Only unpublished Content Template revisions can be updated."


Scenario: Should create unpublished Content Template if it doesn't exist 
	Given no Content Templates exist
	When I send a POST request to /api/content-templates/test-content with content:
	"""
	{
		"PageTitle": "Test title",
	}
	"""
	Then I should get a 200 response
	And Content Template with id "test-content" and contentId "test-content" should match:
	"""
	{
		"isPublished": false,
		"PageTitle": "Test title"
	}
	"""


Scenario: Should create revision if it doesn't exist
	Given unpublished Content Template with id "test-content" and contentId "test-content" exists:
	"""
	{
		"PageTitle": "Test title"
	}
	"""
	When I send a POST request to /api/content-templates/test-content?revision=revision1 with content:
	"""
	{
		"PageTitle": "Updated test title"
	}
	"""
	Then I should get a 200 response
	And Content Template with id "test-content" and contentId "test-content" should match:
	"""
	{
		"isPublished": false,
		"PageTitle": "Test title"
	}
	"""
	And Content Template with id "revision1" and contentId "test-content" should match:
	"""
	{
		"isPublished": false,
		"PageTitle": "Updated test title"
	}
	"""


Scenario: Should update unpublished Content Template
	Given unpublished Content Template with id "test-content" and contentId "test-content" exists:
	"""
	{
		"PageTitle": "Test title"
	}
	"""
	When I send a POST request to /api/content-templates/test-content with content:
	"""
	{
		"PageTitle": "Updated title",
	}
	"""
	Then I should get a 200 response
	And Content Template with id "test-content" and contentId "test-content" should match:
	"""
	{
		"isPublished": false,
		"PageTitle": "Updated title"
	}
	"""


Scenario: Should update unpublished Content Template using revision parameter
	Given unpublished Content Template with id "test-content" and contentId "test-content" exists:
	"""
	{
		"PageTitle": "Test title"
	}
	"""
	When I send a POST request to /api/content-templates/test-content?revision=test-content with content:
	"""
	{
		"PageTitle": "Updated test title"
	}
	"""
	Then I should get a 200 response
	And Content Template with id "test-content" and contentId "test-content" should match:
	"""
	{
		"isPublished": false,
		"PageTitle": "Updated test title"
	}
	"""


Scenario: Should update unpublished revision
	Given published Content Template with id "test-content" and contentId "test-content" exists:
	"""
	{
		"PageTitle": "Test title"
	}
	"""
	And unpublished Content Template with id "revision1" and contentId "test-content" exists:
	"""
	{
		"PageTitle": "Test title (revised)"
	}
	"""
	When I send a POST request to /api/content-templates/test-content?revision=revision1 with content:
	"""
	{
		"PageTitle": "Updated test title (revised)"
	}
	"""
	Then I should get a 200 response
	And Content Template with id "test-content" and contentId "test-content" should match:
	"""
	{
		"isPublished": true,
		"PageTitle": "Test title"
	}
	"""
	And Content Template with id "revision1" and contentId "test-content" should match:
	"""
	{
		"isPublished": false,
		"PageTitle": "Updated test title (revised)"
	}
	"""

Scenario: Should ignore isPublished property when updating
	Given published Content Template with id "test-content" and contentId "test-content" exists:
	"""
	{
		"PageTitle": "Test title"
	}
	"""
	And unpublished Content Template with id "revision1" and contentId "test-content" exists:
	"""
	{
		"PageTitle": "Test title (revised)"
	}
	"""
	When I send a POST request to /api/content-templates/test-content?revision=revision1 with content:
	"""
	{
		"isPublished": true,
		"PageTitle": "Updated test title (revised)"
	}
	"""
	Then I should get a 200 response
	And Content Template with id "test-content" and contentId "test-content" should match:
	"""
	{
		"isPublished": true,
		"PageTitle": "Test title"
	}
	"""
	And Content Template with id "revision1" and contentId "test-content" should match:
	"""
	{
		"isPublished": false,
		"PageTitle": "Updated test title (revised)"
	}
	"""