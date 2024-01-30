Feature: Home page

Scenario: Path / points to home page
	When I navigate to /
	Then I should get a 200 response
	And the page title should be "Home | Analyse school performance"
	And the HTML element with selector "h1.govuk-heading-xl" should have the text content "Analyse school performance"

Scenario: Path /home/ points to home page
	When I navigate to /home/
	Then I should get a 200 response
	And the page title should be "Home | Analyse school performance"
	And the HTML element with selector "h1.govuk-heading-xl" should have the text content "Analyse school performance"

Scenario: Path /home/index/ points to home page
	When I navigate to /home/index/
	Then I should get a 200 response
	And the page title should be "Home | Analyse school performance"
	And the HTML element with selector "h1.govuk-heading-xl" should have the text content "Analyse school performance"