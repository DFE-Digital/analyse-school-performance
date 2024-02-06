Feature: Home page

Scenario Outline: Home page paths
	When I navigate to <Path>
	Then I should get a 200 response
	And the page title should be "Home | Analyse school performance"
	And the HTML element with selector "h1.govuk-heading-xl" should have the text content "Analyse school performance"
Examples:
	| Path        |
	| /           |
	| /home       |
	| /home/index |