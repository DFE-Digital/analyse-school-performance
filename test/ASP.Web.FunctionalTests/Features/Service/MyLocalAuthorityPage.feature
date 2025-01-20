Feature: My local authority page

Background:
	Given I am a LA Named user for Local Authority "301"

@Javascript:disabled
Scenario: A non-LA user should not be able to access the 'My local authority' page. Instead, they should see a 403 Access not allowed page.
	Given I am a MAT Named user for Multi-Academy Trust "1234"
	When I navigate to /my-local-authority/
	Then I should get a 403 response
	And the page title should be "Access not allowed"
	And the element "h1.govuk-heading-l" should have the text content "Access not allowed"

@Javascript:disabled
Scenario: A user with access to all LAs should not be able to access the 'My local authority' page. Instead, they should see a 403 Access not allowed page.
	Given I am a DfE Named user
	When I navigate to /my-local-authority/
	Then I should get a 403 response
	And the page title should be "Access not allowed"
	And the element "h1.govuk-heading-l" should have the text content "Access not allowed"

@Javascript:disabled
Scenario: Should return (200) response if the LA Named user accesses /my-local-authority/download-data
	Given I am a LA Named user for Local Authority "301"
	And Local Authority "301" exists:
		"""
		{
		    "name": "Test LA"
		}
		"""
	And blob storage file downloads-config.json exists in config container:
		"""
		[
		    {
		        "id": "kts-la-ks2-pupil",
		        "source": "KTS",
		        "scope": "LocalAuthority",
		        "dataSetType": "KeyStage2",
		        "label": "Key stage 2 (KS2)",
		        "filePathPattern": "LA/{code}/{year}/{filetype}/ks2_pupil_{version}.{filetype}"
		    },
		    {
		        "id": "kts-school-ks2-pupil",
		        "source": "KTS",
		        "scope": "School",
		        "dataSetType": "KeyStage2",
		        "label": "Key stage 2 (KS2)",
		        "filePathPattern": "School/{code}/{year}/{filetype}/ks2_pupil_{version}.{filetype}"
		    }
		]
		"""
	And blob storage file LA/301/2022/csv/ks2_pupil_final.csv exists in downloads-kts container:
		"""
		Column A,Column B,Column C
		1,2,3
		"""
	When I navigate to /my-local-authority/download-data
	Then I should get a 200 response


@Javascript:disabled
Scenario Outline: Should return (200) response if DfE Named or Super Admin user accesses /local-authority/301/download-data
	Given I am a <userRole>
	And Local Authority "301" exists:
		"""
		{
		    "name": "Test LA"
		}
		"""
	And blob storage file downloads-config.json exists in config container:
		"""
		[
		    {
		        "id": "kts-la-ks2-pupil",
		        "source": "KTS",
		        "scope": "LocalAuthority",
		        "dataSetType": "KeyStage2",
		        "label": "Key stage 2 (KS2)",
		        "filePathPattern": "LA/{code}/{year}/{filetype}/ks2_pupil_{version}.{filetype}"
		    },
		    {
		        "id": "kts-school-ks2-pupil",
		        "source": "KTS",
		        "scope": "School",
		        "dataSetType": "KeyStage2",
		        "label": "Key stage 2 (KS2)",
		        "filePathPattern": "School/{code}/{year}/{filetype}/ks2_pupil_{version}.{filetype}"
		    }
		]
		"""
	And blob storage file LA/301/2022/csv/ks2_pupil_final.csv exists in downloads-kts container:
		"""
		Column A,Column B,Column C
		1,2,3
		"""
	When I navigate to /local-authority/301/download-data
	Then I should get a 200 response
Examples:
	| userRole         |
	| DfE Named user   |
	| Super Admin user |

@Javascript:disabled
Scenario Outline: Should return (403) response if the below mentioned user roles access /my-local-authority/download-data
	Given I am a <userRole>
	And Local Authority "301" exists:
		"""
		{
		    "name": "Test LA"
		}
		"""
	And blob storage file downloads-config.json exists in config container:
		"""
		[
		    {
		        "id": "kts-la-ks2-pupil",
		        "source": "KTS",
		        "scope": "LocalAuthority",
		        "dataSetType": "KeyStage2",
		        "label": "Key stage 2 (KS2)",
		        "filePathPattern": "LA/{code}/{year}/{filetype}/ks2_pupil_{version}.{filetype}"
		    },
		    {
		        "id": "kts-school-ks2-pupil",
		        "source": "KTS",
		        "scope": "School",
		        "dataSetType": "KeyStage2",
		        "label": "Key stage 2 (KS2)",
		        "filePathPattern": "School/{code}/{year}/{filetype}/ks2_pupil_{version}.{filetype}"
		    }
		]
		"""
	And blob storage file LA/301/2022/csv/ks2_pupil_final.csv exists in downloads-kts container:
		"""
		Column A,Column B,Column C
		1,2,3
		"""
	When I navigate to /my-local-authority/download-data
	Then I should get a 403 response
Examples:
	| userRole                                         |
	| LA Unnamed user for Local Authority "301"        |
	| MAT Unnamed user for Multi-Academy Trust "1234"  |
	| MAT Governor user for Multi-Academy Trust "1234" |
	| Diocese Unnamed user for Diocese "Test Diocese"  |
	| School Unnamed user for Establishment "123456"   |
	| School Governor user for Establishment "123456"  |
	| DfE Unnamed user                                 |
	| Ofsted Unnamed user                              |


@Javascript:disabled
Scenario Outline: Should return (403) response if the below mentioned user roles access /local-authority/301/download-data
	Given I am a <userRole>
	And Local Authority "301" exists:
		"""
		{
		    "name": "Test LA"
		}
		"""
	And blob storage file downloads-config.json exists in config container:
		"""
		[
		    {
		        "id": "kts-la-ks2-pupil",
		        "source": "KTS",
		        "scope": "LocalAuthority",
		        "dataSetType": "KeyStage2",
		        "label": "Key stage 2 (KS2)",
		        "filePathPattern": "LA/{code}/{year}/{filetype}/ks2_pupil_{version}.{filetype}"
		    },
		    {
		        "id": "kts-school-ks2-pupil",
		        "source": "KTS",
		        "scope": "School",
		        "dataSetType": "KeyStage2",
		        "label": "Key stage 2 (KS2)",
		        "filePathPattern": "School/{code}/{year}/{filetype}/ks2_pupil_{version}.{filetype}"
		    }
		]
		"""
	And blob storage file LA/301/2022/csv/ks2_pupil_final.csv exists in downloads-kts container:
		"""
		Column A,Column B,Column C
		1,2,3
		"""
	When I navigate to /local-authority/301/download-data
	Then I should get a 403 response
Examples:
	| userRole                                         |
	| LA Named user for Local Authority "301"          |
	| LA Unnamed user for Local Authority "301"        |
	| MAT Named user for Multi-Academy Trust "1234"    |
	| MAT Unnamed user for Multi-Academy Trust "1234"  |
	| MAT Governor user for Multi-Academy Trust "1234" |
	| Diocese Named user for Diocese "Test Diocese"    |
	| Diocese Unnamed user for Diocese "Test Diocese"  |
	| School Named user for Establishment "123456"     |
	| School Unnamed user for Establishment "123456"   |
	| School Governor user for Establishment "123456"  |
	| DfE Unnamed user                                 |
	| Ofsted Unnamed user                              |
	
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
	And the page title should be "Sorry, there is a problem with the service"
	And the element "h1.govuk-heading-l" should have the text content "Sorry, there is a problem with the service"
	And the element "*[data-testid='error-display-message']" should have the text content "Error message: API error: /api/GetLocalAuthority Could not find Local Authority with code "302"."
		
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
	And the page title should be "My local authority"
	And the page subtitle should be "All schools within Test Name"
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
Scenario: LA Named user should see Download data card on LA landing page
	Given Local Authority "301" exists:
		"""
			{
				"Name": "Test Name"
			}
		"""
	And I am an LA Named user for Local Authority "301"
	And Content Template "la-landing-page" exists:
		"""
		{
		    "Views": [
		        {
		            "ViewId": "Card",
		            "ViewContent": {
		                "AuthorizationPolicy": "AccessToAllSchools",
		                "Title": "All schools",
		                "LinkUrl": "schools/",
		                "Text": "All schools found in this LA."
		            }
		        },
		        {
		            "ViewId": "Card",
		            "ViewContent": {
		                "AuthorizationPolicy": "AccessToMySchools",
		                "Title": "My schools",
		                "LinkUrl": "/my-schools/",
		                "Text": "All schools found in this LA."
		            }
		        },
		        {
		            "ViewId": "Card",
		            "ViewContent": {
		                "AuthorizationPolicy": "NamedData",
		                "Title": "Download data",
		                "LinkUrl": "download-data/",
		                "Text": "Download data for Analyse school performance and Key to success."
		            }
		        }
		    ]
		}
		"""
	When I navigate to /my-local-authority/
	Then I should get a 200 response
	And the landing page cards should be:
		| Title         | Url            | Content                                                          |
		| My schools    | /my-schools/   | All schools found in this LA.                                    |
		| Download data | download-data/ | Download data for Analyse school performance and Key to success. |
   
@Javascript:disabled
Scenario: LA Unnamed user should not see Download data card on LA landing page
	Given Local Authority "301" exists:
		"""
			{
				"Name": "Test Name"
			}
		"""
	And I am an LA Unnamed user for Local Authority "301"
	And Content Template "la-landing-page" exists:
		"""
		{
		    "Views": [
		        {
		            "ViewId": "Card",
		            "ViewContent": {
		                "AuthorizationPolicy": "AccessToAllSchools",
		                "Title": "All schools",
		                "LinkUrl": "schools/",
		                "Text": "All schools found in this LA."
		            }
		        },
		        {
		            "ViewId": "Card",
		            "ViewContent": {
		                "AuthorizationPolicy": "AccessToMySchools",
		                "Title": "My schools",
		                "LinkUrl": "/my-schools/",
		                "Text": "All schools found in this LA."
		            }
		        },
		        {
		            "ViewId": "Card",
		            "ViewContent": {
		                "AuthorizationPolicy": "NamedData",
		                "Title": "Download data",
		                "LinkUrl": "download-data/",
		                "Text": "Download data for Analyse school performance and Key to success."
		            }
		        }
		    ]
		}
		"""
	When I navigate to /my-local-authority/
	Then I should get a 200 response
	And the landing page cards should be:
		| Title      | Url          | Content                       |
		| My schools | /my-schools/ | All schools found in this LA. |


@Javascript:disabled
Scenario: Data downloads > Pupil level and aggregated LA data > Dates available for Download - No data files available
	Given Local Authority "301" exists:
		"""
		{
			"Name": "Test Name",
			"Code": "301"
		}
		"""
	And blob storage file downloads-config.json exists in config container:
		"""
		[
			{
				"id": "kts-la-ks2-pupil",
				"source": "KTS",
				"scope": "LocalAuthority",
				"dataSetType": "KeyStage2",
				"label": "Key stage 2 (KS2)",
				"filePathPattern": "LA/{code}/{year}/{filetype}/ks2_pupil_{version}.{filetype}"
			}
		]
		"""
	And I am a LA Named user for Local Authority "301"
	When I navigate to /my-local-authority/download-data/pupil-level-aggregated-la-data/
	Then I should get a 200 response
	And the page title should be "Download data"
	And the sub-page title should be "We could not find any data downloads" with caption "Pupil level and aggregated LA data"
	And the element "[data-testid="no-downloads-description"]" should have the text content "There were no data files available to download for your LA."
	And the element "[data-testid="no-downloads-other-dates"]" should not exist
	And the breadcrumb trail should be:
		| text                                 | href                               | current |
		| Home                                 | /                                  |         |
		| My local authority                   | /my-local-authority/               |         |
		| Download data                        | /my-local-authority/download-data/ |         |
		| We could not find any data downloads |                                    | true    |
	
@Javascript:disabled
Scenario: Data downloads > Pupil level and aggregated LA data > Dates available for Download - Common page elements 
	Given Local Authority "301" exists:
		"""
		{
			"Name": "Test Name",
			"Code": "301"
		}
		"""
	And blob storage file downloads-config.json exists in config container:
		"""
		[
				{
				"id": "kts-la-ks2-pupil",
				"source": "KTS",
				"scope": "LocalAuthority",
				"dataSetType": "KeyStage2",
				"label": "Key stage 2 (KS2)",
				"filePathPattern": "LA/{code}/{year}/{filetype}/ks2_pupil_{version}.{filetype}"
				}
		]
		"""
	And blob storage file LA/301/2022/csv/ks2_pupil_final.csv exists in downloads-kts container:
		"""
		Column A,Column B,Column C
		1,2,3
		"""
	And I am a LA Named user for Local Authority "301"
	When I navigate to /my-local-authority/download-data/pupil-level-aggregated-la-data/
	Then I should get a 200 response
	And the page title should be "Download data"
	And the breadcrumb trail should be:
		| text                         | href                               | current |
		| Home                         | /                                  |         |
		| My local authority           | /my-local-authority/               |         |
		| Download data                | /my-local-authority/download-data/ |         |
		| Dates available for download |                                    | true    |
	And the sub-navigation should be:
		| text          | href                               | current |
		| Download data | /my-local-authority/download-data/ | true    |
	And the side navigation should be:
		| text                               | href                                                              | current |
		| Pupil level and aggregated LA data | /my-local-authority/download-data/pupil-level-aggregated-la-data/ | true    |
		| Individual school data             | /my-local-authority/download-data/individual-school-data/         |         |
	And the sub-page title should be "Dates available for download" with caption "Pupil level and aggregated LA data"

@Javascript:disabled
Scenario Outline: Data downloads > Pupil level and aggregated LA data > Dates Available for Download - Page should contain multiple radio buttons
	Given Local Authority "301" exists:
		"""
		{
			"Name": "Test Name",
			"Code": "301"
		}
		"""
	And blob storage file downloads-config.json exists in config container:
		"""
		[
			{
				"id": "kts-la-ks2-pupil",
				"source": "KTS",
				"scope": "LocalAuthority",
				"dataSetType": "KeyStage2",
				"label": "Key stage 2 (KS2)",
				"filePathPattern": "LA/{code}/{year}/{filetype}/ks2_pupil_{version}.{filetype}"
			}
		]
		"""
	And blob storage file LA/301/2022/csv/ks2_pupil_final.csv exists in downloads-kts container:
		"""
		Column A,Column B,Column C
		1,2,3
		"""
	And blob storage file LA/301/2023/csv/ks2_pupil_final.csv exists in downloads-kts container:
		"""
		Column A,Column B,Column C
		1,2,3
		"""
	And blob storage file LA/301/2024/csv/ks2_pupil_final.csv exists in downloads-kts container:
		"""
		Column A,Column B,Column C
		1,2,3
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
Scenario: Data downloads > Pupil level and aggregated LA data > Dates available for download - When no date is selected and Continue button clicked, should show validation error
	Given Local Authority "301" exists:
		"""
		{
			"Name": "Test Name",
			"Code": "301"
		}
		"""
	And blob storage file downloads-config.json exists in config container:
		"""
		[
			{
				"id": "kts-la-ks2-pupil",
				"source": "KTS",
				"scope": "LocalAuthority",
				"dataSetType": "KeyStage2",
				"label": "Key stage 2 (KS2)",
				"filePathPattern": "LA/{code}/{year}/{filetype}/ks2_pupil_{version}.{filetype}"
			}
		]
		"""
	And blob storage file LA/301/2022/csv/ks2_pupil_final.csv exists in downloads-kts container:
		"""
		Column A,Column B,Column C
		1,2,3
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
Scenario: Data downloads > Pupil level and aggregated LA data > Dates available for download - When date is selected and Continue button clicked, should move to next step
	Given Local Authority "301" exists:
		"""
		{
			"Name": "Test Name",
			"Code": "301"
		}
		"""
	And blob storage file downloads-config.json exists in config container:
		"""
		[
			{
				"id": "kts-la-ks2-pupil",
				"source": "KTS",
				"scope": "LocalAuthority",
				"dataSetType": "KeyStage2",
				"label": "Key stage 2 (KS2)",
				"filePathPattern": "LA/{code}/{year}/{filetype}/ks2_pupil_{version}.{filetype}"
			}
		]
		"""
	And blob storage file LA/301/2022/csv/ks2_pupil_final.csv exists in downloads-kts container:
		"""
		Column A,Column B,Column C
		1,2,3
		"""
	And I am a LA Named user for Local Authority "301"
	When I navigate to /my-local-authority/download-data/pupil-level-aggregated-la-data/
	And I update the element "#app-available-downloads-dates-2022" to be checked
	And I click the button "*[data-testid='selectedYearSubmit']"
	Then the path should be /my-local-authority/download-data/pupil-level-aggregated-la-data/select-files/?selectedYear=2022

@Javascript:disabled
Scenario: Data downloads > Pupil level and aggregated LA data > Data files available for download - No data files available
	Given Local Authority "301" exists:
		"""
		{
			"Name": "Test Name",
			"Code": "301"
		}
		"""
	And blob storage file downloads-config.json exists in config container:
		"""
		[
			{
				"id": "kts-la-ks2-pupil",
				"source": "KTS",
				"scope": "LocalAuthority",
				"dataSetType": "KeyStage2",
				"label": "Key stage 2 (KS2)",
				"filePathPattern": "LA/{code}/{year}/{filetype}/ks2_pupil_{version}.{filetype}"
			}
		]
		"""
	And I am a LA Named user for Local Authority "301"
	When I navigate to /my-local-authority/download-data/pupil-level-aggregated-la-data/select-files/?selectedYear=2022
	Then I should get a 200 response
	And the page title should be "Download data"
	And the sub-page title should be "We could not find any data downloads" with caption "Pupil level and aggregated LA data"
	And the element "[data-testid="no-downloads-description"]" should have the text content "There were no data files available to download for your LA for 2022."
	And the element "[data-testid="no-downloads-other-dates"]" should have the href "/my-local-authority/download-data/pupil-level-aggregated-la-data/"
	And the breadcrumb trail should be:
		| text                                 | href                                                              | current |
		| Home                                 | /                                                                 |         |
		| My local authority                   | /my-local-authority/                                              |         |
		| Download data                        | /my-local-authority/download-data/                                |         |
		| Dates available for download         | /my-local-authority/download-data/pupil-level-aggregated-la-data/ |         |
		| We could not find any data downloads |                                                                   | true    |

@Javascript:disabled
Scenario: Data downloads > Pupil level and aggregated LA data > Data files available for download - Common page elements
	Given Local Authority "301" exists:
		"""
			{
				"Name": "Test Name",
				"Code": "301"
			}
		"""
	And blob storage file downloads-config.json exists in config container:
		"""
		[
			{
				"id": "kts-la-ks2-pupil",
				"source": "KTS",
				"scope": "LocalAuthority",
				"dataSetType": "KeyStage2",
				"label": "Key stage 2 (KS2)",
				"filePathPattern": "LA/{code}/{year}/{filetype}/ks2_pupil_{version}.{filetype}"
			}
		]
		"""
	And blob storage file LA/301/2022/csv/ks2_pupil_final.csv exists in downloads-kts container:
		"""
		Column A,Column B,Column C
		1,2,3
		"""
	And I am a LA Named user for Local Authority "301"
	When I navigate to /my-local-authority/download-data/pupil-level-aggregated-la-data/select-files/?selectedYear=2022
	Then I should get a 200 response
	And the page title should be "Download data"
	And the breadcrumb trail should be:
		| text                              | href                                                              | current |
		| Home                              | /                                                                 |         |
		| My local authority                | /my-local-authority/                                              |         |
		| Download data                     | /my-local-authority/download-data/                                |         |
		| Dates available for download      | /my-local-authority/download-data/pupil-level-aggregated-la-data/ |         |
		| Data files available for download |                                                                   | true    |
	And the sub-navigation should be:
		| text          | href                               | current |
		| Download data | /my-local-authority/download-data/ | true    |
	And the side navigation should be:
		| text                               | href                                                              | current |
		| Pupil level and aggregated LA data | /my-local-authority/download-data/pupil-level-aggregated-la-data/ | true    |
		| Individual school data             | /my-local-authority/download-data/individual-school-data/         |         |
	And the sub-page title should be "Data files available for download" with caption "Pupil level and aggregated LA data"	

