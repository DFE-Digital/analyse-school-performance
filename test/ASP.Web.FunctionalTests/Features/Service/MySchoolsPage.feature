Feature: My schools page

Background:
	Given I am a LA Named user for Local Authority "100"

@Javascript:disabled
Scenario: School Named user is denied access to my-schools page
	Given I am a School Named user for Establishment "123456"
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
	Given I am a LA Named user for Local Authority "100"
	When I navigate to /my-schools/
	Then I should get a 500 response
	And the page title should be "Sorry, there is a problem with the service"

@Javascript:disabled
Scenario: No results for LA Named user when no schools in their Local Authority
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
	When I navigate to /my-schools/
	Then the page title should be "We found no schools"

@Javascript:disabled
Scenario: Correct results displayed for LA Named user with schools in their Local Authority
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
	When I navigate to /my-schools/
	Then the element "*[data-testid='NumberOfPages-Footer']" should have the text content "Showing 1 - 2 of 2 schools"
	And the element "[data-testid="school-listing-urn-<Counter>"]" should have the text content "<URN>"
	And the element "[data-testid="school-listing-name-<Counter>"]" should have the text content "<Name>"

Examples:
	| Counter | URN    | Name          |
	| 1       | 111111 | Test School 1 |
	| 2       | 222222 | Test School 2 |

@Javascript:disabled
Scenario: Server error when MAT Named user accesses /my-schools/ page and MAT doesn't exist
	Given I am a MAT Named user for Multi-Academy Trust "1234"
	When I navigate to /my-schools/
	Then I should get a 500 response
	And the page title should be "Sorry, there is a problem with the service"

@Javascript:disabled
Scenario: MAT Named user sees 'No schools found' message when MAT has no associated schools
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
	When I navigate to /my-schools/
	Then the page title should be "We found no schools"

@Javascript:disabled
Scenario: MAT Named user sees correct list of schools associated with their Multi-Academy Trust
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
	When I navigate to /my-schools/
	Then the element "*[data-testid='NumberOfPages-Footer']" should have the text content "Showing 1 - 2 of 2 schools"
	And the element "[data-testid="school-listing-urn-<Counter>"]" should have the text content "<URN>"
	And the element "[data-testid="school-listing-name-<Counter>"]" should have the text content "<Name>"

Examples:
	| Counter | URN    | Name          |
	| 1       | 111111 | Test School 1 |
	| 2       | 333333 | Test School 3 |

