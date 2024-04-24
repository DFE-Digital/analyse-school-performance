Feature: Page Not Found 404 Error

  @Javascript:disabled
  Scenario: Application displays a page not found error page
    When I navigate to /not-found
    Then I should get a 404 response
    And the page title should be "Page not found – ASP – GOV.UK | Analyse school performance"
    And the element "h1.govuk-heading-l" should have the text content "Page not found"
    And the element "*[data-testid='address-typing-instruction']" should have the text content "If you typed the web address, check it is correct."
    And the element "*[data-testid='address-pasting-instruction']" should have the text content "If you pasted the web address, check you copied the entire address."
    And the element "*[data-testid='error-display-message']" should not exist

  @Javascript:disabled
  Scenario: Application displays a page not found error page with an error message
    When the application returns a 404 with error message "this is a test error."
    Then I should get a 404 response
    And the page title should be "Page not found – ASP – GOV.UK | Analyse school performance"
    And the element "h1.govuk-heading-l" should have the text content "Page not found"
    And the element "*[data-testid='address-typing-instruction']" should have the text content "If you typed the web address, check it is correct."
    And the element "*[data-testid='address-pasting-instruction']" should have the text content "If you pasted the web address, check you copied the entire address."
    And the element "*[data-testid='error-display-message']" should exist
    And the element "*[data-testid='error-display-message']" should have the text content "Error message: this is a test error."


    
    