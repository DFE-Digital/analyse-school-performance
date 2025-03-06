Feature: All local authorities page
Background:
	Given I am a DfE Named user

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
Scenario: No results when no schools exist
	Given no Establishments exist
	When I navigate to /local-authorities/
	Then the page title should be "We found no local authorities"

@Javascript:disabled
Scenario: Page should show a breadcrumb trail
	Given I am a DfE Named user
	And Local Authority "301" exists:
		"""
		{
			"Name": "Test Name"
		}
		"""
	When I navigate to /local-authorities/
	Then I should get a 200 response
	And the page title should be "All local authorities"
	And the breadcrumb trail should be:
		| Link Text | Url |
		| Home      | /   |

@Javascript:disabled
Scenario: DfE Named user should see Generic Local Authorities page
	Given Local Authority "301" exists:
		"""
		{
			"name": "Some Test Name"
		}
		"""
	And Local Authority "302" exists:
		"""
		{
			"name": "Some Other Test Name"
		}
		"""

	And Local Authority "303" exists:
		"""
		{
			"name": "A Different Test Name"
		}
		"""

	And Local Authority "304" exists:
		"""
		{
			"name": "The Training Centre"
		}
		"""
	When I navigate to /local-authorities/
	Then the pagination summary should be "Showing 1 - 4 of 4 local authorities"
	And the listings should be:
		| Index | Code | Name                  |
		| 1     | 303  | A Different Test Name |
		| 2     | 302  | Some Other Test Name  |
		| 3     | 301  | Some Test Name        |
		| 4     | 304  | The Training Centre   |

@Javascript:disabled
Scenario: Pagination in Generic Local Authorities page
	Given 251 Local Authorities exist with properties:
		| code      | name                        |
		| (100 + n) | ASP Test LA Named (100 + n) |
	When I navigate to /local-authorities/
	Then the pagination summary should be "Showing 1 - 50 of 251 local authorities"
	And the pagination links should be:
		| Link Text | Url                        |
		| 1         | /local-authorities/?page=1 |
		| 2         | /local-authorities/?page=2 |
		| ...       |                            |
		| 6         | /local-authorities/?page=6 |
		| Next page | /local-authorities/?page=2 |
	And the listings should be:
		| Index | Name                  | Url                   | Code |
		| 1     | ASP Test LA Named 101 | /local-authority/101/ | 101  |
		| 2     | ASP Test LA Named 102 | /local-authority/102/ | 102  |
		| 3     | ASP Test LA Named 103 | /local-authority/103/ | 103  |
		| 4     | ASP Test LA Named 104 | /local-authority/104/ | 104  |
		| 5     | ASP Test LA Named 105 | /local-authority/105/ | 105  |

@Javascript:disabled
Scenario: Pagination in Generic Local Authorities Validation 2
	Given 501 Local Authorities exist with properties:
		| code      | name                        |
		| (100 + n) | ASP Test LA Named (100 + n) |
	When I navigate to /local-authorities/?page=3
	Then the pagination summary should be "Showing 101 - 150 of 501 local authorities"
	And the pagination links should be:
		| Link Text | Url                         |
		| Prev page | /local-authorities/?page=2  |
		| 1         | /local-authorities/?page=1  |
		| 2         | /local-authorities/?page=2  |
		| 3         | /local-authorities/?page=3  |
		| 4         | /local-authorities/?page=4  |
		| ...       |                             |
		| 11        | /local-authorities/?page=11 |
		| Next page | /local-authorities/?page=4  |
	And the listings should be:
		| Index | Name                  | Url                   | Code |
		| 1     | ASP Test LA Named 201 | /local-authority/201/ | 201  |
		| 2     | ASP Test LA Named 202 | /local-authority/202/ | 202  |
		| 3     | ASP Test LA Named 203 | /local-authority/203/ | 203  |
		| 4     | ASP Test LA Named 204 | /local-authority/204/ | 204  |
		| 5     | ASP Test LA Named 205 | /local-authority/205/ | 205  |

@Javascript:disabled
Scenario: Page title should show correct text when search returns results
	Given Local Authority "301" exists:
		"""
		{
			"name": "Some Test Name"
		}
		"""
	And Local Authority "302" exists:
		"""
		{
			"name": "Some Other Test Name"
		}
		"""
	When I navigate to /local-authorities/
	And I update the textbox "#app-field-Search" to have the value "Test"
	And I click the button "#searchSubmit"
	Then the path should be /local-authorities/?search=Test
	And the page title should be "Search results for "Test""

