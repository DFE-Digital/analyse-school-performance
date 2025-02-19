Feature: GetAllContentTemplates


Scenario: Should only return published content templates
	Given published Content Template "test-content-one" exists:
		"""
		{
			"PageTitle": "test one"
		}
		"""
	And unpublished Content Template "test-content-two" exists:
		"""
		{
			"PageTitle": "test two"
		}
		""" 
	When I send a GET request to /api/content-templates
	Then I should get a 200 response
	And the response should be an array of objects containing these properties:
		"""
		[
			{
				"PageTitle": "test one"
			}
		]
		"""

Scenario: Should return multiple template objects if content templates exists
	Given published Content Template "test-content-one" exists:
		"""
		{
			"PageTitle": "test one"
		}
		"""
	And published Content Template "test-content-two" exists:
		"""
		{
			"PageTitle": "test two"
		}
		""" 
	When I send a GET request to /api/content-templates
	Then I should get a 200 response
	And the response should be an array of objects containing these properties:
		"""
		[
			{
				"PageTitle": "test one"
			},
			{
				"PageTitle": "test two"
			}
		]
		"""

Scenario: Should only accept GET method 
	Given no Content Templates exist
	When I send a <method> request to /api/content-templates
	Then I should get a 405 response

Examples: 
	| method |
	| POST   |
	| DELETE |

Scenario: Should return NotFound (404) response if no published Content Templates exist 
	Given no Content Templates exist
	When I send a GET request to /api/content-templates
	Then I should get a 404 response
	And the response should be the message "Not found: Could not find any published Content Templates"

Scenario: Should return NotFound (404) response if only unpublished Content Templates exist 
	And unpublished Content Template "test-content-two" exists:
		"""
		{
			"PageTitle": "test two"
		}
		""" 
	When I send a GET request to /api/content-templates
	Then I should get a 404 response
	And the response should be the message "Not found: Could not find any published Content Templates"