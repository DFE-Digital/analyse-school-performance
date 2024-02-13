Feature: Link component


Scenario: Link content should display html correctly with link
	Given page content "help-test" exists:
			"""
			{
				"Views": [
					{
						"ViewId": "Link",
						"ViewContent": {
							"Id": "test",
							"Text": "Test link",
							"Url": "https://google.co.uk"
						}
					}
				]
			}
			"""
	When I navigate to /help/test
	Then I should get a 200 response
	Then the element "#test" should have the following markup:
	  """
		<p id="test" class="govuk-body">
			<a href="https://google.co.uk" class="govuk-link">Test link</a>
		</p>
	  """


Scenario: Link content should display html correctly without link
	Given page content "help-test" exists:
			"""
			{
				"Views": [
					{
						"ViewId": "Link",
						"ViewContent": {
							"Id": "test",
							"Text": "Test link",
						}
					}
				]
			}
			"""
	When I navigate to /help/test
	Then I should get a 200 response
	Then the element "#test" should have the following markup:
	  """
		<p id="test" class="govuk-body">
			<a class="govuk-link">Test link</a>
		</p>
	  """


Scenario: Link content should encode html correctly
	Given page content "help-test" exists:
			"""
			{
				"Views": [
					{
						"ViewId": "Link",
						"ViewContent": {
							"Id": "test",
							"Text": "<script>alert('Hello');</script>",
							"Url": "https://google.co.uk"
						}
					}
				]
			}
			"""
	When I navigate to /help/test
	Then I should get a 200 response
	Then the element "#test" should have the following markup:
	  """
		<p id="test" class="govuk-body">
			<a href=https://google.co.uk class="govuk-link">
				&lt;script&gt;alert(&#39;Hello&#39;);&lt;/script&gt;
			</a>
		</p>
	  """