Feature: LocalAuthoritiesGetAll with searchTerm

	Scenario: Should not accept POST method
		When I send a POST request to /api/local-authorities
		Then I should get a 405 response
		And the response should be the message "Method not allowed: The HTTP method POST is not allowed."
		And the response should include the header "Allow: GET"

	Scenario: Should return BadRequest (400) response if searchTerm parameter is empty string
		When I send a GET request to /api/local-authorities?searchTerm=
		Then I should get a 400 response
		And the response should be the message "Bad request: The query parameter "searchTerm" should not be empty."

	Scenario Outline: Should return BadRequest (400) response if page parameter is not a whole number greater than 1
		When I send a GET request to /api/local-authorities?searchTerm=x&page=<page>
		Then I should get a 400 response
		And the response should be the message "Bad request: The query parameter "page" should be a whole number greater than or equal to 1."

		Examples:
			| page |
			| y    |
			| 1.5  |
			| 0    |
			| -1   |

	Scenario Outline: Should return BadRequest (400) response if resultsPerPage parameter is not a whole number greater than or equal to 1
		When I send a GET request to /api/local-authorities?searchTerm=x&resultsPerPage=<resultsPerPage>
		Then I should get a 400 response
		And the response should be the message "Bad request: The query parameter "resultsPerPage" should be a whole number greater than or equal to 1."

		Examples:
			| resultsPerPage |
			| y              |
			| 1.5            |
			| 0              |
			| -1             |

	Scenario: Should return NotFound (404) response if no Local Authorities exist
		Given no Local Authorities exist
		When I send a GET request to /api/local-authorities?searchTerm=x
		Then I should get a 404 response
		And the response should be the message "Not found: there were no matches for "x"."

	Scenario: Should return NotFound (404) response if Local Authorities exist but none match the search term
		Given Local Authority "111" exists:
		"""
		{
			"name": "Some Local Authority"
		}
		"""
		When I send a GET request to /api/local-authorities?searchTerm=test
		Then I should get a 404 response
		And the response should be the message "Not found: there were no matches for "test"."

	Scenario: Should return 200 response with search results when matches are found for the given searchTerm
		Given Local Authority "111" exists:
		"""
		{
			"name": "Some Local Authority"
		}
		"""
		When I send a GET request to /api/local-authorities?searchTerm=<searchTerm>
		Then I should get a 200 response
		And the response should be an object containing these properties:
		"""
		{
			"TotalResults": 1,
			"ResultsPerPage": 50,
			"Page": 1,
			"Results": [
				{
					"Code": "111",
					"Name": "Some Local Authority"
				}
			]
		}
		"""

	Examples:
		| searchTerm |
		| local      |
		| LOcAL      |

	Scenario: Search results should be sorted alphabetically by name
		Given Local Authority "111" exists:
		"""
		{
			"name": "Local Authority B"
		}
		"""
		And Local Authority "222" exists:
		"""
		{
			"name": "Local Authority A"
		}
		"""
		And Local Authority "333" exists:
		"""
		{
			"name": "Local Authority C"
		}
		"""
		When I send a GET request to /api/local-authorities?searchTerm=local
		Then I should get a 200 response
		And the response should be an object containing these properties:
		"""
		{
			"TotalResults": 3,
			"ResultsPerPage": 50,
			"Page": 1,
			"Results": [
			{
				"Code": "222",
				"Name": "Local Authority A"
			},
			{
				"Code": "111",
				"Name": "Local Authority B"
			},
			{
				"Code": "333",
				"Name": "Local Authority C"
			}  
			]
		}
		"""

	Scenario: Should return a result for an exact match on LA code
		Given Local Authority "111" exists:
		"""
		{
			"name": "Some Local Authority"
		}
		"""
		When I send a GET request to /api/local-authorities?searchTerm=111
		Then I should get a 200 response
		And the response should be an object containing these properties:
		"""
		{
			"TotalResults": 1,
			"ResultsPerPage": 50,
			"Page": 1,
			"Results": [
			{
				"Code": "111",
				"Name": "Some Local Authority"
			}
			]
		}
		"""

	Scenario: Should return no results for a partial match on LA code
		Given Local Authority "111" exists:
		"""
		{
			"name": "Some Local Authority"
		}
		"""
		When I send a GET request to /api/local-authorities?searchTerm=11
		Then I should get a 404 response
		And the response should be the message "Not found: there were no matches for "11"."


