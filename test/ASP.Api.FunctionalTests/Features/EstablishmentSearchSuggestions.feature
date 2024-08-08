Feature: Establishment Search Suggestions

    Scenario: Should not accept POST method
        When I send a POST request to /api/EstablishmentSearchSuggestions
        Then I should get a 405 response
        And the response should be the message "Method not allowed: The HTTP method POST is not allowed."
        And the response should include the header "Allow: GET"

    Scenario: Should return BadRequest (400) response if searchTerm parameter is missing
        When I send a GET request to /api/EstablishmentSearchSuggestions
        Then I should get a 400 response
        And the response should be the message "Bad request: The parameter "searchTerm" is missing."

    Scenario: Should return BadRequest (400) response if searchTerm parameter is empty string
        When I send a GET request to /api/EstablishmentSearchSuggestions?searchTerm=
        Then I should get a 400 response
        And the response should be the message "Bad request: The parameter "searchTerm" should not be empty."

    Scenario Outline: Should return BadRequest (400) response if page parameter is not a whole number greater than 1
        When I send a GET request to /api/EstablishmentSearchSuggestions?searchTerm=x&maxSuggestions=<maxSuggestions>
        Then I should get a 400 response
        And the response should be the message "Bad request: The parameter "maxSuggestions" should be a whole number greater than or equal to 1."

        Examples:
          | maxSuggestions |
          | y              |
          | 1.5            |
          | 0              |
          | -1             |

    Scenario: Should return NotFound (404) response if no matches found for the searchTerm
        Given no Establishments exist
        When I send a GET request to /api/EstablishmentSearchSuggestions?searchTerm=x
        Then I should get a 404 response
        And the response should be the message "Not found: there were no matches for "x"."

    Scenario: Should return NotFound (404) response if there were no relevant matches for the given searchTerm
        Given Establishment "111111" exists:
        """
        {
            "name": "Some Primary School"
        }
        """
        When I send a GET request to /api/EstablishmentSearchSuggestions?searchTerm=secondary
        Then I should get a 404 response
        And the response should be the message "Not found: there were no matches for "secondary"."

    Scenario: Should return a NotFound (404) response if the requested establishment has been deleted for the given searchTerm
        Given deleted Establishment "222222" exists:
        """
        {
            "name": "Some Primary School"
        }
        """
        When I send a GET request to /api/EstablishmentSearchSuggestions?searchTerm=222222
        Then I should get a 404 response
        And the response should be the message "Not found: The requested establishment with URN "222222" has been deleted."

    Scenario: Should return a NotFound (404) response if the requested establishment is not currently visible for the given searchTerm
        Given non-visible Establishment "111111" exists:
        """
        {
            "name": "Some Primary School"
        }
        """
        When I send a GET request to /api/EstablishmentSearchSuggestions?searchTerm=111111
        Then I should get a 404 response
        And the response should be the message "Not found: The requested establishment with URN "111111" is not currently visible."

    Scenario: Should not return 400 response if maxSuggestions = 1
    Given Establishment "111111" exists: 
    """
    {
        "name": "Some Primary School"
    }
    """
    When I send a GET request to /api/EstablishmentSearchSuggestions?searchTerm=111111&maxSuggestions=1
    Then I should get a 200 response

    Scenario Outline: Should return 200 response with search results when matches are found for the given searchTerm
        Given Establishment "987654" exists:
        """
        {
            "laestab": "123/4567",
            "name": "Some Primary School"
        }
        """
        When I send a GET request to /api/EstablishmentSearchSuggestions?searchTerm=<searchTerm>
        Then I should get a 200 response
        And the response should be an object containing these properties excluding null:
        """
        {
            "SearchTerm": "<searchTerm>",
            "MaxSuggestions": 10,
            "Suggestions": [
                {
                    "Urn": "987654",
                    "Laestab": "123/4567",
                    "Name": "Some Primary School"
                }
            ]
        }
        """

        Examples:
          | searchTerm |
          | Prim       |
          | PRiMaRY    |
          | ry sc      |
          | 87         |
          | 23         |
          | 56         |
          | 23/45      |
          | 2345       |
          | 56         |

    Scenario: Should return a 200 response with search suggestions results and results are sorted alphabetically for the given searchTerm
        Given Establishment "111111" exists:
        """
        {
            "laestab": "123/1111",
            "name": "Primary School C"
        }
        """
        And Establishment "222222" exists:
        """
        {
            "laestab": "123/2222",
            "name": "Primary School D"
        }
        """
        And Establishment "333333" exists:
        """
        {
            "laestab": "123/3333",
            "name": "Primary School B"
        }
        """
        And Establishment "444444" exists:
        """
        {
            "laestab": "123/4444",
            "name": "Primary School A"
        }
        """
        When I send a GET request to /api/EstablishmentSearchSuggestions?searchTerm=primary
        Then I should get a 200 response
        And the response should be an object containing these properties excluding null:
        """
        {
            "SearchTerm": "primary",
            "MaxSuggestions": 10,
            "Suggestions": [
                {
                    "Urn": "444444",
                    "Laestab": "123/4444",
                    "Name": "Primary School A"
                },
                {
                    "Urn": "333333",
                    "Laestab": "123/3333",
                    "Name": "Primary School B"
                },
                {
                    "Urn": "111111",
                    "Laestab": "123/1111",
                    "Name": "Primary School C"
                },
                {
                    "Urn": "222222",
                    "Laestab": "123/2222",
                    "Name": "Primary School D"
                }
            ]
        }
        """

    Scenario: Should return a 200 response with search suggestion results that are sorted alphabetically, up to the maximum number of suggestions specified for the given search term
        Given Establishment "111111" exists:
        """
        {
            "laestab": "123/1111",
            "name": "Primary School C"
        }
        """
        And Establishment "222222" exists:
        """
        {
            "laestab": "123/2222",
            "name": "Primary School B"
        }
        """
        And Establishment "333333" exists:
        """
        {
            "laestab": "123/3333",
            "name": "Primary School A"
        }
        """
        And Establishment "444444" exists:
        """
        {
            "laestab": "123/4444",
            "name": "Primary School D"
        }
        """
        When I send a GET request to /api/EstablishmentSearchSuggestions?searchTerm=primary&maxSuggestions=2
        Then I should get a 200 response
        And the response should be an object containing these properties excluding null:
        """
        {
            "SearchTerm": "primary",
            "MaxSuggestions": 2,
            "Suggestions": [
                {
                    "Urn": "333333",
                    "Laestab": "123/3333",
                    "Name": "Primary School A"
                },
                {
                    "Urn": "222222",
                    "Laestab": "123/2222",
                    "Name": "Primary School B"
                }
            ]
        }
        """