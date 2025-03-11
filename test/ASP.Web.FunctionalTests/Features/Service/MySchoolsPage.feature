Feature: My schools page

Background:
	Given I am a LA Named user for Local Authority 100

@Javascript:disabled
Scenario: School Named user is denied access to my-schools page
	Given I am a School Named user for Establishment 123456
	When I navigate to /my-schools/
	Then I should get a 403 response
	And the element "h1.govuk-heading-l" should have the text content "Access not allowed"

@Javascript:disabled
Scenario: DfE Named user is denied access to my-schools page
	Given I am a DfE Named user
	When I navigate to /my-schools/
	Then I should get a 403 response
	And the element "h1.govuk-heading-l" should have the text content "Access not allowed"

@Javascript:disabled
Scenario: Ofsted Unnamed user is denied access to my-schools page
	Given I am a Ofsted Unnamed user
	When I navigate to /my-schools/
	Then I should get a 403 response
	And the element "h1.govuk-heading-l" should have the text content "Access not allowed"

@Javascript:disabled
Scenario: Server error when accessing /my-schools/ with LA Named role when LA doesn't exist
	Given I am a LA Named user for Local Authority 100
	When I navigate to /my-schools/
	Then I should get a 500 response
	And the page title should be "Sorry, there is a problem with the service"

@Javascript:disabled
Scenario: No results for LA Named user when no schools in their Local Authority
	Given I am a LA Named user for Local Authority 100
	And local authority Test LA (100) exists
	And establishment Test School 1 (111111) exists in local authority 999
	When I navigate to /my-schools/
	Then the page title should be "We found no schools"

@Javascript:disabled
Scenario: Correct results displayed for LA Named user with schools in their Local Authority
	Given I am a LA Named user for Local Authority 100
	And local authority Test LA (100) exists
	And establishment Test School 1 (111111) exists in local authority 100
	And establishment Test School 2 (222222) exists in local authority 100
	And establishment Test School 3 (333333) exists in local authority 999
	When I navigate to /my-schools/
	Then the pagination summary should be "Showing 1 - 2 of 2 schools"
	And the listings should be:
		| Index | URN    | Name          |
		| 1     | 111111 | Test School 1 |
		| 2     | 222222 | Test School 2 |

@Javascript:disabled
Scenario: Server error when MAT Named user accesses /my-schools/ page and MAT doesn't exist
	Given I am a MAT Named user for Multi-Academy Trust 1234
	When I navigate to /my-schools/
	Then I should get a 500 response
	And the page title should be "Sorry, there is a problem with the service"

@Javascript:disabled
Scenario: MAT Named user sees 'No schools found' message when MAT has no associated schools
	Given I am a MAT Named user for Multi-Academy Trust 1234
	And multi-academy trust Test MAT (1234) exists
	And establishment Test School 1 (111111) exists
	When I navigate to /my-schools/
	Then the page title should be "We found no schools"

@Javascript:disabled
Scenario: MAT Named user sees correct list of schools associated with their Multi-Academy Trust
	Given I am a MAT Named user for Multi-Academy Trust 1234
	And multi-academy trust Test MAT (1234) exists
	And establishment Test School 1 (111111) exists in multi-academy trust 1234
	And establishment Test School 2 (222222) exists
	And establishment Test School 3 (333333) exists in multi-academy trust 1234
	When I navigate to /my-schools/
	Then the listings should be:
		| Index | URN    | Name          |
		| 1     | 111111 | Test School 1 |
		| 2     | 333333 | Test School 3 |

@Javascript:disabled
Scenario: Diocese Named user sees 'No schools found' message when no schools are associated with their diocese
	Given I am a Diocese Named user for Diocese Test Diocese
	And establishment Test School 1 (111111) exists with properties:
		"""
		{
		    "diocese": {
		        "code": "0000",
		        "name": "Not applicable",
		        "lname": "not applicable",
		        "isNullish": true
		    }
		}
		"""
	And establishment Test School 2 (222222) exists with properties:
		"""
		{
		    "diocese": null
		}
		"""
	And establishment Test School 3 (333333) exists
	When I navigate to /my-schools/
	Then the page title should be "We found no schools"

@Javascript:disabled
Scenario: Diocese Named user sees correct list of schools associated with their diocese
	Given I am a Diocese Named user for Diocese Test Diocese
	And establishment Test School 1 (111111) exists in diocese Test Diocese
	And establishment Test School 2 (222222) exists in diocese Another Diocese
	And establishment Test School 3 (333333) exists in diocese Test Diocese
	When I navigate to /my-schools/
	Then the pagination summary should be "Showing 1 - 2 of 2 schools"
	And the listings should be:
		| Index | URN    | Name          |
		| 1     | 111111 | Test School 1 |
		| 2     | 333333 | Test School 3 |

