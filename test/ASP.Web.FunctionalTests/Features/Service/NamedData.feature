Feature: Accessing named data 

@Javascript:disabled
Scenario: Named user can access named data on policy protected action
	Given I am a user with ASP DfE Named and ASP DfE Admin roles
	When I navigate to /named-data/get-named-data-by-policy
	Then I should get a 200 response
	And the element "p" should have the text content "Named data visible"

@Javascript:disabled
Scenario: Unnamed user cannot access named data on policy protected action
	Given I am a user with ASP DfE Unnamed roles
	When I navigate to /named-data/get-named-data-by-policy
	Then I should get a 403 response

@Javascript:disabled
Scenario: Named user can access named data
	Given I am a user with ASP DfE Named and ASP DfE Admin roles
	When I navigate to /named-data/get-named-data-by-claim
	Then I should get a 200 response
	And the element "p" should have the text content "Named data visible"

@Javascript:disabled
Scenario: Unnamed user cannot access named data
	Given I am a user with ASP DfE Unnamed roles
	When I navigate to /named-data/get-named-data-by-claim
	Then I should get a 200 response
	And the element "p" should have the text content "Named data not visible"