Feature: Search page

Background:
	Given I am a DfE Named user

@Javascript:disabled
Scenario: Page title should show correct text when search returns results
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
	When I navigate to /search/
	And I update the textbox "#searchTerm" to have the value "Primary"
	And I click the button "#searchSubmit"
	Then the path should be /search/?search=Primary
	And the page title should be "Search results for "Primary" | Analyse school performance"

@Javascript:enabled
Scenario: Page title should show correct text when search returns results (JS)
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
	When I navigate to /search/
	And I update the textbox "#searchTerm" to have the value "Primary"
	And I click the button "#searchSubmit"
	Then the path should be /search/?search=Primary
	And the page title should be "Search results for "Primary" | Analyse school performance"

@Javascript:disabled
Scenario: Page title should show correct text when search returns no results
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
	When I navigate to /search/
	And I update the textbox "#searchTerm" to have the value "Secondary"
	And I click the button "#searchSubmit"
	Then the path should be /search/?search=Secondary
	And the page title should be "We found no matches for "Secondary" | Analyse school performance"

@Javascript:enabled
Scenario: Page title should show correct text when search returns no results (JS)
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
	When I navigate to /search/
	And I update the textbox "#searchTerm" to have the value "Secondary"
	And I click the button "#searchSubmit"
	Then the path should be /search/?search=Secondary
	And the page title should be "We found no matches for "Secondary" | Analyse school performance"

@Javascript:disabled
Scenario: Page should show a breadcrumb trail when search returns results
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
	When I navigate to /search/
	And I update the textbox "#searchTerm" to have the value "Primary"
	And I click the button "#searchSubmit"
	Then the path should be /search/?search=Primary
	And the breadcrumb trail should be:
		| text                         | href     | current |
		| Home                         | /        |         |
		| Search                       | /search/ |         |
		| Search results for "Primary" |          | true    |

@Javascript:enabled
Scenario: Page should show a breadcrumb trail when search returns results (JS)
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
	When I navigate to /search/
	And I update the textbox "#searchTerm" to have the value "Primary"
	And I click the button "#searchSubmit"
	Then the path should be /search/?search=Primary
	And the breadcrumb trail should be:
		| text                         | href     | current |
		| Home                         | /        |         |
		| Search                       | /search/ |         |
		| Search results for "Primary" |          | true    |

@Javascript:disabled
Scenario: Page should show a breadcrumb trail when search returns no results
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
	When I navigate to /search/
	And I update the textbox "#searchTerm" to have the value "Secondary"
	And I click the button "#searchSubmit"
	Then the path should be /search/?search=Secondary
	And the breadcrumb trail should be:
		| text                                | href     | current |
		| Home                                | /        |         |
		| Search                              | /search/ |         |
		| We found no matches for "Secondary" |          | true    |

@Javascript:enabled
Scenario: Page should show a breadcrumb trail when search returns no results (JS)
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
	When I navigate to /search/
	And I update the textbox "#searchTerm" to have the value "Secondary"
	And I click the button "#searchSubmit"
	Then the path should be /search/?search=Secondary
	And the breadcrumb trail should be:
		| text                                | href     | current |
		| Home                                | /        |         |
		| Search                              | /search/ |         |
		| We found no matches for "Secondary" |          | true    |
 
@Javascript:disabled
Scenario: Search Term Validation
	When I navigate to /search/
	Then I should get a 200 response
	And the page title should be "Search | Analyse school performance"
	And the element "h1.govuk-heading-l" should have the text content "Search for a school"
	And the element "#searchForm" should have the text content "Enter school name, address, URN (Unique Reference Number) or LAESTAB (Local Authority Establishment Number) Search"
 
@Javascript:enabled
Scenario: Search Term Validation (JS)
	When I navigate to /search/
	Then I should get a 200 response
	And the page title should be "Search | Analyse school performance"
	And the element "h1.govuk-heading-l" should have the text content "Search for a school"
	And the element "#searchForm" should have the text content "Enter school name, address, URN (Unique Reference Number) or LAESTAB (Local Authority Establishment Number) When autocomplete results are available use up and down arrows to review and enter to select. Touch device users, explore by touch or with swipe gestures. Search"

@Javascript:disabled
Scenario: Errors in Search Term Validation
	When I navigate to /search/
	And I click the button "#searchSubmit"
	Then the path should be /search/?search=
	And the element "#searchTerm-input-error" should have the text content "Please enter a school name, address, URN (Unique Reference Number) or LAESTAB (Local Authority Establishment Number)"
	And the element "h2.govuk-error-summary__title" should have the text content "There is a problem"
	And the element "*[data-testid='searchTerm']" should have the text content "Please enter a school name, address, URN (Unique Reference Number) or LAESTAB (Local Authority Establishment Number)"

@Javascript:enabled
Scenario: Errors in Search Term Validation (JS)
	When I navigate to /search/
	And I click the button "#searchSubmit"
	Then the path should be /search/?search=
	And the element "#searchTerm-input-error" should have the text content "Please enter a school name, address, URN (Unique Reference Number) or LAESTAB (Local Authority Establishment Number)"
	And the element "h2.govuk-error-summary__title" should have the text content "There is a problem"
	And the element "*[data-testid='searchTerm']" should have the text content "Please enter a school name, address, URN (Unique Reference Number) or LAESTAB (Local Authority Establishment Number)"

@Javascript:disabled
Scenario: School search page should show correct message when there is no data
	Given no Establishments exist
	When I navigate to /search/
	And I update the textbox "#searchTerm" to have the value "primary"
	And I click the button "#searchSubmit"
	Then the path should be /search/?search=primary
	And the element "h1" should have the text content "We found no matches for "primary""

@Javascript:enabled
Scenario: School search page should show correct message when there is no data (JS)
	Given no Establishments exist
	When I navigate to /search/
	And I update the textbox "#searchTerm" to have the value "primary"
	And I click the button "#searchSubmit"
	Then the path should be /search/?search=primary
	And the element "h1" should have the text content "We found no matches for "primary""

@Javascript:disabled
Scenario: School search page should show correct message for search term with no matches
	Given Establishment "111111" exists:
	"""
	{
		"name": "Some Primary School"
	}
	"""
	When I navigate to /search/
	And I update the textbox "#searchTerm" to have the value "secondary"
	And I click the button "#searchSubmit"
	Then the path should be /search/?search=secondary
	And the element "h1" should have the text content "We found no matches for "secondary""

@Javascript:enabled
Scenario: School search page should show correct message for search term with no matches (JS)
	Given Establishment "111111" exists:
	"""
	{
		"name": "Some Primary School"
	}
	"""
	When I navigate to /search/
	And I update the textbox "#searchTerm" to have the value "secondary"
	And I click the button "#searchSubmit"
	Then the path should be /search/?search=secondary
	And the element "h1" should have the text content "We found no matches for "secondary""

@Javascript:disabled
Scenario: Matching URN search should redirect to school landing page
	Given Establishment "111111" exists:
	"""
	{
		"name": "Some Primary School",
	    "localAuthority": {
			   "code": "999",
	       	   "name": "Test LA"
		}
	}
	"""
	When I navigate to /search/
	And I update the textbox "#searchTerm" to have the value "111111"
	And I click the button "#searchSubmit"   
	Then the path should be /school/111111/
	And the element "#app-page-subtitle span" should have the text content "(URN: 111111)"

