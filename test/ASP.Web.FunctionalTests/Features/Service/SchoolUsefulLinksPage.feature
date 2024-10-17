Feature: School Useful links page

@Javascript:disabled
Scenario Outline: Other reports page should be accessible when valid urn is provided (My school page)
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
    And I am a School Named user for Establishment "136028"
    When I navigate to /my-school/useful-links/
    Then I should get a 200 response
    And the element "h1.govuk-heading-xl" should have the text content "Useful links"
    And the element "[data-testid='sub-navigation-item-useful-links']" should have the text content "Useful links"
    And the element "[data-testid='sub-navigation-item-useful-links'] a" should have the attribute "aria-current" set to "page"

@Javascript:disabled
Scenario Outline: Other reports page should be accessible when valid urn is provided (Generic school page)
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
    And I am a DfE Named user
    When I navigate to /school/136028/useful-links/
    Then I should get a 200 response
    And the element "h1.govuk-heading-xl" should have the text content "Useful links"
    And the element "[data-testid='sub-navigation-item-useful-links']" should have the text content "Useful links"
    And the element "[data-testid='sub-navigation-item-useful-links'] a" should have the attribute "aria-current" set to "page"