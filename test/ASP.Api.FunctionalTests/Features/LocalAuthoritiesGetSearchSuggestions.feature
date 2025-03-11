Feature: LocalAuthoritiesGetSearchSuggestions

	Scenario: Should return 405 response if wrong HTTP method is used
		When I send a POST request to /api/local-authorities/search-suggestions
		Then I should get a 405 response
		And the response should be the message "Method not allowed: The HTTP method POST is not allowed."
		And the response should include the header "Allow: GET"

	Scenario: Should return a BadRequest (400) response if searchTerm parameter is missing
		When I send a GET request to /api/local-authorities/search-suggestions
		Then I should get a 400 response
		And the response should be the message "Bad request: The query parameter "searchTerm" is missing."

	Scenario: Should return a BadRequest (400) response if searchTerm parameter is empty
		When I send a GET request to /api/local-authorities/search-suggestions?searchTerm=
		Then I should get a 400 response
		And the response should be the message "Bad request: The query parameter "searchTerm" should not be empty."

	Scenario Outline: Should return BadRequest (400) response if maxSuggestions parameter is not a whole number greater than 1
		When I send a GET request to /api/local-authorities/search-suggestions?searchTerm=x&maxSuggestions=<maxSuggestions>
		Then I should get a 400 response
		And the response should be the message "Bad request: The query parameter "maxSuggestions" should be a whole number greater than or equal to 1."

		Examples:
			| maxSuggestions |
			| y              |
			| 1.5            |
			| 0              |
			| -1             |

	Scenario: Should return NotFound (404) response if no Local Authorities exist
		Given no local authorities exist
		When I send a GET request to /api/local-authorities/search-suggestions?searchTerm=x
		Then I should get a 404 response
		And the response should be the message "Not found: there were no matches for "x"."

	Scenario: Should return NotFound (404) response if no Local Authorities match the search term
		Given local authority Some Local Authority (111) exists
		When I send a GET request to /api/local-authorities/search-suggestions?searchTerm=test
		Then I should get a 404 response
		And the response should be the message "Not found: there were no matches for "test"."

	Scenario Outline: Should return Success (200) response with suggestions when matches are found for the given searchTerm
		Given local authority Some Local Authority (123) exists
		When I send a GET request to /api/local-authorities/search-suggestions?searchTerm=<searchTerm>
		Then I should get a 200 response
		And the response should be an object containing these properties:
		"""
		[
			{
				"Code": "123",
				"Name": "Some Local Authority"
			}
		]
		"""

		Examples:
			| searchTerm |
			| loca       |
			| LOcAL      |
			| aL%20aUth  |
			| Al%20AuTH  |
			| 1          |
			| 12         |
			| 23         |
			| 123        |

	Scenario: When searching for a partial name, suggestions should be sorted by name
		Given local authority Local Authority C (111) exists
		And local authority Local Authority D (222) exists
		And local authority Local Authority B (333) exists
		And local authority Local Authority A (444) exists
		When I send a GET request to /api/local-authorities/search-suggestions?searchTerm=local
		Then I should get a 200 response
		And the response should be an object containing these properties:
		"""
		[
			{
				"Code": "444",
				"Name": "Local Authority A"
			},
			{
				"Code": "333",
				"Name": "Local Authority B"
			},
			{
				"Code": "111",
				"Name": "Local Authority C"
			},
			{
				"Code": "222",
				"Name": "Local Authority D"
			}
		]
		"""

	Scenario: When searching for a partial code, suggestions should be sorted by code
		Given local authority Local Authority C (101) exists
		And local authority Local Authority D (102) exists
		And local authority Local Authority B (103) exists
		And local authority Local Authority A (104) exists
		When I send a GET request to /api/local-authorities/search-suggestions?searchTerm=10
		Then I should get a 200 response
		And the response should be an object containing these properties:
		"""
		[
			{
				"Code": "101",
				"Name": "Local Authority C"
			},
			{
				"Code": "102",
				"Name": "Local Authority D"
			},
			{
				"Code": "103",
				"Name": "Local Authority B"
			},
			{
				"Code": "104",
				"Name": "Local Authority A"
			}
		]
		"""

	Scenario: Suggestions should be limited by maxSuggestions parameter
		Given local authority Local Authority C (111) exists
		And local authority Local Authority B (222) exists
		And local authority Local Authority A (333) exists
		And local authority Local Authority D (444) exists
		When I send a GET request to /api/local-authorities/search-suggestions?searchTerm=local&maxSuggestions=2
		Then I should get a 200 response
		And the response should be an object containing these properties:
		"""
		[
			{
				"Code": "333",
				"Name": "Local Authority A"
			},
			{
				"Code": "222",
				"Name": "Local Authority B"
			}
		]
		"""