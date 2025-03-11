Feature: SchoolsGetAll with searchTerm

Scenario: Should not accept POST method
	When I send a POST request to /api/schools
	Then I should get a 405 response
	And the response should be the message "Method not allowed: The HTTP method POST is not allowed."
	And the response should include the header "Allow: GET"

Scenario: Should return BadRequest (400) response if searchTerm parameter is empty string
	When I send a GET request to /api/schools?searchTerm=
	Then I should get a 400 response
	And the response should be the message "Bad request: The query parameter "searchTerm" should not be empty."

Scenario Outline: Should return BadRequest (400) response if page parameter is not a whole number greater than or equal to 1
	When I send a GET request to /api/schools?searchTerm=x&page=<page>
	Then I should get a 400 response
	And the response should be the message "Bad request: The query parameter "page" should be a whole number greater than or equal to 1."

Examples:
	| page |
	| y    |
	| 1.5  |
	| 0    |
	| -1   |

Scenario Outline: Should return BadRequest (400) response if resultsPerPage parameter is not a whole number greater than 1
	When I send a GET request to /api/schools?searchTerm=x&resultsPerPage=<resultsPerPage>
	Then I should get a 400 response
	And the response should be the message "Bad request: The query parameter "resultsPerPage" should be a whole number greater than or equal to 1."

Examples:
	| resultsPerPage |
	| y              |
	| 1.5            |
	| 0              |
	| -1             |

Scenario: Should return BadRequest (400) response if scope parameter is empty string
	When I send a GET request to /api/schools?searchTerm=xyz&scope=
	Then I should get a 400 response
	And the response should be the message "Bad request: The query parameter "scope" should not be empty."

Scenario: Should return BadRequest (400) response if scope parameter is invalid
	When I send a GET request to /api/schools?searchTerm=xyz&scope=xyz
	Then I should get a 400 response
	And the response should be the message "Bad request: "xyz" is not a valid scope."

Scenario Outline: Should return BadRequest (400) response if scopeId parameter is missing
	When I send a GET request to /api/schools?searchTerm=xyz&scope=<Scope>
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

Scenario: Should return NotFound (404) response if no matches found for the searchTerm
	Given no establishments exist
	When I send a GET request to /api/schools?searchTerm=x
	Then I should get a 404 response
	And the response should be the message "Not found: There were no matches for "x" within the given scope."

Scenario: Should return NotFound (404) response if there were no relevant matches for the given searchTerm
	Given establishment Some Primary School (111111) exists
	When I send a GET request to /api/schools?searchTerm=secondary
	Then I should get a 404 response
	And the response should be the message "Not found: There were no matches for "secondary" within the given scope."

Scenario: Should return a NotFound (404) response if the requested establishment has been deleted for the given searchTerm
	Given deleted establishment Some Primary School (222222) exists
	When I send a GET request to /api/schools?searchTerm=222222
	Then I should get a 404 response
	And the response should be the message "Not found: There were no matches for "222222" within the given scope."

Scenario: Should return a NotFound (404) response if the requested establishment is not currently visible for the given searchTerm
	Given non-visible establishment Some Primary School (111111) exists
	When I send a GET request to /api/schools?searchTerm=111111
	Then I should get a 404 response
	And the response should be the message "Not found: There were no matches for "111111" within the given scope."

Scenario: Should not return 400 response if page = 1
	Given establishment Some Primary School (111111) exists
	When I send a GET request to /api/schools?searchTerm=111111&page=1
	Then I should get a 200 response

Scenario: Should not return 400 response if resultsPerPage = 1
	Given establishment Some Primary School (111111) exists
	When I send a GET request to /api/schools?searchTerm=111111&resultsPerPage=1
	Then I should get a 200 response

Scenario Outline: Should return 200 response with search results when matches are found for the given searchTerm
	Given establishment Some Primary School (111111) exists
	When I send a GET request to /api/schools?searchTerm=<searchTerm>
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

Examples:
	| searchTerm |
	| Prim       |
	| PRiMaRY    |