@Javascript:disabled
Scenario: Diocese Named user sees 'No schools found' message when no schools are associated with their diocese
	Given I am a Diocese Named user for Diocese "Test Diocese"
	And Establishment "111111" exists:
		"""
		    {
		    "name": "Test School 1",
		    "diocese": {
		        "code": "0000",
		        "name": "Not applicable",
		        "lname": "not applicable",
		        "isNullish": true
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
	When I navigate to /my-schools/
	Then the page title should be "We found no schools"

@Javascript:disabled
Scenario: Diocese Named user sees correct list of schools associated with their diocese
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
	When I navigate to /my-schools/
	Then the element "*[data-testid='NumberOfPages-Footer']" should have the text content "Showing 1 - 2 of 2 schools"
	And the element "[data-testid="school-listing-urn-<Counter>"]" should have the text content "<URN>"
	And the element "[data-testid="school-listing-name-<Counter>"]" should have the text content "<Name>"

Examples:
	| Counter | URN    | Name          |
	| 1       | 111111 | Test School 1 |
	| 2       | 333333 | Test School 3 |

@Javascript:disabled
Scenario: Pagination in my schools
	Given I am a MAT Named user for Multi-Academy Trust "1234"
	And Multi Academy Trust "1234" exists:
		"""
		    { 
		     "name": "Test MAT"
		    }
		"""
	And 251 Establishments exist with properties:
		| urn          | name                        | multiAcademyTrust |
		| (100000 + n) | Primary School (100000 + n) | { "uid": "1234" } |
	When I navigate to /my-schools/
	Then the element "*[data-testid='NumberOfPages-Footer']" should have the text content "Showing 1 - 50 of 251 schools"
	And the element "*[data-testid='PageLinks-Footer-1']" should have the href "/my-schools/?page=1"
	And the element "*[data-testid='PageLinks-Footer-2']" should have the href "/my-schools/?page=2"
	And the element "*[data-testid='govuk-pagination__link--Footer']" should have the text content "..."
	And the element "*[data-testid='PageLinks-Footer-6']" should have the href "/my-schools/?page=6"
	And the element "*[data-testid='PageLinks-Footer-Next']" should have the href "/my-schools/?page=2"
	And the element "*[data-testid='school-listing-name-1']" should have the text content "Primary School 100001"
	And the element "*[data-testid='school-listing-name-2']" should have the text content "Primary School 100002"
	And the element "*[data-testid='school-listing-name-3']" should have the text content "Primary School 100003"
	And the element "*[data-testid='school-listing-name-4']" should have the text content "Primary School 100004"
	And the element "*[data-testid='school-listing-name-5']" should have the text content "Primary School 100005"
	And the element "*[data-testid='school-listing-urn-1']" should have the text content "100001"
	And the element "*[data-testid='school-listing-urn-2']" should have the text content "100002"
	And the element "*[data-testid='school-listing-urn-3']" should have the text content "100003"
	And the element "*[data-testid='school-listing-urn-4']" should have the text content "100004"
	And the element "*[data-testid='school-listing-urn-5']" should have the text content "100005"

@Javascript:disabled
Scenario Outline: My schools page - common page elements
	Given 251 Establishments exist with properties:
		| urn          | name                        | multiAcademyTrust |
		| (100000 + n) | Primary School (100000 + n) | { "uid": 1234 }   |
	And Multi Academy Trust "1234" exists:
		"""
		    { 
		     "name": "Test MAT"
		    }
		"""
	And I am a MAT Named user for Multi-Academy Trust "1234"
	When I navigate to /my-schools/
	Then I should get a 200 response
	Then the page title should be "My schools"
	And the page subtitle should be "251 schools"
	And the breadcrumb trail should be:
		| text | href |
		| Home | /    |

@Javascript:disabled
Scenario: Page title should show correct text when search returns results
	Given Local Authority "100" exists:
		"""
		    { 
		    "name": "Test LA"
		    }
		"""
	And Establishment "111111" exists:
		"""
		{
			"name": "Some Primary School",
			"address": {
				"street": "13 The Street",
				"town": "SomeTown",
				"postCode": "B1 1AA"
			},
			"localAuthority": {
		      "code": "100"
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
		      "code": "100"
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
	Given Local Authority "001" exists:
		"""
		    { 
		    "name": "Test LA"
		    }
		"""
	Given I am a LA Named user for Local Authority "001"
	And Establishment "111111" exists:
		"""
		    {
		    "name": "Test School 1",
		    "localAuthority": {
				  "code": "002",
			  },
		    }
		"""
	And Establishment "222222" exists:
		"""
		    {
		    "name": "Test School 2",
		    "localAuthority": {
				  "code": "001",
			  },
		    }
		"""
	When I navigate to /my-schools/
	And I update the textbox "#app-field-Search" to have the value "111111"
	And I click the button "#searchSubmit"
	Then the path should be /my-schools/?search=111111
	Then the page title should be "We found no matches for "111111""


@Javascript:disabled
Scenario: Diocese user sees 'No school found' message when searching for school URNs that are not associated with their Diocese
	Given I am a Diocese Named user for Diocese "Test Diocese"
	And Establishment "111111" exists:
		"""
		    {
		    "name": "Test School 1",
		    "diocese": {
				   "name": "Test Diocese 1"
			   }
		    }
		"""
	And Establishment "222222" exists:
		"""
		    {
		    "name": "Test School",
		    "diocese": {
				   "name": "Test Diocese"
			   }
		    }
		"""
	When I navigate to /my-schools/
	And I update the textbox "#app-field-Search" to have the value "111111"
	And I click the button "#searchSubmit"
	Then the path should be /my-schools/?search=111111
	Then the page title should be "We found no matches for "111111""

@Javascript:disabled
Scenario: MAT user sees 'No school found' message when searching for school URNs that are not associated with their MAT
	And Multi Academy Trust "1111" exists:
		"""
		{
			"multiAcademyTrust": {
			    "uid": 1111
			}
		}
		"""
	Given I am a MAT Named user for Multi-Academy Trust "1111"
	And Establishment "111111" exists:
		"""
		    {
		    "name": "Test School 1",
		    "multiAcademyTrust": {
					"uid": 2222
				}
		    }
		"""
	And Establishment "222222" exists:
		"""
		    {
		    "name": "Test School",
			"multiAcademyTrust": {
					"uid": 1111
				}
		    }
		"""
	When I navigate to /my-schools/
	And I update the textbox "#app-field-Search" to have the value "111111"
	And I click the button "#searchSubmit"
	Then the path should be /my-schools/?search=111111
	Then the page title should be "We found no matches for "111111""

@Javascript:enabled
Scenario: Page title should show correct text when search returns results (JS)
	Given Local Authority "100" exists:
		"""
		    { 
		    "name": "Test LA"
		    }
		"""
	And Establishment "111111" exists:
		"""
		{
			"name": "Some Primary School",
			"address": {
				"street": "13 The Street",
				"town": "SomeTown",
				"postCode": "B1 1AA"
			},
			"localAuthority": {
		      "code": "100"
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
		      "code": "100"
		    }
		}
		"""
	When I navigate to /my-schools/
	And I update the textbox "#app-field-Search" to have the value "Primary"
	And I click the button "#searchSubmit"
	Then the path should be /my-schools/?search=Primary
	And the page title should be "Search results for "Primary""

@Javascript:disabled
Scenario: Page should show a breadcrumb trail when search returns results
	Given Local Authority "100" exists:
		"""
		    { 
		    "name": "Test LA"
		    }
		"""
	And Establishment "111111" exists:
		"""
		{
			"name": "Some Primary School",
			"address": {
				"street": "13 The Street",
				"town": "SomeTown",
				"postCode": "B1 1AA"
			},
		    "localAuthority": {
		   		"code": "100"
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
		   		"code": "100"
		 	}
		}
		"""
	When I navigate to /my-schools/
	And I update the textbox "#app-field-Search" to have the value "Primary"
	And I click the button "#searchSubmit"
	Then the path should be /my-schools/?search=Primary
	And the breadcrumb trail should be:
		| text       | href         |
		| Home       | /            |
		| My schools | /my-schools/ |

@Javascript:enabled
Scenario: Page should show a breadcrumb trail when search returns results (JS)
	Given Local Authority "100" exists:
		"""
		    { 
		    "name": "Test LA"
		    }
		"""
	And Establishment "111111" exists:
		"""
		{
			"name": "Some Primary School",
			"address": {
				"street": "13 The Street",
				"town": "SomeTown",
				"postCode": "B1 1AA"
			},
		    "localAuthority": {
		   		"code": "100"
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
		   		"code": "100"
		 	}
		}
		"""
	When I navigate to /my-schools/
	And I update the textbox "#app-field-Search" to have the value "Primary"
	And I click the button "#searchSubmit"
	Then the path should be /my-schools/?search=Primary
	And the breadcrumb trail should be:
		| text       | href         |
		| Home       | /            |
		| My schools | /my-schools/ |

@Javascript:disabled
Scenario: Page should show a breadcrumb trail when search returns no results
	Given Local Authority "100" exists:
		"""
		    { 
		    "name": "Test LA"
		    }
		"""
	And Establishment "111111" exists:
		"""
		{
			"name": "Some Primary School",
			"address": {
				"street": "13 The Street",
				"town": "SomeTown",
				"postCode": "B1 1AA"
			},
		 	"localAuthority": {
				"code": "100"
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
				"code": "100"
			}
		}
		"""
	When I navigate to /my-schools/
	And I update the textbox "#app-field-Search" to have the value "Secondary"
	And I click the button "#searchSubmit"
	Then the path should be /my-schools/?search=Secondary
	And the breadcrumb trail should be:
		| text       | href         |
		| Home       | /            |
		| My schools | /my-schools/ |

@Javascript:enabled
Scenario: Page should show a breadcrumb trail when search returns no results (JS)
	Given Local Authority "100" exists:
		"""
		    { 
		    "name": "Test LA"
		    }
		"""
	And Establishment "111111" exists:
		"""
		{
			"name": "Some Primary School",
			"address": {
				"street": "13 The Street",
				"town": "SomeTown",
				"postCode": "B1 1AA"
			},
		 	"localAuthority": {
				"code": "100"
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
				"code": "100"
			}
		}
		"""
	When I navigate to /my-schools/
	And I update the textbox "#app-field-Search" to have the value "Secondary"
	And I click the button "#searchSubmit"
	Then the path should be /my-schools/?search=Secondary
	And the breadcrumb trail should be:
		| text       | href         |
		| Home       | /            |
		| My schools | /my-schools/ |

@Javascript:disabled
Scenario: Search Term Validation
	Given Establishment "111111" exists:
		"""
		{
			"name": "Some Primary School",
			"localAuthority": {
				"code": "100",
				"name": "Oxfordshire"
			}
		}
		"""
	And Local Authority "100" exists:
		"""
		{
			"Name": "Oxfordshire",
			"Code": "100"
		}
		"""
	When I navigate to /my-schools/
	Then I should get a 200 response
	And the page title should be "My schools"
	And the element "#searchForm" should have the text content "Enter school name, address, URN (Unique Reference Number) or LAESTAB (Local Authority Establishment Number) Search"

@Javascript:enabled
Scenario: Search Term Validation (JS)
	Given Establishment "111111" exists:
		"""
		{
			"name": "Some Primary School",
			"localAuthority": {
				"code": "100",
				"name": "Oxfordshire"
			}
		}
		"""
	And Local Authority "100" exists:
		"""
		{
			"Name": "Oxfordshire",
			"Code": "100"
		}
		"""
	When I navigate to /my-schools/
	Then I should get a 200 response
	And the page title should be "My schools"
	And the element "#searchForm" should have the text content "Enter school name, address, URN (Unique Reference Number) or LAESTAB (Local Authority Establishment Number) When autocomplete results are available use up and down arrows to review and enter to select. Touch device users, explore by touch or with swipe gestures. Search"

@Javascript:disabled
Scenario: Errors in Search Term Validation
	Given Establishment "111111" exists:
		"""
		{
			"name": "Some Primary School",
			"localAuthority": {
				"code": "100",
				"name": "Oxfordshire"
			}
		}
		"""
	And Local Authority "100" exists:
		"""
		{
			"Name": "Oxfordshire",
			"Code": "100"
		}
		"""
	When I navigate to /my-schools/
	And I click the button "#searchSubmit"
	Then the path should be /my-schools/?search=
	And the element "#app-field-Search-input-error" should have the text content "Please enter a school name, address, URN (Unique Reference Number) or LAESTAB (Local Authority Establishment Number)"
	And the element "h2.govuk-error-summary__title" should have the text content "There is a problem"
	And the element "*[data-testid='app-error-summary-Search']" should have the text content "Please enter a school name, address, URN (Unique Reference Number) or LAESTAB (Local Authority Establishment Number)"

@Javascript:enabled
Scenario: Errors in Search Term Validation (JS)
	Given Establishment "111111" exists:
		"""
		{
			"name": "Some Primary School",
			"localAuthority": {
				"code": "100",
				"name": "Oxfordshire"
			}
		}
		"""
	And Local Authority "100" exists:
		"""
		{
			"Name": "Oxfordshire",
			"Code": "100"
		}
		"""
	When I navigate to /my-schools/
	And I click the button "#searchSubmit"
	Then the path should be /my-schools/?search=
	And the element "#app-field-Search-input-error" should have the text content "Please enter a school name, address, URN (Unique Reference Number) or LAESTAB (Local Authority Establishment Number)"
	And the element "h2.govuk-error-summary__title" should have the text content "There is a problem"
	And the element "*[data-testid='app-error-summary-Search']" should have the text content "Please enter a school name, address, URN (Unique Reference Number) or LAESTAB (Local Authority Establishment Number)"

@Javascript:disabled
Scenario: School search page should show correct message for search term with no matches
	Given Local Authority "100" exists:
		"""
		    { 
		    "name": "Test LA"
		    }
		"""
	Given Establishment "111111" exists:
		"""
		{
			"name": "Some Primary School",
		     "localAuthority": {
				"code": "100"
			 }
		}
		"""
	When I navigate to /my-schools/
	And I update the textbox "#app-field-Search" to have the value "secondary"
	And I click the button "#searchSubmit"
	Then the path should be /my-schools/?search=secondary
	And the element "[data-testid="result-not-found-search-url"]" should have the href "/my-schools/"
	And the element "#app-page-title" should have the text content "We found no matches for "secondary""

@Javascript:enabled
Scenario: School search page should show correct message for search term with no matches (JS)
	Given Local Authority "100" exists:
		"""
		    { 
		    "name": "Test LA"
		    }
		"""
	Given Establishment "111111" exists:
		"""
		{
			"name": "Some Primary School",
		     "localAuthority": {
				"code": "100"
			 }
		}
		"""
	When I navigate to /my-schools/
	And I update the textbox "#app-field-Search" to have the value "secondary"
	And I click the button "#searchSubmit"
	Then the path should be /my-schools/?search=secondary
	And the element "[data-testid="result-not-found-search-url"]" should have the href "/my-schools/"
	And the element "#app-page-title" should have the text content "We found no matches for "secondary""

@Javascript:disabled
Scenario: Pagination in Search Validation
	Given Local Authority "100" exists:
		"""
		    { 
		    "name": "Test LA"
		    }
		"""
	And 251 Establishments exist with properties:
		| urn          | name                        | localAuthority    |
		| (100000 + n) | Primary School (100000 + n) | { "code": "100" } |
	When I navigate to /my-schools/?search=primary
	Then the element "*[data-testid='NumberOfPages-Footer']" should have the text content "Showing 1 - 50 of 251 schools"
	And the element "*[data-testid='PageLinks-Footer-1']" should have the href "/my-schools/?search=primary&page=1"
	And the element "*[data-testid='PageLinks-Footer-2']" should have the href "/my-schools/?search=primary&page=2"
	And the element "*[data-testid='govuk-pagination__link--Footer']" should have the text content "..."
	And the element "*[data-testid='PageLinks-Footer-6']" should have the href "/my-schools/?search=primary&page=6"
	And the element "*[data-testid='PageLinks-Footer-Next']" should have the href "/my-schools/?search=primary&page=2"
	And the element "*[data-testid='school-listing-name-1']" should have the text content "Primary School 100001"
	And the element "*[data-testid='school-listing-name-2']" should have the text content "Primary School 100002"
	And the element "*[data-testid='school-listing-name-3']" should have the text content "Primary School 100003"
	And the element "*[data-testid='school-listing-name-4']" should have the text content "Primary School 100004"
	And the element "*[data-testid='school-listing-name-5']" should have the text content "Primary School 100005"
	And the element "*[data-testid='school-listing-urn-1']" should have the text content "100001"
	And the element "*[data-testid='school-listing-urn-2']" should have the text content "100002"
	And the element "*[data-testid='school-listing-urn-3']" should have the text content "100003"
	And the element "*[data-testid='school-listing-urn-4']" should have the text content "100004"
	And the element "*[data-testid='school-listing-urn-5']" should have the text content "100005"

@Javascript:enabled
Scenario: Pagination in Search Validation (JS)
	Given Local Authority "100" exists:
		"""
		    { 
		    "name": "Test LA"
		    }
		"""
	And 251 Establishments exist with properties:
		| urn          | name                        | localAuthority    |
		| (100000 + n) | Primary School (100000 + n) | { "code": "100" } |
	When I navigate to /my-schools/?search=primary
	Then the element "*[data-testid='NumberOfPages-Footer']" should have the text content "Showing 1 - 50 of 251 schools"
	And the element "*[data-testid='PageLinks-Footer-1']" should have the href "/my-schools/?search=primary&page=1"
	And the element "*[data-testid='PageLinks-Footer-2']" should have the href "/my-schools/?search=primary&page=2"
	And the element "*[data-testid='govuk-pagination__link--Footer']" should have the text content "..."
	And the element "*[data-testid='PageLinks-Footer-6']" should have the href "/my-schools/?search=primary&page=6"
	And the element "*[data-testid='PageLinks-Footer-Next']" should have the href "/my-schools/?search=primary&page=2"
	And the element "*[data-testid='school-listing-name-1']" should have the text content "Primary School 100001"
	And the element "*[data-testid='school-listing-name-2']" should have the text content "Primary School 100002"
	And the element "*[data-testid='school-listing-name-3']" should have the text content "Primary School 100003"
	And the element "*[data-testid='school-listing-name-4']" should have the text content "Primary School 100004"
	And the element "*[data-testid='school-listing-name-5']" should have the text content "Primary School 100005"
	And the element "*[data-testid='school-listing-urn-1']" should have the text content "100001"
	And the element "*[data-testid='school-listing-urn-2']" should have the text content "100002"
	And the element "*[data-testid='school-listing-urn-3']" should have the text content "100003"
	And the element "*[data-testid='school-listing-urn-4']" should have the text content "100004"
	And the element "*[data-testid='school-listing-urn-5']" should have the text content "100005"

@Javascript:disabled
Scenario: Pagination in Search Validation 2
	Given Local Authority "100" exists:
		"""
		    { 
		    "name": "Test LA"
		    }
		"""
	And 501 Establishments exist with properties:
		| urn          | name                        | localAuthority    |
		| (100000 + n) | Primary School (100000 + n) | { "code": "100" } |
	When I navigate to /my-schools/?page=3&search=primary
	Then the element "*[data-testid='NumberOfPages-Footer']" should have the text content "Showing 101 - 150 of 501 schools"
	And the elements "*[data-testid='PageLinks-Footer-Prev']" should all have the href "/my-schools/?search=primary&page=2"
	And the elements "*[data-testid='PageLinks-Footer-1']" should all have the href "/my-schools/?search=primary&page=1"
	And the elements "*[data-testid='PageLinks-Footer-2']" should all have the href "/my-schools/?search=primary&page=2"
	And the elements "*[data-testid='PageLinks-Footer-3']" should all have the href "/my-schools/?search=primary&page=3"
	And the elements "*[data-testid='PageLinks-Footer-4']" should all have the href "/my-schools/?search=primary&page=4"
	And the element "*[data-testid='govuk-pagination__link--Footer']" should have the text content "..."
	And the elements "*[data-testid='PageLinks-Footer-Next']" should all have the href "/my-schools/?search=primary&page=4"
	And the element "*[data-testid='school-listing-name-1']" should have the text content "Primary School 100101"
	And the element "*[data-testid='school-listing-name-2']" should have the text content "Primary School 100102"
	And the element "*[data-testid='school-listing-name-3']" should have the text content "Primary School 100103"
	And the element "*[data-testid='school-listing-name-4']" should have the text content "Primary School 100104"
	And the element "*[data-testid='school-listing-name-5']" should have the text content "Primary School 100105"
	And the element "*[data-testid='school-listing-urn-1']" should have the text content "100101"
	And the element "*[data-testid='school-listing-urn-2']" should have the text content "100102"
	And the element "*[data-testid='school-listing-urn-3']" should have the text content "100103"
	And the element "*[data-testid='school-listing-urn-4']" should have the text content "100104"
	And the element "*[data-testid='school-listing-urn-5']" should have the text content "100105"

@Javascript:enabled
Scenario: Pagination in Search Validation 2 (JS)
	Given Local Authority "100" exists:
		"""
		    { 
		    "name": "Test LA"
		    }
		"""
	And 501 Establishments exist with properties:
		| urn          | name                        | localAuthority    |
		| (100000 + n) | Primary School (100000 + n) | { "code": "100" } |
	When I navigate to /my-schools/?page=3&search=primary
	Then the element "*[data-testid='NumberOfPages-Footer']" should have the text content "Showing 101 - 150 of 501 schools"
	And the elements "*[data-testid='PageLinks-Footer-Prev']" should all have the href "/my-schools/?search=primary&page=2"
	And the elements "*[data-testid='PageLinks-Footer-1']" should all have the href "/my-schools/?search=primary&page=1"
	And the elements "*[data-testid='PageLinks-Footer-2']" should all have the href "/my-schools/?search=primary&page=2"
	And the elements "*[data-testid='PageLinks-Footer-3']" should all have the href "/my-schools/?search=primary&page=3"
	And the elements "*[data-testid='PageLinks-Footer-4']" should all have the href "/my-schools/?search=primary&page=4"
	And the element "*[data-testid='govuk-pagination__link--Footer']" should have the text content "..."
	And the elements "*[data-testid='PageLinks-Footer-Next']" should all have the href "/my-schools/?search=primary&page=4"
	And the element "*[data-testid='school-listing-name-1']" should have the text content "Primary School 100101"
	And the element "*[data-testid='school-listing-name-2']" should have the text content "Primary School 100102"
	And the element "*[data-testid='school-listing-name-3']" should have the text content "Primary School 100103"
	And the element "*[data-testid='school-listing-name-4']" should have the text content "Primary School 100104"
	And the element "*[data-testid='school-listing-name-5']" should have the text content "Primary School 100105"
	And the element "*[data-testid='school-listing-urn-1']" should have the text content "100101"
	And the element "*[data-testid='school-listing-urn-2']" should have the text content "100102"
	And the element "*[data-testid='school-listing-urn-3']" should have the text content "100103"
	And the element "*[data-testid='school-listing-urn-4']" should have the text content "100104"
	And the element "*[data-testid='school-listing-urn-5']" should have the text content "100105"

@Javascript:disabled
Scenario: Pagination in Search Validation 3
	Given Local Authority "100" exists:
		"""
		    { 
		    "name": "Test LA"
		    }
		"""
	And 51 Establishments exist with properties:
		| urn          | name                        | localAuthority    |
		| (100000 + n) | Primary School (100000 + n) | { "code": "100" } |
	When I navigate to /my-schools/?page=2&search=primary
	Then the element "*[data-testid='NumberOfPages-Footer']" should have the text content "Showing 51 - 51 of 51 schools"
	And the elements "*[data-testid='PageLinks-Footer-Prev']" should all have the href "/my-schools/?search=primary&page=1"
	And the elements "*[data-testid='PageLinks-Footer-1']" should all have the href "/my-schools/?search=primary&page=1"
	And the element "*[data-testid='school-listing-name-1']" should have the text content "Primary School 100051"
	And the element "*[data-testid='school-listing-urn-1']" should have the text content "100051"

@Javascript:enabled
Scenario: Pagination in Search Validation 3 (JS)
	Given Local Authority "100" exists:
		"""
		    { 
		    "name": "Test LA"
		    }
		"""
	And 51 Establishments exist with properties:
		| urn          | name                        | localAuthority    |
		| (100000 + n) | Primary School (100000 + n) | { "code": "100" } |
	When I navigate to /my-schools/?page=2&search=primary
	Then the element "*[data-testid='NumberOfPages-Footer']" should have the text content "Showing 51 - 51 of 51 schools"
	And the elements "*[data-testid='PageLinks-Footer-Prev']" should all have the href "/my-schools/?search=primary&page=1"
	And the elements "*[data-testid='PageLinks-Footer-1']" should all have the href "/my-schools/?search=primary&page=1"
	And the element "*[data-testid='school-listing-name-1']" should have the text content "Primary School 100051"
	And the element "*[data-testid='school-listing-urn-1']" should have the text content "100051"

@Javascript:disabled
Scenario: Matching URN search should redirect to school landing page
	Given Local Authority "100" exists:
		"""
		    { 
		    "name": "Test LA"
		    }
		"""
	Given Establishment "111111" exists:
		"""
		{
			"name": "Some Primary School",
		    "localAuthority": {
				   "code": "100",
		       	   "name": "Test LA"
			}
		}
		"""
	When I navigate to /my-schools/
	And I update the textbox "#app-field-Search" to have the value "111111"
	And I click the button "#searchSubmit"
	Then the path should be /my-schools/111111/
	And the element "#app-page-subtitle span" should have the text content "(URN: 111111)"

@Javascript:enabled
Scenario: Matching URN search should redirect to school landing page (JS)
	Given Local Authority "100" exists:
		"""
		    { 
		    "name": "Test LA"
		    }
		"""
	Given Establishment "111111" exists:
		"""
		{
			"name": "Some Primary School",
		    "localAuthority": {
				   "code": "100",
		       	   "name": "Test LA"
			}
		}
		"""
	When I navigate to /my-schools/
	And I update the textbox "#app-field-Search" to have the value "111111"
	And I click the button "#searchSubmit"
	Then the path should be /my-schools/111111/
	And the element "#app-page-subtitle span" should have the text content "(URN: 111111)"

@Javascript:disabled
Scenario: Partial match for school name should redirect to school landing page
	Given Local Authority "100" exists:
		"""
		    { 
		    "name": "Test LA"
		    }
		"""
	And Establishment "111111" exists:
		"""
		{
			"name": "Some Primary School",
		    "localAuthority": {
				"code": "100",
		       	"name": "Test LA"
			}
		}
		"""
	When I navigate to /my-schools/
	And I update the textbox "#app-field-Search" to have the value "PRiMaRY"
	And I click the button "#searchSubmit"
	Then the path should be /my-schools/111111/
	And the element "#app-page-subtitle span" should have the text content "(URN: 111111)"

@Javascript:enabled
Scenario: Partial match for school name should redirect to school landing page (JS)
	Given Local Authority "100" exists:
		"""
		    { 
		    "name": "Test LA"
		    }
		"""
	And Establishment "111111" exists:
		"""
		{
			"name": "Some Primary School",
		    "localAuthority": {
				"code": "100",
		       	"name": "Test LA"
			}
		}
		"""
	When I navigate to /my-schools/
	And I update the textbox "#app-field-Search" to have the value "PRiMaRY"
	And I click the button "#searchSubmit"
	Then the path should be /my-schools/111111/
	And the element "#app-page-subtitle span" should have the text content "(URN: 111111)"

@Javascript:disabled
Scenario: Partial street match should redirect to school landing page
	Given Local Authority "100" exists:
		"""
		    { 
		    "name": "Test LA"
		    }
		"""
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
				"code": "100",
		       	"name": "Test LA"
			}
		} 
		"""
	When I navigate to /my-schools/
	And I update the textbox "#app-field-Search" to have the value "str"
	And I click the button "#searchSubmit"
	Then the path should be /my-schools/111111/
	And the element "[data-testid="school-details-address-value"]" should have the text content "13 The Street, SomeTown TR18 3JT"

@Javascript:enabled
Scenario: Partial street match should redirect to school landing page (JS)
	Given Local Authority "100" exists:
		"""
		    { 
		    "name": "Test LA"
		    }
		"""
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
				"code": "100",
		       	"name": "Test LA"
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
	Given Local Authority "100" exists:
		"""
		    { 
		    "name": "Test LA"
		    }
		"""
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
				"code": "100",
		       	"name": "Test LA"
			}
		} 
		"""
	When I navigate to /my-schools/
	And I update the textbox "#app-field-Search" to have the value "some"
	And I click the button "#searchSubmit"
	Then the path should be /my-schools/111111/
	And the element "[data-testid="school-details-address-value"]" should have the text content "13 The Street, SomeTown TR18 3JT"

