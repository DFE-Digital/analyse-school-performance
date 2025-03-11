Feature: SchoolsGetSearchSuggestions

	Scenario: Should not accept POST method
		When I send a POST request to /api/schools/search-suggestions
		Then I should get a 405 response
		And the response should be the message "Method not allowed: The HTTP method POST is not allowed."
		And the response should include the header "Allow: GET"

	Scenario: Should return BadRequest (400) response if searchTerm parameter is missing
		When I send a GET request to /api/schools/search-suggestions
		Then I should get a 400 response
		And the response should be the message "Bad request: The query parameter "searchTerm" is missing."

	Scenario: Should return BadRequest (400) response if searchTerm parameter is empty string
		When I send a GET request to /api/schools/search-suggestions?searchTerm=
		Then I should get a 400 response
		And the response should be the message "Bad request: The query parameter "searchTerm" should not be empty."

	Scenario Outline: Should return BadRequest (400) response if page parameter is not a whole number greater than 1
		When I send a GET request to /api/schools/search-suggestions?searchTerm=x&maxSuggestions=<maxSuggestions>
		Then I should get a 400 response
		And the response should be the message "Bad request: The query parameter "maxSuggestions" should be a whole number greater than or equal to 1."

		Examples:
			| maxSuggestions |
			| y              |
			| 1.5            |
			| 0              |
			| -1             |

	Scenario: Should return NotFound (404) response if no matches found for the searchTerm
		Given no establishments exist
		When I send a GET request to /api/schools/search-suggestions?searchTerm=x
		Then I should get a 404 response
		And the response should be the message "Not found: There were no matches for "x" within the given scope."

	Scenario: Should return NotFound (404) response if there were no relevant matches for the given searchTerm
		Given establishment Some Primary School (111111) exists
		When I send a GET request to /api/schools/search-suggestions?searchTerm=secondary
		Then I should get a 404 response
		And the response should be the message "Not found: There were no matches for "secondary" within the given scope."

	Scenario: Should return a NotFound (404) response if the requested establishment has been deleted for the given searchTerm
		Given deleted establishment Some Primary School (222222) exists
		When I send a GET request to /api/schools/search-suggestions?searchTerm=222222
		Then I should get a 404 response
		And the response should be the message "Not found: There were no matches for "222222" within the given scope."

	Scenario: Should return a NotFound (404) response if the requested establishment is not currently visible for the given searchTerm
		Given non-visible establishment Some Primary School (111111) exists
		When I send a GET request to /api/schools/search-suggestions?searchTerm=111111
		Then I should get a 404 response
		And the response should be the message "Not found: There were no matches for "111111" within the given scope."

	Scenario: Should not return 400 response if maxSuggestions = 1
		Given establishment Some Primary School (111111) exists
		When I send a GET request to /api/schools/search-suggestions?searchTerm=111111&maxSuggestions=1
		Then I should get a 200 response

	Scenario Outline: Should return 200 response with search results when matches are found for the given searchTerm
		Given establishment Some Primary School (987654) exists with LAESTAB code 123/4567
		When I send a GET request to /api/schools/search-suggestions?searchTerm=<searchTerm>
		Then I should get a 200 response
		And the response should be an object containing these properties (ignoring null values):
		"""
		[
			{
				"Urn": "987654",
				"Laestab": "123/4567",
				"Name": "Some Primary School"
			}
		]
		"""

		Examples:
			| searchTerm |
			| Prim       |
			| PRiMaRY    |
			| ry%20sc    |
			| 87         |
			| 23         |
			| 56         |
			| 23%2F45    |
			| 2345       |
			| 56         |

	Scenario: Should return a 200 response with search suggestions results and results are sorted alphabetically for the given searchTerm
		Given establishment Primary School C (111111) exists with LAESTAB code 123/1111
		And establishment Primary School D (222222) exists with LAESTAB code 123/2222
		And establishment Primary School B (333333) exists with LAESTAB code 123/3333
		And establishment Primary School A (444444) exists with LAESTAB code 123/4444
		When I send a GET request to /api/schools/search-suggestions?searchTerm=primary
		Then I should get a 200 response
		And the response should be an object containing these properties (ignoring null values):
		"""
		[
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
		"""

	Scenario: Should return a 200 response with search suggestion results that are sorted alphabetically, up to the maximum number of suggestions specified for the given search term
		Given establishment Primary School C (111111) exists with LAESTAB code 123/1111
		And establishment Primary School B (222222) exists with LAESTAB code 123/2222
		And establishment Primary School A (333333) exists with LAESTAB code 123/3333
		And establishment Primary School D (444444) exists with LAESTAB code 123/4444
		When I send a GET request to /api/schools/search-suggestions?searchTerm=primary&maxSuggestions=2
		Then I should get a 200 response
		And the response should be an object containing these properties (ignoring null values):
		"""
		[
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
		"""

	Scenario: Should return BadRequest (400) response if parameter "scope" should not be empty
		When I send a GET request to /api/schools/search-suggestions?searchTerm=xyz&scope=
		Then I should get a 400 response
		And the response should be the message "Bad request: The query parameter "scope" should not be empty."

	Scenario: Should return BadRequest (400) response when "xyz" is not a valid scope
		When I send a GET request to /api/schools/search-suggestions?searchTerm=xyz&scope=xyz
		Then I should get a 400 response
		And the response should be the message "Bad request: "xyz" is not a valid scope."

	Scenario Outline: Should return BadRequest (400) response if scopeId parameter is missing
		When I send a GET request to /api/schools/search-suggestions?searchTerm=xyz&scope=<Scope>
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

	Scenario: Should return BadRequest (400) response if Local Authority with code does not exist
		When I send a GET request to /api/schools/search-suggestions?searchTerm=Test&scope=LA&scopeId=100
		Then I should get a 400 response
		And the response should be the message "Bad request: Local Authority with code "100" does not exist."

	Scenario: Should return NotFound (404) response if Local Authority with code does not exist
		Given local authority Test LA (100) exists
		And establishment Test School 1 (111111) exists in local authority 999
		When I send a GET request to /api/schools/search-suggestions?searchTerm=Test&scope=LA&scopeId=100
		Then I should get a 404 response
		And the response should be the message "Not found: There were no matches for "Test" within the given scope."

	Scenario: Should return 200 response if Local Authority with code exist within the given scope "LA"
		Given local authority Test LA (100) exists
		And establishment Test School 1 (111111) exists in local authority 100
		And establishment Test School 2 (222222) exists in local authority 100
		And establishment Test School 3 (333333) exists in local authority 999
		When I send a GET request to /api/schools/search-suggestions?searchTerm=Test&scope=LA&scopeId=100
		Then I should get a 200 response
		And the response should be an object containing these properties (ignoring null values):
		"""
		[
			{
				"Urn": "111111",
				"Name": "Test School 1"
			},
			{
				"Urn": "222222",
				"Name": "Test School 2"
			}
		]
		"""

	Scenario: Should return BadRequest (400) response if Multi Academy Trust with id does not exist
		When I send a GET request to /api/schools/search-suggestions?searchTerm=Test&scope=MAT&scopeId=1234
		Then I should get a 400 response
		And the response should be the message "Bad request: Multi-Academy Trust with UID "1234" does not exist."

	Scenario: Should return NotFound (404) response if Multi Academy Trust with id does not exist
		Given multi-academy trust Test MAT (1234) exists
		And establishment Test School 1 (111111) exists
		When I send a GET request to /api/schools/search-suggestions?searchTerm=Test&scope=MAT&scopeId=1234
		Then I should get a 404 response
		And the response should be the message "Not found: There were no matches for "Test" within the given scope."

	Scenario: Should return 200 response if Multi Academy Trust with id exist within the given scope "MAT"
		Given multi-academy trust Test MAT (1234) exists
		And establishment Test School 1 (111111) exists in multi-academy trust 1234
		And establishment Test School 2 (222222) exists
		And establishment Test School 3 (333333) exists in multi-academy trust 1234
		When I send a GET request to /api/schools/search-suggestions?searchTerm=Test&scope=MAT&scopeId=1234
		Then I should get a 200 response
		And the response should be an object containing these properties (ignoring null values):
		"""
		[
			{
				"Urn": "111111",
				"Name": "Test School 1"
			},
			{
				"Urn": "333333",
				"Name": "Test School 3"
			}
		]
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
		When I send a GET request to /api/schools/search-suggestions?searchTerm=Test&scope=Diocese&scopeId=Test%20Diocese
		Then I should get a 404 response
		And the response should be the message "Not found: There were no matches for "Test" within the given scope."

	Scenario: Should return 200 response if there are matches for the search within the given scope "Diocese"
		Given establishment Test School 1 (111111) exists in diocese Test Diocese
		And establishment Test School 2 (222222) exists in diocese Another Diocese
		And establishment Test School 3 (333333) exists in diocese Test Diocese
		When I send a GET request to /api/schools/search-suggestions?searchTerm=Test&scope=Diocese&scopeId=Test%20Diocese
		Then I should get a 200 response
		And the response should be an object containing these properties (ignoring null values):
		"""
		[
			{
				"Urn": "111111",
				"Name": "Test School 1"
			},
			{
				"Urn": "333333",
				"Name": "Test School 3"
			}
		]
		"""

	Scenario: Should return 200 response if there are matches for the search within the given scope "All"
		Given local authority Test LA (100) exists
		And establishment Test School 1 (111111) exists in local authority 100
		Given multi-academy trust Test MAT (1234) exists
		And establishment Test School 2 (222222) exists
		And establishment Test School 3 (333333) exists in diocese Test Diocese
		When I send a GET request to /api/schools/search-suggestions?searchTerm=Test
		Then I should get a 200 response
		And the response should be an object containing these properties (ignoring null values):
		"""
		[
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
		"""