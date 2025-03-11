Feature: LocalAuthoritiesGetAll

	Scenario: Should not accept POST method
		When I send a POST request to /api/local-authorities
		Then I should get a 405 response
		And the response should be the message "Method not allowed: The HTTP method POST is not allowed."
		And the response should include the header "Allow: GET"

	Scenario Outline: Should return BadRequest (400) response if page parameter is not a whole number greater than or equal to 1
		When I send a GET request to /api/local-authorities?&page=<page>
		Then I should get a 400 response
		And the response should be the message "Bad request: The query parameter "page" should be a whole number greater than or equal to 1."

		Examples:
			| page |
			| y    |
			| 1.5  |
			| 0    |
			| -1   |
			
	Scenario Outline: Should allow page = 1
		Given local authority Some Local Authority (123) exists
		When I send a GET request to /api/local-authorities?page=1
		Then I should get a 200 response
		And the response should be an object containing these properties:
		"""
			{
				"TotalResults": 1,
				"ResultsPerPage": 50,
				"Page": 1,
				"Results": [
					{
						"Code": "123",
						"Name": "Some Local Authority"
					}
				]
			}
		"""

	Scenario Outline: Should return BadRequest (400) response if resultsPerPage parameter is not a whole number greater than or equal to 1
		When I send a GET request to /api/local-authorities?resultsPerPage=<resultsPerPage>
		Then I should get a 400 response
		And the response should be the message "Bad request: The query parameter "resultsPerPage" should be a whole number greater than or equal to 1."

		Examples:
			| resultsPerPage |
			| y              |
			| 1.5            |
			| 0              |
			| -1             |
			
	Scenario Outline: Should allow resultsPerPage = 1
		Given local authority Some Local Authority (123) exists
		When I send a GET request to /api/local-authorities?resultsPerPage=1
		Then I should get a 200 response
		And the response should be an object containing these properties:
		"""
			{
				"TotalResults": 1,
				"ResultsPerPage": 1,
				"Page": 1,
				"Results": [
					{
						"Code": "123",
						"Name": "Some Local Authority"
					}
				]
			}
		"""
	Scenario Outline: Should return NotFound (404) response if there were no Local Authorities
		Given no local authorities exist
		When I send a GET request to /api/local-authorities
		Then I should get a 404 response
		And the response should be the message "Not found: there were no Local Authorities." 
		
	Scenario Outline: Should return 200 response when Local Authorities exist
		Given local authority Some Local Authority (123) exists
		When I send a GET request to /api/local-authorities
		Then I should get a 200 response
		And the response should be an object containing these properties:
		"""
			{
				"TotalResults": 1,
				"ResultsPerPage": 50,
				"Page": 1,
				"Results": [
					{
						"Code": "123",
						"Name": "Some Local Authority"
					}
				]
			}
		"""  
		
	Scenario Outline: Should order by name
		Given local authority Test LA C (100) exists
		And local authority Test LA A (101) exists
		And local authority Test LA B (102) exists
		When I send a GET request to /api/local-authorities
		Then I should get a 200 response
		And the response should be an object containing these properties:
		"""
			{
				"TotalResults": 3,
				"ResultsPerPage": 50,
				"Page": 1,
				"Results": [
					{
						"Code": "101",
						"Name": "Test LA A"
					},
					{
						"Code": "102",
						"Name": "Test LA B"
					},
					{
						"Code": "100",
						"Name": "Test LA C"
					}
				]
			}       
		"""  
	
	Scenario Outline: Should limit the results to results per page
		Given local authority Local Authority 111 (111) exists
		And local authority Local Authority 222 (222) exists
		And local authority Local Authority 333 (333) exists
		When I send a GET request to /api/local-authorities?resultsPerPage=2
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

	Scenario: Pagination (TBC)
		Given local authority Local Authority 111 (111) exists
		And local authority Local Authority 222 (222) exists
		And local authority Local Authority 333 (333) exists
		When I send a GET request to /api/local-authorities?resultsPerPage=2&page=2
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
		
	Scenario: If page number is too big returns an empty page of results
		Given local authority Local Authority 111 (111) exists
		And local authority Local Authority 222 (222) exists
		And local authority Local Authority 333 (333) exists
		When I send a GET request to /api/local-authorities?resultsPerPage=2&page=3
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
	 
