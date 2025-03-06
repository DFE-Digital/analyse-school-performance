Feature: All schools page

Background:
	Given I am a DfE Named user

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
	And the page title should be "Access not allowed"
	And the element "h1.govuk-heading-l" should have the text content "Access not allowed"

@Javascript:disabled
Scenario: No results when no schools exist
	Given no Establishments exist
	When I navigate to /schools/
	Then the page title should be "We found no schools"

@Javascript:disabled
Scenario Outline: Generic Local authority > All schools page - common page elements
	Given 251 Establishments exist with properties:
		| urn          | name                        |
		| (100000 + n) | Primary School (100000 + n) |
	And I am a <AccessToAllSchools> user
	When I navigate to /schools/
	Then I should get a 200 response
	And the page title should be "All schools"
	And the page subtitle should be "251 schools"
	And the breadcrumb trail should be:
		| Link Text | Url |
		| Home      | /   |

Examples:
	| AccessToAllSchools |
	| DfE Named          |
	| DfE Unnamed        |
	| Ofsted Unnamed     |
	| Super Admin        |

@Javascript:disabled
Scenario: DfE Named user should see All schools
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
	Then the pagination summary should be "Showing 1 - 4 of 4 schools"
	And the listings should be:
		| Index | URN    | Name                       | Address                        | Url             |
		| 1     | 333333 | A Different Primary School | 13 The Road, SomeTown TR18 3JT | /school/333333/ |
		| 2     | 222222 | Some Other Primary School  | 13 The Road, Tring B1 1AA      | /school/222222/ |
		| 3     | 111111 | Some Primary School        | 13 The Street, SomeTown B1 1AA | /school/111111/ |
		| 4     | 444444 | The Training Centre        | Data not available             | /school/444444/ |

@Javascript:disabled
Scenario: Pagination in all schools
	Given 251 Establishments exist with properties:
		| urn          | name                        |
		| (100000 + n) | Primary School (100000 + n) |
	When I navigate to /schools/
	Then the pagination summary should be "Showing 1 - 50 of 251 schools"
	And the pagination links should be:
		| Link Text | Url              |
		| 1         | /schools/?page=1 |
		| 2         | /schools/?page=2 |
		| ...       |                  |
		| 6         | /schools/?page=6 |
		| Next page | /schools/?page=2 |
	And the listings should be:
		| Index | URN    | Name                  |
		| 1     | 100001 | Primary School 100001 |
		| 2     | 100002 | Primary School 100002 |
		| 3     | 100003 | Primary School 100003 |
		| 4     | 100004 | Primary School 100004 |
		| 5     | 100005 | Primary School 100005 |

@Javascript:disabled
Scenario: Pagination in all schools Validation 2
	Given 501 Establishments exist with properties:
		| urn          | name                        |
		| (100000 + n) | Primary School (100000 + n) |
	When I navigate to /schools/?page=3
	Then the pagination summary should be "Showing 101 - 150 of 501 schools"
	And the pagination links should be:
		| Link Text | Url               |
		| Prev page | /schools/?page=2  |
		| 1         | /schools/?page=1  |
		| 2         | /schools/?page=2  |
		| 3         | /schools/?page=3  |
		| 4         | /schools/?page=4  |
		| ...       |                   |
		| 11        | /schools/?page=11 |
		| Next page | /schools/?page=4  |
	And the listings should be:
		| Index | URN    | Name                  |
		| 1     | 100101 | Primary School 100101 |
		| 2     | 100102 | Primary School 100102 |
		| 3     | 100103 | Primary School 100103 |
		| 4     | 100104 | Primary School 100104 |
		| 5     | 100105 | Primary School 100105 |

@Javascript:disabled
Scenario: Page should show a breadcrumb trail
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
	When I navigate to /schools/
	Then I should get a 200 response
	And the page title should be "All schools"
	And the breadcrumb trail should be:
		| Link Text | Url |
		| Home      | /   |

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
	When I navigate to /schools/
	And I update the textbox "#app-field-Search" to have the value "Primary"
	And I click the button "#searchSubmit"
	Then the path should be /schools/?search=Primary
	And the page title should be "Search results for "Primary""

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
	When I navigate to /schools/
	And I update the textbox "#app-field-Search" to have the value "Primary"
	And I click the button "#searchSubmit"
	Then the path should be /schools/?search=Primary
	And the breadcrumb trail should be:
		| Link Text   | Url       |
		| Home        | /         |
		| All schools | /schools/ |

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
	When I navigate to /schools/
	And I update the textbox "#app-field-Search" to have the value "Secondary"
	And I click the button "#searchSubmit"
	Then the path should be /schools/?search=Secondary
	And the breadcrumb trail should be:
		| Link Text   | Url       |
		| Home        | /         |
		| All schools | /schools/ |