@Javascript:enabled
Scenario: Matching URN search should redirect to school landing page (JS)
	Given Establishment "111111" exists:
	"""
	{
		"name": "Some Primary School",
	    "localAuthority": {
			 "code": "999",
	       	 "name": "Test LA"
		}
	}
	"""
	When I navigate to /search/
	And I update the textbox "#searchTerm" to have the value "111111"
	And I click the button "#searchSubmit"   
	Then the path should be /school/111111/
	And the element "#app-page-subtitle span" should have the text content "(URN: 111111)"

@Javascript:disabled
Scenario: Partial match for school name should redirect to school landing page
	Given Establishment "111111" exists:
	"""
	{
		"name": "Some Primary School",
	    "localAuthority": {
			"code": "999",
	       	"name": "Test LA"
		}
	}
	"""
	When I navigate to /search/
	And I update the textbox "#searchTerm" to have the value "PRiMaRY"
	And I click the button "#searchSubmit"  
	Then the path should be /school/111111/
	And the element "#app-page-subtitle span" should have the text content "(URN: 111111)"

@Javascript:enabled
Scenario: Partial match for school name should redirect to school landing page (JS)
	Given Establishment "111111" exists:
	"""
	{
		"name": "Some Primary School",
	    "localAuthority": {
			"code": "999",
	       	"name": "Test LA"
		}
	}
	"""
	When I navigate to /search/
	And I update the textbox "#searchTerm" to have the value "PRiMaRY"
	And I click the button "#searchSubmit"  
	Then the path should be /school/111111/
	And the element "#app-page-subtitle span" should have the text content "(URN: 111111)"

@Javascript:disabled
Scenario: Partial street match should redirect to school landing page
	Given Establishment "111111" exists:
	"""
	{
		"name": "Some Primary School",
		"address": {
			"street": "13 The Street",
			"town": "SomeTown",
			"postCode": "TR18 3JT"
		},
	    "localAuthority": {
			"code": "999",
	       	"name": "Test LA"
		}
	} 
	"""
	When I navigate to /search/
	And I update the textbox "#searchTerm" to have the value "str"
	And I click the button "#searchSubmit"  
	Then the path should be /school/111111/
	And the element "[data-testid="school-details-address-value"]" should have the text content "13 The Street, SomeTown TR18 3JT"

@Javascript:enabled
Scenario: Partial street match should redirect to school landing page (JS)
	Given Establishment "111111" exists:
	"""
	{
		"name": "Some Primary School",
		"address": {
			"street": "13 The Street",
			"town": "SomeTown",
			"postCode": "TR18 3JT"
		},
	    "localAuthority": {
			"code": "999",
	       	"name": "Test LA"
		}
	} 
	"""
	When I navigate to /search/
	And I update the textbox "#searchTerm" to have the value "str"
	And I click the button "#searchSubmit"  
	Then the path should be /school/111111/
	And the element "[data-testid="school-details-address-value"]" should have the text content "13 The Street, SomeTown TR18 3JT"

@Javascript:disabled
Scenario: Partial town match should redirect to school landing page
	Given Establishment "111111" exists:
	"""
	{
		"name": "Some Primary School",
		"address": {
			"street": "13 The Street",
			"town": "SomeTown",
			"postCode": "TR18 3JT"
		},
	    "localAuthority": {
			"code": "999",
	       	"name": "Test LA"
		}
	} 
	"""
	When I navigate to /search/
	And I update the textbox "#searchTerm" to have the value "some"
	And I click the button "#searchSubmit"  
	Then the path should be /school/111111/
	And the element "[data-testid="school-details-address-value"]" should have the text content "13 The Street, SomeTown TR18 3JT"

@Javascript:enabled
Scenario: Partial town match should redirect to school landing page (JS)
	Given Establishment "111111" exists:
	"""
	{
		"name": "Some Primary School",
		"address": {
			"street": "13 The Street",
			"town": "SomeTown",
			"postCode": "TR18 3JT"
		},
	    "localAuthority": {
			"code": "999",
	       	"name": "Test LA"
		}
	} 
	"""
	When I navigate to /search/
	And I update the textbox "#searchTerm" to have the value "some"
	And I click the button "#searchSubmit"  
	Then the path should be /school/111111/
	And the element "[data-testid="school-details-address-value"]" should have the text content "13 The Street, SomeTown TR18 3JT"

@Javascript:disabled
Scenario: Partial postcode match should redirect to school landing page
	Given Establishment "111111" exists:
	"""
	{
		"name": "Some Primary School",
		"address": {
			"street": "13 The Street",
			"town": "SomeTown",
			"postCode": "TR18 3JT"
		},
	    "localAuthority": {
			"code": "999",
	       	"name": "Test LA"
		}
	} 
	"""
	When I navigate to /search/
	And I update the textbox "#searchTerm" to have the value "tr1"
	And I click the button "#searchSubmit"   
	Then the path should be /school/111111/
	And the element "[data-testid="school-details-address-value"]" should have the text content "13 The Street, SomeTown TR18 3JT"

@Javascript:enabled
Scenario: Partial postcode match should redirect to school landing page (JS)
	Given Establishment "111111" exists:
	"""
	{
		"name": "Some Primary School",
		"address": {
			"street": "13 The Street",
			"town": "SomeTown",
			"postCode": "TR18 3JT"
		},
	    "localAuthority": {
			"code": "999",
	       	"name": "Test LA"
		}
	} 
	"""
	When I navigate to /search/
	And I update the textbox "#searchTerm" to have the value "tr1"
	And I click the button "#searchSubmit"   
	Then the path should be /school/111111/
	And the element "[data-testid="school-details-address-value"]" should have the text content "13 The Street, SomeTown TR18 3JT"

@Javascript:disabled
Scenario Outline: Results page should show partial name and address matches
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
	When I navigate to /search/
	And I update the textbox "#searchTerm" to have the value "tr"
	And I click the button "#searchSubmit"
	Then the path should be /search/?search=tr
	And the element "[data-testid="establishment-listing-urn-<Counter>"]" should have the text content "<URN>"
	And the element "[data-testid="establishment-listing-name-<Counter>"]" should have the text content "<Name>"
	And the element "[data-testid="establishment-listing-address-<Counter>"]" should have the text content "<Address>"
Examples: 
| Counter | URN    | Name                       | Address                        |
| 1       | 333333 | A Different Primary School | 13 The Road, SomeTown TR18 3JT |
| 2       | 222222 | Some Other Primary School  | 13 The Road, Tring B1 1AA      |
| 3       | 111111 | Some Primary School        | 13 The Street, SomeTown B1 1AA |
| 4       | 444444 | The Training Centre        | No address available           |

@Javascript:enabled
Scenario Outline: Results page should show partial name and address matches (JS)
	Given Establishment "111111" exists:
	"""
	{
		"name": "Some Primary School",
		"address": {
			"street": "13 The Street",
			"town": "SomeTown",
			"postCode": "B1 1AA"
		},
	    "localAuthority": {
			"code": "999",
	       	"name": "Test LA"
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
		},
	    "localAuthority": {
			"code": "999",
	       	"name": "Test LA"
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
		},
	    "localAuthority": {
			 "code": "999",
	       	 "name": "Test LA"
		}
	}
	"""
	And Establishment "444444" exists:
	"""
	{
		"name": "The Training Centre",
	     "localAuthority": {
			 "code": "999",
	       	 "name": "Test LA"
		}
	}
	"""
	When I navigate to /search/
	And I update the textbox "#searchTerm" to have the value "tr"
	And I click the button "#searchSubmit"
	Then the path should be /search/?search=tr
	And the element "[data-testid="establishment-listing-urn-<Counter>"]" should have the text content "<URN>"
	And the element "[data-testid="establishment-listing-name-<Counter>"]" should have the text content "<Name>"
	And the element "[data-testid="establishment-listing-address-<Counter>"]" should have the text content "<Address>"
