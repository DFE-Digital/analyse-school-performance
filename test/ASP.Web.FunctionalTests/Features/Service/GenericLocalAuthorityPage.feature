Feature: Generic Local authority page

Background:
    Given I am a DfE Named user

@Javascript:disabled
Scenario: A user with no access to all Local Authorities should not be able to access the generic 'Local authority' page. Instead, they should see a 403 Access not allowed page.
    Given I am a MAT Named user for Multi-Academy Trust "1234"
    When I navigate to /local-authority/301/
    Then I should get a 403 response
    And the page title should be "Access not allowed | Analyse school performance"
    And the element "h1.govuk-heading-l" should have the text content "Access not allowed"

@Javascript:disabled
Scenario: An LA user should not be able to access the generic 'Local authority' page, even if it's for their own LA. Instead, they should see a 403 Access not allowed page.
    Given I am a LA Named user for Local Authority "301"
    When I navigate to /local-authority/301/
    Then I should get a 403 response
    And the page title should be "Access not allowed | Analyse school performance"
    And the element "h1.govuk-heading-l" should have the text content "Access not allowed"

@Javascript:disabled
Scenario: Local authority page should not be found when invalid code is provided
    And Local Authority "301" exists:
    """
    {
        "Name": "Test LA",
    }
    """
    When I navigate to /local-authority/302/
    Then I should get a 404 response
    And the page title should be "Page not found | Analyse school performance"
    And the element "h1.govuk-heading-l" should have the text content "Page not found"

@Javascript:disabled
Scenario: Local authority page should be accessible when valid code is provided
    Given I am a DfE Named user
    And Local Authority "301" exists:
    """
    {
        "Name": "Test LA",
    }
    """
    When I navigate to /local-authority/301/
    Then I should get a 200 response
    And the page title should be "Test LA | Analyse school performance"
    And the element "#app-page-title" should have the text content "Test LA"
    And the element "#app-page-subtitle" should have the text content "All schools within Test LA"
    And the breadcrumb trail should be:
		| text                  | href                | current |
		| Home                  | /                   |         |
		| All local authorities | /local-authorities/ |         |
		| Test LA               |                     | true    |

@Javascript:disabled
Scenario: Local authority page cards should be populated from the "la-landing-page" content template
    Given Content Template "la-landing-page" exists:
    """
    {
        "Views": [
        	{
                "ViewId": "Card",
                "ViewContent": {
                    "Id": "app-card-la-all-schools",
                    "Title": "All schools",
                    "LinkUrl": "schools/",
                    "Text": "All schools found in this LA."
                }
            },
            {
                "ViewId": "Card",
                "ViewContent": {
                    "Id": "app-card-la-download",
                    "Title": "Download data",
                    "LinkUrl": "download-data/",
                    "Text": "Download data for Analyse school performance and Key to success."
                }
            }
        ]
    }
    """
    And Local Authority "302" exists:
    """
    {
        "Name": "Test LA",
    }
    """
    When I navigate to /local-authority/302/
    Then the element "#app-card-la-all-schools h2 a" should have the href "schools/"
    And the element "#app-card-la-all-schools h2 a" should have the text content "All schools"
    And the element "#app-card-la-all-schools p" should have the text content "All schools found in this LA."
    Then the element "#app-card-la-download h2 a" should have the href "download-data/"
    And the element "#app-card-la-download h2 a" should have the text content "Download data"
    And the element "#app-card-la-download p" should have the text content "Download data for Analyse school performance and Key to success."
    
@Javascript:disabled
Scenario: Page should show a breadcrumb trail
    Given Local Authority "301" exists:
    """
    {
        "Name": "Test LA",
    }
    """
    When I navigate to /local-authority/301/
    Then I should get a 200 response
    And the page title should be "Test LA | Analyse school performance"
    And the element "#app-page-subtitle" should have the text content "All schools within Test LA"
	And the breadcrumb trail should be:
		| text                  | href                | current |
		| Home                  | /                   |         |
		| All local authorities | /local-authorities/ |         |
		| Test LA               |                     | true    |

@Javascript:disabled
	Scenario: Data downloads 'Dates Available for Download' - page should be accessible with a valid laCode
	Given Local Authority "301" exists:
	"""
	{
		"Name": "Test LA",
	}
	"""
	When I navigate to /local-authority/301/download-data/pupil-level-aggregated-la-data/
	Then I should get a 200 response
	And the element "#app-page-title" should have the text content "Download data"
	And the sub-navigation should be:
		| text          | href                                | current |
		| Download data | /local-authority/301/download-data/ | true    |
	And the side navigation should be:
		| text                               | href                                                               | current |
		| Pupil level and aggregated LA data | /local-authority/301/download-data/pupil-level-aggregated-la-data/ | true    |
		| Individual school data             | /local-authority/301/download-data/individual-school-data/         |         |
	And the element "#app-subpage-title" should have the text content "Pupil level and aggregated LA data Dates available for download"
	And the element "#app-subpage-title-caption" should have the text content "Pupil level and aggregated LA data"

