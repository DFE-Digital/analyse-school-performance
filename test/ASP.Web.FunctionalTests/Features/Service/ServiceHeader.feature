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
Scenario Outline: Any user can view the Home link
	Given I am a <Role> user
    When I navigate to /
	Then the element "[data-testid="app-header-navigation-item-home"]" should have the text content "Home"
	And  the element "[data-testid="app-header-navigation-item-home"] a" should have the href "/"
Examples: 
	| Role            |
	| School Named    |
	| School Unnamed  |
	| School Governor |
	| LA Named        |
	| LA Unnamed      |
    | MAT Named       |
	| MAT Unnamed     |
	| MAT Governor    |
	| Diocese Named   |
	| Diocese Unnamed |
	| DfE Named       |
	| DfE Unnamed     |
	| Ofsted Unnamed  |
	| Super Admin     |

@Javascript:disabled
Scenario Outline: LA users can view the My local authority link
	Given I am a <Role> user
    When I navigate to /
	Then the element "[data-testid="app-header-navigation-item-my-local-authority"]" should have the text content "My local authority"
	And  the element "[data-testid="app-header-navigation-item-my-local-authority"] a" should have the href "/my-local-authority/"
Examples: 
	| Role       | 
	| LA Named   |
	| LA Unnamed |

@Javascript:disabled
Scenario Outline: Non LA users cannot view the My local authority link
	Given I am a <Role> user
    When I navigate to /
	Then the element "[data-testid="app-header-navigation-item-my-local-authority"]" should not exist
Examples: 
	| Role            |
	| School Named    |
	| School Unnamed  |
	| School Governor |
	| MAT Named       |
	| MAT Unnamed     |
	| MAT Governor    |
	| Diocese Named   |
	| Diocese Unnamed |
	| DfE Named       |
	| DfE Unnamed     |
	| Ofsted Unnamed  |
	| Super Admin     |

@Javascript:disabled
Scenario Outline: Users with all local authorities access can view the All local authorities link
	Given I am a <Role> user
    When I navigate to /
	Then the element "[data-testid="app-header-navigation-item-local-authorities"]" should have the text content "All local authorities"
	And  the element "[data-testid="app-header-navigation-item-local-authorities"] a" should have the href "/local-authorities/"
Examples: 
	| Role            |
	| DfE Named       |
	| DfE Unnamed     |
	| Super Admin     |
	| Ofsted Unnamed  |

@Javascript:disabled
Scenario Outline: Users without all local authorities access cannot view the All local authorities link
	Given I am a <Role> user
    When I navigate to /
	Then the element "[data-testid="app-header-navigation-item-local-authorities"]" should not exist
Examples: 
	| Role            |
	| School Named    |
	| School Unnamed  |
	| School Governor |
	| LA Named        |
	| LA Unnamed      |
	| MAT Named       |
	| MAT Unnamed     |
	| MAT Governor    |
	| Diocese Named   |
	| Diocese Unnamed |


@Javascript:disabled
Scenario Outline: Users with all schools access can view the All schools link
	Given I am a <Role> user
    When I navigate to /
	Then the element "[data-testid="app-header-navigation-item-schools"]" should have the text content "All schools"
	And  the element "[data-testid="app-header-navigation-item-schools"] a" should have the href "/schools/"
Examples: 
	| Role            |
	| DfE Named       |
	| DfE Unnamed     |
	| Ofsted Unnamed  |
	| Super Admin     |

@Javascript:disabled
Scenario Outline: Users without all schools access cannot view the All schools link
	Given I am a <Role> user
    When I navigate to /
	Then the element "[data-testid="app-header-navigation-item-schools"]" should not exist
Examples: 
	| Role            |
	| School Named    |
	| School Unnamed  |
	| School Governor |
	| LA Named        |
	| LA Unnamed      |
	| MAT Named       |
	| MAT Unnamed     |
	| MAT Governor    |
	| Diocese Named   |
	| Diocese Unnamed |

