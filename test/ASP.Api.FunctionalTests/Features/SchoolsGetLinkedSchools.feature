Feature: SchoolsGetLinkedSchools

  Scenario: Should not accept POST method
    When I send a POST request to /api/schools/100001/linked-schools
    Then I should get a 405 response
    And the response should be the message "Method not allowed: The HTTP method POST is not allowed."
    And the response should include the header "Allow: GET"

  Scenario: Should return BadRequest (400) response if urn parameter is empty
    When I send a GET request to /api/schools//linked-schools
    Then I should get a 404 response 
    And the response should be the message "Not found: Function not found for path: /api/schools//linked-schools"

  Scenario Outline: Should return BadRequest (400) response if urn parameter is not 6 characters long
    Given no establishments exist
    When I send a GET request to /api/schools/<urn>/linked-schools
    Then I should get a 400 response
    And the response should be the message "Bad request: The path parameter "urn" must be exactly 6 characters long."

  Examples:
      | urn     |
      | 12345   |
      | 1234567 |

  Scenario: Should return NotFound (404) response if School does not exist
    Given establishment Test School 1 (100001) exists
    When I send a GET request to /api/schools/100002/linked-schools
    Then I should get a 404 response 
    And the response should be the message "Not found: Could not find school with URN "100002"."

  Scenario: Should return empty links response if School exists but has no links
    Given establishment Test School 1 (100001) exists
    When I send a GET request to /api/schools/100001/linked-schools
    Then I should get a 200 response 
    And the response should be an object containing these properties:
    """
    []
    """

  Scenario: Should return no link if linked Establishment does not exist
    Given establishment Test School 1 (100001) exists with properties:
    """
    {
      "links": [
        {
          "linkedUrn": "100002"
        }
      ]
    }
    """
    When I send a GET request to /api/schools/100001/linked-schools
    Then I should get a 200 response 
    And the response should be an object containing these properties:
    """
    []
    """

  Scenario: Should return no link if linked Establishment is deleted
    Given establishment Test School 1 (100001) exists with properties:
    """
    {
      "links": [
        {
          "linkedUrn": "100002"
        }
      ]
    }
    """
    And deleted establishment Test School 2 (100002) exists
    When I send a GET request to /api/schools/100001/linked-schools
    Then I should get a 200 response 
    And the response should be an object containing these properties:
    """
    []
    """

  Scenario: Should return no link if linked Establishment is not visible
    Given establishment Test School 1 (100001) exists with properties:
    """
    {
      "links": [
        {
          "linkedUrn": "100002"
        }
      ]
    }
    """
    And non-visible establishment Test School 2 (100002) exists
    When I send a GET request to /api/schools/100001/linked-schools
    Then I should get a 200 response 
    And the response should be an object containing these properties:
    """
    []
    """

  Scenario: Should provide default description if linkType is missing
    Given establishment Test School 1 (100001) exists with properties:
    """
    {
      "links": [
        {
          "linkedUrn": "100002"
        },
        {
          "linkedUrn": "100003",
          "establishedDate": "2020-03-01"
        }
      ]
    }
    """
    And establishment Test School 2 (100002) exists
    And establishment Test School 3 (100003) exists
    When I send a GET request to /api/schools/100001/linked-schools
    Then I should get a 200 response 
    And the response should be an object containing these properties:
    """
    [
      {
        "Date": null,
        "LinkType": null,
        "Establishments": [
          {
            "Urn": "100002",
            "Name": "Test School 2"
          }
        ],
        "Description": "Test School 1 was linked to [Test School 2](100002)."
      },
      {
        "Date": "2020-03-01",
        "LinkType": null,
        "Establishments": [
          {
            "Urn": "100003",
            "Name": "Test School 3"
          }
        ],
        "Description": "Test School 1 was linked to [Test School 3](100003) on 1 March 2020."
      }
    ]
    """

  Scenario: Descriptions for link to single establishment
    Given establishment Test School 1 (100001) exists with properties:
    """
    {
      "links": [
        {
          "linkedUrn": "100002",
          "establishedDate": <EstablishedDate>,
          "linkType": {
            "code": "<Code>",
            "name": "<Name>"
          }
        }
      ]
    }
    """
    And establishment Test School 2 (100002) exists
    When I send a GET request to /api/schools/100001/linked-schools
    Then I should get a 200 response 
    And the response should be an object containing these properties:
    """
    [
      {
        "Date": <EstablishedDate>,
        "LinkType": {
          "Code": "<Code>",
          "Name": "<Name>"
        },
        "Establishments": [
          {
            "Urn": "100002",
            "Name": "Test School 2"
          }
        ],
        "Description": "<Description>"
      }
    ]
    """

  Examples:
    | Code | Name                                                           | EstablishedDate | Description                                                                                        |
    | 1    | Predecessor                                                    | null            | Test School 1 was previously [Test School 2](100002).                                              |
    | 1    | Predecessor                                                    | "2020-10-01"    | Test School 1 was previously [Test School 2](100002) up until 1 October 2020.                      |
    | 1F   | Predecessor - Split School                                     | null            | Test School 1 was created as the result of a split from [Test School 2](100002).                   |
    | 1F   | Predecessor - Split School                                     | "2020-10-01"    | Test School 1 was created as the result of a split from [Test School 2](100002) on 1 October 2020. |
    | 1I   | Closure                                                        | null            | Test School 1 was previously [Test School 2](100002).                                              |
    | 1I   | Closure                                                        | "2020-10-01"    | Test School 1 was previously [Test School 2](100002), which closed on 1 October 2020.              |
    | 1L   | Predecessor - merged                                           | null            | Test School 1 was merged with [Test School 2](100002).                                             |
    | 1L   | Predecessor - merged                                           | "2020-10-01"    | Test School 1 was merged with [Test School 2](100002) on 1 October 2020.                           |
    | 2    | Successor                                                      | null            | Test School 1 became [Test School 2](100002).                                                      |
    | 2    | Successor                                                      | "2020-10-01"    | Test School 1 became [Test School 2](100002) on 1 October 2020.                                    |
    | 2A   | Expansion                                                      | null            | Test School 1 became [Test School 2](100002).                                                      |
    | 2A   | Expansion                                                      | "2020-10-01"    | Test School 1 became [Test School 2](100002) on 1 October 2020.                                    |
    | 2F   | Successor - Split School                                       | null            | [Test School 2](100002) was split off from Test School 1.                                          |
    | 2F   | Successor - Split School                                       | "2020-10-01"    | [Test School 2](100002) was split off from Test School 1 on 1 October 2020.                        |
    | 2K   | Result of Amalgamation                                         | null            | Test School 1 was the result of an amalgamation of [Test School 2](100002).                        |
    | 2K   | Result of Amalgamation                                         | "2020-10-01"    | Test School 1 was the result of an amalgamation of [Test School 2](100002) on 1 October 2020.      |
    | 2O   | Merged - change in age range                                   | null            | Test School 1 was merged with [Test School 2](100002).                                             |
    | 2O   | Merged - change in age range                                   | "2020-10-01"    | Test School 1 was merged with [Test School 2](100002) on 1 October 2020.                           |
    | 2P   | Merged - expansion of school capacity                          | null            | Test School 1 was merged with [Test School 2](100002).                                             |
    | 2P   | Merged - expansion of school capacity                          | "2020-10-01"    | Test School 1 was merged with [Test School 2](100002) on 1 October 2020.                           |
    | 2Q   | Merged - expansion in school capacity and changer in age range | null            | Test School 1 was merged with [Test School 2](100002).                                             |
    | 2Q   | Merged - expansion in school capacity and changer in age range | "2020-10-01"    | Test School 1 was merged with [Test School 2](100002) on 1 October 2020.                           |
    | 6    | Successor - merged                                             | null            | Test School 1 was merged with [Test School 2](100002).                                             |
    | 6    | Successor - merged                                             | "2020-10-01"    | Test School 1 was merged with [Test School 2](100002) on 1 October 2020.                           |
    | 6.1  | Predecessor - amalgamated                                      | null            | Test School 1 was the result of an amalgamation of [Test School 2](100002).                        |
    | 6.1  | Predecessor - amalgamated                                      | "2020-10-01"    | Test School 1 was the result of an amalgamation of [Test School 2](100002) on 1 October 2020.      |
    | 6.2  | Successor - amalgamated                                        | null            | Test School 1 was amalgamated into [Test School 2](100002).                                        |
    | 6.2  | Successor - amalgamated                                        | "2020-10-01"    | Test School 1 was amalgamated into [Test School 2](100002) on 1 October 2020.                      |

  Scenario: Descriptions for link to multiple establishments
    Given establishment Test School 1 (100001) exists with properties:
    """
    {
      "links": [
        {
          "linkedUrn": "100002",
          "establishedDate": <EstablishedDate>,
          "linkType": {
            "code": "<Code>",
            "name": "<Name>"
          }
        },
        {
          "linkedUrn": "100003",
          "establishedDate": <EstablishedDate>,
          "linkType": {
            "code": "<Code>",
            "name": "<Name>"
          }
        },
        {
          "linkedUrn": "100004",
          "establishedDate": <EstablishedDate>,
          "linkType": {
            "code": "<Code>",
            "name": "<Name>"
          }
        }
      ]
    }
    """
    And establishment Test School 2 (100002) exists
    And establishment Test School 3 (100003) exists
    And establishment Test School 4 (100004) exists
    When I send a GET request to /api/schools/100001/linked-schools
    Then I should get a 200 response 
    And the response should be an object containing these properties:
    """
    [
      {
        "Date": <EstablishedDate>,
        "LinkType": {
          "Code": "<Code>",
          "Name": "<Name>"
        },
        "Establishments": [
          {
            "Urn": "100002",
            "Name": "Test School 2"
          },
          {
            "Urn": "100003",
            "Name": "Test School 3"
          },
          {
            "Urn": "100004",
            "Name": "Test School 4"
          }
        ],
        "Description": "<Description>"
      }
    ]
    """

  Examples:
    | Code | Name                                                           | EstablishedDate | Description                                                                                                                                             |
    | 1    | Predecessor                                                    | null            | Test School 1 was previously [Test School 2](100002), [Test School 3](100003) and [Test School 4](100004).                                              |
    | 1    | Predecessor                                                    | "2020-10-01"    | Test School 1 was previously [Test School 2](100002), [Test School 3](100003) and [Test School 4](100004) up until 1 October 2020.                      |
    | 1F   | Predecessor - Split School                                     | null            | Test School 1 was created as the result of a split from [Test School 2](100002), [Test School 3](100003) and [Test School 4](100004).                   |
    | 1F   | Predecessor - Split School                                     | "2020-10-01"    | Test School 1 was created as the result of a split from [Test School 2](100002), [Test School 3](100003) and [Test School 4](100004) on 1 October 2020. |
    | 1I   | Closure                                                        | null            | Test School 1 was previously [Test School 2](100002), [Test School 3](100003) and [Test School 4](100004).                                              |
    | 1I   | Closure                                                        | "2020-10-01"    | Test School 1 was previously [Test School 2](100002), [Test School 3](100003) and [Test School 4](100004), which closed on 1 October 2020.              |
    | 1L   | Predecessor - merged                                           | null            | Test School 1 was merged with [Test School 2](100002), [Test School 3](100003) and [Test School 4](100004).                                             |
    | 1L   | Predecessor - merged                                           | "2020-10-01"    | Test School 1 was merged with [Test School 2](100002), [Test School 3](100003) and [Test School 4](100004) on 1 October 2020.                           |
    | 2    | Successor                                                      | null            | Test School 1 became [Test School 2](100002), [Test School 3](100003) and [Test School 4](100004).                                                      |
    | 2    | Successor                                                      | "2020-10-01"    | Test School 1 became [Test School 2](100002), [Test School 3](100003) and [Test School 4](100004) on 1 October 2020.                                    |
    | 2A   | Expansion                                                      | null            | Test School 1 became [Test School 2](100002), [Test School 3](100003) and [Test School 4](100004).                                                      |
    | 2A   | Expansion                                                      | "2020-10-01"    | Test School 1 became [Test School 2](100002), [Test School 3](100003) and [Test School 4](100004) on 1 October 2020.                                    |
    | 2F   | Successor - Split School                                       | null            | [Test School 2](100002), [Test School 3](100003) and [Test School 4](100004) were split off from Test School 1.                                         |
    | 2F   | Successor - Split School                                       | "2020-10-01"    | [Test School 2](100002), [Test School 3](100003) and [Test School 4](100004) were split off from Test School 1 on 1 October 2020.                       |
    | 2K   | Result of Amalgamation                                         | null            | Test School 1 was the result of an amalgamation of [Test School 2](100002), [Test School 3](100003) and [Test School 4](100004).                                        |
    | 2K   | Result of Amalgamation                                         | "2020-10-01"    | Test School 1 was the result of an amalgamation of [Test School 2](100002), [Test School 3](100003) and [Test School 4](100004) on 1 October 2020.                      |
    | 2O   | Merged - change in age range                                   | null            | Test School 1 was merged with [Test School 2](100002), [Test School 3](100003) and [Test School 4](100004).                                             |
    | 2O   | Merged - change in age range                                   | "2020-10-01"    | Test School 1 was merged with [Test School 2](100002), [Test School 3](100003) and [Test School 4](100004) on 1 October 2020.                           |
    | 2P   | Merged - expansion of school capacity                          | null            | Test School 1 was merged with [Test School 2](100002), [Test School 3](100003) and [Test School 4](100004).                                             |
    | 2P   | Merged - expansion of school capacity                          | "2020-10-01"    | Test School 1 was merged with [Test School 2](100002), [Test School 3](100003) and [Test School 4](100004) on 1 October 2020.                           |
    | 2Q   | Merged - expansion in school capacity and changer in age range | null            | Test School 1 was merged with [Test School 2](100002), [Test School 3](100003) and [Test School 4](100004).                                             |
    | 2Q   | Merged - expansion in school capacity and changer in age range | "2020-10-01"    | Test School 1 was merged with [Test School 2](100002), [Test School 3](100003) and [Test School 4](100004) on 1 October 2020.                           |
    | 6    | Successor - merged                                             | null            | Test School 1 was merged with [Test School 2](100002), [Test School 3](100003) and [Test School 4](100004).                                             |
    | 6    | Successor - merged                                             | "2020-10-01"    | Test School 1 was merged with [Test School 2](100002), [Test School 3](100003) and [Test School 4](100004) on 1 October 2020.                           |
    | 6.1  | Predecessor - amalgamated                                      | null            | Test School 1 was the result of an amalgamation of [Test School 2](100002), [Test School 3](100003) and [Test School 4](100004).                                        |
    | 6.1  | Predecessor - amalgamated                                      | "2020-10-01"    | Test School 1 was the result of an amalgamation of [Test School 2](100002), [Test School 3](100003) and [Test School 4](100004) on 1 October 2020.                      |
    | 6.2  | Successor - amalgamated                                        | null            | Test School 1 was amalgamated into [Test School 2](100002), [Test School 3](100003) and [Test School 4](100004).                                        |
    | 6.2  | Successor - amalgamated                                        | "2020-10-01"    | Test School 1 was amalgamated into [Test School 2](100002), [Test School 3](100003) and [Test School 4](100004) on 1 October 2020.                      |

  Scenario Outline: 2F - Successor - Split School
    Given establishment Test School 1 (100001) exists with properties:
    """
    {
      "closeDate": <OldSchoolCloseDate>,
      "links": [
        {
          "linkType": {
            "code": "2F",
            "name": "Successor - Split School"
          },
          "linkedUrn": "100002",
          "establishedDate": <SplitDate>
        },
        {
          "linkType": {
            "code": "2F",
            "name": "Successor - Split School"
          },
          "linkedUrn": "100003",
          "establishedDate": <SplitDate>
        }
      ]
    }
    """
    And establishment Test School 2 (100002) exists
    And establishment Test School 3 (100003) exists
    When I send a GET request to /api/schools/100001/linked-schools
    Then I should get a 200 response 
    And the response should be an object containing these properties:
    """
    [
      {
        "Date": <SplitDate>,
        "LinkType": {
          "Code": "2F",
          "Name": "Successor - Split School"
        },
        "Establishments": [
          {
            "Urn": "100002",
            "Name": "Test School 2"
          },
          {
            "Urn": "100003",
            "Name": "Test School 3"
          }
        ],
        "Description": "<Description>"
      }
    ]
    """
    Examples:
      | OldSchoolCloseDate | SplitDate    | Description                                                                                              |
      | null               | null         | [Test School 2](100002) and [Test School 3](100003) were split off from Test School 1.                   |
      | null               | "2020-10-01" | [Test School 2](100002) and [Test School 3](100003) were split off from Test School 1 on 1 October 2020. |
      | "2021-02-01"       | null         | [Test School 2](100002) and [Test School 3](100003) were split off from Test School 1.                   |
      | "2021-02-01"       | "2020-10-01" | [Test School 2](100002) and [Test School 3](100003) were split off from Test School 1 on 1 October 2020. |
      | "2020-10-01"       | "2020-10-01" | Test School 1 was split into [Test School 2](100002) and [Test School 3](100003) on 1 October 2020.      |

  Scenario Outline: 6.1 - Predecessor - amalgamated
    Given establishment Test School 1 (100001) exists with properties:
    """
    {
      "openDate": <NewSchoolOpenDate>,
      "links": [
        {
          "linkType": {
            "code": "6.1",
            "name": "Predecessor - amalgamated"
          },
          "linkedUrn": "100002",
          "establishedDate": <AmalgamateDate>
        },
        {
          "linkType": {
            "code": "6.1",
            "name": "Predecessor - amalgamated"
          },
          "linkedUrn": "100003",
          "establishedDate": <AmalgamateDate>
        }
      ]
    }
    """
    And establishment Test School 2 (100002) exists
    And establishment Test School 3 (100003) exists
    When I send a GET request to /api/schools/100001/linked-schools
    Then I should get a 200 response 
    And the response should be an object containing these properties:
    """
    [
      {
        "Date": <AmalgamateDate>,
        "LinkType": {
          "Code": "6.1",
          "Name": "Predecessor - amalgamated"
        },
        "Establishments": [
          {
            "Urn": "100002",
            "Name": "Test School 2"
          },
          {
            "Urn": "100003",
            "Name": "Test School 3"
          }
        ],
        "Description": "<Description>"
      }
    ]
    """
    Examples:
      | NewSchoolOpenDate | AmalgamateDate | Description                                                                                                               |
      | null              | null           | Test School 1 was the result of an amalgamation of [Test School 2](100002) and [Test School 3](100003).                   |
      | null              | "2020-10-01"   | Test School 1 was the result of an amalgamation of [Test School 2](100002) and [Test School 3](100003) on 1 October 2020. |
      | "2019-02-01"      | null           | Test School 1 was the result of an amalgamation of [Test School 2](100002) and [Test School 3](100003).                   |
      | "2019-02-01"      | "2020-10-01"   | Test School 1 was the result of an amalgamation of [Test School 2](100002) and [Test School 3](100003) on 1 October 2020. |
      | "2020-10-01"      | "2020-10-01"   | Test School 1 was the result of an amalgamation of [Test School 2](100002) and [Test School 3](100003) on 1 October 2020. |
      | "2021-01-01"      | "2020-10-01"   | Test School 1 was the result of an amalgamation of [Test School 2](100002) and [Test School 3](100003) on 1 October 2020. |
