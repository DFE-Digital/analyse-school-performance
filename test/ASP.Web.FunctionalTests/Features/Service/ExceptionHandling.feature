Feature: Exception Handling

@Javascript:disabled
Scenario: Application displays error page on server errors
   When I navigate to /
   And the application receives a server error
   Then the page title should be "Sorry, there is a problem with the service"