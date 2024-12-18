Feature: GetMultiAcademyTrust

	Scenario: Should only accept GET method
		Given no Multi Academy Trust exist
		When I send a <method> request to /api/GetMultiAcademyTrust
		Then I should get a 405 response

	Examples:
		| method |
		| POST   |
		| DELETE |

	Scenario: Should return BadRequest (400) response if id parameter is missing
		Given no Multi Academy Trust exist
		When I send a GET request to /api/GetMultiAcademyTrust
		Then I should get a 400 response
		And the response should be the message "Bad request: The parameter "id" is missing."

	Scenario: Should return BadRequest (400) response if id parameter is duplicated
		Given no Multi Academy Trust exist
		When I send a GET request to /api/GetMultiAcademyTrust?id=x&id=y
		Then I should get a 400 response
		And the response should be the message "Bad request: The parameter "id" is duplicated."

	Scenario: Should return BadRequest (400) response if id parameter is empty string
		Given no Multi Academy Trust exist
		When I send a GET request to /api/GetMultiAcademyTrust?id=
		Then I should get a 400 response
		And the response should be the message "Bad request: The parameter "id" should not be empty."

	Scenario: Should return NotFound (404) response if multi academy trust doesn't exist
		Given no Multi Academy Trust exist
		When I send a GET request to /api/GetMultiAcademyTrust?id=123
		Then I should get a 404 response
		And the response should be the message "Not found: Could not find Multi-Academy Trust with id "123"."

	Scenario: Should return multi academy trust object if id exists
		Given Multi Academy Trust "2044" exists:
		"""
		{
			"name": "Test name"
		}
		"""
		When I send a GET request to /api/GetMultiAcademyTrust?id=2044
		Then I should get a 200 response
		And the response should be an object containing these properties:
		"""
		{
			"Name": "Test name",
			"Id": "2044"
		}
		"""