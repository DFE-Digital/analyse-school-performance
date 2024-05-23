Feature: Card component

@Javascript:disabled
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
  Then the elements "a.app-card-link" should have the text contents:
   | Title   |
   | Title1 |
   | Title2 |
   | Title3 |

@Javascript:disabled
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
  Then the elements "div.app-card-container > h2 > a" should have the hrefs:
   | Link    |
   | /link1/ |
   | /link2/ |
   | /link3/ |

@Javascript:disabled
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
  Then the elements "div.app-card-container > p" should have the text contents:
   | Text  |
   | Text1 |
   | Text2 |
   | Text3 |