@Javascript:enabled
Scenario: Partial town match should redirect to school landing page (JS)
	Given Local Authority "100" exists:
		"""
		    { 
		    "name": "Test LA"
		    }
		"""
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
				"code": "100",
		       	"name": "Test LA"
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
	Given Local Authority "100" exists:
		"""
		    { 
		    "name": "Test LA"
		    }
		"""
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
				"code": "100",
		       	"name": "Test LA"
			}
		} 
		"""
	When I navigate to /my-schools/
	And I update the textbox "#app-field-Search" to have the value "tr1"
	And I click the button "#searchSubmit"
	Then the path should be /my-schools/111111/
	And the element "[data-testid="school-details-address-value"]" should have the text content "13 The Street, SomeTown TR18 3JT"

@Javascript:enabled
Scenario: Partial postcode match should redirect to school landing page (JS)
	Given Local Authority "100" exists:
		"""
		    { 
		    "name": "Test LA"
		    }
		"""
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
				"code": "100",
		       	"name": "Test LA"
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
	Given Local Authority "100" exists:
		"""
		    { 
		    "name": "Test LA"
		    }
		"""
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
				"code": "100",
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
				"code": "100",
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
				"code": "100",
		    	"name": "Test LA"
			}
		}
		"""
	And Establishment "444444" exists:
		"""
		{
			"name": "The Training Centre",
		 	"localAuthority": {
				"code": "100",
		    	"name": "Test LA"
			}
		}
		"""
	When I navigate to /my-schools/
	And I update the textbox "#app-field-Search" to have the value "tr"
	And I click the button "#searchSubmit"
	Then the path should be /my-schools/?search=tr
	And the element "[data-testid="school-listing-urn-<Counter>"]" should have the text content "<URN>"
	And the element "[data-testid="school-listing-name-<Counter>"]" should have the text content "<Name>"
	And the element "[data-testid="school-listing-address-<Counter>"]" should have the text content "<Address>"

