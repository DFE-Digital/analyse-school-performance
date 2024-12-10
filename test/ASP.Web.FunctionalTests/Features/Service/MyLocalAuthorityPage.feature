Feature: My local authority page

@Javascript:disabled
Scenario: A non-LA user should not be able to access the 'My local authority' page. Instead, they should see a 403 Access not allowed page.
    Given I am a MAT Named user for Multi-Academy Trust "1234"
    When I navigate to /my-local-authority/
    Then I should get a 403 response
    And the page title should be "Access not allowed | Analyse school performance"
    And the element "h1.govuk-heading-l" should have the text content "Access not allowed"

@Javascript:disabled
Scenario: A user with access to all LAs should not be able to access the 'My local authority' page. Instead, they should see a 403 Access not allowed page.
    Given I am a DfE Named user
    When I navigate to /my-local-authority/
    Then I should get a 403 response
    And the page title should be "Access not allowed | Analyse school performance"
    And the element "h1.govuk-heading-l" should have the text content "Access not allowed"

@Javascript:disabled
Scenario: My local authority page should display server error page if user's LA does not exist
    Given Local Authority "301" exists:
    """
    {
        "Name": "Test Name",
        "Code": "301"
    }
    """
    And I am a LA Named user for Local Authority "302"
    When I navigate to /my-local-authority/
    Then I should get a 500 response
    And the page title should be "Sorry, there is a problem with the service | Analyse school performance"
    And the element "h1.govuk-heading-l" should have the text content "Sorry, there is a problem with the service"
    And the element "*[data-testid='error-display-message']" should have the text content "Error message: API error: Could not find Local Authority with code "302"."
        
@Javascript:disabled
Scenario: My local authority page should be accessible if user's LA exists
    Given I am a LA Named user for Local Authority "301"
    And Local Authority "301" exists:
    """
    {
        "Name": "Test Name",
        "Code": "301"
    }
    """
    When I navigate to /my-local-authority/
    Then I should get a 200 response
    And the page title should be "My local authority | Analyse school performance"
    And the element "#app-page-title" should have the text content "My local authority"
    And the element "#app-page-subtitle" should have the text content "All schools within Test Name"
    And the breadcrumb trail should be:
		| text               | href | current |
		| Home               | /    |         |
		| My local authority |      | true    |

@Javascript:disabled
Scenario: My local authority page cards should be populated from the "la-landing-page" content template
    Given I am a LA Unnamed user for Local Authority "301"
    And Local Authority "301" exists:
    """
    {
        "Name": "Test Name",
        "Code": "301"
    }
    """
    And Content Template "la-landing-page" exists:
    """
    {
        "Views": [
        	{
                "ViewId": "Card",
                "ViewContent": {
                    "Id": "app-card-download-data",
                    "Title": "Download data",
                    "LinkUrl": "download-data",
                    "Text": "Download data for Analyse school performance and Key to success."
                }
            }
        ]
    }
    """
    When I navigate to /my-local-authority/
    Then the element "#app-card-download-data h2 a" should have the href "download-data"
    And the element "#app-card-download-data h2 a" should have the text content "Download data"
    And the element "#app-card-download-data p" should have the text content "Download data for Analyse school performance and Key to success."

@Javascript:disabled
Scenario: Download data 'Dates Available for Download' - page should be accessible with a valid laCode
	Given Local Authority "301" exists:
	"""
		{
		    "Name": "Test Name",
		    "Code": "301"
		}
	"""
	And I am a LA Named user for Local Authority "301"
	When I navigate to /my-local-authority/download-data/pupil-level-aggregated-la-data/
	Then I should get a 200 response
	And the element "#app-page-title" should have the text content "Download data"
	And the sub-navigation should be:
		| text          | href                               | current |
		| Download data | /my-local-authority/download-data/ | true    |
	And the side navigation should be:
		| text                               | href                                                              | current |
		| Pupil level and aggregated LA data | /my-local-authority/download-data/pupil-level-aggregated-la-data/ | true    |
	And the element "#app-subpage-title" should have the text content "Pupil level and aggregated LA data Dates available for download"
	And the element "#app-subpage-title-caption" should have the text content "Pupil level and aggregated LA data"

