Feature: Phase feedback banner

Scenario: Phase feedback banner text should contain an external link
	When I navigate to /
	Then I should get a 200 response
	And the anchor "#phase-banner-feedback-link" should be an external link to "https://forms.office.com/Pages/ResponsePage.aspx?id=yXfS-grGoU2187O4s0qC-Xv8GFHtjJRNibkLlIawWjVUOUE5QVJTMFc3NEQ0Q1YyWEZDWVFLMUkwMiQlQCN0PWcu"
