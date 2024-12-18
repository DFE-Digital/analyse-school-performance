Feature: Exception Handling

@Javascript:disabled
Scenario: Application displays error page on server errors
	 When the application throws an exception
	 Then I should get a 500 response
	 And the page title should be "Sorry, there is a problem with the service"
	 
@Javascript:disabled
Scenario: Exception handler updates table storage
	 When the application throws an exception
	 Then I should get a 500 response
	 And an InternalServerError entry should be added to table storage