Feature: UpdateContentTemplate

Scenario: Should not accept GET method 
	Given no content template exists
	When I send a GET request to /UpdateContentTemplate?id=test-content
	Then I should get a 405 response
	And the response should be the message "Method not allowed: The HTTP method GET is not allowed."
	And the response should include the header "Allow: POST"


Scenario: Should return BadRequest (400) response if id parameter is missing 
	Given no content template exists
	When I send a POST request to /UpdateContentTemplate
	Then I should get a 400 response
	And the response should be the message "Invalid: The parameter "id" is missing."


Scenario: Should return BadRequest (400) response if id parameter is duplicated
	Given no content template exists
	When I send a POST request to /UpdateContentTemplate?id=x&id=y
	Then I should get a 400 response
	And the response should be the message "Invalid: The parameter "id" is duplicated."


Scenario: Should return BadRequest (400) response if id parameter is empty string 
	Given no content template exists
	When I send a POST request to /UpdateContentTemplate?id=
	Then I should get a 400 response
	And the response should be the message "Invalid: The parameter "id" should not be empty."


Scenario: Should return BadRequest (400) response if revision parameter is empty string 
	Given no content template exists
	When I send a POST request to /UpdateContentTemplate?id=xyz&revision=
	Then I should get a 400 response
	And the response should be the message "Invalid: The parameter "revision" should not be empty."


Scenario: Should return BadRequest (400) response if revision parameter is duplicated
	Given no content template exists
	When I send a POST request to /UpdateContentTemplate?id=xyz&revision=1&revision=2
	Then I should get a 400 response
	And the response should be the message "Invalid: The parameter "revision" is duplicated."


Scenario: Should return BadRequest (400) response if request body is missing 
	Given no content template exists
	When I send a POST request to /UpdateContentTemplate?id=test-content
	Then I should get a 400 response
	And the response should be the message "Invalid: The request body is missing."


Scenario: Should return BadRequest (400) response if request body is not an object 
	Given no content template exists
	When I send a POST request to /UpdateContentTemplate?id=test-content with content:
	"""
	Hello
	"""
	Then I should get a 400 response
	And the response should be the message "Invalid: The request body is not a JSON object."


Scenario: Should return NotFound (404) when updating a revision of a content template that doesn't exist
	Given published content template with id "revision1" and contentId "test-content" exists:
	"""
	{
		"PageTitle": "Test title"
	}
	"""
	When I send a POST request to /UpdateContentTemplate?id=test-content&revision=revision1 with content:
	"""
	{
		"PageTitle": "Updated test title"
	}
	"""
	Then I should get a 404 response
	And the response should be the message "Not found: Could not find content template "test-content"."


Scenario: Should return Forbidden (403) when updating a published content template
	Given published content template with id "test-content" and contentId "test-content" exists:
	"""
	{
		"PageTitle": "Test title"
	}
	"""
	When I send a POST request to /UpdateContentTemplate?id=test-content with content:
	"""
	{
		"PageTitle": "Updated title",
	}
	"""
	Then I should get a 403 response
	And the response should be the message "Not allowed: Only unpublished content template revisions can be updated."


Scenario: Should return Forbidden (403) when updating a published revision
	Given published content template with id "test-content" and contentId "test-content" exists:
	"""
	{
		"PageTitle": "Test title"
	}
	"""
	And published content template with id "revision1" and contentId "test-content" exists:
	"""
	{
		"PageTitle": "Test title (revised)"
	}
	"""
	When I send a POST request to /UpdateContentTemplate?id=test-content&revision=revision1 with content:
	"""
	{
		"PageTitle": "Updated title",
	}
	"""
	Then I should get a 403 response
	And the response should be the message "Not allowed: Only unpublished content template revisions can be updated."


Scenario: Should create unpublished content template if it doesn't exist 
	Given no content template exists
	When I send a POST request to /UpdateContentTemplate?id=test-content with content:
	"""
	{
		"PageTitle": "Test title",
	}
	"""
	Then I should get a 200 response
	And content template with id "test-content" and contentId "test-content" should match:
	"""
	{
		"isPublished": false,
		"PageTitle": "Test title"
	}
	"""


Scenario: Should create revision if it doesn't exist
	Given unpublished content template with id "test-content" and contentId "test-content" exists:
	"""
	{
		"PageTitle": "Test title"
	}
	"""
	When I send a POST request to /UpdateContentTemplate?id=test-content&revision=revision1 with content:
	"""
	{
		"PageTitle": "Updated test title"
	}
	"""
	Then I should get a 200 response
	And content template with id "test-content" and contentId "test-content" should match:
	"""
	{
		"isPublished": false,
		"PageTitle": "Test title"
	}
	"""
	And content template with id "revision1" and contentId "test-content" should match:
	"""
	{
		"isPublished": false,
		"PageTitle": "Updated test title"
	}
	"""


Scenario: Should update unpublished content template
	Given unpublished content template with id "test-content" and contentId "test-content" exists:
	"""
	{
		"PageTitle": "Test title"
	}
	"""
	When I send a POST request to /UpdateContentTemplate?id=test-content with content:
	"""
	{
		"PageTitle": "Updated title",
	}
	"""
	Then I should get a 200 response
	And content template with id "test-content" and contentId "test-content" should match:
	"""
	{
		"isPublished": false,
		"PageTitle": "Updated title"
	}
	"""


Scenario: Should update unpublished content template using revision parameter
	Given unpublished content template with id "test-content" and contentId "test-content" exists:
	"""
	{
		"PageTitle": "Test title"
	}
	"""
	When I send a POST request to /UpdateContentTemplate?id=test-content&revision=test-content with content:
	"""
	{
		"PageTitle": "Updated test title"
	}
	"""
	Then I should get a 200 response
	And content template with id "test-content" and contentId "test-content" should match:
	"""
	{
		"isPublished": false,
		"PageTitle": "Updated test title"
	}
	"""


Scenario: Should update unpublished revision
	Given published content template with id "test-content" and contentId "test-content" exists:
	"""
	{
		"PageTitle": "Test title"
	}
	"""
	And unpublished content template with id "revision1" and contentId "test-content" exists:
	"""
	{
		"PageTitle": "Test title (revised)"
	}
	"""
	When I send a POST request to /UpdateContentTemplate?id=test-content&revision=revision1 with content:
	"""
	{
		"PageTitle": "Updated test title (revised)"
	}
	"""
	Then I should get a 200 response
	And content template with id "test-content" and contentId "test-content" should match:
	"""
	{
		"isPublished": true,
		"PageTitle": "Test title"
	}
	"""
	And content template with id "revision1" and contentId "test-content" should match:
	"""
	{
		"isPublished": false,
		"PageTitle": "Updated test title (revised)"
	}
	"""

Scenario: Should ignore isPublished property when updating
	Given published content template with id "test-content" and contentId "test-content" exists:
	"""
	{
		"PageTitle": "Test title"
	}
	"""
	And unpublished content template with id "revision1" and contentId "test-content" exists:
	"""
	{
		"PageTitle": "Test title (revised)"
	}
	"""
	When I send a POST request to /UpdateContentTemplate?id=test-content&revision=revision1 with content:
	"""
	{
		"isPublished": true,
		"PageTitle": "Updated test title (revised)"
	}
	"""
	Then I should get a 200 response
	And content template with id "test-content" and contentId "test-content" should match:
	"""
	{
		"isPublished": true,
		"PageTitle": "Test title"
	}
	"""
	And content template with id "revision1" and contentId "test-content" should match:
	"""
	{
		"isPublished": false,
		"PageTitle": "Updated test title (revised)"
	}
	"""