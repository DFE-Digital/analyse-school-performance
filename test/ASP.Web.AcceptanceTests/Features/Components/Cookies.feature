Feature: Cookies component

Scenario: Analytics cookie preference defaults to "Do not use' when cookie choice has not been made
	Given page content "help-cookies" exists:
		"""
		{
			"Views": [
				{
					"ViewId": "CookiePreferences"
				}
			]
		}
		"""
	When I navigate to /help/cookies
	Then the radio "#app-reject-analytics" is checked


Scenario: Analytics cookie preference set to 'Use/Do not use' based on cookie choice
	 Given page content "help-cookies" exists:
		"""
		{
			"Views": [
				{
					"ViewId": "CookiePreferences"
				}
			]
		}
		"""
     When the cookie "AnalyticsTracking" has been set to "Accepted"
	 And I navigate to /help/cookies
	 Then the radio "#app-accept-analytics" is checked


Scenario:  Cookie preference is retained when "Save cookie settings" is clicked
    Given page content "help-cookies" exists:
		"""
		{
			"Views": [
				{
					"ViewId": "CookiePreferences"
				}
			]
		}
		"""
	When the cookie "AnalyticsTracking" has been set to "Rejected"
	And I navigate to /help/cookies
	When I check the radio button "#app-accept-analytics"
	And I submit the form "#app-cookie-preferences-form"
	Then the cookie "AnalyticsTracking" should be set to "Accepted"
 