@Javascript:disabled
Scenario: Page should show a breadcrumb trail when search returns results
	Given Local Authority "301" exists:
		"""
		{
			"name": "Some Test Name"
		}
		"""
	And Local Authority "302" exists:
		"""
		{
			"name": "Some Other Test Name"
		}
		"""
	When I navigate to /local-authorities/
	And I update the textbox "#app-field-Search" to have the value "Test"
	And I click the button "#searchSubmit"
	Then the path should be /local-authorities/?search=Test
	And the breadcrumb trail should be:
		| Link Text             | Url                 |
		| Home                  | /                   |
		| All local authorities | /local-authorities/ |

@Javascript:disabled
Scenario: Page should show a breadcrumb trail when search returns no results
	Given Local Authority "301" exists:
		"""
		{
			"name": "Some Test Name"
		}
		"""
	And Local Authority "302" exists:
		"""
		{
			"name": "Some Other Test Name"
		}
		"""
	When I navigate to /local-authorities/
	And I update the textbox "#app-field-Search" to have the value "Test"
	And I click the button "#searchSubmit"
	Then the path should be /local-authorities/?search=Test
	And the breadcrumb trail should be:
		| Link Text             | Url                 |
		| Home                  | /                   |
		| All local authorities | /local-authorities/ |

@Javascript:disabled
Scenario: Search Term validation
	Given Local Authority "301" exists:
		"""
		{
			"name": "Some Test Name"
		}
		"""
	When I navigate to /local-authorities/
	Then I should get a 200 response
	And the page title should be "All local authorities"
	And the element "#searchForm" should have the text content "Enter local authority name or code Search"

@Javascript:enabled
Scenario: Search Term validation still works with JS enabled
	Given Local Authority "301" exists:
		"""
		{
			"name": "Some Test Name"
		}
		"""
	When I navigate to /local-authorities/
	Then I should get a 200 response
	And the page title should be "All local authorities"
	And the element "#searchForm" should have the text content "Enter local authority name or code When autocomplete results are available use up and down arrows to review and enter to select. Touch device users, explore by touch or with swipe gestures. Search"

@Javascript:disabled
Scenario: Search Term validation errors
	Given Local Authority "301" exists:
		"""
		{
			"name": "Some Test Name"
		}
		"""
	When I navigate to /local-authorities/
	And I click the button "#searchSubmit"
	Then the path should be /local-authorities/?search=
	And the element "#app-field-Search-input-error" should have the text content "Please enter a local authority name or code"
	And the element "h2.govuk-error-summary__title" should have the text content "There is a problem"
	And the element "*[data-testid='app-error-summary-Search']" should have the text content "Please enter a local authority name or code"

@Javascript:enabled
Scenario: Search Term validation errors still work with JS enabled
	Given Local Authority "301" exists:
		"""
		{
			"name": "Some Test Name"
		}
		"""
	When I navigate to /local-authorities/
	And I click the button "#searchSubmit"
	Then the path should be /local-authorities/?search=
	And the element "#app-field-Search-input-error" should have the text content "Please enter a local authority name or code"
	And the element "h2.govuk-error-summary__title" should have the text content "There is a problem"
	And the element "*[data-testid='app-error-summary-Search']" should have the text content "Please enter a local authority name or code"

@Javascript:disabled
Scenario: Generic Local Authorities search page should show correct message for search term with no matches
	Given Local Authority "301" exists:
		"""
		{
			"name": "Some Test Name"
		}
		"""
	When I navigate to /local-authorities/
	And I update the textbox "#app-field-Search" to have the value "secondary"
	And I click the button "#searchSubmit"
	Then the path should be /local-authorities/?search=secondary
	And the element "[data-testid="result-not-found-search-url"]" should have the href "/local-authorities/"
	And the element "h1" should have the text content "We found no matches for "secondary""

@Javascript:disabled
Scenario: Pagination in Search Validation
	Given 251 Local Authorities exist with properties:
		| code      | name                        |
		| (100 + n) | ASP Test LA Named (100 + n) |
	When I navigate to /local-authorities/?search=test
	Then the pagination summary should be "Showing 1 - 50 of 251 local authorities"
	And the pagination links should be:
		| Link Text | Url                                    |
		| 1         | /local-authorities/?search=test&page=1 |
		| 2         | /local-authorities/?search=test&page=2 |
		| ...       |                                        |
		| 6         | /local-authorities/?search=test&page=6 |
		| Next page | /local-authorities/?search=test&page=2 |
	And the listings should be:
		| Index | Name                  | Url                   | Code |
		| 1     | ASP Test LA Named 101 | /local-authority/101/ | 101  |
		| 2     | ASP Test LA Named 102 | /local-authority/102/ | 102  |
		| 3     | ASP Test LA Named 103 | /local-authority/103/ | 103  |
		| 4     | ASP Test LA Named 104 | /local-authority/104/ | 104  |
		| 5     | ASP Test LA Named 105 | /local-authority/105/ | 105  |

