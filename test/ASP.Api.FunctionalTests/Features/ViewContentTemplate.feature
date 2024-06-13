Feature: ViewContentTemplate

Scenario: Should not accept POST method 
	Given no content template exists
	When I send a POST request to /ViewContentTemplate?id=test-content
	Then I should get a 405 response
	And the response should be the message "Method not allowed: The HTTP method POST is not allowed."
	And the response should include the header "Allow: GET"

Scenario: Should return BadRequest (400) response if id parameter is missing 
	Given no content template exists
	When I send a GET request to /ViewContentTemplate
	Then I should get a 400 response
	And the response should be the message "Invalid: The parameter "id" is missing."

Scenario: Should return BadRequest (400) response if id parameter is duplicated
	Given no content template exists
	When I send a GET request to /ViewContentTemplate?id=x&id=y
	Then I should get a 400 response
	And the response should be the message "Invalid: The parameter "id" is duplicated."

Scenario: Should return BadRequest (400) response if id parameter is empty string 
	Given no content template exists
	When I send a GET request to /ViewContentTemplate?id=
	Then I should get a 400 response
	And the response should be the message "Invalid: The parameter "id" should not be empty."

Scenario: Should return BadRequest (400) response if revision parameter is empty string 
	When I send a GET request to /ViewContentTemplate?id=xyz&revision=
	Then I should get a 400 response
	And the response should be the message "Invalid: The parameter "revision" should not be empty."

Scenario: Should return BadRequest (400) response if revision parameter is duplicated
	When I send a GET request to /ViewContentTemplate?id=xyz&revision=1&revision=2
	Then I should get a 400 response
	And the response should be the message "Invalid: The parameter "revision" is duplicated."

Scenario: Should return NotFound (404) response if content template doesn't exist 
	Given no content template exists
	When I send a GET request to /ViewContentTemplate?id=test-content
	Then I should get a 404 response
	And the response should be the message "Not found: Could not find content template "test-content"."

Scenario: Should return NotFound (404) response if content template exists but is unpublished
	Given unpublished content template with id "test-content" and contentId "test-content" exists:
	"""
	{
	  "PageTitle": "Test title"
	}
	"""
	When I send a GET request to /ViewContentTemplate?id=test-content
	Then I should get a 404 response
	And the response should be the message "Not found: Could not find a published revision for content template "test-content"."

Scenario: Should return NotFound (404) response if revision does not exist
	Given published content template with id "test-content" and contentId "test-content" exists:
	"""
	{
	  "PageTitle": "Test title"
	}
	"""
	When I send a GET request to /ViewContentTemplate?id=test-content&revision=revision1
	Then I should get a 404 response
	And the response should be the message "Not found: Could not find revision "revision1" for content template "test-content"."

Scenario: Should return NotFound (404) when content template does not exist, even if revision does exist
	Given published content template with id "revision1" and contentId "test-content" exists:
	"""
	{
		"PageTitle": "Test title"
	}
	"""
	When I send a GET request to /ViewContentTemplate?id=test-content&revision=revision1
	Then I should get a 404 response
	And the response should be the message "Not found: Could not find content template "test-content"."

Scenario: Should return template object if content template exists and is published
	Given published content template "test-content" exists:
		"""
		{
			"PageTitle": "Test title"
		}
		"""
	When I send a GET request to /ViewContentTemplate?id=test-content
	Then I should get a 200 response
	And the response should be an object containing these properties:
		"""
		{
			"PageTitle": "Test title"
		}
		"""

Scenario: Should return template object if unpublished content template exists and revision parameter is provided
	Given unpublished content template with id "test-content" and contentId "test-content" exists:
	"""
	{
	  "PageTitle": "Test title"
	}
	"""
	When I send a GET request to /ViewContentTemplate?id=test-content&revision=test-content
	Then I should get a 200 response
	And the response should be an object containing these properties:
	"""
	{
	  "PageTitle": "Test title"
	}
	"""

Scenario: Should return template object if revision exists
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
	When I send a GET request to /ViewContentTemplate?id=test-content&revision=revision1
	Then I should get a 200 response
	And the response should be an object containing these properties:
	"""
	{
	  "PageTitle": "Test title (revised)"
	}
	"""

Scenario: Should return template object for published revision
	Given unpublished content template with id "test-content" and contentId "test-content" exists:
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
	When I send a GET request to /ViewContentTemplate?id=test-content
	Then I should get a 200 response
	And the response should be an object containing these properties:
	"""
	{
	  "PageTitle": "Test title (revised)"
	}
	"""