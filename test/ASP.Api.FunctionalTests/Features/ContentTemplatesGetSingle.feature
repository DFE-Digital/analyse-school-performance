Feature: ContentTemplatesGetSingle

Scenario Outline: Should only accept GET method 
	Given no Content Templates exist
	When I send a <method> request to /api/content-templates/xyz
	Then I should get a 405 response

Examples: 
	| method |
	| PUT    |
	| DELETE |

Scenario: Should return NotFound (404) response if id parameter is missing 
	Given no Content Templates exist
	When I send a GET request to /api/content-templates//
	Then I should get a 404 response
	And the response should be the message "Not found: Function not found for path: /api/content-templates//"

Scenario: Should return BadRequest (400) response if revision parameter is empty string 
	When I send a GET request to /api/content-templates/xyz?revision=
	Then I should get a 400 response
	And the response should be the message "Bad request: The query parameter "revision" should not be empty."

Scenario: Should return BadRequest (400) response if revision parameter is duplicated
	When I send a GET request to /api/content-templates/xyz?revision=1&revision=2
	Then I should get a 400 response
	And the response should be the message "Bad request: The query parameter "revision" is duplicated."

Scenario: Should return NotFound (404) response if Content Template doesn't exist 
	Given no Content Templates exist
	When I send a GET request to /api/content-templates/test-content
	Then I should get a 404 response
	And the response should be the message "Not found: Could not find Content Template "test-content"."

Scenario: Should return NotFound (404) response if Content Template exists but is unpublished
	Given unpublished Content Template with id "test-content" and contentId "test-content" exists:
	"""
	{
		"PageTitle": "Test title"
	}
	"""
	When I send a GET request to /api/content-templates/test-content
	Then I should get a 404 response
	And the response should be the message "Not found: Could not find a published revision for Content Template "test-content"."

Scenario: Should return NotFound (404) response if revision does not exist
	Given published Content Template with id "test-content" and contentId "test-content" exists:
	"""
	{
		"PageTitle": "Test title"
	}
	"""
	When I send a GET request to /api/content-templates/test-content?revision=revision1
	Then I should get a 404 response
	And the response should be the message "Not found: Could not find revision "revision1" for Content Template "test-content"."

Scenario: Should return NotFound (404) when Content Template does not exist, even if revision does exist
	Given published Content Template with id "revision1" and contentId "test-content" exists:
	"""
	{
		"PageTitle": "Test title"
	}
	"""
	When I send a GET request to /api/content-templates/test-content?revision=revision1
	Then I should get a 404 response
	And the response should be the message "Not found: Could not find Content Template "test-content"."

Scenario: Should return template object if Content Template exists and is published
	Given published Content Template "test-content" exists:
		"""
		{
			"PageTitle": "Test title"
		}
		"""
	When I send a GET request to /api/content-templates/test-content
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
	When I send a GET request to /api/content-templates/test-content?revision=test-content
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
	When I send a GET request to /api/content-templates/test-content?revision=revision1
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
	When I send a GET request to /api/content-templates/test-content
	Then I should get a 200 response
	And the response should be an object containing these properties:
	"""
	{
		"PageTitle": "Test title (revised)"
	}
	"""