@Javascript:disabled
Scenario Outline: Data downloads > Pupil level and aggregated LA data > Data files available for download - Page should contain multiple checkbox groups
	Given Local Authority "301" exists:
		"""
		{
			"Name": "Test Name",
		}
		"""
	And blob storage file downloads-config.json exists in config container:
		"""
		[
			 {
				"id": "kts-la-ks4-pupil",
				"source": "KTS",
				"scope": "LocalAuthority",
				"dataSetType": "KeyStage4",
				"label": "Key stage 4 (KS4)",
				"filePathPattern": "LA/{code}/{year}/{filetype}/ks4_pupil_{version}.{filetype}"
				},
				{
				"id": "kts-la-ks2-pupil",
				"source": "KTS",
				"scope": "LocalAuthority",
				"dataSetType": "KeyStage2",
				"label": "Key stage 2 (KS2)",
				"filePathPattern": "LA/{code}/{year}/{filetype}/ks2_pupil_{version}.{filetype}"
				}
		]
		"""
	And blob storage file LA/301/2022/csv/ks2_pupil_final.csv exists in downloads-kts container:
		"""
		Column A,Column B,Column C
		1,2,3
		"""
	And blob storage file LA/301/2022/csv/ks4_pupil_final.csv exists in downloads-kts container:
		"""
		Column A,Column B,Column C
		1,2,3
		"""
	And I am a LA Named user for Local Authority "301"
	When I navigate to /my-local-authority/download-data/pupil-level-aggregated-la-data/select-files/?selectedYear=2022
	Then I should get a 200 response
	And the element "[data-testid='available-downloads-file-group-<group>']" should have the text content "<text>"
Examples:
	| group             | text              |
	| Key stage 2 (KS2) | Key stage 2 (KS2) |
	| Key stage 4 (KS4) | Key stage 4 (KS4) |
	
@Javascript:disabled
Scenario Outline: Data downloads > Pupil level and aggregated LA data > Data files available for download - Page should contain multiple checkboxes
	Given Local Authority "301" exists:
		"""
		{
			"Name": "Test Name",
		}
		"""
	And blob storage file downloads-config.json exists in config container:
		"""
		[
				{
				"id": "asp-la-mtc-pupil",
				"source": "ASP",
				"scope": "LocalAuthority",
				"dataSetType": "MTC",
				"label": "Test",
				"filePathPattern": "LA/{code}/{year}/MTC/mtc_pupil_{version}.{filetype}"
				},
				{
				"id": "kts-la-ks2-pupil",
				"source": "KTS",
				"scope": "LocalAuthority",
				"dataSetType": "KeyStage2",
				"label": "Key stage 2 (KS2)",
				"filePathPattern": "LA/{code}/{year}/{filetype}/ks2_pupil_{version}.{filetype}"
				},
				{
				"id": "kts-la-ks4-pupil",
				"source": "KTS",
				"scope": "LocalAuthority",
				"dataSetType": "KeyStage4",
				"label": "Key stage 4 (KS4)",
				"filePathPattern": "LA/{code}/{year}/{filetype}/ks4_pupil_{version}.{filetype}"
				}
		]
		"""
	And blob storage file LA/301/2022/csv/ks2_pupil_final.csv exists in downloads-kts container:
		"""
		Column A,Column B,Column C
		1,2,3
		"""
	And blob storage file LA/301/2022/csv/ks4_pupil_final.csv exists in downloads-kts container:
		"""
		Column A,Column B,Column C
		1,2,3
		"""
	And blob storage file LA/301/2022/mtc/mtc_pupil_final.csv exists in downloads-asp container:
		"""
		Column A,Column B,Column C
		1,2,3
		"""
	And I am a LA Named user for Local Authority "301"
	When I navigate to /my-local-authority/download-data/pupil-level-aggregated-la-data/select-files/?selectedYear=2022
	Then I should get a 200 response
	And the element "[data-testid='available-downloads-file-<fileid>-label']" should have the text content "<label>"

Examples:
	| fileid                          | label                                                                 |
	| kts-la-ks2-pupil-301-2022-final | Key stage 2 (KS2) (Final) (Key to success)                            |
	| kts-la-ks4-pupil-301-2022-final | Key stage 4 (KS4) (Final) (Key to success)                            |
	| asp-la-mtc-pupil-301-2022-final | Multiplication table check (MTC) (Final) (Analyse school performance) |