Examples:
	| Counter | URN    | Name                       | Address                        | Href                |
	| 1       | 333333 | A Different Primary School | 13 The Road, SomeTown TR18 3JT | /my-schools/333333/ |
	| 2       | 222222 | Some Other Primary School  | 13 The Road, Tring B1 1AA      | /my-schools/222222/ |
	| 3       | 111111 | Some Primary School        | 13 The Street, SomeTown B1 1AA | /my-schools/111111/ |
	| 4       | 444444 | The Training Centre        | Data not available           | /my-schools/444444/ |

@Javascript:enabled
Scenario Outline: Results page should show partial name and address matches (JS)
	Given Local Authority "100" exists:
		"""
		    { 
		    "name": "Test LA"
		    }
		"""
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
				"code": "100",
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
				"code": "100",
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
				"code": "100",
		    	"name": "Test LA"
			}
		}
		"""
	And Establishment "444444" exists:
		"""
		{
			"name": "The Training Centre",
		 	"localAuthority": {
				"code": "100",
		    	"name": "Test LA"
			}
		}
		"""
	When I navigate to /my-schools/
	And I update the textbox "#app-field-Search" to have the value "tr"
	And I click the button "#searchSubmit"
	Then the path should be /my-schools/?search=tr
	And the element "[data-testid="school-listing-urn-<Counter>"]" should have the text content "<URN>"
	And the element "[data-testid="school-listing-name-<Counter>"]" should have the text content "<Name>"
	And the element "[data-testid="school-listing-address-<Counter>"]" should have the text content "<Address>"

Examples:
	| Counter | URN    | Name                       | Address                        | Href                |
	| 1       | 333333 | A Different Primary School | 13 The Road, SomeTown TR18 3JT | /my-schools/333333/ |
	| 2       | 222222 | Some Other Primary School  | 13 The Road, Tring B1 1AA      | /my-schools/222222/ |
	| 3       | 111111 | Some Primary School        | 13 The Street, SomeTown B1 1AA | /my-schools/111111/ |
	| 4       | 444444 | The Training Centre        | Data not available           | /my-schools/444444/ |

@Javascript:disabled
Scenario: School search successful for 6-digit URN
	Given Local Authority "100" exists:
		"""
		    { 
		    "name": "Test LA"
		    }
		"""
	And Establishment "111111" exists:
		"""
		{
			"name": "Some Primary School",
		    "localAuthority": {
				"code": "100",
		       	"name": "Test LA"
			}
		}
		"""
	When I navigate to /my-schools/
	And I update the textbox "#app-field-Search" to have the value "111111"
	And I click the button "#searchSubmit"
	Then the path should be /my-schools/111111/
	And the element "#app-page-subtitle span" should have the text content "(URN: 111111)"

@Javascript:enabled
Scenario: School search successful for 6-digit URN (JS)
	Given Local Authority "100" exists:
		"""
		    { 
		    "name": "Test LA"
		    }
		"""
	And Establishment "111111" exists:
		"""
		{
			"name": "Some Primary School",
		    "localAuthority": {
				"code": "100",
		       	"name": "Test LA"
			}
		}
		"""
	When I navigate to /my-schools/
	And I update the textbox "#app-field-Search" to have the value "111111"
	And I click the button "#searchSubmit"
	Then the path should be /my-schools/111111/
	And the element "#app-page-subtitle span" should have the text content "(URN: 111111)"

@Javascript:disabled
Scenario Outline: School search with less than 6 digits does not match on URN
	Given Local Authority "100" exists:
		"""
		    { 
		    "name": "Test LA"
		    }
		"""
	Given Establishment "111111" exists:
		"""
		{
			"name": "Some Primary School",
		 	"localAuthority": {
				"code": "100",
		    	"name": "Test LA"
			}
		}
		"""
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

@Javascript:enabled
Scenario Outline: School search with less than 6 digits does not match on URN (JS)
	Given Local Authority "100" exists:
		"""
		    { 
		    "name": "Test LA"
		    }
		"""
	Given Establishment "111111" exists:
		"""
		{
			"name": "Some Primary School",
		 	"localAuthority": {
				"code": "100",
		    	"name": "Test LA"
			}
		}
		"""
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
	Given Local Authority "100" exists:
		"""
		    { 
		    "name": "Test LA"
		    }
		"""
	Given Establishment "111111" exists:
		"""
		{
			"name": "Some Primary School",
		    "localAuthority": {
				"code": "100",
		       	"name": "Test LA"
			}
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
				"code": "100",
		       	"name": "Test LA"
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

@Javascript:enabled
Scenario Outline: School search with less than 6 digits matches on school address (JS)
	Given Local Authority "100" exists:
		"""
		    { 
		    "name": "Test LA"
		    }
		"""
	Given Establishment "111111" exists:
		"""
		{
			"name": "Some Primary School",
		    "localAuthority": {
				"code": "100",
		       	"name": "Test LA"
			}
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
				"code": "100",
		       	"name": "Test LA"
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
	Given Local Authority "100" exists:
		"""
		    { 
		    "name": "Test LA"
		    }
		"""
	And Establishment "111111" exists:
		"""
		{
			"name": "Some Primary School",
		    "localAuthority": {
				"code": "100",
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
			},
		    "localAuthority": {
				"code": "100",
		       	"name": "Test LA"
			}
		} 
		"""
	When I navigate to /my-schools/
	And I update the textbox "#app-field-Search" to have the value "111111"
	And I click the button "#searchSubmit"
	Then the path should be /my-schools/111111/
	And the element "#app-page-subtitle span" should have the text content "(URN: 111111)"

@Javascript:enabled
Scenario: If searchTerm is a 6-digit number, treat it as an exact URN search (JS)
	Given Local Authority "100" exists:
		"""
		    { 
		    "name": "Test LA"
		    }
		"""
	And Establishment "111111" exists:
		"""
		{
			"name": "Some Primary School",
		    "localAuthority": {
				"code": "100",
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
			},
		    "localAuthority": {
				"code": "100",
		       	"name": "Test LA"
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
	Given Local Authority "100" exists:
		"""
		    { 
		    "name": "Test LA"
		    }
		"""
	And Establishment "111111" exists:
		"""
		{
			"name": "Some Primary School",
			"laestab": "894/2200",
		    "localAuthority": {
				"code": "100",
		       	"name": "Test LA"
			}
		}
		"""
	When I navigate to /my-schools/
	And I update the textbox "#app-field-Search" to have the value "894/2200"
	And I click the button "#searchSubmit"
	Then the path should be /my-schools/111111/
	And the element "#app-page-subtitle span" should have the text content "(URN: 111111)"

@Javascript:enabled
Scenario: Search term matching establishment LAESTAB code (with forward slash) (JS)
	Given Local Authority "100" exists:
		"""
		    { 
		    "name": "Test LA"
		    }
		"""
	And Establishment "111111" exists:
		"""
		{
			"name": "Some Primary School",
			"laestab": "894/2200",
		    "localAuthority": {
				"code": "100",
		       	"name": "Test LA"
			}
		}
		"""
	When I navigate to /my-schools/
	And I update the textbox "#app-field-Search" to have the value "894/2200"
	And I click the button "#searchSubmit"
	Then the path should be /my-schools/111111/
	And the element "#app-page-subtitle span" should have the text content "(URN: 111111)"

@Javascript:disabled
Scenario: Search term matching establishment LAESTAB code (without forward slash)
	Given Local Authority "100" exists:
		"""
		    { 
		    "name": "Test LA"
		    }
		"""
	Given Establishment "111111" exists:
		"""
		{
			"name": "Some Primary School",
			"laestab": "894/2200",
		    "localAuthority": {
				"code": "100",
		       	"name": "Test LA"
			}
		}
		"""
	When I navigate to /my-schools/
	And I update the textbox "#app-field-Search" to have the value "8942200"
	And I click the button "#searchSubmit"
	Then the path should be /my-schools/111111/
	And the element "#app-page-subtitle span" should have the text content "(URN: 111111)"

@Javascript:enabled
Scenario: Search term matching establishment LAESTAB code (without forward slash) (JS)
	Given Local Authority "100" exists:
		"""
		    { 
		    "name": "Test LA"
		    }
		"""
	Given Establishment "111111" exists:
		"""
		{
			"name": "Some Primary School",
			"laestab": "894/2200",
		    "localAuthority": {
				"code": "100",
		       	"name": "Test LA"
			}
		}
		"""
	When I navigate to /my-schools/
	And I update the textbox "#app-field-Search" to have the value "8942200"
	And I click the button "#searchSubmit"
	Then the path should be /my-schools/111111/
	And the element "#app-page-subtitle span" should have the text content "(URN: 111111)"

@Javascript:disabled
Scenario Outline: School results page shows multiple partial LAESTAB matches (LA part)
	Given Local Authority "100" exists:
		"""
		    { 
		    "name": "Test LA"
		    }
		"""
	Given Establishment "111111" exists:
		"""
		{
			"name": "Some Primary School",
			"laestab": "894/2200",
		 	"localAuthority": {
				"code": "100",
		    	"name": "Test LA"
			}
		}
		"""
	And Establishment "222222" exists:
		"""
		{
			"name": "Some Other Primary School",
			"laestab": "894/1234",
		 	"localAuthority": {
				"code": "100",
		    	"name": "Test LA"
			}
		}
		"""
	When I navigate to /my-schools/
	And I update the textbox "#app-field-Search" to have the value "894"
	And I click the button "#searchSubmit"
	Then the path should be /my-schools/?search=894
	And the element "[data-testid="school-listing-urn-<Counter>"]" should have the text content "<URN>"
	And the element "[data-testid="school-listing-name-<Counter>"]" should have the text content "<Name>"
	And the element "[data-testid="school-listing-laestab-<Counter>"]" should have the text content "<LAESTAB>"

Examples:
	| Counter | URN    | LAESTAB  | Name                      | Href                |
	| 2       | 111111 | 894/2200 | Some Primary School       | /my-schools/111111/ |
	| 1       | 222222 | 894/1234 | Some Other Primary School | /my-schools/222222/ |

@Javascript:enabled
Scenario Outline: School results page shows multiple partial LAESTAB matches (LA part) (JS)
	Given Local Authority "100" exists:
		"""
		    { 
		    "name": "Test LA"
		    }
		"""
	Given Establishment "111111" exists:
		"""
		{
			"name": "Some Primary School",
			"laestab": "894/2200",
		 	"localAuthority": {
				"code": "100",
		    	"name": "Test LA"
			}
		}
		"""
	And Establishment "222222" exists:
		"""
		{
			"name": "Some Other Primary School",
			"laestab": "894/1234",
		 	"localAuthority": {
				"code": "100",
		    	"name": "Test LA"
			}
		}
		"""
	When I navigate to /my-schools/
	And I update the textbox "#app-field-Search" to have the value "894"
	And I click the button "#searchSubmit"
	Then the path should be /my-schools/?search=894
	And the element "[data-testid="school-listing-urn-<Counter>"]" should have the text content "<URN>"
	And the element "[data-testid="school-listing-name-<Counter>"]" should have the text content "<Name>"
	And the element "[data-testid="school-listing-laestab-<Counter>"]" should have the text content "<LAESTAB>"

Examples:
	| Counter | URN    | LAESTAB  | Name                      | Href                |
	| 2       | 111111 | 894/2200 | Some Primary School       | /my-schools/111111/ |
	| 1       | 222222 | 894/1234 | Some Other Primary School | /my-schools/222222/ |

@Javascript:disabled
Scenario Outline: School results page shows multiple partial LAESTAB matches (ESTAB part)
	Given Local Authority "100" exists:
		"""
		    { 
		    "name": "Test LA"
		    }
		"""
	And Establishment "111111" exists:
		"""
		{
			"name": "Some Primary School",
			"laestab": "894/2200",
			"localAuthority": {
				"code": "100",
			 	"name": "Test LA"
			}
		}
		"""
	And Establishment "222222" exists:
		"""
		{
			"name": "Some Other Primary School",
			"laestab": "600/2200",
			"localAuthority": {
				"code": "100",
		 		"name": "Test LA"
			}
		}
		"""
	When I navigate to /my-schools/
	And I update the textbox "#app-field-Search" to have the value "2200"
	And I click the button "#searchSubmit"
	Then the path should be /my-schools/?search=2200
	And the element "[data-testid="school-listing-urn-<Counter>"]" should have the text content "<URN>"
	And the element "[data-testid="school-listing-name-<Counter>"]" should have the text content "<Name>"
	And the element "[data-testid="school-listing-laestab-<Counter>"]" should have the text content "<LAESTAB>"
	And the element "[data-testid="school-listing-name-<Counter>"]" should have the href "<Href>"

Examples:
	| Counter | URN    | LAESTAB  | Name                      | Href                |
	| 2       | 111111 | 894/2200 | Some Primary School       | /my-schools/111111/ |
	| 1       | 222222 | 600/2200 | Some Other Primary School | /my-schools/222222/ |

@Javascript:enabled
Scenario Outline: School results page shows multiple partial LAESTAB matches (ESTAB part) (JS)
	Given Local Authority "100" exists:
		"""
		    { 
		    "name": "Test LA"
		    }
		"""
	And Establishment "111111" exists:
		"""
		{
			"name": "Some Primary School",
			"laestab": "894/2200",
			"localAuthority": {
				"code": "100",
			 	"name": "Test LA"
			}
		}
		"""
	And Establishment "222222" exists:
		"""
		{
			"name": "Some Other Primary School",
			"laestab": "600/2200",
			"localAuthority": {
				"code": "100",
		 		"name": "Test LA"
			}
		}
		"""
	When I navigate to /my-schools/
	And I update the textbox "#app-field-Search" to have the value "2200"
	And I click the button "#searchSubmit"
	Then the path should be /my-schools/?search=2200
	And the element "[data-testid="school-listing-urn-<Counter>"]" should have the text content "<URN>"
	And the element "[data-testid="school-listing-name-<Counter>"]" should have the text content "<Name>"
	And the element "[data-testid="school-listing-laestab-<Counter>"]" should have the text content "<LAESTAB>"
	And the element "[data-testid="school-listing-name-<Counter>"]" should have the href "<Href>"

Examples:
	| Counter | URN    | LAESTAB  | Name                      | Href                |
	| 2       | 111111 | 894/2200 | Some Primary School       | /my-schools/111111/ |
	| 1       | 222222 | 600/2200 | Some Other Primary School | /my-schools/222222/ |

@Javascript:disabled
Scenario: Partial LAESTAB (LA part) match should show no matching results
	Given Local Authority "100" exists:
		"""
		    { 
		    "name": "Test LA"
		    }
		"""
	And Establishment "111111" exists:
		"""
		{
			"name": "Some Primary School",
			"laestab" : "894/2200",
			"localAuthority": {
				"code": "100",
				"name": "Test LA"
			}
		}
		"""
	When I navigate to /my-schools/
	And I update the textbox "#app-field-Search" to have the value "89"
	And I click the button "#searchSubmit"
	Then the path should be /my-schools/?search=89
	And the element "h1" should have the text content "We found no matches for "89""

@Javascript:enabled
Scenario: Partial LAESTAB (LA part) match should show no matching results (JS)
	Given Local Authority "100" exists:
		"""
		    { 
		    "name": "Test LA"
		    }
		"""
	And Establishment "111111" exists:
		"""
		{
			"name": "Some Primary School",
			"laestab" : "894/2200",
			"localAuthority": {
				"code": "100",
				"name": "Test LA"
			}
		}
		"""
	When I navigate to /my-schools/
	And I update the textbox "#app-field-Search" to have the value "89"
	And I click the button "#searchSubmit"
	Then the path should be /my-schools/?search=89
	And the element "h1" should have the text content "We found no matches for "89""

@Javascript:disabled
Scenario: Partial LAESTAB (ESTAB only) match should show no matching results
	Given Local Authority "100" exists:
		"""
		    { 
		    "name": "Test LA"
		    }
		"""
	And Establishment "111111" exists:
		"""
		{
			"name": "Some Primary School",
			"laestab" : "894/2200",
			"localAuthority": {
				"code": "100",
				"name": "Test LA"
			}
		}
		"""
	When I navigate to /my-schools/
	And I update the textbox "#app-field-Search" to have the value "22"
	And I click the button "#searchSubmit"
	Then the path should be /my-schools/?search=22
	And the element "h1" should have the text content "We found no matches for "22""

@Javascript:enabled
Scenario: Partial LAESTAB (ESTAB only) match should show no matching results (JS)
	Given Local Authority "100" exists:
		"""
		    { 
		    "name": "Test LA"
		    }
		"""
	And Establishment "111111" exists:
		"""
		{
			"name": "Some Primary School",
			"laestab" : "894/2200",
			"localAuthority": {
				"code": "100",
				"name": "Test LA"
			}
		}
		"""
	When I navigate to /my-schools/
	And I update the textbox "#app-field-Search" to have the value "22"
	And I click the button "#searchSubmit"
	Then the path should be /my-schools/?search=22
	And the element "h1" should have the text content "We found no matches for "22""

@Javascript:disabled
Scenario: If searchTerm is a 7-digit number, treat it as an exact LAESTAB code search (ignoring other matching fields)
	Given Local Authority "100" exists:
		"""
		    { 
		    "name": "Test LA"
		    }
		"""
	Given Establishment "111111" exists:
		"""
		{
			"name": "Some Primary School",
			"laestab": "894/2200",
		    "localAuthority": {
				"code": "100",
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
			},
			"localAuthority": {
				"code": "100",
				"name": "Test LA"
			}
		} 
		"""
	When I navigate to /my-schools/
	And I update the textbox "#app-field-Search" to have the value "8942200"
	And I click the button "#searchSubmit"
	Then the path should be /my-schools/111111/
	And the element "#app-page-subtitle span" should have the text content "(URN: 111111)"

@Javascript:enabled
Scenario: If searchTerm is a 7-digit number, treat it as an exact LAESTAB code search (ignoring other matching fields) (JS)
	Given Local Authority "100" exists:
		"""
		    { 
		    "name": "Test LA"
		    }
		"""
	Given Establishment "111111" exists:
		"""
		{
			"name": "Some Primary School",
			"laestab": "894/2200",
		    "localAuthority": {
				"code": "100",
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
			},
			"localAuthority": {
				"code": "100",
				"name": "Test LA"
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
	Given Local Authority "100" exists:
		"""
		    { 
		    "name": "Test LA"
		    }
		"""
	And Establishment "111111" exists:
		"""
		{
			"name": "Some Primary School",
			"laestab": "894/2200",
		    "localAuthority": {
				"code": "100",
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
			},
			"localAuthority": {
				"code": "100",
				"name": "Test LA"
			}
		} 
		"""
	When I navigate to /my-schools/
	And I update the textbox "#app-field-Search" to have the value "894/2200"
	And I click the button "#searchSubmit"
	Then the path should be /my-schools/111111/
	And the element "#app-page-subtitle span" should have the text content "(URN: 111111)"

@Javascript:enabled
Scenario: If searchTerm is a 7-digit number with forward slash in the right place, treat it as an exact LAESTAB code search (ignoring other matching fields) (JS)
	Given Local Authority "100" exists:
		"""
		    { 
		    "name": "Test LA"
		    }
		"""
	And Establishment "111111" exists:
		"""
		{
			"name": "Some Primary School",
			"laestab": "894/2200",
		    "localAuthority": {
				"code": "100",
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
			},
			"localAuthority": {
				"code": "100",
				"name": "Test LA"
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
	Given Local Authority "100" exists:
		"""
		    { 
		    "name": "Test LA"
		    }
		"""
	And Establishment "111111" exists:
		"""
		{
			"name": "Some Primary School",
			"laestab": "894/2200",
		    "localAuthority": {
				"code": "100",
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
			},
			"localAuthority": {
				"code": "100",
				"name": "Test LA"
			}
		} 
		"""
	When I navigate to /my-schools/
	And I update the textbox "#app-field-Search" to have the value "894"
	And I click the button "#searchSubmit"
	Then the path should be /my-schools/111111/
	And the element "#app-page-subtitle span" should have the text content "(URN: 111111)"

@Javascript:enabled
Scenario: If searchTerm is a 3-digit number, treat it as an exact LA code search (ignoring other matching fields) (JS)
	Given Local Authority "100" exists:
		"""
		    { 
		    "name": "Test LA"
		    }
		"""
	And Establishment "111111" exists:
		"""
		{
			"name": "Some Primary School",
			"laestab": "894/2200",
		    "localAuthority": {
				"code": "100",
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
			},
			"localAuthority": {
				"code": "100",
				"name": "Test LA"
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
	Given Local Authority "100" exists:
		"""
		    { 
		    "name": "Test LA"
		    }
		"""
	Given Establishment "111111" exists:
		"""
		{
			"name": "Some Primary School",
			"laestab": "894/2200",
		    "localAuthority": {
				"code": "100",
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
			},
			"localAuthority": {
				"code": "100",
				"name": "Test LA"
			}
		} 
		"""
	When I navigate to /my-schools/
	And I update the textbox "#app-field-Search" to have the value "2200"
	And I click the button "#searchSubmit"
	Then the path should be /my-schools/111111/
	And the element "#app-page-subtitle span" should have the text content "(URN: 111111)"

@Javascript:enabled
Scenario: if searchTerm is a 4-digit number, treat it as an exact ESTAB code search (ignoring other matching fields) (JS)
	Given Local Authority "100" exists:
		"""
		    { 
		    "name": "Test LA"
		    }
		"""
	Given Establishment "111111" exists:
		"""
		{
			"name": "Some Primary School",
			"laestab": "894/2200",
		    "localAuthority": {
				"code": "100",
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
			},
			"localAuthority": {
				"code": "100",
				"name": "Test LA"
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
	Given Local Authority "100" exists:
		"""
		    { 
		    "name": "Test LA"
		    }
		"""
	Given Establishment "111111" exists:
		"""
		{
			"name": "School A",
			"address": {
				"street": "13 The Street",
				"postCode": "AB12 3CD"
			},
			"localAuthority": {
				"code": "100",
				"name": "Test LA"
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
			},
			"localAuthority": {
				"code": "100",
				"name": "Test LA"
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
			},
			"localAuthority": {
				"code": "100",
				"name": "Test LA"
			}
		}
		"""
	When I navigate to /my-schools/
	And I update the textbox "#app-field-Search" to have the value "School"
	And I click the button "#searchSubmit"
	Then the path should be /my-schools/?search=School
	And the element "[data-testid="school-listing-urn-<Counter>"]" should have the text content "<URN>"
	And the element "[data-testid="school-listing-name-<Counter>"]" should have the text content "<Name>"
	And the element "[data-testid="school-listing-address-<Counter>"]" should have the text content "<Address>"
	And the element "[data-testid="school-listing-name-<Counter>"]" should have the href "<Href>"
	    
Examples:
	| Counter | URN    | Name     | Address                                   | Href                |
	| 1       | 111111 | School A | 13 The Street AB12 3CD                    | /my-schools/111111/ |
	| 2       | 222222 | School B | 2a Mornington Crescent, Liverpool LL1 1AB | /my-schools/222222/ |
	| 3       | 333333 | School C | 34 Long Road, Sheffield                   | /my-schools/333333/ |

@Javascript:enabled
Scenario Outline: Multiple successful school name matches show correct search results (JS)
	Given Local Authority "100" exists:
		"""
		    { 
		    "name": "Test LA"
		    }
		"""
	Given Establishment "111111" exists:
		"""
		{
			"name": "School A",
			"address": {
				"street": "13 The Street",
				"postCode": "AB12 3CD"
			},
			"localAuthority": {
				"code": "100",
				"name": "Test LA"
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
			},
			"localAuthority": {
				"code": "100",
				"name": "Test LA"
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
			},
			"localAuthority": {
				"code": "100",
				"name": "Test LA"
			}
		}
		"""
	When I navigate to /my-schools/
	And I update the textbox "#app-field-Search" to have the value "School"
	And I click the button "#searchSubmit"
	Then the path should be /my-schools/?search=School
	And the element "[data-testid="school-listing-urn-<Counter>"]" should have the text content "<URN>"
	And the element "[data-testid="school-listing-name-<Counter>"]" should have the text content "<Name>"
	And the element "[data-testid="school-listing-address-<Counter>"]" should have the text content "<Address>"
	And the element "[data-testid="school-listing-name-<Counter>"]" should have the href "<Href>"
	    
Examples:
	| Counter | URN    | Name     | Address                                   | Href                |
	| 1       | 111111 | School A | 13 The Street AB12 3CD                    | /my-schools/111111/ |
	| 2       | 222222 | School B | 2a Mornington Crescent, Liverpool LL1 1AB | /my-schools/222222/ |
	| 3       | 333333 | School C | 34 Long Road, Sheffield                   | /my-schools/333333/ |

@Javascript:disabled
Scenario Outline: The PageNo parameter should handle invalid values with a default value of 1
	Given Local Authority "100" exists:
		"""
		    { 
		    "name": "Test LA"
		    }
		"""
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
				"code": "100",
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
				"code": "100",
				"name": "Test LA"
			}
		}
		"""
	When I navigate to /my-schools/?search=Primary&page=<page>
	Then the page title should be "Search results for "Primary""
	And the element "#app-page-subtitle" should have the text content "2 schools"
	And the element "*[data-testid='NumberOfPages-Footer']" should have the text content "Showing 1 - 2 of 2 schools"
	And the elements "*[data-testid='PageLinks-Footer-1']" should all have the href "/my-schools/?search=Primary&page=1"

Examples:
          | page |
          | y    |
          | 1.5  |
          | 0    |
          | -1   |

@Javascript:disabled
Scenario: The PageNo parameter number greater than the total number of pages, the last page of results should be shown
	Given Local Authority "100" exists:
		"""
		    { 
		    "name": "Test LA"
		    }
		"""
	And 26 Establishments exist with properties:
		| urn          | name                        | localAuthority    |
		| (100000 + n) | Primary School (100000 + n) | { "code": "100" } |
	When I navigate to /my-schools/?page=50&search=Primary
	Then the page title should be "Search results for "Primary""
	And the element "*[data-testid='NumberOfPages-Footer']" should have the text content "Showing 1 - 26 of 26 schools"
	And the element "#app-page-subtitle" should have the text content "26 schools"
	And the elements "*[data-testid='PageLinks-Footer-1']" should all have the href "/my-schools/?search=Primary&page=1"
	And the element "*[data-testid='school-listing-name-1']" should have the text content "Primary School 100001"
	And the element "*[data-testid='school-listing-name-26']" should have the text content "Primary School 100026"

@Javascript:enabled
Scenario: Autocomplete Should Populate Items When Two Or More Characters Entered
	Given Local Authority "100" exists:
		"""
		    { 
		    "name": "Test LA"
		    }
		"""
	And Establishment "111111" exists:
		"""
		{
			"name": "Some Primary School",
			"address": {
				"street": "13 The Street",
				"town": "SomeTown",
				"postCode": "B1 1AA"
			},
			"localAuthority": {
		      	"code": "100",
		      	"name": "Test LA"
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
			"localAuthority": {
		      	"code": "100",
		      	"name": "Test LA"
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
			"localAuthority": {
		      	"code": "100",
		      	"name": "Test LA"
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
			"localAuthority": {
		      	"code": "100",
		      	"name": "Test LA"
		    },
			"laestab": "894/2203"
		}
		"""
	When I navigate to /my-schools/
	And I update the textbox "#app-field-Search" to have the value "primary"
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
	Given Local Authority "100" exists:
		"""
		    { 
		    "name": "Test LA"
		    }
		"""
	And Establishment "111111" exists:
		"""
		{
			"name": "Some Primary School",
			"address": {
				"street": "13 The Street",
				"town": "SomeTown",
				"postCode": "B1 1AA"
			},
			"localAuthority": {
		      	"code": "100",
		      	"name": "Test LA"
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
			"localAuthority": {
		      	"code": "100",
		      	"name": "Test LA"
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
			"localAuthority": {
		      	"code": "100",
		      	"name": "Test LA"
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
			"localAuthority": {
		      	"code": "100",
		      	"name": "Test LA"
		    },
			"laestab": "894/2203"
		}
		"""
	When I navigate to /my-schools/
	And I update the textbox "#app-field-Search" to have the value "tr"
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
	Given Local Authority "100" exists:
		"""
		    { 
		    "name": "Test LA"
		    }
		"""
	And Establishment "111111" exists:
		"""
		{
			"name": "Some Primary School",
			"address": {
				"street": "13 The Street",
				"town": "SomeTown",
				"postCode": "B1 1AA"
			},
			"laestab": "894/2200",
			"localAuthority": {
		      	"code": "100",
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
			"laestab": "894/2201",
			"localAuthority": {
		      	"code": "100",
		      	"name": "Test LA"
		    }
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
			"laestab": "894/2202",
			"localAuthority": {
		      	"code": "100",
		      	"name": "Test LA"
		    }
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
			"laestab": "894/2203",
			"localAuthority": {
		      	"code": "100",
		      	"name": "Test LA"
		    }
		}
		"""
	When I navigate to /my-schools/
	And I update the textbox "#app-field-Search" to have the value "42"
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
Scenario Outline: Should return (200) response if MAT Named, LA Named or Diocese Named user accesses /my-schools/123456/download-data
	Given I am a <userRole>
	Given Establishment "123456" exists:
		"""
		{
			"name": "Test School",
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
	And Local Authority "301" exists:
		"""
		{
		    "name": "Test LA"
		}
		"""
	And Multi Academy Trust "1234" exists:
		"""
		{
		    "name": "Test MAT"
		}
		"""
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
	| userRole                                      |
	| LA Named user for Local Authority "301"       |
	| MAT Named user for Multi-Academy Trust "1234" |
	| Diocese Named user for Diocese "Test Diocese" |

@Javascript:disabled
Scenario Outline: Should return (403) response if the below mentioned user roles access /my-schools/123456/download-data
	Given I am a <userRole>
	Given Establishment "123456" exists:
		"""
		{
			"name": "Test School",
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
	And Local Authority "301" exists:
		"""
		{
		    "name": "Test LA"
		}
		"""
	And Multi Academy Trust "1234" exists:
		"""
		{
		    "name": "Test MAT"
		}
		"""
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
	| userRole                                         |
	| LA Unnamed user for Local Authority "301"        |
	| MAT Unnamed user for Multi-Academy Trust "1234"  |
	| MAT Governor user for Multi-Academy Trust "1234" |
	| Diocese Unnamed user for Diocese "Test Diocese"  |
	| School Named user for Establishment "123456"     |
	| School Unnamed user for Establishment "123456"   |
	| School Governor user for Establishment "123456"  |
	| DfE Named user                                   |
	| DfE Unnamed user                                 |
	| Ofsted Unnamed user                              |
	| Super Admin user                                 |