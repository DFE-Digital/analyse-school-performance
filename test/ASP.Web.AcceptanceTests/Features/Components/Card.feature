Feature: Card component

Scenario: Home page cards should contain correct titles
  Given page content "home-page" exists:
		"""
		{
			"Views": [
				 {
                    "ViewId": "Card",
                    "ViewContent": {
                        "Title": "Title1"
                    }
                  },
                  {
                    "ViewId": "Card",
                    "ViewContent": {
                        "Title": "Title2"
                     }
                  },
                  {
                    "ViewId": "Card",
                    "ViewContent": {
                        "Title": "Title3",
                   }
                }
			]
		}
      """
  When I navigate to /
  Then the elements "a.app-card-link" should have the following content
   | Title   |
   | Title1 |
   | Title2 |
   | Title3 |


Scenario: Home page cards should contain correct link URLs
  Given page content "home-page" exists:
		"""
		{
			"Views": [
				 {
                    "ViewId": "Card",
                    "ViewContent": {
                       "LinkUrl": "/link1/"
                    }
                  },
                  {
                    "ViewId": "Card",
                    "ViewContent": {
                       "LinkUrl": "/link2/"
                     }
                  },
                  {
                    "ViewId": "Card",
                    "ViewContent": {
                       "LinkUrl": "/link3/"
                   }
                }
			]
		}
      """
  When I navigate to /
  Then the anchors "div.app-card-container > h2 > a" should have the following URLs
   | Link    |
   | /link1/ |
   | /link2/ |
   | /link3/ |


Scenario: Home page cards should contain correct text
  Given page content "home-page" exists:
		"""
		{
			"Views": [
				 {
                    "ViewId": "Card",
                    "ViewContent": {
                        "Text": "Text1"
                    }
                  },
                  {
                    "ViewId": "Card",
                    "ViewContent": {
                        "Text": "Text2"
                     }
                  },
                  {
                    "ViewId": "Card",
                    "ViewContent": {
                        "Text": "Text3",
                   }
                }
			]
		}
      """
  When I navigate to /
  Then the elements "div.app-card-container > p" should have the following content
   | Text   |
   | Text1 |
   | Text2 |
   | Text3 |