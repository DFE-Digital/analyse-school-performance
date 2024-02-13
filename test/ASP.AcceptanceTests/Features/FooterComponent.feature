Feature: Footer component


Scenario: Footer should display Contact link
	When I navigate to /
	Then I should get a 200 response
	And the element "#footer-link-contact" should have the text content "Contact"
	Then the anchor "#footer-link-contact" should be an external link to "https://form.education.gov.uk/en/AchieveForms/?form_uri=sandbox-publish://AF-Process-2b61dfcd-9296-4f6a-8a26-4671265cae67/AF-Stage-f3f5200e-e605-4a1b-ae6b-3536bc77305c/definition.json"


Scenario: Footer should display Terms of use link
	When I navigate to /
	Then I should get a 200 response
	And the element "#footer-link-terms-of-use" should have the text content "Terms of use"
	Then the anchor "#footer-link-terms-of-use" should be an internal link to "/help/terms-of-use"

Scenario: Footer should display Privacy link
	When I navigate to /
	Then I should get a 200 response
	And the element "#footer-link-privacy" should have the text content "Privacy"
	Then the anchor "#footer-link-privacy" should be an internal link to "/help/privacy"

Scenario: Footer should display Acceptable user link
	When I navigate to /
	Then I should get a 200 response
	And the element "#footer-link-acceptable-use" should have the text content "Acceptable use"
	Then the anchor "#footer-link-acceptable-use" should be an internal link to "/help/acceptable-use"

Scenario: Footer should display Cookies link
	When I navigate to /
	Then I should get a 200 response
	And the element "#footer-link-cookies" should have the text content "Cookies"
	Then the anchor "#footer-link-cookies" should be an internal link to "/help/cookies"

Scenario: Footer should display accessibility link
	When I navigate to /
	Then I should get a 200 response
	And the element "#footer-link-accessibility" should have the text content "Accessibility statement"
	Then the anchor "#footer-link-accessibility" should be an internal link to "/help/accessibility"