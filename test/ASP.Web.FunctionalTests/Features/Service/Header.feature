Feature: Service header

@Javascript:disabled
Scenario: Header should contain user account and sign in links
	Given I am a School Named user called "Jimmy Jones"
	When I navigate to /
	Then the element "#header-link-service-name" should have the text content "Analyse school performance"
	And the element "#header-link-account-name" should have the text content "Jimmy Jones (School Named)"
	And the element "#header-link-sign-out" should have the text content "Sign out"

@Javascript:disabled
Scenario Outline: Any user can view the Home link
	Given I am a <Role> user
    When I navigate to /
	Then the element "#header-navigation-link-home" should have the text content "Home"
	And  the element "#header-navigation-link-home" should have the href "/"
Examples: 
	| Role            |
	| DfE Named       |
	| DfE Unnamed     |
	| Super Admin     |
	| Ofsted Unnamed  |
	| LA Named        |
	| LA Unnamed      |
    | MAT Named       |
	| MAT Unnamed     |
	| School Named    |
	| School Unnamed  |
	| Diocese Named   |
	| Diocese Unnamed |
	| MAT Governor    |
	| School Governor |


@Javascript:disabled
Scenario Outline: LA users can view the My local authority link
	Given I am a <Role> user
    When I navigate to /
	Then the element "#header-navigation-link-my-local-authority" should have the text content "My local authority"
	And  the element "#header-navigation-link-my-local-authority" should have the href "/my-local-authority/001"
Examples: 
	| Role       | 
	| LA Named   |
	| LA Unnamed |


@Javascript:disabled
Scenario Outline: Non LA users cannot view the My local authority link
	Given I am a <Role> user
    When I navigate to /
	Then the element "#header-navigation-link-my-local-authority" should not exist
Examples: 
	| Role            |
	| DfE Named       |
	| DfE Unnamed     |
	| Super Admin     |
	| Ofsted Unnamed  |
	| MAT Named       |
	| MAT Unnamed     |
	| School Named    |
	| School Unnamed  |
	| Diocese Named   |
	| Diocese Unnamed |
	| MAT Governor    |
	| School Governor |


@Javascript:disabled
Scenario Outline: Users who can search can view the Search link
	Given I am a <Role> user
    When I navigate to /
	Then the element "#header-navigation-link-search" should have the text content "Search"
	And  the element "#header-navigation-link-search" should have the href "/search"
Examples: 
	| Role            |
	| DfE Named       |
	| DfE Unnamed     |
	| Super Admin     |
	| Ofsted Unnamed  |
	| LA Named        |
	| LA Unnamed      |
	| MAT Named       |
	| MAT Unnamed     |
	| Diocese Named   |
	| Diocese Unnamed |
	| MAT Governor    |


@Javascript:disabled
Scenario Outline: Users who cannot search cannot view the Search link
	Given I am a <Role> user
    When I navigate to /
	Then the element "#header-navigation-link-search" should not exist
Examples: 
	| Role            |
	| School Unnamed  |
	| School Named    |
	| School Governor |


@Javascript:disabled
Scenario Outline: Users with access to My school can view the My school link
	Given I am a <Role> user
    When I navigate to /
	Then the element "#header-navigation-link-my-school" should have the text content "My school"
	And  the element "#header-navigation-link-my-school" should have the href "/school/136028"
Examples: 
	| Role           |
	| School Unnamed |
	| School Named   |


@Javascript:disabled
Scenario Outline: Users without access to My school cannot view the My school link
	Given I am a <Role> user
    When I navigate to /
	Then the element "#header-navigation-link-my-school" should not exist
Examples: 
	| Role            |
	| DfE Named       |
	| DfE Unnamed     |
	| Super Admin     |
	| Ofsted Unnamed  |
	| LA Named        |
	| LA Unnamed      |
	| MAT Named       |
	| MAT Unnamed     |
	| Diocese Named   |
	| Diocese Unnamed |
	| MAT Governor    |
	| School Governor |


@Javascript:disabled
Scenario Outline: Users with access to My schools can view the My schools link
	Given I am a <Role> user
    When I navigate to /
	Then the element "#header-navigation-link-my-schools" should have the text content "My schools"
	And  the element "#header-navigation-link-my-schools" should have the href "/my-schools"
Examples: 
	| Role            |
	| LA Named        |
	| LA Unnamed      |
	| MAT Named       |
	| MAT Unnamed     |
	| MAT Governor    |
	| Diocese Named   |
	| Diocese Unnamed |


@Javascript:disabled
Scenario Outline: Users without access to My schools cannot view the My schools link
	Given I am a <Role> user
    When I navigate to /
	Then the element "#header-navigation-link-my-schools" should not exist
Examples: 
	| Role            |
	| DfE Named       |
	| DfE Unnamed     |
	| School Unnamed  |
	| School Named    |
	| School Governor |
	| Super Admin     |
	| Ofsted Unnamed  |


@Javascript:disabled
Scenario Outline: Any user can view the Site news link
	Given I am a <Role> user
    When I navigate to /
	Then the element "#header-navigation-link-news" should have the text content "Site news"
	And  the element "#header-navigation-link-news" should have the href "/news"
Examples: 
	| Role            |
	| DfE Named       |
	| DfE Unnamed     |
	| Super Admin     |
	| Ofsted Unnamed  |
	| LA Named        |
	| LA Unnamed      |
    | MAT Named       |
	| MAT Unnamed     |
	| School Named    |
	| School Unnamed  |
	| Diocese Named   |
	| Diocese Unnamed |
	| MAT Governor    |
	| School Governor |


@Javascript:disabled
Scenario Outline: Any user can view the Release timetable link
	Given I am a <Role> user
    When I navigate to /
	Then the element "#header-navigation-link-release-timetable" should have the text content "Release timetable"
	And  the element "#header-navigation-link-release-timetable" should have the href "/help/release-timetable"
Examples: 
	| Role            |
	| DfE Named       |
	| DfE Unnamed     |
	| Super Admin     |
	| Ofsted Unnamed  |
	| LA Named        |
	| LA Unnamed      |
    | MAT Named       |
	| MAT Unnamed     |
	| School Named    |
	| School Unnamed  |
	| Diocese Named   |
	| Diocese Unnamed |
	| MAT Governor    |
	| School Governor |


@Javascript:disabled
Scenario Outline: Any user can view the Guidance link
	Given I am a <Role> user
    When I navigate to /
	Then the element "#header-navigation-link-guidance" should have the text content "Guidance"
	And  the element "#header-navigation-link-guidance" should have the href "/help/guidance"
Examples: 
	| Role            |
	| DfE Named       |
	| DfE Unnamed     |
	| Super Admin     |
	| Ofsted Unnamed  |
	| LA Named        |
	| LA Unnamed      |
    | MAT Named       |
	| MAT Unnamed     |
	| School Named    |
	| School Unnamed  |
	| Diocese Named   |
	| Diocese Unnamed |
	| MAT Governor    |
	| School Governor |