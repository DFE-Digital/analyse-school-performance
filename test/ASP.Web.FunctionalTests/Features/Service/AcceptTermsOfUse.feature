Feature: Accept terms of use

Background:
	Given I am a DfE Named user

@Javascript:disabled
Scenario: Continue button exists on terms of use page
	Given Content Template "help-accept-terms-of-use" exists:
	"""
	{
		"Views": [
			{
				"ViewId": "AcceptTerms",
			}
		]
	}
	"""
	When I navigate to <Path>
	Then I should get a 200 response
	And the path should be <Path>
	When I click the button "#app-accept-terms-button"
	Then the cookie "AcceptedTermsOfUse" should be set to "Accepted"
Examples:
	| Path                       |
	| /help/accept-terms-of-use/ |


@Javascript:disabled
Scenario: Should not be redirected to accept terms when Accepted terms cookie is set to Accepted
	Given Establishment "136028" exists:
	"""
	{
		"name": "Some Primary School"
	}
	"""
	And Local Authority "301" exists:
	"""
	{
		"name": "Some Local Authority"
	}
	"""
	When I navigate to <Path>
	Then I should get a 200 response
	And the path should be <Path>
Examples:
	| Path                  |
	| /                     |
	| /school/136028/       |
	| /local-authority/301/ |

@Javascript:disabled
Scenario: Should not be redirected to accept terms when page not found
	When I navigate to /non-existent-page
	Then I should get a 404 response
	And the path should be /non-existent-page/


@Javascript:disabled
Scenario: Should not be redirected to accept terms on server error
	When the application throws an exception
	Then I should get a 500 response
	And the path should be /error-test/throw-exception/


@Javascript:disabled
Scenario: Should get redirected to Accept terms when Accepted terms cookie is set to Rejected
	Given Content Template "help-accept-terms-of-use" exists:
	"""
	{
		"Views": [
			{
				"ViewId": "AcceptTerms",
			}
		]
	}
	"""
	And the cookie "AcceptedTermsOfUse" has been set to "Rejected"
	When I navigate to <Path>
	Then I should get a 200 response
	And the path should be <ExpectedPath>
	And the element "#app-accept-terms-button" should exist
Examples:
	| Path | ExpectedPath               |
	| /    | /help/accept-terms-of-use/ |


@Javascript:disabled
Scenario: Should set referral url when Accepted terms cookie is set to Rejected
	Given Content Template "help-accept-terms-of-use" exists:
	"""
	{
		"Views": [
			{
				"ViewId": "AcceptTerms",
			}
		]
	}
	"""
	And Establishment "136028" exists:
	"""
	{
		"name": "Some Primary School"
	}
	"""
	And Local Authority "301" exists:
	"""
	{
		"name": "Some Local Authority"
	}
	"""
	And the cookie "AcceptedTermsOfUse" has been set to "Rejected"
	When I navigate to <Path>
	Then I should get a 200 response
	And the path should be <ExpectedPath>
	And the element "#app-accept-terms-button" should exist
Examples:
	| Path                  | ExpectedPath                                              |
	| /school/136028/       | /help/accept-terms-of-use/?referrer=/school/136028/       |
	| /local-authority/301/ | /help/accept-terms-of-use/?referrer=/local-authority/301/ |


@Javascript:disabled
Scenario: Clicking Continue should set terms of use cookie to Accepted
	Given Content Template "help-accept-terms-of-use" exists:
	"""
	{
		"Views": [
			{
				"ViewId": "AcceptTerms",
			}
		]
	}
	"""
	And the cookie "AcceptedTermsOfUse" has been set to "Rejected"
	When I navigate to <Path>
	And I click the button "#app-accept-terms-button"
	Then the cookie "AcceptedTermsOfUse" should be set to "Accepted"
Examples:
	| Path                      |
	| /help/accept-terms-of-use |

@Javascript:disabled
Scenario: Should redirect to the correct referrer
	Given Content Template "help-accept-terms-of-use" exists:
	"""
	{
		"Views": [
			{
				"ViewId": "AcceptTerms",
			}
		]
	}
	"""
	And Establishment "136028" exists:
	"""
	{
		"name": "Some Primary School"
	}
	"""
	And Local Authority "301" exists:
	"""
	{
		"name": "Some Local Authority"
	}
	"""
	And the cookie "AcceptedTermsOfUse" has been set to "Rejected"
	When I navigate to <Path>
	Then I should get a 200 response
	And the path should be <ExpectedPath>
	And the element "#app-accept-terms-button" should exist
	When I click the button "#app-accept-terms-button"
	Then the path should be <Path>
Examples:
	| Path                  | ExpectedPath                                              |
	| /school/136028/       | /help/accept-terms-of-use/?referrer=/school/136028/       |
	| /local-authority/301/ | /help/accept-terms-of-use/?referrer=/local-authority/301/ |