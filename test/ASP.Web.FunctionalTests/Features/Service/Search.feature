Feature: Search Page

@Javascript:disabled
Scenario: Page title should show correct text when search returns results
    Given establishment "111111" exists:
    """
    {
      "urn": "111111",
      "name": "Some Primary School",
      "address": {
            "street": "13 The Street",
            "town": "SomeTown",
            "postCode": "B1 1AA"
        }
    }
    """
    And establishment "222222" exists:
    """
    {
      "urn": "222222",
      "name": "Some Other Primary School",
      "address": {
            "street": "13 The Road",
            "town": "Tring",
            "postCode": "B1 1AA"
        }
    }
    """
    When I navigate to /search/
	And I update the textbox "#searchTerm" to have the value "Primary"
	And I click the button "#searchSubmit"
    Then the page title should be "Search results for Primary | Analyse school performance"

@Javascript:disabled
Scenario: Page title should show correct text when search returns no results
    Given establishment "111111" exists:
    """
    {
      "urn": "111111",
      "name": "Some Primary School",
      "address": {
            "street": "13 The Street",
            "town": "SomeTown",
            "postCode": "B1 1AA"
        }
    }
    """
    And establishment "222222" exists:
    """
    {
      "urn": "222222",
      "name": "Some Other Primary School",
      "address": {
            "street": "13 The Road",
            "town": "Tring",
            "postCode": "B1 1AA"
        }
    }
    """
    When I navigate to /search/
	And I update the textbox "#searchTerm" to have the value "Secondary"
	And I click the button "#searchSubmit"
    Then the page title should be "We found no matching results for Secondary | Analyse school performance"

@Javascript:disabled
Scenario: Page should show a breadcrumb trail when search returns results
    Given establishment "111111" exists:
    """
    {
      "urn": "111111",
      "name": "Some Primary School",
      "address": {
            "street": "13 The Street",
            "town": "SomeTown",
            "postCode": "B1 1AA"
        }
    }
    """
    And establishment "222222" exists:
    """
    {
      "urn": "222222",
      "name": "Some Other Primary School",
      "address": {
            "street": "13 The Road",
            "town": "Tring",
            "postCode": "B1 1AA"
        }
    }
    """
    When I navigate to /search/
	And I update the textbox "#searchTerm" to have the value "Primary"
	And I click the button "#searchSubmit"
    Then the element "[data-testid='breadcrumb-home']" should have the href "/"
    And the element "[data-testid='breadcrumb-search']" should have the text content "Search"
    And the element "[data-testid='breadcrumb-current-page']" should have the text content "Search results for Primary"

@Javascript:disabled
Scenario: Page should show a breadcrumb trail when search returns no results
    Given establishment "111111" exists:
    """
    {
      "urn": "111111",
      "name": "Some Primary School",
      "address": {
            "street": "13 The Street",
            "town": "SomeTown",
            "postCode": "B1 1AA"
        }
    }
    """
    And establishment "222222" exists:
    """
    {
      "urn": "222222",
      "name": "Some Other Primary School",
      "address": {
            "street": "13 The Road",
            "town": "Tring",
            "postCode": "B1 1AA"
        }
    }
    """
    When I navigate to /search/
	And I update the textbox "#searchTerm" to have the value "Secondary"
	And I click the button "#searchSubmit"
    Then the element "[data-testid='breadcrumb-home']" should have the href "/"
    And the element "[data-testid='breadcrumb-search']" should have the text content "Search"
    And the element "[data-testid='breadcrumb-current-page']" should have the text content "We found no matches for Secondary"
 
@Javascript:disabled
Scenario: Search Term Validation
    When I navigate to /search/
	Then I should get a 200 response
	And the page title should be "Search | Analyse school performance"
	And the element "h1.govuk-heading-l" should have the text content "Search for a school"
	And the element "*[data-testid='searchTerm']" should have the text content "Enter school name, address, URN (Unique Reference Number) or LAESTAB (Local Authority Establishment Number) Search"