Examples: 
| Counter | URN    | Name                       | Address                        |
| 1       | 333333 | A Different Primary School | 13 The Road, SomeTown TR18 3JT |
| 2       | 222222 | Some Other Primary School  | 13 The Road, Tring B1 1AA      |
| 3       | 111111 | Some Primary School        | 13 The Street, SomeTown B1 1AA |
| 4       | 444444 | The Training Centre        | No address available           |

@Javascript:disabled
Scenario: School search successful for 6-digit URN
	Given Establishment "111111" exists:
	"""
	{
		"name": "Some Primary School",
	    "localAuthority": {
			"code": "999",
	       	"name": "Test LA"
		}
	}
	"""
	When I navigate to /search/
	And I update the textbox "#searchTerm" to have the value "111111"
	And I click the button "#searchSubmit"   
	Then the path should be /school/111111/
	And the element "#app-page-subtitle span" should have the text content "(URN: 111111)"

@Javascript:enabled
Scenario: School search successful for 6-digit URN (JS)
	Given Establishment "111111" exists:
	"""
	{
		"name": "Some Primary School",
	    "localAuthority": {
			"code": "999",
	       	"name": "Test LA"
		}
	}
	"""
	When I navigate to /search/
	And I update the textbox "#searchTerm" to have the value "111111"
	And I click the button "#searchSubmit"   
	Then the path should be /school/111111/
	And the element "#app-page-subtitle span" should have the text content "(URN: 111111)"

@Javascript:disabled
Scenario Outline: School search with less than 6 digits does not match on URN
	Given Establishment "111111" exists:
	"""
	{
		"name": "Some Primary School"
	}
	"""
	When I navigate to /search/
	And I update the textbox "#searchTerm" to have the value "<SearchTerm>"
	And I click the button "#searchSubmit"   
	Then the path should be /search/?search=<SearchTerm>
	And the element "h1" should have the text content "We found no matches for "<SearchTerm>""
Examples: 
| SearchTerm |
| 1          |
| 11         |
| 111        |
| 1111       |
| 11111      |

@Javascript:enabled
Scenario Outline: School search with less than 6 digits does not match on URN (JS)
	Given Establishment "111111" exists:
	"""
	{
		"name": "Some Primary School"
	}
	"""
	When I navigate to /search/
	And I update the textbox "#searchTerm" to have the value "<SearchTerm>"
	And I click the button "#searchSubmit"   
	Then the path should be /search/?search=<SearchTerm>
	And the element "h1" should have the text content "We found no matches for "<SearchTerm>""
Examples: 
| SearchTerm |
| 1          |
| 11         |
| 111        |
| 1111       |
| 11111      |

@Javascript:disabled
Scenario Outline: School search with less than 6 digits matches on school address
	Given Establishment "111111" exists:
	"""
	{
		"name": "Some Primary School"
	}
	"""
	And Establishment "222222" exists:
	"""
	{
		"name": "Another Primary School",
		"address": {
			"street": "<SearchTerm> The Street",
			"town": "SomeTown",
			"postCode": "TR18 3JT"
		},
	    "localAuthority": {
			"code": "999",
	       	"name": "Test LA"
		}
	} 
	"""
	When I navigate to /search/
	And I update the textbox "#searchTerm" to have the value "<SearchTerm>"
	And I click the button "#searchSubmit"  
	Then the path should be /school/222222/
	And the element "#app-page-subtitle span" should have the text content "(URN: 222222)"
Examples: 
| SearchTerm |
| 1          |
| 11         |
| 111        |
| 1111       |
| 11111      |

@Javascript:enabled
Scenario Outline: School search with less than 6 digits matches on school address (JS)
	Given Establishment "111111" exists:
	"""
	{
		"name": "Some Primary School"
	}
	"""
	And Establishment "222222" exists:
	"""
	{
		"name": "Another Primary School",
		"address": {
			"street": "<SearchTerm> The Street",
			"town": "SomeTown",
			"postCode": "TR18 3JT"
		},
	    "localAuthority": {
			"code": "999",
	       	"name": "Test LA"
		}
	} 
	"""
	When I navigate to /search/
	And I update the textbox "#searchTerm" to have the value "<SearchTerm>"
	And I click the button "#searchSubmit"  
	Then the path should be /school/222222/
	And the element "#app-page-subtitle span" should have the text content "(URN: 222222)"
Examples: 
| SearchTerm |
| 1          |
| 11         |
| 111        |
| 1111       |
| 11111      |

