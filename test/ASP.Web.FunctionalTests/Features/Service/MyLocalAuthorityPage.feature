Feature: My local authority page

@Javascript:disabled
Scenario: A non-LA user should not be able to access the 'My local authority' page. Instead, they should see a 403 Access not allowed page.
    Given I am a MAT Named user for Multi-Academy Trust "1234"
    When I navigate to /my-local-authority/
    Then I should get a 403 response
    And the page title should be "Access not allowed | Analyse school performance"
    And the element "h1.govuk-heading-l" should have the text content "Access not allowed"

@Javascript:disabled
Scenario: A user with access to all LAs should not be able to access the 'My local authority' page. Instead, they should see a 403 Access not allowed page.
    Given I am a DfE Named user
    When I navigate to /my-local-authority/
    Then I should get a 403 response
    And the page title should be "Access not allowed | Analyse school performance"
    And the element "h1.govuk-heading-l" should have the text content "Access not allowed"

@Javascript:disabled
Scenario: My local authority page should display server error page if user's LA does not exist
    Given Local Authority "301" exists:
    """
    {
        "Name": "Test Name",
        "Code": "301"
    }
    """
    And I am a LA Named user for Local Authority "302"
    When I navigate to /my-local-authority/
    Then I should get a 500 response
    And the page title should be "Sorry, there is a problem with the service | Analyse school performance"
    And the element "h1.govuk-heading-l" should have the text content "Sorry, there is a problem with the service"
    And the element "*[data-testid='error-display-message']" should have the text content "Error message: API error: Could not find Local Authority with code "302"."
        
@Javascript:disabled
Scenario: My local authority page should be accessible if user's LA exists
    Given I am a LA Named user for Local Authority "301"
    And Local Authority "301" exists:
    """
    {
        "Name": "Test Name",
        "Code": "301"
    }
    """
    When I navigate to /my-local-authority/
    Then I should get a 200 response
    And the page title should be "My local authority | Analyse school performance"
    And the element "h1.govuk-heading-xl" should have the text content "My local authority"
    And the element "[data-testid='all-school-in-la-sub-title']" should have the text content "All schools within Test Name"
    And the element "[data-testid='breadcrumb-home']" should have the href "/"
    And the element "[data-testid='breadcrumb-home']" should have the text content "Home"
    And the element "[data-testid='breadcrumb-current-page']" should have the text content "My local authority"

@Javascript:disabled
Scenario: My local authority page cards should be populated from the "la-landing-page" content template
    Given I am a LA Unnamed user for Local Authority "301"
    And Local Authority "301" exists:
    """
    {
        "Name": "Test Name",
        "Code": "301"
    }
    """
    And Content Template "la-landing-page" exists:
    """
    {
        "Views": [
        	{
                "ViewId": "Card",
                "ViewContent": {
                    "Id": "app-card-download-data",
                    "Title": "Download data",
                    "LinkUrl": "download-data",
                    "Text": "Download data for Analyse school performance and Key to success."
                }
            }
        ]
    }
    """
    When I navigate to /my-local-authority/
    Then the element "#app-card-download-data h2 a" should have the href "download-data"
    And the element "#app-card-download-data h2 a" should have the text content "Download data"
    And the element "#app-card-download-data p" should have the text content "Download data for Analyse school performance and Key to success."