@Javascript:disabled
Scenario: Errors in Search Term Validation
    When I navigate to /search/
	And I click the button "#searchSubmit"
	Then the element "#searchTerm-input-error" should have the text content "Please enter a search term such as a school name, address, URN (Unique Reference Number) or LAESTAB (Local Authority Establishment Number)"
    And the element "h2.govuk-error-summary__title" should have the text content "Please correct the following error(s)."
	And the element "*[data-testid='SearchTerm']" should have the text content "Enter school name, address or reference number"

@Javascript:disabled
Scenario: School search page should show correct message when there is no data
	Given no establishments exist
	When I navigate to /search/
	And I update the textbox "#searchTerm" to have the value "primary"
	And I click the button "#searchSubmit"
    Then the element "h1" should have the text content "We found no matches for "primary""

@Javascript:disabled
Scenario: School search page should show correct message for search term with no matches
	Given establishment "111111" exists:
	"""
	{
	  "urn": "111111",
	  "name": "Some Primary School"
	}
	"""
    When I navigate to /search/
	And I update the textbox "#searchTerm" to have the value "secondary"
	And I click the button "#searchSubmit"
    Then the element "h1" should have the text content "We found no matches for "secondary""

@Javascript:disabled
Scenario: Matching URN search should redirect to school landing page
	Given establishment "111111" exists:
		"""
		{
		  "urn": "111111",
		  "name": "Some Primary School"
		}
		"""
    When I navigate to /search/
	And I update the textbox "#searchTerm" to have the value "111111"
	And I click the button "#searchSubmit"   
    Then the path should match "/school/111111/"
    And the element "[data-testid="school-page-school-name"]" should have the text content "Some Primary School (URN: 111111)"

@Javascript:disabled
Scenario: Partial match for school name should redirect to school landing page
	Given establishment "111111" exists:
		"""
		{
		  "urn": "111111",
		  "name": "Some Primary School"
		}
		"""
    When I navigate to /search/
	And I update the textbox "#searchTerm" to have the value "PRiMaRY"
	And I click the button "#searchSubmit"  
    Then the path should match "/school/111111/"
    And the element "[data-testid="school-page-school-name"]" should have the text content "Some Primary School (URN: 111111)"

@Javascript:disabled
Scenario: Partial street match should redirect to school landing page
	Given establishment "111111" exists:
		"""
        {
		     "urn": "111111",
             "name": "Some Primary School",
             "address": {
                "street": "13 The Street",
                "town": "SomeTown",
                "postCode": "TR18 3JT"
             }
        } 
		"""
    When I navigate to /search/
	And I update the textbox "#searchTerm" to have the value "str"
	And I click the button "#searchSubmit"  
    Then the path should match "/school/111111/"
    And the element "[data-testid="school-details-address-value"]" should have the text content "13 The Street, SomeTown, TR18 3JT"

@Javascript:disabled
Scenario: Partial town match should redirect to school landing page
	Given establishment "111111" exists:
		"""
        {
		     "urn": "111111",
             "name": "Some Primary School",
             "address": {
                "street": "13 The Street",
                "town": "SomeTown",
                "postCode": "TR18 3JT"
             }
        } 
		"""
    When I navigate to /search/
	And I update the textbox "#searchTerm" to have the value "some"
	And I click the button "#searchSubmit"  
    Then the path should match "/school/111111/"
    And the element "[data-testid="school-details-address-value"]" should have the text content "13 The Street, SomeTown, TR18 3JT"

@Javascript:disabled
Scenario: Partial postcode match should redirect to school landing page
	Given establishment "111111" exists:
		"""
        {
		     "urn": "111111",
             "name": "Some Primary School",
             "address": {
                "street": "13 The Street",
                "town": "SomeTown",
                "postCode": "TR18 3JT"
             }
        } 
		"""
    When I navigate to /search/
	And I update the textbox "#searchTerm" to have the value "tr1"
	And I click the button "#searchSubmit"   
    Then the path should match "/school/111111/"
    And the element "[data-testid="school-details-address-value"]" should have the text content "13 The Street, SomeTown, TR18 3JT"

