Feature: Exception Handling

@Javascript:disabled
Scenario: Application displays error page on server errors
   When the application processes an error with status code "500"
   Then the page title should be "Sorry, there is a problem with the service | Analyse school performance"
#
#Scenario: Service remains available if table storage is not available
# When the application processes an error with status code "500"
# And the application processes an exception with status code "500"
# Then the page title should be "Sorry, there is a problem with the service | Analyse school performance"


#Scenario: Error page shows the correct error code
# When the application processes a server error with status code "500"
# Then the page title should be "Sorry, there is a problem with the service | Analyse school performance"
# And the page contains the correct error code









   