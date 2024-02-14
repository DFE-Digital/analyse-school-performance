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
						"HeadingType": "h2",
						"Text": "Test heading text"
					}
				}
			]
		}
		"""
	When I navigate to /help/test
	Then I should get a 200 response
	And the element "#test" should have the following markup:
		"""
		<h2 id="test" class="govuk-heading-l">
			Test heading text
		</h2>
		"""

Scenario: Heading type h2 should set tag to h2 and class to l
	Given page content "help-test" exists:
		"""
		{
			"Views": [
				{
					"ViewId": "Heading",
					"ViewContent": {
						"Id": "test",
						"HeadingType": "h2"
					}
				}
			]
		}
		"""
	When I navigate to /help/test
	Then I should get a 200 response
	And the element "#test" should have the tag name "h2"
	And the element "#test" should have the class "govuk-heading-l"

Scenario: Heading type h3 should set tag to h2 and class to m
	Given page content "help-test" exists:
		"""
		{
			"Views": [
				{
					"ViewId": "Heading",
					"ViewContent": {
						"Id": "test",
						"HeadingType": "h3"
					}
				}
			]
		}
		"""
	When I navigate to /help/test
	Then I should get a 200 response
	And the element "#test" should have the tag name "h3"
	And the element "#test" should have the class "govuk-heading-m"

Scenario: If heading type is missing tag should default to h2 and class to l
	Given page content "help-test" exists:
		"""
		{
			"Views": [
				{
					"ViewId": "Heading",
					"ViewContent": {
						"Id": "test",
						"HeadingType": "h2"
					}
				}
			]
		}
		"""
	When I navigate to /help/test
	Then I should get a 200 response
	And the element "#test" should have the tag name "h2"
	And the element "#test" should have the class "govuk-heading-l"

Scenario: Any other heading type should default to h2 and class to l
	Given page content "help-test" exists:
		"""
		{
			"Views": [
				{
					"ViewId": "Heading",
					"ViewContent": {
						"Id": "test1",
						"HeadingType": "h1"
					}
				},
				{
					"ViewId": "Heading",
					"ViewContent": {
						"Id": "test2",
						"HeadingType": "h4"
					}
				},
				{
					"ViewId": "Heading",
					"ViewContent": {
						"Id": "test3",
						"HeadingType": "xxx"
					}
				}
			]
		}
		"""
	When I navigate to /help/test
	Then I should get a 200 response
	And the element "#test1" should have the tag name "h2"
	And the element "#test1" should have the class "govuk-heading-l"
	And the element "#test2" should have the tag name "h2"
	And the element "#test2" should have the class "govuk-heading-l"
	And the element "#test3" should have the tag name "h2"
	And the element "#test3" should have the class "govuk-heading-l"

Scenario: Caption should appear if Caption text populated in template
Given page content "help-test" exists:
	"""
	{
		"Views": [
			{
				"ViewId": "Heading",
				"ViewContent": {
					"Id": "test",
					"Caption": "Test caption",
					"Text": "Test heading text"
				}
			}
		]
	}
	"""
When I navigate to /help/test
Then I should get a 200 response
And the element "#test" should have the following markup:
	"""
	<h2 id="test" class="govuk-heading-l">
		<span class="govuk-caption-l">Test caption</span>
		Test heading text
	</h2>
	"""

Scenario: Heading type h2 should set caption class to l
	Given page content "help-test" exists:
		"""
		{
			"Views": [
				{
					"ViewId": "Heading",
					"ViewContent": {
						"Id": "test",
						"HeadingType": "h2",
						"Caption": "Test caption"
					}
				}
			]
		}
		"""
	When I navigate to /help/test
	Then I should get a 200 response
	And the element "#test > span" should have the class "govuk-caption-l"

Scenario: Heading type h3 should set caption class to m
	Given page content "help-test" exists:
		"""
		{
			"Views": [
				{
					"ViewId": "Heading",
					"ViewContent": {
						"Id": "test",
						"HeadingType": "h3",
						"Caption": "Test caption"
					}
				}
			]
		}
		"""
	When I navigate to /help/test
	Then I should get a 200 response
	And the element "#test > span" should have the class "govuk-caption-m"

Scenario: If heading type is missing caption class should default to l
	Given page content "help-test" exists:
		"""
		{
			"Views": [
				{
					"ViewId": "Heading",
					"ViewContent": {
						"Id": "test",
						"Caption": "Test caption"
					}
				}
			]
		}
		"""
	When I navigate to /help/test
	Then I should get a 200 response
	And the element "#test > span" should have the class "govuk-caption-l"

Scenario: Any other heading type should default caption class to l
	Given page content "help-test" exists:
		"""
		{
			"Views": [
				{
					"ViewId": "Heading",
					"ViewContent": {
						"Id": "test1",
						"HeadingType": "h1",
						"Caption": "Test caption"
					}
				},
				{
					"ViewId": "Heading",
					"ViewContent": {
						"Id": "test2",
						"Caption": "Test caption"
					}
				},
				{
					"ViewId": "Heading",
					"ViewContent": {
						"Id": "test3",
						"HeadingType": "xxx",
						"Caption": "Test caption"
					}
				}
			]
		}
		"""
	When I navigate to /help/test
	Then I should get a 200 response
	And the element "#test1 > span" should have the class "govuk-caption-l"
	And the element "#test2 > span" should have the class "govuk-caption-l"
	And the element "#test3 > span" should have the class "govuk-caption-l"

Scenario: Changing heading text should update template
	Given page content "help-test" exists:
		"""
		{
			"Views": [
				{
					"ViewId": "Heading",
					"ViewContent": {
						"Id": "test",
						"HeadingType": "h2",
						"Text": "Test heading text"
					}
				}
			]
		}
		"""
	And I navigate to /help/test/edit
	When I update the textbox "#test .app-content-edit-heading-text" to have the value "Updated heading"
	And I submit the form "#app-content-edit-form"
	Then I should get a 200 response
	And page content "help-test" should have property "Views[0].ViewContent.Text" set to "Updated heading"