@Javascript:disabled
Scenario: Data downloads 'Dates Available for Download' - page should show a breadcrumb trail
	Given Local Authority "301" exists:
	"""
	{
		"Name": "Test LA",
	}
	"""
	When I navigate to /local-authority/301/download-data/pupil-level-aggregated-la-data/
	Then I should get a 200 response
	And the page title should be "Download data | Analyse school performance"
	And the breadcrumb trail should be:
		| text                  | href                  | current |
		| Home                  | /                     |         |
		| All local authorities | /local-authorities/   |         |
		| Test LA               | /local-authority/301/ |         |
		| Download data         |                       | true    |

@Javascript:disabled
Scenario Outline: Data downloads 'Dates Available for Download' - page should contain three radio buttons
	Given Local Authority "301" exists:
	"""
	{
		"Name": "Test LA",
	}
	"""
	When I navigate to /local-authority/301/download-data/pupil-level-aggregated-la-data/
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
		"Name": "Test LA",
	}
	"""
	When I navigate to /local-authority/301/download-data/pupil-level-aggregated-la-data/
    And I click the button "*[data-testid='selectedYearSubmit']"
    Then the path should be /local-authority/301/download-data/pupil-level-aggregated-la-data/
    And the element "*[data-testid='app-error-summary'] h2" should have the text content "There is a problem"
    And the element "*[data-testid='app-error-summary-selectedYear']" should have the text content "Please choose an academic year to download"
    And the element "*[data-testid='app-error-summary-selectedYear']" should have the href "#app-field-selectedYear"
    And the element "*[data-testid='app-field-selectedYear-error']" should have the text content "Please choose an academic year to download"

@Javascript:disabled
Scenario: Data downloads 'Dates available for download' - when date is selected and Continue button clicked, should move to next step
	Given Local Authority "301" exists:
	"""
	{
		"Name": "Test LA",
	}
	"""
	When I navigate to /local-authority/301/download-data/pupil-level-aggregated-la-data/
    And I update the element "#app-available-downloads-dates-2022" to be checked
    And I click the button "*[data-testid='selectedYearSubmit']"
    Then the path should be /local-authority/301/download-data/pupil-level-aggregated-la-data/select-files/?selectedYear=2022

@Javascript:disabled
Scenario: Data downloads 'Data files available for download' - page should be accessible when a valid laCode
	Given Local Authority "301" exists:
	"""
	{
		"Name": "Test LA",
	}
	"""
	When I navigate to /local-authority/301/download-data/pupil-level-aggregated-la-data/select-files/?selectedYear=2022
	Then I should get a 200 response
	And the element "#app-page-title" should have the text content "Download data"
	And the sub-navigation should be:
		| text          | href           | current |
		| Download data | /local-authority/301/download-data/ | true    |
	And the side navigation should be:
		| text                               | href                                                               | current |
		| Pupil level and aggregated LA data | /local-authority/301/download-data/pupil-level-aggregated-la-data/ | true    |
		| Individual school data             | /local-authority/301/download-data/individual-school-data/         |         |
	And the element "#app-subpage-title" should have the text content "Pupil level and aggregated LA data Data files available for download"
	And the element "#app-subpage-title-caption" should have the text content "Pupil level and aggregated LA data"

@Javascript:disabled
Scenario: Data downloads 'Data files available for download' - page should show a breadcrumb trail
	Given Local Authority "301" exists:
	"""
	{
		"Name": "Test LA",
	}
	"""
	When I navigate to /local-authority/301/download-data/pupil-level-aggregated-la-data/select-files/?selectedYear=2022
	Then I should get a 200 response
	And the page title should be "Download data | Analyse school performance"
	And the breadcrumb trail should be:
		| text                              | href                                | current |
		| Home                              | /                                   |         |
		| All local authorities             | /local-authorities/                 |         |
		| Test LA                           | /local-authority/301/               |         |
		| Download data                     | /local-authority/301/download-data/ |         |
		| Data files available for download |                                     | true    |

@Javascript:disabled
Scenario Outline: Data downloads 'Data files available for download' - page should contain three checkbox groups
	Given Local Authority "301" exists:
	"""
	{
		"Name": "Test LA",
	}
	"""
	When I navigate to /local-authority/301/download-data/pupil-level-aggregated-la-data/select-files/?selectedYear=2022
	Then I should get a 200 response
	And the element "[data-testid='available-downloads-file-group-<group>']" should have the text content "<text>"
Examples:
  | group       | text        |
  | Key stage 2 | Key stage 2 |
  | Key stage 4 | Key stage 4 |
  | Phonics     | Phonics     |	
  
@Javascript:disabled
Scenario Outline: Data downloads 'Data files available for download' - page should contain five checkboxes
	Given Local Authority "301" exists:
	"""
	{
		"Name": "Test LA",
	}
	"""
    When I navigate to /local-authority/301/download-data/pupil-level-aggregated-la-data/select-files/?selectedYear=2022
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
		"Name": "Test LA",
	}
	"""
    When I navigate to /local-authority/301/download-data/pupil-level-aggregated-la-data/select-files/?selectedYear=2022
    And I click the button "*[data-testid='selectedFilesSubmit']"
    Then the path should be /local-authority/301/download-data/pupil-level-aggregated-la-data/select-files/?selectedYear=2022
    And the element "*[data-testid='app-error-summary'] h2" should have the text content "There is a problem"
    And the element "*[data-testid='app-error-summary-selectedFiles']" should have the text content "Please choose one or more data files to download"
    And the element "*[data-testid='app-error-summary-selectedFiles']" should have the href "#app-field-selectedFiles"
    And the element "*[data-testid='app-field-selectedFiles-error']" should have the text content "Please choose one or more data files to download"

