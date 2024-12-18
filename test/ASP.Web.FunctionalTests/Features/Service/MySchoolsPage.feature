Feature: My schools page

@Javascript:disabled
Scenario: School Named user is denied access to my-schools page 
	Given I am a School Named user for Establishment "123456"
		When I navigate to /my-schools/
		Then I should get a 403 response
		And the element "h1.govuk-heading-l" should have the text content "Access not allowed"

@Javascript:disabled
Scenario: DfE Named user is denied access to my-schools page
		Given I am a DfE Named user
		When I navigate to /my-schools/
		Then I should get a 403 response
		And the element "h1.govuk-heading-l" should have the text content "Access not allowed"

 @Javascript:disabled
 Scenario: Ofsted Unnamed user is denied access to my-schools page
		Given I am a Ofsted Unnamed user
		When I navigate to /my-schools/
		Then I should get a 403 response
		And the element "h1.govuk-heading-l" should have the text content "Access not allowed"
 
@Javascript:disabled
Scenario: Server error when accessing /my-schools/ with LA Named role             
	 Given I am a LA Named user for Local Authority "100"
	 When I navigate to /my-schools/
	 Then I should get a 500 response
	 And the page title should be "Sorry, there is a problem with the service"  
	 
@Javascript:disabled
Scenario: No results for LA Named user when no schools in their Local Authority 
	Given I am a LA Named user for Local Authority "100"
	And Local Authority "100" exists:
	"""
		{ 
		 "name": "Test LA"
		}
	"""
	And Establishment "111111" exists:
	"""
		{
			"name": "Test School 1",
			"localAuthority":
			 {
				"code": "999"
			 }
		}
	"""
	When I navigate to /my-schools/
	Then the element "h1.govuk-heading-l" should have the text content "We found no schools."
	
@Javascript:disabled
Scenario: Correct results displayed for LA Named user with schools in their Local Authority
	Given I am a LA Named user for Local Authority "100"
	And Local Authority "100" exists:
	"""
		{ 
		"name": "Test LA"
		}
	"""
	And Establishment "111111" exists:
	"""
		{
		 "name": "Test School 1",
		 "localAuthority":
		 {
			"code": "100"
		 }
		}
	"""
	And Establishment "222222" exists:
	"""
		{
			"name": "Test School 2",
			"localAuthority":
			{
			"code": "100"
			}
		}
	"""
	And Establishment "333333" exists:
	"""
		{
		"name": "Test School 3",
		"localAuthority":
			{
			"code": "999"
			}
		}
	"""
	When I navigate to /my-schools/
	Then the element "*[data-testid='NumberOfPages-Footer']" should have the text content "Showing 1 - 2 of 2 schools"
	And the element "[data-testid="establishment-listing-urn-<Counter>"]" should have the text content "<URN>"
	And the element "[data-testid="establishment-listing-name-<Counter>"]" should have the text content "<Name>"
Examples: 
	| Counter | URN    | Name          |
	| 1       | 111111 | Test School 1 |
	| 2       | 222222 | Test School 2 |  
	
@Javascript:disabled
Scenario: Server error when MAT Named user accesses /my-schools/ page
	Given I am a MAT Named user for Multi-Academy Trust "1234"
	When I navigate to /my-schools/
	Then I should get a 500 response
	And the page title should be "Sorry, there is a problem with the service"  
	
@Javascript:disabled
Scenario: MAT Named user sees 'No schools found' message when MAT has no associated schools
	Given I am a MAT Named user for Multi-Academy Trust "1234"
	And Multi Academy Trust "1234" exists:
	"""
		{ 
		 "name": "Test MAT"
		}
	"""
	And Establishment "111111" exists:
	"""
		{
		 "name": "Test School 1"
		}
	"""
	When I navigate to /my-schools/
	Then the element "h1.govuk-heading-l" should have the text content "We found no schools."
	
