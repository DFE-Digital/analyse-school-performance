Feature: Accept terms

@Javascript:disabled
Scenario: Continue button exists on terms of use page
	Given page content "help-accept-terms-of-use" exists:
		"""
		{
			"Views": [
				{
					"ViewId": "AcceptTerms",
				}
			]
		}
		"""
	Given I navigate to <Path>
	Then I should get a 200 response
	Then The path should match <Path>
	When I click the button "#app-accept-terms-button"
	Then the cookie "AcceptedTermsOfUse" should be set to "Accepted"
Examples:
	| Path                       |
	| /help/accept-terms-of-use/ |


@Javascript:disabled
Scenario: Should not be redirected to accept terms when Accepted terms cookie is set to Accepted
	Given I navigate to <Path>
	Then I should get a 200 response
	Then The path should match <Path>
Examples:
	| Path           |
	| /              |
	| /download/     |
	| /news/         |


@Javascript:disabled
Scenario: Should get redirected to Accept terms when Accepted terms cookie is set to Rejected
	Given page content "help-accept-terms-of-use" exists:
		"""
		{
			"Views": [
				{
					"ViewId": "AcceptTerms",
				}
			]
		}
		"""
	Given the cookie "AcceptedTermsOfUse" has been set to "Rejected"
	And I navigate to <Path>
	Then I should get a 200 response
	Then The path should match <ExpectedPath>
	Then the element "#app-accept-terms-button" should exist
Examples:
	| Path | ExpectedPath               |
	| /    | /help/accept-terms-of-use/ |


@Javascript:disabled
Scenario: Should set referral url when Accepted terms cookie is set to Rejected
	Given page content "help-accept-terms-of-use" exists:
		"""
		{
			"Views": [
				{
					"ViewId": "AcceptTerms",
				}
			]
		}
		"""
	Given the cookie "AcceptedTermsOfUse" has been set to "Rejected"
	And I navigate to <Path>
	Then I should get a 200 response
	Then The path should match <ExpectedPath>
	Then the element "#app-accept-terms-button" should exist
Examples:
	| Path           | ExpectedPath                                       |
	| /school/136028 | /help/accept-terms-of-use/?ref-url=/school/136028/ |
	| /download      | /help/accept-terms-of-use/?ref-url=/download/      |
	| /news          | /help/accept-terms-of-use/?ref-url=/news/          |


@Javascript:disabled
Scenario: Clicking Continue should set terms of use cookie to Accepted
	Given page content "help-accept-terms-of-use" exists:
		"""
		{
			"Views": [
				{
					"ViewId": "AcceptTerms",
				}
			]
		}
		"""
	Given the cookie "AcceptedTermsOfUse" has been set to "Rejected"
	Given I navigate to <Path>
	When I click the button "#app-accept-terms-button"
	Then the cookie "AcceptedTermsOfUse" should be set to "Accepted"
Examples:
	| Path                      |
	| /help/accept-terms-of-use |