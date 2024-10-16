Feature: Generic Local Authorities page

    @Javascript:disabled
    Scenario: School Named user is denied access to local-authorities page
        Given I am a School Named user for Establishment "123456"
        When I navigate to /local-authorities/
        Then I should get a 403 response
        And the element "h1.govuk-heading-l" should have the text content "Access not allowed"

    @Javascript:disabled
    Scenario: La Named user is denied access to local-authorities page
        Given I am a LA Named user for Local Authority "301"
        When I navigate to /local-authorities/
        Then I should get a 403 response
        And the element "h1.govuk-heading-l" should have the text content "Access not allowed"

    @Javascript:disabled
    Scenario: Diocese Named user is denied access to local-authorities page
        Given I am a Diocese Named user
        When I navigate to /local-authorities/
        Then I should get a 403 response
        And the element "h1.govuk-heading-l" should have the text content "Access not allowed"

    @Javascript:disabled
    Scenario: Display an error page for DfE Named User when no local authorities are found due to server errors.
        Given I am a DfE Named user
        And no Local Authorities exist
        When I navigate to /local-authorities/
        Then I should get a 500 response
        And the page title should be "Sorry, there is a problem with the service | Analyse school performance"

    @Javascript:disabled
    Scenario: Pagination in Generic Local Authorities page
        Given I am a DfE Named user
        And 251 Local Authorities exist with properties:
          | code      | name                        |
          | (100 + n) | ASP Test LA Named (100 + n) |
        When I navigate to /local-authorities/
        Then the element "*[data-testid='NumberOfPages-Header']" should have the text content "Showing 1 - 50 of 251 local authorities"
        And the element "*[data-testid='PageLinks-Header-1']" should have the href "/local-authorities/?page=1"
        And the element "*[data-testid='PageLinks-Header-2']" should have the href "/local-authorities/?page=2"
        And the element "*[data-testid='PageLinks-Header-3']" should have the href "/local-authorities/?page=3"
        And the element "*[data-testid='PageLinks-Header-4']" should have the href "/local-authorities/?page=4"
        And the element "*[data-testid='PageLinks-Header-5']" should have the href "/local-authorities/?page=5"
        And the element "*[data-testid='PageLinks-Header-Next']" should have the href "/local-authorities/?page=2"
        And the element "*[data-testid='all-local-authorities-listing-name-1']" should have the text content "ASP Test LA Named 101"
        And the element "*[data-testid='all-local-authorities-listing-name-2']" should have the text content "ASP Test LA Named 102"
        And the element "*[data-testid='all-local-authorities-listing-name-3']" should have the text content "ASP Test LA Named 103"
        And the element "*[data-testid='all-local-authorities-listing-name-4']" should have the text content "ASP Test LA Named 104"
        And the element "*[data-testid='all-local-authorities-listing-name-5']" should have the text content "ASP Test LA Named 105"
        And the element "*[data-testid='all-local-authorities-listing-name-1']" should have the href "/local-authority/101/"
        And the element "*[data-testid='all-local-authorities-listing-name-2']" should have the href "/local-authority/102/"
        And the element "*[data-testid='all-local-authorities-listing-name-3']" should have the href "/local-authority/103/"
        And the element "*[data-testid='all-local-authorities-listing-name-4']" should have the href "/local-authority/104/"
        And the element "*[data-testid='all-local-authorities-listing-name-5']" should have the href "/local-authority/105/"
        And the element "*[data-testid='all-local-authorities-listing-code-1']" should have the text content "101"
        And the element "*[data-testid='all-local-authorities-listing-code-2']" should have the text content "102"
        And the element "*[data-testid='all-local-authorities-listing-code-3']" should have the text content "103"
        And the element "*[data-testid='all-local-authorities-listing-code-4']" should have the text content "104"
        And the element "*[data-testid='all-local-authorities-listing-code-5']" should have the text content "105"
        
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
        When I navigate to /local-authorities/
        Then I should get a 200 response
        And the page title should be "All local authorities | Analyse school performance"
        And the element "h1.govuk-heading-xl" should have the text content "All local authorities"
        And the element "[data-testid='breadcrumb-home']" should have the href "/"
        And the element "[data-testid='breadcrumb-home']" should have the text content "Home"
        And the element "[data-testid='breadcrumb-current-page']" should have the text content "All local authorities"   