@Javascript:disabled
Scenario Outline: Results page should show partial name and address matches
    Given establishment "111111" exists:
    """
    {
      "urn": "111111",
      "name": "Some Primary School",
      "address": {
            "street": "13 The Street",
            "town": "SomeTown",
            "postCode": "B1 1AA"
        }
    }
    """
    And establishment "222222" exists:
    """
    {
      "urn": "222222",
      "name": "Some Other Primary School",
      "address": {
            "street": "13 The Road",
            "town": "Tring",
            "postCode": "B1 1AA"
        }
    }
    """
    And establishment "333333" exists:
    """
    {
      "urn": "333333",
      "name": "A Different Primary School",
      "address": {
            "street": "13 The Road",
            "town": "SomeTown",
            "postCode": "TR18 3JT"
        }
    }
    """
    And establishment "444444" exists:
    """
    {
      "urn": "444444",
      "name": "The Training Centre"
    }
    """
    When I navigate to /search/
	And I update the textbox "#searchTerm" to have the value "tr"
	And I click the button "#searchSubmit"
    Then the element "[data-testid="school-search-results-urn-<Counter>"]" should have the text content "<URN>"
    And the element "[data-testid="school-search-results-name-<Counter>"]" should have the text content "<Name>"
    And the element "[data-testid="school-search-results-address-<Counter>"]" should have the text content "<Address>"
Examples: 
| Counter | URN    | Name                       | Address                         |
| 1       | 333333 | A Different Primary School | 13 The Road, SomeTown, TR18 3JT |
| 2       | 222222 | Some Other Primary School  | 13 The Road, Tring, B1 1AA      |
| 3       | 111111 | Some Primary School        | 13 The Street, SomeTown, B1 1AA |
| 4       | 444444 | The Training Centre        ||

@Javascript:disabled
Scenario: School search successful for 6-digit URN
	Given establishment "111111" exists:
		"""
		{
		  "urn": "111111",
		  "name": "Some Primary School"
		}
		"""
    When I navigate to /search/
	And I update the textbox "#searchTerm" to have the value "111111"
	And I click the button "#searchSubmit"   
    Then the path should match "/school/111111/"
    And the element "[data-testid="school-page-school-name"]" should have the text content "Some Primary School (URN: 111111)"

@Javascript:disabled
Scenario: School search unsuccessful for 2-digit URN
	Given establishment "111111" exists:
		"""
		{
		  "urn": "111111",
		  "name": "Some Primary School"
		}
		"""
    When I navigate to /search/
	And I update the textbox "#searchTerm" to have the value "11"
	And I click the button "#searchSubmit"   
    Then the element "h1" should have the text content "We found no matches for "11""

@Javascript:disabled
Scenario: Partial matching street redirects to school landing page
	Given establishment "111111" exists:
		"""
        {
		     "urn": "111111",
             "name": "Some Primary School"        
        }
        """
    And establishment "222222" exists:
        """
        {
		     "urn": "222222",
             "name": "Another Primary School",
             "address": {
                "street": "111 The Street",
                "town": "SomeTown",
                "postCode": "TR18 3JT"
             }
        } 
		"""
    When I navigate to /search/
	And I update the textbox "#searchTerm" to have the value "111"
	And I click the button "#searchSubmit"  
    Then the path should match "/school/222222/"
    And the element "[data-testid="school-page-school-name"]" should have the text content "Another Primary School (URN: 222222)"

@Javascript:disabled
Scenario: If searchTerm is a 6-digit number, treat it as an exact URN search
	Given establishment "111111" exists:
		"""
		{
		  "urn": "111111",
		  "name": "Some Primary School"
		}
		"""
    And establishment "222222" exists:
        """
        {
		    "urn": "222222",
            "name": "Another Primary School",
            "address": {
                "street": "111111 The Street",
                "town": "SomeTown",
                "postCode": "TR18 3JT"
            }
        } 
		"""
    When I navigate to /search/
	And I update the textbox "#searchTerm" to have the value "111111"
	And I click the button "#searchSubmit"   
    Then the path should match "/school/111111/"
    And the element "[data-testid="school-page-school-name"]" should have the text content "Some Primary School (URN: 111111)"

