Feature: Generic All Schools Local Authority page

    @Javascript:disabled
    Scenario Outline: The 'NotAccessToAllSchools' user should not be able to access 'Generic All schools page for Local Authority' page.
        Given I am a <NotAccessToAllSchools> user
        When I navigate to /local-authority/301/schools
        Then I should get a 403 response
        And the page title should be "Access not allowed | Analyse school performance"
        And the element "h1.govuk-heading-l" should have the text content "Access not allowed"

        Examples:
          | NotAccessToAllSchools |
          | LA Named              |
          | LA Unnamed            |
          | MAT Named             |
          | MAT Unnamed           |
          | MAT Governor          |
          | Diocese Named         |
          | Diocese Unnamed       |

    @Javascript:disabled
    Scenario Outline: Generic All Schools Local Authority page should have the correct title and subtitle for 'AccessToAllSchools' users
        Given 251 Establishments exist with properties:
          | urn          | name                        | localAuthority                       |
          | (100000 + n) | Primary School (100000 + n) | { "code": "999", "name": "Test LA" } |
        And Local Authority "999" exists:
        """
        {
            "Name": "Test LA",
            "Code": "999"
        }
        """
        And I am a <AccessToAllSchools> user
        When I navigate to /local-authority/999/schools
        Then I should get a 200 response
        Then the page title should be "All schools | Analyse school performance"
        Then the element "*[data-testid='schools-title']" should have the text content "All schools"
        Then the element "*[data-testid='schools-subtitle']" should have the text content "Test LA - 251 schools"

        Examples:
          | AccessToAllSchools |
          | DfE Named          |
          | DfE Unnamed        |
          | Ofsted Unnamed     |
          | Super Admin        |

    @Javascript:disabled
    Scenario Outline: Page should show a breadcrumb trail for 'AccessToAllSchools' users
        Given I am a <AccessToAllSchools> user
        And Establishment "111111" exists:
        """
            {
              "name": "Test School 1",
              "localAuthority": {
        		"code": "999",
               	"name": "Oxfordshire"
        	  }
            }
        """
        And Local Authority "999" exists:
        """
        {
            "Name": "Oxfordshire",
            "Code": "999"
        }
        """
        When I navigate to /local-authority/999/schools
        Then I should get a 200 response
        Then the page title should be "All schools | Analyse school performance"
        And the element "[data-testid='breadcrumb-home']" should have the href "/"
        And the element "[data-testid='breadcrumb-home']" should have the text content "Home"
        And the element "[data-testid='breadcrumb-all-local-authorities']" should have the href "/local-authorities"
        And the element "[data-testid='breadcrumb-all-local-authorities']" should have the text content "All local authorities"
        And the element "[data-testid='breadcrumb-oxfordshire']" should have the href "/local-authority/999"
        And the element "[data-testid='breadcrumb-oxfordshire']" should have the text content "Oxfordshire"
        And the element "[data-testid='breadcrumb-current-page']" should have the text content "All schools"

        Examples:
          | AccessToAllSchools |
          | DfE Named          |
          | DfE Unnamed        |
          | Ofsted Unnamed     |
          | Super Admin        |

    @Javascript:disabled
    Scenario Outline: Pagination in Generic All Schools Local Authority page for 'AccessToAllSchools' users
        Given I am a <AccessToAllSchools> user
        And 251 Establishments exist with properties:
          | urn          | name                        | localAuthority                       |
          | (100000 + n) | Primary School (100000 + n) | { "code": "999", "name": "Test LA" } |
        And Local Authority "999" exists:
        """
        {
            "Name": "Test LA",
            "Code": "999"
        }
        """
        When I navigate to /local-authority/999/schools
        Then the element "*[data-testid='NumberOfPages-Footer']" should have the text content "Showing 1 - 50 of 251 schools"
        And the element "*[data-testid='PageLinks-Footer-1']" should have the href "/local-authority/999/schools/?page=1"
        And the element "*[data-testid='PageLinks-Footer-2']" should have the href "/local-authority/999/schools/?page=2"
        And the element "*[data-testid='govuk-pagination__link--Footer']" should have the text content "..."
        And the element "*[data-testid='PageLinks-Footer-6']" should have the href "/local-authority/999/schools/?page=6"
        And the element "*[data-testid='PageLinks-Footer-Next']" should have the href "/local-authority/999/schools/?page=2"
        And the element "*[data-testid='establishment-listing-name-1']" should have the text content "Primary School 100001"
        And the element "*[data-testid='establishment-listing-name-2']" should have the text content "Primary School 100002"
        And the element "*[data-testid='establishment-listing-name-3']" should have the text content "Primary School 100003"
        And the element "*[data-testid='establishment-listing-name-4']" should have the text content "Primary School 100004"
        And the element "*[data-testid='establishment-listing-name-5']" should have the text content "Primary School 100005"
        And the element "*[data-testid='establishment-listing-urn-1']" should have the text content "100001"
        And the element "*[data-testid='establishment-listing-urn-2']" should have the text content "100002"
        And the element "*[data-testid='establishment-listing-urn-3']" should have the text content "100003"
        And the element "*[data-testid='establishment-listing-urn-4']" should have the text content "100004"
        And the element "*[data-testid='establishment-listing-urn-5']" should have the text content "100005"

        Examples:
          | AccessToAllSchools |
          | DfE Named          |
          | DfE Unnamed        |
          | Ofsted Unnamed     |
          | Super Admin        |