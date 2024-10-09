Feature: Generic Local authority page

@Javascript:disabled
Scenario: A user with no access to all Local Authorities should not be able to access the generic 'Local authority' page. Instead, they should see a 403 Access not allowed page.
    Given I am a MAT Named user for Multi-Academy Trust "1234"
    When I navigate to /local-authority/301/
    Then I should get a 403 response
    And the page title should be "Access not allowed | Analyse school performance"
    And the element "h1.govuk-heading-l" should have the text content "Access not allowed"

@Javascript:disabled
Scenario: An LA user should not be able to access the generic 'Local authority' page, even if it's for their own LA. Instead, they should see a 403 Access not allowed page.
    Given I am a LA Named user for Local Authority "301"
    When I navigate to /local-authority/301/
    Then I should get a 403 response
    And the page title should be "Access not allowed | Analyse school performance"
    And the element "h1.govuk-heading-l" should have the text content "Access not allowed"

@Javascript:disabled
Scenario: Local authority page should not be found when invalid code is provided
    Given I am a DfE Named user
    And Local Authority "301" exists:
    """
    {
        "Name": "Test Name",
        "Code": "301"
    }
    """
    When I navigate to /local-authority/302/
    Then I should get a 404 response
    And the page title should be "Page not found | Analyse school performance"
    And the element "h1.govuk-heading-l" should have the text content "Page not found"

@Javascript:disabled
Scenario: Local authority page should be accessible when valid code is provided
    Given I am a DfE Named user
    And Local Authority "301" exists:
    """
    {
        "Name": "Test Name",
        "Code": "301"
    }
    """
    When I navigate to /local-authority/301/
    Then I should get a 200 response
    And the page title should be "Test Name | Analyse school performance"
    And the element "h1.govuk-heading-xl" should have the text content "Test Name"
    And the element "[data-testid='all-school-in-la-sub-title']" should have the text content "All schools within Test Name"
    And the element "[data-testid='breadcrumb-home']" should have the href "/"
    And the element "[data-testid='breadcrumb-home']" should have the text content "Home"
    And the element "[data-testid='breadcrumb-current-page']" should have the text content "Test Name"

@Javascript:disabled
Scenario: Local authority page cards should be populated from the "la-landing-page" content template
    Given I am a DfE Named user
    And Content Template "la-landing-page" exists:
    """
    {
        "Views": [
        	{
                "ViewId": "Card",
                "ViewContent": {
                    "Id": "app-card-la-all-schools",
                    "Title": "All schools",
                    "LinkUrl": "schools/",
                    "Text": "All schools found in this LA."
                }
            },
            {
                "ViewId": "Card",
                "ViewContent": {
                    "Id": "app-card-la-download",
                    "Title": "Download data",
                    "LinkUrl": "download-data/",
                    "Text": "Download data for Analyse school performance and Key to success."
                }
            }
        ]
    }
    """
    And Local Authority "302" exists:
    """
    {
        "Name": "Test Name",
        "Code": "302"
    }
    """
    When I navigate to /local-authority/302/
    Then the element "#app-card-la-all-schools h2 a" should have the href "schools/"
    And the element "#app-card-la-all-schools h2 a" should have the text content "All schools"
    And the element "#app-card-la-all-schools p" should have the text content "All schools found in this LA."
    Then the element "#app-card-la-download h2 a" should have the href "download-data/"
    And the element "#app-card-la-download h2 a" should have the text content "Download data"
    And the element "#app-card-la-download p" should have the text content "Download data for Analyse school performance and Key to success."
    
@Javascript:disabled
Scenario: Page should show a breadcrumb trail
    Given I am a DfE Named user
    And Local Authority "301" exists:
    """
    {
        "Name": "Test Name",
        "Code": "301"
    }
    """
    When I navigate to /local-authority/301/
    Then I should get a 200 response
    And the page title should be "Test Name | Analyse school performance"
    And the element "[data-testid='all-school-in-la-sub-title']" should have the text content "All schools within Test Name"
    And the element "[data-testid='breadcrumb-home']" should have the href "/"
    And the element "[data-testid='breadcrumb-all-local-authorities']" should have the text content "All local authorities"
    And the element "[data-testid='breadcrumb-all-local-authorities']" should have the href "/local-authorities/"
    And the element "[data-testid='breadcrumb-current-page']" should have the text content "Test Name"