@Javascript:disabled
Scenario: Search term matching establishment LAESTAB code (with forward slash)
	Given establishment "111111" exists:
		"""
		{
		  "urn": "111111",
		  "name": "Some Primary School",
          "laestab": "894/2200"
		}
		"""
    When I navigate to /search/
	And I update the textbox "#searchTerm" to have the value "894/2200"
	And I click the button "#searchSubmit" 
    Then the path should match "/school/111111/"
    And the element "[data-testid="school-page-school-name"]" should have the text content "Some Primary School (URN: 111111)"

@Javascript:disabled
Scenario: Search term matching establishment LAESTAB code (without forward slash)
	Given establishment "111111" exists:
		"""
		{
		  "urn": "111111",
		  "name": "Some Primary School",
          "laestab": "894/2200"
		}
		"""
    When I navigate to /search/
	And I update the textbox "#searchTerm" to have the value "8942200"
	And I click the button "#searchSubmit"   
    Then the path should match "/school/111111/"
    And the element "[data-testid="school-page-school-name"]" should have the text content "Some Primary School (URN: 111111)"

@Javascript:disabled
Scenario Outline: School results page shows multiple partial LAESTAB matches (LA part)
    Given establishment "111111" exists:
    """
    {
      "urn": "111111",
      "name": "Some Primary School",
      "laestab": "894/2200"
    }
    """
    And establishment "222222" exists:
    """
    {
      "urn": "222222",
      "name": "Some Other Primary School",
      "laestab": "894/1234"
    }
    """
    When I navigate to /search/
	And I update the textbox "#searchTerm" to have the value "894"
	And I click the button "#searchSubmit"
    Then the element "[data-testid="school-search-results-urn-<Counter>"]" should have the text content "<URN>"
    And the element "[data-testid="school-search-results-name-<Counter>"]" should have the text content "<Name>"
    And the element "[data-testid="school-search-results-laestab-<Counter>"]" should have the text content "<LAESTAB>"
Examples: 
| Counter | URN      | LAESTAB     | Name                      |
| 2       | 111111   | 894/2200    | Some Primary School       |
| 1       | 222222   | 894/1234    | Some Other Primary School |

@Javascript:disabled
Scenario Outline: School results page shows multiple partial LAESTAB matches (ESTAB part)
    Given establishment "111111" exists:
    """
    {
      "urn": "111111",
      "name": "Some Primary School",
      "laestab": "894/2200"
    }
    """
    And establishment "222222" exists:
    """
    {
      "urn": "222222",
      "name": "Some Other Primary School",
      "laestab": "600/2200"
    }
    """
    When I navigate to /search/
	And I update the textbox "#searchTerm" to have the value "2200"
	And I click the button "#searchSubmit"
    Then the element "[data-testid="school-search-results-urn-<Counter>"]" should have the text content "<URN>"
    And the element "[data-testid="school-search-results-name-<Counter>"]" should have the text content "<Name>"
    And the element "[data-testid="school-search-results-laestab-<Counter>"]" should have the text content "<LAESTAB>"
Examples: 
| Counter | URN      | LAESTAB     | Name                      |
| 2       | 111111   | 894/2200    | Some Primary School       |
| 1       | 222222   | 600/2200    | Some Other Primary School |

@Javascript:disabled
Scenario: Partial LAESTAB (LA part) match should show no matching results
	Given establishment "111111" exists:
	"""
	{
	  "urn": "111111",
	  "name": "Some Primary School",
      "laestab" : "894/2200"
	}
	"""
    When I navigate to /search/
	And I update the textbox "#searchTerm" to have the value "89"
	And I click the button "#searchSubmit"
    Then the element "h1" should have the text content "We found no matches for "89""

@Javascript:disabled
Scenario: Partial LAESTAB (ESTAB only) match should show no matching results
	Given establishment "111111" exists:
	"""
	{
	  "urn": "111111",
	  "name": "Some Primary School",
      "laestab" : "894/2200"
	}
	"""
    When I navigate to /search/
	And I update the textbox "#searchTerm" to have the value "22"
	And I click the button "#searchSubmit"
    Then the element "h1" should have the text content "We found no matches for "22""

