Feature: SchoolsGetAll

Scenario: Should not accept POST method
	When I send a POST request to /api/schools
	Then I should get a 405 response
	And the response should be the message "Method not allowed: The HTTP method POST is not allowed."
	And the response should include the header "Allow: GET"

Scenario: Should return BadRequest (400) response if scope parameter is empty string
	When I send a GET request to /api/schools?scope=
	Then I should get a 400 response
	And the response should be the message "Bad request: The query parameter "scope" should not be empty."

Scenario: Should return BadRequest (400) response if scope parameter is invalid
	When I send a GET request to /api/schools?scope=xyz
	Then I should get a 400 response
	And the response should be the message "Bad request: "xyz" is not a valid scope."

Scenario Outline: Should return BadRequest (400) response if scopeId parameter is missing
	When I send a GET request to /api/schools?scope=<Scope>
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

Scenario Outline: Should return BadRequest (400) response if page parameter is not a whole number greater than or equal to 1
	When I send a GET request to /api/schools?page=<page>
	Then I should get a 400 response
	And the response should be the message "Bad request: The query parameter "page" should be a whole number greater than or equal to 1."

Examples:
	| page |
	| y    |
	| 1.5  |
	| 0    |
	| -1   |

Scenario: Should allow page = 1
	Given Establishment "111111" exists:
		"""
		{
			"name": "Some Primary School"
		}
		"""
	When I send a GET request to /api/schools?page=1
	Then I should get a 200 response
	And the response should be an object containing these properties:
		"""
		{
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

Scenario Outline: Should return BadRequest (400) response if resultsPerPage parameter is not a whole number greater than 1
	When I send a GET request to /api/schools?resultsPerPage=<resultsPerPage>
	Then I should get a 400 response
	And the response should be the message "Bad request: The query parameter "resultsPerPage" should be a whole number greater than or equal to 1."

Examples:
	| resultsPerPage |
	| y              |
	| 1.5            |
	| 0              |
	| -1             |

Scenario: Should allow resultsPerPage = 1
	Given Establishment "111111" exists:
		"""
		{
			"name": "Some Primary School"
		}
		"""
	When I send a GET request to /api/schools?resultsPerPage=1
	Then I should get a 200 response
	And the response should be an object containing these properties:
		"""
		{
			"TotalResults": 1,
			"ResultsPerPage": 1,
			"Page": 1,
			"Results": [
				{
					"Urn": "111111",
					"Name": "Some Primary School"
				}
			]
		}
		"""

Scenario: Should return NotFound (404) response if there were no establishments exist within the given scope
	Given no Establishments exist
	When I send a GET request to /api/schools
	Then I should get a 404 response
	And the response should be the message "Not found: There were no schools within the given scope."

Scenario: Should not return deleted establishments
	Given deleted Establishment "222222" exists:
		"""
		{
			"name": "Some Primary School"
		}
		"""
	When I send a GET request to /api/schools
	Then I should get a 404 response
	And the response should be the message "Not found: There were no schools within the given scope."

Scenario: Should not return non-visible establishments
	Given non-visible Establishment "111111" exists:
		"""
		{
			"name": "Some Primary School"
		}
		"""
	When I send a GET request to /api/schools
	Then I should get a 404 response
	And the response should be the message "Not found: There were no schools within the given scope."

Scenario: Should return 200 response when establishments exist
	Given Establishment "111111" exists:
		"""
		{
			"name": "Some Primary School"
		}
		"""
	When I send a GET request to /api/schools
	Then I should get a 200 response
	And the response should be an object containing these properties:
		"""
		{
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

Scenario: Should return BadRequest (400) response if Local Authority with code does not exist
	When I send a GET request to /api/schools?scope=LA&scopeId=100
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
	When I send a GET request to /api/schools?scope=LA&scopeId=100
	Then I should get a 404 response
	And the response should be the message "Not found: There were no schools within the given scope."

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
	When I send a GET request to /api/schools?scope=LA&scopeId=100
	Then I should get a 200 response
	And the response should be an object containing these properties excluding null:
		"""
		{
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
	When I send a GET request to /api/schools?scope=MAT&scopeId=1234
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
	When I send a GET request to /api/schools?scope=MAT&scopeId=1234
	Then I should get a 404 response
	And the response should be the message "Not found: There were no schools within the given scope."

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
	When I send a GET request to /api/schools?scope=MAT&scopeId=1234
	Then I should get a 200 response
	And the response should be an object containing these properties excluding null:
		"""
		{
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

Scenario: Should return NotFound (404) response if there are no matches for Diocese scope
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

	When I send a GET request to /api/schools?scope=Diocese&scopeId=Test%20Diocese
	Then I should get a 404 response
	And the response should be the message "Not found: There were no schools within the given scope."

Scenario: Should return 200 response if there are matches for the given scope "Diocese"
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
	When I send a GET request to /api/schools?scope=Diocese&scopeId=Test%20Diocese
	Then I should get a 200 response
	And the response should be an object containing these properties excluding null:
		"""
		{
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

Scenario: Should return 200 response if there are matches for the given scope "All"
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
	When I send a GET request to /api/schools
	Then I should get a 200 response
	And the response should be an object containing these properties excluding null:
		"""
		{
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

Scenario: Should return a 200 response with results and expected pagination for the given scope
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
	When I send a GET request to /api/schools
	Then I should get a 200 response
	And the response should be an object containing these properties excluding null:
		"""
		{
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

Scenario: Should return a 200 response with results and expected pagination for the given resultsPerPage
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
	When I send a GET request to /api/schools?resultsPerPage=2
	Then I should get a 200 response
	And the response should be an object containing these properties excluding null:
		"""
		{
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

Scenario: Should return a 200 response with results and expected pagination for the given resultsPerPage and page
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
	When I send a GET request to /api/schools?resultsPerPage=2&page=2
	Then I should get a 200 response
	And the response should be an object containing these properties excluding null:
		"""
		{
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

Scenario: Should return a 200 response with expected pagination and no results for the given resultsPerPage, and page
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
	When I send a GET request to /api/schools?resultsPerPage=2&page=3
	Then I should get a 200 response
	And the response should be an object containing these properties excluding null:
		"""
		{
			"TotalResults": 3,
			"ResultsPerPage": 2,
			"Page": 3,
			"Results": [
			]
		}
		"""

Scenario: Should return a 200 response with results and a computed address field
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
	When I send a GET request to /api/schools
	Then I should get a 200 response
	And the response should be an object containing these properties excluding null:
		"""
		{
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

Scenario: Should return a 200 response with results and a computed address field when the URN and given address has street and postcode
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
	When I send a GET request to /api/schools
	Then I should get a 200 response
	And the response should be an object containing these properties excluding null:
		"""
		{
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

Scenario: Should return a 200 response with results and a computed address field when the URN and given address has street and town
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
	When I send a GET request to /api/schools
	Then I should get a 200 response
	And the response should be an object containing these properties excluding null:
		"""
		{
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

Scenario: Should return a 200 response with results and a computed educationPhase field when the URN and given educationPhase isPrimary equal true
	Given Establishment "111111" exists:
		"""
		{
			"name": "Some Primary School",
			"isPost16": false,
			"isPrimary": true,
			"isSecondary": false
		}
		"""
	When I send a GET request to /api/schools
	Then I should get a 200 response
	And the response should be an object containing these properties excluding null:
		"""
		{
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

Scenario: Should return a 200 response with results and a computed educationPhase field when the URN and given educationPhase isSecondary equal true
	Given Establishment "111111" exists:
		"""
		{
			"name": "Some Primary School",
			"isPost16": false,
			"isPrimary": false,
			"isSecondary": true
		}
		"""
	When I send a GET request to /api/schools
	Then I should get a 200 response
	And the response should be an object containing these properties excluding null:
		"""
		{
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

Scenario: Should return a 200 response with results and a computed educationPhase field when the URN and given educationPhase isPost16 equal true
	Given Establishment "111111" exists:
		"""
		{
			"name": "Some Primary School",
			"isPost16": true,
			"isPrimary": false,
			"isSecondary": false
		}
		"""
	When I send a GET request to /api/schools
	Then I should get a 200 response
	And the response should be an object containing these properties excluding null:
		"""
		{
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