@Javascript:disabled
Scenario: Pagination in my schools
	Given I am a MAT Named user for Multi-Academy Trust 1234
	And multi-academy trust Test MAT (1234) exists
	And 251 establishments exist with properties:
		| urn          | name                        | multiAcademyTrust |
		| (100000 + n) | Primary School (100000 + n) | { "uid": "1234" } |
	When I navigate to /my-schools/
	Then the pagination summary should be "Showing 1 - 50 of 251 schools"
	And the pagination links should be:
		| Link Text | Url                 |
		| 1         | /my-schools/?page=1 |
		| 2         | /my-schools/?page=2 |
		| ...       |                     |
		| 6         | /my-schools/?page=6 |
		| Next page | /my-schools/?page=2 |
	And the listings should be:
		| Index | URN    | Name                  |
		| 1     | 100001 | Primary School 100001 |
		| 2     | 100002 | Primary School 100002 |
		| 3     | 100003 | Primary School 100003 |
		| 4     | 100004 | Primary School 100004 |
		| 5     | 100005 | Primary School 100005 |

@Javascript:disabled
Scenario Outline: My schools page - common page elements
	Given 251 establishments exist with properties:
		| urn          | name                        | multiAcademyTrust |
		| (100000 + n) | Primary School (100000 + n) | { "uid": 1234 }   |
	And multi-academy trust Test MAT (1234) exists
	And I am a MAT Named user for Multi-Academy Trust 1234
	When I navigate to /my-schools/
	Then I should get a 200 response
	And the page title should be "My schools"
	And the page subtitle should be "251 schools"
	And the breadcrumb trail should be:
		| Link Text | Url |
		| Home | /    |

@Javascript:disabled
Scenario: Page title should show correct text when search returns results
	Given local authority Test LA (100) exists
	And establishment Some Primary School (111111) exists in local authority 100 with properties:
		"""
		{
			"address": {
				"street": "13 The Street",
				"town": "SomeTown",
				"postCode": "B1 1AA"
			}
		}
		"""
	And establishment Some Other Primary School (222222) exists in local authority 100 with properties:
		"""
		{
			"address": {
				"street": "13 The Road",
				"town": "Tring",
				"postCode": "B1 1AA"
			}
		}
		"""
	When I navigate to /my-schools/
	And I update the textbox "#app-field-Search" to have the value "Primary"
	And I click the button "#searchSubmit"
	Then the path should be /my-schools/?search=Primary
	And the page title should be "Search results for "Primary""

@Javascript:disabled
Scenario: LA user sees 'No school found' message when searching for school URNs that are not associated with their LA
	Given local authority Test LA (001) exists
	Given I am a LA Named user for Local Authority 001
	And establishment Test School 1 (111111) exists in local authority 002
	And establishment Test School 2 (222222) exists in local authority 001
	When I navigate to /my-schools/
	And I update the textbox "#app-field-Search" to have the value "111111"
	And I click the button "#searchSubmit"
	Then the path should be /my-schools/?search=111111
	And the page title should be "We found no matches for "111111""


@Javascript:disabled
Scenario: Diocese user sees 'No school found' message when searching for school URNs that are not associated with their Diocese
	Given I am a Diocese Named user for Diocese Test Diocese
	And establishment Test School 1 (111111) exists in diocese Test Diocese 1
	And establishment Test School (222222) exists in diocese Test Diocese
	When I navigate to /my-schools/
	And I update the textbox "#app-field-Search" to have the value "111111"
	And I click the button "#searchSubmit"
	Then the path should be /my-schools/?search=111111
	And the page title should be "We found no matches for "111111""

@Javascript:disabled
Scenario: MAT user sees 'No school found' message when searching for school URNs that are not associated with their MAT
	And multi-academy trust Test MAT (1111) exists
	Given I am a MAT Named user for Multi-Academy Trust 1111
	And establishment Test School 1 (111111) exists in multi-academy trust 2222
	And establishment Test School (222222) exists in multi-academy trust 1111
	When I navigate to /my-schools/
	And I update the textbox "#app-field-Search" to have the value "111111"
	And I click the button "#searchSubmit"
	Then the path should be /my-schools/?search=111111
	And the page title should be "We found no matches for "111111""

@Javascript:disabled
Scenario: Page should show a breadcrumb trail when search returns results
	Given local authority Test LA (100) exists
	And establishment Some Primary School (111111) exists in local authority 100 with properties:
		"""
		{
			"address": {
				"street": "13 The Street",
				"town": "SomeTown",
				"postCode": "B1 1AA"
			}
		}
		"""
	And establishment Some Other Primary School (222222) exists in local authority 100 with properties:
		"""
		{
			"address": {
				"street": "13 The Road",
				"town": "Tring",
				"postCode": "B1 1AA"
			}
		}
		"""
	When I navigate to /my-schools/
	And I update the textbox "#app-field-Search" to have the value "Primary"
	And I click the button "#searchSubmit"
	Then the path should be /my-schools/?search=Primary
	And the breadcrumb trail should be:
		| Link Text  | Url          |
		| Home       | /            |
		| My schools | /my-schools/ |

@Javascript:disabled
Scenario: Page should show a breadcrumb trail when search returns no results
	Given local authority Test LA (100) exists
	And establishment Some Primary School (111111) exists in local authority 100 with properties:
		"""
		{
			"address": {
				"street": "13 The Street",
				"town": "SomeTown",
				"postCode": "B1 1AA"
			}
		}
		"""
	And establishment Some Other Primary School (222222) exists in local authority 100 with properties:
		"""
		{
			"address": {
				"street": "13 The Road",
				"town": "Tring",
				"postCode": "B1 1AA"
			}
		}
		"""
	When I navigate to /my-schools/
	And I update the textbox "#app-field-Search" to have the value "Secondary"
	And I click the button "#searchSubmit"
	Then the path should be /my-schools/?search=Secondary
	And the breadcrumb trail should be:
		| Link Text  | Url          |
		| Home       | /            |
		| My schools | /my-schools/ |

