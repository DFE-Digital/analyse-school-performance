Feature: ViewContentPage

Scenario: Endpoint should not accept POST method 
	Given no page content exists
	When I send a POST request to /ViewContentPage?id=help-test
	Then I should get a 405 response
	And the response should be the message "The HTTP method POST is not allowed."
	And the response should include the header "Allow: GET"

Scenario: Endpoint should return BadRequest (400) response if id parameter is missing 
	Given no page content exists
	When I send a GET request to /ViewContentPage
	Then I should get a 400 response
	And the response should be the message "Missing parameter: "id"."

Scenario: Endpoint should return BadRequest (400) response if id parameter is empty string 
	Given no page content exists
	When I send a GET request to /ViewContentPage?id=
	Then I should get a 400 response
	And the response should be the message "Missing parameter: "id"."

Scenario: Endpoint should return NotFound (404) response if template with id doesn't exist 
	Given no page content exists
	When I send a GET request to /ViewContentPage?id=help-test
	Then I should get a 404 response
	And the response should be the message "Document with id "help-test" and partition key "help-test" does not exist in container "content"."

Scenario: Endpoint should return template object if template exists
	Given page content "help-test" exists:
		"""
		{
			"PageTitle": "Test title"
		}
		"""
	When I send a GET request to /ViewContentPage?id=help-test
	Then I should get a 200 response
	And the response should be an object containing these properties:
		"""
		{
			"id": "help-test",
			"PageTitle": "Test title"
		}
		"""