Scenario Outline: Should return 200 response with search results when searchTerm matching establishment street, town and postcode partially
	Given establishment Some Primary School (111111) exists with properties:
		"""
		{
			"address": {
				"street": "13 The Street",
				"town": "SomeTown",
				"postCode": "TR18 3JT"
			} 
		}
		"""
	When I send a GET request to /api/schools?searchTerm=<searchTerm>
	Then I should get a 200 response
	And the response should be an object containing these properties (ignoring null values):
		"""
		{
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
	Given establishment Some Primary School (111111) exists with properties:
		"""
		{
			"address": {
				"street": "13 The Street",
				"town": "SomeTown",
				"postCode": "B1 1AA"
			} 
		}
		"""
	And establishment Some Other Primary School (222222) exists with properties:
		"""
		{
			"address": {
				"street": "13 The Road",
				"town": "Tring",
				"postCode": "B1 1AA"
			} 
		}
		"""
	And establishment A Different Primary School (333333) exists with properties:
		"""
		{
			"address": {
				"street": "13 The Road",
				"town": "SomeTown",
				"postCode": "TR18 1AA"
			} 
		}
		"""
	And establishment The Training Center (444444) exists
	When I send a GET request to /api/schools?searchTerm=tr
	Then I should get a 200 response
	And the response should be an object containing these properties (ignoring null values):
		"""
		{
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
	Given establishment Some Primary School (111111) exists
	When I send a GET request to /api/schools?searchTerm=111111
	Then I should get a 200 response
	And the response should be an object containing these properties (ignoring null values):
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

Scenario: Should return a NotFound (404) response if no relevant matches are found for the establishment URN based on the given searchTerm
	Given establishment Some Primary School (111111) exists
	When I send a GET request to /api/schools?searchTerm=11
	Then I should get a 404 response
	And the response should be the message "Not found: There were no matches for "11" within the given scope."

Scenario: Should return 200 response with search results when searchTerm matches partially with the address street name
	Given establishment Some Primary School (111111) exists
	And establishment A Different Primary School (222222) exists with properties:
		"""
		{
			"address": {
				"street": "111 The Street",
				"town": "SomeTown",
				"postCode": "TR18 3JT"
			} 
		}
		"""
	When I send a GET request to /api/schools?searchTerm=11
	Then I should get a 200 response
	And the response should be an object containing these properties (ignoring null values):
		"""
		{
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
	Given establishment Some Primary School (111111) exists
	And establishment A Different Primary School (222222) exists with properties:
		"""
		{
			"address": {
				"street": "111111 The Street",
				"town": "SomeTown",
				"postCode": "TR18 3JT"
			} 
		}
		"""
	When I send a GET request to /api/schools?searchTerm=111111
	Then I should get a 200 response
	And the response should be an object containing these properties (ignoring null values):
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

Scenario Outline: Should return 200 response with search results when searchTerm matching establishment LAESTAB code (with and without forward slash)
	Given establishment Some Primary School (111111) exists with LAESTAB code 894/2200
	When I send a GET request to /api/schools?searchTerm=<searchTerm>
	Then I should get a 200 response
	And the response should be an object containing these properties (ignoring null values):
		"""
		{
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
	| 894%2F2200 |
	| 8942200    |

Scenario: Should return 200 response with search results when searchTerm matches with LAESTAB 3 digit code partially
	Given establishment Some Primary School (111111) exists with LAESTAB code 894/2200
	And establishment Some Other Primary School (222222) exists with LAESTAB code 894/1234
	When I send a GET request to /api/schools?searchTerm=894
	Then I should get a 200 response
	And the response should be an object containing these properties (ignoring null values):
		"""
		{
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
	Given establishment Some Primary School (111111) exists with LAESTAB code 894/2200
	And establishment Some Other Primary School (222222) exists with LAESTAB code 123/2200
	When I send a GET request to /api/schools?searchTerm=2200
	Then I should get a 200 response
	And the response should be an object containing these properties (ignoring null values):
		"""
		{
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
	Given establishment Some Primary School (111111) exists with LAESTAB code 894/2200
	When I send a GET request to /api/schools?searchTerm=<searchTerm>
	Then I should get a 404 response
	And the response should be the message "Not found: There were no matches for "<searchTerm>" within the given scope."

Examples:
	| searchTerm |
	| 89         |
	| 22         |

Scenario: Should return 200 response with search results when searchTerm matching establishment 7 digits LAESTAB code ignoring other matching fields
	Given establishment Some Primary School (111111) exists with LAESTAB code 894/2200
	And establishment Some Other Primary School (222222) exists with properties:
		"""
		{
			"address": {
				"street": "8942200 The Street",
				"town": "SomeTown", 
				"postCode": "TR18 3JT"
			}
		}
		"""
	When I send a GET request to /api/schools?searchTerm=8942200
	Then I should get a 200 response
	And the response should be an object containing these properties (ignoring null values):
		"""
		{
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
	Given establishment Some Primary School (111111) exists with LAESTAB code 894/2200
	And establishment Some other Primary School (222222) exists with properties:
		"""
		{
			"address": {
				"street": "894/2200 The Street",
				"town": "SomeTown", 
				"postCode": "TR18 3JT"
			}  
		}
		"""
	When I send a GET request to /api/schools?searchTerm=894%2F2200
	Then I should get a 200 response
	And the response should be an object containing these properties (ignoring null values):
		"""
		{
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
	Given establishment Some Primary School (111111) exists with LAESTAB code 894/2200
	And establishment Some other Primary School (222222) exists with properties:
		"""
		{
			"address": {
				"street": "894 The Street",
				"town": "SomeTown", 
				"postCode": "TR18 3JT"
			}  
		}
		"""
	When I send a GET request to /api/schools?searchTerm=894
	Then I should get a 200 response
	And the response should be an object containing these properties (ignoring null values):
		"""
		{
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
	Given establishment Some Primary School (111111) exists with LAESTAB code 894/2200
	And establishment Some other Primary School (222222) exists with properties:
		"""
		{
			"address": {
				"street": "2200 The Street",
				"town": "SomeTown", 
				"postCode": "TR18 3JT"
			}  
		}
		"""
	When I send a GET request to /api/schools?searchTerm=2200
	Then I should get a 200 response
	And the response should be an object containing these properties (ignoring null values):
		"""
		{
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
	Given establishment Primary School 111111 (111111) exists
	And establishment Primary School 222222 (222222) exists
	And establishment Primary School 333333 (333333) exists
	When I send a GET request to /api/schools?searchTerm=primary
	Then I should get a 200 response
	And the response should be an object containing these properties (ignoring null values):
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

Scenario: Should return a 200 response with search results and expected pagination for the given searchTerm and resultsPerPage
	Given establishment Primary School 111111 (111111) exists
	And establishment Primary School 222222 (222222) exists
	And establishment Primary School 333333 (333333) exists
	When I send a GET request to /api/schools?searchTerm=primary&resultsPerPage=2
	Then I should get a 200 response
	And the response should be an object containing these properties (ignoring null values):
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

Scenario: Should return a 200 response with search results and expected pagination for the given searchTerm, resultsPerPage and page
	Given establishment Primary School 111111 (111111) exists
	And establishment Primary School 222222 (222222) exists
	And establishment Primary School 333333 (333333) exists
	When I send a GET request to /api/schools?searchTerm=primary&resultsPerPage=2&page=2
	Then I should get a 200 response
	And the response should be an object containing these properties (ignoring null values):
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

Scenario: Should return a 200 response with expected pagination and no results for the given searchTerm, resultsPerPage, and page
	Given establishment Primary School 111111 (111111) exists
	And establishment Primary School 222222 (222222) exists
	And establishment Primary School 333333 (333333) exists
	When I send a GET request to /api/schools?searchTerm=primary&resultsPerPage=2&page=3
	Then I should get a 200 response
	And the response should be an object containing these properties (ignoring null values):
		"""
		{
			"TotalResults": 3,
			"ResultsPerPage": 2,
			"Page": 3,
			"Results": [
			]
		}
		"""

Scenario: Should return a 200 response with search results and a computed address field when the searchTerm matches the URN and given address has street, town and postcode
	Given establishment Some Primary School (111111) exists with properties:
		"""
		{
			"address": {
				"street": "13 The Street",
				"town": "SomeTown",
				"postCode": "AB12 3CD"
			} 
		}
		"""
	When I send a GET request to /api/schools?searchTerm=111111
	Then I should get a 200 response
	And the response should be an object containing these properties (ignoring null values):
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

Scenario: Should return a 200 response with search results and a computed address field when the searchTerm matches the URN and given address has street and postcode
	Given establishment Some Primary School (111111) exists with properties:
		"""
		{
			"address": {
				"street": "13 The Street",
				"postCode": "AB12 3CD"
			} 
		}
		"""
	When I send a GET request to /api/schools?searchTerm=111111
	Then I should get a 200 response
	And the response should be an object containing these properties (ignoring null values):
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

Scenario: Should return a 200 response with search results and a computed address field when the searchTerm matches the URN and given address has street and town
	Given establishment Some Primary School (111111) exists with properties:
		"""
		{
			"address": {
				"street": "13 The Street",
				"town": "SomeTown"
			} 
		}
		"""
	When I send a GET request to /api/schools?searchTerm=111111
	Then I should get a 200 response
	And the response should be an object containing these properties (ignoring null values):
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

Scenario: Should return a 200 response with search results and a computed educationPhase field when the searchTerm matches the URN and given educationPhase isPrimary equal true
	Given establishment Some Primary School (111111) exists with properties:
		"""
		{
			"isPost16": false,
			"isPrimary": true,
			"isSecondary": false
		}
		"""
	When I send a GET request to /api/schools?searchTerm=111111
	Then I should get a 200 response
	And the response should be an object containing these properties (ignoring null values):
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

Scenario: Should return a 200 response with search results and a computed educationPhase field when the searchTerm matches the URN and given educationPhase isSecondary equal true
	Given establishment Some Primary School (111111) exists with properties:
		"""
		{
			"isPost16": false,
			"isPrimary": false,
			"isSecondary": true
		}
		"""
	When I send a GET request to /api/schools?searchTerm=111111
	Then I should get a 200 response
	And the response should be an object containing these properties (ignoring null values):
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

Scenario: Should return a 200 response with search results and a computed educationPhase field when the searchTerm matches the URN and given educationPhase isPost16 equal true
	Given establishment Some Primary School (111111) exists with properties:
		"""
		{
			"isPost16": true,
			"isPrimary": false,
			"isSecondary": false
		}
		"""
	When I send a GET request to /api/schools?searchTerm=111111
	Then I should get a 200 response
	And the response should be an object containing these properties (ignoring null values):
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

Scenario: Should return BadRequest (400) response if Local Authority with code does not exist
	When I send a GET request to /api/schools?searchTerm=Test&scope=LA&scopeId=100
	Then I should get a 400 response
	And the response should be the message "Bad request: Local Authority with code "100" does not exist."

Scenario: Should return NotFound (404) response if Local Authority with code does not exist
	Given local authority Test LA (100) exists
	And establishment Test School 1 (111111) exists in local authority 999
	When I send a GET request to /api/schools?searchTerm=Test&scope=LA&scopeId=100
	Then I should get a 404 response
	And the response should be the message "Not found: There were no matches for "Test" within the given scope."

Scenario: Should return 200 response if Local Authority with code exist within the given scope "LA"
	Given local authority Test LA (100) exists
	And establishment Test School 1 (111111) exists in local authority 100
	And establishment Test School 2 (222222) exists in local authority 100
	And establishment Test School 3 (333333) exists in local authority 999
	When I send a GET request to /api/schools?searchTerm=Test&scope=LA&scopeId=100
	Then I should get a 200 response
	And the response should be an object containing these properties (ignoring null values):
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
	When I send a GET request to /api/schools?searchTerm=Test&scope=MAT&scopeId=1234
	Then I should get a 400 response
	And the response should be the message "Bad request: Multi-Academy Trust with UID "1234" does not exist."
			 
Scenario: Should return NotFound (404) response if Multi Academy Trust with id does not exist
	Given multi-academy trust Test MAT (1234) exists
	And establishment Test School 1 (111111) exists
	When I send a GET request to /api/schools?searchTerm=Test&scope=MAT&scopeId=1234
	Then I should get a 404 response
	And the response should be the message "Not found: There were no matches for "Test" within the given scope."
		
Scenario: Should return 200 response if Multi Academy Trust with id exist within the given scope "MAT"
	Given multi-academy trust Test MAT (1234) exists
	And establishment Test School 1 (111111) exists in multi-academy trust 1234
	And establishment Test School 2 (222222) exists
	And establishment Test School 3 (333333) exists in multi-academy trust 1234
	When I send a GET request to /api/schools?searchTerm=Test&scope=MAT&scopeId=1234
	Then I should get a 200 response
	And the response should be an object containing these properties (ignoring null values):
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
		
Scenario: Should return NotFound (404) response if there are no matches for Diocese scope search
	Given establishment Test School 1 (111111) exists in diocese Not applicable
	And establishment Test School 2 (222222) exists with properties:
		"""
		{
			"diocese": null
		}
		"""
	And establishment Test School 3 (333333) exists
		
	When I send a GET request to /api/schools?searchTerm=Test&scope=Diocese&scopeId=Test%20Diocese
	Then I should get a 404 response
	And the response should be the message "Not found: There were no matches for "Test" within the given scope."
		
Scenario: Should return 200 response if there are matches for the search within the given scope "Diocese"
	Given establishment Test School 1 (111111) exists in diocese Test Diocese
	And establishment Test School 2 (222222) exists in diocese Another Diocese
	And establishment Test School 3 (333333) exists in diocese Test Diocese
	When I send a GET request to /api/schools?searchTerm=Test&scope=Diocese&scopeId=Test%20Diocese
	Then I should get a 200 response
	And the response should be an object containing these properties (ignoring null values):
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
		
Scenario: Should return 200 response if there are matches for the search within the given scope "All"
	Given local authority Test LA (100) exists
	And establishment Test School 1 (111111) exists in local authority 100
	Given multi-academy trust Test MAT (1234) exists
	And establishment Test School 2 (222222) exists
	And establishment Test School 3 (333333) exists in diocese Test Diocese
	When I send a GET request to /api/schools?searchTerm=Test
	Then I should get a 200 response
	And the response should be an object containing these properties (ignoring null values):
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