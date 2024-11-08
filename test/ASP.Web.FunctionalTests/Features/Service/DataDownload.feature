Feature: Data downloads page

@Javascript:disabled
Scenario: Data downloads 'Dates available for download' page should be accessible when valid urn is provided (My school page)
    Given Establishment "136028" exists:
	"""
	{
        "name": "Dagenham Park CofE School"
    }
	"""
    And I am a School Named user for Establishment "136028"
    When I navigate to /my-school/download-data
    Then I should get a 200 response
    And the element "h1.govuk-heading-xl" should have the text content "Download data"
    And the element "[data-testid='sub-navigation-item-download-data']" should have the text content "Download data"
    And the element "[data-testid='sub-navigation-item-download-data'] a" should have the attribute "aria-current" set to "page"
    And the element "[data-testid='side-navigation-item-name']" should have the text content "Dagenham Park CofE School data"
    And the element "[data-testid='available-downloads-caption']" should have the text content "Dagenham Park CofE School data"
    And the element "[data-testid='available-downloads-dates-heading']" should have the text content "Dagenham Park CofE School data Dates available for download"
  

@Javascript:disabled
Scenario Outline: Data downloads 'Dates available for download' page should contain three radio buttons
    Given Establishment "136028" exists:
	"""
	{
        "name": "Dagenham Park CofE School"
    }
	"""
    And I am a School Named user for Establishment "136028"
    When I navigate to /my-school/download-data
    Then I should get a 200 response
    And the element "[data-testid='available-downloads-dates-<year>-label']" should have the text content "<label>"
Examples:
	| year | label        |
	| 2022 | 2021 to 2022 |
	| 2023 | 2022 to 2023 |
	| 2024 | 2023 to 2024 |



@Javascript:disabled
Scenario: Data downloads "Data files available for download' page should be accessible when valid urn is provided (My school page)
    Given Establishment "136028" exists:
	"""
	{
        "name": "Dagenham Park CofE School"
    }
	"""
    And I am a School Named user for Establishment "136028"
    When I navigate to /my-school/download-data/select-files/?selectedYear=2022
    Then I should get a 200 response
    And the element "h1.govuk-heading-xl" should have the text content "Download data"
    And the element "[data-testid='sub-navigation-item-download-data']" should have the text content "Download data"
    And the element "[data-testid='sub-navigation-item-download-data'] a" should have the attribute "aria-current" set to "page"
    And the element "[data-testid='side-navigation-item-name']" should have the text content "Dagenham Park CofE School data"
    And the element "[data-testid='available-downloads-caption']" should have the text content "Dagenham Park CofE School data"
    And the element "[data-testid='available-downloads-files-heading']" should have the text content "Dagenham Park CofE School data Data files available for download"

@Javascript:disabled
Scenario Outline: Data downloads 'Data files available for download' page should contain three checkbox groups
    Given Establishment "136028" exists:
	"""
	{
        "name": "Dagenham Park CofE School"
    }
	"""
    And I am a School Named user for Establishment "136028"
    When I navigate to /my-school/download-data/select-files/?selectedYear=2022
    Then I should get a 200 response
    And the element "[data-testid='available-downloads-file-group-<group>']" should have the text content "<text>"
Examples:
	| group       | text        |
	| Key stage 2 | Key stage 2 |
	| Key stage 4 | Key stage 4 |
	| Phonics     | Phonics     |

@Javascript:disabled
Scenario Outline: Data downloads 'Data files available for download' page should contain five checkboxes
    Given Establishment "136028" exists:
	"""
	{
        "name": "Dagenham Park CofE School"
    }
	"""
    And I am a School Named user for Establishment "136028"
    When I navigate to /my-school/download-data/select-files/?selectedYear=2022
    Then I should get a 200 response
    And the element "[data-testid='available-downloads-file-<fileid>-label']" should have the text content "<label>"
