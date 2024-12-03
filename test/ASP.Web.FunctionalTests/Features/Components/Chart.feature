Feature: Chart component
The Chart component provides the ability to add charts to content templates.

@Javascript:disabled
Scenario: When ViewContent property is missing, component should not error
	Given a content template contains the component:
		"""
		{
			"ViewId": "Chart"
		}
		"""
	When I view the component on the page
	Then there should be no errors
	And the component should exist

@Javascript:disabled
Scenario: When ViewContent property is null, component should not error
	Given a content template contains the component:
		"""
		{
			"ViewId": "Chart",
			"ViewContent": null
		}
		"""
	When I view the component on the page
	Then there should be no errors
	And the component should exist

# @Javascript:disabled
# Scenario: When ViewContent property is not an object, component should not error and heading type should default to h2
#	Given a content template contains the component:
#		"""
#		{
#			"ViewId": "Chart",
#			"ViewContent": []
#		}
#		"""
#	When I view the component on the page
#	Then there should be no errors
#	And the component should exist

@Javascript:disabled
Scenario: When Javascript is disabled, chart should display as a table
	Given a content template contains the component:
		"""
		{
			"ViewId": "Chart",
			"ViewContent": {
				"SomeValue": 1234
			}
		}
		"""
	When I view the component on the page
	Then the element ".app-chart-label" within the component should have the text content "This is a table. SomeValue is 1234"

@Javascript:enabled
Scenario: 📜 When Javascript is enabled, chart should display as a graph
	Given a content template contains the component:
		"""
		{
			"ViewId": "Chart",
			"ViewContent": {
				"SomeValue": 1234
			}
		}
		"""
	When I view the component on the page
	Then the element ".app-chart-label" within the component should have the text content "This is a graph. SomeValue is 1234"

@Javascript:enabled
Scenario: 📜 When Javascript is enabled, clicking on view as table should show table view
	Given a content template contains the component:
		"""
		{
			"ViewId": "Chart",
			"ViewContent": {
				"SomeValue": 1234
			}
		}
		"""
	When I view the component on the page
	And I click on the element ".app-chart-toggle" within the component
	Then the element ".app-chart-label" within the component should have the text content "This is a table. SomeValue is 1234"

@Javascript:enabled
Scenario: 📜 When Javascript is enabled, clicking on view as graph should show graph view
	Given a content template contains the component:
		"""
		{
			"ViewId": "Chart",
			"ViewContent": {
				"SomeValue": 1234
			}
		}
		"""
	When I view the component on the page
	And I click on the element ".app-chart-toggle" within the component
	And I click on the element ".app-chart-toggle" within the component
	Then the element ".app-chart-label" within the component should have the text content "This is a graph. SomeValue is 1234"