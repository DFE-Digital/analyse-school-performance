Feature: Card component

@Javascript:disabled
Scenario: Home page cards should contain correct titles
	Given a content template contains the component:
	"""
	{
		"ViewId": "Card",
		"ViewContent": {
			"Title": "Title1"
		}
	}
	"""
	When I view the component on the page
	Then there should be no errors
	And the element "a.app-card-link" within the component should have the text content "Title1"

@Javascript:disabled
Scenario: Home page cards should contain correct link URLs
	Given a content template contains the component:
	"""
	{
		"ViewId": "Card",
		"ViewContent": {
			"LinkUrl": "/link1/"
		}
	}
	"""
	When I view the component on the page
	Then there should be no errors
	Then the element "div.app-card-container > h2 > a" within the component should have the href "/link1/"

@Javascript:disabled
Scenario: Home page cards should contain correct text
	Given a content template contains the component:
	"""
	{
		"ViewId": "Card",
		"ViewContent": {
			"Text": "Text1"
		}
	}
	"""
	When I view the component on the page
	Then there should be no errors
	Then the element "div.app-card-container > p" within the component should have the text content "Text1"