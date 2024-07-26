Feature: LocalAuthority

    @Javascript:disabled
    Scenario: my local authority page should be accessible when valid code is provided
        Given localAuthority "301" exists:
        """
        {
                  "Name": "Test Name",
                  "Code": "301"
              }
        """
        When I navigate to /my-local-authority/301
        Then I should get a 200 response
        And the page title should be "My local authority | Analyse school performance"
        And the element "h1.govuk-heading-xl" should have the text content "My local authority"
        And the element "[data-testid='all-school-in-la-sub-title']" should have the text content "All schools within Test Name"
        And the element "[data-testid='breadcrumb-home']" should have the href "/"
        And the element "[data-testid='breadcrumb-home']" should have the text content "Home"
        And the element "[data-testid='breadcrumb-current-page']" should have the text content "My local authority"

    @Javascript:disabled
    Scenario: my local authority page cards should be populated from the "la-landing-page" content template

        Given localAuthority "301" exists:
        """
        {
                  "Name": "Test Name",
                  "Code": "301"
              }
        """

        And content template "la-landing-page" exists:
        """
        {
        	"Views": [
        		 {
                          "ViewId": "Card"
                 }
        	]
        }
        """

        When I navigate to /my-local-authority/301
        Then the element "#app-card-container" class should contain "app-grid-container-three-column"
        And the elements "#app-card-container .app-card" should total 1

    @Javascript:disabled
    Scenario: my local authority page cards should be populated correctly
        Given content template "la-landing-page" exists:
        """
        {
        	"Views": [
        		    {
                          "ViewId": "Card",
                          "ViewContent": {
                              "Id": "app-card-la-download",
                              "Title": "Download data",
                              "LinkUrl": "la-download",
                              "Text": "Download data for Analyse school performance and Key to success."
                          }
                      }
        	]
        }
        """
        And localAuthority "302" exists:
        """
        {
                  "Name": "Test Name",
                  "Code": "302"
        }
        """
        When I navigate to /my-local-authority/302
        Then the element "#app-card-la-download h2 a" should have the href "la-download"
        And the element "#app-card-la-download h2 a" should have the text content "Download data"
        And the element "#app-card-la-download p" should have the text content "Download data for Analyse school performance and Key to success."