@Javascript:disabled
Scenario: Search Term validation
	Given establishment Some Primary School (111111) exists in local authority Oxfordshire (100)
	And local authority Oxfordshire (100) exists
	When I navigate to /my-schools/
	Then I should get a 200 response
	And the page title should be "My schools"
	And the element "#searchForm" should have the text content "Enter school name, address, URN (Unique Reference Number) or LAESTAB (Local Authority Establishment Number) Search"

@Javascript:enabled
Scenario: Search Term validation should work with JS enabled
	Given establishment Some Primary School (111111) exists in local authority Oxfordshire (100)
	And local authority Oxfordshire (100) exists
	When I navigate to /my-schools/
	Then I should get a 200 response
	And the page title should be "My schools"
	And the element "#searchForm" should have the text content "Enter school name, address, URN (Unique Reference Number) or LAESTAB (Local Authority Establishment Number) When autocomplete results are available use up and down arrows to review and enter to select. Touch device users, explore by touch or with swipe gestures. Search"

@Javascript:disabled
Scenario: Search Term validation errors
	Given establishment Some Primary School (111111) exists in local authority Oxfordshire (100)
	And local authority Oxfordshire (100) exists
	When I navigate to /my-schools/
	And I click the button "#searchSubmit"
	Then the path should be /my-schools/?search=
	And the element "#app-field-Search-input-error" should have the text content "Please enter a school name, address, URN (Unique Reference Number) or LAESTAB (Local Authority Establishment Number)"
	And the element "h2.govuk-error-summary__title" should have the text content "There is a problem"
	And the element "*[data-testid='app-error-summary-Search']" should have the text content "Please enter a school name, address, URN (Unique Reference Number) or LAESTAB (Local Authority Establishment Number)"

@Javascript:enabled
Scenario: Search Term validation errors should work with JS enabled
	Given establishment Some Primary School (111111) exists in local authority Oxfordshire (100)
	And local authority Oxfordshire (100) exists
	When I navigate to /my-schools/
	And I click the button "#searchSubmit"
	Then the path should be /my-schools/?search=
	And the element "#app-field-Search-input-error" should have the text content "Please enter a school name, address, URN (Unique Reference Number) or LAESTAB (Local Authority Establishment Number)"
	And the element "h2.govuk-error-summary__title" should have the text content "There is a problem"
	And the element "*[data-testid='app-error-summary-Search']" should have the text content "Please enter a school name, address, URN (Unique Reference Number) or LAESTAB (Local Authority Establishment Number)"

@Javascript:disabled
Scenario: School search page should show correct message for search term with no matches
	Given local authority Test LA (100) exists
	Given establishment Some Primary School (111111) exists in local authority 100
	When I navigate to /my-schools/
	And I update the textbox "#app-field-Search" to have the value "secondary"
	And I click the button "#searchSubmit"
	Then the path should be /my-schools/?search=secondary
	And the element "[data-testid="result-not-found-search-url"]" should have the href "/my-schools/"
	And the element "#app-page-title" should have the text content "We found no matches for "secondary""

@Javascript:disabled
Scenario: Pagination in Search Validation
	Given local authority Test LA (100) exists
	And 251 establishments exist with properties:
		| urn          | name                        | localAuthority    |
		| (100000 + n) | Primary School (100000 + n) | { "code": "100" } |
	When I navigate to /my-schools/?search=primary
	Then the pagination summary should be "Showing 1 - 50 of 251 schools"
	And the pagination links should be:
		| Link Text | Url                                |
		| 1         | /my-schools/?search=primary&page=1 |
		| 2         | /my-schools/?search=primary&page=2 |
		| ...       |                                    |
		| 6         | /my-schools/?search=primary&page=6 |
		| Next page | /my-schools/?search=primary&page=2 |
	And the listings should be:
		| Index | URN    | Name                  |
		| 1     | 100001 | Primary School 100001 |
		| 2     | 100002 | Primary School 100002 |
		| 3     | 100003 | Primary School 100003 |
		| 4     | 100004 | Primary School 100004 |
		| 5     | 100005 | Primary School 100005 |

@Javascript:disabled
Scenario: Pagination in Search Validation 2
	Given local authority Test LA (100) exists
	And 501 establishments exist with properties:
		| urn          | name                        | localAuthority    |
		| (100000 + n) | Primary School (100000 + n) | { "code": "100" } |
	When I navigate to /my-schools/?page=3&search=primary
	Then the pagination summary should be "Showing 101 - 150 of 501 schools"
	And the pagination links should be:
		| Link Text | Url                                 |
		| Prev page | /my-schools/?search=primary&page=2  |
		| 1         | /my-schools/?search=primary&page=1  |
		| 2         | /my-schools/?search=primary&page=2  |
		| 3         | /my-schools/?search=primary&page=3  |
		| 4         | /my-schools/?search=primary&page=4  |
		| ...       |                                     |
		| 11        | /my-schools/?search=primary&page=11 |
		| Next page | /my-schools/?search=primary&page=4  |
	And the listings should be:
		| Index | URN    | Name                  |
		| 1     | 100101 | Primary School 100101 |
		| 2     | 100102 | Primary School 100102 |
		| 3     | 100103 | Primary School 100103 |
		| 4     | 100104 | Primary School 100104 |
		| 5     | 100105 | Primary School 100105 |

