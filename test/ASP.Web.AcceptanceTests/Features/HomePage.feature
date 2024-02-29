Feature: Home page

Scenario Outline: Home page should be accessible from multiple paths
	When I navigate to <Path>
	Then I should get a 200 response
	And the page title should be "Home | Analyse school performance"
	And the element "h1.govuk-heading-xl" should have the text content "Analyse school performance"
Examples:
	| Path        |
	| /           |
	| /home       |
	| /home/index |