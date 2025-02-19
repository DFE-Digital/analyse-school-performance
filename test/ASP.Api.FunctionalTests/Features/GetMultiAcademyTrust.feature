Feature: GetMultiAcademyTrust

	Scenario: Should only accept GET method
		Given no Multi Academy Trust exist
		When I send a <method> request to /api/multi-academy-trusts/123
		Then I should get a 405 response

	Examples:
		| method |
		| POST   |
		| DELETE |

	Scenario: Should return BadRequest (400) response if id parameter is missing
		Given no Multi Academy Trust exist
		When I send a GET request to /api/multi-academy-trusts//
		Then I should get a 404 response
		And the response should be the message "Not found: Function not found for path: /api/multi-academy-trusts//"

	Scenario: Should return NotFound (404) response if multi academy trust doesn't exist
		Given no Multi Academy Trust exist
		When I send a GET request to /api/multi-academy-trusts/123
		Then I should get a 404 response
		And the response should be the message "Not found: Could not find Multi-Academy Trust with id "123"."

	Scenario: Should return multi academy trust object if id exists
		Given Multi Academy Trust "2044" exists:
		"""
		{
			"name": "Test name"
		}
		"""
		When I send a GET request to /api/multi-academy-trusts/2044
		Then I should get a 200 response
		And the response should be an object containing these properties:
		"""
		{
			"Name": "Test name",
			"Id": "2044"
		}
		"""