@Javascript:disabled
Scenario: Pagination in Search Validation 3
	Given local authority Test LA (100) exists
	And 51 establishments exist with properties:
		| urn          | name                        | localAuthority    |
		| (100000 + n) | Primary School (100000 + n) | { "code": "100" } |
	When I navigate to /my-schools/?page=2&search=primary
	Then the pagination summary should be "Showing 51 - 51 of 51 schools"
	And the pagination links should be:
		| Link Text | Url                                |
		| Prev page | /my-schools/?search=primary&page=1 |
		| 1         | /my-schools/?search=primary&page=1 |
		| 2         | /my-schools/?search=primary&page=2 |
	And the listings should be:
		| Index | URN    | Name                  |
		| 1     | 100051 | Primary School 100051 |

@Javascript:disabled
Scenario: Matching URN search should redirect to school landing page
	Given local authority Test LA (100) exists
	Given establishment Some Primary School (111111) exists in local authority Test LA (100)
	When I navigate to /my-schools/
	And I update the textbox "#app-field-Search" to have the value "111111"
	And I click the button "#searchSubmit"
	Then the path should be /my-schools/111111/
	And the element "#app-page-subtitle span" should have the text content "(URN: 111111)"

@Javascript:enabled
Scenario: Matching URN search should redirect to school landing page with JS enabled
	Given local authority Test LA (100) exists
	Given establishment Some Primary School (111111) exists in local authority Test LA (100)
	When I navigate to /my-schools/
	And I update the textbox "#app-field-Search" to have the value "111111"
	And I click the button "#searchSubmit"
	Then the path should be /my-schools/111111/
	And the element "#app-page-subtitle span" should have the text content "(URN: 111111)"

@Javascript:disabled
Scenario: Partial match for school name should redirect to school landing page
	Given local authority Test LA (100) exists
	And establishment Some Primary School (111111) exists in local authority Test LA (100)
	When I navigate to /my-schools/
	And I update the textbox "#app-field-Search" to have the value "PRiMaRY"
	And I click the button "#searchSubmit"
	Then the path should be /my-schools/111111/
	And the element "#app-page-subtitle span" should have the text content "(URN: 111111)"

@Javascript:disabled
Scenario: Partial street match should redirect to school landing page
	Given local authority Test LA (100) exists
	Given establishment Some Primary School (111111) exists in local authority 100 with properties:
		"""
		{
			"address": {
				"street": "13 The Street",
				"town": "SomeTown",
				"postCode": "TR18 3JT"
			}
		} 
		"""
	When I navigate to /my-schools/
	And I update the textbox "#app-field-Search" to have the value "str"
	And I click the button "#searchSubmit"
	Then the path should be /my-schools/111111/
	And the element "[data-testid="school-details-address-value"]" should have the text content "13 The Street, SomeTown TR18 3JT"

@Javascript:disabled
Scenario: Partial town match should redirect to school landing page
	Given local authority Test LA (100) exists
	Given establishment Some Primary School (111111) exists in local authority 100 with properties:
		"""
		{
			"address": {
				"street": "13 The Street",
				"town": "SomeTown",
				"postCode": "TR18 3JT"
			}
		} 
		"""
	When I navigate to /my-schools/
	And I update the textbox "#app-field-Search" to have the value "some"
	And I click the button "#searchSubmit"
	Then the path should be /my-schools/111111/
	And the element "[data-testid="school-details-address-value"]" should have the text content "13 The Street, SomeTown TR18 3JT"

@Javascript:disabled
Scenario: Partial postcode match should redirect to school landing page
	Given local authority Test LA (100) exists
	Given establishment Some Primary School (111111) exists in local authority 100 with properties:
		"""
		{
			"address": {
				"street": "13 The Street",
				"town": "SomeTown",
				"postCode": "TR18 3JT"
			}
		} 
		"""
	When I navigate to /my-schools/
	And I update the textbox "#app-field-Search" to have the value "tr1"
	And I click the button "#searchSubmit"
	Then the path should be /my-schools/111111/
	And the element "[data-testid="school-details-address-value"]" should have the text content "13 The Street, SomeTown TR18 3JT"

