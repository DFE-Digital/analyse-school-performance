Feature: Search Page
 
@Javascript:disabled
Scenario: Search Term Validation
    When I navigate to /search/
	Then I should get a 200 response
	And the page title should be "Search | Analyse school performance"
	And the element "h1.govuk-heading-l" should have the text content "Search for a school"
	And the element "*[data-testid='searchTerm']" should have the text content "Enter school name, address, URN (Unique Reference Number) or LAESTAB (Local Authority Establishment Number) Search"

@Javascript:disabled
Scenario: Errors in Search Term Validation
    When I navigate to /search/
	And I click the button "#searchSubmit"
	Then the element "#searchTerm-input-error" should have the text content "Please enter a search term such as a school name, address, URN (Unique Reference Number) or LAESTAB (Local Authority Establishment Number)"
    And the element "h2.govuk-error-summary__title" should have the text content "Please correct the following error(s)."
	And the element "*[data-testid='SearchTerm']" should have the text content "Enter school name, address or reference number"
