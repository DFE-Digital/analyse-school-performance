Feature: Establishment Search

  Scenario: Should not accept POST method
    When I send a POST request to /EstablishmentSearch
    Then I should get a 405 response
    And the response should be the message "Method not allowed: The HTTP method POST is not allowed."
    And the response should include the header "Allow: GET"

  Scenario: Should return BadRequest (400) response if searchTerm parameter is missing
    When I send a GET request to /EstablishmentSearch
    Then I should get a 400 response
    And the response should be the message "Invalid: The parameter "searchTerm" is missing."

  Scenario: Should return BadRequest (400) response if searchTerm parameter is empty string
    When I send a GET request to /EstablishmentSearch?searchTerm=
    Then I should get a 400 response
    And the response should be the message "Invalid: The parameter "searchTerm" should not be empty."

  Scenario Outline: Should return BadRequest (400) response if page parameter is not a whole number greater than 1
    When I send a GET request to /EstablishmentSearch?searchTerm=x&page=<page>
    Then I should get a 400 response
    And the response should be the message "Invalid: Bad request: parameter "page" should be a whole number greater than 1."

    Examples:
      | page |
      | y    |
      | 1.5  |
      | 0    |
      | -1   |

  Scenario Outline: Should return BadRequest (400) response if resultsPerPage parameter is not a whole number greater than 1
    When I send a GET request to /EstablishmentSearch?searchTerm=x&resultsPerPage=<resultsPerPage>
    Then I should get a 400 response
    And the response should be the message "Invalid: Bad request: parameter "resultsPerPage" should be a whole number greater than 1."

    Examples:
      | resultsPerPage |
      | y              |
      | 1.5            |
      | 0              |
      | -1             |


  Scenario: Should return NotFound (404) response if no matches found for the searchTerm
    Given no establishments exist
    When I send a GET request to /EstablishmentSearch?searchTerm=x
    Then I should get a 404 response
    And the response should be the message "Not found: there were no matches for "x"."

  Scenario: Should return NotFound (404) response if there were no relevant matches for the given searchTerm
    Given  establishment "111111" exists: 
           """
            {
              "Urn": "111111",
              "Name": "Some Primary School"
            }
           """
    When I send a GET request to /EstablishmentSearch?searchTerm=secondary
    Then I should get a 404 response
    And the response should be the message "Not found: there were no matches for "secondary"."

  Scenario: Should return a NotFound (404) response if the requested establishment has been deleted for the given searchTerm
    Given establishment "222222" exists: 
           """
            {
              "Urn": "222222",
              "IsDeleted": true,
              "Name": "Some Primary School"
            }
           """
    When I send a GET request to /EstablishmentSearch?searchTerm=222222
    Then I should get a 404 response
    And the response should be the message "Not found: The requested establishment with URN "222222" has been deleted."

  Scenario: Should return a NotFound (404) response if the requested establishment is not currently visible for the given searchTerm
    Given non visible establishment "111111" exists: 
           """
            {
              "Urn": "111111",
              "Name": "Some Primary School"
            }
           """
    When I send a GET request to /EstablishmentSearch?searchTerm=111111
    Then I should get a 404 response
    And the response should be the message "Not found: The requested establishment with URN "111111" is not currently visible."

  Scenario Outline: Should return 200 response with search results when matches are found for the given searchTerm
    Given establishment "111111" exists: 
             """
              {
                "Urn": "111111",
                "Name": "Some Primary School"
              }
             """
    When I send a GET request to /EstablishmentSearch?searchTerm=<searchTerm>
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
    Given establishment "111111" exists: 
             """
              {
                 "Urn": "111111", 
                 "Name": "Some Primary School",
                  "Address": {
                        "Street": "13 The Street",
                        "Town": "SomeTown",
                        "PostCode": "TR18 3JT"
                    } 
              }
             """
    When I send a GET request to /EstablishmentSearch?searchTerm=<searchTerm>
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
                      "Address": "13 The Street, SomeTown, TR18 3JT"
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
    Given establishment "111111" exists: 
             """
              {
                 "Urn": "111111", 
                 "Name": "Some Primary School",
                 "Address": {
                        "Street": "13 The Street",
                        "Town": "SomeTown",
                        "PostCode": "B1 1AA"
                 } 
              }
             """
    And establishment "222222" exists: 
             """
              {
                 "Urn": "222222", 
                 "Name": "Some Other Primary School",
                 "Address": {
                        "Street": "13 The Road",
                        "Town": "Tring",
                        "PostCode": "B1 1AA"
                    } 
              }
             """
    And establishment "333333" exists: 
             """
              {
                 "Urn": "333333", 
                 "Name": "A Different Primary School",
                  "Address": {
                        "Street": "13 The Road",
                        "Town": "SomeTown",
                        "PostCode": "TR18 1AA"
                    } 
              }
             """
    And establishment "444444" exists: 
             """
              {
                 "Urn": "444444",
                 "Name": "The Training Center"
              }
             """
    When I send a GET request to /EstablishmentSearch?searchTerm=tr
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
                       "Address": "13 The Street, SomeTown, TR18 1AA"
                   },
                   {
                      "Urn": "222222", 
                      "Name": "Some Other Primary School",
                      "Address": "13 The Road, Tring, B1 1AA"
                  },
                  {
                    "Urn": "111111", 
                    "Name": "Some Primary School",
                    "Address": "13 The Street, SomeTown, B1 1AA"
                  },
                  {
                    "Urn": "444444", 
                    "Name": "The Training Center"
                  }
              ]
          }

        """

  Scenario: Should return 200 response with search results when searchTerm matching establishment URN
    Given  establishment "111111" exists: 
           """
            {
              "Urn": "111111",
              "Name": "Some Primary School"
            }
           """
    When I send a GET request to /EstablishmentSearch?searchTerm=111111
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
    Given establishment "111111" exists: 
           """
            {
              "Urn": "111111",
              "Name": "Some Primary School"
            }
           """
    When I send a GET request to /EstablishmentSearch?searchTerm=11
    Then I should get a 404 response
    And the response should be the message "Not found: there were no matches for "11"."

  Scenario: Should return 200 response with search results when searchTerm matches partially with the address street name
    Given establishment "111111" exists: 
           """
            {
              "Urn": "111111",
              "Name": "Some Primary School"
            }
           """
    And establishment "222222" exists: 
             """
              {
                 "Urn": "222222", 
                 "Name": "A Different Primary School",
                  "Address": {
                        "Street": "111 The Street",
                        "Town": "SomeTown",
                        "PostCode": "TR18 3JT"
                    } 
              }
             """
    When I send a GET request to /EstablishmentSearch?searchTerm=11
    Then I should get a 200 response
    And  the response should be an object containing these properties excluding null:
        """
          {
              "SearchTerm": "11",
              "TotalResults": 1,
              "ResultsPerPage": 50,
              "Page": 1,
              "Results": [
                  {
                      "Urn": "222222", 
                      "Name": "Some Primary School",
                      "Address": "111 The Street, SomeTown, TR18 3JT"
                  }
              ]
          }
    
        """

  Scenario: Should return 200 response with search results when searchTerm is 6 digit number treat it as an exact URN search
    Given establishment "111111" exists: 
           """
            {
              "Urn": "111111",
              "Name": "Some Primary School"
            }
           """
    And establishment "222222" exists: 
             """
              {
                 "Urn": "222222", 
                 "Name": "A Different Primary School",
                  "Address": {
                        "Street": "111111 The Street",
                        "Town": "SomeTown",
                        "PostCode": "TR18 3JT"
                    } 
              }
             """
    When I send a GET request to /EstablishmentSearch?searchTerm=111111
    Then I should get a 200 response
    And  the response should be an object containing these properties excluding null:
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
    Given establishment "111111" exists: 
           """
            {
              "Urn": "111111",
              "Name": "Some Primary School",
              "Laestab": "894/2200",
            }
            """
    When I send a GET request to /EstablishmentSearch?searchTerm=<searchTerm>
    Then I should get a 200 response
    And  the response should be an object containing these properties excluding null:
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
    Given establishment "111111" exists: 
           """
            {
              "Urn": "111111",
              "Laestab": "894/2200",
              "Name": "Some Primary School"
            }
           """
    And establishment "222222" exists: 
             """
              {
                 "Urn": "222222", 
                 "Laestab": "894/1234",
                 "Name": "Some Other Primary School"
              }
             """
    When I send a GET request to /EstablishmentSearch?searchTerm=894
    Then I should get a 200 response
    And  the response should be an object containing these properties excluding null:
        """
          {
              "SearchTerm": "894",
              "TotalResults": 2,
              "ResultsPerPage": 50,
              "Page": 1,
              "Results": [
                  { 
                      "Urn": "111111", 
                      "Laestab": "894/2200",
                      "Name": "Some Primary School" 
                  },
                  { 
                      "Urn": "222222", 
                      "Laestab": "894/1234",
                      "Name": "Some Other Primary School" 
                  }
              ]
          }    
    
        """

  Scenario: Should return 200 response with search results when searchTerm matches with LAESTAB 4 digit code partially
    Given establishment "111111" exists: 
           """
            {
              "Urn": "111111",
              "Laestab": "894/2200",
              "Name": "Some Primary School"
            }
           """
    And establishment "222222" exists: 
             """
              {
                 "Urn": "222222", 
                 "Laestab": "123/2200",
                 "Name": "Some Other Primary School"
              }
             """
    When I send a GET request to /EstablishmentSearch?searchTerm=2200
    Then I should get a 200 response
    And  the response should be an object containing these properties excluding null:
        """
          {
              "SearchTerm": "2200",
              "TotalResults": 2,
              "ResultsPerPage": 50,
              "Page": 1,
              "Results": [
                  { 
                      "Urn": "111111", 
                      "Laestab": "894/2200",
                      "Name": "Some Primary School" 
                  },
                  { 
                      "Urn": "222222", 
                      "Laestab": "123/2200",
                      "Name": "Some Other Primary School" 
                  }
              ]
          }    
    
        """

  Scenario Outline: Should return NotFound (404) response if there were no relevant matches for the given searchTerm associated with a LAESTAB code
    Given establishment "111111" exists: 
           """
            {
              "Urn": "111111",
              "Laestab": "894/2200",
              "Name": "Some Primary School"
            }
           """
    When I send a GET request to /EstablishmentSearch?searchTerm=<searchTerm>
    Then I should get a 404 response
    And the response should be the message "Not found: there were no matches for "<searchTerm>"."

    Examples:
      | searchTerm |
      | 89         |
      | 22         |

  Scenario: Should return 200 response with search results when searchTerm matching establishment 7 digits LAESTAB code ignoring other matching fields
    Given establishment "111111" exists: 
           """
            {
              "Urn": "111111",
              "Name": "Some Primary School",
              "Laestab": "894/2200"
            }
            """
    And establishment "222222" exists: 
             """
              {
                 "Urn": "222222", 
                 "Name": "Some Other Primary School",
                 "Address": {
                    "Street": "8942200 The Street",
                    "Town": "SomeTown", 
                    "PostCode": "TR18 3JT"
                  }  
              }
             """
    When I send a GET request to /EstablishmentSearch?searchTerm=8942200
    Then I should get a 200 response
    And  the response should be an object containing these properties excluding null:
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
    Given establishment "111111" exists: 
           """
            {
              "Urn": "111111",
              "Name": "Some Primary School",
              "Laestab": "894/2200"
            }
            """
    And establishment "222222" exists: 
             """
              {
                 "Urn": "222222",
                 "Name": "Some other Primary School",
                 "Address": {
                    "Street": "894/2200 The Street",
                    "Town": "SomeTown", 
                    "PostCode": "TR18 3JT"
                  }  
              }
             """
    When I send a GET request to /EstablishmentSearch?searchTerm=894/2200
    Then I should get a 200 response
    And  the response should be an object containing these properties excluding null:
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
    Given establishment "111111" exists: 
           """
            {
              "Urn": "111111",
              "Name": "Some Primary School",
              "Laestab": "894/2200"
            }
            """
    And establishment "222222" exists: 
             """
              {
                 "Urn": "222222",
                 "Name": "Some other Primary School",
                 "Address": {
                    "Street": "894 The Street",
                    "Town": "SomeTown", 
                    "PostCode": "TR18 3JT"
                  }  
              }
             """
    When I send a GET request to /EstablishmentSearch?searchTerm=894
    Then I should get a 200 response
    And  the response should be an object containing these properties excluding null:
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
    Given establishment "111111" exists: 
           """
            {
              "Urn": "111111",
              "Name": "Some Primary School",
              "Laestab": "894/2200"
            }
            """
    And establishment "222222" exists: 
             """
              {
                 "Urn": "222222",
                 "Name": "Some other Primary School",
                 "Address": {
                    "Street": "2200 The Street",
                    "Town": "SomeTown", 
                    "PostCode": "TR18 3JT"
                  }  
              }
             """
    When I send a GET request to /EstablishmentSearch?searchTerm=2200
    Then I should get a 200 response
    And  the response should be an object containing these properties excluding null:
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
    Given establishment "111111" exists: 
           """
            {
              "Urn": "111111",
              "Name": "Primary School 111111"
            }
           """
    And establishment "222222" exists: 
             """
              {
                 "Urn": "222222", 
                 "Name": "Primary School 222222"
              }
             """
    And establishment "333333" exists: 
             """
              {
                 "Urn": "333333", 
                 "Name": "Primary School 333333"
              }
             """
    When I send a GET request to /EstablishmentSearch?searchTerm=primary
    Then I should get a 200 response
    And  the response should be an object containing these properties excluding null:
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
    Given establishment "111111" exists: 
           """
            {
              "Urn": "111111",
              "Name": "Primary School 111111"
            }
           """
    And establishment "222222" exists: 
             """
              {
                 "Urn": "222222", 
                 "Name": "Primary School 222222"
              }
             """
    And establishment "333333" exists: 
             """
              {
                 "Urn": "333333", 
                 "Name": "Primary School 333333"
              }
             """
    When I send a GET request to /EstablishmentSearch?searchTerm=primary&resultsPerPage=2
    Then I should get a 200 response
    And  the response should be an object containing these properties excluding null:
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
    Given establishment "111111" exists: 
           """
            {
              "Urn": "111111",
              "Name": "Primary School 111111"
            }
           """
    And establishment "222222" exists: 
             """
              {
                 "Urn": "222222", 
                 "Name": "Primary School 222222"
              }
             """
    And establishment "333333" exists: 
             """
              {
                 "Urn": "333333", 
                 "Name": "Primary School 333333"
              }
             """
    When I send a GET request to /EstablishmentSearch?searchTerm=primary&resultsPerPage=2&page=2
    Then I should get a 200 response
    And  the response should be an object containing these properties excluding null:
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
    Given establishment "111111" exists: 
           """
            {
              "Urn": "111111",
              "Name": "Primary School 111111"
            }
           """
    And establishment "222222" exists: 
             """
              {
                 "Urn": "222222", 
                 "Name": "Primary School 222222"
              }
             """
    And establishment "333333" exists: 
             """
              {
                 "Urn": "333333", 
                 "Name": "Primary School 333333"
              }
             """
    When I send a GET request to /EstablishmentSearch?searchTerm=primary&resultsPerPage=2&page=3
    Then I should get a 200 response
    And  the response should be an object containing these properties excluding null:
        """
         {
              "SearchTerm": "primary",
              "TotalResults": 3,
              "ResultsPerPage": 2,
              "Page": 3,
              "Results": [
                
              ]
          }   
    
        """

  Scenario: Should return a 200 response with search results and a computed address field when the searchTerm matches the URN and given address has street, town and postcode
    Given establishment "111111" exists: 
           """
            {
              "Urn": "111111",
              "Name": "Some Primary School",
              "Address": {
                 "Street": "13 The Street",
                 "Town": "SomeTown",
                 "PostCode": "AB12 3CD"
              } 
            }
           """
    When I send a GET request to /EstablishmentSearch?searchTerm=111111
    Then I should get a 200 response
    And  the response should be an object containing these properties excluding null:
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
    Given establishment "111111" exists: 
           """
            {
              "Urn": "111111",
              "Name": "Some Primary School",
              "Address": {
                 "Street": "13 The Street",
                 "PostCode": "AB12 3CD"
              } 
            }
           """
    When I send a GET request to /EstablishmentSearch?searchTerm=111111
    Then I should get a 200 response
    And  the response should be an object containing these properties excluding null:
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
    Given establishment "111111" exists: 
           """
            {
              "Urn": "111111",
              "Name": "Some Primary School",
              "Address": {
                 "Street": "13 The Street",
                  "Town": "SomeTown"
              } 
            }
           """
    When I send a GET request to /EstablishmentSearch?searchTerm=111111
    Then I should get a 200 response
    And  the response should be an object containing these properties excluding null:
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
                      "Address": "13 The Street SomeTown"
                  }
              ]
          }
    
        """

  Scenario: Should return a 200 response with search results and a computed educationPhase field when the searchTerm matches the URN and given educationPhase isPrimary equal true
    Given establishment "111111" exists: 
           """
            {
              "Urn": "111111",
              "Name": "Some Primary School",
              "IsPost16": false,
              "IsPrimary": true,
              "IsSecondary": false
            }
           """
    When I send a GET request to /EstablishmentSearch?searchTerm=111111
    Then I should get a 200 response
    And  the response should be an object containing these properties excluding null:
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
    Given establishment "111111" exists: 
           """
            {
              "Urn": "111111",
              "Name": "Some Primary School",
              "IsPost16": false,
              "IsPrimary": false,
              "IsSecondary": true
            }
           """
    When I send a GET request to /EstablishmentSearch?searchTerm=111111
    Then I should get a 200 response
    And  the response should be an object containing these properties excluding null:
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
    Given establishment "111111" exists: 
           """
            {
              "Urn": "111111",
              "Name": "Some Primary School",
              "IsPost16": true,
              "IsPrimary": false,
              "IsSecondary": false
            }
           """
    When I send a GET request to /EstablishmentSearch?searchTerm=111111
    Then I should get a 200 response
    And  the response should be an object containing these properties excluding null:
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
    Given establishment "111111" exists: 
           """
            {
              "Urn": "111111",
              "Name": "Some Primary School",
              "OfstedLastInspectionDate": "2013-03-22T00:00:00",
              "OfstedRating": {
                "Code": "2",
                "Name": "Good"
               }
            }
           """
    When I send a GET request to /EstablishmentSearch?searchTerm=111111
    Then I should get a 200 response
    And  the response should be an object containing these properties excluding null:
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
    