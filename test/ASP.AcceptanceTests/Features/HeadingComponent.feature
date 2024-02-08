Feature: Heading component

Scenario: Heading text should be populated from template
	Given page content "help-test" exists:
		"""
		{
			"Views": [
				{
					"ViewId": "Heading",
					"ViewContent": {
						"Id": "test",
						"Text": "Test heading text",
					}
				}
			]
		}
		"""
	When I navigate to /help/test
	Then I should get a 200 response
	And the element "#test" should have the text content "Test heading text"

Scenario: Changing heading text should update template
	Given page content "help-test" exists:
		"""
		{
			"Views": [
				{
					"ViewId": "Heading",
					"ViewContent": {
						"Id": "test",
						"Text": "Test heading text",
					}
				}
			]
		}
		"""
	And I navigate to /help/test/edit
	When I update the textbox "#test .asp-content-edit-heading-text" to have the value "Updated heading"
	And I submit the form "#asp-content-edit-form"
	Then I should get a 200 response
	And page content "help-test" should have property "Views[0].ViewContent.Text" set to "Updated heading"