Feature: Link component

@Javascript:disabled
Scenario: Link content should display html correctly with link
	Given a content template contains the component:
		"""
		{
			"ViewId": "Link",
			"ViewContent": {
				"Id": "test",
				"Text": "Test link",
				"Url": "https://google.co.uk"
			}
		}
		"""
	When I view the component on the page
	Then there should be no errors
	And the component should exist
	And the component outer element should have the class "govuk-body"
	And the element "a" within the component should have the class "govuk-link"
	And the element "a" within the component should have the text content "Test link"
	And the element "a" within the component should have the href "https://google.co.uk"

@Javascript:disabled
Scenario: Link content should display html correctly without link
	Given a content template contains the component:
			"""
			{
				"ViewId": "Link",
				"ViewContent": {
					"Id": "test",
					"Text": "Test link",
				}
			}
			"""
	When I view the component on the page
	Then there should be no errors
	And the element "a" within the component should have the class "govuk-link"
	And the element "a" within the component should have the text content "Test link"
	And the element "a" within the component should have the href ""


@Javascript:disabled
Scenario: Link content should encode html correctly
	Given a content template contains the component:
			"""
			{
				"ViewId": "Link",
				"ViewContent": {
					"Id": "test",
					"Text": "<script>alert('Hello');</script>",
					"Url": "https://google.co.uk"
				}
			}
			"""
	When I view the component on the page
	Then there should be no errors
	And the element "a" within the component should have the inner HTML "&lt;script&gt;alert(&#39;Hello&#39;);&lt;/script&gt;"