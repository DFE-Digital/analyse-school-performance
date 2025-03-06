Feature: Service header

@Javascript:disabled
Scenario: Header should contain user account and sign in links
	Given I am a School Named user called "Jimmy Jones"
	When I navigate to /
	Then the element "#header-link-service-name" should have the text content "Analyse school performance"
	And the element "#header-link-account-name" should have the text content "Jimmy Jones (School Named)"
	And the element "#header-link-sign-out" should have the text content "Sign out"

@Javascript:disabled
Scenario: Navigation item should be selected if current page is equal to or sub-path of item path
	Given I am a School Named user for Establishment "123456"
	And Establishment "123456" exists:
	"""
	{
	}
	"""
	When I navigate to <Path>
	Then the element "[data-testid='app-header-navigation-item-home']" class should contain "app-header__navigation-item--<HomeClass>"
	And the element "[data-testid='app-header-navigation-item-my-school']" class should contain "app-header__navigation-item--<MySchoolClass>"
Examples: 
	| Path                      | HomeClass    | MySchoolClass |
	| /                         | current      | not-selected  |
	| /my-school/               | not-selected | current       |
	| /my-school/other-reports/ | not-selected | current       |

@Javascript:disabled
Scenario Outline: School user top navigation
	Given I am a <Role> user
	When I navigate to /
	Then the top navigation should be:
		| Link Text         | Url                      |
		| Home              | /                        |
		| My school         | /my-school/              |
		| Release timetable | /help/release-timetable/ |
		| Guidance          | /help/guidance/          |
Examples: 
	| Role            |
	| School Named    |
	| School Unnamed  |
	| School Governor |

@Javascript:disabled
Scenario Outline: LA user top navigation
	Given I am a <Role> user
	When I navigate to /
	Then the top navigation should be:
		| Link Text          | Url                      |
		| Home               | /                        |
		| My local authority | /my-local-authority/     |
		| My schools         | /my-schools/             |
		| Release timetable  | /help/release-timetable/ |
		| Guidance           | /help/guidance/          |
Examples: 
	| Role            |
	| LA Named        |
	| LA Unnamed      |

@Javascript:disabled
Scenario Outline: MAT/Diocese user top navigation
	Given I am a <Role> user
	When I navigate to /
	Then the top navigation should be:
		| Link Text         | Url                      |
		| Home              | /                        |
		| My schools        | /my-schools/             |
		| Release timetable | /help/release-timetable/ |
		| Guidance          | /help/guidance/          |
Examples: 
	| Role            |
	| MAT Named       |
	| MAT Unnamed     |
	| MAT Governor    |
	| Diocese Named   |
	| Diocese Unnamed |

@Javascript:disabled
Scenario Outline: All schools user top navigation
	Given I am a <Role> user
	When I navigate to /
	Then the top navigation should be:
		| Link Text             | Url                      |
		| Home                  | /                        |
		| All local authorities | /local-authorities/      |
		| All schools           | /schools/                |
		| Release timetable     | /help/release-timetable/ |
		| Guidance              | /help/guidance/          |
Examples: 
	| Role            |
	| DfE Named       |
	| DfE Unnamed     |
	| Ofsted Unnamed  |
	| Super Admin     |