Feature: UpdateContentTemplate

Scenario: Endpoint should not accept GET method 
	Given no page content exists
	When I send a GET request to /UpdateContentTemplate?id=help-test
	Then I should get a 405 response
	And the response should be the message "The HTTP method GET is not allowed."
	And the response should include the header "Allow: POST"

Scenario: Endpoint should return BadRequest (400) response if id parameter is missing 
	Given no page content exists
	When I send a POST request to /UpdateContentTemplate
	Then I should get a 400 response
	And the response should be the message "Missing parameter: "id"."

Scenario: Endpoint should return BadRequest (400) response if id parameter is empty string 
	Given no page content exists
	When I send a POST request to /UpdateContentTemplate?id=
	Then I should get a 400 response
	And the response should be the message "Missing parameter: "id"."

Scenario: Endpoint should return BadRequest (400) response if request body is missing 
	Given no page content exists
	When I send a POST request to /UpdateContentTemplate?id=help-text
	Then I should get a 400 response
	And the response should be the message "Missing request body."

Scenario: Endpoint should return BadRequest (400) response if request body is not an object 
	Given no page content exists
	When I send a POST request to /UpdateContentTemplate?id=help-text with content:
		"""
			Hello
		"""
	Then I should get a 400 response
	And the response should be the message "Request body was not a JSON object."

Scenario: Endpoint should create content template if one doesn't exist 
	Given no page content exists
	When I send a POST request to /UpdateContentTemplate?id=help-test with content:
		"""
		{
			"PageTitle": "Updated title",
		}
		"""
	Then I should get a 200 response
	And page content "help-test" property "PageTitle" should be equal to "Updated title"

Scenario: Endpoint should update content template
	Given page content "help-test" exists:
		"""
		{
			"PageTitle": "Test title",
		}
		"""
	When I send a POST request to /UpdateContentTemplate?id=help-test with content:
		"""
		{
			"PageTitle": "Updated title",
		}
		"""
	Then I should get a 200 response
	And page content "help-test" property "PageTitle" should be equal to "Updated title"
