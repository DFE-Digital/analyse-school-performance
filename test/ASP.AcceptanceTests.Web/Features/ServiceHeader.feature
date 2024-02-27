Feature: Service Header

Scenario: Header should contain user account and sign in links
	When I navigate to /
	Then I should get a 200 response
	And the element "#header-link-service-name" should have the text content "Analyse school performance"
	And the element "#header-link-account-name" should have the text content "Account name"
	And the element "#header-link-sign-in" should have the text content "Sign in"

Scenario: Header navigation should contain Home, My school and Download data links
	When I navigate to /
	Then I should get a 200 response
	And the element "#header-navigation-link-home" should have the text content "Home"
	And the element "#header-navigation-link-my-school" should have the text content "My school"
	And the element "#header-navigation-link-download-data" should have the text content "Download data"
