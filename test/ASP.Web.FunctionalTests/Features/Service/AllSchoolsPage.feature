Feature: All schools page

    @Javascript:disabled
    Scenario: School Named user is denied access to all schools page
        Given I am a School Named user for Establishment "123456"
        When I navigate to /schools/
        Then I should get a 403 response
        And the element "h1.govuk-heading-l" should have the text content "Access not allowed"

    @Javascript:disabled
    Scenario: MAT Named user is denied access to all schools page
        Given I am a MAT Named user for Multi-Academy Trust "1234"
        When I navigate to /schools/
        Then I should get a 403 response
        And the page title should be "Access not allowed | Analyse school performance"
        And the element "h1.govuk-heading-l" should have the text content "Access not allowed"

    @Javascript:disabled
    Scenario: DfE Named user should see All schools
        Given I am a DfE Named user
        Given Establishment "111111" exists:
        """
        {
        	"name": "Some Primary School",
        	"address": {
        		"street": "13 The Street",
        		"town": "SomeTown",
        		"postCode": "B1 1AA"
        	}
        }
        """
        And Establishment "222222" exists:
        """
        {
        	"name": "Some Other Primary School",
        	"address": {
        		"street": "13 The Road",
        		"town": "Tring",
        		"postCode": "B1 1AA"
        	}
        }
        """
        And Establishment "333333" exists:
        """
        {
        	"name": "A Different Primary School",
        	"address": {
        		"street": "13 The Road",
        		"town": "SomeTown",
        		"postCode": "TR18 3JT"
        	}
        }
        """
        And Establishment "444444" exists:
        """
        {
        	"name": "The Training Centre"
        }
        """
        When I navigate to /schools/
        Then the element "*[data-testid='NumberOfPages-Footer']" should have the text content "Showing 1 - 4 of 4 schools"
        And the element "[data-testid="establishment-listing-urn-<Counter>"]" should have the text content "<URN>"
        And the element "[data-testid="establishment-listing-name-<Counter>"]" should have the text content "<Name>"

    Examples:
      | Counter | URN    | Name                       | Address                        |
      | 1       | 333333 | A Different Primary School | 13 The Road, SomeTown TR18 3JT |
      | 2       | 222222 | Some Other Primary School  | 13 The Road, Tring B1 1AA      |
      | 3       | 111111 | Some Primary School        | 13 The Street, SomeTown B1 1AA |
      | 4       | 444444 | The Training Centre        | No address available           |

    @Javascript:disabled
    Scenario: Pagination in all schools
	    Given I am a DfE Named user
        And 251 Establishments exist with properties:
          | urn          | name                        |
          | (100000 + n) | Primary School (100000 + n) |
        When I navigate to /schools/
        Then the element "*[data-testid='NumberOfPages-Footer']" should have the text content "Showing 1 - 50 of 251 schools"
        And the element "*[data-testid='PageLinks-Footer-1']" should have the href "/schools/?page=1"
        And the element "*[data-testid='PageLinks-Footer-2']" should have the href "/schools/?page=2"
        And the element "*[data-testid='PageLinks-Footer-3']" should have the href "/schools/?page=3"
        And the element "*[data-testid='PageLinks-Footer-4']" should have the href "/schools/?page=4"
        And the element "*[data-testid='PageLinks-Footer-5']" should have the href "/schools/?page=5"
        And the element "*[data-testid='PageLinks-Footer-Next']" should have the href "/schools/?page=2"
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
        
	@Javascript:disabled
	Scenario: Page should show a breadcrumb trail
		Given I am a DfE Named user
		And Establishment "111111" exists:
		"""
		{
			"name": "Some Primary School",
			"address": {
				"street": "13 The Street",
				"town": "SomeTown",
				"postCode": "B1 1AA"
			}
		}
		"""
		When I navigate to /schools/
		Then I should get a 200 response
		And the page title should be "All schools | Analyse school performance"
		And the element "h1.govuk-heading-xl" should have the text content "All schools"
		And the element "[data-testid='breadcrumb-home']" should have the href "/"
		And the element "[data-testid='breadcrumb-home']" should have the text content "Home"
		And the element "[data-testid='breadcrumb-current-page']" should have the text content "All schools"      