@Javascript:disabled
Scenario: Download data 'Dates Available for Download' - page should show a breadcrumb trail
	Given Local Authority "301" exists:
	"""
	{
		"Name": "Test Name",
	}
	"""
	And I am a LA Named user for Local Authority "301"
	When I navigate to /my-local-authority/download-data/pupil-level-aggregated-la-data/
	Then I should get a 200 response
	And the page title should be "Download data | Analyse school performance"
	And the breadcrumb trail should be:
		| text                         | href                               | current |
		| Home                         | /                                  |         |
		| My local authority           | /my-local-authority/               |         |
		| Download data                | /my-local-authority/download-data/ |         |
		| Dates available for download |                                    | true    |

@Javascript:disabled
Scenario Outline: Download data 'Dates Available for Download' - page should contain three radio buttons
	Given Local Authority "301" exists:
	"""
		{
		    "Name": "Test Name",
		    "Code": "301"
		}
	"""
	And I am a LA Named user for Local Authority "301"
	When I navigate to /my-local-authority/download-data/pupil-level-aggregated-la-data/
	Then I should get a 200 response
	And the element "[data-testid='available-downloads-dates-<year>-label']" should have the text content "<label>"
Examples:
  | year | label        |
  | 2022 | 2021 to 2022 |
  | 2023 | 2022 to 2023 |
  | 2024 | 2023 to 2024 |

@Javascript:disabled
Scenario: Data downloads 'Dates available for download' - when no date is selected and Continue button clicked, should show validation error
	Given Local Authority "301" exists:
	"""
		{
		    "Name": "Test Name",
		    "Code": "301"
		}
	"""
	And I am a LA Named user for Local Authority "301"
	When I navigate to /my-local-authority/download-data/pupil-level-aggregated-la-data/
    And I click the button "*[data-testid='selectedYearSubmit']"
    Then the path should be /my-local-authority/download-data/pupil-level-aggregated-la-data/
    And the element "*[data-testid='app-error-summary'] h2" should have the text content "There is a problem"
    And the element "*[data-testid='app-error-summary-selectedYear']" should have the text content "Please choose an academic year to download"
    And the element "*[data-testid='app-error-summary-selectedYear']" should have the href "#app-field-selectedYear"
    And the element "*[data-testid='app-field-selectedYear-error']" should have the text content "Please choose an academic year to download"

@Javascript:disabled
Scenario: Data downloads 'Dates available for download' - when date is selected and Continue button clicked, should move to next step
	Given Local Authority "301" exists:
	"""
		{
		    "Name": "Test Name",
		    "Code": "301"
		}
	"""
	And I am a LA Named user for Local Authority "301"
	When I navigate to /my-local-authority/download-data/pupil-level-aggregated-la-data/
    And I update the element "#app-available-downloads-dates-2022" to be checked
    And I click the button "*[data-testid='selectedYearSubmit']"
    Then the path should be /my-local-authority/download-data/pupil-level-aggregated-la-data/select-files/?selectedYear=2022

@Javascript:disabled
Scenario: Download data 'Data files available for download' - page should be accessible when a valid laCode
	Given Local Authority "301" exists:
	"""
		{
		    "Name": "Test Name",
		    "Code": "301"
		}
	"""
	And I am a LA Named user for Local Authority "301"
	When I navigate to /my-local-authority/download-data/pupil-level-aggregated-la-data/select-files/?selectedYear=2022
	Then I should get a 200 response
	And the element "#app-page-title" should have the text content "Download data"
	And the sub-navigation should be:
		| text          | href                               | current |
		| Download data | /my-local-authority/download-data/ | true    |
	And the side navigation should be:
		| text                               | href                                                              | current |
		| Pupil level and aggregated LA data | /my-local-authority/download-data/pupil-level-aggregated-la-data/ | true    |
	And the element "#app-subpage-title" should have the text content "Pupil level and aggregated LA data Data files available for download"
	And the element "#app-subpage-title-caption" should have the text content "Pupil level and aggregated LA data"
	
@Javascript:disabled