@Javascript:disabled
Scenario: Data downloads 'Data files available for download' - when files are selected and Continue button clicked, should move to next step
	Given Local Authority "301" exists:
	"""
	{
		"Name": "Test LA",
	}
	"""
    When I navigate to /local-authority/301/download-data/pupil-level-aggregated-la-data/select-files/?selectedYear=2022
    And I update the element "#app-available-downloads-file-kts-301-ks2-la-2022-final" to be checked
    And I click the button "*[data-testid='selectedFilesSubmit']"
    Then the path should be /local-authority/301/download-data/pupil-level-aggregated-la-data/select-format/?selectedYear=2022&selectedFiles=kts-301-ks2-la-2022-final

@Javascript:disabled
Scenario: Data downloads 'Download pupil level and aggregated LA data' - page should be accessible with a valid laCode
	Given Local Authority "301" exists:
	"""
	{
		"Name": "Test LA",
	}
	"""
    When I navigate to /local-authority/301/download-data/pupil-level-aggregated-la-data/select-format/?selectedYear=2022&selectedFiles=kts-301-ks2-la-2022-final&selectedFiles=asp-301-ks2-la-2022-provisional
    Then I should get a 200 response
    And the element "#app-page-title" should have the text content "Download data"
    And the sub-navigation should be:
		| text          | href                                | current |
		| Download data | /local-authority/301/download-data/ | true    |
	And the side navigation should be:
		| text                               | href                                                               | current |
		| Pupil level and aggregated LA data | /local-authority/301/download-data/pupil-level-aggregated-la-data/ | true    |
		| Individual school data             | /local-authority/301/download-data/individual-school-data/         |         |
    And the element "#app-subpage-title" should have the text content "Pupil level and aggregated LA data Download pupil level and aggregated LA data"
    And the element "#app-subpage-title-caption" should have the text content "Pupil level and aggregated LA data"

@Javascript:disabled
Scenario: Data downloads 'Download pupil level and aggregated LA data' - page should show a breadcrumb trail
	Given Local Authority "301" exists:
	"""
	{
		"Name": "Test LA",
	}
	"""
	When I navigate to /local-authority/301/download-data/pupil-level-aggregated-la-data/select-format/?selectedYear=2022&selectedFiles=kts-004-phonics-la-2022-final-pupil
	Then I should get a 200 response
	And the page title should be "Download data | Analyse school performance"
	And the breadcrumb trail should be:
		| text                                        | href                                                                                              | current |
		| Home                                        | /                                                                                                 |         |
		| All local authorities                       | /local-authorities/                                                                               |         |
		| Test LA                                     | /local-authority/301/                                                                             |         |
		| Download data                               | /local-authority/301/download-data/                                                               |         |
		| Data files available for download           | /local-authority/301/download-data/pupil-level-aggregated-la-data/select-files/?selectedYear=2022 |         |
		| Download pupil level and aggregated LA data |                                                                                                   | true    |

@Javascript:disabled
Scenario Outline: Data downloads 'Download pupil level and aggregated LA data' - page should contain three links
	Given Local Authority "301" exists:
	"""
	{
		"Name": "Test LA",
	}
	"""
    When I navigate to /local-authority/301/download-data/pupil-level-aggregated-la-data/select-format/?selectedYear=2022&selectedFiles=kts-301-ks2-la-2022-final&selectedFiles=asp-301-ks2-la-2022-provisional
    Then I should get a 200 response
	And the available download formats should be:
    	| text               | href                                                                                                                                                   |
    	| Data in CSV format | /local-authority/301/download-data/download-as-zip/?fileType=CSV&selectedFiles=kts-301-ks2-la-2022-final&selectedFiles=asp-301-ks2-la-2022-provisional |

@Javascript:disabled
Scenario Outline: Data downloads 'Download school data' - Download other dates link should link back to first step
	Given Local Authority "301" exists:
	"""
	{
		"Name": "Test LA",
	}
	"""
	When I navigate to /local-authority/301/download-data/pupil-level-aggregated-la-data/select-format/?selectedYear=2022&selectedFiles=kts-004-phonics-la-2022-final-pupil
	Then the element "[data-testid="available-downloads-other-dates"]" should have the href "/local-authority/301/download-data/pupil-level-aggregated-la-data/"