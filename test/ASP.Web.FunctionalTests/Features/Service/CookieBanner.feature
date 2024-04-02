Feature: Analytics cookies banner

@Javascript:disabled
Scenario Outline: Cookie banner should exist when cookie preference has not been set
	Given I navigate to <Path>
	Then the element "#app-cookie-banner" should exist
Examples:
| Path      |
| /         |
| /school   |
| /download |


@Javascript:disabled
Scenario: Analytics cookie banner should contain an 'Accept analytics cookies' button
	When I navigate to /
	Then the element "#app-cookie-banner-accept-button" should have the text content "Accept analytics cookies"


@Javascript:disabled
Scenario: Analytics cookie banner should contain a 'Reject analytics cookies' button
	When I navigate to /
	Then the element "#app-cookie-banner-reject-button" should have the text content "Reject analytics cookies"


@Javascript:disabled
Scenario: Analytics cookie banner should contain an internal link
	When I navigate to /
	Then the element "#app-cookie-banner-view-cookies-link" should be an internal link to "/help/cookies"


@Javascript:disabled
Scenario Outline: Cookie banner should not exist when cookie has been set
    When the cookie "AnalyticsTracking" has been set to "<CookieValue>"
	And I navigate to /
	Then the element "#app-cookie-banner" should not exist
Examples:
| CookieValue |
| Accepted    |
| Rejected    |


@Javascript:disabled
Scenario Outline: Clicking Accept or Reject analytics cookies should show the confirmation banner
    When I navigate to /
    And I click the button "#<ButtonId>"
	Then the element "#app-cookie-confirmation-banner" should exist
Examples: 
| ButtonId                         |
| app-cookie-banner-accept-button |
| app-cookie-banner-reject-button |


@Javascript:disabled
Scenario: Clicking 'Hide this message' button on the cookie confirmation banner removes the banner
    When the cookie "AnalyticsTrackingConfirmation" has been set to "HideBanner"
    And I navigate to /
	Then the element "#app-cookie-confirmation-banner" should not exist


@Javascript:disabled
Scenario: Accepting analytics tracking should add script tag to layout page
    When the cookie "AnalyticsTracking" has been set to "Accepted"
	And I navigate to /
	Then the element "#app-analytics-tracking-code" should exist