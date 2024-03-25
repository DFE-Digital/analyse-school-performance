Feature: Home page

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


Scenario: Home page should contain three cards
  Given page content "home-page" exists:
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
  Then The number of ".app-card" elements on the page should equal 3


 Scenario: Home page cards are in the correct location
      Given page content "home-page" exists:
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
   Then the element ".app-grid-container-three-column" should have the following markup: 
     """
        <div id="app-card-container" class="app-grid-container-three-column app-grid-container--wider govuk-!-margin-top-5">   
            <div id="Id1" class="app-card">
                <div class="app-card-container">
                    <h2 class="govuk-heading-m">
                        <a href="/link1/" class="app-card-link govuk-link govuk-link--no-visited-state">
                            Title1
                        </a>
                    </h2>
                    <p class="govuk-body">
                        Text1
                    </p>
                </div>
            </div>
            <div id="Id2" class="app-card">
                <div class="app-card-container">
                    <h2 class="govuk-heading-m">
                        <a href="/link2/" class="app-card-link govuk-link govuk-link--no-visited-state">
                            Title2
                        </a>
                    </h2>
                    <p class="govuk-body">
                        Text2
                    </p>
                </div>
            </div>
            <div id="Id3" class="app-card">
                <div class="app-card-container">
                    <h2 class="govuk-heading-m">
                        <a href="/link3/" class="app-card-link govuk-link govuk-link--no-visited-state">
                           Title3
                        </a>
                    </h2>
                    <p class="govuk-body">
                        Text3
                    </p>
                </div>
            </div>
         </div>         
	  """


 Scenario: Home page cards show service title when home-page content is missing in the DB
   When I navigate to /
   Then the element "h1.govuk-heading-xl" should have the following markup: 
   """
      <h1 class="govuk-heading-xl">Analyse school performance</h1>
   """