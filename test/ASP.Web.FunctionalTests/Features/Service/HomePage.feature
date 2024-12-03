Feature: Home page

Background:
	Given I am a logged-in user

@Javascript:disabled
Scenario Outline: Home page should be accessible from multiple paths
	When I navigate to <Path>
	Then I should get a 200 response
	And the page title should be "Home | Analyse school performance"
	And the element "#app-page-title" should have the text content "Analyse school performance"
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
                    "ViewId": "Card",
					"ViewContent": {
					    "AuthorisationPolicy": "Any",
                     }
                  },
                  {
                    "ViewId": "Card",
                    "ViewContent": {
					    "AuthorisationPolicy": "Any",
                     }
                  },
                  {
                    "ViewId": "Card",
					"ViewContent": {
					    "AuthorisationPolicy": "Any",
                     }
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
					    "AuthorisationPolicy": "Any",
                        "Id": "Id1",
                        "Title": "Title1",
                        "LinkUrl": "/link1/",
                        "Text": "Text1"
                    }
                    },
                    {
                    "ViewId": "Card",
                    "ViewContent": {
					    "AuthorisationPolicy": "Any",
                        "Id": "Id2",
                        "Title": "Title2",
                        "LinkUrl": "/link2/",
                        "Text": "Text2"
                        }
                    },
                    {
                    "ViewId": "Card",
                    "ViewContent": {
					    "AuthorisationPolicy": "Any",
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
	
@Javascript:disabled
Scenario Outline: Home page cards should be populated correctly for DfE/Ofsted user roles
	Given I am a <AccessToAllSchools> user
	And Content Template "home-page" exists:
	"""
	{
		"Views": [
			       {
						"ViewId": "Card",
						"ViewContent": {
							"AuthorizationPolicy": "AccessToMyDioceseSchools",
							"Id": "app-card-my-schools",
							"Title": "My schools",
							"LinkUrl": "/my-schools/",
							"Text": "View schools within your diocese"
						} 
	                },
					{ 
						"ViewId": "Card",
						"ViewContent": {
							"AuthorizationPolicy": "AccessToAllSchools",
							"Id": "app-card-local-authorities",
							"Title": "All local authorities",
							"LinkUrl": "/local-authorities/",
							"Text": "Search all local authorities"
	                	}
					},
	               {
						"ViewId": "Card",
						"ViewContent": {
							"AuthorizationPolicy": "AccessToAllSchools",
							"Id": "app-card-schools",
							"Title": "All schools",
							"LinkUrl": "/schools/",
							"Text": "Search all schools."
						}
	                } 
		     ]
	}
	"""
	When I navigate to /
	Then the element "#app-card-local-authorities h2 a" should have the href "/local-authorities/"
	And  the element "#app-card-local-authorities h2 a" should have the text content "All local authorities"
	And  the element "#app-card-local-authorities p" should have the text content "Search all local authorities"
  
	And  the element "#app-card-schools h2 a" should have the href "/schools/"
	And  the element "#app-card-schools h2 a" should have the text content "All schools"
	And  the element "#app-card-schools p" should have the text content "Search all schools."	
Examples:
  | AccessToAllSchools |
  | DfE Named          |
  | DfE Unnamed        |
  | Ofsted Unnamed     |
  | Super Admin        |
  
@Javascript:disabled
Scenario Outline: Home page cards should not be populated correctly for Non DfE/Ofsted user roles
	Given I am a <NoAccessToAllSchools> user
	And Content Template "home-page" exists:
	"""
	{
	 "Views": [
	{
		"ViewId": "Card",
		"ViewContent": {
			"AuthorizationPolicy": "AccessToMyDioceseSchools",
			"Id": "app-card-my-schools",
			"Title": "My schools",
			"LinkUrl": "/my-schools/",
			"Text": "View schools within your diocese"
		   } 
	    },
   		{ 
		"ViewId": "Card",
		"ViewContent": {
			"AuthorizationPolicy": "AccessToAllSchools",
			"Id": "app-card-local-authorities",
			"Title": "All local authorities",
			"LinkUrl": "/local-authorities/",
			"Text": "Search all local authorities"
		    }
   		},
		{
		"ViewId": "Card",
		"ViewContent": {
			"AuthorizationPolicy": "AccessToAllSchools",
			"Id": "app-card-schools",
			"Title": "All schools",
			"LinkUrl": "/schools/",
			"Text": "Search all schools."
			}
		} 
	 ]
	}
	"""
	When I navigate to /
	Then the element "#app-card-local-authorities h2 a" should not exist
	And the element "#app-card-local-authorities h2 a" should not exist
	And the element "#app-card-local-authorities p" should not exist
 
	And the element "#app-card-schools h2 a" should not exist
	And the element "#app-card-schools h2 a" should not exist
	And the element "#app-card-schools p" should not exist
Examples:
  | NoAccessToAllSchools |
  | School Named         |
  | School Unnamed       |
  | School Governor      |
  | MAT Named            |
  | MAT Unnamed          |
  | MAT Governor         |
  | Diocese Named        |
  | Diocese Unnamed      |
  | LA Named             |
  | LA Unnamed           |