Examples:
	| fileid                                 | label                                                  |
	| kts-136028-ks2-2022-final-school       | Key stage 2 (Final) (Key to success)                   |
	| asp-136028-ks2-2022-provisional-school | Key stage 2 (Provisional) (Analyse school performance) |
	| kts-136028-ks4-2022-final-pupil        | Key stage 4 (Final) (Key to success)                   |
	| asp-136028-ks4-2022-final-pupil        | Key stage 4 (Final) (Analyse school performance)       |
	| kts-136028-phonics-2022-final-pupil    | Phonics (Final) (Key to success)                       |



@Javascript:disabled
Scenario: Data downloads "Download school data' page should be accessible when valid urn is provided (My school page)
    Given Establishment "136028" exists:
	"""
	{
        "name": "Dagenham Park CofE School"
    }
	"""
    And I am a School Named user for Establishment "136028"
    When I navigate to /my-school/download-data/select-format/?selectedYear=2022&selectedFiles=kts-800200-ks2-2022-final-school&selectedFiles=asp-800200-ks2-2022-provisional-school
    Then I should get a 200 response
    And the element "h1.govuk-heading-xl" should have the text content "Download data"
    And the element "[data-testid='sub-navigation-item-download-data']" should have the text content "Download data"
    And the element "[data-testid='sub-navigation-item-download-data'] a" should have the attribute "aria-current" set to "page"
    And the element "[data-testid='side-navigation-item-name']" should have the text content "Dagenham Park CofE School data"
    And the element "[data-testid='available-downloads-caption']" should have the text content "Dagenham Park CofE School data"
    And the element "[data-testid='available-downloads-format-heading']" should have the text content "Dagenham Park CofE School data Download Dagenham Park CofE School data"

@Javascript:disabled
Scenario Outline: Data downloads 'Data files available for download' page should contain three links
    Given Establishment "136028" exists:
	"""
	{
        "name": "Dagenham Park CofE School"
    }
	"""
    And I am a School Named user for Establishment "136028"
    When I navigate to /my-school/download-data/select-format/?selectedYear=2022&selectedFiles=kts-800200-ks2-2022-final-school&selectedFiles=asp-800200-ks2-2022-provisional-school
    Then I should get a 200 response
    And the element "#app-available-downloads-format-csv-link" should have the href "/my-school/download-data/download-as-zip/?fileType=CSV&selectedFiles=kts-800200-ks2-2022-final-school&selectedFiles=asp-800200-ks2-2022-provisional-school"
   
@Javascript:disabled
Scenario: The 'Dates Available for Download' page in Data Downloads should be accessible with a valid laCode on the My Local Authority Page
	Given Local Authority "301" exists:
	"""
		{
		    "Name": "Test Name",
		    "Code": "301"
		}
	"""
	And I am a LA Named user for Local Authority "301"
	When I navigate to /my-local-authority/download-data/
	Then I should get a 200 response
	And the element "h1.govuk-heading-xl" should have the text content "Download data"
	And the element "[data-testid='sub-navigation-item-download-data']" should have the text content "Download data"
	And the element "[data-testid='sub-navigation-item-download-data'] a" should have the attribute "aria-current" set to "page"
	And the element "[data-testid='side-navigation-item-pupil-level-and-aggregated-la-data']" should have the text content "Pupil level and aggregated LA data"
	And the element "[data-testid='available-downloads-caption']" should have the text content "Pupil level and aggregated LA data"
	And the element "[data-testid='available-downloads-dates-heading']" should have the text content "Pupil level and aggregated LA data Dates available for download"    
	
@Javascript:disabled
Scenario Outline: The 'Dates Available for Download' page in Data Downloads should contain three radio buttons on the My Local Authority page
	Given Local Authority "301" exists:
	"""
		{
		    "Name": "Test Name",
		    "Code": "301"
		}
	"""
	And I am a LA Named user for Local Authority "301"
	When I navigate to /my-local-authority/download-data/
	Then I should get a 200 response
	And the element "[data-testid='available-downloads-dates-<year>-label']" should have the text content "<label>"
