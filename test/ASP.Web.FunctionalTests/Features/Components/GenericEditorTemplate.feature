Feature: Generic component editor

@Javascript:disabled
Scenario: Component with no editor template should be editable by the generic component editor
	Given a content template contains the component:
		"""
		{
			"ViewId": "TestComponent",
			"ViewContent": {
				"StringProperty": "xyz"
			}
		}
		"""
	When I edit the component on the page
	Then there should be no errors
	And the component should exist

@Javascript:disabled
Scenario: Component string property should be editable by a textbox field
	Given a content template contains the component:
		"""
		{
			"ViewId": "TestComponent",
			"ViewContent": {
				"StringProperty": "xyz"
			}
		}
		"""
	When I edit the component on the page
	Then the input element of the component field labelled "StringProperty" should match the selector "input[type="text"]"
	And the component field labelled "StringProperty" should have the value "xyz"

@Javascript:disabled
Scenario: Component string property should be updated from the textbox field when saved
	Given a content template contains the component:
		"""
		{
			"ViewId": "TestComponent",
			"ViewContent": {
				"StringProperty": "xyz"
			}
		}
		"""
	And I edit the component on the page
	When I update the component field labelled "StringProperty" to have the value "abc"
	And I save the component
	Then I should get a 200 response
	And the component template should have property "StringProperty" equal to "abc"

@Javascript:disabled
Scenario: Component int property should be editable by a textbox field
	Given a content template contains the component:
		"""
		{
			"ViewId": "TestComponent",
			"ViewContent": {
				"IntProperty": 123
			}
		}
		"""
	When I edit the component on the page
	Then the input element of the component field labelled "IntProperty" should match the selector "input[type="text"]"
	And the component field labelled "IntProperty" should have the value "123"

@Javascript:disabled
Scenario: Component int property should be updated from the textbox field when saved
	Given a content template contains the component:
		"""
		{
			"ViewId": "TestComponent",
			"ViewContent": {
				"IntProperty": 123
			}
		}
		"""
	And I edit the component on the page
	When I update the component field labelled "IntProperty" to have the value "456"
	And I save the component
	Then I should get a 200 response
	And the component template should have property "IntProperty" equal to 456

@Javascript:disabled
Scenario: Component array property should be editable by a textarea field
	Given a content template contains the component:
		"""
		{
			"ViewId": "TestComponent",
			"ViewContent": {
				"ArrayProperty": [1,2,3]
			}
		}
		"""
	When I edit the component on the page
	Then the input element of the component field labelled "ArrayProperty" should match the selector "textarea"
	And the component field labelled "ArrayProperty" should match the JSON string "[1,2,3]"

@Javascript:disabled
Scenario: Component array property should be updated from the textarea field when saved
	Given a content template contains the component:
		"""
		{
			"ViewId": "TestComponent",
			"ViewContent": {
				"ArrayProperty": [1,2,3]
			}
		}
		"""
	And I edit the component on the page
	When I update the component field labelled "ArrayProperty" to have the value "[4,5,6]"
	And I save the component
	Then I should get a 200 response
	And the component template should match:
		"""
		{
			"ViewId": "TestComponent",
			"ViewContent": {
				"ArrayProperty": [4,5,6]
			}
		}
		"""

@Javascript:disabled
Scenario: Component object property should be editable by a textarea field
	Given a content template contains the component:
		"""
		{
			"ViewId": "TestComponent",
			"ViewContent": {
				"ObjectProperty": {"x":1,"y":2}
			}
		}
		"""
	When I edit the component on the page
	Then the input element of the component field labelled "ObjectProperty" should match the selector "textarea"
	And the component field labelled "ObjectProperty" should match the JSON string "{"x":1,"y":2}"

