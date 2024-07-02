Feature: TestSteps

@Javascript:disabled
Scenario: Example creating multiple establishments
    Given 999 establishments exist with properties:
    | urn          | name     |
    | (100000 + n) | School n |
    When I navigate to /school/100005/
	Then I should get a 200 response
    Then the element "h1.govuk-heading-l" should have the text content "School 5 (URN: 100005)"

@Javascript:disabled
 Scenario: School search example
    When I navigate to /search/
    And I update the textbox "#searchTerm" to have the value "primary"
    And I click the button "#searchSubmit"
    Then the path should match "/search/search-result/?page=1&searchTerm=primary"
    #And the page title should be "We found no matches for "primary" | Analyse school performance"
    And the element "h1" should have the text content "We found no matches for "primary""