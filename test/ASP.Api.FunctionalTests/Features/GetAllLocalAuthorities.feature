Feature: GetAllLocalAuthorities

	Scenario: Should not accept POST method
		When I send a POST request to /api/GetAllLocalAuthorities
		Then I should get a 405 response
		And the response should be the message "Method not allowed: The HTTP method POST is not allowed."
		And the response should include the header "Allow: GET"

	Scenario Outline: Should return BadRequest (400) response if page parameter is not a whole number greater than or equal to 1
		When I send a GET request to /api/GetAllLocalAuthorities?&page=<page>
		Then I should get a 400 response
		And the response should be the message "Bad request: The parameter "page" should be a whole number greater than or equal to 1."

		Examples:
			| page |
			| y    |
			| 1.5  |
			| 0    |
			| -1   |
			
	Scenario Outline: Should allow page = 1
		Given Local Authority "123" exists:
		"""
		{
			"name": "Some Local Authority"
		}
		"""
		When I send a GET request to /api/GetAllLocalAuthorities?page=1
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
		When I send a GET request to /api/GetAllLocalAuthorities?resultsPerPage=<resultsPerPage>
		Then I should get a 400 response
		And the response should be the message "Bad request: The parameter "resultsPerPage" should be a whole number greater than or equal to 1."

		Examples:
			| resultsPerPage |
			| y              |
			| 1.5            |
			| 0              |
			| -1             |
			
	Scenario Outline: Should allow resultsPerPage = 1
		Given Local Authority "123" exists:
		"""
		{
			"name": "Some Local Authority"
		}
		"""
		When I send a GET request to /api/GetAllLocalAuthorities?resultsPerPage=1
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
		Given no Local Authorities exist
		When I send a GET request to /api/GetAllLocalAuthorities
		Then I should get a 404 response
		And the response should be the message "Not found: there were no Local Authorities." 
		
	Scenario Outline: Should return 200 response when Local Authorities exist
		Given Local Authority "123" exists:
		"""
		{
			"name": "Some Local Authority"
		}
		"""
		When I send a GET request to /api/GetAllLocalAuthorities
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
		Given Local Authority "100" exists:
		"""
			{
			"name": "Test LA C"
			}
		"""
		And Local Authority "101" exists:
		"""
			{
			"name": "Test LA A"
			}
		"""
		And Local Authority "102" exists:
		"""
			{
			"name": "Test LA B"
			}
		"""
		When I send a GET request to /api/GetAllLocalAuthorities
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
		When I send a GET request to /api/GetAllLocalAuthorities?resultsPerPage=2
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
		When I send a GET request to /api/GetAllLocalAuthorities?resultsPerPage=2&page=2
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
		When I send a GET request to /api/GetAllLocalAuthorities?resultsPerPage=2&page=3
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
	 