Examples:
  | year | label        |
  | 2022 | 2021 to 2022 |
  | 2023 | 2022 to 2023 |
  | 2024 | 2023 to 2024 |	 
  

@Javascript:disabled
Scenario: The 'Data files available for download' page in Data Downloads should be accessible when a valid laCode on the My Local Authority Page
	Given Local Authority "301" exists:
	"""
		{
		    "Name": "Test Name",
		    "Code": "301"
		}
	"""
	And I am a LA Named user for Local Authority "301"
	When I navigate to /my-local-authority/download-data/select-files/?selectedYear=2022
	Then I should get a 200 response
	And the element "h1.govuk-heading-xl" should have the text content "Download data"
	And the element "[data-testid='sub-navigation-item-download-data']" should have the text content "Download data"
	And the element "[data-testid='sub-navigation-item-download-data'] a" should have the attribute "aria-current" set to "page"
	And the element "[data-testid='side-navigation-item-pupil-level-and-aggregated-la-data']" should have the text content "Pupil level and aggregated LA data"
	And the element "[data-testid='available-downloads-caption']" should have the text content "Pupil level and aggregated LA data"
	And the element "[data-testid='available-downloads-files-heading']" should have the text content "Pupil level and aggregated LA data Data files available for download"
	
@Javascript:disabled
Scenario Outline: The 'Data files available for download' page in Data Downloads should contain three checkbox groups on the My Local Authority page
	Given Local Authority "301" exists:
	"""
		{
		    "Name": "Test Name",
		    "Code": "301"
		}
	"""
	And I am a LA Named user for Local Authority "301"
	When I navigate to /my-local-authority/download-data/select-files/?selectedYear=2022
	Then I should get a 200 response
	And the element "[data-testid='available-downloads-file-group-<group>']" should have the text content "<text>"
Examples:
  | group       | text        |
  | Key stage 2 | Key stage 2 |
  | Key stage 4 | Key stage 4 |
  | Phonics     | Phonics     |	
  
@Javascript:disabled
Scenario Outline: The 'Data files available for download' page in Data Downloads should contain five checkboxes on the My Local Authority page
	Given Local Authority "301" exists:
	"""
		{
		    "Name": "Test Name",
		    "Code": "301"
		}
	"""
	And I am a LA Named user for Local Authority "301"
    When I navigate to /my-local-authority/download-data/select-files/?selectedYear=2022
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
Scenario: The 'Download pupil level and aggregated LA data' page in Data Downloads should be accessible with a valid laCode on the My Local Authority Page
	Given Local Authority "301" exists:
	"""
		{
		    "Name": "Test Name",
		    "Code": "301"
		}
	"""
	And I am a LA Named user for Local Authority "301"
    When I navigate to /my-local-authority/download-data/select-format/?selectedYear=2022&selectedFiles=kts-301-ks2-la-2022-final&selectedFiles=asp-301-ks2-la-2022-provisional
    Then I should get a 200 response
    And the element "h1.govuk-heading-xl" should have the text content "Download data"
    And the element "[data-testid='sub-navigation-item-download-data']" should have the text content "Download data"
    And the element "[data-testid='sub-navigation-item-download-data'] a" should have the attribute "aria-current" set to "page"
    And the element "[data-testid='side-navigation-item-pupil-level-and-aggregated-la-data']" should have the text content "Pupil level and aggregated LA data"
    And the element "[data-testid='available-downloads-caption']" should have the text content "Pupil level and aggregated LA data"
    And the element "[data-testid='available-downloads-format-heading']" should have the text content "Pupil level and aggregated LA data Download pupil level and aggregated LA data"

