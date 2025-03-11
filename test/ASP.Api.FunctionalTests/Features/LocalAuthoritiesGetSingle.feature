Feature: LocalAuthoritiesGetSingle

	Scenario: Should only accept GET method
		Given no local authorities exist
		When I send a <method> request to /api/local-authorities/123
		Then I should get a 405 response

	Examples:
		| method |
		| POST   |
		| DELETE |

	Scenario: Should return BadRequest (400) response if code parameter is missing
		Given no local authorities exist
		When I send a GET request to /api/local-authorities//
		Then I should get a 404 response
		And the response should be the message "Not found: Function not found for path: /api/local-authorities//"

	Scenario: Should return BadRequest (400) response if urn parameter is not 3 digits
		Given no establishments exist
		When I send a GET request to /api/local-authorities/<code>
		Then I should get a 400 response
		And the response should be the message "Bad request: The path parameter "code" must be exactly 3 characters long."
	Examples:
		| code |
		| 12   |
		| 1234 |

	Scenario: Should return NotFound (404) response if local authority doesn't exist
		Given no local authorities exist
		When I send a GET request to /api/local-authorities/123
		Then I should get a 404 response
		And the response should be the message "Not found: Could not find Local Authority with code "123"."

	Scenario: Should return local authority object if code exists
		Given local authority Test name (321) exists
		When I send a GET request to /api/local-authorities/321
		Then I should get a 200 response
		And the response should be an object containing these properties:
		"""
		{
			"Name": "Test name",
			"Code": "321"
		}
		"""