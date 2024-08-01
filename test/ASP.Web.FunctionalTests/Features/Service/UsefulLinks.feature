Feature: UsefulLinks
@Javascript:disabled
Scenario: Useful links page should be accessible when valid urn is provided and check the provided text description is visible
    Given Establishment "136028" exists:
	"""
	{
        "name": "Dagenham Park CofE School"
    }
	"""
    And Content Template "school-useful-links" exists:
    """
    {
        "Views": [
            {
                "ViewId": "Link",
                "ViewContent": {
                    "Id": "department-for-education", 
                    "Size": "m",
                    "Url": "https://www.gov.uk/government/organisations/department-for-education",
                    "Text": "department-for-education"
                }
             }
         ]
    }
    """
    When I navigate to /school/136028/useful-links
    Then I should get a 200 response
    And the element "h1.govuk-heading-xl" should have the text content "Useful links"
    And the element "*[id='header-navigation-link-my-school']" should have the text content "My school"
    And the element "*[data-testid='my-school-navigation']" should have the class "govuk-header__navigation-item app-header__navigation-item app-header__navigation-item--current"
    And the element "*[data-testid='useful-links-navigation']" should have the text content "Useful links"
    And the element "*[data-testid='useful-links-navigation']" should have the attribute "aria-current" set to "page"
    And the element "*[id='department-for-education']" should have the text content "department-for-education"