@Javascript:disabled
Scenario: Search Term validation
	Given Establishment "111111" exists:
		"""
		{
			"name": "Some Primary School"
		}
		"""
	When I navigate to /schools/
	Then I should get a 200 response
	And the page title should be "All schools"
	And the element "#searchForm" should have the text content "Enter school name, address, URN (Unique Reference Number) or LAESTAB (Local Authority Establishment Number) Search"

@Javascript:enabled
Scenario: Search Term validation still works with JS enabled
	Given Establishment "111111" exists:
		"""
		{
			"name": "Some Primary School"
		}
		"""
	When I navigate to /schools/
	Then I should get a 200 response
	And the page title should be "All schools"
	And the element "#searchForm" should have the text content "Enter school name, address, URN (Unique Reference Number) or LAESTAB (Local Authority Establishment Number) When autocomplete results are available use up and down arrows to review and enter to select. Touch device users, explore by touch or with swipe gestures. Search"

@Javascript:disabled
Scenario: Search Term validation errors
	Given Establishment "111111" exists:
		"""
		{
			"name": "Some Primary School"
		}
		"""
	When I navigate to /schools/
	And I click the button "#searchSubmit"
	Then the path should be /schools/?search=
	And the element "#app-field-Search-input-error" should have the text content "Please enter a school name, address, URN (Unique Reference Number) or LAESTAB (Local Authority Establishment Number)"
	And the element "h2.govuk-error-summary__title" should have the text content "There is a problem"
	And the element "*[data-testid='app-error-summary-Search']" should have the text content "Please enter a school name, address, URN (Unique Reference Number) or LAESTAB (Local Authority Establishment Number)"

@Javascript:enabled
Scenario: Search Term validation errors still work with JS enabled
	Given Establishment "111111" exists:
		"""
		{
			"name": "Some Primary School"
		}
		"""
	When I navigate to /schools/
	And I click the button "#searchSubmit"
	Then the path should be /schools/?search=
	And the element "#app-field-Search-input-error" should have the text content "Please enter a school name, address, URN (Unique Reference Number) or LAESTAB (Local Authority Establishment Number)"
	And the element "h2.govuk-error-summary__title" should have the text content "There is a problem"
	And the element "*[data-testid='app-error-summary-Search']" should have the text content "Please enter a school name, address, URN (Unique Reference Number) or LAESTAB (Local Authority Establishment Number)"

@Javascript:disabled
Scenario: School search page should show correct message for search term with no matches
	Given Establishment "111111" exists:
		"""
		{
			"name": "Some Primary School"
		}
		"""
	When I navigate to /schools/
	And I update the textbox "#app-field-Search" to have the value "secondary"
	And I click the button "#searchSubmit"
	Then the path should be /schools/?search=secondary
	And the element "[data-testid="result-not-found-search-url"]" should have the href "/schools/"
	And the page title should be "We found no matches for "secondary""

@Javascript:disabled
Scenario: Pagination in Search Validation
	Given 251 Establishments exist with properties:
		| urn          | name                        |
		| (100000 + n) | Primary School (100000 + n) |
	When I navigate to /schools/?search=primary
	Then the pagination summary should be "Showing 1 - 50 of 251 schools"
	And the pagination links should be:
		| Link Text | Url                             |
		| 1         | /schools/?search=primary&page=1 |
		| 2         | /schools/?search=primary&page=2 |
		| ...       |                                 |
		| 6         | /schools/?search=primary&page=6 |
		| Next page | /schools/?search=primary&page=2 |
	And the listings should be:
		| Index | URN    | Name                  |
		| 1     | 100001 | Primary School 100001 |
		| 2     | 100002 | Primary School 100002 |
		| 3     | 100003 | Primary School 100003 |
		| 4     | 100004 | Primary School 100004 |
		| 5     | 100005 | Primary School 100005 |