@Javascript:disabled
Scenario: If searchTerm is a 6-digit number, treat it as an exact URN search
	Given Establishment "111111" exists:
	"""
	{
		"name": "Some Primary School",
	    "localAuthority": {
			"code": "999",
	       	"name": "Test LA"
		}
	}
	"""
	And Establishment "222222" exists:
	"""
	{
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
	Then the path should be /school/111111/
	And the element "#app-page-subtitle span" should have the text content "(URN: 111111)"

@Javascript:enabled
Scenario: If searchTerm is a 6-digit number, treat it as an exact URN search (JS)
	Given Establishment "111111" exists:
	"""
	{
		"name": "Some Primary School",
	    "localAuthority": {
			"code": "999",
	       	"name": "Test LA"
		}
	}
	"""
	And Establishment "222222" exists:
	"""
	{
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
	Then the path should be /school/111111/
	And the element "#app-page-subtitle span" should have the text content "(URN: 111111)"

@Javascript:disabled
Scenario: Search term matching establishment LAESTAB code (with forward slash)
	Given Establishment "111111" exists:
	"""
	{
		"name": "Some Primary School",
		"laestab": "894/2200",
	    "localAuthority": {
			"code": "999",
	       	"name": "Test LA"
		}
	}
	"""
	When I navigate to /search/
	And I update the textbox "#searchTerm" to have the value "894/2200"
	And I click the button "#searchSubmit" 
	Then the path should be /school/111111/
	And the element "#app-page-subtitle span" should have the text content "(URN: 111111)"

@Javascript:enabled
Scenario: Search term matching establishment LAESTAB code (with forward slash) (JS)
	Given Establishment "111111" exists:
	"""
	{
		"name": "Some Primary School",
		"laestab": "894/2200",
	    "localAuthority": {
			"code": "999",
	       	"name": "Test LA"
		}
	}
	"""
	When I navigate to /search/
	And I update the textbox "#searchTerm" to have the value "894/2200"
	And I click the button "#searchSubmit" 
	Then the path should be /school/111111/
	And the element "#app-page-subtitle span" should have the text content "(URN: 111111)"

@Javascript:disabled
Scenario: Search term matching establishment LAESTAB code (without forward slash)
	Given Establishment "111111" exists:
	"""
	{
		"name": "Some Primary School",
		"laestab": "894/2200",
	    "localAuthority": {
			"code": "999",
	       	"name": "Test LA"
		}
	}
	"""
	When I navigate to /search/
	And I update the textbox "#searchTerm" to have the value "8942200"
	And I click the button "#searchSubmit"   
	Then the path should be /school/111111/
	And the element "#app-page-subtitle span" should have the text content "(URN: 111111)"

@Javascript:enabled
Scenario: Search term matching establishment LAESTAB code (without forward slash) (JS)
	Given Establishment "111111" exists:
	"""
	{
		"name": "Some Primary School",
		"laestab": "894/2200",
	    "localAuthority": {
			"code": "999",
	       	"name": "Test LA"
		}
	}
	"""
	When I navigate to /search/
	And I update the textbox "#searchTerm" to have the value "8942200"
	And I click the button "#searchSubmit"   
	Then the path should be /school/111111/
	And the element "#app-page-subtitle span" should have the text content "(URN: 111111)"

@Javascript:disabled
Scenario Outline: School results page shows multiple partial LAESTAB matches (LA part)
	Given Establishment "111111" exists:
	"""
	{
		"name": "Some Primary School",
		"laestab": "894/2200"
	}
	"""
	And Establishment "222222" exists:
	"""
	{
		"name": "Some Other Primary School",
		"laestab": "894/1234"
	}
	"""
	When I navigate to /search/
	And I update the textbox "#searchTerm" to have the value "894"
	And I click the button "#searchSubmit"
	Then the path should be /search/?search=894
	And the element "[data-testid="establishment-listing-urn-<Counter>"]" should have the text content "<URN>"
	And the element "[data-testid="establishment-listing-name-<Counter>"]" should have the text content "<Name>"
	And the element "[data-testid="establishment-listing-laestab-<Counter>"]" should have the text content "<LAESTAB>"
Examples: 
| Counter | URN    | LAESTAB  | Name                      |
| 2       | 111111 | 894/2200 | Some Primary School       |
| 1       | 222222 | 894/1234 | Some Other Primary School |

@Javascript:enabled
Scenario Outline: School results page shows multiple partial LAESTAB matches (LA part) (JS)
	Given Establishment "111111" exists:
	"""
	{
		"name": "Some Primary School",
		"laestab": "894/2200"
	}
	"""
	And Establishment "222222" exists:
	"""
	{
		"name": "Some Other Primary School",
		"laestab": "894/1234"
	}
	"""
	When I navigate to /search/
	And I update the textbox "#searchTerm" to have the value "894"
	And I click the button "#searchSubmit"
	Then the path should be /search/?search=894
	And the element "[data-testid="establishment-listing-urn-<Counter>"]" should have the text content "<URN>"
	And the element "[data-testid="establishment-listing-name-<Counter>"]" should have the text content "<Name>"
	And the element "[data-testid="establishment-listing-laestab-<Counter>"]" should have the text content "<LAESTAB>"
Examples: 
| Counter | URN    | LAESTAB  | Name                      |
| 2       | 111111 | 894/2200 | Some Primary School       |
| 1       | 222222 | 894/1234 | Some Other Primary School |

@Javascript:disabled
Scenario Outline: School results page shows multiple partial LAESTAB matches (ESTAB part)
	Given Establishment "111111" exists:
	"""
	{
		"name": "Some Primary School",
		"laestab": "894/2200"
	}
	"""
	And Establishment "222222" exists:
	"""
	{
		"name": "Some Other Primary School",
		"laestab": "600/2200"
	}
	"""
	When I navigate to /search/
	And I update the textbox "#searchTerm" to have the value "2200"
	And I click the button "#searchSubmit"
	Then the path should be /search/?search=2200
	And the element "[data-testid="establishment-listing-urn-<Counter>"]" should have the text content "<URN>"
	And the element "[data-testid="establishment-listing-name-<Counter>"]" should have the text content "<Name>"
	And the element "[data-testid="establishment-listing-laestab-<Counter>"]" should have the text content "<LAESTAB>"
Examples: 
| Counter | URN    | LAESTAB  | Name                      |
| 2       | 111111 | 894/2200 | Some Primary School       |
| 1       | 222222 | 600/2200 | Some Other Primary School |

@Javascript:enabled
Scenario Outline: School results page shows multiple partial LAESTAB matches (ESTAB part) (JS)
	Given Establishment "111111" exists:
	"""
	{
		"name": "Some Primary School",
		"laestab": "894/2200"
	}
	"""
	And Establishment "222222" exists:
	"""
	{
		"name": "Some Other Primary School",
		"laestab": "600/2200"
	}
	"""
	When I navigate to /search/
	And I update the textbox "#searchTerm" to have the value "2200"
	And I click the button "#searchSubmit"
	Then the path should be /search/?search=2200
	And the element "[data-testid="establishment-listing-urn-<Counter>"]" should have the text content "<URN>"
	And the element "[data-testid="establishment-listing-name-<Counter>"]" should have the text content "<Name>"
	And the element "[data-testid="establishment-listing-laestab-<Counter>"]" should have the text content "<LAESTAB>"
Examples: 
| Counter | URN    | LAESTAB  | Name                      |
| 2       | 111111 | 894/2200 | Some Primary School       |
| 1       | 222222 | 600/2200 | Some Other Primary School |

@Javascript:disabled
Scenario: Partial LAESTAB (LA part) match should show no matching results
	Given Establishment "111111" exists:
	"""
	{
		"name": "Some Primary School",
		"laestab" : "894/2200"
	}
	"""
	When I navigate to /search/
	And I update the textbox "#searchTerm" to have the value "89"
	And I click the button "#searchSubmit"
	Then the path should be /search/?search=89
	And the element "h1" should have the text content "We found no matches for "89""

@Javascript:enabled
Scenario: Partial LAESTAB (LA part) match should show no matching results (JS)
	Given Establishment "111111" exists:
	"""
	{
		"name": "Some Primary School",
		"laestab" : "894/2200"
	}
	"""
	When I navigate to /search/
	And I update the textbox "#searchTerm" to have the value "89"
	And I click the button "#searchSubmit"
	Then the path should be /search/?search=89
	And the element "h1" should have the text content "We found no matches for "89""

@Javascript:disabled
Scenario: Partial LAESTAB (ESTAB only) match should show no matching results
	Given Establishment "111111" exists:
	"""
	{
		"name": "Some Primary School",
		"laestab" : "894/2200"
	}
	"""
	When I navigate to /search/
	And I update the textbox "#searchTerm" to have the value "22"
	And I click the button "#searchSubmit"
	Then the path should be /search/?search=22
	And the element "h1" should have the text content "We found no matches for "22""

@Javascript:enabled
Scenario: Partial LAESTAB (ESTAB only) match should show no matching results (JS)
	Given Establishment "111111" exists:
	"""
	{
		"name": "Some Primary School",
		"laestab" : "894/2200"
	}
	"""
	When I navigate to /search/
	And I update the textbox "#searchTerm" to have the value "22"
	And I click the button "#searchSubmit"
	Then the path should be /search/?search=22
	And the element "h1" should have the text content "We found no matches for "22""

@Javascript:disabled
Scenario: If searchTerm is a 7-digit number, treat it as an exact LAESTAB code search (ignoring other matching fields)
	Given Establishment "111111" exists:
	"""
	{
		"name": "Some Primary School",
		"laestab": "894/2200",
	    "localAuthority": {
			"code": "999",
	       	"name": "Test LA"
		}
	}
	"""
	And Establishment "222222" exists:
	"""
	{
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
	Then the path should be /school/111111/
	And the element "#app-page-subtitle span" should have the text content "(URN: 111111)"

@Javascript:enabled
Scenario: If searchTerm is a 7-digit number, treat it as an exact LAESTAB code search (ignoring other matching fields) (JS)
	Given Establishment "111111" exists:
	"""
	{
		"name": "Some Primary School",
		"laestab": "894/2200",
	    "localAuthority": {
			"code": "999",
	       	"name": "Test LA"
		}
	}
	"""
	And Establishment "222222" exists:
	"""
	{
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
	Then the path should be /school/111111/
	And the element "#app-page-subtitle span" should have the text content "(URN: 111111)"

@Javascript:disabled
Scenario: If searchTerm is a 7-digit number with forward slash in the right place, treat it as an exact LAESTAB code search (ignoring other matching fields)
	Given Establishment "111111" exists:
	"""
	{
		"name": "Some Primary School",
		"laestab": "894/2200",
	    "localAuthority": {
			"code": "999",
	       	"name": "Test LA"
		}
	} 
	"""
	And Establishment "222222" exists:
	"""
	{
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
	Then the path should be /school/111111/
	And the element "#app-page-subtitle span" should have the text content "(URN: 111111)"

@Javascript:enabled
Scenario: If searchTerm is a 7-digit number with forward slash in the right place, treat it as an exact LAESTAB code search (ignoring other matching fields) (JS)
	Given Establishment "111111" exists:
	"""
	{
		"name": "Some Primary School",
		"laestab": "894/2200",
	    "localAuthority": {
			"code": "999",
	       	"name": "Test LA"
		}
	} 
	"""
	And Establishment "222222" exists:
	"""
	{
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
	Then the path should be /school/111111/
	And the element "#app-page-subtitle span" should have the text content "(URN: 111111)"

@Javascript:disabled
Scenario: If searchTerm is a 3-digit number, treat it as an exact LA code search (ignoring other matching fields)
	Given Establishment "111111" exists:
	"""
	{
		"name": "Some Primary School",
		"laestab": "894/2200",
	    "localAuthority": {
			"code": "999",
	       	"name": "Test LA"
		}		 
	}
	"""
	 And Establishment "222222" exists:
	"""
	{
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
	Then the path should be /school/111111/
	And the element "#app-page-subtitle span" should have the text content "(URN: 111111)"

@Javascript:enabled
Scenario: If searchTerm is a 3-digit number, treat it as an exact LA code search (ignoring other matching fields) (JS)
	Given Establishment "111111" exists:
	"""
	{
		"name": "Some Primary School",
		"laestab": "894/2200",
		"localAuthority": {
			"code": "999",
	       	"name": "Test LA"
		}		 
	}
	"""
	 And Establishment "222222" exists:
	"""
	{
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
	Then the path should be /school/111111/
	And the element "#app-page-subtitle span" should have the text content "(URN: 111111)"

@Javascript:disabled
Scenario: if searchTerm is a 4-digit number, treat it as an exact ESTAB code search (ignoring other matching fields)
	Given Establishment "111111" exists:
	"""
	{
		"name": "Some Primary School",
		"laestab": "894/2200",
	    "localAuthority": {
			"code": "999",
	       	"name": "Test LA"
		}
	}
	"""
	And Establishment "222222" exists:
	"""
	{
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
	Then the path should be /school/111111/
	And the element "#app-page-subtitle span" should have the text content "(URN: 111111)"

@Javascript:enabled
Scenario: if searchTerm is a 4-digit number, treat it as an exact ESTAB code search (ignoring other matching fields) (JS)
	Given Establishment "111111" exists:
	"""
	{
		"name": "Some Primary School",
		"laestab": "894/2200",
	    "localAuthority": {
			"code": "999",
	       	"name": "Test LA"
		}
	}
	"""
	And Establishment "222222" exists:
	"""
	{
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
	Then the path should be /school/111111/
	And the element "#app-page-subtitle span" should have the text content "(URN: 111111)"

@Javascript:disabled
Scenario Outline: Multiple successful school name matches show correct search results
	Given Establishment "111111" exists:
	"""
	{
		"name": "School A",
		"address": {
			"street": "13 The Street",
			"postCode": "AB12 3CD"
		}
	}
	"""
	And Establishment "222222" exists:
	"""
	{
		"name": "School B",
		"address": {
			"street": "2a Mornington Crescent",
			"town": "Liverpool",
			"postCode": "LL1 1AB"
		} 
	}
	"""
	And Establishment "333333" exists:
	"""
	{
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
	Then the path should be /search/?search=School
	And the element "[data-testid="establishment-listing-urn-<Counter>"]" should have the text content "<URN>"
	And the element "[data-testid="establishment-listing-name-<Counter>"]" should have the text content "<Name>"
	And the element "[data-testid="establishment-listing-address-<Counter>"]" should have the text content "<Address>"
Examples: 
| Counter | URN    | Name     | Address                                   |
| 1       | 111111 | School A | 13 The Street AB12 3CD                    |
| 2       | 222222 | School B | 2a Mornington Crescent, Liverpool LL1 1AB |
| 3       | 333333 | School C | 34 Long Road, Sheffield                   |

@Javascript:enabled
Scenario Outline: Multiple successful school name matches show correct search results (JS)
	Given Establishment "111111" exists:
	"""
	{
		"name": "School A",
		"address": {
			"street": "13 The Street",
			"postCode": "AB12 3CD"
		}
	}
	"""
	And Establishment "222222" exists:
	"""
	{
		"name": "School B",
		"address": {
			"street": "2a Mornington Crescent",
			"town": "Liverpool",
			"postCode": "LL1 1AB"
		} 
	}
	"""
	And Establishment "333333" exists:
	"""
	{
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
	Then the path should be /search/?search=School
	And the element "[data-testid="establishment-listing-urn-<Counter>"]" should have the text content "<URN>"
	And the element "[data-testid="establishment-listing-name-<Counter>"]" should have the text content "<Name>"
	And the element "[data-testid="establishment-listing-address-<Counter>"]" should have the text content "<Address>"
Examples: 
| Counter | URN    | Name     | Address                                   |
| 1       | 111111 | School A | 13 The Street AB12 3CD                    |
| 2       | 222222 | School B | 2a Mornington Crescent, Liverpool LL1 1AB |
| 3       | 333333 | School C | 34 Long Road, Sheffield                   |

@Javascript:disabled
Scenario: Pagination in Search Validation
	Given 251 Establishments exist with properties:
	| urn		  | name						|
	| (100000 + n) | Primary School (100000 + n) |
	When I navigate to /search/?search=primary
	Then the element "*[data-testid='NumberOfPages-Footer']" should have the text content "Showing 1 - 50 of 251 schools or colleges"
	And the element "*[data-testid='PageLinks-Footer-1']" should have the href "/search/?search=primary&page=1"
	And the element "*[data-testid='PageLinks-Footer-2']" should have the href "/search/?search=primary&page=2"
	And the element "*[data-testid='govuk-pagination__link--Footer']" should have the text content "..."
	And the element "*[data-testid='PageLinks-Footer-6']" should have the href "/search/?search=primary&page=6"
	And the element "*[data-testid='PageLinks-Footer-Next']" should have the href "/search/?search=primary&page=2"
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

@Javascript:enabled
Scenario: Pagination in Search Validation (JS)
	Given 251 Establishments exist with properties:
	| urn		  | name						|
	| (100000 + n) | Primary School (100000 + n) |
	When I navigate to /search/?search=primary
	Then the element "*[data-testid='NumberOfPages-Footer']" should have the text content "Showing 1 - 50 of 251 schools or colleges"
	And the element "*[data-testid='PageLinks-Footer-1']" should have the href "/search/?search=primary&page=1"
	And the element "*[data-testid='PageLinks-Footer-2']" should have the href "/search/?search=primary&page=2"
	And the element "*[data-testid='govuk-pagination__link--Footer']" should have the text content "..."
	And the element "*[data-testid='PageLinks-Footer-6']" should have the href "/search/?search=primary&page=6"
	And the element "*[data-testid='PageLinks-Footer-Next']" should have the href "/search/?search=primary&page=2"
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
Scenario: Pagination in Search Validation 2
	Given 251 Establishments exist with properties:
	| urn		  | name						|
	| (100000 + n) | Primary School (100000 + n) |
	When I navigate to /search/?page=3&search=primary
	Then the element "*[data-testid='NumberOfPages-Footer']" should have the text content "Showing 101 - 150 of 251 schools or colleges"
	And the elements "*[data-testid='PageLinks-Footer-Prev']" should all have the href "/search/?search=primary&page=2"
	And the elements "*[data-testid='PageLinks-Footer-1']" should all have the href "/search/?search=primary&page=1"
	And the elements "*[data-testid='PageLinks-Footer-2']" should all have the href "/search/?search=primary&page=2"
	And the elements "*[data-testid='PageLinks-Footer-3']" should all have the href "/search/?search=primary&page=3"
	And the elements "*[data-testid='PageLinks-Footer-4']" should all have the href "/search/?search=primary&page=4"
	And the elements "*[data-testid='PageLinks-Footer-5']" should all have the href "/search/?search=primary&page=5"
	And the elements "*[data-testid='PageLinks-Footer-Next']" should all have the href "/search/?search=primary&page=4"
	And the element "*[data-testid='establishment-listing-name-1']" should have the text content "Primary School 100101"
	And the element "*[data-testid='establishment-listing-name-2']" should have the text content "Primary School 100102"
	And the element "*[data-testid='establishment-listing-name-3']" should have the text content "Primary School 100103"
	And the element "*[data-testid='establishment-listing-name-4']" should have the text content "Primary School 100104"
	And the element "*[data-testid='establishment-listing-name-5']" should have the text content "Primary School 100105"
	And the element "*[data-testid='establishment-listing-urn-1']" should have the text content "100101"
	And the element "*[data-testid='establishment-listing-urn-2']" should have the text content "100102"
	And the element "*[data-testid='establishment-listing-urn-3']" should have the text content "100103"
	And the element "*[data-testid='establishment-listing-urn-4']" should have the text content "100104"
	And the element "*[data-testid='establishment-listing-urn-5']" should have the text content "100105"

@Javascript:enabled
Scenario: Pagination in Search Validation 2 (JS)
	Given 251 Establishments exist with properties:
	| urn		  | name						|
	| (100000 + n) | Primary School (100000 + n) |
	When I navigate to /search/?page=3&search=primary
	Then the element "*[data-testid='NumberOfPages-Footer']" should have the text content "Showing 101 - 150 of 251 schools or colleges"
	And the elements "*[data-testid='PageLinks-Footer-Prev']" should all have the href "/search/?search=primary&page=2"
	And the elements "*[data-testid='PageLinks-Footer-1']" should all have the href "/search/?search=primary&page=1"
	And the elements "*[data-testid='PageLinks-Footer-2']" should all have the href "/search/?search=primary&page=2"
	And the elements "*[data-testid='PageLinks-Footer-3']" should all have the href "/search/?search=primary&page=3"
	And the elements "*[data-testid='PageLinks-Footer-4']" should all have the href "/search/?search=primary&page=4"
	And the elements "*[data-testid='PageLinks-Footer-5']" should all have the href "/search/?search=primary&page=5"
	And the elements "*[data-testid='PageLinks-Footer-Next']" should all have the href "/search/?search=primary&page=4"
	And the element "*[data-testid='establishment-listing-name-1']" should have the text content "Primary School 100101"
	And the element "*[data-testid='establishment-listing-name-2']" should have the text content "Primary School 100102"
	And the element "*[data-testid='establishment-listing-name-3']" should have the text content "Primary School 100103"
	And the element "*[data-testid='establishment-listing-name-4']" should have the text content "Primary School 100104"
	And the element "*[data-testid='establishment-listing-name-5']" should have the text content "Primary School 100105"
	And the element "*[data-testid='establishment-listing-urn-1']" should have the text content "100101"
	And the element "*[data-testid='establishment-listing-urn-2']" should have the text content "100102"
	And the element "*[data-testid='establishment-listing-urn-3']" should have the text content "100103"
	And the element "*[data-testid='establishment-listing-urn-4']" should have the text content "100104"
	And the element "*[data-testid='establishment-listing-urn-5']" should have the text content "100105"

 
@Javascript:disabled
Scenario: Pagination in Search Validation 3
	Given 51 Establishments exist with properties:
	| urn		  | name						|
	| (100000 + n) | Primary School (100000 + n) |
	When I navigate to /search/?page=2&search=primary
	Then the element "*[data-testid='NumberOfPages-Footer']" should have the text content "Showing 51 - 51 of 51 schools or colleges"
	And the elements "*[data-testid='PageLinks-Footer-Prev']" should all have the href "/search/?search=primary&page=1"
	And the elements "*[data-testid='PageLinks-Footer-1']" should all have the href "/search/?search=primary&page=1"
	And the element "*[data-testid='establishment-listing-name-1']" should have the text content "Primary School 100051"
	And the element "*[data-testid='establishment-listing-urn-1']" should have the text content "100051"

 
@Javascript:enabled
Scenario: Pagination in Search Validation 3 (JS)
	Given 51 Establishments exist with properties:
	| urn		  | name						|
	| (100000 + n) | Primary School (100000 + n) |
	When I navigate to /search/?page=2&search=primary
	Then the element "*[data-testid='NumberOfPages-Footer']" should have the text content "Showing 51 - 51 of 51 schools or colleges"
	And the elements "*[data-testid='PageLinks-Footer-Prev']" should all have the href "/search/?search=primary&page=1"
	And the elements "*[data-testid='PageLinks-Footer-1']" should all have the href "/search/?search=primary&page=1"
	And the element "*[data-testid='establishment-listing-name-1']" should have the text content "Primary School 100051"
	And the element "*[data-testid='establishment-listing-urn-1']" should have the text content "100051"
	
@Javascript:disabled
Scenario Outline: The PageNo parameter should handle invalid values with a default value of 1
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
	When I navigate to /search/?search=Primary&page=<page>
	Then the page title should be "Search results for "Primary" | Analyse school performance"
	And the element "*[data-testid='NumberOfPages-Footer']" should have the text content "Showing 1 - 2 of 2 schools or colleges"
	And the elements "*[data-testid='PageLinks-Footer-1']" should all have the href "/search/?search=Primary&page=1"
  
Examples:
  | page |
  | y    |
  | 1.5  |
  | 0    |
  | -1   |
 
@Javascript:disabled
Scenario: The PageNo parameter number greater than the total number of pages, the last page of results should be shown
	Given 26 Establishments exist with properties:
	  | urn          | name                        |
	  | (100000 + n) | Primary School (100000 + n) |
	When I navigate to /search/?page=50&search=Primary
	Then the page title should be "Search results for "Primary" | Analyse school performance"
	And the element "*[data-testid='NumberOfPages-Footer']" should have the text content "Showing 1 - 26 of 26 schools or colleges"
	And the elements "*[data-testid='PageLinks-Footer-1']" should all have the href "/search/?search=Primary&page=1"
	And the element "*[data-testid='establishment-listing-name-1']" should have the text content "Primary School 100001"
	And the element "*[data-testid='establishment-listing-name-26']" should have the text content "Primary School 100026"

@Javascript:enabled
Scenario: Autocomplete Should Populate Items When Two Or More Characters Entered
	Given Establishment "111111" exists:
	"""
	{
		"name": "Some Primary School",
		"address": {
			"street": "13 The Street",
			"town": "SomeTown",
			"postCode": "B1 1AA"
		},
	    "laestab": "894/2200"
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
		},
	    "laestab": "894/2201"
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
		},
	    "laestab": "894/2202"
	}
	"""
	And Establishment "444444" exists:
	"""
	{
		"name": "Some Secondary School",
		"address": {
			"street": "13 The Road",
			"town": "SomeTown",
			"postCode": "TR18 3JT"
		},
	    "laestab": "894/2203"
	}
	"""
	When I navigate to /search/
	And I update the textbox "#searchTerm" to have the value "primary"
	Then the autocomplete results should appear
	Then there should be 3 autocomplete items
	Then the elements ".autocomplete__option strong" should have the text contents:
	  | Highlighted Values |
	  | Primary            |
	  | Primary            |
	  | Primary            |
	Then the elements ".autocomplete__option" should have the text contents:
	  | Autocomplete Items                                                                             |
	  | A Different Primary School Address:13 The Road, SomeTown TR18 3JT URN:333333, LAESTAB:894/2202 |
	  | Some Other Primary School Address:13 The Road, Tring B1 1AA URN:222222, LAESTAB:894/2201       |
	  | Some Primary School Address:13 The Street, SomeTown B1 1AA URN:111111, LAESTAB:894/2200        |
   
@Javascript:enabled
Scenario: Autocomplete Should Populate Items When Two Or More Characters Entered Highlighting Name and Address
	Given Establishment "111111" exists:
	"""
	{
		"name": "Some Primary School",
		"address": {
			"street": "13 The Street",
			"town": "SomeTown",
			"postCode": "B1 1AA"
		},
	    "laestab": "894/2200"
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
		},
	    "laestab": "894/2201"
	}
	"""
	And Establishment "333333" exists:
	"""
	{
		"name": "A Different Primary School Centre",
		"address": {
			"street": "13 The Road",
			"town": "SomeTown",
			"postCode": "TR18 3JT"
		},
	    "laestab": "894/2202"
	}
	"""
	And Establishment "444444" exists:
	"""
	{
		"name": "Some Secondary School",
		"address": {
			"street": "13 The Road",
			"town": "SomeTown",
			"postCode": "TR18 3JT"
		},
	    "laestab": "894/2203"
	}
	"""
	When I navigate to /search/
	And I update the textbox "#searchTerm" to have the value "tr"
	Then the autocomplete results should appear
	Then there should be 4 autocomplete items
	Then the elements ".autocomplete__option strong" should have the text contents: 
	  | Highlighted Values |
	  | tr                 |
	  | TR                 |
	  | Tr                 |
	  | tr                 |
	  | TR                 |
	Then the elements ".autocomplete__option" should have the text contents:
	  | Autocomplete Items                                                                                    |
	  | A Different Primary School Centre Address:13 The Road, SomeTown TR18 3JT URN:333333, LAESTAB:894/2202 |
	  | Some Other Primary School Address:13 The Road, Tring B1 1AA URN:222222, LAESTAB:894/2201              |
	  | Some Primary School Address:13 The Street, SomeTown B1 1AA URN:111111, LAESTAB:894/2200               |
	  | Some Secondary School Address:13 The Road, SomeTown TR18 3JT URN:444444, LAESTAB:894/2203             |
   
@Javascript:enabled
Scenario: Autocomplete Should Populate Items When Two Or More Characters Entered Highlighting URN and LaEstab
	Given Establishment "111111" exists:
	"""
	{
		"name": "Some Primary School",
		"address": {
			"street": "13 The Street",
			"town": "SomeTown",
			"postCode": "B1 1AA"
		},
	    "laestab": "894/2200"
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
		},
	    "laestab": "894/2201"
	}
	"""
	And Establishment "333333" exists:
	"""
	{
		"name": "A Different Primary School Centre",
		"address": {
			"street": "13 The Road",
			"town": "SomeTown",
			"postCode": "TR18 3JT"
		},
	    "laestab": "894/2202"
	}
	"""
	And Establishment "444442" exists:
	"""
	{
		"name": "Some Secondary School",
		"address": {
			"street": "13 The Road",
			"town": "SomeTown",
			"postCode": "TR18 3JT"
		},
	    "laestab": "894/2203"
	}
	"""
	When I navigate to /search/
	And I update the textbox "#searchTerm" to have the value "42"
	Then the autocomplete results should appear
	Then there should be 4 autocomplete items
	Then the elements ".autocomplete__option strong" should have the text contents:
	  | Highlighted Values |
	  | 42                 |
	  | 4/2                |
	  | 4/2                |
	  | 4/2                |
	  | 4/2                |
	Then the elements ".autocomplete__option" should have the text contents:
	  | Autocomplete Items                                                                                    |
	  | Some Secondary School Address:13 The Road, SomeTown TR18 3JT URN:444442, LAESTAB:894/2203             |
	  | Some Primary School Address:13 The Street, SomeTown B1 1AA URN:111111, LAESTAB:894/2200               |
	  | Some Other Primary School Address:13 The Road, Tring B1 1AA URN:222222, LAESTAB:894/2201              |
	  | A Different Primary School Centre Address:13 The Road, SomeTown TR18 3JT URN:333333, LAESTAB:894/2202 |
   
@Javascript:disabled
Scenario: Search page returns 'Access not allowed' error for School Named user (Unauthorized user)
	Given I am a School Named user for Establishment "123456"
	When I navigate to /search/
	Then I should get a 403 response
	And the element "h1.govuk-heading-l" should have the text content "Access not allowed"
	
@Javascript:disabled
Scenario: Search functionality returns server error for LA Named user when no Local Authorities are present
	Given I am a LA Named user for Local Authority "100"
	And no Local Authorities exist
	When I navigate to /search/
	And I update the textbox "#searchTerm" to have the value "Test"
	And I click the button "#searchSubmit"
	Then I should get a 500 response
	And the page title should be "Sorry, there is a problem with the service | Analyse school performance"
	
@Javascript:disabled
Scenario: Search functionality returns no results for LA Named user when querying establishments from other Local Authorities
	Given I am a LA Named user for Local Authority "100"
	And Local Authority "100" exists:
	"""
		{ 
		 "name": "Test LA"
		}
	"""
	And Establishment "111111" exists:
	"""
		{
			"name": "Test School 1",
			"localAuthority":
			 {
			 	"code": "999"
			 }
		}
	"""
	When I navigate to /search/
	And I update the textbox "#searchTerm" to have the value "Test"
	And I click the button "#searchSubmit"
	Then the element "h1.govuk-heading-l" should have the text content "We found no matches for "Test""	
	
	
@Javascript:disabled
Scenario: LA Named user sees only establishments within their Local Authority in search results
	Given I am a LA Named user for Local Authority "100"
	And Local Authority "100" exists:
	"""
		{ 
		"name": "Test LA"
		}
	"""
	And Establishment "111111" exists:
	"""
		{
			"name": "Test School 1",
			"localAuthority":
			{
			"code": "100"
			}
		}
	"""
	And Establishment "222222" exists:
	"""
		{
			"name": "Test School 2",
			"localAuthority":
			{
			"code": "100"
			}
		}
	"""
	And Establishment "333333" exists:
	"""
		{
			"name": "Test School 3",
			"localAuthority":
			{
			"code": "999"
			}
		}
	"""
	When I navigate to /search/
	And I update the textbox "#searchTerm" to have the value "Test"
	And I click the button "#searchSubmit"
	Then the element "*[data-testid='NumberOfPages-Footer']" should have the text content "Showing 1 - 2 of 2 schools or colleges"
	And the element "[data-testid="establishment-listing-urn-<Counter>"]" should have the text content "<URN>"
	And the element "[data-testid="establishment-listing-name-<Counter>"]" should have the text content "<Name>"
Examples: 
  | Counter | URN    | Name          |
  | 1       | 111111 | Test School 1 |
  | 2       | 222222 | Test School 2 |
  

@Javascript:disabled
Scenario: MAT Named user encounters server error when searching with no Multi-Academy Trusts existing
	Given I am a MAT Named user for Multi-Academy Trust "1234"
	And no Multi Academy Trust exist
	When I navigate to /search/
	And I update the textbox "#searchTerm" to have the value "Test"
	And I click the button "#searchSubmit"
	Then I should get a 500 response
	And the page title should be "Sorry, there is a problem with the service | Analyse school performance"
	
	
@Javascript:disabled
Scenario: MAT Named user receives no results when searching for establishments not associated with their Multi-Academy Trust
	Given I am a MAT Named user for Multi-Academy Trust "1234"
	And Multi Academy Trust "1234" exists:
	  """
	  { 
	  	"name": "Test MAT"
	  }
	  """
	And Establishment "111111" exists:
	  """
	  {
	    "name": "Test School 1"
	  }
	  """
	When I navigate to /search/
	And I update the textbox "#searchTerm" to have the value "Test"
	And I click the button "#searchSubmit"
	Then the element "h1.govuk-heading-l" should have the text content "We found no matches for "Test""	
	
@Javascript:disabled
Scenario: MAT Named user sees only establishments within their Multi-Academy Trust in search results
	Given I am a MAT Named user for Multi-Academy Trust "1234"
	And Multi Academy Trust "1234" exists:
	"""
		{ 
		 "name": "Test MAT"
		}
	"""
	And Establishment "111111" exists:
	"""
		{
		 "name": "Test School 1",
	      "multiAcademyTrust": {
	         "uid": 1234
	      }
		}
	"""
	And Establishment "222222" exists:
	"""
		{
		"name": "Test School 2"
		}
	"""
	And Establishment "333333" exists:
	"""
		{
		 "name": "Test School 3",
	      "multiAcademyTrust": {
	         "uid": 1234
	      }
		}
    """
	When I navigate to /search/
	And I update the textbox "#searchTerm" to have the value "Test"
	And I click the button "#searchSubmit"
	Then the element "*[data-testid='NumberOfPages-Footer']" should have the text content "Showing 1 - 2 of 2 schools or colleges"
	And the element "[data-testid="establishment-listing-urn-<Counter>"]" should have the text content "<URN>"
	And the element "[data-testid="establishment-listing-name-<Counter>"]" should have the text content "<Name>"
	Examples: 
	| Counter | URN      | Name          |
	| 1       | 111111   | Test School 1 |
	| 2       | 333333   | Test School 3 |


@Javascript:disabled
Scenario: Diocese Named user receives no results when searching for establishments not associated with any diocese		
	 Given I am a Diocese Named user for Diocese "Test Diocese"
	 And Establishment "111111" exists:
	 """
	  {
	        "name": "Test School 1",
	        "diocese": {
	            "code": "1000",
	            "name": "Test Diocese1",
	            "lname": "test diocese1",
	            "isNullish": false
	       }
	     }
	 """
	 And Establishment "222222" exists:
	 """
	  {
	      "name": "Test School 2",
	      "diocese": null
	  }
	 """
	 And Establishment "333333" exists:
	 """
	 {
	 	"name": "Test School 3"
	 }
	 """
	 When I navigate to /search/
	 And I update the textbox "#searchTerm" to have the value "Test"
	 And I click the button "#searchSubmit"
	 Then the element "h1.govuk-heading-l" should have the text content "We found no matches for "Test""	  
	 
@Javascript:disabled
Scenario: Diocese Named user sees only establishments within their Diocese in search results	 
	Given I am a Diocese Named user for Diocese "Test Diocese"
	And Establishment "111111" exists:
		"""
		 {
		     "name": "Test School 1",
		     "diocese": {
		         "code": "1000",
		         "name": "Test Diocese",
		         "lname": "test diocese",
		         "isNullish": false
		     }
		 }
		"""
	And Establishment "222222" exists:
		"""
		 {
		    "name": "Test School 2",
		    "diocese": {
		        "code": "1001",
		        "name": "Another Diocese",
		        "lname": "another diocese",
		        "isNullish": false
		    }
		 }
		"""
	And Establishment "333333" exists:
		"""
		 {
		    "name": "Test School 3",
		    "diocese": {
		        "code": "1000",
		        "name": "Test Diocese",
		        "lname": "test diocese",
		        "isNullish": false
		    }
		 }
		"""
	When I navigate to /search/
	And I update the textbox "#searchTerm" to have the value "Test"
	And I click the button "#searchSubmit"
	Then the element "*[data-testid='NumberOfPages-Footer']" should have the text content "Showing 1 - 2 of 2 schools or colleges"
	And the element "[data-testid="establishment-listing-urn-<Counter>"]" should have the text content "<URN>"
	And the element "[data-testid="establishment-listing-name-<Counter>"]" should have the text content "<Name>"
	Examples: 
	| Counter | URN      | Name          |
	| 1       | 111111   | Test School 1 |
	| 2       | 333333   | Test School 3 |
	
@Javascript:disabled
Scenario: DfE Named user sees all establishments across different affiliations in search results
	Given I am a DfE Named user
	And Local Authority "100" exists:
	 """
		{ 
		   "name": "Test LA"
		}
	 """
	And Establishment "111111" exists:
	 """
		{
		   "name": "Test School 1",
		   "localAuthority":
		   {
		       "code": "100"
		   }
		}
	 """
	And Multi Academy Trust "1234" exists:
	 """
		{ 
		   "name": "Test MAT"
		}
	 """
	And Establishment "222222" exists:
	 """
		{
		   "name": "Test School 2"
		}
	 """
	And Establishment "333333" exists:
	 """
		{
		   "name": "Test School 3",
		   "diocese": {
		       "code": "1000",
		       "name": "Test Diocese",
		       "lname": "test diocese",
		       "isNullish": false
		   }
		}
	 """
	When I navigate to /search/
	And I update the textbox "#searchTerm" to have the value "Test"
	And I click the button "#searchSubmit"
	Then the element "*[data-testid='NumberOfPages-Footer']" should have the text content "Showing 1 - 3 of 3 schools or colleges"
	And the element "[data-testid="establishment-listing-urn-<Counter>"]" should have the text content "<URN>"
	And the element "[data-testid="establishment-listing-name-<Counter>"]" should have the text content "<Name>"
	Examples: 
	| Counter | URN      | Name          |
	| 1       | 111111   | Test School 1 |
	| 2       | 222222   | Test School 2 |
	| 3       | 333333   | Test School 3 |