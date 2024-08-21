Feature: Page footer

Background:
	Given I am a logged-in user

@Javascript:disabled
Scenario: Footer should display Contact link
	When I navigate to /
	Then the element "#footer-link-contact" should have the text content "Contact"
	And  the element "#footer-link-contact" should have the href "https://form.education.gov.uk/en/AchieveForms/?form_uri=sandbox-publish://AF-Process-2b61dfcd-9296-4f6a-8a26-4671265cae67/AF-Stage-f3f5200e-e605-4a1b-ae6b-3536bc77305c/definition.json"

@Javascript:disabled
Scenario: Footer should display Terms of use link
	When I navigate to /
	Then the element "#footer-link-terms-of-use" should have the text content "Terms of use"
	And  the element "#footer-link-terms-of-use" should have the href "/help/terms-of-use"

@Javascript:disabled
Scenario: Footer should display Privacy link
	When I navigate to /
	Then the element "#footer-link-privacy" should have the text content "Privacy"
	And  the element "#footer-link-privacy" should have the href "/help/privacy"

@Javascript:disabled
Scenario: Footer should display Acceptable user link
	When I navigate to /
	Then the element "#footer-link-acceptable-use" should have the text content "Acceptable use"
	And  the element "#footer-link-acceptable-use" should have the href "/help/acceptable-use"

@Javascript:disabled
Scenario: Footer should display Cookies link
	When I navigate to /
	Then the element "#footer-link-cookies" should have the text content "Cookies"
	And  the element "#footer-link-cookies" should have the href "/help/cookies"

@Javascript:disabled
Scenario: Footer should display accessibility link
	When I navigate to /
	Then the element "#footer-link-accessibility" should have the text content "Accessibility statement"
	And  the element "#footer-link-accessibility" should have the href "/help/accessibility"