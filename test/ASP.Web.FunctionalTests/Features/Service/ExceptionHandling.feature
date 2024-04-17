Feature: Exception Handling

@Javascript:disabled
Scenario: Application displays error page on server errors
   Given I navigate to /
   When the application throws an exception
   Then the page title should be "Sorry, there is a problem with the service | Analyse school performance"
   
@Javascript:disabled
Scenario: Exception handler updates table storage
   Given I navigate to /
   When the application throws an exception
   Then the exception details are added to table storage