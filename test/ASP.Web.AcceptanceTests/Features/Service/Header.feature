Feature: Service header

@Javascript:disabled
Scenario: Header should contain user account and sign in links
	When I navigate to /
	Then the element "#header-link-service-name" should have the text content "Analyse school performance"
	# Commenting out until implemented
	# And the element "#header-link-account-name" should have the text content "Account name"
	# And the element "#header-link-sign-in" should have the text content "Sign in"

@Javascript:disabled
Scenario: Header navigation should contain Home link
	When I navigate to /
	Then the element "#header-navigation-link-home" should have the text content "Home"
	And  the element "#header-navigation-link-home" should be an internal link to "/"

@Javascript:disabled
Scenario: Header navigation should contain My school link
	When I navigate to /
	Then the element "#header-navigation-link-my-school" should have the text content "My school"
	And  the element "#header-navigation-link-my-school" should be an internal link to "/school"

@Javascript:disabled
Scenario: Header navigation should contain Download data link
	When I navigate to /
	Then the element "#header-navigation-link-download" should have the text content "Download data"
	And  the element "#header-navigation-link-download" should be an internal link to "/download"

@Javascript:disabled
Scenario: Header navigation should contain Site news link
	When I navigate to /
	Then the element "#header-navigation-link-news" should have the text content "Site news"
	And  the element "#header-navigation-link-news" should be an internal link to "/news"

@Javascript:disabled
Scenario: Header navigation should contain Release timetable link
	When I navigate to /
	Then the element "#header-navigation-link-release-timetable" should have the text content "Release timetable"
	And  the element "#header-navigation-link-release-timetable" should be an internal link to "/help/release-timetable"

@Javascript:disabled
Scenario: Header navigation should contain Guidance link
	When I navigate to /
	Then the element "#header-navigation-link-guidance" should have the text content "Guidance"
	And  the element "#header-navigation-link-guidance" should be an internal link to "/help/guidance"