@Javascript:disabled
Scenario: Data downloads > Pupil level and aggregated LA data > Data files available for download - When no files are selected and Continue button clicked, should show validation error
	Given Local Authority "301" exists:
		"""
		{
			"Name": "Test Name",
		}
		"""
	And blob storage file downloads-config.json exists in config container:
		"""
		[
				{
				"id": "kts-la-ks2-pupil",
				"source": "KTS",
				"scope": "LocalAuthority",
				"dataSetType": "KeyStage2",
				"label": "Key stage 2 (KS2)",
				"filePathPattern": "LA/{code}/{year}/{filetype}/ks2_pupil_{version}.{filetype}"
				}
		]
		"""
	And blob storage file LA/301/2022/csv/ks2_pupil_final.csv exists in downloads-kts container:
		"""
		Column A,Column B,Column C
		1,2,3
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
Scenario: Data downloads > Pupil level and aggregated LA data > Data files available for download - When files are selected and Continue button clicked, should move to next step
	Given Local Authority "301" exists:
		"""
		{
			"Name": "Test Name",
		}
		"""
	And blob storage file downloads-config.json exists in config container:
		"""
		[
				{
				"id": "kts-la-ks2-pupil",
				"source": "KTS",
				"scope": "LocalAuthority",
				"dataSetType": "KeyStage2",
				"label": "Key stage 2 (KS2)",
				"filePathPattern": "LA/{code}/{year}/{filetype}/ks2_pupil_{version}.{filetype}"
				}
		]
		"""
	And blob storage file LA/301/2022/csv/ks2_pupil_final.csv exists in downloads-kts container:
		"""
		Column A,Column B,Column C
		1,2,3
		"""
	And I am a LA Named user for Local Authority "301"
	When I navigate to /my-local-authority/download-data/pupil-level-aggregated-la-data/select-files/?selectedYear=2022
	And I update the element "#app-available-downloads-file-kts-la-ks2-pupil-301-2022-final" to be checked
	And I click the button "*[data-testid='selectedFilesSubmit']"
	Then the path should be /my-local-authority/download-data/pupil-level-aggregated-la-data/select-format/?selectedYear=2022&selectedFiles=kts-la-ks2-pupil-301-2022-final

@Javascript:disabled
Scenario: Data downloads > Pupil level and aggregated LA data > Download data - Common page elements
	Given Local Authority "301" exists:
		"""
		{
			"Name": "Test Name",
		}
		"""
	And I am a LA Named user for Local Authority "301"
	When I navigate to /my-local-authority/download-data/pupil-level-aggregated-la-data/select-format/?selectedYear=2022&selectedFiles=kts-301-ks2-la-2022-final&selectedFiles=asp-301-ks2-la-2022-provisional
	Then I should get a 200 response
	And the page title should be "Download data"
	And the breadcrumb trail should be:
		| text                                        | href                                                                                             | current |
		| Home                                        | /                                                                                                |         |
		| My local authority                          | /my-local-authority/                                                                             |         |
		| Download data                               | /my-local-authority/download-data/                                                               |         |
		| Dates available for download                | /my-local-authority/download-data/pupil-level-aggregated-la-data/                                |         |
		| Data files available for download           | /my-local-authority/download-data/pupil-level-aggregated-la-data/select-files/?selectedYear=2022 |         |
		| Download pupil level and aggregated LA data |                                                                                                  | true    |
	And the sub-navigation should be:
		| text          | href                               | current |
		| Download data | /my-local-authority/download-data/ | true    |
	And the side navigation should be:
		| text                               | href                                                              | current |
		| Pupil level and aggregated LA data | /my-local-authority/download-data/pupil-level-aggregated-la-data/ | true    |
		| Individual school data             | /my-local-authority/download-data/individual-school-data/         |         |
	And the sub-page title should be "Download pupil level and aggregated LA data" with caption "Pupil level and aggregated LA data"

@Javascript:disabled
Scenario Outline: Data downloads > Pupil level and aggregated LA data > Download data - Page should contain three links
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
		| text                | href                                                                                                                                                                                  |
		| Data in CSV format  | /my-local-authority/download-data/pupil-level-aggregated-la-data/download-as-zip/?fileType=CSV&selectedFiles=kts-301-ks2-la-2022-final&selectedFiles=asp-301-ks2-la-2022-provisional  |
		| Data in XLSX format | /my-local-authority/download-data/pupil-level-aggregated-la-data/download-as-zip/?fileType=XLSX&selectedFiles=kts-301-ks2-la-2022-final&selectedFiles=asp-301-ks2-la-2022-provisional |
		| Data in TSV format  | /my-local-authority/download-data/pupil-level-aggregated-la-data/download-as-zip/?fileType=TSV&selectedFiles=kts-301-ks2-la-2022-final&selectedFiles=asp-301-ks2-la-2022-provisional  |

@Javascript:disabled
Scenario Outline: Data downloads > Pupil level and aggregated LA data > Download school data - Download other dates link should link back to first step
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

@Javascript:disabled
Scenario: Data downloads > Individual school data > Search for a school - No results when Local Authority contains no schools
    Given Local Authority "301" exists:
		"""
		{ 
			"name": "Test LA"
		}
		"""
    When I navigate to /my-local-authority/download-data/individual-school-data/
	Then I should get a 200 response
	And the page title should be "Download data"
	And the page subtitle should be "Individual school data"
	And the breadcrumb trail should be:
		| text                | href                                                      | current |
		| Home                | /                                                         |         |
		| My local authority  | /my-local-authority/                                      |         |
		| Download data       | /my-local-authority/download-data/                        |         |
		| Search for a school | /my-local-authority/download-data/individual-school-data/ |         |
		| We found no schools |                                                           | true    |
	And the sub-navigation should be:
		| text          | href                                | current |
		| Download data | /my-local-authority/download-data/ | true    |
	And the side navigation should be:
		| text                               | href                                                              | current |
		| Pupil level and aggregated LA data | /my-local-authority/download-data/pupil-level-aggregated-la-data/ |         |
		| Individual school data             | /my-local-authority/download-data/individual-school-data/         | true    |
	And the sub-page title should be "We found no schools" with caption "Individual school data"

@Javascript:disabled
Scenario: Data downloads > Individual school data > Search for a school - Common page elements
	Given Local Authority "301" exists:
		"""
		{
			"name": "Test LA",
		}
		"""
    And Establishment "111111" exists:
		"""
		{
			"name": "Test School 1",
			"localAuthority":
			{
				"code": "301"
			}
		}
		"""
	When I navigate to /my-local-authority/download-data/individual-school-data/
	Then I should get a 200 response
	And the page title should be "Download data"
	And the page subtitle should be "Individual school data"
	And the breadcrumb trail should be:
		| text                | href                               | current |
		| Home                | /                                  |         |
		| My local authority  | /my-local-authority/               |         |
		| Download data       | /my-local-authority/download-data/ |         |
		| Search for a school |                                    | true    |
	And the sub-navigation should be:
		| text          | href                                | current |
		| Download data | /my-local-authority/download-data/ | true    |
	And the side navigation should be:
		| text                               | href                                                              | current |
		| Pupil level and aggregated LA data | /my-local-authority/download-data/pupil-level-aggregated-la-data/ |         |
		| Individual school data             | /my-local-authority/download-data/individual-school-data/         | true    |
	And the sub-page title should be "Search for a school" with caption "Individual school data"

@Javascript:disabled
Scenario: Data downloads > Individual school data > Search for a school - Should display only schools within the Local Authority
	Given Local Authority "301" exists:
		"""
		{ 
			"name": "Test LA"
		}
		"""
	And Establishment "111111" exists:
		"""
		{
			"name": "Test School 1",
			"localAuthority":
			{
				"code": "301"
			}
		}
		"""
	And Establishment "222222" exists:
		"""
		{
			"name": "Test School 2",
			"localAuthority":
			{
				"code": "301"
			}
		}
		"""
	And Establishment "333333" exists:
		"""
		{
			"name": "Test School 3",
			"localAuthority":
			{
				"code": "999"
			}
		}
		"""
	When I navigate to /my-local-authority/download-data/individual-school-data/
	Then the element "*[data-testid='NumberOfPages-Footer']" should have the text content "Showing 1 - 2 of 2 schools"
	And the element "[data-testid="establishment-listing-urn-<Counter>"]" should have the text content "<URN>"
	And the element "[data-testid="establishment-listing-name-<Counter>"]" should have the text content "<Name>"

Examples:
	| Counter | URN    | Name          |
	| 1       | 111111 | Test School 1 |
	| 2       | 222222 | Test School 2 |

@Javascript:disabled
Scenario Outline: Data downloads > Individual school data > Search for a school - Pagination
	And 251 Establishments exist with properties:
		| urn          | name                        | localAuthority                       |
		| (100000 + n) | Primary School (100000 + n) | { "code": "301", "name": "Test LA" } |
	And Local Authority "301" exists:
		"""
		{
			"name": "Test LA",
			"code": "301"
		}
		"""
	When I navigate to /my-local-authority/download-data/individual-school-data/
	Then the element "*[data-testid='NumberOfPages-Footer']" should have the text content "Showing 1 - 50 of 251 schools"
	And the element "*[data-testid='PageLinks-Footer-1']" should have the href "/my-local-authority/download-data/individual-school-data/?page=1"
	And the element "*[data-testid='PageLinks-Footer-2']" should have the href "/my-local-authority/download-data/individual-school-data/?page=2"
	And the element "*[data-testid='govuk-pagination__link--Footer']" should have the text content "..."
	And the element "*[data-testid='PageLinks-Footer-6']" should have the href "/my-local-authority/download-data/individual-school-data/?page=6"
	And the element "*[data-testid='PageLinks-Footer-Next']" should have the href "/my-local-authority/download-data/individual-school-data/?page=2"
	And the element "*[data-testid='establishment-listing-name-1']" should have the text content "Primary School 100001"
	And the element "*[data-testid='establishment-listing-name-2']" should have the text content "Primary School 100002"
	And the element "*[data-testid='establishment-listing-name-3']" should have the text content "Primary School 100003"
	And the element "*[data-testid='establishment-listing-name-4']" should have the text content "Primary School 100004"
	And the element "*[data-testid='establishment-listing-name-5']" should have the text content "Primary School 100005"
	And the element "*[data-testid='establishment-listing-urn-1']" should have the text content "100001"
	And the element "*[data-testid='establishment-listing-urn-2']" should have the text content "100002"
	And the element "*[data-testid='establishment-listing-urn-3']" should have the text content "100003"
	And the element "*[data-testid='establishment-listing-urn-4']" should have the text content "100004"
	And the element "*[data-testid='establishment-listing-urn-5']" should have the text content "100005"

@Javascript:disabled
Scenario Outline: Data downloads > Individual school data > Search for a school - Pagination 2
	And 501 Establishments exist with properties:
		| urn          | name                        | localAuthority                       |
		| (100000 + n) | Primary School (100000 + n) | { "code": "301", "name": "Test LA" } |
	And Local Authority "301" exists:
		"""
		{
			"name": "Test LA",
			"code": "301"
		}
		"""
	When I navigate to /my-local-authority/download-data/individual-school-data/?page=3
	Then the element "*[data-testid='NumberOfPages-Footer']" should have the text content "Showing 101 - 150 of 501 schools"
	And the elements "*[data-testid='PageLinks-Footer-Prev']" should all have the href "/my-local-authority/download-data/individual-school-data/?page=2"
	And the element "*[data-testid='PageLinks-Footer-1']" should have the href "/my-local-authority/download-data/individual-school-data/?page=1"
	And the element "*[data-testid='PageLinks-Footer-2']" should have the href "/my-local-authority/download-data/individual-school-data/?page=2"
	And the elements "*[data-testid='PageLinks-Footer-3']" should all have the href "/my-local-authority/download-data/individual-school-data/?page=3"
	And the elements "*[data-testid='PageLinks-Footer-4']" should all have the href "/my-local-authority/download-data/individual-school-data/?page=4"
	And the element "*[data-testid='govuk-pagination__link--Footer']" should have the text content "..."
	And the elements "*[data-testid='PageLinks-Footer-Next']" should all have the href "/my-local-authority/download-data/individual-school-data/?page=4"
	And the element "*[data-testid='establishment-listing-name-1']" should have the text content "Primary School 100101"
	And the element "*[data-testid='establishment-listing-name-2']" should have the text content "Primary School 100102"
	And the element "*[data-testid='establishment-listing-name-3']" should have the text content "Primary School 100103"
	And the element "*[data-testid='establishment-listing-name-4']" should have the text content "Primary School 100104"
	And the element "*[data-testid='establishment-listing-name-5']" should have the text content "Primary School 100105"
	And the element "*[data-testid='establishment-listing-urn-1']" should have the text content "100101"
	And the element "*[data-testid='establishment-listing-urn-2']" should have the text content "100102"
	And the element "*[data-testid='establishment-listing-urn-3']" should have the text content "100103"
	And the element "*[data-testid='establishment-listing-urn-4']" should have the text content "100104"
	And the element "*[data-testid='establishment-listing-urn-5']" should have the text content "100105"

@Javascript:disabled
Scenario: Data downloads > Individual school data > Search for a school - Page title should show correct text when search returns results
	Given Establishment "111111" exists:
		"""
		{
			"name": "Some Primary School",
			"localAuthority": {
				"code": "301",
				"name": "Oxfordshire"
			}
		}
		"""
	And Establishment "222222" exists:
		"""
		{
			"name": "Some Other Primary School",
			"localAuthority": {
				"code": "301",
				"name": "Oxfordshire"
			}
		}
		"""
	And Local Authority "301" exists:
		"""
		{
			"name": "Oxfordshire",
			"code": "301"
		}
		"""
	When I navigate to /my-local-authority/download-data/individual-school-data/
	And I update the textbox "#app-field-Search" to have the value "Primary"
	And I click the button "#searchSubmit"
	Then the path should be /my-local-authority/download-data/individual-school-data/?search=Primary
	And the page title should be "Download data"
	And the page subtitle should be "Individual school data"
	And the sub-page title should be "Search results for "Primary"" with caption "Individual school data"

@Javascript:enabled
Scenario: Data downloads > Individual school data > Search for a school - Page title should show correct text when search returns results (JS)
	Given Establishment "111111" exists:
		"""
		{
			"name": "Some Primary School",
			"localAuthority": {
				"code": "301",
				"name": "Oxfordshire"
			}
		}
		"""
	And Establishment "222222" exists:
		"""
		{
			"name": "Some Other Primary School",
			"localAuthority": {
				"code": "301",
					"name": "Oxfordshire"
			}
		}
		"""
	And Local Authority "301" exists:
		"""
		{
			"name": "Oxfordshire",
			"code": "301"
		}
		"""
	When I navigate to /my-local-authority/download-data/individual-school-data/
	And I update the textbox "#app-field-Search" to have the value "Primary"
	And I click the button "#searchSubmit"
	Then the path should be /my-local-authority/download-data/individual-school-data/?search=Primary
	And the page title should be "Download data"
	And the page subtitle should be "Individual school data"
	And the sub-page title should be "Search results for "Primary"" with caption "Individual school data"

@Javascript:disabled
Scenario: Data downloads > Individual school data > Search for a school - Page should show a breadcrumb trail when search returns results
	Given Establishment "111111" exists:
		"""
		{
			"name": "Some Primary School",
			"localAuthority": {
				"code": "301",
				"name": "Oxfordshire"
			}
		}
		"""
	And Establishment "222222" exists:
		"""
		{
			"name": "Some Other Primary School",
			"localAuthority": {
				"code": "301",
				"name": "Oxfordshire"
			}
		}
		"""
	And Local Authority "301" exists:
		"""
		{
			"name": "Oxfordshire",
			"code": "301"
		}
		"""
	When I navigate to /my-local-authority/download-data/individual-school-data/
	And I update the textbox "#app-field-Search" to have the value "Primary"
	And I click the button "#searchSubmit"
	Then the path should be /my-local-authority/download-data/individual-school-data/?search=Primary
	And the breadcrumb trail should be:
		| text                         | href                                                      | current |
		| Home                         | /                                                         |         |
		| My local authority           | /my-local-authority/                                      |         |
		| Download data                | /my-local-authority/download-data/                        |         |
		| Search for a school          | /my-local-authority/download-data/individual-school-data/ |         |
		| Search results for "Primary" |                                                           | true    |

@Javascript:enabled
Scenario: Data downloads > Individual school data > Search for a school - Page should show a breadcrumb trail when search returns results (JS)
	Given Establishment "111111" exists:
		"""
		{
			"name": "Some Primary School",
			"localAuthority": {
				"code": "301",
				"name": "Oxfordshire"
			}
		}
		"""
	And Establishment "222222" exists:
		"""
		{
			"name": "Some Other Primary School",
			"localAuthority": {
				"code": "301",
				"name": "Oxfordshire"
			}
		}
		"""
	And Local Authority "301" exists:
		"""
		{
			"name": "Oxfordshire",
			"code": "301"
		}
		"""
	When I navigate to /my-local-authority/download-data/individual-school-data/
	And I update the textbox "#app-field-Search" to have the value "Primary"
	And I click the button "#searchSubmit"
	Then the path should be /my-local-authority/download-data/individual-school-data/?search=Primary
	And the breadcrumb trail should be:
		| text                         | href                                                      | current |
		| Home                         | /                                                         |         |
		| My local authority           | /my-local-authority/                                      |         |
		| Download data                | /my-local-authority/download-data/                        |         |
		| Search for a school          | /my-local-authority/download-data/individual-school-data/ |         |
		| Search results for "Primary" |                                                           | true    |

@Javascript:disabled
Scenario: Data downloads > Individual school data > Search for a school - Page should show a breadcrumb trail when search returns no results
	Given Establishment "111111" exists:
		"""
		{
			"name": "Some Primary School",
			"localAuthority": {
				"code": "301",
				"name": "Oxfordshire"
			}
		}
		"""
	And Establishment "222222" exists:
		"""
		{
			"name": "Some Other Primary School",
			"localAuthority": {
				"code": "301",
				"name": "Oxfordshire"
			}
		}
		"""
	And Local Authority "301" exists:
		"""
		{
			"name": "Oxfordshire",
			"code": "301"
		}
		"""
	When I navigate to /my-local-authority/download-data/individual-school-data/
	And I update the textbox "#app-field-Search" to have the value "Secondary"
	And I click the button "#searchSubmit"
	Then the path should be /my-local-authority/download-data/individual-school-data/?search=Secondary
	And the breadcrumb trail should be:
		| text                                | href                                                      | current |
		| Home                                | /                                                         |         |
		| My local authority                  | /my-local-authority/                                      |         |
		| Download data                       | /my-local-authority/download-data/                        |         |
		| Search for a school                 | /my-local-authority/download-data/individual-school-data/ |         |
		| We found no matches for "Secondary" |                                                           | true    |

@Javascript:enabled
Scenario: Data downloads > Individual school data > Search for a school - Page should show a breadcrumb trail when search returns no results (JS)
	Given Establishment "111111" exists:
		"""
		{
			"name": "Some Primary School",
			"localAuthority": {
				"code": "301",
				"name": "Oxfordshire"
			}
		}
		"""
	And Establishment "222222" exists:
		"""
		{
			"name": "Some Other Primary School",
			"localAuthority": {
				"code": "301",
				"name": "Oxfordshire"
			}
		}
		"""
	And Local Authority "301" exists:
		"""
		{
			"name": "Oxfordshire",
			"code": "301"
		}
		"""
	When I navigate to /my-local-authority/download-data/individual-school-data/
	And I update the textbox "#app-field-Search" to have the value "Secondary"
	And I click the button "#searchSubmit"
	Then the path should be /my-local-authority/download-data/individual-school-data/?search=Secondary
	And the breadcrumb trail should be:
		| text                                | href                                                      | current |
		| Home                                | /                                                         |         |
		| My local authority                  | /my-local-authority/                                      |         |
		| Download data                       | /my-local-authority/download-data/                        |         |
		| Search for a school                 | /my-local-authority/download-data/individual-school-data/ |         |
		| We found no matches for "Secondary" |                                                           | true    |

@Javascript:disabled
Scenario: Data downloads > Individual school data > Search for a school - Search Term Validation
	Given Establishment "111111" exists:
		"""
		{
			"name": "Some Primary School",
			"localAuthority": {
				"code": "301",
				"name": "Oxfordshire"
			}
		}
		"""
	And Local Authority "301" exists:
		"""
		{
			"name": "Oxfordshire",
			"code": "301"
		}
		"""
	When I navigate to /my-local-authority/download-data/individual-school-data/
	Then I should get a 200 response
	And the page title should be "Download data"
	And the page subtitle should be "Individual school data"
	And the sub-page title should be "Search for a school" with caption "Individual school data"
	And the element "#searchForm" should have the text content "Enter school name, address, URN (Unique Reference Number) or LAESTAB (Local Authority Establishment Number) Search"

@Javascript:enabled
Scenario: Data downloads > Individual school data > Search for a school - Search Term Validation (JS)
	Given Establishment "111111" exists:
		"""
		{
			"name": "Some Primary School",
			"localAuthority": {
				"code": "301",
				"name": "Oxfordshire"
			}
		}
		"""
	And  Local Authority "301" exists:
		"""
		{
			"name": "Oxfordshire",
			"code": "301"
		}
		"""
	When I navigate to /my-local-authority/download-data/individual-school-data/
	Then I should get a 200 response
	And the page title should be "Download data"
	And the page subtitle should be "Individual school data"
	And the sub-page title should be "Search for a school" with caption "Individual school data"
	And the element "#searchForm" should have the text content "Enter school name, address, URN (Unique Reference Number) or LAESTAB (Local Authority Establishment Number) When autocomplete results are available use up and down arrows to review and enter to select. Touch device users, explore by touch or with swipe gestures. Search"

@Javascript:disabled
Scenario: Data downloads > Individual school data > Search for a school - Errors in Search Term Validation
	Given Establishment "111111" exists:
		"""
		{
			"name": "Some Primary School",
			"localAuthority": {
				"code": "301",
				"name": "Oxfordshire"
			}
		}
		"""
	And Local Authority "301" exists:
		"""
		{
			"name": "Oxfordshire",
			"code": "301"
		}
		"""
	When I navigate to /my-local-authority/download-data/individual-school-data/
	And I click the button "#searchSubmit"
	Then the path should be /my-local-authority/download-data/individual-school-data/?search=
	And the element "#app-field-Search-input-error" should have the text content "Please enter a school name, address, URN (Unique Reference Number) or LAESTAB (Local Authority Establishment Number)"
	And the element "h2.govuk-error-summary__title" should have the text content "There is a problem"
	And the element "*[data-testid='app-error-summary-Search']" should have the text content "Please enter a school name, address, URN (Unique Reference Number) or LAESTAB (Local Authority Establishment Number)"

@Javascript:enabled
Scenario: Data downloads > Individual school data > Search for a school - Errors in Search Term Validation (JS)
	Given Establishment "111111" exists:
		"""
		{
			"name": "Some Primary School",
			"localAuthority": {
				"code": "301",
				"name": "Oxfordshire"
			}
		}
		"""
	And Local Authority "301" exists:
		"""
		{
			"name": "Oxfordshire",
			"code": "301"
		}
		"""
	When I navigate to /my-local-authority/download-data/individual-school-data/
	And I click the button "#searchSubmit"
	Then the path should be /my-local-authority/download-data/individual-school-data/?search=
	And the element "#app-field-Search-input-error" should have the text content "Please enter a school name, address, URN (Unique Reference Number) or LAESTAB (Local Authority Establishment Number)"
	And the element "h2.govuk-error-summary__title" should have the text content "There is a problem"
	And the element "*[data-testid='app-error-summary-Search']" should have the text content "Please enter a school name, address, URN (Unique Reference Number) or LAESTAB (Local Authority Establishment Number)"
	
@Javascript:disabled
Scenario: Data downloads > Individual school data > Search for a school - Should show correct message for search term with no matches
	Given Establishment "111111" exists:
		"""
		{
			"name": "Some Primary School",
			"localAuthority": {
				"code": "301",
				"name": "Oxfordshire"
			}
		}
		"""
	And Local Authority "301" exists:
		"""
		{
			"name": "Oxfordshire",
			"code": "301"
		}
		"""
	When I navigate to /my-local-authority/download-data/individual-school-data/
	And I update the textbox "#app-field-Search" to have the value "secondary"
	And I click the button "#searchSubmit"
	Then the path should be /my-local-authority/download-data/individual-school-data/?search=secondary
	And the element "[data-testid="result-not-found-search-url"]" should have the href "/my-local-authority/download-data/individual-school-data/"
	And the page title should be "Download data"
	And the page subtitle should be "Individual school data"
	And the sub-page title should be "We found no matches for "secondary"" with caption "Individual school data"

@Javascript:enabled
Scenario: Data downloads > Individual school data > Search for a school - Should show correct message for search term with no matches (JS)
	Given Establishment "111111" exists:
		"""
		{
			"name": "Some Primary School",
			"localAuthority": {
				"code": "301",
				"name": "Oxfordshire"
			}
		}
		"""
	And Local Authority "301" exists:
		"""
		{
			"name": "Oxfordshire",
			"code": "301"
		}
		"""
	When I navigate to /my-local-authority/download-data/individual-school-data/
	And I update the textbox "#app-field-Search" to have the value "secondary"
	And I click the button "#searchSubmit"
	Then the path should be /my-local-authority/download-data/individual-school-data/?search=secondary
	And the element "[data-testid="result-not-found-search-url"]" should have the href "/my-local-authority/download-data/individual-school-data/"
	And the page title should be "Download data"
	And the page subtitle should be "Individual school data"
	And the sub-page title should be "We found no matches for "secondary"" with caption "Individual school data"

@Javascript:disabled
Scenario: Data downloads > Individual school data > Search for a school - Pagination 3
	Given 251 Establishments exist with properties:
		| urn          | name                        | localAuthority                       |
		| (100000 + n) | Primary School (100000 + n) | { "code": "301", "name": "Test LA" } |
	And Local Authority "301" exists:
		"""
		{
			"name": "Test LA",
			"code": "301"
		}
		"""
	When I navigate to /my-local-authority/download-data/individual-school-data/?search=primary
	Then the element "*[data-testid='NumberOfPages-Footer']" should have the text content "Showing 1 - 50 of 251 schools"
	And the element "*[data-testid='PageLinks-Footer-1']" should have the href "/my-local-authority/download-data/individual-school-data/?search=primary&page=1"
	And the element "*[data-testid='PageLinks-Footer-2']" should have the href "/my-local-authority/download-data/individual-school-data/?search=primary&page=2"
	And the element "*[data-testid='govuk-pagination__link--Footer']" should have the text content "..."
	And the element "*[data-testid='PageLinks-Footer-6']" should have the href "/my-local-authority/download-data/individual-school-data/?search=primary&page=6"
	And the element "*[data-testid='PageLinks-Footer-Next']" should have the href "/my-local-authority/download-data/individual-school-data/?search=primary&page=2"
	And the element "*[data-testid='establishment-listing-name-1']" should have the text content "Primary School 100001"
	And the element "*[data-testid='establishment-listing-name-2']" should have the text content "Primary School 100002"
	And the element "*[data-testid='establishment-listing-name-3']" should have the text content "Primary School 100003"
	And the element "*[data-testid='establishment-listing-name-4']" should have the text content "Primary School 100004"
	And the element "*[data-testid='establishment-listing-name-5']" should have the text content "Primary School 100005"
	And the element "*[data-testid='establishment-listing-urn-1']" should have the text content "100001"
	And the element "*[data-testid='establishment-listing-urn-2']" should have the text content "100002"
	And the element "*[data-testid='establishment-listing-urn-3']" should have the text content "100003"
	And the element "*[data-testid='establishment-listing-urn-4']" should have the text content "100004"
	And the element "*[data-testid='establishment-listing-urn-5']" should have the text content "100005"

@Javascript:enabled
Scenario: Data downloads > Individual school data > Search for a school - Pagination 3 (JS)
	Given 251 Establishments exist with properties:
		| urn          | name                        | localAuthority                       |
		| (100000 + n) | Primary School (100000 + n) | { "code": "301", "name": "Test LA" } |
	And Local Authority "301" exists:
		"""
		{
			"name": "Test LA",
			"code": "301"
		}
		"""
	When I navigate to /my-local-authority/download-data/individual-school-data/?search=primary
	Then the element "*[data-testid='NumberOfPages-Footer']" should have the text content "Showing 1 - 50 of 251 schools"
	And the element "*[data-testid='PageLinks-Footer-1']" should have the href "/my-local-authority/download-data/individual-school-data/?search=primary&page=1"
	And the element "*[data-testid='PageLinks-Footer-2']" should have the href "/my-local-authority/download-data/individual-school-data/?search=primary&page=2"
	And the element "*[data-testid='govuk-pagination__link--Footer']" should have the text content "..."
	And the element "*[data-testid='PageLinks-Footer-6']" should have the href "/my-local-authority/download-data/individual-school-data/?search=primary&page=6"
	And the element "*[data-testid='PageLinks-Footer-Next']" should have the href "/my-local-authority/download-data/individual-school-data/?search=primary&page=2"
	And the element "*[data-testid='establishment-listing-name-1']" should have the text content "Primary School 100001"
	And the element "*[data-testid='establishment-listing-name-2']" should have the text content "Primary School 100002"
	And the element "*[data-testid='establishment-listing-name-3']" should have the text content "Primary School 100003"
	And the element "*[data-testid='establishment-listing-name-4']" should have the text content "Primary School 100004"
	And the element "*[data-testid='establishment-listing-name-5']" should have the text content "Primary School 100005"
	And the element "*[data-testid='establishment-listing-urn-1']" should have the text content "100001"
	And the element "*[data-testid='establishment-listing-urn-2']" should have the text content "100002"
	And the element "*[data-testid='establishment-listing-urn-3']" should have the text content "100003"
	And the element "*[data-testid='establishment-listing-urn-4']" should have the text content "100004"
	And the element "*[data-testid='establishment-listing-urn-5']" should have the text content "100005"

@Javascript:disabled
Scenario: Data downloads > Individual school data > Search for a school - Pagination 4
	Given 501 Establishments exist with properties:
		| urn          | name                        | localAuthority                       |
		| (100000 + n) | Primary School (100000 + n) | { "code": "301", "name": "Test LA" } |
	And Local Authority "301" exists:
		"""
		{
			"name": "Test LA",
			"code": "301"
		}
		"""
	When I navigate to /my-local-authority/download-data/individual-school-data/?page=3&search=primary
	Then the element "*[data-testid='NumberOfPages-Footer']" should have the text content "Showing 101 - 150 of 501 schools"
	And the elements "*[data-testid='PageLinks-Footer-Prev']" should all have the href "/my-local-authority/download-data/individual-school-data/?search=primary&page=2"
	And the elements "*[data-testid='PageLinks-Footer-1']" should all have the href "/my-local-authority/download-data/individual-school-data/?search=primary&page=1"
	And the elements "*[data-testid='PageLinks-Footer-2']" should all have the href "/my-local-authority/download-data/individual-school-data/?search=primary&page=2"
	And the elements "*[data-testid='PageLinks-Footer-3']" should all have the href "/my-local-authority/download-data/individual-school-data/?search=primary&page=3"
	And the elements "*[data-testid='PageLinks-Footer-4']" should all have the href "/my-local-authority/download-data/individual-school-data/?search=primary&page=4"
	And the element "*[data-testid='govuk-pagination__link--Footer']" should have the text content "..."
	And the elements "*[data-testid='PageLinks-Footer-Next']" should all have the href "/my-local-authority/download-data/individual-school-data/?search=primary&page=4"
	And the element "*[data-testid='establishment-listing-name-1']" should have the text content "Primary School 100101"
	And the element "*[data-testid='establishment-listing-name-2']" should have the text content "Primary School 100102"
	And the element "*[data-testid='establishment-listing-name-3']" should have the text content "Primary School 100103"
	And the element "*[data-testid='establishment-listing-name-4']" should have the text content "Primary School 100104"
	And the element "*[data-testid='establishment-listing-name-5']" should have the text content "Primary School 100105"
	And the element "*[data-testid='establishment-listing-urn-1']" should have the text content "100101"
	And the element "*[data-testid='establishment-listing-urn-2']" should have the text content "100102"
	And the element "*[data-testid='establishment-listing-urn-3']" should have the text content "100103"
	And the element "*[data-testid='establishment-listing-urn-4']" should have the text content "100104"
	And the element "*[data-testid='establishment-listing-urn-5']" should have the text content "100105"

@Javascript:enabled
Scenario: Data downloads > Individual school data > Search for a school - Pagination 4 (JS)
	Given 501 Establishments exist with properties:
		| urn          | name                        | localAuthority                       |
		| (100000 + n) | Primary School (100000 + n) | { "code": "301", "name": "Test LA" } |
	And Local Authority "301" exists:
		"""
		{
			"name": "Test LA",
			"code": "301"
		}
		"""
	When I navigate to /my-local-authority/download-data/individual-school-data/?page=3&search=primary
	Then the element "*[data-testid='NumberOfPages-Footer']" should have the text content "Showing 101 - 150 of 501 schools"
	And the elements "*[data-testid='PageLinks-Footer-Prev']" should all have the href "/my-local-authority/download-data/individual-school-data/?search=primary&page=2"
	And the elements "*[data-testid='PageLinks-Footer-1']" should all have the href "/my-local-authority/download-data/individual-school-data/?search=primary&page=1"
	And the elements "*[data-testid='PageLinks-Footer-2']" should all have the href "/my-local-authority/download-data/individual-school-data/?search=primary&page=2"
	And the elements "*[data-testid='PageLinks-Footer-3']" should all have the href "/my-local-authority/download-data/individual-school-data/?search=primary&page=3"
	And the elements "*[data-testid='PageLinks-Footer-4']" should all have the href "/my-local-authority/download-data/individual-school-data/?search=primary&page=4"
	And the element "*[data-testid='govuk-pagination__link--Footer']" should have the text content "..."
	And the elements "*[data-testid='PageLinks-Footer-Next']" should all have the href "/my-local-authority/download-data/individual-school-data/?search=primary&page=4"
	And the element "*[data-testid='establishment-listing-name-1']" should have the text content "Primary School 100101"
	And the element "*[data-testid='establishment-listing-name-2']" should have the text content "Primary School 100102"
	And the element "*[data-testid='establishment-listing-name-3']" should have the text content "Primary School 100103"
	And the element "*[data-testid='establishment-listing-name-4']" should have the text content "Primary School 100104"
	And the element "*[data-testid='establishment-listing-name-5']" should have the text content "Primary School 100105"
	And the element "*[data-testid='establishment-listing-urn-1']" should have the text content "100101"
	And the element "*[data-testid='establishment-listing-urn-2']" should have the text content "100102"
	And the element "*[data-testid='establishment-listing-urn-3']" should have the text content "100103"
	And the element "*[data-testid='establishment-listing-urn-4']" should have the text content "100104"
	And the element "*[data-testid='establishment-listing-urn-5']" should have the text content "100105"

@Javascript:disabled
Scenario: Data downloads > Individual school data > Search for a school - Pagination 5
	Given 51 Establishments exist with properties:
		| urn          | name                        | localAuthority                       |
		| (100000 + n) | Primary School (100000 + n) | { "code": "301", "name": "Test LA" } |
	And Local Authority "301" exists:
		"""
		{
			"name": "Test LA",
			"code": "301"
		}
		"""
	When I navigate to /my-local-authority/download-data/individual-school-data/?page=2&search=primary
	Then the element "*[data-testid='NumberOfPages-Footer']" should have the text content "Showing 51 - 51 of 51 schools"
	And the elements "*[data-testid='PageLinks-Footer-Prev']" should all have the href "/my-local-authority/download-data/individual-school-data/?search=primary&page=1"
	And the elements "*[data-testid='PageLinks-Footer-1']" should all have the href "/my-local-authority/download-data/individual-school-data/?search=primary&page=1"
	And the element "*[data-testid='establishment-listing-name-1']" should have the text content "Primary School 100051"
	And the element "*[data-testid='establishment-listing-urn-1']" should have the text content "100051"

@Javascript:enabled
Scenario: Data downloads > Individual school data > Search for a school - Pagination 5 (JS)
	Given 51 Establishments exist with properties:
		| urn          | name                        | localAuthority                       |
		| (100000 + n) | Primary School (100000 + n) | { "code": "301", "name": "Test LA" } |
	And Local Authority "301" exists:
		"""
		{
			"name": "Test LA",
			"code": "301"
		}
		"""
	When I navigate to /my-local-authority/download-data/individual-school-data/?page=2&search=primary
	Then the element "*[data-testid='NumberOfPages-Footer']" should have the text content "Showing 51 - 51 of 51 schools"
	And the elements "*[data-testid='PageLinks-Footer-Prev']" should all have the href "/my-local-authority/download-data/individual-school-data/?search=primary&page=1"
	And the elements "*[data-testid='PageLinks-Footer-1']" should all have the href "/my-local-authority/download-data/individual-school-data/?search=primary&page=1"
	And the element "*[data-testid='establishment-listing-name-1']" should have the text content "Primary School 100051"
	And the element "*[data-testid='establishment-listing-urn-1']" should have the text content "100051"

@Javascript:disabled
Scenario: Data downloads > Individual school data > Search for a school - Matching URN search should redirect to Dates available for download
	Given Establishment "111111" exists:
		"""
		{
			"name": "Some Primary School",
			"localAuthority": {
				"code": "301",
				"name": "Test LA"
			}
		}
		"""
	And Local Authority "301" exists:
		"""
		{
			"name": "Test LA",
			"code": "301"
		}
		"""
	And blob storage file downloads-config.json exists in config container:
		"""
		[
			{
				"id": "kts-school-ks2-pupil",
				"source": "KTS",
				"scope": "School",
				"dataSetType": "KeyStage2",
				"label": "Key stage 2 (KS2)",
				"filePathPattern": "School/{urn}/{year}/{filetype}/ks2_pupil_{version}.{filetype}"
			}
		]
		"""
	And blob storage file School/111111/2022/csv/ks2_pupil_final.csv exists in downloads-kts container:
		"""
		Column A,Column B,Column C
		1,2,3
		"""
	When I navigate to /my-local-authority/download-data/individual-school-data/
	And I update the textbox "#app-field-Search" to have the value "111111"
	And I click the button "#searchSubmit"
	Then the path should be /my-local-authority/download-data/individual-school-data/111111/select-year/
	And the page title should be "Download data"
	And the page subtitle should be "Individual school data"
	And the sub-page title should be "Dates available for download" with caption "Some Primary School (URN: 111111)"

@Javascript:enabled
Scenario: Data downloads > Individual school data > Search for a school - Matching URN search should redirect to Dates available for download (JS)
	Given Establishment "111111" exists:
		"""
		{
			"name": "Some Primary School",
			"localAuthority": {
				"code": "301",
				"name": "Test LA"
			}
		}
		"""
	And Local Authority "301" exists:
		"""
		{
			"name": "Test LA",
			"code": "301"
		}
		"""
	And blob storage file downloads-config.json exists in config container:
		"""
		[
			{
				"id": "kts-school-ks2-pupil",
				"source": "KTS",
				"scope": "School",
				"dataSetType": "KeyStage2",
				"label": "Key stage 2 (KS2)",
				"filePathPattern": "School/{urn}/{year}/{filetype}/ks2_pupil_{version}.{filetype}"
			}
		]
		"""
	And blob storage file School/111111/2022/csv/ks2_pupil_final.csv exists in downloads-kts container:
		"""
		Column A,Column B,Column C
		1,2,3
		"""
	When I navigate to /my-local-authority/download-data/individual-school-data/
	And I update the textbox "#app-field-Search" to have the value "111111"
	And I click the button "#searchSubmit"
	Then the path should be /my-local-authority/download-data/individual-school-data/111111/select-year/
	And the page title should be "Download data"
	And the page subtitle should be "Individual school data"
	And the sub-page title should be "Dates available for download" with caption "Some Primary School (URN: 111111)"

@Javascript:disabled
Scenario: Data downloads > Individual school data > Search for a school - Partial match for school name should redirect to Dates available for download
	Given Establishment "111111" exists:
		"""
		{
			"name": "Some Primary School",
			"localAuthority": {
				"code": "301",
				"name": "Test LA"
			}
		}
		"""
	And Local Authority "301" exists:
		"""
		{
			"name": "Test LA",
			"code": "301"
		}
		"""
	And blob storage file downloads-config.json exists in config container:
		"""
		[
			{
				"id": "kts-school-ks2-pupil",
				"source": "KTS",
				"scope": "School",
				"dataSetType": "KeyStage2",
				"label": "Key stage 2 (KS2)",
				"filePathPattern": "School/{urn}/{year}/{filetype}/ks2_pupil_{version}.{filetype}"
			}
		]
		"""
	And blob storage file School/111111/2022/csv/ks2_pupil_final.csv exists in downloads-kts container:
		"""
		Column A,Column B,Column C
		1,2,3
		"""
	When I navigate to /my-local-authority/download-data/individual-school-data/
	And I update the textbox "#app-field-Search" to have the value "PRiMaRY"
	And I click the button "#searchSubmit"
	Then the path should be /my-local-authority/download-data/individual-school-data/111111/select-year/
	And the page title should be "Download data"
	And the page subtitle should be "Individual school data"
	And the sub-page title should be "Dates available for download" with caption "Some Primary School (URN: 111111)"

@Javascript:enabled
Scenario: Data downloads > Individual school data > Search for a school - Partial match for school name should redirect to Dates available for download (JS)
	Given Establishment "111111" exists:
		"""
		{
			"name": "Some Primary School",
			"localAuthority": {
				"code": "301",
				"name": "Test LA"
			}
		}
		"""
	And Local Authority "301" exists:
		"""
		{
			"name": "Test LA",
			"code": "301"
		}
		"""
	And blob storage file downloads-config.json exists in config container:
		"""
		[
			{
				"id": "kts-school-ks2-pupil",
				"source": "KTS",
				"scope": "School",
				"dataSetType": "KeyStage2",
				"label": "Key stage 2 (KS2)",
				"filePathPattern": "School/{urn}/{year}/{filetype}/ks2_pupil_{version}.{filetype}"
			}
		]
		"""
	And blob storage file School/111111/2022/csv/ks2_pupil_final.csv exists in downloads-kts container:
		"""
		Column A,Column B,Column C
		1,2,3
		"""
	When I navigate to /my-local-authority/download-data/individual-school-data/
	And I update the textbox "#app-field-Search" to have the value "PRiMaRY"
	And I click the button "#searchSubmit"
	Then the path should be /my-local-authority/download-data/individual-school-data/111111/select-year/
	And the page title should be "Download data"
	And the page subtitle should be "Individual school data"
	And the sub-page title should be "Dates available for download" with caption "Some Primary School (URN: 111111)"

@Javascript:disabled
Scenario: Data downloads > Individual school data > Search for a school - Partial street match should redirect to Dates available for download
	Given Establishment "111111" exists:
		"""
		{
			"name": "Some Primary School",
			"address": {
				"street": "13 The Street",
				"town": "SomeTown",
				"postCode": "TR18 3JT"
			},
			"localAuthority": {
				"code": "301",
				"name": "Test LA"
			}
		} 
		"""
	And Local Authority "301" exists:
		"""
		{
			"name": "Test LA",
			"code": "301"
		}
		"""
	And blob storage file downloads-config.json exists in config container:
		"""
		[
			{
				"id": "kts-school-ks2-pupil",
				"source": "KTS",
				"scope": "School",
				"dataSetType": "KeyStage2",
				"label": "Key stage 2 (KS2)",
				"filePathPattern": "School/{urn}/{year}/{filetype}/ks2_pupil_{version}.{filetype}"
			}
		]
		"""
	And blob storage file School/111111/2022/csv/ks2_pupil_final.csv exists in downloads-kts container:
		"""
		Column A,Column B,Column C
		1,2,3
		"""
	When I navigate to /my-local-authority/download-data/individual-school-data/
	And I update the textbox "#app-field-Search" to have the value "str"
	And I click the button "#searchSubmit"
	Then the path should be /my-local-authority/download-data/individual-school-data/111111/select-year/

@Javascript:enabled
Scenario: Data downloads > Individual school data > Search for a school - Partial street match should redirect to Dates available for download (JS)
	Given Establishment "111111" exists:
		"""
		{
			"name": "Some Primary School",
			"address": {
				"street": "13 The Street",
				"town": "SomeTown",
				"postCode": "TR18 3JT"
			},
			"localAuthority": {
				"code": "301",
				"name": "Test LA"
			}
		} 
		"""
	And Local Authority "301" exists:
		"""
		{
			"name": "Test LA",
			"code": "301"
		}
		"""
	And blob storage file downloads-config.json exists in config container:
		"""
		[
			{
				"id": "kts-school-ks2-pupil",
				"source": "KTS",
				"scope": "School",
				"dataSetType": "KeyStage2",
				"label": "Key stage 2 (KS2)",
				"filePathPattern": "School/{urn}/{year}/{filetype}/ks2_pupil_{version}.{filetype}"
			}
		]
		"""
	And blob storage file School/111111/2022/csv/ks2_pupil_final.csv exists in downloads-kts container:
		"""
		Column A,Column B,Column C
		1,2,3
		"""
	When I navigate to /my-local-authority/download-data/individual-school-data/
	And I update the textbox "#app-field-Search" to have the value "str"
	And I click the button "#searchSubmit"
	Then the path should be /my-local-authority/download-data/individual-school-data/111111/select-year/

@Javascript:disabled
Scenario: Data downloads > Individual school data > Search for a school - Partial town match should redirect to Dates available for download
	Given Establishment "111111" exists:
		"""
		{
			"name": "Some Primary School",
			"address": {
				"street": "13 The Street",
				"town": "SomeTown",
				"postCode": "TR18 3JT"
			},
			"localAuthority": {
				"code": "301",
				"name": "Test LA"
			}
		} 
		"""
	And Local Authority "301" exists:
		"""
		{
			"name": "Test LA",
			"code": "301"
		}
		"""
	And blob storage file downloads-config.json exists in config container:
		"""
		[
			{
				"id": "kts-school-ks2-pupil",
				"source": "KTS",
				"scope": "School",
				"dataSetType": "KeyStage2",
				"label": "Key stage 2 (KS2)",
				"filePathPattern": "School/{urn}/{year}/{filetype}/ks2_pupil_{version}.{filetype}"
			}
		]
		"""
	And blob storage file School/111111/2022/csv/ks2_pupil_final.csv exists in downloads-kts container:
		"""
		Column A,Column B,Column C
		1,2,3
		"""
	When I navigate to /my-local-authority/download-data/individual-school-data/
	And I update the textbox "#app-field-Search" to have the value "some"
	And I click the button "#searchSubmit"
	Then the path should be /my-local-authority/download-data/individual-school-data/111111/select-year/

@Javascript:enabled
Scenario: Data downloads > Individual school data > Search for a school - Partial town match should redirect to Dates available for download (JS)
	Given Establishment "111111" exists:
		"""
		{
			"name": "Some Primary School",
			"address": {
				"street": "13 The Street",
				"town": "SomeTown",
				"postCode": "TR18 3JT"
			},
			"localAuthority": {
				"code": "301",
				"name": "Test LA"
			}
		} 
		"""
	And Local Authority "301" exists:
		"""
		{
			"name": "Test LA",
			"code": "301"
		}
		"""
	And blob storage file downloads-config.json exists in config container:
		"""
		[
			{
				"id": "kts-school-ks2-pupil",
				"source": "KTS",
				"scope": "School",
				"dataSetType": "KeyStage2",
				"label": "Key stage 2 (KS2)",
				"filePathPattern": "School/{urn}/{year}/{filetype}/ks2_pupil_{version}.{filetype}"
			}
		]
		"""
	And blob storage file School/111111/2022/csv/ks2_pupil_final.csv exists in downloads-kts container:
		"""
		Column A,Column B,Column C
		1,2,3
		"""
	When I navigate to /my-local-authority/download-data/individual-school-data/
	And I update the textbox "#app-field-Search" to have the value "some"
	And I click the button "#searchSubmit"
	Then the path should be /my-local-authority/download-data/individual-school-data/111111/select-year/

@Javascript:disabled
Scenario: Data downloads > Individual school data > Search for a school - Partial postcode match should redirect to Dates available for download
	Given Establishment "111111" exists:
		"""
		{
			"name": "Some Primary School",
			"address": {
				"street": "13 The Street",
				"town": "SomeTown",
				"postCode": "TR18 3JT"
			},
			"localAuthority": {
				"code": "301",
				"name": "Test LA"
			}
		} 
		"""
	And Local Authority "301" exists:
		"""
		{
			"name": "Test LA",
			"code": "301"
		}
		"""
	And blob storage file downloads-config.json exists in config container:
		"""
		[
			{
				"id": "kts-school-ks2-pupil",
				"source": "KTS",
				"scope": "School",
				"dataSetType": "KeyStage2",
				"label": "Key stage 2 (KS2)",
				"filePathPattern": "School/{urn}/{year}/{filetype}/ks2_pupil_{version}.{filetype}"
			}
		]
		"""
	And blob storage file School/111111/2022/csv/ks2_pupil_final.csv exists in downloads-kts container:
		"""
		Column A,Column B,Column C
		1,2,3
		"""
	When I navigate to /my-local-authority/download-data/individual-school-data/
	And I update the textbox "#app-field-Search" to have the value "tr1"
	And I click the button "#searchSubmit"
	Then the path should be /my-local-authority/download-data/individual-school-data/111111/select-year/

@Javascript:enabled
Scenario: Data downloads > Individual school data > Search for a school - Partial postcode match should redirect to Dates available for download (JS)
	Given Establishment "111111" exists:
		"""
		{
			"name": "Some Primary School",
			"address": {
				"street": "13 The Street",
				"town": "SomeTown",
				"postCode": "TR18 3JT"
			},
			"localAuthority": {
				"code": "301",
				"name": "Test LA"
			}
		} 
		"""
	And Local Authority "301" exists:
		"""
		{
			"name": "Test LA",
			"code": "301"
		}
		"""
	And blob storage file downloads-config.json exists in config container:
		"""
		[
			{
				"id": "kts-school-ks2-pupil",
				"source": "KTS",
				"scope": "School",
				"dataSetType": "KeyStage2",
				"label": "Key stage 2 (KS2)",
				"filePathPattern": "School/{urn}/{year}/{filetype}/ks2_pupil_{version}.{filetype}"
			}
		]
		"""
	And blob storage file School/111111/2022/csv/ks2_pupil_final.csv exists in downloads-kts container:
		"""
		Column A,Column B,Column C
		1,2,3
		"""
	When I navigate to /my-local-authority/download-data/individual-school-data/
	And I update the textbox "#app-field-Search" to have the value "tr1"
	And I click the button "#searchSubmit"
	Then the path should be /my-local-authority/download-data/individual-school-data/111111/select-year/

@Javascript:disabled
Scenario Outline: Data downloads > Individual school data > Search for a school - Results page should show partial name and address matches
	Given Establishment "111111" exists:
		"""
		{
			"name": "Some Primary School",
			"address": {
				"street": "13 The Street",
				"town": "SomeTown",
				"postCode": "B1 1AA"
			},
			"localAuthority": {
				"code": "301",
				"name": "Test LA"
			}
		}
		"""
	And Establishment "222222" exists:
		"""
		{
			"name": "Some Other Primary School",
			"address": {
				"street": "13 The Road",
				"town": "Tring",
				"postCode": "B1 1AA"
			},
			"localAuthority": {
				"code": "301",
				"name": "Test LA"
			}
		}
		"""
	And Establishment "333333" exists:
		"""
		{
			"name": "A Different Primary School",
			"address": {
				"street": "13 The Road",
				"town": "SomeTown",
				"postCode": "TR18 3JT"
			},
			"localAuthority": {
				"code": "301",
				"name": "Test LA"
			}
		}
		"""
	And Establishment "444444" exists:
		"""
		{
			"name": "The Training Centre",
			"localAuthority": {
				"code": "301",
				"name": "Test LA"
			}
		}
		"""
	And Local Authority "301" exists:
		"""
		{
			"name": "Test LA",
			"code": "301"
		}
		"""
	When I navigate to /my-local-authority/download-data/individual-school-data/
	And I update the textbox "#app-field-Search" to have the value "tr"
	And I click the button "#searchSubmit"
	Then the path should be /my-local-authority/download-data/individual-school-data/?search=tr
	And the element "[data-testid="establishment-listing-urn-<Counter>"]" should have the text content "<URN>"
	And the element "[data-testid="establishment-listing-name-<Counter>"]" should have the text content "<Name>"
	And the element "[data-testid="establishment-listing-address-<Counter>"]" should have the text content "<Address>"
	And the element "[data-testid="establishment-listing-name-<Counter>"]" should have the href "<Href>"

Examples:
	| Counter | URN    | Name                       | Address                        | Href                                                                          |
	| 1       | 333333 | A Different Primary School | 13 The Road, SomeTown TR18 3JT | /my-local-authority/download-data/individual-school-data/333333/select-year/ |
	| 2       | 222222 | Some Other Primary School  | 13 The Road, Tring B1 1AA      | /my-local-authority/download-data/individual-school-data/222222/select-year/ |
	| 3       | 111111 | Some Primary School        | 13 The Street, SomeTown B1 1AA | /my-local-authority/download-data/individual-school-data/111111/select-year/ |
	| 4       | 444444 | The Training Centre        | No address available           | /my-local-authority/download-data/individual-school-data/444444/select-year/ |

@Javascript:enabled
Scenario Outline: Data downloads > Individual school data > Search for a school - Results page should show partial name and address matches (JS)
	Given Establishment "111111" exists:
		"""
		{
			"name": "Some Primary School",
			"address": {
				"street": "13 The Street",
				"town": "SomeTown",
				"postCode": "B1 1AA"
			},
			"localAuthority": {
				"code": "301",
				"name": "Test LA"
			}
		}
		"""
	And Establishment "222222" exists:
		"""
		{
			"name": "Some Other Primary School",
			"address": {
				"street": "13 The Road",
				"town": "Tring",
				"postCode": "B1 1AA"
			},
			"localAuthority": {
				"code": "301",
				"name": "Test LA"
			}
		}
		"""
	And Establishment "333333" exists:
		"""
		{
			"name": "A Different Primary School",
			"address": {
				"street": "13 The Road",
				"town": "SomeTown",
				"postCode": "TR18 3JT"
			},
			"localAuthority": {
				"code": "301",
				"name": "Test LA"
			}
		}
		"""
	And Establishment "444444" exists:
		"""
		{
			"name": "The Training Centre",
			"localAuthority": {
				"code": "301",
				"name": "Test LA"
			}
		}
		"""
	And Local Authority "301" exists:
		"""
		{
			"name": "Test LA",
			"code": "301"
		}
		"""
	When I navigate to /my-local-authority/download-data/individual-school-data/
	And I update the textbox "#app-field-Search" to have the value "tr"
	And I click the button "#searchSubmit"
	Then the path should be /my-local-authority/download-data/individual-school-data/?search=tr
	And the element "[data-testid="establishment-listing-urn-<Counter>"]" should have the text content "<URN>"
	And the element "[data-testid="establishment-listing-name-<Counter>"]" should have the text content "<Name>"
	And the element "[data-testid="establishment-listing-address-<Counter>"]" should have the text content "<Address>"

Examples:
	| Counter | URN    | Name                       | Address                        | Href                                                                          |
	| 1       | 333333 | A Different Primary School | 13 The Road, SomeTown TR18 3JT | /my-local-authority/download-data/individual-school-data/333333/select-year/ |
	| 2       | 222222 | Some Other Primary School  | 13 The Road, Tring B1 1AA      | /my-local-authority/download-data/individual-school-data/222222/select-year/ |
	| 3       | 111111 | Some Primary School        | 13 The Street, SomeTown B1 1AA | /my-local-authority/download-data/individual-school-data/111111/select-year/ |
	| 4       | 444444 | The Training Centre        | No address available           | /my-local-authority/download-data/individual-school-data/444444/select-year/ |

@Javascript:disabled
Scenario: Data downloads > Individual school data > Search for a school - School search successful for 6-digit URN
	Given Establishment "111111" exists:
		"""
		{
			"name": "Some Primary School",
			"localAuthority": {
				"code": "301",
				"name": "Test LA"
			}
		}
		"""
	And Local Authority "301" exists:
		"""
		{
			"name": "Test LA",
			"code": "301"
		}
		"""
	And blob storage file downloads-config.json exists in config container:
		"""
		[
			{
				"id": "kts-school-ks2-pupil",
				"source": "KTS",
				"scope": "School",
				"dataSetType": "KeyStage2",
				"label": "Key stage 2 (KS2)",
				"filePathPattern": "School/{urn}/{year}/{filetype}/ks2_pupil_{version}.{filetype}"
			}
		]
		"""
	And blob storage file School/111111/2022/csv/ks2_pupil_final.csv exists in downloads-kts container:
		"""
		Column A,Column B,Column C
		1,2,3
		"""
	When I navigate to /my-local-authority/download-data/individual-school-data/
	And I update the textbox "#app-field-Search" to have the value "111111"
	And I click the button "#searchSubmit"
	Then the path should be /my-local-authority/download-data/individual-school-data/111111/select-year/
	And the page title should be "Download data"
	And the page subtitle should be "Individual school data"
	And the sub-page title should be "Dates available for download" with caption "Some Primary School (URN: 111111)"

@Javascript:enabled
Scenario: Data downloads > Individual school data > Search for a school - School search successful for 6-digit URN (JS)
	Given Establishment "111111" exists:
		"""
		{
			"name": "Some Primary School",
			"localAuthority": {
				"code": "301",
				"name": "Test LA"
			}
		}
		"""
	And Local Authority "301" exists:
		"""
		{
			"name": "Test LA",
			"code": "301"
		}
		"""
	And blob storage file downloads-config.json exists in config container:
		"""
		[
			{
				"id": "kts-school-ks2-pupil",
				"source": "KTS",
				"scope": "School",
				"dataSetType": "KeyStage2",
				"label": "Key stage 2 (KS2)",
				"filePathPattern": "School/{urn}/{year}/{filetype}/ks2_pupil_{version}.{filetype}"
			}
		]
		"""
	And blob storage file School/111111/2022/csv/ks2_pupil_final.csv exists in downloads-kts container:
		"""
		Column A,Column B,Column C
		1,2,3
		"""
	When I navigate to /my-local-authority/download-data/individual-school-data/
	And I update the textbox "#app-field-Search" to have the value "111111"
	And I click the button "#searchSubmit"
	Then the path should be /my-local-authority/download-data/individual-school-data/111111/select-year/
	And the page title should be "Download data"
	And the page subtitle should be "Individual school data"
	And the sub-page title should be "Dates available for download" with caption "Some Primary School (URN: 111111)"

@Javascript:disabled
Scenario Outline: Data downloads > Individual school data > Search for a school - School search with less than 6 digits does not match on URN
	Given Establishment "111111" exists:
		"""
		{
			"name": "Some Primary School",
			"localAuthority": {
				"code": "301",
				"name": "Test LA"
			}
		}
		"""
	And Local Authority "301" exists:
		"""
		{
			"name": "Test LA",
			"code": "301"
		}
		"""
	When I navigate to /my-local-authority/download-data/individual-school-data/
	And I update the textbox "#app-field-Search" to have the value "<SearchTerm>"
	And I click the button "#searchSubmit"
	Then the path should be /my-local-authority/download-data/individual-school-data/?search=<SearchTerm>
	And the page title should be "Download data"
	And the page subtitle should be "Individual school data"
	And the sub-page title should be "We found no matches for "<SearchTerm>"" with caption "Individual school data"

Examples:
	| SearchTerm |
	| 1          |
	| 11         |
	| 111        |
	| 1111       |
	| 11111      |

@Javascript:enabled
Scenario Outline: Data downloads > Individual school data > Search for a school - School search with less than 6 digits does not match on URN (JS)
	Given Establishment "111111" exists:
		"""
		{
			"name": "Some Primary School",
			"localAuthority": {
				"code": "301",
				"name": "Test LA"
			}
		}
		"""
	And Local Authority "301" exists:
		"""
		{
			"name": "Test LA",
			"code": "301"
		}
		"""
	When I navigate to /my-local-authority/download-data/individual-school-data/
	And I update the textbox "#app-field-Search" to have the value "<SearchTerm>"
	And I click the button "#searchSubmit"
	Then the path should be /my-local-authority/download-data/individual-school-data/?search=<SearchTerm>
	And the page title should be "Download data"
	And the page subtitle should be "Individual school data"
	And the sub-page title should be "We found no matches for "<SearchTerm>"" with caption "Individual school data"

Examples:
	| SearchTerm |
	| 1          |
	| 11         |
	| 111        |
	| 1111       |
	| 11111      |

@Javascript:disabled
Scenario Outline: Data downloads > Individual school data > Search for a school - School search with less than 6 digits matches on school address
	Given Establishment "111111" exists:
		"""
		{
			"name": "Some Primary School",
			"localAuthority": {
				"code": "301",
		 		"name": "Test LA"
			}
		}
		"""
	And Establishment "222222" exists:
		"""
		{
			"name": "Another Primary School",
			"address": {
				"street": "<SearchTerm> The Street",
				"town": "SomeTown",
				"postCode": "TR18 3JT"
			},
			"localAuthority": {
				"code": "301",
				"name": "Test LA"
			}
		} 
		"""
	And Local Authority "301" exists:
		"""
		{
			"name": "Test LA",
			"code": "301"
		}
		"""
	And blob storage file downloads-config.json exists in config container:
		"""
		[
			{
				"id": "kts-school-ks2-pupil",
				"source": "KTS",
				"scope": "School",
				"dataSetType": "KeyStage2",
				"label": "Key stage 2 (KS2)",
				"filePathPattern": "School/{urn}/{year}/{filetype}/ks2_pupil_{version}.{filetype}"
			}
		]
		"""
	And blob storage file School/222222/2022/csv/ks2_pupil_final.csv exists in downloads-kts container:
		"""
		Column A,Column B,Column C
		1,2,3
		"""
	When I navigate to /my-local-authority/download-data/individual-school-data/
	And I update the textbox "#app-field-Search" to have the value "<SearchTerm>"
	And I click the button "#searchSubmit"
	Then the path should be /my-local-authority/download-data/individual-school-data/222222/select-year/
	And the page title should be "Download data"
	And the page subtitle should be "Individual school data"
	And the sub-page title should be "Dates available for download" with caption "Another Primary School (URN: 222222)"

Examples:
	| SearchTerm |
	| 1          |
	| 11         |
	| 111        |
	| 1111       |
	| 11111      |

@Javascript:enabled
Scenario Outline: Data downloads > Individual school data > Search for a school - School search with less than 6 digits matches on school address (JS)
	Given Establishment "111111" exists:
		"""
		{
			"name": "Some Primary School",
			"localAuthority": {
				"code": "301",
		 		"name": "Test LA"
			}
		}
		"""
	And Establishment "222222" exists:
		"""
		{
			"name": "Another Primary School",
			"address": {
				"street": "<SearchTerm> The Street",
				"town": "SomeTown",
				"postCode": "TR18 3JT"
			},
			"localAuthority": {
				"code": "301",
				"name": "Test LA"
			}
		} 
		"""
	And Local Authority "301" exists:
		"""
		{
			"name": "Test LA",
			"code": "301"
		}
		"""
	And blob storage file downloads-config.json exists in config container:
		"""
		[
			{
				"id": "kts-school-ks2-pupil",
				"source": "KTS",
				"scope": "School",
				"dataSetType": "KeyStage2",
				"label": "Key stage 2 (KS2)",
				"filePathPattern": "School/{urn}/{year}/{filetype}/ks2_pupil_{version}.{filetype}"
			}
		]
		"""
	And blob storage file School/222222/2022/csv/ks2_pupil_final.csv exists in downloads-kts container:
		"""
		Column A,Column B,Column C
		1,2,3
		"""
	When I navigate to /my-local-authority/download-data/individual-school-data/
	And I update the textbox "#app-field-Search" to have the value "<SearchTerm>"
	And I click the button "#searchSubmit"
	Then the path should be /my-local-authority/download-data/individual-school-data/222222/select-year/
	And the page title should be "Download data"
	And the page subtitle should be "Individual school data"
	And the sub-page title should be "Dates available for download" with caption "Another Primary School (URN: 222222)"

Examples:
	| SearchTerm |
	| 1          |
	| 11         |
	| 111        |
	| 1111       |
	| 11111      |

@Javascript:disabled
Scenario: Data downloads > Individual school data > Search for a school - If searchTerm is a 6-digit number, treat it as an exact URN search
	Given Establishment "111111" exists:
		"""
		{
			"name": "Some Primary School",
			"localAuthority": {
				"code": "301",
				"name": "Test LA"
			}
		}
		"""
	And Establishment "222222" exists:
		"""
		{
			"name": "Another Primary School",
			"address": {
				"street": "111111 The Street",
				"town": "SomeTown",
				"postCode": "TR18 3JT"
			},
			"localAuthority": {
				"code": "301",
				"name": "Test LA"
			}
		} 
		"""
	And Local Authority "301" exists:
		"""
		{
			"name": "Test LA",
			"code": "301"
		}
		"""
	And blob storage file downloads-config.json exists in config container:
		"""
		[
			{
				"id": "kts-school-ks2-pupil",
				"source": "KTS",
				"scope": "School",
				"dataSetType": "KeyStage2",
				"label": "Key stage 2 (KS2)",
				"filePathPattern": "School/{urn}/{year}/{filetype}/ks2_pupil_{version}.{filetype}"
			}
		]
		"""
	And blob storage file School/111111/2022/csv/ks2_pupil_final.csv exists in downloads-kts container:
		"""
		Column A,Column B,Column C
		1,2,3
		"""
	When I navigate to /my-local-authority/download-data/individual-school-data/
	And I update the textbox "#app-field-Search" to have the value "111111"
	And I click the button "#searchSubmit"
	Then the path should be /my-local-authority/download-data/individual-school-data/111111/select-year/
	And the page title should be "Download data"
	And the page subtitle should be "Individual school data"
	And the sub-page title should be "Dates available for download" with caption "Some Primary School (URN: 111111)"

@Javascript:enabled
Scenario: Data downloads > Individual school data > Search for a school - If searchTerm is a 6-digit number, treat it as an exact URN search (JS)
	Given Establishment "111111" exists:
		"""
		{
			"name": "Some Primary School",
			"localAuthority": {
				"code": "301",
				"name": "Test LA"
			}
		}
		"""
	And Establishment "222222" exists:
		"""
		{
			"name": "Another Primary School",
			"address": {
				"street": "111111 The Street",
				"town": "SomeTown",
				"postCode": "TR18 3JT"
			},
			"localAuthority": {
				"code": "301",
				"name": "Test LA"
			}
		} 
		"""
	And Local Authority "301" exists:
		"""
		{
			"name": "Test LA",
			"code": "301"
		}
		"""
	And blob storage file downloads-config.json exists in config container:
		"""
		[
			{
				"id": "kts-school-ks2-pupil",
				"source": "KTS",
				"scope": "School",
				"dataSetType": "KeyStage2",
				"label": "Key stage 2 (KS2)",
				"filePathPattern": "School/{urn}/{year}/{filetype}/ks2_pupil_{version}.{filetype}"
			}
		]
		"""
	And blob storage file School/111111/2022/csv/ks2_pupil_final.csv exists in downloads-kts container:
		"""
		Column A,Column B,Column C
		1,2,3
		"""
	When I navigate to /my-local-authority/download-data/individual-school-data/
	And I update the textbox "#app-field-Search" to have the value "111111"
	And I click the button "#searchSubmit"
	Then the path should be /my-local-authority/download-data/individual-school-data/111111/select-year/
	And the page title should be "Download data"
	And the page subtitle should be "Individual school data"
	And the sub-page title should be "Dates available for download" with caption "Some Primary School (URN: 111111)"

@Javascript:disabled
Scenario: Data downloads > Individual school data > Search for a school - Search term matching establishment LAESTAB code (with forward slash)
	Given Establishment "111111" exists:
		"""
		{
			"name": "Some Primary School",
			"laestab": "894/2200",
			"localAuthority": {
				"code": "301",
				"name": "Test LA"
			}
		}
		"""
	And Local Authority "301" exists:
		"""
		{
			"name": "Test LA",
			"code": "301"
		}
		"""
	And blob storage file downloads-config.json exists in config container:
		"""
		[
			{
				"id": "kts-school-ks2-pupil",
				"source": "KTS",
				"scope": "School",
				"dataSetType": "KeyStage2",
				"label": "Key stage 2 (KS2)",
				"filePathPattern": "School/{urn}/{year}/{filetype}/ks2_pupil_{version}.{filetype}"
			}
		]
		"""
	And blob storage file School/111111/2022/csv/ks2_pupil_final.csv exists in downloads-kts container:
		"""
		Column A,Column B,Column C
		1,2,3
		"""
	When I navigate to /my-local-authority/download-data/individual-school-data/
	And I update the textbox "#app-field-Search" to have the value "894/2200"
	And I click the button "#searchSubmit"
	Then the path should be /my-local-authority/download-data/individual-school-data/111111/select-year/
	And the page title should be "Download data"
	And the page subtitle should be "Individual school data"
	And the sub-page title should be "Dates available for download" with caption "Some Primary School (URN: 111111)"

@Javascript:enabled
Scenario: Data downloads > Individual school data > Search for a school - Search term matching establishment LAESTAB code (with forward slash) (JS)
	Given Establishment "111111" exists:
		"""
		{
			"name": "Some Primary School",
			"laestab": "894/2200",
			"localAuthority": {
				"code": "301",
				"name": "Test LA"
			}
		}
		"""
	And Local Authority "301" exists:
		"""
		{
			"name": "Test LA",
			"code": "301"
		}
		"""
	And blob storage file downloads-config.json exists in config container:
		"""
		[
			{
				"id": "kts-school-ks2-pupil",
				"source": "KTS",
				"scope": "School",
				"dataSetType": "KeyStage2",
				"label": "Key stage 2 (KS2)",
				"filePathPattern": "School/{urn}/{year}/{filetype}/ks2_pupil_{version}.{filetype}"
			}
		]
		"""
	And blob storage file School/111111/2022/csv/ks2_pupil_final.csv exists in downloads-kts container:
		"""
		Column A,Column B,Column C
		1,2,3
		"""
	When I navigate to /my-local-authority/download-data/individual-school-data/
	And I update the textbox "#app-field-Search" to have the value "894/2200"
	And I click the button "#searchSubmit"
	Then the path should be /my-local-authority/download-data/individual-school-data/111111/select-year/
	And the page title should be "Download data"
	And the page subtitle should be "Individual school data"
	And the sub-page title should be "Dates available for download" with caption "Some Primary School (URN: 111111)"

@Javascript:disabled
Scenario: Data downloads > Individual school data > Search for a school - Search term matching establishment LAESTAB code (without forward slash)
	Given Establishment "111111" exists:
		"""
		{
			"name": "Some Primary School",
			"laestab": "894/2200",
			"localAuthority": {
				"code": "301",
				"name": "Test LA"
			}
		}
		"""
	And Local Authority "301" exists:
		"""
		{
			"name": "Test LA",
			"code": "301"
		}
		"""
	And blob storage file downloads-config.json exists in config container:
		"""
		[
			{
				"id": "kts-school-ks2-pupil",
				"source": "KTS",
				"scope": "School",
				"dataSetType": "KeyStage2",
				"label": "Key stage 2 (KS2)",
				"filePathPattern": "School/{urn}/{year}/{filetype}/ks2_pupil_{version}.{filetype}"
			}
		]
		"""
	And blob storage file School/111111/2022/csv/ks2_pupil_final.csv exists in downloads-kts container:
		"""
		Column A,Column B,Column C
		1,2,3
		"""
	When I navigate to /my-local-authority/download-data/individual-school-data/
	And I update the textbox "#app-field-Search" to have the value "8942200"
	And I click the button "#searchSubmit"
	Then the path should be /my-local-authority/download-data/individual-school-data/111111/select-year/
	And the page title should be "Download data"
	And the page subtitle should be "Individual school data"
	And the sub-page title should be "Dates available for download" with caption "Some Primary School (URN: 111111)"

@Javascript:enabled
Scenario: Data downloads > Individual school data > Search for a school - Search term matching establishment LAESTAB code (without forward slash) (JS)
	Given Establishment "111111" exists:
		"""
		{
			"name": "Some Primary School",
			"laestab": "894/2200",
			"localAuthority": {
				"code": "301",
				"name": "Test LA"
			}
		}
		"""
	And Local Authority "301" exists:
		"""
		{
			"name": "Test LA",
			"code": "301"
		}
		"""
	And blob storage file downloads-config.json exists in config container:
		"""
		[
			{
				"id": "kts-school-ks2-pupil",
				"source": "KTS",
				"scope": "School",
				"dataSetType": "KeyStage2",
				"label": "Key stage 2 (KS2)",
				"filePathPattern": "School/{urn}/{year}/{filetype}/ks2_pupil_{version}.{filetype}"
			}
		]
		"""
	And blob storage file School/111111/2022/csv/ks2_pupil_final.csv exists in downloads-kts container:
		"""
		Column A,Column B,Column C
		1,2,3
		"""
	When I navigate to /my-local-authority/download-data/individual-school-data/
	And I update the textbox "#app-field-Search" to have the value "8942200"
	And I click the button "#searchSubmit"
	Then the path should be /my-local-authority/download-data/individual-school-data/111111/select-year/
	And the page title should be "Download data"
	And the page subtitle should be "Individual school data"
	And the sub-page title should be "Dates available for download" with caption "Some Primary School (URN: 111111)"

@Javascript:disabled
Scenario Outline: Data downloads > Individual school data > Search for a school - School results page shows multiple partial LAESTAB matches (LA part)
	Given Establishment "111111" exists:
		"""
		{
			"name": "Some Primary School",
			"laestab": "894/2200",
			"localAuthority": {
				"code": "301",
				"name": "Test LA"
			}
		}
		"""
	And Establishment "222222" exists:
		"""
		{
			"name": "Some Other Primary School",
			"laestab": "894/1234",
			"localAuthority": {
				"code": "301",
				"name": "Test LA"
			}
		}
		"""
	And Local Authority "301" exists:
		"""
		{
			"name": "Test LA",
			"code": "301"
		}
		"""
	When I navigate to /my-local-authority/download-data/individual-school-data/
	And I update the textbox "#app-field-Search" to have the value "894"
	And I click the button "#searchSubmit"
	Then the path should be /my-local-authority/download-data/individual-school-data/?search=894
	And the element "[data-testid="establishment-listing-urn-<Counter>"]" should have the text content "<URN>"
	And the element "[data-testid="establishment-listing-name-<Counter>"]" should have the text content "<Name>"
	And the element "[data-testid="establishment-listing-laestab-<Counter>"]" should have the text content "<LAESTAB>"
	And the element "[data-testid="establishment-listing-name-<Counter>"]" should have the href "<Href>"

Examples:
	| Counter | URN    | LAESTAB  | Name                      | Href                                                                          |
	| 2       | 111111 | 894/2200 | Some Primary School       | /my-local-authority/download-data/individual-school-data/111111/select-year/ |
	| 1       | 222222 | 894/1234 | Some Other Primary School | /my-local-authority/download-data/individual-school-data/222222/select-year/ |

@Javascript:enabled
Scenario Outline: Data downloads > Individual school data > Search for a school - School results page shows multiple partial LAESTAB matches (LA part) (JS)
	Given Establishment "111111" exists:
		"""
		{
			"name": "Some Primary School",
			"laestab": "894/2200",
			"localAuthority": {
				"code": "301",
				"name": "Test LA"
			}
		}
		"""
	And Establishment "222222" exists:
		"""
		{
			"name": "Some Other Primary School",
			"laestab": "894/1234",
			"localAuthority": {
				"code": "301",
				"name": "Test LA"
			}
		}
		"""
	And Local Authority "301" exists:
		"""
		{
			"name": "Test LA",
			"code": "301"
		}
		"""
	When I navigate to /my-local-authority/download-data/individual-school-data/
	And I update the textbox "#app-field-Search" to have the value "894"
	And I click the button "#searchSubmit"
	Then the path should be /my-local-authority/download-data/individual-school-data/?search=894
	And the element "[data-testid="establishment-listing-urn-<Counter>"]" should have the text content "<URN>"
	And the element "[data-testid="establishment-listing-name-<Counter>"]" should have the text content "<Name>"
	And the element "[data-testid="establishment-listing-laestab-<Counter>"]" should have the text content "<LAESTAB>"

Examples:
	| Counter | URN    | LAESTAB  | Name                      | Href                                                                          |
	| 2       | 111111 | 894/2200 | Some Primary School       | /my-local-authority/download-data/individual-school-data/111111/select-year/ |
	| 1       | 222222 | 894/1234 | Some Other Primary School | /my-local-authority/download-data/individual-school-data/222222/select-year/ |

@Javascript:disabled
Scenario Outline: Data downloads > Individual school data > Search for a school - School results page shows multiple partial LAESTAB matches (ESTAB part)
	Given Establishment "111111" exists:
		"""
		{
			"name": "Some Primary School",
			"laestab": "894/2200",
			"localAuthority": {
				"code": "301",
		 		"name": "Test LA"
			}
		}
		"""
	And Establishment "222222" exists:
		"""
		{
			"name": "Some Other Primary School",
			"laestab": "600/2200",
			"localAuthority": {
				"code": "301",
		 		"name": "Test LA"
			}
		}
		"""
	And Local Authority "301" exists:
		"""
		{
			"name": "Test LA",
			"code": "301"
		}
		"""
	When I navigate to /my-local-authority/download-data/individual-school-data/
	And I update the textbox "#app-field-Search" to have the value "2200"
	And I click the button "#searchSubmit"
	Then the path should be /my-local-authority/download-data/individual-school-data/?search=2200
	And the element "[data-testid="establishment-listing-urn-<Counter>"]" should have the text content "<URN>"
	And the element "[data-testid="establishment-listing-name-<Counter>"]" should have the text content "<Name>"
	And the element "[data-testid="establishment-listing-laestab-<Counter>"]" should have the text content "<LAESTAB>"

Examples:
	| Counter | URN    | LAESTAB  | Name                      |
	| 2       | 111111 | 894/2200 | Some Primary School       |
	| 1       | 222222 | 600/2200 | Some Other Primary School |

@Javascript:enabled
Scenario Outline: Data downloads > Individual school data > Search for a school - School results page shows multiple partial LAESTAB matches (ESTAB part) (JS)
	Given Establishment "111111" exists:
		"""
		{
			"name": "Some Primary School",
			"laestab": "894/2200",
			"localAuthority": {
				"code": "301",
		 		"name": "Test LA"
			}
		}
		"""
	And Establishment "222222" exists:
		"""
		{
			"name": "Some Other Primary School",
			"laestab": "600/2200",
			"localAuthority": {
				"code": "301",
		 		"name": "Test LA"
			}
		}
		"""
	And Local Authority "301" exists:
		"""
		{
			"name": "Test LA",
			"code": "301"
		}
		"""
	When I navigate to /my-local-authority/download-data/individual-school-data/
	And I update the textbox "#app-field-Search" to have the value "2200"
	And I click the button "#searchSubmit"
	Then the path should be /my-local-authority/download-data/individual-school-data/?search=2200
	And the element "[data-testid="establishment-listing-urn-<Counter>"]" should have the text content "<URN>"
	And the element "[data-testid="establishment-listing-name-<Counter>"]" should have the text content "<Name>"
	And the element "[data-testid="establishment-listing-laestab-<Counter>"]" should have the text content "<LAESTAB>"

Examples:
	| Counter | URN    | LAESTAB  | Name                      |
	| 2       | 111111 | 894/2200 | Some Primary School       |
	| 1       | 222222 | 600/2200 | Some Other Primary School |

@Javascript:disabled
Scenario: Data downloads > Individual school data > Search for a school - Partial LAESTAB (LA part) match should show no matching results
	Given Establishment "111111" exists:
		"""
		{
			"name": "Some Primary School",
			"laestab" : "894/2200",
			"localAuthority": {
				"code": "301",
				"name": "Test LA"
			}
		}
		"""
	And Local Authority "301" exists:
		"""
		{
			"name": "Test LA",
			"code": "301"
		}
		"""
	And Local Authority "301" exists:
		"""
		{
			"name": "Test LA",
			"code": "301"
		}
		"""
	When I navigate to /my-local-authority/download-data/individual-school-data/
	And I update the textbox "#app-field-Search" to have the value "89"
	And I click the button "#searchSubmit"
	Then the path should be /my-local-authority/download-data/individual-school-data/?search=89
	And the page title should be "Download data"
	And the page subtitle should be "Individual school data"
	And the sub-page title should be "We found no matches for "89"" with caption "Individual school data"

@Javascript:enabled
Scenario: Data downloads > Individual school data > Search for a school - Partial LAESTAB (LA part) match should show no matching results (JS)
	Given Establishment "111111" exists:
		"""
		{
			"name": "Some Primary School",
			"laestab" : "894/2200",
			"localAuthority": {
				"code": "301",
				"name": "Test LA"
			}
		}
		"""
	And Local Authority "301" exists:
		"""
		{
			"name": "Test LA",
			"code": "301"
		}
		"""
	When I navigate to /my-local-authority/download-data/individual-school-data/
	And I update the textbox "#app-field-Search" to have the value "89"
	And I click the button "#searchSubmit"
	Then the path should be /my-local-authority/download-data/individual-school-data/?search=89
	And the page title should be "Download data"
	And the page subtitle should be "Individual school data"
	And the sub-page title should be "We found no matches for "89"" with caption "Individual school data"

@Javascript:disabled
Scenario: Data downloads > Individual school data > Search for a school - Partial LAESTAB (ESTAB only) match should show no matching results
	Given Establishment "111111" exists:
		"""
		{
			"name": "Some Primary School",
			"laestab" : "894/2200",
			"localAuthority": {
				"code": "301",
				"name": "Test LA"
			}
		}
		"""
	And Local Authority "301" exists:
		"""
		{
			"name": "Test LA",
			"code": "301"
		}
		"""
	When I navigate to /my-local-authority/download-data/individual-school-data/
	And I update the textbox "#app-field-Search" to have the value "22"
	And I click the button "#searchSubmit"
	Then the path should be /my-local-authority/download-data/individual-school-data/?search=22
	And the page title should be "Download data"
	And the page subtitle should be "Individual school data"
	And the sub-page title should be "We found no matches for "22"" with caption "Individual school data"

@Javascript:enabled
Scenario: Data downloads > Individual school data > Search for a school - Partial LAESTAB (ESTAB only) match should show no matching results (JS)
	Given Establishment "111111" exists:
		"""
		{
			"name": "Some Primary School",
			"laestab" : "894/2200",
			"localAuthority": {
				"code": "301",
				"name": "Test LA"
			}
		}
		"""
	And Local Authority "301" exists:
		"""
		{
			"name": "Test LA",
			"code": "301"
		}
		"""
	When I navigate to /my-local-authority/download-data/individual-school-data/
	And I update the textbox "#app-field-Search" to have the value "22"
	And I click the button "#searchSubmit"
	Then the path should be /my-local-authority/download-data/individual-school-data/?search=22
	And the page title should be "Download data"
	And the page subtitle should be "Individual school data"
	And the sub-page title should be "We found no matches for "22"" with caption "Individual school data"

@Javascript:disabled
Scenario: Data downloads > Individual school data > Search for a school - If searchTerm is a 7-digit number, treat it as an exact LAESTAB code search (ignoring other matching fields)
	Given Establishment "111111" exists:
		"""
		{
			"name": "Some Primary School",
			"laestab": "894/2200",
			"localAuthority": {
				"code": "301",
				"name": "Test LA"
			}
		}
		"""
	And Establishment "222222" exists:
		"""
		{
			"name": "Another Primary School",
			"laestab": "123/4567",
			"address": {
				"street": "8942200 The Street",
				"town": "SomeTown",
				"postCode": "TR18 3JT"
			},
			"localAuthority": {
				"code": "301",
				"name": "Test LA"
			}
		} 
		"""
	And Local Authority "301" exists:
		"""
		{
			"name": "Test LA",
			"code": "301"
		}
		"""
	And blob storage file downloads-config.json exists in config container:
		"""
		[
			{
				"id": "kts-school-ks2-pupil",
				"source": "KTS",
				"scope": "School",
				"dataSetType": "KeyStage2",
				"label": "Key stage 2 (KS2)",
				"filePathPattern": "School/{urn}/{year}/{filetype}/ks2_pupil_{version}.{filetype}"
			}
		]
		"""
	And blob storage file School/111111/2022/csv/ks2_pupil_final.csv exists in downloads-kts container:
		"""
		Column A,Column B,Column C
		1,2,3
		"""
	When I navigate to /my-local-authority/download-data/individual-school-data/
	And I update the textbox "#app-field-Search" to have the value "8942200"
	And I click the button "#searchSubmit"
	Then the path should be /my-local-authority/download-data/individual-school-data/111111/select-year/
	And the page title should be "Download data"
	And the page subtitle should be "Individual school data"
	And the sub-page title should be "Dates available for download" with caption "Some Primary School (URN: 111111)"

@Javascript:enabled
Scenario: Data downloads > Individual school data > Search for a school - If searchTerm is a 7-digit number, treat it as an exact LAESTAB code search (ignoring other matching fields) (JS)
	Given Establishment "111111" exists:
		"""
		{
			"name": "Some Primary School",
			"laestab": "894/2200",
			"localAuthority": {
				"code": "301",
				"name": "Test LA"
			}
		}
		"""
	And Establishment "222222" exists:
		"""
		{
			"name": "Another Primary School",
			"laestab": "123/4567",
			"address": {
				"street": "8942200 The Street",
				"town": "SomeTown",
				"postCode": "TR18 3JT"
			},
			"localAuthority": {
				"code": "301",
				"name": "Test LA"
			}
		} 
		"""
	And Local Authority "301" exists:
		"""
		{
			"name": "Test LA",
			"code": "301"
		}
		"""
	And blob storage file downloads-config.json exists in config container:
		"""
		[
			{
				"id": "kts-school-ks2-pupil",
				"source": "KTS",
				"scope": "School",
				"dataSetType": "KeyStage2",
				"label": "Key stage 2 (KS2)",
				"filePathPattern": "School/{urn}/{year}/{filetype}/ks2_pupil_{version}.{filetype}"
			}
		]
		"""
	And blob storage file School/111111/2022/csv/ks2_pupil_final.csv exists in downloads-kts container:
		"""
		Column A,Column B,Column C
		1,2,3
		"""
	When I navigate to /my-local-authority/download-data/individual-school-data/
	And I update the textbox "#app-field-Search" to have the value "8942200"
	And I click the button "#searchSubmit"
	Then the path should be /my-local-authority/download-data/individual-school-data/111111/select-year/
	And the page title should be "Download data"
	And the page subtitle should be "Individual school data"
	And the sub-page title should be "Dates available for download" with caption "Some Primary School (URN: 111111)"

@Javascript:disabled
Scenario: Data downloads > Individual school data > Search for a school - If searchTerm is a 7-digit number with forward slash in the right place, treat it as an exact LAESTAB code search (ignoring other matching fields)
	Given Establishment "111111" exists:
		"""
		{
			"name": "Some Primary School",
			"laestab": "894/2200",
			"localAuthority": {
				"code": "301",
				"name": "Test LA"
			}
		} 
		"""
	And Establishment "222222" exists:
		"""
		{
			"name": "Another Primary School",
			"laestab": "123/4567",
			"address": {
				"street": "894/2200 The Street",
				"town": "SomeTown",
				"postCode": "TR18 3JT"
			},
			"localAuthority": {
				"code": "301",
				"name": "Test LA"
			}
			
		} 
		"""
	And Local Authority "301" exists:
		"""
		{
			"name": "Test LA",
			"code": "301"
		}
		"""
	And blob storage file downloads-config.json exists in config container:
		"""
		[
			{
				"id": "kts-school-ks2-pupil",
				"source": "KTS",
				"scope": "School",
				"dataSetType": "KeyStage2",
				"label": "Key stage 2 (KS2)",
				"filePathPattern": "School/{urn}/{year}/{filetype}/ks2_pupil_{version}.{filetype}"
			}
		]
		"""
	And blob storage file School/111111/2022/csv/ks2_pupil_final.csv exists in downloads-kts container:
		"""
		Column A,Column B,Column C
		1,2,3
		"""
	When I navigate to /my-local-authority/download-data/individual-school-data/
	And I update the textbox "#app-field-Search" to have the value "894/2200"
	And I click the button "#searchSubmit"
	Then the path should be /my-local-authority/download-data/individual-school-data/111111/select-year/
	And the page title should be "Download data"
	And the page subtitle should be "Individual school data"
	And the sub-page title should be "Dates available for download" with caption "Some Primary School (URN: 111111)"

@Javascript:enabled
Scenario: Data downloads > Individual school data > Search for a school - If searchTerm is a 7-digit number with forward slash in the right place, treat it as an exact LAESTAB code search (ignoring other matching fields) (JS)
	Given Establishment "111111" exists:
		"""
		{
			"name": "Some Primary School",
			"laestab": "894/2200",
			"localAuthority": {
				"code": "301",
				"name": "Test LA"
			}
		} 
		"""
	And Establishment "222222" exists:
		"""
		{
			"name": "Another Primary School",
			"laestab": "123/4567",
			"address": {
				"street": "894/2200 The Street",
				"town": "SomeTown",
				"postCode": "TR18 3JT"
			},
			"localAuthority": {
				"code": "301",
				"name": "Test LA"
			}
			
		} 
		"""
	And Local Authority "301" exists:
		"""
		{
			"name": "Test LA",
			"code": "301"
		}
		"""
	And blob storage file downloads-config.json exists in config container:
		"""
		[
			{
				"id": "kts-school-ks2-pupil",
				"source": "KTS",
				"scope": "School",
				"dataSetType": "KeyStage2",
				"label": "Key stage 2 (KS2)",
				"filePathPattern": "School/{urn}/{year}/{filetype}/ks2_pupil_{version}.{filetype}"
			}
		]
		"""
	And blob storage file School/111111/2022/csv/ks2_pupil_final.csv exists in downloads-kts container:
		"""
		Column A,Column B,Column C
		1,2,3
		"""
	When I navigate to /my-local-authority/download-data/individual-school-data/
	And I update the textbox "#app-field-Search" to have the value "894/2200"
	And I click the button "#searchSubmit"
	Then the path should be /my-local-authority/download-data/individual-school-data/111111/select-year/
	And the page title should be "Download data"
	And the page subtitle should be "Individual school data"
	And the sub-page title should be "Dates available for download" with caption "Some Primary School (URN: 111111)"

@Javascript:disabled
Scenario: Data downloads > Individual school data > Search for a school - If searchTerm is a 3-digit number, treat it as an exact LA code search (ignoring other matching fields)
	Given Establishment "111111" exists:
		"""
		{
			"name": "Some Primary School",
			"laestab": "894/2200",
			"localAuthority": {
				"code": "301",
				"name": "Test LA"
			}		 
		}
		"""
	And Establishment "222222" exists:
		"""
		{
			"name": "Another Primary School",
			"laestab": "123/4567",
			"address": {
				"street": "894 The Street",
				"town": "SomeTown",
				"postCode": "TR18 3JT"
			},
			"localAuthority": {
				"code": "301",
				"name": "Test LA"
			}		 
		} 
		"""
	And Local Authority "301" exists:
		"""
		{
			"name": "Test LA",
			"code": "301"
		}
		"""
	And blob storage file downloads-config.json exists in config container:
		"""
		[
			{
				"id": "kts-school-ks2-pupil",
				"source": "KTS",
				"scope": "School",
				"dataSetType": "KeyStage2",
				"label": "Key stage 2 (KS2)",
				"filePathPattern": "School/{urn}/{year}/{filetype}/ks2_pupil_{version}.{filetype}"
			}
		]
		"""
	And blob storage file School/111111/2022/csv/ks2_pupil_final.csv exists in downloads-kts container:
		"""
		Column A,Column B,Column C
		1,2,3
		"""
	When I navigate to /my-local-authority/download-data/individual-school-data/
	And I update the textbox "#app-field-Search" to have the value "894"
	And I click the button "#searchSubmit"
	Then the path should be /my-local-authority/download-data/individual-school-data/111111/select-year/
	And the page title should be "Download data"
	And the page subtitle should be "Individual school data"
	And the sub-page title should be "Dates available for download" with caption "Some Primary School (URN: 111111)"

@Javascript:enabled
Scenario: Data downloads > Individual school data > Search for a school - If searchTerm is a 3-digit number, treat it as an exact LA code search (ignoring other matching fields) (JS)
	Given Establishment "111111" exists:
		"""
		{
			"name": "Some Primary School",
			"laestab": "894/2200",
			"localAuthority": {
				"code": "301",
				"name": "Test LA"
			}		 
		}
		"""
	And Establishment "222222" exists:
		"""
		{
			"name": "Another Primary School",
			"laestab": "123/4567",
			"address": {
				"street": "894 The Street",
				"town": "SomeTown",
				"postCode": "TR18 3JT"
			},
			"localAuthority": {
				"code": "301",
				"name": "Test LA"
			}		 
		} 
		"""
	And Local Authority "301" exists:
		"""
		{
			"name": "Test LA",
			"code": "301"
		}
		"""
	And blob storage file downloads-config.json exists in config container:
		"""
		[
			{
				"id": "kts-school-ks2-pupil",
				"source": "KTS",
				"scope": "School",
				"dataSetType": "KeyStage2",
				"label": "Key stage 2 (KS2)",
				"filePathPattern": "School/{urn}/{year}/{filetype}/ks2_pupil_{version}.{filetype}"
			}
		]
		"""
	And blob storage file School/111111/2022/csv/ks2_pupil_final.csv exists in downloads-kts container:
		"""
		Column A,Column B,Column C
		1,2,3
		"""
	When I navigate to /my-local-authority/download-data/individual-school-data/
	And I update the textbox "#app-field-Search" to have the value "894"
	And I click the button "#searchSubmit"
	Then the path should be /my-local-authority/download-data/individual-school-data/111111/select-year/
	And the page title should be "Download data"
	And the page subtitle should be "Individual school data"
	And the sub-page title should be "Dates available for download" with caption "Some Primary School (URN: 111111)"

@Javascript:disabled
Scenario: Data downloads > Individual school data > Search for a school - if searchTerm is a 4-digit number, treat it as an exact ESTAB code search (ignoring other matching fields)
	Given Establishment "111111" exists:
		"""
		{
			"name": "Some Primary School",
			"laestab": "894/2200",
			"localAuthority": {
				"code": "301",
				"name": "Test LA"
			}
		}
		"""
	And Establishment "222222" exists:
		"""
		{
			"name": "Another Primary School",
			"laestab": "123/4567",
			"address": {
				"street": "2200 The Street",
				"town": "SomeTown",
				"postCode": "TR18 3JT"
			},
			"localAuthority": {
				"code": "301",
				"name": "Test LA"
			}
		} 
		"""
	And Local Authority "301" exists:
		"""
		{
			"name": "Test LA",
			"code": "301"
		}
		"""
	And blob storage file downloads-config.json exists in config container:
		"""
		[
			{
				"id": "kts-school-ks2-pupil",
				"source": "KTS",
				"scope": "School",
				"dataSetType": "KeyStage2",
				"label": "Key stage 2 (KS2)",
				"filePathPattern": "School/{urn}/{year}/{filetype}/ks2_pupil_{version}.{filetype}"
			}
		]
		"""
	And blob storage file School/111111/2022/csv/ks2_pupil_final.csv exists in downloads-kts container:
		"""
		Column A,Column B,Column C
		1,2,3
		"""
	When I navigate to /my-local-authority/download-data/individual-school-data/
	And I update the textbox "#app-field-Search" to have the value "2200"
	And I click the button "#searchSubmit"
	Then the path should be /my-local-authority/download-data/individual-school-data/111111/select-year/
	And the page title should be "Download data"
	And the page subtitle should be "Individual school data"
	And the sub-page title should be "Dates available for download" with caption "Some Primary School (URN: 111111)"

@Javascript:enabled
Scenario: Data downloads > Individual school data > Search for a school - if searchTerm is a 4-digit number, treat it as an exact ESTAB code search (ignoring other matching fields) (JS)
	Given Establishment "111111" exists:
		"""
		{
			"name": "Some Primary School",
			"laestab": "894/2200",
			"localAuthority": {
				"code": "301",
				"name": "Test LA"
			}
		}
		"""
	And Establishment "222222" exists:
		"""
		{
			"name": "Another Primary School",
			"laestab": "123/4567",
			"address": {
				"street": "2200 The Street",
				"town": "SomeTown",
				"postCode": "TR18 3JT"
			},
			"localAuthority": {
				"code": "301",
				"name": "Test LA"
			}
		} 
		"""
	And Local Authority "301" exists:
		"""
		{
			"name": "Test LA",
			"code": "301"
		}
		"""
	And blob storage file downloads-config.json exists in config container:
		"""
		[
			{
				"id": "kts-school-ks2-pupil",
				"source": "KTS",
				"scope": "School",
				"dataSetType": "KeyStage2",
				"label": "Key stage 2 (KS2)",
				"filePathPattern": "School/{urn}/{year}/{filetype}/ks2_pupil_{version}.{filetype}"
			}
		]
		"""
	And blob storage file School/111111/2022/csv/ks2_pupil_final.csv exists in downloads-kts container:
		"""
		Column A,Column B,Column C
		1,2,3
		"""
	When I navigate to /my-local-authority/download-data/individual-school-data/
	And I update the textbox "#app-field-Search" to have the value "2200"
	And I click the button "#searchSubmit"
	Then the path should be /my-local-authority/download-data/individual-school-data/111111/select-year/
	And the page title should be "Download data"
	And the page subtitle should be "Individual school data"
	And the sub-page title should be "Dates available for download" with caption "Some Primary School (URN: 111111)"

@Javascript:disabled
Scenario Outline: Data downloads > Individual school data > Search for a school - Multiple successful school name matches show correct search results
	Given Establishment "111111" exists:
		"""
		{
			"name": "School A",
			"address": {
				"street": "13 The Street",
				"postCode": "AB12 3CD"
			},
			"localAuthority": {
				"code": "301",
				"name": "Test LA"
			}
		}
		"""
	And Establishment "222222" exists:
		"""
		{
			"name": "School B",
			"address": {
				"street": "2a Mornington Crescent",
				"town": "Liverpool",
				"postCode": "LL1 1AB"
			},
			"localAuthority": {
				"code": "301",
				"name": "Test LA"
			}
		}
		"""
	And Establishment "333333" exists:
		"""
		{
			"name": "School C",
			"address": {
				"street": "34 Long Road",
				"town": "Sheffield"
			},
			"localAuthority": {
				"code": "301",
				"name": "Test LA"
			}
		}
		"""
	And Local Authority "301" exists:
		"""
		{
			"name": "Test LA",
			"code": "301"
		}
		"""
	When I navigate to /my-local-authority/download-data/individual-school-data/
	And I update the textbox "#app-field-Search" to have the value "School"
	And I click the button "#searchSubmit"
	Then the path should be /my-local-authority/download-data/individual-school-data/?search=School
	And the element "[data-testid="establishment-listing-urn-<Counter>"]" should have the text content "<URN>"
	And the element "[data-testid="establishment-listing-name-<Counter>"]" should have the text content "<Name>"
	And the element "[data-testid="establishment-listing-address-<Counter>"]" should have the text content "<Address>"
	And the element "[data-testid="establishment-listing-name-<Counter>"]" should have the href "<Href>"

Examples:
	| Counter | URN    | Name     | Address                                   | Href                                                                          |
	| 1       | 111111 | School A | 13 The Street AB12 3CD                    | /my-local-authority/download-data/individual-school-data/111111/select-year/ |
	| 2       | 222222 | School B | 2a Mornington Crescent, Liverpool LL1 1AB | /my-local-authority/download-data/individual-school-data/222222/select-year/ |
	| 3       | 333333 | School C | 34 Long Road, Sheffield                   | /my-local-authority/download-data/individual-school-data/333333/select-year/ |

@Javascript:enabled
Scenario Outline: Data downloads > Individual school data > Search for a school - Multiple successful school name matches show correct search results (JS)
	Given Establishment "111111" exists:
		"""
		{
			"name": "School A",
			"address": {
				"street": "13 The Street",
				"postCode": "AB12 3CD"
			},
			"localAuthority": {
				"code": "301",
				"name": "Test LA"
			}
		}
		"""
	And Establishment "222222" exists:
		"""
		{
			"name": "School B",
			"address": {
				"street": "2a Mornington Crescent",
				"town": "Liverpool",
				"postCode": "LL1 1AB"
			},
			"localAuthority": {
				"code": "301",
				"name": "Test LA"
			}
		}
		"""
	And Establishment "333333" exists:
		"""
		{
			"name": "School C",
			"address": {
				"street": "34 Long Road",
				"town": "Sheffield"
			},
			"localAuthority": {
				"code": "301",
				"name": "Test LA"
			}
		}
		"""
	And Local Authority "301" exists:
		"""
		{
			"name": "Test LA",
			"code": "301"
		}
		"""
	When I navigate to /my-local-authority/download-data/individual-school-data/
	And I update the textbox "#app-field-Search" to have the value "School"
	And I click the button "#searchSubmit"
	Then the path should be /my-local-authority/download-data/individual-school-data/?search=School
	And the element "[data-testid="establishment-listing-urn-<Counter>"]" should have the text content "<URN>"
	And the element "[data-testid="establishment-listing-name-<Counter>"]" should have the text content "<Name>"
	And the element "[data-testid="establishment-listing-address-<Counter>"]" should have the text content "<Address>"

Examples:
	| Counter | URN    | Name     | Address                                   | Href                                                                          |
	| 1       | 111111 | School A | 13 The Street AB12 3CD                    | /my-local-authority/download-data/individual-school-data/111111/select-year/ |
	| 2       | 222222 | School B | 2a Mornington Crescent, Liverpool LL1 1AB | /my-local-authority/download-data/individual-school-data/222222/select-year/ |
	| 3       | 333333 | School C | 34 Long Road, Sheffield                   | /my-local-authority/download-data/individual-school-data/333333/select-year/ |

@Javascript:disabled
Scenario Outline: Data downloads > Individual school data > Search for a school - The PageNo parameter should handle invalid values with a default value of 1
	Given Establishment "111111" exists:
		"""
		{
			"name": "Some Primary School",
			"address": {
				"street": "13 The Street",
				"town": "SomeTown",
				"postCode": "B1 1AA"
			},
			"localAuthority": {
				"code": "301",
		 		"name": "Test LA"
			}
		}
		"""
	And Establishment "222222" exists:
		"""
		{
			"name": "Some Other Primary School",
			"address": {
				"street": "13 The Road",
				"town": "Tring",
				"postCode": "B1 1AA"
			},
			"localAuthority": {
				"code": "301",
		 		"name": "Test LA"
			}
		}
		"""
	And Local Authority "301" exists:
		"""
		{
			"name": "Test LA",
			"code": "301"
		}
		"""
	When I navigate to /my-local-authority/download-data/individual-school-data/?search=Primary&page=<page>
	Then the page title should be "Download data"
	And the page subtitle should be "Individual school data"
	And the sub-page title should be "Search results for "Primary"" with caption "Individual school data"
	And the element "*[data-testid='NumberOfPages-Footer']" should have the text content "Showing 1 - 2 of 2 schools"
	And the elements "*[data-testid='PageLinks-Footer-1']" should all have the href "/my-local-authority/download-data/individual-school-data/?search=Primary&page=1"

Examples:
	| page |
	| y    |
	| 1.5  |
	| 0    |
	| -1   |

@Javascript:disabled
Scenario: Data downloads > Individual school data > Search for a school - The PageNo parameter number greater than the total number of pages, the last page of results should be shown
	Given 26 Establishments exist with properties:
		| urn          | name                        | localAuthority                       |
		| (100000 + n) | Primary School (100000 + n) | { "code": "301", "name": "Test LA" } |
	And Local Authority "301" exists:
		"""
		{
			"name": "Test LA",
			"code": "301"
		}
		"""
	When I navigate to /my-local-authority/download-data/individual-school-data/?page=50&search=Primary
	Then the page title should be "Download data"
	And the page subtitle should be "Individual school data"
	And the sub-page title should be "Search results for "Primary"" with caption "Individual school data"
	And the element "*[data-testid='NumberOfPages-Footer']" should have the text content "Showing 1 - 26 of 26 schools"
	And the elements "*[data-testid='PageLinks-Footer-1']" should all have the href "/my-local-authority/download-data/individual-school-data/?search=Primary&page=1"
	And the element "*[data-testid='establishment-listing-name-1']" should have the text content "Primary School 100001"
	And the element "*[data-testid='establishment-listing-name-26']" should have the text content "Primary School 100026" 
		
@Javascript:enabled
Scenario: Data downloads > Individual school data > Search for a school - Autocomplete Should Populate Items When Two Or More Characters Entered
	Given Establishment "111111" exists:
		"""
		{
			"name": "Some Primary School",
			"address": {
				"street": "13 The Street",
				"town": "SomeTown",
				"postCode": "B1 1AA"
			},
			"laestab": "894/2200",
			"localAuthority": {
				"code": "301",
		 		"name": "Test LA"
			}
		}
		"""
	And Establishment "222222" exists:
		"""
		{
			"name": "Some Other Primary School",
			"address": {
				"street": "13 The Road",
				"town": "Tring",
				"postCode": "B1 1AA"
			},
			"laestab": "894/2201",
			"localAuthority": {
				"code": "301",
		 		"name": "Test LA"
			}
		}
		"""
	And Establishment "333333" exists:
		"""
		{
			"name": "A Different Primary School",
			"address": {
				"street": "13 The Road",
				"town": "SomeTown",
				"postCode": "TR18 3JT"
			},
			"laestab": "894/2202",
			"localAuthority": {
				"code": "301",
		 		"name": "Test LA"
			}
		}
		"""
	And Establishment "444444" exists:
		"""
		{
			"name": "Some Secondary School",
			"address": {
				"street": "13 The Road",
				"town": "SomeTown",
				"postCode": "TR18 3JT"
			},
			"laestab": "894/2203",
			"localAuthority": {
				"code": "301",
		 		"name": "Test LA"
			}
		}
		"""
	And Local Authority "301" exists:
		"""
		{
			"name": "Test LA",
			"code": "301"
		}
		"""
	When I navigate to /my-local-authority/download-data/individual-school-data/
	And I update the textbox "#app-field-Search" to have the value "primary"
	Then the autocomplete results should appear
	Then there should be 3 autocomplete items
	Then the elements ".autocomplete__option strong" should have the text contents:
		| Highlighted Values |
		| Primary            |
		| Primary            |
		| Primary            |
	Then the elements ".autocomplete__option" should have the text contents:
		| Autocomplete Items                                                                             |
		| A Different Primary School Address:13 The Road, SomeTown TR18 3JT URN:333333, LAESTAB:894/2202 |
		| Some Other Primary School Address:13 The Road, Tring B1 1AA URN:222222, LAESTAB:894/2201       |
		| Some Primary School Address:13 The Street, SomeTown B1 1AA URN:111111, LAESTAB:894/2200        |

@Javascript:enabled
Scenario: Data downloads > Individual school data > Search for a school - Autocomplete Should Populate Items When Two Or More Characters Entered Highlighting Name and Address
	Given Establishment "111111" exists:
		"""
		{
			"name": "Some Primary School",
			"address": {
				"street": "13 The Street",
				"town": "SomeTown",
				"postCode": "B1 1AA"
			},
			"laestab": "894/2200",
			"localAuthority": {
				"code": "301",
		 		"name": "Test LA"
			}
		}
		"""
	And Establishment "222222" exists:
		"""
		{
			"name": "Some Other Primary School",
			"address": {
				"street": "13 The Road",
				"town": "Tring",
				"postCode": "B1 1AA"
			},
			"laestab": "894/2201",
			"localAuthority": {
				"code": "301",
		 		"name": "Test LA"
			}
		}
		"""
	And Establishment "333333" exists:
		"""
		{
			"name": "A Different Primary School Centre",
			"address": {
				"street": "13 The Road",
				"town": "SomeTown",
				"postCode": "TR18 3JT"
			},
			"laestab": "894/2202",
			"localAuthority": {
				"code": "301",
		 		"name": "Test LA"
			}
		}
		"""
	And Establishment "444444" exists:
		"""
		{
			"name": "Some Secondary School",
			"address": {
				"street": "13 The Road",
				"town": "SomeTown",
				"postCode": "TR18 3JT"
			},
			"laestab": "894/2203",
			"localAuthority": {
				"code": "301",
		 		"name": "Test LA"
			}
		}
		"""
	And Local Authority "301" exists:
		"""
		{
			"name": "Test LA",
			"code": "301"
		}
		"""
	When I navigate to /my-local-authority/download-data/individual-school-data/
	And I update the textbox "#app-field-Search" to have the value "tr"
	Then the autocomplete results should appear
	Then there should be 4 autocomplete items
	Then the elements ".autocomplete__option strong" should have the text contents:
		| Highlighted Values |
		| tr                 |
		| TR                 |
		| Tr                 |
		| tr                 |
		| TR                 |
	Then the elements ".autocomplete__option" should have the text contents:
		| Autocomplete Items                                                                                    |
		| A Different Primary School Centre Address:13 The Road, SomeTown TR18 3JT URN:333333, LAESTAB:894/2202 |
		| Some Other Primary School Address:13 The Road, Tring B1 1AA URN:222222, LAESTAB:894/2201              |
		| Some Primary School Address:13 The Street, SomeTown B1 1AA URN:111111, LAESTAB:894/2200               |
		| Some Secondary School Address:13 The Road, SomeTown TR18 3JT URN:444444, LAESTAB:894/2203             |

@Javascript:enabled
Scenario: Data downloads > Individual school data > Search for a school - Autocomplete Should Populate Items When Two Or More Characters Entered Highlighting URN and LaEstab
	Given Establishment "111111" exists:
		"""
		{
			"name": "Some Primary School",
			"address": {
				"street": "13 The Street",
				"town": "SomeTown",
				"postCode": "B1 1AA"
			},
			"laestab": "894/2200",
			"localAuthority": {
				"code": "301",
		 		"name": "Test LA"
			}
		}
		"""
	And Establishment "222222" exists:
		"""
		{
			"name": "Some Other Primary School",
			"address": {
				"street": "13 The Road",
				"town": "Tring",
				"postCode": "B1 1AA"
			},
			"laestab": "894/2201",
			"localAuthority": {
				"code": "301",
		 		"name": "Test LA"
			}
		}
		"""
	And Establishment "333333" exists:
		"""
		{
			"name": "A Different Primary School Centre",
			"address": {
				"street": "13 The Road",
				"town": "SomeTown",
				"postCode": "TR18 3JT"
			},
			"laestab": "894/2202",
			"localAuthority": {
				"code": "301",
		 		"name": "Test LA"
			}
		}
		"""
	And Establishment "444442" exists:
		"""
		{
			"name": "Some Secondary School",
			"address": {
				"street": "13 The Road",
				"town": "SomeTown",
				"postCode": "TR18 3JT"
			},
			"laestab": "894/2203",
			"localAuthority": {
				"code": "301",
		 		"name": "Test LA"
			}
		}
		"""
	And Local Authority "301" exists:
		"""
		{
			"name": "Test LA",
			"code": "301"
		}
		"""
	When I navigate to /my-local-authority/download-data/individual-school-data/
	And I update the textbox "#app-field-Search" to have the value "42"
	Then the autocomplete results should appear
	Then there should be 4 autocomplete items
	Then the elements ".autocomplete__option strong" should have the text contents:
		| Highlighted Values |
		| 42                 |
		| 4/2                |
		| 4/2                |
		| 4/2                |
		| 4/2                |
	Then the elements ".autocomplete__option" should have the text contents:
		| Autocomplete Items                                                                                    |
		| Some Secondary School Address:13 The Road, SomeTown TR18 3JT URN:444442, LAESTAB:894/2203             |
		| Some Primary School Address:13 The Street, SomeTown B1 1AA URN:111111, LAESTAB:894/2200               |
		| Some Other Primary School Address:13 The Road, Tring B1 1AA URN:222222, LAESTAB:894/2201              |
		| A Different Primary School Centre Address:13 The Road, SomeTown TR18 3JT URN:333333, LAESTAB:894/2202 |       

@Javascript:disabled
Scenario: Data downloads > Individual school data > Dates available for download - Common page elements
	Given Local Authority "301" exists:
		"""
		{
			"name": "Test LA",
		}
		"""
	And Establishment "111111" exists:
		"""
		{
			"name": "Test School 1",
			"localAuthority":
			{
				"code": "301"
			}
		}
		"""
	And Establishment "222222" exists:
		"""
		{
			"name": "Test School 2",
			"localAuthority":
			{
				"code": "301"
			}
		}
		"""
	And blob storage file downloads-config.json exists in config container:
		"""
		[
			{
				"id": "kts-school-ks2-pupil",
				"source": "KTS",
				"scope": "School",
				"dataSetType": "KeyStage2",
				"label": "Key stage 2 (KS2)",
				"filePathPattern": "School/{urn}/{year}/{filetype}/ks2_pupil_{version}.{filetype}"
			}
		]
		"""
	And blob storage file School/111111/2022/csv/ks2_pupil_final.csv exists in downloads-kts container:
		"""
		Column A,Column B,Column C
		1,2,3
		"""
	When I navigate to /my-local-authority/download-data/individual-school-data/111111/select-year/
	Then I should get a 200 response
	And the page title should be "Download data"
	And the page subtitle should be "Individual school data"
	And the breadcrumb trail should be:
		| text                         | href                                                      | current |
		| Home                         | /                                                         |         |
		| My local authority           | /my-local-authority/                                      |         |
		| Download data                | /my-local-authority/download-data/                        |         |
		| Search for a school          | /my-local-authority/download-data/individual-school-data/ |         |
		| Dates available for download |                                                           | true    |
	And the sub-navigation should be:
		| text          | href                                | current |
		| Download data | /my-local-authority/download-data/ | true    |
	And the side navigation should be:
		| text                               | href                                                              | current |
		| Pupil level and aggregated LA data | /my-local-authority/download-data/pupil-level-aggregated-la-data/ |         |
		| Individual school data             | /my-local-authority/download-data/individual-school-data/         | true    |
	And the sub-page title should be "Dates available for download" with caption "Test School 1 (URN: 111111)"

@Javascript:disabled
Scenario Outline: Data downloads > Individual school data > Dates Available for Download - Page should contain multiple radio buttons
	Given Local Authority "301" exists:
		"""
		{
			"name": "Test LA"
		}
		"""
	And Establishment "111111" exists:
		"""
		{
			"name": "Test School 1",
			"localAuthority":
			{
				"code": "301"
			}
		}
		"""
	And Establishment "222222" exists:
		"""
		{
			"name": "Test School 2",
			"localAuthority":
			{
				"code": "301"
			}
		}
		"""
	And blob storage file downloads-config.json exists in config container:
		"""
		[
			{
				"id": "kts-school-ks2-pupil",
				"source": "KTS",
				"scope": "School",
				"dataSetType": "KeyStage2",
				"label": "Key stage 2 (KS2)",
				"filePathPattern": "School/{urn}/{year}/{filetype}/ks2_pupil_{version}.{filetype}"
			}
		]
		"""
	And blob storage file School/111111/2022/csv/ks2_pupil_final.csv exists in downloads-kts container:
		"""
		Column A,Column B,Column C
		1,2,3
		"""
	And blob storage file School/111111/2023/csv/ks2_pupil_final.csv exists in downloads-kts container:
		"""
		Column A,Column B,Column C
		1,2,3
		"""
	And blob storage file School/111111/2024/csv/ks2_pupil_final.csv exists in downloads-kts container:
		"""
		Column A,Column B,Column C
		1,2,3
		"""
	When I navigate to /my-local-authority/download-data/individual-school-data/111111/select-year/
	Then I should get a 200 response
	And the element "[data-testid='available-downloads-dates-<year>-label']" should have the text content "<label>"

Examples:
	| year | label        |
	| 2022 | 2021 to 2022 |
	| 2023 | 2022 to 2023 |
	| 2024 | 2023 to 2024 |

@Javascript:disabled
Scenario: Data downloads > Individual school data > Dates available for download - When no date is selected and Continue button clicked, should show validation error
	Given Local Authority "301" exists:
		"""
		{
			"name": "Test LA"
		}
		"""
	And Establishment "111111" exists:
		"""
		{
			"name": "Test School 1",
			"localAuthority":
			{
				"code": "301"
			}
		}
		"""
	And Establishment "222222" exists:
		"""
		{
			"name": "Test School 2",
			"localAuthority":
			{
				"code": "301"
			}
		}
		"""
	And blob storage file downloads-config.json exists in config container:
		"""
		[
			{
				"id": "kts-school-ks2-pupil",
				"source": "KTS",
				"scope": "School",
				"dataSetType": "KeyStage2",
				"label": "Key stage 2 (KS2)",
				"filePathPattern": "School/{urn}/{year}/{filetype}/ks2_pupil_{version}.{filetype}"
			}
		]
		"""
	And blob storage file School/111111/2022/csv/ks2_pupil_final.csv exists in downloads-kts container:
		"""
		Column A,Column B,Column C
		1,2,3
		"""
	When I navigate to /my-local-authority/download-data/individual-school-data/111111/select-year/
	And I click the button "*[data-testid='selectedYearSubmit']"
	Then the path should be /my-local-authority/download-data/individual-school-data/111111/select-year/
	And the element "*[data-testid='app-error-summary'] h2" should have the text content "There is a problem"
	And the element "*[data-testid='app-error-summary-selectedYear']" should have the text content "Please choose an academic year to download"
	And the element "*[data-testid='app-error-summary-selectedYear']" should have the href "#app-field-selectedYear"
	And the element "*[data-testid='app-field-selectedYear-error']" should have the text content "Please choose an academic year to download"

@Javascript:disabled
Scenario: Data downloads > Individual school data > Dates available for download - When date is selected and Continue button clicked, should move to next step
	Given Local Authority "301" exists:
		"""
		{
			"name": "Test LA"
		}
		"""
	And Establishment "111111" exists:
		"""
		{
			"name": "Test School 1",
			"localAuthority":
			{
				"code": "301"
			}
		}
		"""
	And Establishment "222222" exists:
		"""
		{
			"name": "Test School 2",
			"localAuthority":
			{
				"code": "301"
			}
		}
		"""
	And blob storage file downloads-config.json exists in config container:
		"""
		[
			{
				"id": "kts-school-ks2-pupil",
				"source": "KTS",
				"scope": "School",
				"dataSetType": "KeyStage2",
				"label": "Key stage 2 (KS2)",
				"filePathPattern": "School/{urn}/{year}/{filetype}/ks2_pupil_{version}.{filetype}"
			}
		]
		"""
	And blob storage file School/111111/2022/csv/ks2_pupil_final.csv exists in downloads-kts container:
		"""
		Column A,Column B,Column C
		1,2,3
		"""
	When I navigate to /my-local-authority/download-data/individual-school-data/111111/select-year/
	And I update the element "#app-available-downloads-dates-2022" to be checked
	And I click the button "*[data-testid='selectedYearSubmit']"
	Then the path should be /my-local-authority/download-data/individual-school-data/111111/select-files/?selectedYear=2022

@Javascript:disabled
Scenario: Data downloads > Individual school data > Data files available for download - Common page elements
	Given Local Authority "301" exists:
		"""
		{
			"name": "Test LA",
		}
		"""
	And Establishment "111111" exists:
		"""
		{
			"name": "Test School 1",
			"localAuthority":
			{
				"code": "301"
			}
		}
		"""
	And Establishment "222222" exists:
		"""
		{
			"name": "Test School 2",
			"localAuthority":
			{
				"code": "301"
			}
		}
		"""
	And blob storage file downloads-config.json exists in config container:
		"""
		[
			{
				"id": "kts-school-ks2-pupil",
				"source": "KTS",
				"scope": "School",
				"dataSetType": "KeyStage2",
				"label": "Key stage 2 (KS2)",
				"filePathPattern": "School/{urn}/{year}/{filetype}/ks2_pupil_{version}.{filetype}"
			}
		]
		"""
	And blob storage file School/111111/2022/csv/ks2_pupil_final.csv exists in downloads-kts container:
		"""
		Column A,Column B,Column C
		1,2,3
		"""
	When I navigate to /my-local-authority/download-data/individual-school-data/111111/select-files/?selectedYear=2022
	Then I should get a 200 response
	And the page title should be "Download data"
	And the page subtitle should be "Individual school data"
	And the breadcrumb trail should be:
		| text                              | href                                                                         | current |
		| Home                              | /                                                                            |         |
		| My local authority                | /my-local-authority/                                                         |         |
		| Download data                     | /my-local-authority/download-data/                                           |         |
		| Search for a school               | /my-local-authority/download-data/individual-school-data/                    |         |
		| Dates available for download      | /my-local-authority/download-data/individual-school-data/111111/select-year/ |         |
		| Data files available for download |                                                                              | true    |
	And the sub-navigation should be:
		| text          | href                                | current |
		| Download data | /my-local-authority/download-data/ | true    |
	And the side navigation should be:
		| text                               | href                                                              | current |
		| Pupil level and aggregated LA data | /my-local-authority/download-data/pupil-level-aggregated-la-data/ |         |
		| Individual school data             | /my-local-authority/download-data/individual-school-data/         | true    |
	And the sub-page title should be "Data files available for download" with caption "Test School 1 (URN: 111111)"

@Javascript:disabled
Scenario Outline: Data downloads > Individual school data > Data files available for download - Page should contain multiple checkbox groups
	Given Local Authority "301" exists:
		"""
		{
			"name": "Test LA"
		}
		"""
	And Establishment "111111" exists:
		"""
		{
			"name": "Test School 1",
			"localAuthority":
			{
				"code": "301"
			}
		}
		"""
	And Establishment "222222" exists:
		"""
		{
			"name": "Test School 2",
			"localAuthority":
			{
				"code": "301"
			}
		}
		"""
	And blob storage file downloads-config.json exists in config container:
		"""
		[
			{
				"id": "kts-school-ks4-pupil",
				"source": "KTS",
				"scope": "School",
				"dataSetType": "KeyStage4",
				"label": "Key stage 4 (KS4)",
				"filePathPattern": "School/{urn}/{year}/{filetype}/ks4_pupil_{version}.{filetype}"
			},
			{
				"id": "kts-school-ks2-pupil",
				"source": "KTS",
				"scope": "School",
				"dataSetType": "KeyStage2",
				"label": "Key stage 2 (KS2)",
				"filePathPattern": "School/{urn}/{year}/{filetype}/ks2_pupil_{version}.{filetype}"
			}
		]
		"""
	And blob storage file School/111111/2022/csv/ks2_pupil_final.csv exists in downloads-kts container:
		"""
		Column A,Column B,Column C
		1,2,3
		"""
	And blob storage file School/111111/2022/csv/ks4_pupil_final.csv exists in downloads-kts container:
		"""
		Column A,Column B,Column C
		1,2,3
		"""
	When I navigate to /my-local-authority/download-data/individual-school-data/111111/select-files/?selectedYear=2022
	Then I should get a 200 response
	And the element "[data-testid='available-downloads-file-group-<group>']" should have the text content "<text>"

Examples:
	| group             | text              |
	| Key stage 2 (KS2) | Key stage 2 (KS2) |
	| Key stage 4 (KS4) | Key stage 4 (KS4) |

@Javascript:disabled
Scenario Outline: Data downloads > Individual school data > Data files available for download - Page should contain multiple checkboxes
	Given Local Authority "301" exists:
		"""
		{
			"name": "Test LA"
		}
		"""
	And Establishment "111111" exists:
		"""
		{
			"name": "Test School 1",
			"localAuthority":
			{
				"code": "301"
			}
		}
		"""
	And Establishment "222222" exists:
		"""
		{
			"name": "Test School 2",
			"localAuthority":
			{
				"code": "301"
			}
		}
		"""
	And blob storage file downloads-config.json exists in config container:
		"""
		[
			{
				"id": "asp-school-mtc-pupil",
				"source": "ASP",
				"scope": "School",
				"dataSetType": "MTC",
				"label": "Test",
				"filePathPattern": "School/{urn}/{year}/MTC/mtc_pupil_{version}.{filetype}"
			},
			{
				"id": "kts-school-ks2-pupil",
				"source": "KTS",
				"scope": "School",
				"dataSetType": "KeyStage2",
				"label": "Key stage 2 (KS2)",
				"filePathPattern": "School/{urn}/{year}/{filetype}/ks2_pupil_{version}.{filetype}"
			},
			{
				"id": "kts-school-ks4-pupil",
				"source": "KTS",
				"scope": "School",
				"dataSetType": "KeyStage4",
				"label": "Key stage 4 (KS4)",
				"filePathPattern": "School/{urn}/{year}/{filetype}/ks4_pupil_{version}.{filetype}"
			}
		]
		"""
	And blob storage file School/111111/2022/csv/ks2_pupil_final.csv exists in downloads-kts container:
		"""
		Column A,Column B,Column C
		1,2,3
		"""
	And blob storage file School/111111/2022/csv/ks4_pupil_final.csv exists in downloads-kts container:
		"""
		Column A,Column B,Column C
		1,2,3
		"""
	And blob storage file School/111111/2022/mtc/mtc_pupil_final.csv exists in downloads-asp container:
		"""
		Column A,Column B,Column C
		1,2,3
		"""
	When I navigate to /my-local-authority/download-data/individual-school-data/111111/select-files/?selectedYear=2022
	Then I should get a 200 response
	And the element "[data-testid='available-downloads-file-<fileid>-label']" should have the text content "<label>"

Examples:
	| fileid                                 | label                                                                 |
	| kts-school-ks2-pupil-111111-2022-final | Key stage 2 (KS2) (Final) (Key to success)                            |
	| kts-school-ks4-pupil-111111-2022-final | Key stage 4 (KS4) (Final) (Key to success)                            |
	| asp-school-mtc-pupil-111111-2022-final | Multiplication table check (MTC) (Final) (Analyse school performance) |

@Javascript:disabled
Scenario: Data downloads > Individual school data > Data files available for download - When no files are selected and Continue button clicked, should show validation error
	Given Local Authority "301" exists:
		"""
		{
			"name": "Test LA",
		}
		"""
	And Establishment "111111" exists:
		"""
		{
			"name": "Test School 1",
			"localAuthority":
			{
				"code": "301"
			}
		}
		"""
	And Establishment "222222" exists:
		"""
		{
			"name": "Test School 2",
			"localAuthority":
			{
				"code": "301"
			}
		}
		"""
	And blob storage file downloads-config.json exists in config container:
		"""
		[
			{
				"id": "kts-school-ks2-pupil",
				"source": "KTS",
				"scope": "School",
				"dataSetType": "KeyStage2",
				"label": "Key stage 2 (KS2)",
				"filePathPattern": "School/{urn}/{year}/{filetype}/ks2_pupil_{version}.{filetype}"
			}
		]
		"""
	And blob storage file School/111111/2022/csv/ks2_pupil_final.csv exists in downloads-kts container:
		"""
		Column A,Column B,Column C
		1,2,3
		"""
	When I navigate to /my-local-authority/download-data/individual-school-data/111111/select-files/?selectedYear=2022
	And I click the button "*[data-testid='selectedFilesSubmit']"
	Then the path should be /my-local-authority/download-data/individual-school-data/111111/select-files/?selectedYear=2022
	And the element "*[data-testid='app-error-summary'] h2" should have the text content "There is a problem"
	And the element "*[data-testid='app-error-summary-selectedFiles']" should have the text content "Please choose one or more data files to download"
	And the element "*[data-testid='app-error-summary-selectedFiles']" should have the href "#app-field-selectedFiles"
	And the element "*[data-testid='app-field-selectedFiles-error']" should have the text content "Please choose one or more data files to download"

@Javascript:disabled
Scenario: Data downloads > Individual school data > Data files available for download - When files are selected and Continue button clicked, should move to next step
	Given Local Authority "301" exists:
		"""
		{
			"name": "Test LA"
		}
		"""
	And Establishment "111111" exists:
		"""
		{
			"name": "Test School 1",
			"localAuthority":
			{
				"code": "301"
			}
		}
		"""
	And Establishment "222222" exists:
		"""
		{
			"name": "Test School 2",
			"localAuthority":
			{
				"code": "301"
			}
		}
		"""
	And blob storage file downloads-config.json exists in config container:
		"""
		[
			{
				"id": "kts-school-ks2-pupil",
				"source": "KTS",
				"scope": "School",
				"dataSetType": "KeyStage2",
				"label": "Key stage 2 (KS2)",
				"filePathPattern": "School/{urn}/{year}/{filetype}/ks2_pupil_{version}.{filetype}"
			}
		]
		"""
	And blob storage file School/111111/2022/csv/ks2_pupil_final.csv exists in downloads-kts container:
		"""
		Column A,Column B,Column C
		1,2,3
		"""
	When I navigate to /my-local-authority/download-data/individual-school-data/111111/select-files/?selectedYear=2022
	And I update the element "#app-available-downloads-file-kts-school-ks2-pupil-111111-2022-final" to be checked
	And I click the button "*[data-testid='selectedFilesSubmit']"
	Then the path should be /my-local-authority/download-data/individual-school-data/111111/select-format/?selectedYear=2022&selectedFiles=kts-school-ks2-pupil-111111-2022-final

@Javascript:disabled
Scenario: Data downloads > Individual school data > Download data - Common page elements
	Given Local Authority "301" exists:
		"""
		{
			"name": "Test LA"
		}
		"""
	And Establishment "111111" exists:
		"""
		{
			"name": "Test School 1",
			"localAuthority":
			{
				"code": "301"
			}
		}
		"""
	And Establishment "222222" exists:
		"""
		{
			"name": "Test School 2",
			"localAuthority":
			{
				"code": "301"
			}
		}
		"""
	When I navigate to /my-local-authority/download-data/individual-school-data/111111/select-format/?selectedYear=2022&selectedFiles=kts-school-ks2-pupil-111111-2022-final&selectedFiles=kts-school-ks2-pupil-111111-2023-provisional
	Then I should get a 200 response
	And the page title should be "Download data"
	And the page subtitle should be "Individual school data"
	And the breadcrumb trail should be:
		| text                              | href                                                                                            | current |
		| Home                              | /                                                                                               |         |
		| My local authority                | /my-local-authority/                                                                            |         |
		| Download data                     | /my-local-authority/download-data/                                                              |         |
		| Search for a school               | /my-local-authority/download-data/individual-school-data/                                       |         |
		| Dates available for download      | /my-local-authority/download-data/individual-school-data/111111/select-year/                    |         |
		| Data files available for download | /my-local-authority/download-data/individual-school-data/111111/select-files/?selectedYear=2022 |         |
		| Download individual school data   |                                                                                                 | true    |
	And the sub-navigation should be:
		| text          | href                               | current |
		| Download data | /my-local-authority/download-data/ | true    |
	And the side navigation should be:
		| text                               | href                                                              | current |
		| Pupil level and aggregated LA data | /my-local-authority/download-data/pupil-level-aggregated-la-data/ |         |
		| Individual school data             | /my-local-authority/download-data/individual-school-data/         | true    |
	And the sub-page title should be "Download individual school data" with caption "Test School 1 (URN: 111111)"

@Javascript:disabled
Scenario Outline: Data downloads > Individual school data > Download data - Page should contain three links
	Given Local Authority "301" exists:
		"""
		{
			"name": "Test LA"
		}
		"""
	And Establishment "111111" exists:
		"""
		{
			"name": "Test School 1",
			"localAuthority":
			{
				"code": "301"
			}
		}
		"""
	And Establishment "222222" exists:
		"""
		{
			"name": "Test School 2",
			"localAuthority":
			{
				"code": "301"
			}
		}
		"""
	When I navigate to /my-local-authority/download-data/individual-school-data/111111/select-format/?selectedYear=2022&selectedFiles=kts-school-ks2-pupil-111111-2022-final&selectedFiles=kts-school-ks2-pupil-111111-2023-provisional
	Then I should get a 200 response
	And the element "[data-testid="select-format-description"]" should have the text content "The data included in your download is the pupil level / aggregated data for your school."
	And the available download formats should be:
		| text                | href                                                                                                                                                                                                           |
		| Data in CSV format  | /my-local-authority/download-data/individual-school-data/111111/download-as-zip/?fileType=CSV&selectedFiles=kts-school-ks2-pupil-111111-2022-final&selectedFiles=kts-school-ks2-pupil-111111-2023-provisional  |
		| Data in XLSX format | /my-local-authority/download-data/individual-school-data/111111/download-as-zip/?fileType=XLSX&selectedFiles=kts-school-ks2-pupil-111111-2022-final&selectedFiles=kts-school-ks2-pupil-111111-2023-provisional |
		| Data in TSV format  | /my-local-authority/download-data/individual-school-data/111111/download-as-zip/?fileType=TSV&selectedFiles=kts-school-ks2-pupil-111111-2022-final&selectedFiles=kts-school-ks2-pupil-111111-2023-provisional  |

@Javascript:disabled
Scenario Outline: Data downloads > Individual school data > Download data - Download other dates link should link back to first step
	Given Local Authority "301" exists:
		"""
		{
			"name": "Test LA"
		}
		"""
	And Establishment "111111" exists:
		"""
		{
			"name": "Test School 1",
			"localAuthority":
			{
				"code": "301"
			}
		}
		"""
	And Establishment "222222" exists:
		"""
		{
			"name": "Test School 2",
			"localAuthority":
			{
				"code": "301"
			}
		}
		"""
	When I navigate to /my-local-authority/download-data/individual-school-data/111111/select-format/?selectedYear=2022&kts-school-ks2-pupil-111111-2022-final
	Then the element "[data-testid="available-downloads-other-dates"]" should have the href "/my-local-authority/download-data/individual-school-data/111111/select-year/"