@Javascript:disabled
Scenario: Pagination in Search Validation 2
	Given 501 Local Authorities exist with properties:
		| code      | name                        |
		| (100 + n) | ASP Test LA Named (100 + n) |
	When I navigate to /local-authorities/?page=3&search=test
	Then the pagination summary should be "Showing 101 - 150 of 501 local authorities"
	And the pagination links should be:
		| Link Text | Url                                     |
		| Prev page | /local-authorities/?search=test&page=2  |
		| 1         | /local-authorities/?search=test&page=1  |
		| 2         | /local-authorities/?search=test&page=2  |
		| 3         | /local-authorities/?search=test&page=3  |
		| 4         | /local-authorities/?search=test&page=4  |
		| ...       |                                         |
		| 11        | /local-authorities/?search=test&page=11 |
		| Next page | /local-authorities/?search=test&page=4  |
	And the listings should be:
		| Index | Name                  | Url                   | Code |
		| 1     | ASP Test LA Named 201 | /local-authority/201/ | 201  |
		| 2     | ASP Test LA Named 202 | /local-authority/202/ | 202  |
		| 3     | ASP Test LA Named 203 | /local-authority/203/ | 203  |
		| 4     | ASP Test LA Named 204 | /local-authority/204/ | 204  |
		| 5     | ASP Test LA Named 205 | /local-authority/205/ | 205  |

@Javascript:disabled
Scenario: Pagination in Search Validation 3
	Given 51 Local Authorities exist with properties:
		| code      | name                        |
		| (100 + n) | ASP Test LA Named (100 + n) |
	When I navigate to /local-authorities/?page=2&search=test
	Then the pagination summary should be "Showing 51 - 51 of 51 local authorities"
	And the pagination links should be:
		| Link Text | Url                                    |
		| Prev page | /local-authorities/?search=test&page=1 |
		| 1         | /local-authorities/?search=test&page=1 |
		| 2         | /local-authorities/?search=test&page=2 |
	And the listings should be:
		| Index | Name                  | Url                   | Code |
		| 1     | ASP Test LA Named 151 | /local-authority/151/ | 151  |

@Javascript:disabled
Scenario: Matching LA Code search should redirect to LA landing page
	Given Local Authority "301" exists:
		"""
		{
			"name": "Some Test LA Name"
		}
		"""
	When I navigate to /local-authorities/
	And I update the textbox "#app-field-Search" to have the value "301"
	And I click the button "#searchSubmit"
	Then the path should be /local-authority/301/
	And the page subtitle should be "All schools within Some Test LA Name"

@Javascript:enabled
Scenario: Matching LA Code search should redirect to LA landing page with JS enabled
	Given Local Authority "301" exists:
		"""
		{
			"name": "Some Test LA Name"
		}
		"""
	When I navigate to /local-authorities/
	And I update the textbox "#app-field-Search" to have the value "301"
	And I click the button "#searchSubmit"
	Then the path should be /local-authority/301/
	And the page subtitle should be "All schools within Some Test LA Name"

@Javascript:disabled
Scenario: LA search successful for 3-digit LA Code
	Given Local Authority "301" exists:
		"""
		{
			"name": "Some Test LA Name"
		}
		"""
	When I navigate to /local-authorities/
	And I update the textbox "#app-field-Search" to have the value "301"
	And I click the button "#searchSubmit"
	Then the path should be /local-authority/301/
	And the page subtitle should be "All schools within Some Test LA Name"

@Javascript:disabled
Scenario Outline: LA search with less than 3 digits does not match on LA Code
	Given Local Authority "111" exists:
		"""
		{
			"name": "Some Test LA Name"
		}
		"""
	When I navigate to /local-authorities/
	And I update the textbox "#app-field-Search" to have the value "<SearchTerm>"
	And I click the button "#searchSubmit"
	Then the path should be /local-authorities/?search=<SearchTerm>
	And the element "h1" should have the text content "We found no matches for "<SearchTerm>""

Examples:
	| SearchTerm |
	| 1          |
	| 11         |


