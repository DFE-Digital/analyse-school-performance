Feature: Home page

@Javascript:disabled
Scenario Outline: Home page should be accessible from multiple paths
	When I navigate to <Path>
	Then I should get a 200 response
	And the page title should be "Home | Analyse school performance"
	And the element "h1.govuk-heading-xl" should have the text content "Analyse school performance"
Examples:
	| Path        |
	| /           |
	| /home       |
	| /home/index |

@Javascript:disabled
Scenario: Home page cards should be populated from the "home-page" content template
  Given Content Template "home-page" exists:
		"""
		{
			"Views": [
				 {
                    "ViewId": "Card"
                  },
                  {
                    "ViewId": "Card",
                  },
                  {
                    "ViewId": "Card",
                  }
			]
		}
		"""
  When I navigate to /
  Then the element "#app-card-container" class should contain "app-grid-container-three-column"
  And the elements "#app-card-container .app-card" should total 3

@Javascript:disabled
Scenario: Home page cards should be populated correctly
      Given Content Template "home-page" exists:
		"""
		{
			"Views": [
				    {
                    "ViewId": "Card",
                    "ViewContent": {
                        "Id": "Id1",
                        "Title": "Title1",
                        "LinkUrl": "/link1/",
                        "Text": "Text1"
                    }
                    },
                    {
                    "ViewId": "Card",
                    "ViewContent": {
                        "Id": "Id2",
                        "Title": "Title2",
                        "LinkUrl": "/link2/",
                        "Text": "Text2"
                        }
                    },
                    {
                    "ViewId": "Card",
                    "ViewContent": {
                        "Id": "Id3",
                        "Title": "Title3",
                        "LinkUrl": "/link3/",
                        "Text": "Text3"
                    }
                }
			]
		}
		"""
   When I navigate to /
   Then the element "#Id1 h2 a" should have the href "/link1/"
   And  the element "#Id1 h2 a" should have the text content "Title1"
   And  the element "#Id1 p" should have the text content "Text1"
   
   And  the element "#Id2 h2 a" should have the href "/link2/"
   And  the element "#Id2 h2 a" should have the text content "Title2"
   And  the element "#Id2 p" should have the text content "Text2"
   
   And  the element "#Id3 h2 a" should have the href "/link3/"
   And  the element "#Id3 h2 a" should have the text content "Title3"
   And  the element "#Id3 p" should have the text content "Text3"

@Javascript:disabled
Scenario: Home page hero show service title when home-page content is missing in the DB
   When I navigate to /
   Then the element "#app-hero h1" should have the text content "Analyse school performance"

@Javascript:disabled
Scenario: Home page hero should display placeholder title when PageContent is missing
	Given Content Template "home-page" exists:
		"""
		{
			"PageTitle": "Analyse school performance"
		}
		"""
	When I navigate to /
	Then I should get a 200 response
	Then the element "#app-hero h1" should have the text content "Analyse school performance"

@Javascript:disabled
Scenario: Home page hero should display placeholder description when HeroDescription is missing
	Given Content Template "home-page" exists:
		"""
		{
			"PageTitle": "Analyse school performance",
			"PageContent": {
			}
		}
		"""
	When I navigate to /
	Then I should get a 200 response
	Then the element "#app-hero p" should have the text content "Service description goes here..."

@Javascript:disabled
Scenario: Home page hero should display placeholder description when HeroDescription is null
	Given Content Template "home-page" exists:
		"""
		{
			"PageTitle": "Analyse school performance",
			"PageContent": {
				"HeroDescription": null
			}
		}
		"""
	When I navigate to /
	Then I should get a 200 response
	Then the element "#app-hero p" should have the text content "Service description goes here..."

@Javascript:disabled
Scenario: Home page hero should display correct heading and description
	Given Content Template "home-page" exists:
		"""
		{
			"PageTitle": "Analyse school performance",
			"PageContent": {
				"HeroDescription": "Hero description"
			}
		}
		"""
	When I navigate to /
	Then I should get a 200 response
	Then the element "#app-hero h1" should have the text content "Analyse school performance"
	Then the element "#app-hero p" should have the text content "Hero description"