@Javascript:disabled
Scenario: Pagination in Search Validation 2
	Given 501 Establishments exist with properties:
		| urn          | name                        |
		| (100000 + n) | Primary School (100000 + n) |
	When I navigate to /schools/?page=3&search=primary
	Then the pagination summary should be "Showing 101 - 150 of 501 schools"
	And the pagination links should be:
		| Link Text | Url                              |
		| Prev page | /schools/?search=primary&page=2  |
		| 1         | /schools/?search=primary&page=1  |
		| 2         | /schools/?search=primary&page=2  |
		| 3         | /schools/?search=primary&page=3  |
		| 4         | /schools/?search=primary&page=4  |
		| ...       |                                  |
		| 11        | /schools/?search=primary&page=11 |
		| Next page | /schools/?search=primary&page=4  |
	And the listings should be:
		| Index | URN    | Name                  |
		| 1     | 100101 | Primary School 100101 |
		| 2     | 100102 | Primary School 100102 |
		| 3     | 100103 | Primary School 100103 |
		| 4     | 100104 | Primary School 100104 |
		| 5     | 100105 | Primary School 100105 |

@Javascript:disabled
Scenario: Pagination in Search Validation 3
	Given 51 Establishments exist with properties:
		| urn          | name                        |
		| (100000 + n) | Primary School (100000 + n) |
	When I navigate to /schools/?page=2&search=primary
	Then the pagination summary should be "Showing 51 - 51 of 51 schools"
	And the pagination links should be:
		| Link Text | Url                             |
		| Prev page | /schools/?search=primary&page=1 |
		| 1         | /schools/?search=primary&page=1 |
		| 2         | /schools/?search=primary&page=2 |
	And the listings should be:
		| URN    | Name                  |
		| 100051 | Primary School 100051 |

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
	When I navigate to /schools/
	And I update the textbox "#app-field-Search" to have the value "111111"
	And I click the button "#searchSubmit"
	Then the path should be /school/111111/
	And the element "#app-page-subtitle span" should have the text content "(URN: 111111)"

@Javascript:enabled
Scenario: Matching URN search should redirect to school landing page with JS enabled
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
	When I navigate to /schools/
	And I update the textbox "#app-field-Search" to have the value "111111"
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
	When I navigate to /schools/
	And I update the textbox "#app-field-Search" to have the value "PRiMaRY"
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
	When I navigate to /schools/
	And I update the textbox "#app-field-Search" to have the value "str"
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
	When I navigate to /schools/
	And I update the textbox "#app-field-Search" to have the value "some"
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
	When I navigate to /schools/
	And I update the textbox "#app-field-Search" to have the value "tr1"
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
	When I navigate to /schools/
	And I update the textbox "#app-field-Search" to have the value "tr"
	And I click the button "#searchSubmit"
	Then the path should be /schools/?search=tr
	And the listings should be:
		| Index | URN    | Name                       | Address                        | Url             |
		| 1     | 333333 | A Different Primary School | 13 The Road, SomeTown TR18 3JT | /school/333333/ |
		| 2     | 222222 | Some Other Primary School  | 13 The Road, Tring B1 1AA      | /school/222222/ |
		| 3     | 111111 | Some Primary School        | 13 The Street, SomeTown B1 1AA | /school/111111/ |
		| 4     | 444444 | The Training Centre        | Data not available             | /school/444444/ |

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
	When I navigate to /schools/
	And I update the textbox "#app-field-Search" to have the value "111111"
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
	When I navigate to /schools/
	And I update the textbox "#app-field-Search" to have the value "<SearchTerm>"
	And I click the button "#searchSubmit"
	Then the path should be /schools/?search=<SearchTerm>
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
	When I navigate to /schools/
	And I update the textbox "#app-field-Search" to have the value "<SearchTerm>"
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
	When I navigate to /schools/
	And I update the textbox "#app-field-Search" to have the value "111111"
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
	When I navigate to /schools/
	And I update the textbox "#app-field-Search" to have the value "894/2200"
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
	When I navigate to /schools/
	And I update the textbox "#app-field-Search" to have the value "8942200"
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
	When I navigate to /schools/
	And I update the textbox "#app-field-Search" to have the value "894"
	And I click the button "#searchSubmit"
	Then the path should be /schools/?search=894
	And the listings should be:
		| Index | URN    | LAESTAB  | Name                      | Url             |
		| 2     | 111111 | 894/2200 | Some Primary School       | /school/111111/ |
		| 1     | 222222 | 894/1234 | Some Other Primary School | /school/222222/ |

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
	When I navigate to /schools/
	And I update the textbox "#app-field-Search" to have the value "2200"
	And I click the button "#searchSubmit"
	Then the path should be /schools/?search=2200
	And the listings should be:
		| Index | URN    | LAESTAB  | Name                      | Url             |
		| 2     | 111111 | 894/2200 | Some Primary School       | /school/111111/ |
		| 1     | 222222 | 600/2200 | Some Other Primary School | /school/222222/ |

