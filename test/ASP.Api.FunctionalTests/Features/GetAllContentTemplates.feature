Feature: GetAllContentTemplates


Scenario: Should only return published content templates
	Given published content template "test-content-one" exists:
		"""
		{
			"PageTitle": "test one"
		}
		"""
	And unpublished content template "test-content-two" exists:
		"""
		{
			"PageTitle": "test two"
		}
		""" 
	When I send a GET request to /GetAllContentTemplates
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
	Given published content template "test-content-one" exists:
		"""
		{
			"PageTitle": "test one"
		}
		"""
	And published content template "test-content-two" exists:
		"""
		{
			"PageTitle": "test two"
		}
		""" 
	When I send a GET request to /GetAllContentTemplates
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
	Given no content template exists
	When I send a <method> request to /GetAllContentTemplates
	Then I should get a 405 response

Examples: 
	| method |
	| POST   |
	| DELETE |

Scenario: Should return NotFound (404) response if no published content templates exist 
	Given no content template exists
	When I send a GET request to /GetAllContentTemplates
	Then I should get a 404 response
	And the response should be the message "Not found: Could not find any published revision content templates"

Scenario: Should return NotFound (404) response if only unpublished content templates exist 
	And unpublished content template "test-content-two" exists:
		"""
		{
			"PageTitle": "test two"
		}
		""" 
	When I send a GET request to /GetAllContentTemplates
	Then I should get a 404 response
	And the response should be the message "Not found: Could not find any published revision content templates"