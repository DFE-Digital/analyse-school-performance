Feature: GetEstablishmentDetails

    Scenario: Should only accept GET method
        Given no Establishments exist
        When I send a <method> request to /api/GetEstablishmentDetails
        Then I should get a 405 response
    Examples:
      | method |
      | POST   |
      | DELETE |

    Scenario: Should return BadRequest (400) response if urn parameter is missing
        Given no Establishments exist
        When I send a GET request to /api/GetEstablishmentDetails
        Then I should get a 400 response
        And the response should be the message "Bad request: The parameter "urn" is missing."

    Scenario: Should return BadRequest (400) response if urn parameter is duplicated
        Given no Establishments exist
        When I send a GET request to /api/GetEstablishmentDetails?urn=x&urn=y
        Then I should get a 400 response
        And the response should be the message "Bad request: The parameter "urn" is duplicated."

    Scenario: Should return BadRequest (400) response if urn parameter is empty string
        Given no Establishments exist
        When I send a GET request to /api/GetEstablishmentDetails?urn=
        Then I should get a 400 response
        And the response should be the message "Bad request: The parameter "urn" should not be empty."

    Scenario: Should return BadRequest (400) response if urn parameter is not digits
        Given no Establishments exist
        When I send a GET request to /api/GetEstablishmentDetails?urn=xyz
        Then I should get a 400 response
        And the response should be the message "Bad request: The parameter "urn" must contain only digits."

    Scenario: Should return BadRequest (400) response if urn parameter is not 6 digits
        Given no Establishments exist
        When I send a GET request to /api/GetEstablishmentDetails?urn=<urn>
        Then I should get a 400 response
        And the response should be the message "Bad request: The parameter "urn" must be exactly 6 characters long."
    Examples:
      | urn     |
      | 123     |
      | 1234567 |

    Scenario: Should return NotFound (404) response if Establishment doesn't exist
        Given no Establishments exist
        When I send a GET request to /api/GetEstablishmentDetails?urn=123456
        Then I should get a 404 response
        And the response should be the message "Not found: Could not find Establishment with URN "123456"."

    Scenario: Should return Establishment object if urn exists
        Given Establishment "123456" exists:
        """
        {
            "Name": "Test name"
        }
        """
        When I send a GET request to /api/GetEstablishmentDetails?urn=123456
        Then I should get a 200 response
        And the response should be an object containing these properties:
        """
        {
        	"Name": "Test name",
            "Urn": "123456"
        }
        """