@Javascript:disabled
Scenario: Component object property should be updated from the textarea field when saved
	Given a content template contains the component:
		"""
		{
			"ViewId": "TestComponent",
			"ViewContent": {
				"ObjectProperty": {"x":1,"y":2}
			}
		}
		"""
	And I edit the component on the page
	When I update the component field labelled "ObjectProperty" to have the value "{"x":3,"y":4}"
	And I save the component
	Then I should get a 200 response
	And the component template should match:
		"""
		{
			"ViewId": "TestComponent",
			"ViewContent": {
				"ObjectProperty": {"x":3,"y":4}
			}
		}
		"""

@Javascript:disabled
Scenario: Component property with null value should be editable by a textarea field
	Given a content template contains the component:
		"""
		{
			"ViewId": "TestComponent",
			"ViewContent": {
				"NullProperty": null
			}
		}
		"""
	When I edit the component on the page
	Then the input element of the component field labelled "NullProperty" should match the selector "textarea"
	And the component field labelled "NullProperty" should have the value ""

@Javascript:disabled
Scenario: Component object property should be updated to null when saved if textarea field is set to "null"
	Given a content template contains the component:
		"""
		{
			"ViewId": "TestComponent",
			"ViewContent": {
				"ObjectProperty": {"x": 1}
			}
		}
		"""
	And I edit the component on the page
	When I update the component field labelled "ObjectProperty" to have the value "null"
	And I save the component
	Then I should get a 200 response
	And the component template should have property "ObjectProperty" equal to null

@Javascript:disabled
Scenario: Component object property should be updated to null when saved if textarea field is empty
	Given a content template contains the component:
		"""
		{
			"ViewId": "TestComponent",
			"ViewContent": {
				"ObjectProperty": {"x": 1}
			}
		}
		"""
	And I edit the component on the page
	When I update the component field labelled "ObjectProperty" to have the value ""
	And I save the component
	Then I should get a 200 response
	And the component template should have property "ObjectProperty" equal to null

@Javascript:disabled
Scenario: Component ViewContent property should be unchanged by the generic editor template when saving unmodified
	Given a content template contains the component:
		"""
		{
			"ViewId": "TestComponent",
			"ViewContent": {
				"StringProperty": "abc",
				"IntProperty": 123,
				"ArrayProperty": [1,2,3],
				"ObjectProperty": {"x":1,"y":2}
			}
		}
		"""
	And I edit the component on the page
	When I save the component
	Then I should get a 200 response
	And the component template should match:
		"""
		{
			"ViewId": "TestComponent",
			"ViewContent": {
				"StringProperty": "abc",
				"IntProperty": 123,
				"ArrayProperty": [1,2,3],
				"ObjectProperty": {"x":1,"y":2}
			}
		}
		"""

@Javascript:disabled
Scenario: Component ChildViews property should be unchanged by the generic editor template when saving unmodified
	Given a content template contains the component:
		"""
		{
			"ViewId": "TestComponent",
			"ChildViews": [
				{
					"ViewId": "TestComponent",
					"ViewContent": {
						"StringProperty": "abc",
						"IntProperty": 123,
						"ArrayProperty": [1,2,3],
						"ObjectProperty": {"x":1,"y":2}
					}
				},
				{
					"ViewId": "TestComponent",
					"ViewContent": {
						"StringProperty": "def",
						"IntProperty": 456,
						"ArrayProperty": [4,5,6],
						"ObjectProperty": {"x":3,"y":4}
					}
				}
			]
		}
		"""
	And I edit the component on the page
	When I save the component
	Then I should get a 200 response
	And the component template should match:
		"""
		{
			"ViewId": "TestComponent",
			"ChildViews": [
				{
					"ViewId": "TestComponent",
					"ViewContent": {
						"StringProperty": "abc",
						"IntProperty": 123,
						"ArrayProperty": [1,2,3],
						"ObjectProperty": {"x":1,"y":2}
					}
				},
				{
					"ViewId": "TestComponent",
					"ViewContent": {
						"StringProperty": "def",
						"IntProperty": 456,
						"ArrayProperty": [4,5,6],
						"ObjectProperty": {"x":3,"y":4}
					}
				}
			]
		}
		"""