@Javascript:disabled
Scenario Outline: Results page should show partial name and address matches
	Given local authority Test LA (100) exists
	Given establishment Some Primary School (111111) exists in local authority 100 with properties:
		"""
		{
			"address": {
				"street": "13 The Street",
				"town": "SomeTown",
				"postCode": "B1 1AA"
			}
		}
		"""
	And establishment Some Other Primary School (222222) exists in local authority 100 with properties:
		"""
		{
			"address": {
				"street": "13 The Road",
				"town": "Tring",
				"postCode": "B1 1AA"
			}
		}
		"""
	And establishment A Different Primary School (333333) exists in local authority 100 with properties:
		"""
		{
			"address": {
				"street": "13 The Road",
				"town": "SomeTown",
				"postCode": "TR18 3JT"
			}
		}
		"""
	And establishment The Training Centre (444444) exists in local authority Test LA (100)
	When I navigate to /my-schools/
	And I update the textbox "#app-field-Search" to have the value "tr"
	And I click the button "#searchSubmit"
	Then the path should be /my-schools/?search=tr
	And the listings should be:
		| Index | URN    | Name                       | Address                        | Url                 |
		| 1     | 333333 | A Different Primary School | 13 The Road, SomeTown TR18 3JT | /my-schools/333333/ |
		| 2     | 222222 | Some Other Primary School  | 13 The Road, Tring B1 1AA      | /my-schools/222222/ |
		| 3     | 111111 | Some Primary School        | 13 The Street, SomeTown B1 1AA | /my-schools/111111/ |
		| 4     | 444444 | The Training Centre        | Data not available             | /my-schools/444444/ |

@Javascript:disabled
Scenario: School search successful for 6-digit URN
	Given local authority Test LA (100) exists
	And establishment Some Primary School (111111) exists in local authority Test LA (100)
	When I navigate to /my-schools/
	And I update the textbox "#app-field-Search" to have the value "111111"
	And I click the button "#searchSubmit"
	Then the path should be /my-schools/111111/
	And the element "#app-page-subtitle span" should have the text content "(URN: 111111)"

@Javascript:disabled
Scenario Outline: School search with less than 6 digits does not match on URN
	Given local authority Test LA (100) exists
	Given establishment Some Primary School (111111) exists in local authority Test LA (100)
	When I navigate to /my-schools/
	And I update the textbox "#app-field-Search" to have the value "<SearchTerm>"
	And I click the button "#searchSubmit"
	Then the path should be /my-schools/?search=<SearchTerm>
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
	Given local authority Test LA (100) exists
	Given establishment Some Primary School (111111) exists in local authority Test LA (100)
	And establishment Another Primary School (222222) exists in local authority 100 with properties:
		"""
		{
			"address": {
				"street": "<SearchTerm> The Street",
				"town": "SomeTown",
				"postCode": "TR18 3JT"
			}
		}
		"""
	When I navigate to /my-schools/
	And I update the textbox "#app-field-Search" to have the value "<SearchTerm>"
	And I click the button "#searchSubmit"
	Then the path should be /my-schools/222222/
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
	Given local authority Test LA (100) exists
	And establishment Some Primary School (111111) exists in local authority Test LA (100)
	And establishment Another Primary School (222222) exists in local authority 100 with properties:
		"""
		{
			"address": {
				"street": "111111 The Street",
				"town": "SomeTown",
				"postCode": "TR18 3JT"
			}
		} 
		"""
	When I navigate to /my-schools/
	And I update the textbox "#app-field-Search" to have the value "111111"
	And I click the button "#searchSubmit"
	Then the path should be /my-schools/111111/
	And the element "#app-page-subtitle span" should have the text content "(URN: 111111)"

@Javascript:disabled
Scenario: Search term matching establishment LAESTAB code (with forward slash)
	Given local authority Test LA (100) exists
	And establishment Some Primary School (111111) exists in local authority 100 with properties:
		"""
		{
			"laestab": "894/2200"
		}
		"""
	When I navigate to /my-schools/
	And I update the textbox "#app-field-Search" to have the value "894/2200"
	And I click the button "#searchSubmit"
	Then the path should be /my-schools/111111/
	And the element "#app-page-subtitle span" should have the text content "(URN: 111111)"

@Javascript:disabled
Scenario: Search term matching establishment LAESTAB code (without forward slash)
	Given local authority Test LA (100) exists
	Given establishment Some Primary School (111111) exists in local authority 100 with properties:
		"""
		{
			"laestab": "894/2200"
		}
		"""
	When I navigate to /my-schools/
	And I update the textbox "#app-field-Search" to have the value "8942200"
	And I click the button "#searchSubmit"
	Then the path should be /my-schools/111111/
	And the element "#app-page-subtitle span" should have the text content "(URN: 111111)"

@Javascript:disabled
Scenario Outline: School results page shows multiple partial LAESTAB matches (LA part)
	Given local authority Test LA (100) exists
	Given establishment Some Primary School (111111) exists in local authority 100 with properties:
		"""
		{
			"laestab": "894/2200"
		}
		"""
	And establishment Some Other Primary School (222222) exists in local authority 100 with properties:
		"""
		{
			"laestab": "894/1234"
		}
		"""
	When I navigate to /my-schools/
	And I update the textbox "#app-field-Search" to have the value "894"
	And I click the button "#searchSubmit"
	Then the path should be /my-schools/?search=894
	And the listings should be:
		| Index | URN    | LAESTAB  | Name                      | Url                 |
		| 2     | 111111 | 894/2200 | Some Primary School       | /my-schools/111111/ |
		| 1     | 222222 | 894/1234 | Some Other Primary School | /my-schools/222222/ |

