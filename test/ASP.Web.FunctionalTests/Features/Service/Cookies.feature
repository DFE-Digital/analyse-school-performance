Feature: Cookies component

Background:
	Given I am a logged-in user

@Javascript:disabled
Scenario: Analytics cookie preference defaults to "Do not use' when cookie choice has not been made
	Given Content Template "help-cookies" exists:
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
	Then the element "#app-reject-analytics" should be checked

@Javascript:disabled
Scenario: Analytics cookie preference set to 'Use/Do not use' based on cookie choice
	 Given Content Template "help-cookies" exists:
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
	 Then the element "#app-accept-analytics" should be checked

@Javascript:disabled
Scenario: Cookie preference is retained when "Save cookie settings" is clicked
	Given Content Template "help-cookies" exists:
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
	When I update the element "#app-accept-analytics" to be checked
	And I click the button "#app-cookie-preferences-save-button"
	Then the cookie "AnalyticsTracking" should be set to "Accepted"