Scenario: Download data 'Data files available for download' - page should show a breadcrumb trail
	Given Local Authority "301" exists:
	"""
	{
		"Name": "Test Name",
	}
	"""
	And I am a LA Named user for Local Authority "301"
	When I navigate to /my-local-authority/download-data/pupil-level-aggregated-la-data/select-files/?selectedYear=2022
	Then I should get a 200 response
	And the page title should be "Download data | Analyse school performance"
	And the breadcrumb trail should be:
		| text                              | href                                                              | current |
		| Home                              | /                                                                 |         |
		| My local authority                | /my-local-authority/                                              |         |
		| Download data                     | /my-local-authority/download-data/                                |         |
		| Dates available for download      | /my-local-authority/download-data/pupil-level-aggregated-la-data/ |         |
		| Data files available for download |                                                                   | true    |

@Javascript:disabled
Scenario Outline: Download data 'Data files available for download' - page should contain three checkbox groups
	Given Local Authority "301" exists:
	"""
	{
		"Name": "Test Name",
	}
	"""
	And I am a LA Named user for Local Authority "301"
	When I navigate to /my-local-authority/download-data/pupil-level-aggregated-la-data/select-files/?selectedYear=2022
	Then I should get a 200 response
	And the element "[data-testid='available-downloads-file-group-<group>']" should have the text content "<text>"
Examples:
  | group       | text        |
  | Key stage 2 | Key stage 2 |
  | Key stage 4 | Key stage 4 |
  | Phonics     | Phonics     |	
  
@Javascript:disabled
Scenario Outline: Download data 'Data files available for download' - page should contain five checkboxes
	Given Local Authority "301" exists:
	"""
	{
		"Name": "Test Name",
	}
	"""
	And I am a LA Named user for Local Authority "301"
    When I navigate to /my-local-authority/download-data/pupil-level-aggregated-la-data/select-files/?selectedYear=2022
    Then I should get a 200 response
    And the element "[data-testid='available-downloads-file-<fileid>-label']" should have the text content "<label>"

Examples:
  | fileid                              | label                                                  |
  | kts-301-ks2-la-2022-final           | Key stage 2 (Final) (Key to success)                   |
  | asp-301-ks2-la-2022-provisional     | Key stage 2 (Provisional) (Analyse school performance) |
  | kts-301-ks4-la-2022-final-pupil     | Key stage 4 (Final) (Key to success)                   |
  | asp-301-ks4-la-2022-final-pupil     | Key stage 4 (Final) (Analyse school performance)       |
  | kts-301-phonics-la-2022-final-pupil | Phonics (Final) (Key to success)                       |

@Javascript:disabled
Scenario: Data downloads 'Data files available for download' - when no files are selected and Continue button clicked, should show validation error
	Given Local Authority "301" exists:
	"""
	{
		"Name": "Test Name",
	}
	"""
	And I am a LA Named user for Local Authority "301"
    When I navigate to /my-local-authority/download-data/pupil-level-aggregated-la-data/select-files/?selectedYear=2022
    And I click the button "*[data-testid='selectedFilesSubmit']"
    Then the path should be /my-local-authority/download-data/pupil-level-aggregated-la-data/select-files/?selectedYear=2022
    And the element "*[data-testid='app-error-summary'] h2" should have the text content "There is a problem"
    And the element "*[data-testid='app-error-summary-selectedFiles']" should have the text content "Please choose one or more data files to download"
    And the element "*[data-testid='app-error-summary-selectedFiles']" should have the href "#app-field-selectedFiles"
    And the element "*[data-testid='app-field-selectedFiles-error']" should have the text content "Please choose one or more data files to download"

@Javascript:disabled
Scenario: Data downloads 'Data files available for download' - when files are selected and Continue button clicked, should move to next step
	Given Local Authority "301" exists:
	"""
	{
		"Name": "Test Name",
	}
	"""
	And I am a LA Named user for Local Authority "301"
    When I navigate to /my-local-authority/download-data/pupil-level-aggregated-la-data/select-files/?selectedYear=2022
    And I update the element "#app-available-downloads-file-kts-301-ks2-la-2022-final" to be checked
    And I click the button "*[data-testid='selectedFilesSubmit']"
    Then the path should be /my-local-authority/download-data/pupil-level-aggregated-la-data/select-format/?selectedYear=2022&selectedFiles=kts-301-ks2-la-2022-final

