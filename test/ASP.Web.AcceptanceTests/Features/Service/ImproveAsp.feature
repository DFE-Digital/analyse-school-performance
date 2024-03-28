Feature: Improve ASP details

@Javascript:disabled
Scenario: Improve ASP details reveal should contain three external links
	When I navigate to /
	Then I should get a 200 response
	And the element "#details-link-report-issue-asp" should be an external link to "https://form.education.gov.uk/service/Data-collections-service-request-form"
	And the element "#details-link-report-issue-ofsted" should be an external link to "https://contact.ofsted.gov.uk/contact-form"
    And the element "#details-link-report-issue-dsi" should be an external link to "https://help.signin.education.gov.uk/contact/"