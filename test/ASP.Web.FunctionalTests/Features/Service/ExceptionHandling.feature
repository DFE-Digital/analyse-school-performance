Feature: Exception Handling

@Javascript:disabled
Scenario: Application displays error page on server errors
   Given I navigate to /
   When the application processes an error with status code "500"
   Then the page title should be "Sorry, there is a problem with the service | Analyse school performance"
   