@Javascript:disabled
Scenario: MAT Named user sees correct list of schools associated with their Multi-Academy Trust
	Given I am a MAT Named user for Multi-Academy Trust "1234"
	And Multi Academy Trust "1234" exists:
	"""
		{ 
		 "name": "Test MAT"
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
	When I navigate to /my-schools/
	Then the element "*[data-testid='NumberOfPages-Footer']" should have the text content "Showing 1 - 2 of 2 schools"
	And the element "[data-testid="establishment-listing-urn-<Counter>"]" should have the text content "<URN>"
	And the element "[data-testid="establishment-listing-name-<Counter>"]" should have the text content "<Name>"
Examples: 
	| Counter | URN    | Name          |
	| 1       | 111111 | Test School 1 |
	| 2       | 333333 | Test School 3 |  
	
@Javascript:disabled
Scenario: Diocese Named user sees 'No schools found' message when no schools are associated with their diocese
	Given I am a Diocese Named user for Diocese "Test Diocese"
	And Establishment "111111" exists:
	"""
		{
		"name": "Test School 1",
		"diocese": {
			"code": "0000",
			"name": "Not applicable",
			"lname": "not applicable",
			"isNullish": true
			}
		}
	"""
	And Establishment "222222" exists:
	"""
		{
			"name": "Test School 2",
			"diocese": null
		}
	"""
	And Establishment "333333" exists:
	"""
		{
			"name": "Test School 3"
		}
	"""
	When I navigate to /my-schools/
	Then the element "h1.govuk-heading-l" should have the text content "We found no schools."
	
@Javascript:disabled
Scenario: Diocese Named user sees correct list of schools associated with their diocese    
	Given I am a Diocese Named user for Diocese "Test Diocese"
	And Establishment "111111" exists:
	"""
		{
		"name": "Test School 1",
		"diocese": {
			"code": "1000",
			"name": "Test Diocese",
			"lname": "test diocese",
			"isNullish": false
			}
		}
	"""
	And Establishment "222222" exists:
	"""
		{
		"name": "Test School 2",
		"diocese": {
			"code": "1001",
			"name": "Another Diocese",
			"lname": "another diocese",
			"isNullish": false
			}
		}
	"""
	And Establishment "333333" exists:
	"""
		{
		"name": "Test School 3",
		"diocese": {
			"code": "1000",
			"name": "Test Diocese",
			"lname": "test diocese",
			"isNullish": false
			}
		}
	"""
	When I navigate to /my-schools/
	Then the element "*[data-testid='NumberOfPages-Footer']" should have the text content "Showing 1 - 2 of 2 schools"
	And the element "[data-testid="establishment-listing-urn-<Counter>"]" should have the text content "<URN>"
	And the element "[data-testid="establishment-listing-name-<Counter>"]" should have the text content "<Name>"
Examples: 
	| Counter | URN    | Name          |
	| 1       | 111111 | Test School 1 |
	| 2       | 333333 | Test School 3 |
	
@Javascript:disabled
Scenario: Pagination in my schools    
	Given I am a MAT Named user for Multi-Academy Trust "1234"
	And Multi Academy Trust "1234" exists:
	"""
		{ 
		 "name": "Test MAT"
		}
	"""
	And 251 Establishments exist with properties:
		| urn          | name                        | multiAcademyTrust |
		| (100000 + n) | Primary School (100000 + n) | { "uid": "1234" } |
	When I navigate to /my-schools/
	Then the element "*[data-testid='NumberOfPages-Footer']" should have the text content "Showing 1 - 50 of 251 schools"
	And the element "*[data-testid='PageLinks-Footer-1']" should have the href "/my-schools/?page=1"
	And the element "*[data-testid='PageLinks-Footer-2']" should have the href "/my-schools/?page=2"
	And the element "*[data-testid='govuk-pagination__link--Footer']" should have the text content "..."
	And the element "*[data-testid='PageLinks-Footer-6']" should have the href "/my-schools/?page=6"
	And the element "*[data-testid='PageLinks-Footer-Next']" should have the href "/my-schools/?page=2"
	And the element "*[data-testid='establishment-listing-name-1']" should have the text content "Primary School 100001"
	And the element "*[data-testid='establishment-listing-name-2']" should have the text content "Primary School 100002"
	And the element "*[data-testid='establishment-listing-name-3']" should have the text content "Primary School 100003"
	And the element "*[data-testid='establishment-listing-name-4']" should have the text content "Primary School 100004"
	And the element "*[data-testid='establishment-listing-name-5']" should have the text content "Primary School 100005"
	And the element "*[data-testid='establishment-listing-urn-1']" should have the text content "100001"
	And the element "*[data-testid='establishment-listing-urn-2']" should have the text content "100002"
	And the element "*[data-testid='establishment-listing-urn-3']" should have the text content "100003"
	And the element "*[data-testid='establishment-listing-urn-4']" should have the text content "100004"
	And the element "*[data-testid='establishment-listing-urn-5']" should have the text content "100005"
	
@Javascript:disabled
Scenario: Page should show a breadcrumb trail
	Given I am a MAT Named user for Multi-Academy Trust "1234"
	And Multi Academy Trust "1234" exists:
	"""
		{ 
		 "name": "Test MAT"
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
	When I navigate to /my-schools/
	Then I should get a 200 response
	And the page title should be "My schools"
	And the breadcrumb trail should be:
		| text       | href | current |
		| Home       | /    |         |
		| My schools |      | true    |