@Javascript:disabled
Scenario Outline: School results page shows multiple partial LAESTAB matches (ESTAB part)
	Given local authority Test LA (100) exists
	And establishment Some Primary School (111111) exists in local authority 100 with properties:
		"""
		{
			"laestab": "894/2200"
		}
		"""
	And establishment Some Other Primary School (222222) exists in local authority 100 with properties:
		"""
		{
			"laestab": "600/2200"
		}
		"""
	When I navigate to /my-schools/
	And I update the textbox "#app-field-Search" to have the value "2200"
	And I click the button "#searchSubmit"
	Then the path should be /my-schools/?search=2200
	And the listings should be:
		| Index | URN    | LAESTAB  | Name                      | Url                 |
		| 2     | 111111 | 894/2200 | Some Primary School       | /my-schools/111111/ |
		| 1     | 222222 | 600/2200 | Some Other Primary School | /my-schools/222222/ |

@Javascript:disabled
Scenario: Partial LAESTAB (LA part) match should show no matching results
	Given local authority Test LA (100) exists
	And establishment Some Primary School (111111) exists in local authority 100 with properties:
		"""
		{
			"laestab" : "894/2200"
		}
		"""
	When I navigate to /my-schools/
	And I update the textbox "#app-field-Search" to have the value "89"
	And I click the button "#searchSubmit"
	Then the path should be /my-schools/?search=89
	And the element "h1" should have the text content "We found no matches for "89""

@Javascript:disabled
Scenario: Partial LAESTAB (ESTAB only) match should show no matching results
	Given local authority Test LA (100) exists
	And establishment Some Primary School (111111) exists in local authority 100 with properties:
		"""
		{
			"laestab" : "894/2200"
		}
		"""
	When I navigate to /my-schools/
	And I update the textbox "#app-field-Search" to have the value "22"
	And I click the button "#searchSubmit"
	Then the path should be /my-schools/?search=22
	And the element "h1" should have the text content "We found no matches for "22""

@Javascript:disabled
Scenario: If searchTerm is a 7-digit number, treat it as an exact LAESTAB code search (ignoring other matching fields)
	Given local authority Test LA (100) exists
	Given establishment Some Primary School (111111) exists in local authority 100 with properties:
		"""
		{
			"laestab": "894/2200"
		}
		"""
	And establishment Another Primary School (222222) exists in local authority 100 with properties:
		"""
		{
			"laestab": "123/4567",
			"address": {
				"street": "8942200 The Street",
				"town": "SomeTown",
				"postCode": "TR18 3JT"
			}
		}
		"""
	When I navigate to /my-schools/
	And I update the textbox "#app-field-Search" to have the value "8942200"
	And I click the button "#searchSubmit"
	Then the path should be /my-schools/111111/
	And the element "#app-page-subtitle span" should have the text content "(URN: 111111)"

@Javascript:disabled
Scenario: If searchTerm is a 7-digit number with forward slash in the right place, treat it as an exact LAESTAB code search (ignoring other matching fields)
	Given local authority Test LA (100) exists
	And establishment Some Primary School (111111) exists in local authority 100 with properties:
		"""
		{
			"laestab": "894/2200"
		} 
		"""
	And establishment Another Primary School (222222) exists in local authority 100 with properties:
		"""
		{
			"laestab": "123/4567",
			"address": {
				"street": "894/2200 The Street",
				"town": "SomeTown",
				"postCode": "TR18 3JT"
			}
		} 
		"""
	When I navigate to /my-schools/
	And I update the textbox "#app-field-Search" to have the value "894/2200"
	And I click the button "#searchSubmit"
	Then the path should be /my-schools/111111/
	And the element "#app-page-subtitle span" should have the text content "(URN: 111111)"

@Javascript:disabled
Scenario: If searchTerm is a 3-digit number, treat it as an exact LA code search (ignoring other matching fields)
	Given local authority Test LA (100) exists
	And establishment Some Primary School (111111) exists in local authority 100 with properties:
		"""
		{
			"laestab": "894/2200"
		}
		"""
	And establishment Another Primary School (222222) exists in local authority 100 with properties:
		"""
		{
			"laestab": "123/4567",
			"address": {
				"street": "894 The Street",
				"town": "SomeTown",
				"postCode": "TR18 3JT"
			}
		} 
		"""
	When I navigate to /my-schools/
	And I update the textbox "#app-field-Search" to have the value "894"
	And I click the button "#searchSubmit"
	Then the path should be /my-schools/111111/
	And the element "#app-page-subtitle span" should have the text content "(URN: 111111)"

@Javascript:disabled
Scenario: if searchTerm is a 4-digit number, treat it as an exact ESTAB code search (ignoring other matching fields)
	Given local authority Test LA (100) exists
	Given establishment Some Primary School (111111) exists in local authority 100 with properties:
		"""
		{
			"laestab": "894/2200"
		}
		"""
	And establishment Another Primary School (222222) exists in local authority 100 with properties:
		"""
		{
			"laestab": "123/4567",
			"address": {
				"street": "2200 The Street",
				"town": "SomeTown",
				"postCode": "TR18 3JT"
			}
		} 
		"""
	When I navigate to /my-schools/
	And I update the textbox "#app-field-Search" to have the value "2200"
	And I click the button "#searchSubmit"
	Then the path should be /my-schools/111111/
	And the element "#app-page-subtitle span" should have the text content "(URN: 111111)"

