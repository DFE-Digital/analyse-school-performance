Feature: Establishment Search

    Scenario: Should not accept POST method
        When I send a POST request to /api/EstablishmentSearch
        Then I should get a 405 response
        And the response should be the message "Method not allowed: The HTTP method POST is not allowed."
        And the response should include the header "Allow: GET"

    Scenario: Should return BadRequest (400) response if searchTerm parameter is missing
        When I send a GET request to /api/EstablishmentSearch?scope=All
        Then I should get a 400 response
        And the response should be the message "Bad request: The parameter "searchTerm" is missing."

    Scenario: Should return BadRequest (400) response if searchTerm parameter is empty string
        When I send a GET request to /api/EstablishmentSearch?scope=All&searchTerm=
        Then I should get a 400 response
        And the response should be the message "Bad request: The parameter "searchTerm" should not be empty."

    Scenario Outline: Should return BadRequest (400) response if page parameter is not a whole number greater than or equal to 1
        When I send a GET request to /api/EstablishmentSearch?scope=All&searchTerm=x&page=<page>
        Then I should get a 400 response
        And the response should be the message "Bad request: The parameter "page" should be a whole number greater than or equal to 1."

        Examples:
          | page |
          | y    |
          | 1.5  |
          | 0    |
          | -1   |

    Scenario Outline: Should return BadRequest (400) response if resultsPerPage parameter is not a whole number greater than 1
        When I send a GET request to /api/EstablishmentSearch?scope=All&searchTerm=x&resultsPerPage=<resultsPerPage>
        Then I should get a 400 response
        And the response should be the message "Bad request: The parameter "resultsPerPage" should be a whole number greater than or equal to 1."

        Examples:
          | resultsPerPage |
          | y              |
          | 1.5            |
          | 0              |
          | -1             |

    Scenario: Should return NotFound (404) response if no matches found for the searchTerm
        Given no Establishments exist
        When I send a GET request to /api/EstablishmentSearch?scope=All&searchTerm=x
        Then I should get a 404 response
        And the response should be the message "Not found: there were no matches for "x" within the given scope."

    Scenario: Should return NotFound (404) response if there were no relevant matches for the given searchTerm
        Given Establishment "111111" exists:
        """
        {
            "name": "Some Primary School"
        }
        """
        When I send a GET request to /api/EstablishmentSearch?scope=All&searchTerm=secondary
        Then I should get a 404 response
        And the response should be the message "Not found: there were no matches for "secondary" within the given scope."

    Scenario: Should return a NotFound (404) response if the requested establishment has been deleted for the given searchTerm
        Given deleted Establishment "222222" exists:
        """
        {
            "name": "Some Primary School"
        }
        """
        When I send a GET request to /api/EstablishmentSearch?scope=All&searchTerm=222222
        Then I should get a 404 response
        And the response should be the message "Not found: there were no matches for "222222" within the given scope."

    Scenario: Should return a NotFound (404) response if the requested establishment is not currently visible for the given searchTerm
        Given non-visible Establishment "111111" exists:
        """
        {
            "name": "Some Primary School"
        }
        """
        When I send a GET request to /api/EstablishmentSearch?scope=All&searchTerm=111111
        Then I should get a 404 response
        And the response should be the message "Not found: there were no matches for "111111" within the given scope."

    Scenario: Should not return 400 response if page = 1
        Given Establishment "111111" exists:
        """
        {
            "name": "Some Primary School"
        }
        """
        When I send a GET request to /api/EstablishmentSearch?scope=All&searchTerm=111111&page=1
        Then I should get a 200 response

    Scenario: Should not return 400 response if resultsPerPage = 1
        Given Establishment "111111" exists:
        """
        {
            "name": "Some Primary School"
        }
        """
        When I send a GET request to /api/EstablishmentSearch?scope=All&searchTerm=111111&resultsPerPage=1
        Then I should get a 200 response

    Scenario Outline: Should return 200 response with search results when matches are found for the given searchTerm
        Given Establishment "111111" exists:
        """
        {
            "name": "Some Primary School"
        }
        """
        When I send a GET request to /api/EstablishmentSearch?scope=All&searchTerm=<searchTerm>
        Then I should get a 200 response
        And the response should be an object containing these properties excluding null:
        """
        {
            "SearchTerm": "<searchTerm>",
            "TotalResults": 1,
            "ResultsPerPage": 50,
            "Page": 1,
            "Results": [
                {
                    "Urn": "111111",
                    "Name": "Some Primary School"
                }
            ]
        }
        """

        Examples:
          | searchTerm |
          | Prim       |
          | PRiMaRY    |

    Scenario Outline: Should return 200 response with search results when searchTerm matching establishment street, town and postcode partially
        Given Establishment "111111" exists:
        """
        {
            "name": "Some Primary School",
            "address": {
                "street": "13 The Street",
                "town": "SomeTown",
                "postCode": "TR18 3JT"
            } 
        }
        """
        When I send a GET request to /api/EstablishmentSearch?scope=All&searchTerm=<searchTerm>
        Then I should get a 200 response
        And the response should be an object containing these properties excluding null:
        """
            {
                "SearchTerm": "<searchTerm>",
                "TotalResults": 1,
                "ResultsPerPage": 50,
                "Page": 1,
                "Results": [
                    {
                        "Urn": "111111",
                        "Name": "Some Primary School",
                        "Address": "13 The Street, SomeTown TR18 3JT"
                    }
                ]
            }
        """

        Examples:
          | searchTerm |
          | str        |
          | some       |
          | tr1        |

    Scenario: Should return 200 response with search results when searchTerm matching establishment name and address partially
        Given Establishment "111111" exists:
        """
        {
            "name": "Some Primary School",
            "address": {
                "street": "13 The Street",
                "town": "SomeTown",
                "postCode": "B1 1AA"
            } 
        }
        """
        And Establishment "222222" exists:
        """
        {
            "name": "Some Other Primary School",
            "address": {
                "street": "13 The Road",
                "town": "Tring",
                "postCode": "B1 1AA"
            } 
        }
        """
        And Establishment "333333" exists:
        """
        {
            "name": "A Different Primary School",
            "address": {
                "street": "13 The Road",
                "town": "SomeTown",
                "postCode": "TR18 1AA"
            } 
        }
        """
        And Establishment "444444" exists:
        """
        {
            "name": "The Training Center"
        }
        """
        When I send a GET request to /api/EstablishmentSearch?scope=All&searchTerm=tr
        Then I should get a 200 response
        And the response should be an object containing these properties excluding null:
        """
        {
            "SearchTerm": "tr",
            "TotalResults": 4,
            "ResultsPerPage": 50,
            "Page": 1,
            "Results": [
                {
                    "Urn": "333333", 
                    "Name": "A Different Primary School",
                    "Address": "13 The Road, SomeTown TR18 1AA"
                },
                {
                    "Urn": "222222", 
                    "Name": "Some Other Primary School",
                    "Address": "13 The Road, Tring B1 1AA"
                },
                {
                    "Urn": "111111", 
                    "Name": "Some Primary School",
                    "Address": "13 The Street, SomeTown B1 1AA"
                },
                {
                    "Urn": "444444", 
                    "Name": "The Training Center"
                }
            ]
        }
        """

    Scenario: Should return 200 response with search results when searchTerm matching establishment URN
        Given Establishment "111111" exists:
        """
        {
            "Urn": "111111",
            "name": "Some Primary School"
        }
        """
        When I send a GET request to /api/EstablishmentSearch?scope=All&searchTerm=111111
        Then I should get a 200 response
        And the response should be an object containing these properties excluding null:
        """
        {
            "SearchTerm": "111111",
            "TotalResults": 1,
            "ResultsPerPage": 50,
            "Page": 1,
            "Results": [
                {
                    "Urn": "111111",
                    "Name": "Some Primary School"
                }
            ]
        }
        """

    Scenario: Should return a NotFound (404) response if no relevant matches are found for the establishment URN based on the given searchTerm
        Given Establishment "111111" exists:
        """
        {
            "name": "Some Primary School"
        }
        """
        When I send a GET request to /api/EstablishmentSearch?scope=All&searchTerm=11
        Then I should get a 404 response
        And the response should be the message "Not found: there were no matches for "11" within the given scope."

    Scenario: Should return 200 response with search results when searchTerm matches partially with the address street name
        Given Establishment "111111" exists:
        """
        {
            "name": "Some Primary School"
        }
        """
        And Establishment "222222" exists:
        """
        {
            "name": "A Different Primary School",
            "address": {
                "street": "111 The Street",
                "town": "SomeTown",
                "postCode": "TR18 3JT"
            } 
        }
        """
        When I send a GET request to /api/EstablishmentSearch?scope=All&searchTerm=11
        Then I should get a 200 response
        And the response should be an object containing these properties excluding null:
        """
        {
            "SearchTerm": "11",
            "TotalResults": 1,
            "ResultsPerPage": 50,
            "Page": 1,
            "Results": [
                {
                    "Urn": "222222", 
                    "Name": "A Different Primary School",
                    "Address": "111 The Street, SomeTown TR18 3JT"
                }
            ]
        }
        """

    Scenario: Should return 200 response with search results when searchTerm is 6 digit number treat it as an exact URN search
        Given Establishment "111111" exists:
        """
        {
            "name": "Some Primary School"
        }
        """
        And Establishment "222222" exists:
        """
        {
            "name": "A Different Primary School",
            "address": {
                "street": "111111 The Street",
                "town": "SomeTown",
                "postCode": "TR18 3JT"
            } 
        }
        """
        When I send a GET request to /api/EstablishmentSearch?scope=All&searchTerm=111111
        Then I should get a 200 response
        And the response should be an object containing these properties excluding null:
        """
        {
            "SearchTerm": "111111",
            "TotalResults": 1,
            "ResultsPerPage": 50,
            "Page": 1,
            "Results": [
                {
                    "Urn": "111111", 
                    "Name": "Some Primary School"
                }
            ]
        }
        """

    Scenario Outline: Should return 200 response with search results when searchTerm matching establishment LAESTAB code (with and without forward slash)
        Given Establishment "111111" exists:
        """
        {
            "name": "Some Primary School",
            "laestab": "894/2200",
        }
        """
        When I send a GET request to /api/EstablishmentSearch?scope=All&searchTerm=<searchTerm>
        Then I should get a 200 response
        And the response should be an object containing these properties excluding null:
        """
        {
            "SearchTerm": "<searchTerm>",
            "TotalResults": 1,
            "ResultsPerPage": 50,
            "Page": 1,
            "Results": [
                {
                    "Urn": "111111", 
                    "Name": "Some Primary School",
                    "Laestab": "894/2200"
                }
            ]
        }
        """

        Examples:
          | searchTerm |
          | 894/2200   |
          | 8942200    |

    Scenario: Should return 200 response with search results when searchTerm matches with LAESTAB 3 digit code partially
        Given Establishment "111111" exists:
        """
        {
            "laestab": "894/2200",
            "name": "Some Primary School"
        }
        """
        And Establishment "222222" exists:
        """
        {
            "laestab": "894/1234",
            "name": "Some Other Primary School"
        }
        """
        When I send a GET request to /api/EstablishmentSearch?scope=All&searchTerm=894
        Then I should get a 200 response
        And the response should be an object containing these properties excluding null:
        """
            {
                "SearchTerm": "894",
                "TotalResults": 2,
                "ResultsPerPage": 50,
                "Page": 1,
                "Results": [
                    { 
                        "Urn": "222222", 
                        "Laestab": "894/1234",
                        "Name": "Some Other Primary School" 
                    },
                    { 
                        "Urn": "111111", 
                        "Laestab": "894/2200",
                        "Name": "Some Primary School" 
                    }
                ]
            }    

        """

    Scenario: Should return 200 response with search results when searchTerm matches with LAESTAB 4 digit code partially
        Given Establishment "111111" exists:
        """
        {
            "laestab": "894/2200",
            "name": "Some Primary School"
        }
        """
        And Establishment "222222" exists:
        """
        {
            "laestab": "123/2200",
            "name": "Some Other Primary School"
        }
        """
        When I send a GET request to /api/EstablishmentSearch?scope=All&searchTerm=2200
        Then I should get a 200 response
        And the response should be an object containing these properties excluding null:
        """
        {
            "SearchTerm": "2200",
            "TotalResults": 2,
            "ResultsPerPage": 50,
            "Page": 1,
            "Results": [
                {
                    "Urn": "222222", 
                    "Laestab": "123/2200",
                    "Name": "Some Other Primary School" 
                },
                {
                    "Urn": "111111", 
                    "Laestab": "894/2200",
                    "Name": "Some Primary School" 
                }
            ]
        }    
        """

    Scenario Outline: Should return NotFound (404) response if there were no relevant matches for the given searchTerm associated with a LAESTAB code
        Given Establishment "111111" exists:
        """
        {
            "laestab": "894/2200",
            "name": "Some Primary School"
        }
        """
        When I send a GET request to /api/EstablishmentSearch?scope=All&searchTerm=<searchTerm>
        Then I should get a 404 response
        And the response should be the message "Not found: there were no matches for "<searchTerm>" within the given scope."

        Examples:
          | searchTerm |
          | 89         |
          | 22         |

    Scenario: Should return 200 response with search results when searchTerm matching establishment 7 digits LAESTAB code ignoring other matching fields
        Given Establishment "111111" exists:
        """
        {
            "laestab": "894/2200",
            "name": "Some Primary School",
        }
        """
        And Establishment "222222" exists:
        """
        {
            "name": "Some Other Primary School",
            "address": {
                "street": "8942200 The Street",
                "town": "SomeTown", 
                "postCode": "TR18 3JT"
            }
        }
        """
        When I send a GET request to /api/EstablishmentSearch?scope=All&searchTerm=8942200
        Then I should get a 200 response
        And the response should be an object containing these properties excluding null:
        """
        {
            "SearchTerm": "8942200",
            "TotalResults": 1,
            "ResultsPerPage": 50,
            "Page": 1,
            "Results": [
                {
                    "Urn": "111111", 
                    "Name": "Some Primary School",
                    "Laestab": "894/2200"
                }
            ]
        }
        """

    Scenario: Should return 200 response with search results when searchTerm matching establishment 7 digits LAESTAB code with forward slash ignoring other matching fields
        Given Establishment "111111" exists:
        """
        {
            "name": "Some Primary School",
            "laestab": "894/2200"
        }
        """
        And Establishment "222222" exists:
        """
        {
            "name": "Some other Primary School",
            "address": {
                "street": "894/2200 The Street",
                "town": "SomeTown", 
                "postCode": "TR18 3JT"
            }  
        }
        """
        When I send a GET request to /api/EstablishmentSearch?scope=All&searchTerm=894/2200
        Then I should get a 200 response
        And the response should be an object containing these properties excluding null:
        """
        {
            "SearchTerm": "894/2200",
            "TotalResults": 1,
            "ResultsPerPage": 50,
            "Page": 1,
            "Results": [
                {
                    "Urn": "111111", 
                    "Name": "Some Primary School",
                    "Laestab": "894/2200"
                }
            ]
        }
        """

    Scenario: Should return 200 response with search results when searchTerm matching establishment 3 digits LAESTAB code ignoring other matching fields
        Given Establishment "111111" exists:
        """
        {
            "name": "Some Primary School",
            "laestab": "894/2200"
        }
        """
        And Establishment "222222" exists:
        """
        {
            "name": "Some other Primary School",
            "address": {
                "street": "894 The Street",
                "town": "SomeTown", 
                "postCode": "TR18 3JT"
            }  
        }
        """
        When I send a GET request to /api/EstablishmentSearch?scope=All&searchTerm=894
        Then I should get a 200 response
        And the response should be an object containing these properties excluding null:
        """
        {
            "SearchTerm": "894",
            "TotalResults": 1,
            "ResultsPerPage": 50,
            "Page": 1,
            "Results": [
                {
                    "Urn": "111111", 
                    "Name": "Some Primary School",
                    "Laestab": "894/2200"
                }
            ]
        }
        """

    Scenario: Should return 200 response with search results when searchTerm matching establishment 4 digits LAESTAB code ignoring other matching fields
        Given Establishment "111111" exists:
        """
        {
            "name": "Some Primary School",
            "laestab": "894/2200"
        }
        """
        And Establishment "222222" exists:
        """
        {
            "name": "Some other Primary School",
            "address": {
                "street": "2200 The Street",
                "town": "SomeTown", 
                "postCode": "TR18 3JT"
            }  
        }
        """
        When I send a GET request to /api/EstablishmentSearch?scope=All&searchTerm=2200
        Then I should get a 200 response
        And the response should be an object containing these properties excluding null:
        """
        {
            "SearchTerm": "2200",
            "TotalResults": 1,
            "ResultsPerPage": 50,
            "Page": 1,
            "Results": [
                {
                    "Urn": "111111", 
                    "Name": "Some Primary School",
                    "Laestab": "894/2200"
                }
            ]
        }
        """

    Scenario: Should return a 200 response with search results and expected pagination for the given searchTerm
        Given Establishment "111111" exists:
        """
        {
            "name": "Primary School 111111"
        }
        """
        And Establishment "222222" exists:
        """
        {
            "name": "Primary School 222222"
        }
        """
        And Establishment "333333" exists:
        """
        {
            "name": "Primary School 333333"
        }
        """
        When I send a GET request to /api/EstablishmentSearch?scope=All&searchTerm=primary
        Then I should get a 200 response
        And the response should be an object containing these properties excluding null:
        """
        {
            "SearchTerm": "primary",
            "TotalResults": 3,
            "ResultsPerPage": 50,
            "Page": 1,
            "Results": [
                {
                "Urn": "111111",
                "Name": "Primary School 111111"
                },
                {
                    "Urn": "222222", 
                    "Name": "Primary School 222222"
                },
                {
                    "Urn": "333333", 
                    "Name": "Primary School 333333"
                }
            ]
        }    
        """

    Scenario: Should return a 200 response with search results and expected pagination for the given searchTerm and resultsPerPage
        Given Establishment "111111" exists:
        """
        {
            "name": "Primary School 111111"
        }
        """
        And Establishment "222222" exists:
        """
        {
            "name": "Primary School 222222"
        }
        """
        And Establishment "333333" exists:
        """
        {
            "name": "Primary School 333333"
        }
        """
        When I send a GET request to /api/EstablishmentSearch?scope=All&searchTerm=primary&resultsPerPage=2
        Then I should get a 200 response
        And the response should be an object containing these properties excluding null:
        """
        {
            "SearchTerm": "primary",
            "TotalResults": 3,
            "ResultsPerPage": 2,
            "Page": 1,
            "Results": [
                {
                "Urn": "111111",
                "Name": "Primary School 111111"
                },
                {
                    "Urn": "222222", 
                    "Name": "Primary School 222222"
                }
            ]
        }    
        """

    Scenario: Should return a 200 response with search results and expected pagination for the given searchTerm, resultsPerPage and page
        Given Establishment "111111" exists:
        """
        {
            "name": "Primary School 111111"
        }
        """
        And Establishment "222222" exists:
        """
        {
            "name": "Primary School 222222"
        }
        """
        And Establishment "333333" exists:
        """
        {
            "name": "Primary School 333333"
        }
        """
        When I send a GET request to /api/EstablishmentSearch?scope=All&searchTerm=primary&resultsPerPage=2&page=2
        Then I should get a 200 response
        And the response should be an object containing these properties excluding null:
        """
        {
            "SearchTerm": "primary",
            "TotalResults": 3,
            "ResultsPerPage": 2,
            "Page": 2,
            "Results": [
                {
                "Urn": "333333", 
                "Name": "Primary School 333333"
                }
            ]
        }
        """

    Scenario: Should return a 200 response with expected pagination and no results for the given searchTerm, resultsPerPage, and page
        Given Establishment "111111" exists:
        """
        {
            "name": "Primary School 111111"
        }
        """
        And Establishment "222222" exists:
        """
        {
            "name": "Primary School 222222"
        }
        """
        And Establishment "333333" exists:
        """
        {
            "name": "Primary School 333333"
        }
        """
        When I send a GET request to /api/EstablishmentSearch?scope=All&searchTerm=primary&resultsPerPage=2&page=3
        Then I should get a 200 response
        And the response should be an object containing these properties excluding null:
        """
        {
            "SearchTerm": "primary",
            "TotalResults": 3,
            "ResultsPerPage": 2,
            "Page": 2,
            "Results": [
            ]
        }
        """

    Scenario: Should return a 200 response with search results and a computed address field when the searchTerm matches the URN and given address has street, town and postcode
        Given Establishment "111111" exists:
        """
        {
            "name": "Some Primary School",
            "address": {
                "street": "13 The Street",
                "town": "SomeTown",
                "postCode": "AB12 3CD"
            } 
        }
        """
        When I send a GET request to /api/EstablishmentSearch?scope=All&searchTerm=111111
        Then I should get a 200 response
        And the response should be an object containing these properties excluding null:
        """
        {
            "SearchTerm": "111111",
            "TotalResults": 1,
            "ResultsPerPage": 50,
            "Page": 1,
            "Results": [
                {
                    "Urn": "111111", 
                    "Name": "Some Primary School",
                    "Address": "13 The Street, SomeTown AB12 3CD"
                }
            ]
        }
        """

    Scenario: Should return a 200 response with search results and a computed address field when the searchTerm matches the URN and given address has street and postcode
        Given Establishment "111111" exists:
        """
        {
            "name": "Some Primary School",
            "address": {
                "street": "13 The Street",
                "postCode": "AB12 3CD"
            } 
        }
        """
        When I send a GET request to /api/EstablishmentSearch?scope=All&searchTerm=111111
        Then I should get a 200 response
        And the response should be an object containing these properties excluding null:
        """
        {
            "SearchTerm": "111111",
            "TotalResults": 1,
            "ResultsPerPage": 50,
            "Page": 1,
            "Results": [
                {
                    "Urn": "111111", 
                    "Name": "Some Primary School",
                    "Address": "13 The Street AB12 3CD"
                }
            ]
        }
        """

    Scenario: Should return a 200 response with search results and a computed address field when the searchTerm matches the URN and given address has street and town
        Given Establishment "111111" exists:
        """
        {
            "name": "Some Primary School",
            "address": {
                "street": "13 The Street",
                "town": "SomeTown"
            } 
        }
        """
        When I send a GET request to /api/EstablishmentSearch?scope=All&searchTerm=111111
        Then I should get a 200 response
        And the response should be an object containing these properties excluding null:
        """
        {
            "SearchTerm": "111111",
            "TotalResults": 1,
            "ResultsPerPage": 50,
            "Page": 1,
            "Results": [
                {
                    "Urn": "111111", 
                    "Name": "Some Primary School",
                    "Address": "13 The Street, SomeTown"
                }
            ]
        }
        """

    Scenario: Should return a 200 response with search results and a computed educationPhase field when the searchTerm matches the URN and given educationPhase isPrimary equal true
        Given Establishment "111111" exists:
        """
        {
            "name": "Some Primary School",
            "isPost16": false,
            "isPrimary": true,
            "isSecondary": false
        }
        """
        When I send a GET request to /api/EstablishmentSearch?scope=All&searchTerm=111111
        Then I should get a 200 response
        And the response should be an object containing these properties excluding null:
        """
        {
            "SearchTerm": "111111",
            "TotalResults": 1,
            "ResultsPerPage": 50,
            "Page": 1,
            "Results": [
                {
                    "Urn": "111111", 
                    "Name": "Some Primary School",
                    "EducationPhase": "Primary"
                }
            ]
        }
        """

    Scenario: Should return a 200 response with search results and a computed educationPhase field when the searchTerm matches the URN and given educationPhase isSecondary equal true
        Given Establishment "111111" exists:
        """
        {
            "name": "Some Primary School",
            "isPost16": false,
            "isPrimary": false,
            "isSecondary": true
        }
        """
        When I send a GET request to /api/EstablishmentSearch?scope=All&searchTerm=111111
        Then I should get a 200 response
        And the response should be an object containing these properties excluding null:
        """
        {
            "SearchTerm": "111111",
            "TotalResults": 1,
            "ResultsPerPage": 50,
            "Page": 1,
            "Results": [
                {
                    "Urn": "111111", 
                    "Name": "Some Primary School",
                    "EducationPhase": "Secondary"
                }
            ]
        }
        """

    Scenario: Should return a 200 response with search results and a computed educationPhase field when the searchTerm matches the URN and given educationPhase isPost16 equal true
        Given Establishment "111111" exists:
        """
        {
            "name": "Some Primary School",
            "isPost16": true,
            "isPrimary": false,
            "isSecondary": false
        }
        """
        When I send a GET request to /api/EstablishmentSearch?scope=All&searchTerm=111111
        Then I should get a 200 response
        And the response should be an object containing these properties excluding null:
        """
        {
            "SearchTerm": "111111",
            "TotalResults": 1,
            "ResultsPerPage": 50,
            "Page": 1,
            "Results": [
                {
                    "Urn": "111111", 
                    "Name": "Some Primary School",
                    "EducationPhase": "16 to 18"
                }
            ]
        }
        """

    Scenario: Should return a 200 response with search results and a computed ofstedRating field when the searchTerm matches the URN
        Given Establishment "111111" exists:
        """
        {
            "name": "Some Primary School",
            "ofstedLastInspectionDate": "2013-03-22T00:00:00",
            "ofstedRating": {
                "code": "2",
                "name": "Good"
            }
        }
        """
        When I send a GET request to /api/EstablishmentSearch?scope=All&searchTerm=111111
        Then I should get a 200 response
        And the response should be an object containing these properties excluding null:
        """
        {
            "SearchTerm": "111111",
            "TotalResults": 1,
            "ResultsPerPage": 50,
            "Page": 1,
            "Results": [
                {
                    "Urn": "111111", 
                    "Name": "Some Primary School",
                    "OfstedRating": {
                    "Code": "2",
                    "Name": "Good",
                    "LastInspected": "22 March 2013"
                    }
                }
            ]
        }
        """

    Scenario: Should return BadRequest (400) response if scope parameter is missing
        When I send a GET request to /api/EstablishmentSearch?searchTerm=xyz
        Then I should get a 400 response
        And the response should be the message "Bad request: The parameter "scope" is missing."

    Scenario: Should return BadRequest (400) response if scope parameter is empty string
        When I send a GET request to /api/EstablishmentSearch?searchTerm=xyz&scope=
        Then I should get a 400 response
        And the response should be the message "Bad request: The parameter "scope" should not be empty."

    Scenario: Should return BadRequest (400) response if scope parameter is invalid
        When I send a GET request to /api/EstablishmentSearch?searchTerm=xyz&scope=xyz
        Then I should get a 400 response
        And the response should be the message "Bad request: "xyz" is not a valid "scope"."

    Scenario Outline: Should return BadRequest (400) response if scopeIdentifier parameter is missing
        When I send a GET request to /api/EstablishmentSearch?searchTerm=xyz&scope=<Scope>
        Then I should get a 400 response
        And the response should be the message "Bad request: The parameter "scopeIdentifier" is missing."

        Examples:
          | Scope   |
          | LA      |
          | la      |
          | MAT     |
          | mat     |
          | Diocese |
          | diocese |

    Scenario: Should return BadRequest (400) response if Local Authority with code does not exist
        When I send a GET request to /api/EstablishmentSearch?searchTerm=Test&scope=LA&scopeIdentifier=100
        Then I should get a 400 response
        And the response should be the message "Bad request: Local Authority with code "100" does not exist."

    Scenario: Should return NotFound (404) response if Local Authority with code does not exist
        Given Local Authority "100" exists:
        """
        {
            "Name": "Test LA"
        }
        """
        And Establishment "111111" exists:
        """
        {
            "name": "Test School 1",
             "localAuthority": {
                 "code": "999"
              }
        }
        """
        When I send a GET request to /api/EstablishmentSearch?searchTerm=Test&scope=LA&scopeIdentifier=100
        Then I should get a 404 response
        And the response should be the message "Not found: there were no matches for "Test" within the given scope."

    Scenario: Should return 200 response if Local Authority with code exist within the given scope "LA"
        Given Local Authority "100" exists:
        """
        {
            "Name": "Test LA"
        }
        """
        And Establishment "111111" exists:
        """
        {
           "name": "Test School 1",
            "localAuthority": {
              "code": "100"
            }
        }
        """
        And Establishment "222222" exists:
        """
        {
             "name": "Test School 2",
             "localAuthority": {
                "code": "100"
             }
        }
        """
        And Establishment "333333 " exists:
        """
        {
             "name": "Test School 3",
             "localAuthority": {
                "code": "999"
             }
        }
        """
        When I send a GET request to /api/EstablishmentSearch?searchTerm=Test&scope=LA&scopeIdentifier=100
        Then I should get a 200 response
        And the response should be an object containing these properties excluding null:
        """
        {
            "SearchTerm": "Test",
            "Scope": "LA",
            "ScopeIdentifier": "100",
            "TotalResults": 2,
            "ResultsPerPage": 50,
            "Page": 1,
            "Results": [
                {
                    "Urn": "111111",
                    "Name": "Test School 1"
                },
                {
                    "Urn": "222222",
                    "Name": "Test School 2"
                }
            ]   
        }
        """
   
    Scenario: Should return BadRequest (400) response if Multi Academy Trust with id does not exist
        When I send a GET request to /api/EstablishmentSearch?searchTerm=Test&scope=MAT&scopeIdentifier=1234
        Then I should get a 400 response
        And the response should be the message "Bad request: Multi-Academy Trust with UID "1234" does not exist."
             
    Scenario: Should return NotFound (404) response if Multi Academy Trust with id does not exist
        Given Multi Academy Trust "1234" exists:
        """
        {
            "Name": "Test MAT"
        }
        """
        And Establishment "111111" exists:
        """
        {
            "name": "Test School 1"
        }
        """
        When I send a GET request to /api/EstablishmentSearch?searchTerm=Test&scope=MAT&scopeIdentifier=1234
        Then I should get a 404 response
        And the response should be the message "Not found: there were no matches for "Test" within the given scope."    
        
    Scenario: Should return 200 response if Multi Academy Trust with id exist within the given scope "MAT"
        Given Multi Academy Trust "1234" exists:
        """
        {
            "Name": "Test MAT"
        }
        """
        And Establishment "111111" exists:
        """
        {
           "name": "Test School 1",
            "multiAcademyTrust": {
                "uid": 1234
            }
           
        }
        """
        And Establishment "222222" exists:
        """
        {
             "name": "Test School 2"
        }
        """
        And Establishment "333333" exists:
        """
        {
             "name": "Test School 3",
             "multiAcademyTrust": {
                "uid": 1234
             }
        }
        """
        When I send a GET request to /api/EstablishmentSearch?searchTerm=Test&scope=MAT&scopeIdentifier=1234
        Then I should get a 200 response
        And the response should be an object containing these properties excluding null:
        """
        {
             "SearchTerm": "Test",
             "Scope": "MAT",
             "ScopeIdentifier": "1234",
             "TotalResults": 2,
             "ResultsPerPage": 50,
             "Page": 1,
             "Results": [
                {
                    "Urn": "111111",
                    "Name": "Test School 1"
                },
                {
                    "Urn": "333333",
                    "Name": "Test School 3"
                }
             ]
        }
        """
        
    Scenario: Should return NotFound (404) response if there are no matches for Diocese scope search
        Given Establishment "111111" exists:
        """
        {
            "name": "Test School 1",
             "diocese": {
                "code": "0000",
                "name": "Not applicable"
             }
        }
        """
        And Establishment "222222 " exists:
        """
        {
            "name": "Test School 2",
             "diocese": null
        }
        """
        And Establishment "333333" exists:
        """
        {
            "name": "Test School 3",
        }
        """
        
        When I send a GET request to /api/EstablishmentSearch?searchTerm=Test&scope=Diocese&scopeIdentifier=Test Diocese
        Then I should get a 404 response
        And the response should be the message "Not found: there were no matches for "Test" within the given scope." 
        
    Scenario: Should return 200 response if there are matches for the search within the given scope "Diocese"
        Given Establishment "111111" exists:
        """
        {
           "name": "Test School 1",
             "diocese": {
                "code": "1000",
                "name": "Test Diocese"
             }
        }
        """
        And Establishment "222222" exists:
        """
        {
             "name": "Test School 2",
             "diocese": {
                "code": "1001",
                "name": "Another Diocese"
             }
        }
        """
        And Establishment "333333" exists:
        """
        {
             "name": "Test School 3",
             "diocese": {
                "code": "1000",
                "name": "Test Diocese"
             }
        }
        """
        When I send a GET request to /api/EstablishmentSearch?searchTerm=Test&scope=Diocese&scopeIdentifier=Test Diocese
        Then I should get a 200 response
        And the response should be an object containing these properties excluding null:
        """
        {
              "SearchTerm": "Test",
              "Scope": "Diocese",
              "ScopeIdentifier": "Test Diocese",
              "TotalResults": 2,
              "ResultsPerPage": 50,
              "Page": 1,
              "Results": [
                {
                    "Urn": "111111",
                    "Name": "Test School 1"
                },
                {
                    "Urn": "333333",
                    "Name": "Test School 3"
                }
              ]
        }
        """   
        
    Scenario: Should return 200 response if there are matches for the search within the given scope "All"
        Given Local Authority "100" exists:
        """
        {
            "Name": "Test LA"
        }
        """
        And Establishment "111111" exists:
        """
        {
           "name": "Test School 1",
            "localAuthority": {
              "code": "100"
            } 
        }
        """
        Given Multi Academy Trust "1234" exists:
        """
        {
            "Name": "Test MAT"
        }
        """
        And Establishment "222222" exists:
        """
        {
             "name": "Test School 2"
        }
        """
        And Establishment "333333" exists:
        """
        {
             "name": "Test School 3",
             "diocese": {
                "code": "1000",
                "name": "Test Diocese"
             }
        }
        """
        When I send a GET request to /api/EstablishmentSearch?searchTerm=Test&scope=All
        Then I should get a 200 response
        And the response should be an object containing these properties excluding null:
        """
        {
              "SearchTerm": "Test",
              "Scope": "All",
              "TotalResults": 3,
              "ResultsPerPage": 50,
              "Page": 1,
              "Results": [
                {
                    "Urn": "111111",
                    "Name": "Test School 1"
                },
                {
                    "Urn": "222222",
                    "Name": "Test School 2"
                },
                {
                    "Urn": "333333",
                    "Name": "Test School 3"
                }
              ]
        }
        """            