@Javascript:disabled
Scenario: If searchTerm is a 7-digit number, treat it as an exact LAESTAB code search (ignoring other matching fields)
	Given establishment "111111" exists:
		"""
        {
		     "urn": "111111",
             "name": "Some Primary School",
             "laestab": "894/2200"
        }
        """
    And establishment "222222" exists:
        """
        {
		     "urn": "222222",
             "name": "Another Primary School",
             "laestab": "123/4567",
             "address": {
                "street": "8942200 The Street",
                "town": "SomeTown",
                "postCode": "TR18 3JT"
             }
        } 
		"""
    When I navigate to /search/
	And I update the textbox "#searchTerm" to have the value "8942200"
	And I click the button "#searchSubmit"
    Then the path should match "/school/111111/"
    And the element "[data-testid="school-page-school-name"]" should have the text content "Some Primary School (URN: 111111)"

@Javascript:disabled
Scenario: If searchTerm is a 7-digit number with forward slash in the right place, treat it as an exact LAESTAB code search (ignoring other matching fields)
	Given establishment "111111" exists:
		"""
        {
		     "urn": "111111",
             "name": "Some Primary School",
             "laestab": "894/2200",
        } 
        """
    And establishment "222222" exists:
        """
        {
		     "urn": "222222",
             "name": "Another Primary School",
             "laestab": "123/4567",
             "address": {
                "street": "894/2200 The Street",
                "town": "SomeTown",
                "postCode": "TR18 3JT"
             }
        } 
		"""
    When I navigate to /search/
	And I update the textbox "#searchTerm" to have the value "894/2200"
	And I click the button "#searchSubmit"  
    Then the path should match "/school/111111/"
    And the element "[data-testid="school-page-school-name"]" should have the text content "Some Primary School (URN: 111111)"

@Javascript:disabled
Scenario: If searchTerm is a 3-digit number, treat it as an exact LA code search (ignoring other matching fields)
	Given establishment "111111" exists:
		"""
        {
		     "urn": "111111",
             "name": "Some Primary School",
             "laestab": "894/2200"         
        }
        """
     And establishment "222222" exists:
        """
        {
		     "urn": "222222",
             "name": "Another Primary School",
             "laestab": "123/4567",
             "address": {
                "street": "894 The Street",
                "town": "SomeTown",
                "postCode": "TR18 3JT"
             }
        } 
		"""
    When I navigate to /search/
	And I update the textbox "#searchTerm" to have the value "894"
	And I click the button "#searchSubmit"  
    Then the path should match "/school/111111/"
    And the element "[data-testid="school-page-school-name"]" should have the text content "Some Primary School (URN: 111111)"

@Javascript:disabled
Scenario: if searchTerm is a 4-digit number, treat it as an exact ESTAB code search (ignoring other matching fields)
	Given establishment "111111" exists:
		"""
        {
		     "urn": "111111",
             "name": "Some Primary School",
             "laestab": "894/2200"
        }
        """
    And establishment "222222" exists:
        """
        {
		     "urn": "222222",
             "name": "Another Primary School",
             "laestab": "123/4567",
             "address": {
                "street": "2200 The Street",
                "town": "SomeTown",
                "postCode": "TR18 3JT"
             }
        } 
		"""
    When I navigate to /search/
	And I update the textbox "#searchTerm" to have the value "2200"
	And I click the button "#searchSubmit"   
    Then the path should match "/school/111111/"
    And the element "[data-testid="school-page-school-name"]" should have the text content "Some Primary School (URN: 111111)"

