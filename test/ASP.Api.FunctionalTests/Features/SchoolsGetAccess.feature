Feature: SchoolsGetAccess

    Scenario: Should not accept POST method
        When I send a POST request to /api/schools/100001/access
        Then I should get a 405 response
        And the response should be the message "Method not allowed: The HTTP method POST is not allowed."
        And the response should include the header "Allow: GET"

    Scenario: Should return BadRequest (400) response if urn parameter is empty
        When I send a GET request to /api/schools//access
        Then I should get a 404 response
        And the response should be the message "Not found: Function not found for path: /api/schools//access"

    Scenario Outline: Should return BadRequest (400) response if urn parameter is not 6 characters long
        Given no establishments exist
        When I send a GET request to /api/schools/<urn>/access
        Then I should get a 400 response
        And the response should be the message "Bad request: The path parameter "urn" must be exactly 6 characters long."

        Examples:
          | urn     |
          | 12345   |
          | 1234567 |

    Scenario: Should return NotFound (404) response if School does not exist
        Given establishment Test School 1 (100001) exists
        When I send a GET request to /api/schools/100002/access
        Then I should get a 404 response
        And the response should be the message "Not found: Could not find school with URN "100002"."

    Scenario: Return 400 response when "scope" parameter is missing and empty in the request
        Given establishment Test School 1 (100001) exists
        When I send a GET request to /api/schools/100001/access?scope=
        Then I should get a 400 response
        And the response should be the message "Bad request: The query parameter "scope" should not be empty."

    Scenario: Return 400 for invalid scope in SchoolsGetAccess API
        Given establishment Test School 1 (100001) exists
        When I send a GET request to /api/schools/100001/access?scope=xyz
        Then I should get a 400 response
        And the response should be the message "Bad request: "xyz" is not a valid scope."

    Scenario: Return 400 response when "scopeId" parameter is missing in the request
        Given establishment Test School 1 (100001) exists
        When I send a GET request to /api/schools/100001/access?scope=<Scope>
        Then I should get a 400 response
        And the response should be the message "Bad request: The query parameter "scopeId" is missing."

    Examples:
      | Scope   |
      | LA      |
      | la      |
      | MAT     |
      | mat     |
      | Diocese |
      | diocese |

    Scenario: LA Scope - Should return 400 (Bad request) if no LAs exist
        Given establishment Test School 1 (100001) exists
        When I send a GET request to /api/schools/100001/access?scope=LA&scopeId=101
        Then I should get a 400 response
        And the response should be the message "Bad request: Local Authority with code "101" does not exist."

    Scenario: LA Scope - Should return 400 (Bad request) if LA indicated by scope does not exist
        Given establishment Test School 1 (100001) exists
        And local authority Test LA 1 (100) exists
        When I send a GET request to /api/schools/100001/access?scope=LA&scopeId=101
        Then I should get a 400 response
        And the response should be the message "Bad request: Local Authority with code "101" does not exist."

    Scenario: LA Scope - Should not be accessible if not in LA
        Given establishment Test School 1 (100001) exists in local authority 999
        And local authority Test LA 1 (101) exists
        When I send a GET request to /api/schools/100001/access?scope=LA&scopeId=101
        Then I should get a 200 response
        And the response should be an object containing these properties:
        """
        {
          "IsAccessibleInScope": false,
          "IsAccessibleViaLinkedSchools": false
        }
        """

    Scenario: LA Scope - Should be accessible if in LA
        Given establishment Test School 1 (100001) exists in local authority 101
        And local authority Test LA 1 (101) exists
        When I send a GET request to /api/schools/100001/access?scope=LA&scopeId=101
        Then I should get a 200 response
        And the response should be an object containing these properties:
        """
        {
          "IsAccessibleInScope": true,
          "IsAccessibleViaLinkedSchools": false
        }
        """

    Scenario: LA Scope - Should not be accessible if not in LA even if linked school is in LA
        Given establishment Test School 1 (100001) exists in local authority 999 with properties:
        """
        {
          "links": [
            {
              "linkedUrn": "100002",
              "establishedDate": "2020-10-01",
              "linkType": {
                "code": "1",
                "name": "Predecessor"
              }
            }
          ]
        }
        """
        And establishment Test School 2 (100002) exists in local authority 101 with properties:
        """
        {
          "links": [
            {
              "linkedUrn": "100001",
              "establishedDate": "2020-10-01",
              "linkType": {
                "code": "1",
                "name": "Successor"
              }
            }
          ]
        }
        """
        And local authority Test LA 1 (101) exists
        When I send a GET request to /api/schools/100001/access?scope=LA&scopeId=101
        Then I should get a 200 response
        And the response should be an object containing these properties:
        """
        {
          "IsAccessibleInScope": false,
          "IsAccessibleViaLinkedSchools": false
        }
        """

    Scenario: MAT Scope - Should return 400 (Bad request) if no MATs exist
        Given establishment Test School 1 (100001) exists
        When I send a GET request to /api/schools/100001/access?scope=MAT&scopeId=1001
        Then I should get a 400 response
        And the response should be the message "Bad request: Multi-Academy Trust with UID "1001" does not exist."

    Scenario: MAT Scope - Should return 400 (Bad request) if MAT indicated by scope does not exist
        Given establishment Test School 1 (100001) exists
        Given multi-academy trust Test MAT 1 (1000) exists
        When I send a GET request to /api/schools/100001/access?scope=MAT&scopeId=1001
        Then I should get a 400 response
        And the response should be the message "Bad request: Multi-Academy Trust with UID "1001" does not exist."

    Scenario: MAT Scope - Should not be accessible if not in MAT
        Given establishment Test School 1 (100001) exists in multi-academy trust 9999
        And multi-academy trust Test MAT 1 (1001) exists
        When I send a GET request to /api/schools/100001/access?scope=MAT&scopeId=1001
        Then I should get a 200 response
        And the response should be an object containing these properties:
        """
        {
          "IsAccessibleInScope": false,
          "IsAccessibleViaLinkedSchools": false
        }
        """

    Scenario: MAT Scope - Should be accessible if in MAT
        Given establishment Test School 1 (100001) exists in multi-academy trust 1001
        And multi-academy trust Test MAT 1 (1001) exists
        When I send a GET request to /api/schools/100001/access?scope=MAT&scopeId=1001
        Then I should get a 200 response
        And the response should be an object containing these properties:
        """
        {
          "IsAccessibleInScope": true,
          "IsAccessibleViaLinkedSchools": false
        }
        """

    Scenario: MAT Scope - Should be accessible if not in MAT but linked school is in MAT
        Given establishment Test School 1 (100001) exists in multi-academy trust 9999 with properties:
        """
        {
          "links": [
            {
              "linkedUrn": "100002",
              "establishedDate": "2020-10-01",
              "linkType": {
                "code": "1",
                "name": "Predecessor"
              }
            }
          ]
        }
        """
        And establishment Test School 2 (100002) exists in multi-academy trust 1001 with properties:
        """
        {
          "links": [
            {
              "linkedUrn": "100001",
              "establishedDate": "2020-10-01",
              "linkType": {
                "code": "1",
                "name": "Successor"
              }
            }
          ]
        }
        """
        And multi-academy trust Test MAT 1 (1001) exists
        When I send a GET request to /api/schools/100001/access?scope=MAT&scopeId=1001
        Then I should get a 200 response
        And the response should be an object containing these properties:
        """
        {
          "IsAccessibleInScope": false,
          "IsAccessibleViaLinkedSchools": true
        }
        """

    Scenario: Diocese Scope - Should not be accessible if not in diocese
        Given establishment Test School 1 (100001) exists in diocese XYZ
        When I send a GET request to /api/schools/100001/access?scope=Diocese&scopeId=Test%20Diocese%201
        Then I should get a 200 response
        And the response should be an object containing these properties:
        """
        {
          "IsAccessibleInScope": false,
          "IsAccessibleViaLinkedSchools": false
        }
        """

    Scenario: Diocese Scope - Should be accessible if in diocese
        Given establishment Test School 1 (100001) exists in diocese Test Diocese 1
        When I send a GET request to /api/schools/100001/access?scope=Diocese&scopeId=Test%20Diocese%201
        Then I should get a 200 response
        And the response should be an object containing these properties:
        """
        {
          "IsAccessibleInScope": true,
          "IsAccessibleViaLinkedSchools": false
        }
        """

    Scenario: Diocese Scope - Should be accessible if not in diocese but linked school is in diocese
        Given establishment Test School 1 (100001) exists in diocese XYZ with properties:
        """
        {
          "links": [
            {
              "linkedUrn": "100002",
              "establishedDate": "2020-10-01",
              "linkType": {
                "code": "1",
                "name": "Predecessor"
              }
            }
          ]
        }
        """
        And establishment Test School 2 (100002) exists in diocese Test Diocese 1 with properties:
        """
        {
          "links": [
            {
              "linkedUrn": "100001",
              "establishedDate": "2020-10-01",
              "linkType": {
                "code": "1",
                "name": "Successor"
              }
            }
          ]
        }
        """
        When I send a GET request to /api/schools/100001/access?scope=Diocese&scopeId=Test%20Diocese%201
        Then I should get a 200 response
        And the response should be an object containing these properties:
        """
        {
          "IsAccessibleInScope": false,
          "IsAccessibleViaLinkedSchools": true
        }
        """

    Scenario: Check if an establishment is accessible in the 'All' scope
        Given establishment Test School 1 (100001) exists
        When I send a GET request to /api/schools/100001/access
        Then I should get a 200 response
        And the response should be an object containing these properties:
        """
        {
          "IsAccessibleInScope": true,
          "IsAccessibleViaLinkedSchools": false
        }
        """