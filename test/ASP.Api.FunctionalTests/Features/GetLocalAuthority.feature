Feature: GetLocalAuthority

    Scenario: Should only accept GET method
        Given no local authority exists
        When I send a <method> request to /api/GetLocalAuthority
        Then I should get a 405 response

    Examples:
      | method |
      | POST   |
      | DELETE |

    Scenario: Should return BadRequest (400) response if code parameter is missing
        Given no local authority exists
        When I send a GET request to /api/GetLocalAuthority
        Then I should get a 400 response
        And the response should be the message "Invalid: The parameter "code" is missing."

    Scenario: Should return BadRequest (400) response if code parameter is duplicated
        Given no local authority exists
        When I send a GET request to /api/GetLocalAuthority?code=x&code=y
        Then I should get a 400 response
        And the response should be the message "Invalid: The parameter "code" is duplicated."

    Scenario: Should return BadRequest (400) response if code parameter is empty string
        Given no local authority exists
        When I send a GET request to /api/GetLocalAuthority?code=
        Then I should get a 400 response
        And the response should be the message "Invalid: The parameter "code" should not be empty."

    Scenario: Should return NotFound (404) response if local authority doesn't exist
        Given no local authority exists
        When I send a GET request to /api/GetLocalAuthority?code=123
        Then I should get a 404 response
        And the response should be the message "Not found: Could not find local authority with code "123"."

    Scenario: Should return local authority object if code exists
        Given localAuthority "321" exists:
        """
        {
            "Name": "Test name",
            "Code": "321"
        }
        """
        When I send a GET request to /api/GetLocalAuthority?code=321
        Then I should get a 200 response
        And the response should be an object containing these properties:
        """
        {
        	"Name": "Test name",
            "Code": "321"
        }
        """