@Javascript:disabled
Scenario Outline: Multiple successful school name matches show correct search results
    Given establishment "111111" exists:
    """
    {
      "urn": "111111",
      "name": "School A",
      "address": {
        "street": "13 The Street",
        "postCode": "AB12 3CD"
    }
    }
    """
    And establishment "222222" exists:
    """
    {
      "urn": "222222",
      "name": "School B",
      "address": {
         "street": "2a Mornington Crescent",
         "town": "Liverpool",
         "postCode": "LL1 1AB"
    } 
    }
    """
    And establishment "333333" exists:
    """
    {
      "urn": "333333",
      "name": "School C",
      "address": {
     "street": "34 Long Road",
     "town": "Sheffield"
    }  
    }
    """
    When I navigate to /search/
	And I update the textbox "#searchTerm" to have the value "School"
	And I click the button "#searchSubmit"
    Then the element "[data-testid="school-search-results-urn-<Counter>"]" should have the text content "<URN>"
    And the element "[data-testid="school-search-results-name-<Counter>"]" should have the text content "<Name>"
    And the element "[data-testid="school-search-results-address-<Counter>"]" should have the text content "<Address>"
Examples: 
| Counter | URN    | Name     | Address                                   |
| 1       | 111111 | School A | 13 The Street, AB12 3CD                    |
| 2       | 222222 | School B | 2a Mornington Crescent, Liverpool, LL1 1AB |
| 3       | 333333 | School C | 34 Long Road, Sheffield                   |

@Javascript:disabled
Scenario Outline: Multiple successful school name matches show correct education phase in search results
    Given establishment "111111" exists:
    """
    {
      "urn": "111111",
      "name": "School A",
      "isPrimary": true,
      "isSecondary": false,
      "isPost16": false
    }
    """
    And establishment "222222" exists:
    """
    {
      "urn": "222222",
      "name": "School B",
      "isPrimary": false,
      "isSecondary": true,
      "isPost16": false
    }
    """
    And establishment "333333" exists:
    """
    {
      "urn": "333333",
      "name": "School C",
      "isPrimary": false,
      "isSecondary": false,
      "isPost16": true 
    }
    """
    When I navigate to /search/
	And I update the textbox "#searchTerm" to have the value "School"
	And I click the button "#searchSubmit"
    Then the element "[data-testid="school-search-results-urn-<Counter>"]" should have the text content "<URN>"
    And the element "[data-testid="school-search-results-name-<Counter>"]" should have the text content "<Name>"
    And the element "[data-testid="school-search-results-phase-<Counter>"]" should have the text content "<Education phase>"
Examples: 
| Counter | URN    | Name     | Education phase |
| 1       | 111111 | School A | Primary         |
| 2       | 222222 | School B | Secondary       |
| 3       | 333333 | School C | 16 to 18         |

@Javascript:disabled
Scenario Outline: Multiple successful school name matches show correct ofsted rating in search results
    Given establishment "111111" exists:
    """
    {
      "urn": "111111",
      "name": "School A",
      "ofstedLastInspectionDate": "2013-03-22T00:00:00",
      "ofstedRating": {
        "code": "2",
        "name": "Good",
        "lname": "good",
        "isNullish": false
    }
    }
    """
    And establishment "222222" exists:
    """
    {
      "urn": "222222",
      "name": "School B",
      "ofstedLastInspectionDate": null,
      "ofstedRating": {
         "code": "99"
      }
    }
    """
   And establishment "333333" exists:
    """
    {
      "urn": "333333",
      "name": "School C",
      "ofstedLastInspectionDate": null,
      "ofstedRating": null
    }
    """
    When I navigate to /search/
	And I update the textbox "#searchTerm" to have the value "School"
	And I click the button "#searchSubmit"
    Then the element "[data-testid="school-search-results-urn-<Counter>"]" should have the text content "<URN>"
    And the element "[data-testid="school-search-results-name-<Counter>"]" should have the text content "<Name>"
    And the element "[data-testid="school-search-results-ofstedrating-<Counter>"]" should have the text content "<Ofsted rating>"
Examples: 
| Counter | URN    | Name     | Ofsted rating                                   |
| 1       | 111111 | School A | 2 Good \| Ofsted report Inspected 22 March 2013 |
| 2       | 222222 | School B | No data available                               |
| 3       | 333333 | School C | -- No Ofsted assessment published               |