@Javascript:disabled
Scenario Outline: The 'Data files available for download' page in Data Downloads should contain three links on the My Local Authority Page
	Given Local Authority "301" exists:
	"""
		{
		    "Name": "Test Name",
		    "Code": "301"
		}
	"""
	And I am a LA Named user for Local Authority "301"
    When I navigate to /my-local-authority/download-data/select-format/?selectedYear=2022&selectedFiles=kts-301-ks2-la-2022-final&selectedFiles=asp-301-ks2-la-2022-provisional
    Then I should get a 200 response
    And the element "#app-available-downloads-format-csv-link" should have the href "/my-local-authority/download-data/download-as-zip/?fileType=CSV&selectedFiles=kts-301-ks2-la-2022-final&selectedFiles=asp-301-ks2-la-2022-provisional"
 
@Javascript:disabled
Scenario: The 'Dates Available for Download' page in Data Downloads should show a breadcrumb trail on the My Local Authority Page
	Given Local Authority "301" exists:
	"""
		{
		    "Name": "Test Name",
		    "Code": "301"
		}
	"""
	And I am a LA Named user for Local Authority "301"
	When I navigate to /my-local-authority/download-data/
	Then I should get a 200 response
	And the page title should be "Download data | Analyse school performance"
	And the element "[data-testid='breadcrumb-home']" should have the href "/"
	And the element "[data-testid='breadcrumb-my-local-authority']" should have the text content "My local authority"
	And the element "[data-testid='breadcrumb-my-local-authority']" should have the href "/my-local-authority/"
	And the element "[data-testid='breadcrumb-current-page']" should have the text content "Download data"  
	     
@Javascript:disabled
Scenario: The 'Data files available for download' page in Data Downloads should show a breadcrumb trail on the My Local Authority Page
	Given Local Authority "301" exists:
	"""
		{
		    "Name": "Test Name",
		    "Code": "301"
		}
	"""
	And I am a LA Named user for Local Authority "301"
	When I navigate to /my-local-authority/download-data/select-files/?selectedYear=2022
	Then I should get a 200 response
	And the page title should be "Download data | Analyse school performance"
	And the element "[data-testid='breadcrumb-home']" should have the href "/"
	And the element "[data-testid='breadcrumb-my-local-authority']" should have the text content "My local authority"
	And the element "[data-testid='breadcrumb-my-local-authority']" should have the href "/my-local-authority/"
	And the element "[data-testid='breadcrumb-download-data']" should have the text content "Download data"
	And the element "[data-testid='breadcrumb-download-data']" should have the href "/my-local-authority/download-data/"
	And the element "[data-testid='breadcrumb-current-page']" should have the text content "Data files available for download" 
	
@Javascript:disabled
Scenario: The 'Download pupil level and aggregated LA data' page in Data Downloads should show a breadcrumb trail on the My Local Authority Page
	Given Local Authority "301" exists:
	"""
		{
		    "Name": "Test Name",
		    "Code": "301"
		}
	"""
	And I am a LA Named user for Local Authority "301"
	When I navigate to /my-local-authority/download-data/select-format/?selectedYear=2022&selectedFiles=kts-004-phonics-la-2022-final-pupil
	Then I should get a 200 response
	And the page title should be "Download data | Analyse school performance"
	And the element "[data-testid='breadcrumb-home']" should have the href "/"
	And the element "[data-testid='breadcrumb-my-local-authority']" should have the text content "My local authority"
	And the element "[data-testid='breadcrumb-my-local-authority']" should have the href "/my-local-authority/"
	And the element "[data-testid='breadcrumb-download-data']" should have the text content "Download data"
	And the element "[data-testid='breadcrumb-download-data']" should have the href "/my-local-authority/download-data/"
	And the element "[data-testid='breadcrumb-data-files-available-for-download']" should have the text content "Data files available for download"
	And the element "[data-testid='breadcrumb-data-files-available-for-download']" should have the href "/my-local-authority/download-data/select-files/?selectedYear=2022"
	And the element "[data-testid='breadcrumb-current-page']" should have the text content "Download pupil level and aggregated LA data"	   