@Javascript:disabled
Scenario Outline: Multiple successful school name matches show correct search results
	Given local authority Test LA (100) exists
	Given establishment School A (111111) exists in local authority 100 with properties:
		"""
		{
			"address": {
				"street": "13 The Street",
				"postCode": "AB12 3CD"
			}
		}
		"""
	And establishment School B (222222) exists in local authority 100 with properties:
		"""
		{
			"address": {
				"street": "2a Mornington Crescent",
				"town": "Liverpool",
				"postCode": "LL1 1AB"
			}
		}
		"""
	And establishment School C (333333) exists in local authority 100 with properties:
		"""
		{
			"address": {
				"street": "34 Long Road",
				"town": "Sheffield"
			}
		}
		"""
	When I navigate to /my-schools/
	And I update the textbox "#app-field-Search" to have the value "School"
	And I click the button "#searchSubmit"
	Then the path should be /my-schools/?search=School
	And the listings should be:
		| Index | URN    | Name     | Address                                   | Url                 |
		| 1     | 111111 | School A | 13 The Street AB12 3CD                    | /my-schools/111111/ |
		| 2     | 222222 | School B | 2a Mornington Crescent, Liverpool LL1 1AB | /my-schools/222222/ |
		| 3     | 333333 | School C | 34 Long Road, Sheffield                   | /my-schools/333333/ |

@Javascript:disabled
Scenario Outline: The PageNo parameter should handle invalid values with a default value of 1
	Given local authority Test LA (100) exists
	Given establishment Some Primary School (111111) exists in local authority 100 with properties:
		"""
		{
			"address": {
				"street": "13 The Street",
				"town": "SomeTown",
				"postCode": "B1 1AA"
			}
		}
		"""
	And establishment Some Other Primary School (222222) exists in local authority 100 with properties:
		"""
		{
			"address": {
				"street": "13 The Road",
				"town": "Tring",
				"postCode": "B1 1AA"
			}
		}
		"""
	When I navigate to /my-schools/?search=Primary&page=<page>
	Then the page title should be "Search results for "Primary""
	And the element "#app-page-subtitle" should have the text content "2 schools"
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
	Given local authority Test LA (100) exists
	And 26 establishments exist with properties:
		| urn          | name                        | localAuthority    |
		| (100000 + n) | Primary School (100000 + n) | { "code": "100" } |
	When I navigate to /my-schools/?page=50&search=Primary
	Then the page title should be "Search results for "Primary""
	And the pagination summary should be "Showing 1 - 26 of 26 schools"
	And the pagination links should be empty
	And the listings should be:
		| Index | URN    | Name                  |
		| 1     | 100001 | Primary School 100001 |
		| 26    | 100026 | Primary School 100026 |

@Javascript:enabled
Scenario: Autocomplete Should Populate Items When Two Or More Characters Entered
	Given local authority Test LA (100) exists
	And establishment Some Primary School (111111) exists in local authority 100 with properties:
		"""
		{
			"address": {
				"street": "13 The Street",
				"town": "SomeTown",
				"postCode": "B1 1AA"
			},
			"laestab": "894/2200"
		}
		"""
	And establishment Some Other Primary School (222222) exists in local authority 100 with properties:
		"""
		{
			"address": {
				"street": "13 The Road",
				"town": "Tring",
				"postCode": "B1 1AA"
			},
			"laestab": "894/2201"
		}
		"""
	And establishment A Different Primary School (333333) exists in local authority 100 with properties:
		"""
		{
			"address": {
				"street": "13 The Road",
				"town": "SomeTown",
				"postCode": "TR18 3JT"
			},
			"laestab": "894/2202"
		}
		"""
	And establishment Some Secondary School (444444) exists in local authority 100 with properties:
		"""
		{
			"address": {
				"street": "13 The Road",
				"town": "SomeTown",
				"postCode": "TR18 3JT"
			},
			"laestab": "894/2203"
		}
		"""
	When I navigate to /my-schools/
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
	Given local authority Test LA (100) exists
	And establishment Some Primary School (111111) exists in local authority 100 with properties:
		"""
		{
			"address": {
				"street": "13 The Street",
				"town": "SomeTown",
				"postCode": "B1 1AA"
			},
			"laestab": "894/2200"
		}
		"""
	And establishment Some Other Primary School (222222) exists in local authority 100 with properties:
		"""
		{
			"address": {
				"street": "13 The Road",
				"town": "Tring",
				"postCode": "B1 1AA"
			},
			"laestab": "894/2201"
		}
		"""
	And establishment A Different Primary School Centre (333333) exists in local authority 100 with properties:
		"""
		{
			"address": {
				"street": "13 The Road",
				"town": "SomeTown",
				"postCode": "TR18 3JT"
			},
			"laestab": "894/2202"
		}
		"""
	And establishment Some Secondary School (444444) exists in local authority 100 with properties:
		"""
		{
			"address": {
				"street": "13 The Road",
				"town": "SomeTown",
				"postCode": "TR18 3JT"
			},
			"laestab": "894/2203"
		}
		"""
	When I navigate to /my-schools/
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
	Given local authority Test LA (100) exists
	And establishment Some Primary School (111111) exists in local authority 100 with properties:
		"""
		{
			"address": {
				"street": "13 The Street",
				"town": "SomeTown",
				"postCode": "B1 1AA"
			},
			"laestab": "894/2200"
		}
		"""
	And establishment Some Other Primary School (222222) exists in local authority 100 with properties:
		"""
		{
			"address": {
				"street": "13 The Road",
				"town": "Tring",
				"postCode": "B1 1AA"
			},
			"laestab": "894/2201"
		}
		"""
	And establishment A Different Primary School Centre (333333) exists in local authority 100 with properties:
		"""
		{
			"address": {
				"street": "13 The Road",
				"town": "SomeTown",
				"postCode": "TR18 3JT"
			},
			"laestab": "894/2202"
		}
		"""
	And establishment Some Secondary School (444442) exists in local authority 100 with properties:
		"""
		{
			"address": {
				"street": "13 The Road",
				"town": "SomeTown",
				"postCode": "TR18 3JT"
			},
			"laestab": "894/2203"
		}
		"""
	When I navigate to /my-schools/
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


