Feature: Improve ASP details

Background:
	Given I am a logged-in user

@Javascript:disabled
Scenario: Improve ASP details reveal should contain three external links
	When I navigate to /
	Then I should get a 200 response
	And the element "#details-link-report-issue-asp" should have the href "https://form.education.gov.uk/service/Data-collections-service-request-form"
	And the element "#details-link-report-issue-ofsted" should have the href "https://contact.ofsted.gov.uk/contact-form"
	And the element "#details-link-report-issue-dsi" should have the href "https://help.signin.education.gov.uk/contact/"