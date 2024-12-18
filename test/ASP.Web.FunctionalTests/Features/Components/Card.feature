Feature: Card component

@Javascript:disabled
Scenario: Home page cards should contain correct titles
	Given a content template contains the component:
	"""
	{
		"ViewId": "Card",
		"ViewContent": {
			"AuthorisationPolicy": "Any",
			"Title": "Title1"
		}
	}
	"""
	And I am a DfE Named user
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
			"AuthorisationPolicy": "Any",
			"LinkUrl": "/link1/"
		}
	}
	"""
	And I am a DfE Named user
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
			"AuthorisationPolicy": "Any",
			"Text": "Text1"
		}
	}
	"""
	And I am a DfE Named user
	When I view the component on the page
	Then there should be no errors
	Then the element "div.app-card-container > p" within the component should have the text content "Text1"

@Javascript:disabled
Scenario Outline: Card should be shown if user is authorized
	Given a content template contains the component:
	"""
	{
		"ViewId": "Card",
		"ViewContent": {
			"AuthorizationPolicy": "AccessToMySchool",
			"Title": "Title1"
		}
	}
	"""
	And I am a School Named user
	When I view the component on the page
	Then there should be no errors
	And the element "a.app-card-link" within the component should have the text content "Title1"


@Javascript:disabled
Scenario Outline: Card should not be shown if user is not authorized
	Given a content template contains the component:
	"""
	{
		"ViewId": "Card",
		"ViewContent": {
			"AuthorizationPolicy": "AccessToMySchool",
			"Title": "Title1"
		}
	}
	"""
	And I am a LA Named user
	When I view the component on the page
	Then there should be no errors
	And the element "a.app-card-link" should not exist


@Javascript:disabled
Scenario Outline: Card should be shown if the AuthorizationPolicy property is an empty string
	Given a content template contains the component:
	"""
	{
		"ViewId": "Card",
		"ViewContent": {
			"AuthorizationPolicy": "",
			"Title": "Title1"
		}
	}
	"""
	And I am a LA Named user
	When I view the component on the page
	Then there should be no errors
	And the element "a.app-card-link" within the component should have the text content "Title1"


@Javascript:disabled
Scenario Outline: Card should shown if the AuthorizationPolicy property is missing
	Given a content template contains the component:
	"""
	{
		"ViewId": "Card",
		"ViewContent": {
			"Title": "Title1"
		}
	}
	"""
	And I am a LA Named user
	When I view the component on the page
	Then there should be no errors
	And the element "a.app-card-link" within the component should have the text content "Title1"