@Javascript:disabled
Scenario Outline: Should return (200) response if MAT Named, LA Named or Diocese Named user accesses /my-schools/123456/download-data
	Given I am a <userRole>
	Given establishment Test School (123456) exists with properties:
		"""
		{
			"localAuthority": {
				"code": "301",
			},
			"multiAcademyTrust": {
				"uid": 1234
		    },
		    "diocese": {
		        "name": "Test Diocese"
		    }
		}
		"""
	And local authority Test LA (301) exists
	And multi-academy trust Test MAT (1234) exists
	And blob storage file downloads-config.json exists in config container:
		"""
		[
		    {
		        "id": "kts-la-ks2-pupil",
		        "source": "KTS",
		        "scope": "LocalAuthority",
		        "dataSetType": "KeyStage2",
		        "label": "Key stage 2 (KS2)",
		        "filePathPattern": "LA/{code}/{year}/{filetype}/ks2_pupil_{version}.{filetype}"
		    },
		    {
		        "id": "kts-school-ks2-pupil",
		        "source": "KTS",
		        "scope": "School",
		        "dataSetType": "KeyStage2",
		        "label": "Key stage 2 (KS2)",
		        "filePathPattern": "School/{code}/{year}/{filetype}/ks2_pupil_{version}.{filetype}"
		    }
		]
		"""
	And blob storage file School/123456/2022/csv/ks2_pupil_final.csv exists in downloads-kts container:
		"""
		Column A,Column B,Column C
		1,2,3
		"""
	When I navigate to /my-schools/123456/download-data
	Then I should get a 200 response
Examples:
	| userRole                                    |
	| LA Named user for Local Authority 301       |
	| MAT Named user for Multi-Academy Trust 1234 |
	| Diocese Named user for Diocese Test Diocese |

@Javascript:disabled
Scenario Outline: Should return (403) response if the below mentioned user roles access /my-schools/123456/download-data
	Given I am a <userRole>
	Given establishment Test School (123456) exists with properties:
		"""
		{
			"localAuthority": {
				"code": "301",
			},
			"multiAcademyTrust": {
				"uid": 1234
		    },
		    "diocese": {
		        "name": "Test Diocese"
		    }
		}
		"""
	And local authority Test LA (301) exists
	And multi-academy trust Test MAT (1234) exists
	And blob storage file downloads-config.json exists in config container:
		"""
		[
		    {
		        "id": "kts-la-ks2-pupil",
		        "source": "KTS",
		        "scope": "LocalAuthority",
		        "dataSetType": "KeyStage2",
		        "label": "Key stage 2 (KS2)",
		        "filePathPattern": "LA/{code}/{year}/{filetype}/ks2_pupil_{version}.{filetype}"
		    },
		    {
		        "id": "kts-school-ks2-pupil",
		        "source": "KTS",
		        "scope": "School",
		        "dataSetType": "KeyStage2",
		        "label": "Key stage 2 (KS2)",
		        "filePathPattern": "School/{code}/{year}/{filetype}/ks2_pupil_{version}.{filetype}"
		    }
		]
		"""
	And blob storage file School/123456/2022/csv/ks2_pupil_final.csv exists in downloads-kts container:
		"""
		Column A,Column B,Column C
		1,2,3
		"""
	When I navigate to /my-schools/123456/download-data
	Then I should get a 403 response
Examples:
	| userRole                                       |
	| LA Unnamed user for Local Authority 301        |
	| MAT Unnamed user for Multi-Academy Trust 1234  |
	| MAT Governor user for Multi-Academy Trust 1234 |
	| Diocese Unnamed user for Diocese Test Diocese  |
	| School Named user for Establishment 123456     |
	| School Unnamed user for Establishment 123456   |
	| School Governor user for Establishment 123456  |
	| DfE Named user                                 |
	| DfE Unnamed user                               |
	| Ofsted Unnamed user                            |
	| Super Admin user                               |