@Javascript:disabled
Scenario: Partial LAESTAB (LA part) match should show no matching results
	Given Establishment "111111" exists:
		"""
		{
			"name": "Some Primary School",
			"laestab" : "894/2200"
		}
		"""
	When I navigate to /schools/
	And I update the textbox "#app-field-Search" to have the value "89"
	And I click the button "#searchSubmit"
	Then the path should be /schools/?search=89
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
	When I navigate to /schools/
	And I update the textbox "#app-field-Search" to have the value "22"
	And I click the button "#searchSubmit"
	Then the path should be /schools/?search=22
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
	When I navigate to /schools/
	And I update the textbox "#app-field-Search" to have the value "8942200"
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
	When I navigate to /schools/
	And I update the textbox "#app-field-Search" to have the value "894/2200"
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
	When I navigate to /schools/
	And I update the textbox "#app-field-Search" to have the value "894"
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
	When I navigate to /schools/
	And I update the textbox "#app-field-Search" to have the value "2200"
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
	When I navigate to /schools/
	And I update the textbox "#app-field-Search" to have the value "School"
	And I click the button "#searchSubmit"
	Then the path should be /schools/?search=School
	And the listings should be:
		| Index | URN    | Name     | Address                                   |
		| 1     | 111111 | School A | 13 The Street AB12 3CD                    |
		| 2     | 222222 | School B | 2a Mornington Crescent, Liverpool LL1 1AB |
		| 3     | 333333 | School C | 34 Long Road, Sheffield                   |

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
	When I navigate to /schools/?search=Primary&page=<page>
	Then the page title should be "Search results for "Primary""
	And the page subtitle should be "2 schools"
	And the pagination summary should be "Showing 1 - 2 of 2 schools"
	And the pagination links should be empty

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
	When I navigate to /schools/?page=50&search=Primary
	Then the page title should be "Search results for "Primary""
	And the page subtitle should be "26 schools"
	And the pagination summary should be "Showing 1 - 26 of 26 schools"
	And the pagination links should be empty
	And the listings should be:
		| Index | URN    | Name                  |
		| 1     | 100001 | Primary School 100001 |
		| 26    | 100026 | Primary School 100026 |

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
	When I navigate to /schools/
	And I update the textbox "#app-field-Search" to have the value "primary"
	Then the autocomplete results should appear
	And there should be 3 autocomplete items
	And the elements ".autocomplete__option strong" should have the text contents:
		| Highlighted Values |
		| Primary            |
		| Primary            |
		| Primary            |
	And the elements ".autocomplete__option" should have the text contents:
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
	When I navigate to /schools/
	And I update the textbox "#app-field-Search" to have the value "tr"
	Then the autocomplete results should appear
	And there should be 4 autocomplete items
	And the elements ".autocomplete__option strong" should have the text contents:
		| Highlighted Values |
		| tr                 |
		| TR                 |
		| Tr                 |
		| tr                 |
		| TR                 |
	And the elements ".autocomplete__option" should have the text contents:
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
	When I navigate to /schools/
	And I update the textbox "#app-field-Search" to have the value "42"
	Then the autocomplete results should appear
	And there should be 4 autocomplete items
	And the elements ".autocomplete__option strong" should have the text contents:
		| Highlighted Values |
		| 42                 |
		| 4/2                |
		| 4/2                |
		| 4/2                |
		| 4/2                |
	And the elements ".autocomplete__option" should have the text contents:
		| Autocomplete Items                                                                                    |
		| Some Secondary School Address:13 The Road, SomeTown TR18 3JT URN:444442, LAESTAB:894/2203             |
		| Some Primary School Address:13 The Street, SomeTown B1 1AA URN:111111, LAESTAB:894/2200               |
		| Some Other Primary School Address:13 The Road, Tring B1 1AA URN:222222, LAESTAB:894/2201              |
		| A Different Primary School Centre Address:13 The Road, SomeTown TR18 3JT URN:333333, LAESTAB:894/2202 |