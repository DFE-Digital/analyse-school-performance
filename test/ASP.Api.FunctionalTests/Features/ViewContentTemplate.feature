Feature: ViewContentTemplate

Scenario: Should not accept POST method 
	Given no Content Templates exist
	When I send a POST request to /api/ViewContentTemplate?id=test-content
	Then I should get a 405 response
	And the response should be the message "Method not allowed: The HTTP method POST is not allowed."
	And the response should include the header "Allow: GET"

Scenario: Should return BadRequest (400) response if id parameter is missing 
	Given no Content Templates exist
	When I send a GET request to /api/ViewContentTemplate
	Then I should get a 400 response
	And the response should be the message "Bad request: The parameter "id" is missing."

Scenario: Should return BadRequest (400) response if id parameter is duplicated
	Given no Content Templates exist
	When I send a GET request to /api/ViewContentTemplate?id=x&id=y
	Then I should get a 400 response
	And the response should be the message "Bad request: The parameter "id" is duplicated."

Scenario: Should return BadRequest (400) response if id parameter is empty string 
	Given no Content Templates exist
	When I send a GET request to /api/ViewContentTemplate?id=
	Then I should get a 400 response
	And the response should be the message "Bad request: The parameter "id" should not be empty."

Scenario: Should return BadRequest (400) response if revision parameter is empty string 
	When I send a GET request to /api/ViewContentTemplate?id=xyz&revision=
	Then I should get a 400 response
	And the response should be the message "Bad request: The parameter "revision" should not be empty."

Scenario: Should return BadRequest (400) response if revision parameter is duplicated
	When I send a GET request to /api/ViewContentTemplate?id=xyz&revision=1&revision=2
	Then I should get a 400 response
	And the response should be the message "Bad request: The parameter "revision" is duplicated."

Scenario: Should return NotFound (404) response if Content Template doesn't exist 
	Given no Content Templates exist
	When I send a GET request to /api/ViewContentTemplate?id=test-content
	Then I should get a 404 response
	And the response should be the message "Not found: Could not find Content Template "test-content"."

Scenario: Should return NotFound (404) response if Content Template exists but is unpublished
	Given unpublished Content Template with id "test-content" and contentId "test-content" exists:
	"""
	{
	  "PageTitle": "Test title"
	}
	"""
	When I send a GET request to /api/ViewContentTemplate?id=test-content
	Then I should get a 404 response
	And the response should be the message "Not found: Could not find a published revision for Content Template "test-content"."

Scenario: Should return NotFound (404) response if revision does not exist
	Given published Content Template with id "test-content" and contentId "test-content" exists:
	"""
	{
	  "PageTitle": "Test title"
	}
	"""
	When I send a GET request to /api/ViewContentTemplate?id=test-content&revision=revision1
	Then I should get a 404 response
	And the response should be the message "Not found: Could not find revision "revision1" for Content Template "test-content"."

Scenario: Should return NotFound (404) when Content Template does not exist, even if revision does exist
	Given published Content Template with id "revision1" and contentId "test-content" exists:
	"""
	{
		"PageTitle": "Test title"
	}
	"""
	When I send a GET request to /api/ViewContentTemplate?id=test-content&revision=revision1
	Then I should get a 404 response
	And the response should be the message "Not found: Could not find Content Template "test-content"."

Scenario: Should return template object if Content Template exists and is published
	Given published Content Template "test-content" exists:
		"""
		{
			"PageTitle": "Test title"
		}
		"""
	When I send a GET request to /api/ViewContentTemplate?id=test-content
	Then I should get a 200 response
	And the response should be an object containing these properties:
		"""
		{
			"PageTitle": "Test title"
		}
		"""

Scenario: Should return template object if unpublished Content Template exists and revision parameter is provided
	Given unpublished Content Template with id "test-content" and contentId "test-content" exists:
	"""
	{
	  "PageTitle": "Test title"
	}
	"""
	When I send a GET request to /api/ViewContentTemplate?id=test-content&revision=test-content
	Then I should get a 200 response
	And the response should be an object containing these properties:
	"""
	{
	  "PageTitle": "Test title"
	}
	"""

Scenario: Should return template object if revision exists
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
	When I send a GET request to /api/ViewContentTemplate?id=test-content&revision=revision1
	Then I should get a 200 response
	And the response should be an object containing these properties:
	"""
	{
	  "PageTitle": "Test title (revised)"
	}
	"""

Scenario: Should return template object for published revision
	Given unpublished Content Template with id "test-content" and contentId "test-content" exists:
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
	When I send a GET request to /api/ViewContentTemplate?id=test-content
	Then I should get a 200 response
	And the response should be an object containing these properties:
	"""
	{
	  "PageTitle": "Test title (revised)"
	}
	"""