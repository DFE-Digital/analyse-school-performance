Feature: School Other reports page

@Javascript:disabled
Scenario Outline: Other reports page should be accessible when valid urn is provided (My school page)
    Given Establishment "136028" exists:
	"""
	{
        "name": "Dagenham Park CofE School"
    }
	"""
    And I am a School Named user for Establishment "136028"
    When I navigate to /my-school/other-reports/
    Then I should get a 200 response
    And the element "h1.govuk-heading-xl" should have the text content "Other reports"
    And the element "[data-testid='app-sub-navigation-item-other-reports']" should have the text content "Other reports"
    And the element "[data-testid='app-sub-navigation-item-other-reports'] a" should have the attribute "aria-current" set to "page"

@Javascript:disabled
Scenario Outline: Other reports page should be accessible when valid urn is provided (Generic school page)
    Given Establishment "136028" exists:
	"""
	{
        "name": "Dagenham Park CofE School"
    }
	"""
    And I am a DfE Named user
    When I navigate to /school/136028/other-reports/
    Then I should get a 200 response
    And the element "h1.govuk-heading-xl" should have the text content "Other reports"
    And the element "[data-testid='app-sub-navigation-item-other-reports']" should have the text content "Other reports"
    And the element "[data-testid='app-sub-navigation-item-other-reports'] a" should have the attribute "aria-current" set to "page"

@Javascript:disabled
Scenario: Other reports page should show the accordion component when javascript disabled
    Given Establishment "136028" exists:
	"""
	{
        "name": "Dagenham Park CofE School"
    }
	"""
    And I am a <User>
    When I navigate to <Path>/other-reports
    Then I should get a 200 response
    And the element "[data-testid='accordion-default-heading-1']" should have the text content "Ofsted inspection data summary reports"
    And the element "[data-testid='accordion-default-heading-2']" should have the text content "School performance summary"
    And the element "[data-testid='accordion-default-heading-3']" should have the text content "Absence and exclusions"
    And the element "[data-testid='accordion-default-heading-4']" should have the text content "School characteristics"
Examples: 
	| User                                           | Path           |
	| School Named user for Establishment "136028"   | /my-school     |
	| School Unnamed user for Establishment "136028" | /my-school     |
	| DfE Named user                                 | /school/136028 |
	| DfE Unnamed user                               | /school/136028 |

@Javascript:enabled
Scenario: Other reports page should show the accordion component when javascript enabled
    Given Content Template "school-other-reports-ofsted" exists:
    """
    {
        "Views": [
            {
                "ViewId": "Paragraph",
                "ViewContent": {
                    "Id": "ofsted-visit-service",
                    "Size": "m",
                    "Text": "For the latest IDSR, [visit the Ofsted IDSR service](https://idsr.ofsted.gov.uk/idsr/{URN}/){target=\"_blank\"}"
                }
            }
        ]
    }
    """
    And Establishment "136028" exists:
	"""
	{
        "name": "Dagenham Park CofE School"
    }
	"""
    And I am a <User>
    When I navigate to <Path>/other-reports
    Then I should get a 200 response
    And the element "[data-testid='accordion-default-heading-1']" should have the text content "Ofsted inspection data summary reports, Show"
    And the element "[data-testid='accordion-default-heading-2']" should have the text content "School performance summary, Show"
    And the element "[data-testid='accordion-default-heading-3']" should have the text content "Absence and exclusions, Show"
    And the element "[data-testid='accordion-default-heading-4']" should have the text content "School characteristics, Show"
    And the element "#ofsted-visit-service a" should have the href "https://idsr.ofsted.gov.uk/idsr/136028/"
Examples: 
    | User                                           | Path           |
    | School Named user for Establishment "136028"   | /my-school     |
    | School Unnamed user for Establishment "136028" | /my-school     |
    | DfE Named user                                 | /school/136028 |
    | DfE Unnamed user                               | /school/136028 |