@Javascript:disabled
Scenario: Download data 'Download pupil level and aggregated LA data' - page should be accessible with a valid laCode
	Given Local Authority "301" exists:
	"""
	{
		"Name": "Test Name",
	}
	"""
	And I am a LA Named user for Local Authority "301"
    When I navigate to /my-local-authority/download-data/pupil-level-aggregated-la-data/select-format/?selectedYear=2022&selectedFiles=kts-301-ks2-la-2022-final&selectedFiles=asp-301-ks2-la-2022-provisional
    Then I should get a 200 response
    And the element "#app-page-title" should have the text content "Download data"
	And the sub-navigation should be:
		| text          | href                               | current |
		| Download data | /my-local-authority/download-data/ | true    |
    And the side navigation should be:
		| text                               | href                                                              | current |
		| Pupil level and aggregated LA data | /my-local-authority/download-data/pupil-level-aggregated-la-data/ | true    |
    And the element "#app-subpage-title" should have the text content "Pupil level and aggregated LA data Download pupil level and aggregated LA data"
    And the element "#app-subpage-title-caption" should have the text content "Pupil level and aggregated LA data"

@Javascript:disabled
Scenario: Download data 'Download pupil level and aggregated LA data' - page should show a breadcrumb trail
	Given Local Authority "301" exists:
	"""
	{
		"Name": "Test Name",
		"Code": "301"
	}
	"""
	And I am a LA Named user for Local Authority "301"
	When I navigate to /my-local-authority/download-data/pupil-level-aggregated-la-data/select-format/?selectedYear=2022&selectedFiles=kts-004-phonics-la-2022-final-pupil
	Then I should get a 200 response
	And the page title should be "Download data | Analyse school performance"
	And the breadcrumb trail should be:
		| text                                        | href                                                                                             | current |
		| Home                                        | /                                                                                                |         |
		| My local authority                          | /my-local-authority/                                                                             |         |
		| Download data                               | /my-local-authority/download-data/                                                               |         |
		| Dates available for download                | /my-local-authority/download-data/pupil-level-aggregated-la-data/                                |         |
		| Data files available for download           | /my-local-authority/download-data/pupil-level-aggregated-la-data/select-files/?selectedYear=2022 |         |
		| Download pupil level and aggregated LA data |                                                                                                  | true    |

@Javascript:disabled
Scenario Outline: Download data - 'Download pupil level and aggregated LA data' - page should contain three links
	Given Local Authority "301" exists:
	"""
		{
		    "Name": "Test Name",
		    "Code": "301"
		}
	"""
	And I am a LA Named user for Local Authority "301"
    When I navigate to /my-local-authority/download-data/pupil-level-aggregated-la-data/select-format/?selectedYear=2022&selectedFiles=kts-301-ks2-la-2022-final&selectedFiles=asp-301-ks2-la-2022-provisional
    Then I should get a 200 response
	And the element "[data-testid="select-format-description"]" should have the text content "The data included in your download is the pupil level / aggregated data for your LA."
    And the available download formats should be:
    	| text               | href                                                                                                                                                  |
    	| Data in CSV format | /my-local-authority/download-data/pupil-level-aggregated-la-data/download-as-zip/?fileType=CSV&selectedFiles=kts-301-ks2-la-2022-final&selectedFiles=asp-301-ks2-la-2022-provisional |

@Javascript:disabled
Scenario Outline: Data downloads 'Download school data' - Download other dates link should link back to first step
	Given Local Authority "301" exists:
	"""
	{
		"Name": "Test Name",
		"Code": "301"
	}
	"""
	And I am a LA Named user for Local Authority "301"
	When I navigate to /my-local-authority/download-data/pupil-level-aggregated-la-data/select-format/?selectedYear=2022&selectedFiles=kts-301-ks2-la-2022-final&selectedFiles=asp-301-ks2-la-2022-provisional
	Then the element "[data-testid="available-downloads-other-dates"]" should have the href "/my-local-authority/download-data/pupil-level-aggregated-la-data/"