@Javascript:disabled
Scenario Outline: Multiple successful LA name matches show correct search results
	Given Local Authority "111" exists:
		"""
		{
			"name": "Some Test LA Name 111"
		}
		"""
	Given Local Authority "222" exists:
		"""
		{
			"name": "Some Test LA Name 222"
		}
		"""
	Given Local Authority "333" exists:
		"""
		{
			"name": "Some Test LA Name 333"
		}
		"""
	When I navigate to /local-authorities/
	And I update the textbox "#app-field-Search" to have the value "LA"
	And I click the button "#searchSubmit"
	Then the path should be /local-authorities/?search=LA
	And the listings should be:
		| Index | Code | Name                  |
		| 1     | 111  | Some Test LA Name 111 |
		| 2     | 222  | Some Test LA Name 222 |
		| 3     | 333  | Some Test LA Name 333 |

@Javascript:disabled
Scenario Outline: The PageNo parameter should handle invalid values with a default value of 1
	Given Local Authority "111" exists:
		"""
		{
			"name": "Some Test LA Name 111"
		}
		"""
	Given Local Authority "222" exists:
		"""
		{
			"name": "Some Test LA Name 222"
		}
		"""
	When I navigate to /local-authorities/?search=LA&page=<page>
	Then the page title should be "Search results for "LA""
	And the page subtitle should be "2 local authorities"
	And the element "*[data-testid='NumberOfPages']" should have the text content "Showing 1 - 2 of 2 local authorities"
	And the elements "*[data-testid='PageLinks-1']" should all have the href "/local-authorities/?search=LA&page=1"

Examples:
	| page |
	| y    |
	| 1.5  |
	| 0    |
	| -1   |

@Javascript:disabled
Scenario: The PageNo parameter number greater than the total number of pages, the last page of results should be shown
	Given 26 Local Authorities exist with properties:
		| code      | name                        |
		| (100 + n) | ASP Test LA Named (100 + n) |
	When I navigate to /local-authorities/?page=50&search=Test
	Then the page title should be "Search results for "Test""
	And the element "*[data-testid='NumberOfPages']" should have the text content "Showing 1 - 26 of 26 local authorities"
	And the page subtitle should be "26 local authorities"
	And the pagination links should be empty
	And the listings should be:
		| Index | Code | Name                  |
		| 1     | 101  | ASP Test LA Named 101 |
		| 26    | 126  | ASP Test LA Named 126 |

@Javascript:enabled
Scenario: Autocomplete Should Populate Items When Two Or More Characters Entered
	Given Local Authority "111" exists:
		"""
		{
			"name": "Some Test LA Name 111"
		}
		"""
	Given Local Authority "222" exists:
		"""
		{
			"name": "Some Other Test LA Name 222"
		}
		"""
	Given Local Authority "333" exists:
		"""
		{
			"name": "A Different Test LA Name 333"
		}
		"""
	And Local Authority "444" exists:
		"""
		{
			"name": "The Training Centre"
		}
		"""
	When I navigate to /local-authorities/
	And I update the textbox "#app-field-Search" to have the value "test"
	Then the autocomplete results should appear
	Then there should be 3 autocomplete items
	Then the elements ".autocomplete__option strong" should have the text contents:
		| Highlighted Values |
		| Test               |
		| Test               |
		| Test               |
	Then the elements ".autocomplete__option" should have the text contents:
		| Autocomplete Items                    |
		| A Different Test LA Name 333 Code:333 |
		| Some Other Test LA Name 222 Code:222  |
		| Some Test LA Name 111 Code:111        |

@Javascript:enabled
Scenario: Autocomplete Should Populate Items When Two Or More Characters Entered Sort By Code
	Given Local Authority "001" exists:
		"""
		{
			"name": "Some Test LA Name"
		}
		"""
	Given Local Authority "004" exists:
		"""
		{
			"name": "Some Other Test LA Name"
		}
		"""
	Given Local Authority "003" exists:
		"""
		{
			"name": "A Different Test LA Name"
		}
		"""
	And Local Authority "123" exists:
		"""
		{
			"name": "The Training Centre"
		}
		"""
	When I navigate to /local-authorities/
	And I update the textbox "#app-field-Search" to have the value "00"
	Then the autocomplete results should appear
	Then there should be 3 autocomplete items
	Then the elements ".autocomplete__option strong" should have the text contents:
		| Highlighted Values |
		| 00                 |
		| 00                 |
		| 00                 |
	Then the elements ".autocomplete__option" should have the text contents:
		| Autocomplete Items                |
		| Some Test LA Name Code:001        |
		| A Different Test LA Name Code:003 |
		| Some Other Test LA Name Code:004  |