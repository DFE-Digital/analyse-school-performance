Feature: ViewContentTemplate

Scenario: Endpoint should not accept POST method 
	Given no page content exists
	When I send a POST request to /ViewContentTemplate?id=help-test
	Then I should get a 405 response
	And the response should be the message "Bad request: the HTTP method POST is not allowed."
	And the response should include the header "Allow: GET"

Scenario: Endpoint should return BadRequest (400) response if id parameter is missing 
	Given no page content exists
	When I send a GET request to /ViewContentTemplate
	Then I should get a 400 response
	And the response should be the message "Bad request: the parameter "id" is missing."

Scenario: Endpoint should return BadRequest (400) response if id parameter is duplicated
	Given no page content exists
	When I send a GET request to /ViewContentTemplate?id=x&id=y
	Then I should get a 400 response
	And the response should be the message "Bad request: the parameter "id" is duplicated."

Scenario: Endpoint should return BadRequest (400) response if id parameter is empty string 
	Given no page content exists
	When I send a GET request to /ViewContentTemplate?id=
	Then I should get a 400 response
	And the response should be the message "Bad request: the parameter "id" should not be empty."

Scenario: Endpoint should return NotFound (404) response if template with id doesn't exist 
	Given no page content exists
	When I send a GET request to /ViewContentTemplate?id=help-test
	Then I should get a 404 response
	And the response should be the message "Not found: could not find the object with id "help-test" and partition key "help-test" in container "content"."

Scenario: Endpoint should return template object if template exists
	Given page content "help-test" exists:
		"""
		{
			"PageTitle": "Test title"
		}
		"""
	When I send a GET request to /ViewContentTemplate?id=help-test
	Then I should get a 200 response
	And the response should be an object containing these properties:
		"""
		{
			"PageTitle": "Test title"
		}
		"""