#    Pagination

	Scenario: With 3 search results, pagination should default to page 1 with 50 results per page
		Given Local Authority "111" exists:
		"""
		{
			"name": "Local Authority 111"
		}
		"""
		And Local Authority "222" exists:
		"""
		{
			"name": "Local Authority 222"
		}
		"""
		And Local Authority "333" exists:
		"""
		{
			"name": "Local Authority 333"
		}
		"""
		When I send a GET request to /api/local-authorities?searchTerm=local
		Then I should get a 200 response
		And the response should be an object containing these properties:
		"""
		{
			"TotalResults": 3,
			"ResultsPerPage": 50,
			"Page": 1,
			"Results": [
			{
				"Code": "111",
				"Name": "Local Authority 111"
			},
			{
				"Code": "222",
				"Name": "Local Authority 222"
			},
			{
				"Code": "333",
				"Name": "Local Authority 333"
			}
			]
		}
		"""

	Scenario: With 3 search results, pagination should return the first 2 results when 2 results per page
		Given Local Authority "111" exists:
		"""
		{
			"name": "Local Authority 111"
		}
		"""
		And Local Authority "222" exists:
		"""
		{
			"name": "Local Authority 222"
		}
		"""
		And Local Authority "333" exists:
		"""
		{
			"name": "Local Authority 333"
		}
		"""
		When I send a GET request to /api/local-authorities?searchTerm=local&resultsPerPage=2
		Then I should get a 200 response
		And the response should be an object containing these properties:
		"""
		{
			"TotalResults": 3,
			"ResultsPerPage": 2,
			"Page": 1,
			"Results": [
			{
				"Code": "111",
				"Name": "Local Authority 111"
			},
			{
				"Code": "222",
				"Name": "Local Authority 222"
			}
			]
		}
		"""

	Scenario: With 3 search results, pagination should return the 3rd result when requesting page 2 with 2 results per page
		Given Local Authority "111" exists:
		"""
		{
			"name": "Local Authority 111"
		}
		"""
		And Local Authority "222" exists:
		"""
		{
			"name": "Local Authority 222"
		}
		"""
		And Local Authority "333" exists:
		"""
		{
			"name": "Local Authority 333"
		}
		"""
		When I send a GET request to /api/local-authorities?searchTerm=local&resultsPerPage=2&page=2
		Then I should get a 200 response
		And the response should be an object containing these properties:
		"""
		{
			"TotalResults": 3,
			"ResultsPerPage": 2,
			"Page": 2,
			"Results": [
			{
				"Code": "333",
				"Name": "Local Authority 333"
			}
			]
		}
		"""

	Scenario: With 3 search results, pagination should return an empty page when requesting page 3 with 2 results per page
		Given Local Authority "111" exists:
		"""
		{
			"name": "Local Authority 111"
		}
		"""
		And Local Authority "222" exists:
		"""
		{
			"name": "Local Authority 222"
		}
		"""
		And Local Authority "333" exists:
		"""
		{
			"name": "Local Authority 333"
		}
		"""
		When I send a GET request to /api/local-authorities?searchTerm=local&resultsPerPage=2&page=3
		Then I should get a 200 response
		And the response should be an object containing these properties:
		"""
		{
			"TotalResults": 3,
			"ResultsPerPage": 2,
			"Page": 3,
			"Results": [
			]
		}
		"""