@Javascript:disabled
Scenario Outline: Users who can search can view the Search link
	Given I am a <Role> user
    When I navigate to /
	Then the element "[data-testid="app-header-navigation-item-search"]" should have the text content "Search"
	And  the element "[data-testid="app-header-navigation-item-search"] a" should have the href "/search/"
Examples: 
	| Role            |
	| LA Named        |
	| LA Unnamed      |
	| MAT Named       |
	| MAT Unnamed     |
	| MAT Governor    |
	| Diocese Named   |
	| Diocese Unnamed |
	| DfE Named       |
	| DfE Unnamed     |
	| Ofsted Unnamed  |
	| Super Admin     |

@Javascript:disabled
Scenario Outline: Users who cannot search cannot view the Search link
	Given I am a <Role> user
    When I navigate to /
	Then the element "[data-testid="app-header-navigation-item-search"]" should not exist
Examples: 
	| Role            |
	| School Unnamed  |
	| School Named    |
	| School Governor |

@Javascript:disabled
Scenario Outline: Users with access to My school can view the My school link
	Given I am a <Role> user
    When I navigate to /
	Then the element "[data-testid="app-header-navigation-item-my-school"]" should have the text content "My school"
	And  the element "[data-testid="app-header-navigation-item-my-school"] a" should have the href "/my-school/"
Examples: 
	| Role           |
	| School Named   |
	| School Unnamed |
	| School Governor |

@Javascript:disabled
Scenario Outline: Users without access to My school cannot view the My school link
	Given I am a <Role> user
    When I navigate to /
	Then the element "[data-testid="app-header-navigation-item-my-school"]" should not exist
Examples: 
	| Role            |
	| LA Named        |
	| LA Unnamed      |
	| MAT Named       |
	| MAT Unnamed     |
	| MAT Governor    |
	| Diocese Named   |
	| Diocese Unnamed |
	| DfE Named       |
	| DfE Unnamed     |
	| Ofsted Unnamed  |
	| Super Admin     |

@Javascript:disabled
Scenario Outline: Users with access to My schools can view the My schools link
	Given I am a <Role> user
    When I navigate to /
	Then the element "[data-testid="app-header-navigation-item-my-schools"]" should have the text content "My schools"
	And  the element "[data-testid="app-header-navigation-item-my-schools"] a" should have the href "/my-schools/"
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
	Then the element "[data-testid="app-header-navigation-item-my-schools"]" should not exist
Examples: 
	| Role            |
	| School Named    |
	| School Unnamed  |
	| School Governor |
	| DfE Named       |
	| DfE Unnamed     |
	| Ofsted Unnamed  |
	| Super Admin     |

@Javascript:disabled
Scenario Outline: Any user can view the Release timetable link
	Given I am a <Role> user
    When I navigate to /
	Then the element "[data-testid="app-header-navigation-item-release-timetable"]" should have the text content "Release timetable"
	And  the element "[data-testid="app-header-navigation-item-release-timetable"] a" should have the href "/help/release-timetable/"
Examples: 
	| Role            |
	| School Named    |
	| School Unnamed  |
	| School Governor |
	| LA Named        |
	| LA Unnamed      |
    | MAT Named       |
	| MAT Unnamed     |
	| MAT Governor    |
	| Diocese Named   |
	| Diocese Unnamed |
	| DfE Named       |
	| DfE Unnamed     |
	| Ofsted Unnamed  |
	| Super Admin     |

@Javascript:disabled
Scenario Outline: Any user can view the Guidance link
	Given I am a <Role> user
    When I navigate to /
	Then the element "[data-testid="app-header-navigation-item-guidance"]" should have the text content "Guidance"
	And  the element "[data-testid="app-header-navigation-item-guidance"] a" should have the href "/help/guidance/"
Examples: 
	| Role            |
	| School Named    |
	| School Unnamed  |
	| School Governor |
	| LA Named        |
	| LA Unnamed      |
    | MAT Named       |
	| MAT Unnamed     |
	| MAT Governor    |
	| Diocese Named   |
	| Diocese Unnamed |
	| DfE Named       |
	| DfE Unnamed     |
	| Ofsted Unnamed  |
	| Super Admin     |