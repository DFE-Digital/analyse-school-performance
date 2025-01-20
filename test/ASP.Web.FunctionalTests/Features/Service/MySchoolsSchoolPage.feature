Feature: My schools > School page

Background:
	Given I am an LA Named user for Local Authority "301"
 
@Javascript:disabled
Scenario: A School user should not be able to access the My schools > School page, even if it's for their own School. Instead, they should see a 403 Access not allowed page.
	Given I am a School Named user for Establishment "123456"
	When I navigate to /my-schools/123456/
	Then I should get a 403 response
	And the page title should be "Access not allowed"
	And the element "h1.govuk-heading-l" should have the text content "Access not allowed"

@Javascript:disabled
Scenario: A user with access to all schools should not be able to access the My schools > School page. Instead, they should see a 403 Access not allowed page.
	Given I am a DfE Named user
	When I navigate to /my-schools/123456/
	Then I should get a 403 response
	And the page title should be "Access not allowed"
	And the element "h1.govuk-heading-l" should have the text content "Access not allowed"

@Javascript:disabled
Scenario: School page should throw page not found if Establishment is not currently visible
	Given non-visible Establishment "111111" exists:
		"""
		{
			"name": "Thursby Primary School"
		}
		"""
	And Local Authority "301" exists:
		"""
		{
			"name": "Test LA"
		}
		"""
	When I navigate to /my-schools/111111/
	Then I should get a 404 response
	Then the element "*[data-testid='error-display-message']" should have the text content "Error message: Not found: API error: /api/GetEstablishmentDetails Establishment with URN "111111" is not currently visible."

@Javascript:disabled
Scenario: School page should throw page not found if Establishment is deleted
	Given deleted Establishment "111111" exists:
		"""
		{
			"name": "Thursby Primary School"
		}
		"""
	And Local Authority "301" exists:
		"""
		{
			"name": "Test LA"
		}
		"""
	When I navigate to /my-schools/111111/
	Then I should get a 404 response
	Then the element "*[data-testid='error-display-message']" should have the text content "Error message: Not found: API error: /api/GetEstablishmentDetails Establishment with URN "111111" has been deleted."

@Javascript:disabled
Scenario: School page should display page not found page if School URN is invalid
	Given Establishment "111111" exists:
		"""
		{
			"name": "Thursby Primary School"
		}
		"""
	And Local Authority "301" exists:
		"""
		{
			"name": "Test LA"
		}
		"""
	When I navigate to /my-schools/222222/
	Then I should get a 404 response
	And the page title should be "Page not found"
	And the element "h1.govuk-heading-l" should have the text content "Page not found"
	And the element "*[data-testid='address-typing-instruction']" should have the text content "If you typed the web address, check it is correct."
	And the element "*[data-testid='address-pasting-instruction']" should have the text content "If you pasted the web address, check you copied the entire address."
	And the element "*[data-testid='error-display-message']" should exist
	And the element "*[data-testid='error-display-message']" should have the text content "Error message: Not found: API error: /api/GetEstablishmentDetails Could not find Establishment with URN "222222"."

@Javascript:disabled
Scenario: School page should contain seven app card container element
	Given Content Template "school-landing-page" exists:
		"""
		{
			"Views": [
			{
				"ViewId": "Card",
				"ViewContent": {
					"Id": "app-card-phonics",
					"Title": "Phonics",
					"LinkUrl": "phonics",
					"Text": "View data based on expected standard, average score and attainment in phonics."
				}
			},
			{
				"ViewId": "Card",
				"ViewContent": {
					"Id": "app-card-mtc",
					"Title": "Multiplication table check (MTC)",
					"LinkUrl": "mtc",
					"Text": "Identify pupils who have not yet mastered their times tables, so that additional support can be provided."
				}
			},
			{
				"ViewId": "Card",
				"ViewContent": {
					"Id": "app-card-key-stage-2",
					"Title": "Key stage 2",
					"LinkUrl": "key-stage-2",
					"Text": "See data for key stage 2 including headline measures & reports, progress & attainment scatter plots, and additional reports."
				}
			},
			{
				"ViewId": "Card",
				"ViewContent": {
					"Id": "app-card-key-stage-4",
					"Title": "Key stage 4",
					"LinkUrl": "key-stage-4",
					"Text": "See data for key stage 4 including headline measures & reports, progress & attainment scatter plots, and additional reports."
				}
			},
			{
				"ViewId": "Card",
				"ViewContent": {
					"Id": "app-card-qla",
					"Title": "Question level analysis (QLA)",
					"LinkUrl": "qla",
					"Text": "Assess how pupils performed in the key stage 2 tests and compare these with the national average."
				}
			},
			{
				"ViewId": "Card",
				"ViewContent": {
					"Id": "app-card-other-reports",
					"Title": "Other reports",
					"LinkUrl": "other-reports",
					"Text": "View reports on school performance, Ofsted inspections, asbence and exclusions and school characteristics."
				}
			},
			{
				"ViewId": "Card",
				"ViewContent": {
					"Id": "app-card-useful-links",
					"Title": "Useful links",
					"LinkUrl": "useful-links",
					"Text": "View links to other services and published documents that may be useful."
				}
			}
			]
		}
		"""
	And Establishment "123456" exists:
		"""
		{
			"name": "Hollinswood Primary School",
				"localAuthority": {
				"code": "999",
				"name": "Test LA"
				}
		}
		"""
	And Local Authority "301" exists:
		"""
		{
			"name": "Test LA"
		}
		"""
	When I navigate to /my-schools/123456/
	Then the element "#app-card-container" class should contain "app-grid-container-four-column"
	And the elements "#app-card-container .app-card" should total 7

	And the element "#app-card-phonics h2 a" should have the href "phonics"
	And the element "#app-card-phonics h2 a" should have the text content "Phonics"
	And the element "#app-card-phonics p" should have the text content "View data based on expected standard, average score and attainment in phonics."
	
	And the element "#app-card-mtc h2 a" should have the href "mtc"
	And the element "#app-card-mtc h2 a" should have the text content "Multiplication table check (MTC)"
	And the element "#app-card-mtc p" should have the text content "Identify pupils who have not yet mastered their times tables, so that additional support can be provided."
	
	And the element "#app-card-key-stage-2 h2 a" should have the href "key-stage-2"
	And the element "#app-card-key-stage-2 h2 a" should have the text content "Key stage 2"
	And the element "#app-card-key-stage-2 p" should have the text content "See data for key stage 2 including headline measures & reports, progress & attainment scatter plots, and additional reports."

	And the element "#app-card-key-stage-4 h2 a" should have the href "key-stage-4"
	And the element "#app-card-key-stage-4 h2 a" should have the text content "Key stage 4"
	And the element "#app-card-key-stage-4 p" should have the text content "See data for key stage 4 including headline measures & reports, progress & attainment scatter plots, and additional reports."
	
	And the element "#app-card-qla h2 a" should have the href "qla"
	And the element "#app-card-qla h2 a" should have the text content "Question level analysis (QLA)"
	And the element "#app-card-qla p" should have the text content "Assess how pupils performed in the key stage 2 tests and compare these with the national average."

	And the element "#app-card-other-reports h2 a" should have the href "other-reports"
	And the element "#app-card-other-reports h2 a" should have the text content "Other reports"
	And the element "#app-card-other-reports p" should have the text content "View reports on school performance, Ofsted inspections, asbence and exclusions and school characteristics."

	And the element "#app-card-useful-links h2 a" should have the href "useful-links"
	And the element "#app-card-useful-links h2 a" should have the text content "Useful links"
	And the element "#app-card-useful-links p" should have the text content "View links to other services and published documents that may be useful."

@Javascript:disabled
Scenario Outline: Landing page - common page elements
	And Establishment "123456" exists:
		"""
		{
			"name": "Hollinswood Primary School",
		}
		"""
	And Local Authority "301" exists:
		"""
		{
			"name": "Test LA"
		}
		"""
	When I navigate to /my-schools/123456/
	Then the page title should be "My schools"
	And the page subtitle should be "Hollinswood Primary School (URN: 123456)"
	And the breadcrumb trail should be:
		| text                       | href         | current |
		| Home                       | /            |         |
		| My schools                 | /my-schools/ |         |
		| Hollinswood Primary School |              | true    |

@Javascript:disabled
Scenario: School page should contain a school details disclosure element
	Given Establishment "123456" exists:
		"""
		{
			"isPost16": false,
			"isPrimary": true,
			"isSecondary": false,
			"address": {
				"street": "Dale Acre Way",
				"town": "Telford",
				"postCode": "TF3 2EP"
			},
			"admissionsPolicy": {
				"name": "Not applicable",
			},
			"ageRange": {
				"low": 3,
				"high": 11
			},
			"establishmentType": {
				"name": "Community school",
			},
			"gender": {
				"name": "Mixed",
			},
			"headteacher": {
				"title": "Mrs",
				"firstName": "Kath",
				"lastName": "Osborne"
			},
			"localAuthority": {
				"name": "Telford and Wrekin",
				"code": "999"
			},
			"name": "Hollinswood Primary School",
			"noOfPupils": 404,
			"ofstedLastInspectionDate": "2020-01-22T00:00:00",
			"ofstedRating": {
				"code": "2",
				"name": "Good",
			},
			"religiousDenomination": {
				"name": "Does not apply",
			},
			"resourcedProvisionType": {
				"name": "Not recorded",
			}
		}
		"""
	And Local Authority "301" exists:
		"""
		{
			"name": "Test LA"
		}
		"""
	When I navigate to /my-schools/123456/
	Then I should get a 200 response
	And the element "*[data-testid='school-page-details-state-closed']" should have the text content "Show"
	And the element "*[data-testid='school-page-details-state-open']" should have the text content "Hide"
	And the element "*[data-testid='school-details-address-key']" should have the text content "Address"
	And the element "*[data-testid='school-details-address-value']" should have the text content "Dale Acre Way, Telford TF3 2EP"
	And the element "*[data-testid='school-details-school-type-key']" should have the text content "School type"
	And the element "*[data-testid='school-details-school-type-value']" should have the text content "Community school"
	And the element "*[data-testid='school-details-education-key']" should have the text content "Education phase"
	And the element "*[data-testid='school-details-education-value']" should have the text content "Primary"
	And the element "*[data-testid='school-details-gender-key']" should have the text content "Gender of entry"
	And the element "*[data-testid='school-details-gender-value']" should have the text content "Mixed"
	And the element "*[data-testid='school-details-ofsted-key']" should have the text content "Ofsted rating"
	And the element "*[data-testid='school-details-ofsted-code-value']" should have the text content "2"
	And the element "*[data-testid='school-details-ofsted-name-value']" should have the text content "Good"
	And the element "*[data-testid='school-details-ofsted-inspection-value']" should have the text content "Inspected 22 January 2020"
	And the element "*[data-testid='school-details-la-key']" should have the text content "Local authority"
	And the element "*[data-testid='school-details-la-value']" should have the text content "Telford and Wrekin"
	And the element "*[data-testid='school-details-gender-key']" should have the text content "Gender of entry"
	And the element "*[data-testid='school-details-gender-value']" should have the text content "Mixed"
	And the element "*[data-testid='school-details-principal-key']" should have the text content "Headteacher / Principal"
	And the element "*[data-testid='school-details-principal-value']" should have the text content "Mrs Kath Osborne"
	And the element "*[data-testid='school-details-age-key']" should have the text content "Age range"
	And the element "*[data-testid='school-details-age-value']" should have the text content "3 to 11"
	And the element "*[data-testid='school-details-religious-key']" should have the text content "Religious character"
	And the element "*[data-testid='school-details-religious-value']" should have the text content "Does not apply"
	And the element "*[data-testid='school-details-admission-key']" should have the text content "Admissions policy"
	And the element "*[data-testid='school-details-admission-value']" should have the text content "Not applicable"
	And the element "*[data-testid='school-details-provision-key']" should have the text content "SEN unit or resourced provision"
	And the element "*[data-testid='school-details-provision-value']" should have the text content "Not recorded"
	And the element "*[data-testid='school-details-pupils-key']" should have the text content "Number of pupils"
	And the element "*[data-testid='school-details-pupils-value']" should have the text content "404"

@Javascript:disabled
Scenario: School page should show if values are null
	Given Establishment "123456" exists:
		"""
		{
			"isPost16": null,
			"isPrimary": null,
			"isSecondary": null,
			"address": null,
			"admissionsPolicy": null,
			"ageRange": null,
			"establishmentType": null,
			"gender": null,
			"headteacher": null,
			"localAuthority": {
				"code": "999",
				"name": "Test LA"
			},
			"name": "Hollinswood Primary School",
			"noOfPupils": null,
			"ofstedLastInspectionDate": null,
			"ofstedRating": null,
			"religiousDenomination": null,
			"resourcedProvisionType": null
		}
		"""
	And Local Authority "301" exists:
		"""
		{
			"name": "Test LA"
		}
		"""
	When I navigate to /my-schools/123456/
	Then I should get a 200 response
	And the element "*[data-testid='school-page-details-state-closed']" should have the text content "Show"
	And the element "*[data-testid='school-page-details-state-open']" should have the text content "Hide"
	And the element "*[data-testid='school-details-address-key']" should have the text content "Address"
	And the element "*[data-testid='school-details-address-value']" should have the text content ""
	And the element "*[data-testid='school-details-school-type-key']" should have the text content "School type"
	And the element "*[data-testid='school-details-school-type-value']" should have the text content ""
	And the element "*[data-testid='school-details-education-key']" should have the text content "Education phase"
	And the element "*[data-testid='school-details-education-value']" should have the text content ""
	And the element "*[data-testid='school-details-gender-key']" should have the text content "Gender of entry"
	And the element "*[data-testid='school-details-gender-value']" should have the text content ""
	And the element "*[data-testid='school-details-ofsted-key']" should have the text content "Ofsted rating"
	And the element "*[data-testid='school-details-la-key']" should have the text content "Local authority"
	And the element "*[data-testid='school-details-la-value']" should have the text content "Test LA"
	And the element "*[data-testid='school-details-gender-key']" should have the text content "Gender of entry"
	And the element "*[data-testid='school-details-gender-value']" should have the text content ""
	And the element "*[data-testid='school-details-principal-key']" should have the text content "Headteacher / Principal"
	And the element "*[data-testid='school-details-principal-value']" should have the text content ""
	And the element "*[data-testid='school-details-age-key']" should have the text content "Age range"
	And the element "*[data-testid='school-details-age-value']" should have the text content ""
	And the element "*[data-testid='school-details-religious-key']" should have the text content "Religious character"
	And the element "*[data-testid='school-details-religious-value']" should have the text content ""
	And the element "*[data-testid='school-details-admission-key']" should have the text content "Admissions policy"
	And the element "*[data-testid='school-details-admission-value']" should have the text content ""
	And the element "*[data-testid='school-details-provision-key']" should have the text content "SEN unit or resourced provision"
	And the element "*[data-testid='school-details-provision-value']" should have the text content ""
	And the element "*[data-testid='school-details-pupils-key']" should have the text content "Number of pupils"
	And the element "*[data-testid='school-details-pupils-value']" should have the text content ""

@Javascript:disabled
Scenario: School page should show if values are null case 2
	Given Establishment "123456" exists:
		"""
		{
			"name": "Hollinswood Primary School",
			"isPost16": null,
			"isPrimary": null,
			"isSecondary": null,
			"address": {
				"street": null,
				"town": null,
				"postCode": null
			},
			"admissionsPolicy": {
				"name": null,
			},
			"ageRange": {
				"low": null,
				"high": null
			},
			"establishmentType": {
				"name": null,
			},
			"gender": {
				"name": null,
			},
			"headteacher": {
				"title": null,
				"firstName": null,
				"lastName": null
			},
			"localAuthority": {
				"code": "999",
					"name": "Test LA"
			},
			"noOfPupils": null,
			"ofstedLastInspectionDate": null,
			"ofstedRating": {
				"code": null,
				"name": null,
			},
			"religiousDenomination": {
				"name": null,
			},
			"resourcedProvisionType": {
				"name": null,
			}
		}
		"""
	And Local Authority "301" exists:
		"""
		{
			"name": "Test LA"
		}
		"""
	When I navigate to /my-schools/123456/
	Then the element "*[data-testid='school-page-details-state-closed']" should have the text content "Show"
	And the element "*[data-testid='school-page-details-state-open']" should have the text content "Hide"
	And the element "*[data-testid='school-details-address-key']" should have the text content "Address"
	And the element "*[data-testid='school-details-address-value']" should have the text content ""
	And the element "*[data-testid='school-details-school-type-key']" should have the text content "School type"
	And the element "*[data-testid='school-details-school-type-value']" should have the text content ""
	And the element "*[data-testid='school-details-education-key']" should have the text content "Education phase"
	And the element "*[data-testid='school-details-education-value']" should have the text content ""
	And the element "*[data-testid='school-details-gender-key']" should have the text content "Gender of entry"
	And the element "*[data-testid='school-details-gender-value']" should have the text content ""
	And the element "*[data-testid='school-details-ofsted-key']" should have the text content "Ofsted rating"
	And the element "*[data-testid='school-details-la-key']" should have the text content "Local authority"
	And the element "*[data-testid='school-details-la-value']" should have the text content "Test LA"
	And the element "*[data-testid='school-details-gender-key']" should have the text content "Gender of entry"
	And the element "*[data-testid='school-details-gender-value']" should have the text content ""
	And the element "*[data-testid='school-details-principal-key']" should have the text content "Headteacher / Principal"
	And the element "*[data-testid='school-details-principal-value']" should have the text content ""
	And the element "*[data-testid='school-details-age-key']" should have the text content "Age range"
	And the element "*[data-testid='school-details-age-value']" should have the text content ""
	And the element "*[data-testid='school-details-religious-key']" should have the text content "Religious character"
	And the element "*[data-testid='school-details-religious-value']" should have the text content ""
	And the element "*[data-testid='school-details-admission-key']" should have the text content "Admissions policy"
	And the element "*[data-testid='school-details-admission-value']" should have the text content ""
	And the element "*[data-testid='school-details-provision-key']" should have the text content "SEN unit or resourced provision"
	And the element "*[data-testid='school-details-provision-value']" should have the text content ""
	And the element "*[data-testid='school-details-pupils-key']" should have the text content "Number of pupils"
	And the element "*[data-testid='school-details-pupils-value']" should have the text content ""

@Javascript:disabled
Scenario: Details disclosure element text should read 'Show school details' when closed
	Given Establishment "123456" exists:
		"""
		{
			"name": "Hollinswood Primary School",
			"localAuthority": {
				"code": "999",
				"name": "Test LA"
			}
		}
		"""
	And Local Authority "301" exists:
		"""
		{
			"name": "Test LA"
		}
		"""
	When I navigate to /my-schools/123456/
	Then the element "*[data-testid='school-page-details-state-closed']" should have the text content "Show"
	
@Javascript:disabled
Scenario: Data downloads 'Dates available for download' - common page elements
	Given Establishment "136028" exists:
		"""
		{
			"name": "Dagenham Park CofE School"
		}
		"""
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
				"id": "kts-school-ks2-pupil",
				"source": "KTS",
				"scope": "School",
				"dataSetType": "KeyStage2",
				"label": "Key stage 2 (KS2)",
				"filePathPattern": "School/{urn}/{year}/{filetype}/ks2_pupil_{version}.{filetype}"
			}
		]
		"""
	And blob storage file School/136028/2022/csv/ks2_pupil_final.csv exists in downloads-kts container:
		"""
		Column A,Column B,Column C
		1,2,3
		"""
	When I navigate to /my-schools/136028/download-data
	Then the page title should be "Download data"
	And the breadcrumb trail should be:
		| text                         | href                              | current |
		| Home                         | /                                 |         |
		| My schools                   | /my-schools/                      |         |
		| Dagenham Park CofE School    | /my-schools/136028/               |         |
		| Download data                | /my-schools/136028/download-data/ |         |
		| Dates available for download |                                   | true    |
	And the sub-navigation should be:
		| text          | href                              | current |
		| Download data | /my-schools/136028/download-data/ | true    |
		| Other reports | /my-schools/136028/other-reports/ |         |
		| Useful links  | /my-schools/136028/useful-links/  |         |
	And the side navigation should be:
		| text                           | href                              | current |
		| Dagenham Park CofE School data | /my-schools/136028/download-data/ | true    |
	And the sub-page title should be "Dates available for download" with caption "Dagenham Park CofE School data"

@Javascript:disabled
Scenario Outline: Data downloads 'Dates available for download' - page should contain three radio buttons
	Given Establishment "136028" exists:
		"""
		{
			"name": "Dagenham Park CofE School"
		}
		"""
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
				"id": "kts-school-ks2-pupil",
				"source": "KTS",
				"scope": "School",
				"dataSetType": "KeyStage2",
				"label": "Key stage 2 (KS2)",
				"filePathPattern": "School/{urn}/{year}/{filetype}/ks2_pupil_{version}.{filetype}"
			}
		]
		"""
	And blob storage file School/136028/2022/csv/ks2_pupil_final.csv exists in downloads-kts container:
		"""
		Column A,Column B,Column C
		1,2,3
		"""
	And blob storage file School/136028/2023/csv/ks2_pupil_final.csv exists in downloads-kts container:
		"""
		Column A,Column B,Column C
		1,2,3
		"""
	And blob storage file School/136028/2024/csv/ks2_pupil_final.csv exists in downloads-kts container:
		"""
		Column A,Column B,Column C
		1,2,3
		"""
	When I navigate to /my-schools/136028/download-data/
	Then the element "[data-testid='available-downloads-dates-<year>-label']" should have the text content "<label>"
Examples:
	| year | label        |
	| 2022 | 2021 to 2022 |
	| 2023 | 2022 to 2023 |
	| 2024 | 2023 to 2024 |

@Javascript:disabled
Scenario: Data downloads 'Dates available for download' - when no date is selected and Continue button clicked, should show validation error
	Given Establishment "123456" exists:
		"""
		{
			"name": "Hollinswood Primary School"
		}
		"""
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
				"id": "kts-school-ks2-pupil",
				"source": "KTS",
				"scope": "School",
				"dataSetType": "KeyStage2",
				"label": "Key stage 2 (KS2)",
				"filePathPattern": "School/{urn}/{year}/{filetype}/ks2_pupil_{version}.{filetype}"
			}
		]
		"""
	And blob storage file School/123456/2022/csv/ks2_pupil_final.csv exists in downloads-kts container:
		"""
		Column A,Column B,Column C
		1,2,3
		"""
	And blob storage file School/123456/2023/csv/ks2_pupil_final.csv exists in downloads-kts container:
		"""
		Column A,Column B,Column C
		1,2,3
		"""
	And blob storage file School/123456/2024/csv/ks2_pupil_final.csv exists in downloads-kts container:
		"""
		Column A,Column B,Column C
		1,2,3
		"""
	When I navigate to /my-schools/123456/download-data/
	And I click the button "*[data-testid='selectedYearSubmit']"
	Then the path should be /my-schools/123456/download-data/
	And the element "*[data-testid='app-error-summary'] h2" should have the text content "There is a problem"
	And the element "*[data-testid='app-error-summary-selectedYear']" should have the text content "Please choose an academic year to download"
	And the element "*[data-testid='app-error-summary-selectedYear']" should have the href "#app-field-selectedYear"
	And the element "*[data-testid='app-field-selectedYear-error']" should have the text content "Please choose an academic year to download"

@Javascript:disabled
Scenario: Data downloads 'Dates available for download' - when date is selected and Continue button clicked, should move to next step
	Given Establishment "123456" exists:
		"""
		{
			"name": "Hollinswood Primary School"
		}
		"""
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
				"id": "kts-school-ks2-pupil",
				"source": "KTS",
				"scope": "School",
				"dataSetType": "KeyStage2",
				"label": "Key stage 2 (KS2)",
				"filePathPattern": "School/{urn}/{year}/{filetype}/ks2_pupil_{version}.{filetype}"
			}
		]
		"""
	And blob storage file School/123456/2022/csv/ks2_pupil_final.csv exists in downloads-kts container:
		"""
		Column A,Column B,Column C
		1,2,3
		"""
	And blob storage file School/123456/2023/csv/ks2_pupil_final.csv exists in downloads-kts container:
		"""
		Column A,Column B,Column C
		1,2,3
		"""
	And blob storage file School/123456/2024/csv/ks2_pupil_final.csv exists in downloads-kts container:
		"""
		Column A,Column B,Column C
		1,2,3
		"""
	When I navigate to /my-schools/123456/download-data/
	And I update the element "#app-available-downloads-dates-2022" to be checked
	And I click the button "*[data-testid='selectedYearSubmit']"
	Then the path should be /my-schools/123456/download-data/select-files/?selectedYear=2022

@Javascript:disabled
Scenario: Data downloads "Data files available for download' - common page elements
	Given Establishment "136028" exists:
		"""
		{
			"name": "Dagenham Park CofE School"
		}
		"""
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
			},
			{
				"id": "kts-school-ks4-pupil",
				"source": "KTS",
				"scope": "School",
				"dataSetType": "Phonics",
				"label": "Phonics",
				"filePathPattern": "School/{urn}/{year}/{filetype}/phonics_pupil_{version}.{filetype}"
			}
		]
		"""
	And blob storage file School/136028/2022/csv/ks2_pupil_final.csv exists in downloads-kts container:
		"""
		Column A,Column B,Column C
		1,2,3
		"""
	And blob storage file School/136028/2022/csv/ks4_pupil_final.csv exists in downloads-kts container:
		"""
		Column A,Column B,Column C
		1,2,3
		"""
	And blob storage file School/136028/2022/csv/phonics_pupil_final.csv exists in downloads-kts container:
		"""
		Column A,Column B,Column C
		1,2,3
		"""
	When I navigate to /my-schools/136028/download-data/select-files/?selectedYear=2022
	Then the page title should be "Download data"
	And the breadcrumb trail should be:
		| text                              | href                              | current |
		| Home                              | /                                 |         |
		| My schools                        | /my-schools/                      |         |
		| Dagenham Park CofE School         | /my-schools/136028/               |         |
		| Download data                     | /my-schools/136028/download-data/ |         |
		| Dates available for download      | /my-schools/136028/download-data/ |         |
		| Data files available for download |                                   | true    |
	And the sub-navigation should be:
		| text          | href                              | current |
		| Download data | /my-schools/136028/download-data/ | true    |
		| Other reports | /my-schools/136028/other-reports/ |         |
		| Useful links  | /my-schools/136028/useful-links/  |         |
	And the side navigation should be:
		| text                           | href                              | current |
		| Dagenham Park CofE School data | /my-schools/136028/download-data/ | true    |
	And the sub-page title should be "Data files available for download" with caption "Dagenham Park CofE School data"

@Javascript:disabled
Scenario Outline: Data downloads 'Data files available for download' - page should contain three checkbox groups
	Given Establishment "136028" exists:
		"""
		{
			"name": "Dagenham Park CofE School"
		}
		"""
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
			},
			{
				"id": "kts-school-phonics-pupil",
				"source": "KTS",
				"scope": "School",
				"dataSetType": "Phonics",
				"label": "Phonics",
				"filePathPattern": "School/{urn}/{year}/{filetype}/phonics_pupil_{version}.{filetype}"
			}
		]
		"""
	And blob storage file School/136028/2022/csv/ks2_pupil_final.csv exists in downloads-kts container:
		"""
		Column A,Column B,Column C
		1,2,3
		"""
	And blob storage file School/136028/2022/csv/ks4_pupil_final.csv exists in downloads-kts container:
		"""
		Column A,Column B,Column C
		1,2,3
		"""
	And blob storage file School/136028/2022/csv/phonics_pupil_final.csv exists in downloads-kts container:
		"""
		Column A,Column B,Column C
		1,2,3
		"""
	When I navigate to /my-schools/136028/download-data/select-files/?selectedYear=2022
	Then the element "[data-testid='available-downloads-file-group-<group>']" should have the text content "<text>"
Examples:
	| group             | text              |
	| Key stage 2 (KS2) | Key stage 2 (KS2) |
	| Key stage 4 (KS4) | Key stage 4 (KS4) |
	| Phonics           | Phonics           |

@Javascript:disabled
Scenario Outline: Data downloads 'Data files available for download' - page should contain five checkboxes
	Given Establishment "136028" exists:
		"""
		{
			"name": "Dagenham Park CofE School"
		}
		"""
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
			},
			{
				"id": "kts-school-phonics-pupil",
				"source": "KTS",
				"scope": "School",
				"dataSetType": "Phonics",
				"label": "Phonics",
				"filePathPattern": "School/{urn}/{year}/{filetype}/phonics_pupil_{version}.{filetype}"
			},
			{
				"id": "asp-school-ks2-school",
				"source": "ASP",
				"scope": "School",
				"dataSetType": "KeyStage2",
				"label": "Key stage 2 (KS2)",
				"filePathPattern": "School/{urn}/{year}/{filetype}/ks2_school_{version}.{filetype}"
			},
			{
				"id": "asp-school-ks4-school",
				"source": "ASP",
				"scope": "School",
				"dataSetType": "KeyStage4",
				"label": "Key stage 4 (KS4)",
				"filePathPattern": "School/{urn}/{year}/{filetype}/ks4_school_{version}.{filetype}"
			}
		]
		"""
	And blob storage file School/136028/2022/csv/ks2_pupil_final.csv exists in downloads-kts container:
		"""
		Column A,Column B,Column C
		1,2,3
		"""
	And blob storage file School/136028/2022/csv/ks4_pupil_final.csv exists in downloads-kts container:
		"""
		Column A,Column B,Column C
		1,2,3
		"""
	And blob storage file School/136028/2022/csv/phonics_pupil_final.csv exists in downloads-kts container:
		"""
		Column A,Column B,Column C
		1,2,3
		"""
	And blob storage file School/136028/2022/csv/ks2_school_provisional.csv exists in downloads-asp container:
		"""
		Column A,Column B,Column C
		1,2,3
		"""
	And blob storage file School/136028/2022/csv/ks4_school_final.csv exists in downloads-asp container:
		"""
		Column A,Column B,Column C
		1,2,3
		"""
	When I navigate to /my-schools/136028/download-data/select-files/?selectedYear=2022
	Then the element "[data-testid='available-downloads-file-<fileid>-label']" should have the text content "<label>"
Examples:
	| fileid                                        | label                                                        |
	| kts-school-ks2-pupil-136028-2022-final        | Key stage 2 (KS2) (Final) (Key to success)                   |
	| asp-school-ks2-school-136028-2022-provisional | Key stage 2 (KS2) (Provisional) (Analyse school performance) |
	| kts-school-ks4-pupil-136028-2022-final        | Key stage 4 (KS4) (Final) (Key to success)                   |
	| asp-school-ks4-school-136028-2022-final       | Key stage 4 (KS4) (Final) (Analyse school performance)       |
	| kts-school-phonics-pupil-136028-2022-final    | Phonics (Final) (Key to success)                             |

@Javascript:disabled
Scenario: Data downloads 'Data files available for download' - when no files are selected and Continue button clicked, should show validation error
	Given Establishment "123456" exists:
		"""
		{
			"name": "Hollinswood Primary School"
		}
		"""
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
			},
			{
				"id": "kts-school-phonics-pupil",
				"source": "KTS",
				"scope": "School",
				"dataSetType": "Phonics",
				"label": "Phonics",
				"filePathPattern": "School/{urn}/{year}/{filetype}/phonics_pupil_{version}.{filetype}"
			},
			{
				"id": "asp-school-ks2-school",
				"source": "ASP",
				"scope": "School",
				"dataSetType": "KeyStage2",
				"label": "Key stage 2 (KS2)",
				"filePathPattern": "School/{urn}/{year}/{filetype}/ks2_school_{version}.{filetype}"
			},
			{
				"id": "asp-school-ks4-school",
				"source": "ASP",
				"scope": "School",
				"dataSetType": "KeyStage4",
				"label": "Key stage 4 (KS4)",
				"filePathPattern": "School/{urn}/{year}/{filetype}/ks4_school_{version}.{filetype}"
			}
		]
		"""
	And blob storage file School/123456/2022/csv/ks2_pupil_final.csv exists in downloads-kts container:
		"""
		Column A,Column B,Column C
		1,2,3
		"""
	And blob storage file School/123456/2022/csv/ks4_pupil_final.csv exists in downloads-kts container:
		"""
		Column A,Column B,Column C
		1,2,3
		"""
	And blob storage file School/123456/2022/csv/phonics_pupil_final.csv exists in downloads-kts container:
		"""
		Column A,Column B,Column C
		1,2,3
		"""
	And blob storage file School/123456/2022/csv/ks2_school_provisional.csv exists in downloads-asp container:
		"""
		Column A,Column B,Column C
		1,2,3
		"""
	And blob storage file School/123456/2022/csv/ks4_school_final.csv exists in downloads-asp container:
		"""
		Column A,Column B,Column C
		1,2,3
		"""
	When I navigate to /my-schools/123456/download-data/select-files/?selectedYear=2022
	And I click the button "*[data-testid='selectedFilesSubmit']"
	Then the path should be /my-schools/123456/download-data/select-files/?selectedYear=2022
	And the element "*[data-testid='app-error-summary'] h2" should have the text content "There is a problem"
	And the element "*[data-testid='app-error-summary-selectedFiles']" should have the text content "Please choose one or more data files to download"
	And the element "*[data-testid='app-error-summary-selectedFiles']" should have the href "#app-field-selectedFiles"
	And the element "*[data-testid='app-field-selectedFiles-error']" should have the text content "Please choose one or more data files to download"

@Javascript:disabled
Scenario: Data downloads 'Data files available for download' - when files are selected and Continue button clicked, should move to next step
	Given Establishment "123456" exists:
		"""
		{
			"name": "Hollinswood Primary School"
		}
		"""
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
			},
			{
				"id": "kts-school-phonics-pupil",
				"source": "KTS",
				"scope": "School",
				"dataSetType": "Phonics",
				"label": "Phonics",
				"filePathPattern": "School/{urn}/{year}/{filetype}/phonics_pupil_{version}.{filetype}"
			},
			{
				"id": "asp-school-ks2-school",
				"source": "ASP",
				"scope": "School",
				"dataSetType": "KeyStage2",
				"label": "Key stage 2 (KS2)",
				"filePathPattern": "School/{urn}/{year}/{filetype}/ks2_school_{version}.{filetype}"
			},
			{
				"id": "asp-school-ks4-school",
				"source": "ASP",
				"scope": "School",
				"dataSetType": "KeyStage4",
				"label": "Key stage 4 (KS4)",
				"filePathPattern": "School/{urn}/{year}/{filetype}/ks4_school_{version}.{filetype}"
			}
		]
		"""
	And blob storage file School/123456/2022/csv/ks2_pupil_final.csv exists in downloads-kts container:
		"""
		Column A,Column B,Column C
		1,2,3
		"""
	And blob storage file School/123456/2022/csv/ks4_pupil_final.csv exists in downloads-kts container:
		"""
		Column A,Column B,Column C
		1,2,3
		"""
	And blob storage file School/123456/2022/csv/phonics_pupil_final.csv exists in downloads-kts container:
		"""
		Column A,Column B,Column C
		1,2,3
		"""
	And blob storage file School/123456/2022/csv/ks2_school_provisional.csv exists in downloads-asp container:
		"""
		Column A,Column B,Column C
		1,2,3
		"""
	And blob storage file School/123456/2022/csv/ks4_school_final.csv exists in downloads-asp container:
		"""
		Column A,Column B,Column C
		1,2,3
		"""
	When I navigate to /my-schools/123456/download-data/select-files/?selectedYear=2022
	And I update the element "#app-available-downloads-file-asp-school-ks2-school-123456-2022-provisional" to be checked
	And I click the button "*[data-testid='selectedFilesSubmit']"
	Then the path should be /my-schools/123456/download-data/select-format/?selectedYear=2022&selectedFiles=asp-school-ks2-school-123456-2022-provisional

@Javascript:disabled
Scenario: Data downloads 'Download school data' - common page elements
	Given Establishment "136028" exists:
		"""
		{
			"name": "Dagenham Park CofE School"
		}
		"""
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
			},
			{
				"id": "kts-school-phonics-pupil",
				"source": "KTS",
				"scope": "School",
				"dataSetType": "Phonics",
				"label": "Phonics",
				"filePathPattern": "School/{urn}/{year}/{filetype}/phonics_pupil_{version}.{filetype}"
			},
			{
				"id": "asp-school-ks2-school",
				"source": "ASP",
				"scope": "School",
				"dataSetType": "KeyStage2",
				"label": "Key stage 2 (KS2)",
				"filePathPattern": "School/{urn}/{year}/{filetype}/ks2_school_{version}.{filetype}"
			},
			{
				"id": "asp-school-ks4-school",
				"source": "ASP",
				"scope": "School",
				"dataSetType": "KeyStage4",
				"label": "Key stage 4 (KS4)",
				"filePathPattern": "School/{urn}/{year}/{filetype}/ks4_school_{version}.{filetype}"
			}
		]
		"""
	And blob storage file School/136028/2022/csv/ks2_pupil_final.csv exists in downloads-kts container:
		"""
		Column A,Column B,Column C
		1,2,3
		"""
	And blob storage file School/136028/2022/csv/ks4_pupil_final.csv exists in downloads-kts container:
		"""
		Column A,Column B,Column C
		1,2,3
		"""
	And blob storage file School/136028/2022/csv/phonics_pupil_final.csv exists in downloads-kts container:
		"""
		Column A,Column B,Column C
		1,2,3
		"""
	And blob storage file School/136028/2022/csv/ks2_school_provisional.csv exists in downloads-asp container:
		"""
		Column A,Column B,Column C
		1,2,3
		"""
	And blob storage file School/136028/2022/csv/ks4_school_final.csv exists in downloads-asp container:
		"""
		Column A,Column B,Column C
		1,2,3
		"""
	When I navigate to /my-schools/136028/download-data/select-format/?selectedYear=2022&selectedFiles=kts-136028-ks2-2022-final-school&selectedFiles=asp-136028-ks2-2022-provisional-school
	Then the page title should be "Download data"
	And the breadcrumb trail should be:
		| text                                    | href                                                             | current |
		| Home                                    | /                                                                |         |
		| My schools                              | /my-schools/                                                     |         |
		| Dagenham Park CofE School               | /my-schools/136028/                                              |         |
		| Download data                           | /my-schools/136028/download-data/                                |         |
		| Dates available for download            | /my-schools/136028/download-data/                                |         |
		| Data files available for download       | /my-schools/136028/download-data/select-files/?selectedYear=2022 |         |
		| Download Dagenham Park CofE School data |                                                                  | true    |
	And the sub-navigation should be:
		| text          | href                              | current |
		| Download data | /my-schools/136028/download-data/ | true    |
		| Other reports | /my-schools/136028/other-reports/ |         |
		| Useful links  | /my-schools/136028/useful-links/  |         |
	And the side navigation should be:
		| text                           | href                              | current |
		| Dagenham Park CofE School data | /my-schools/136028/download-data/ | true    |
	And the sub-page title should be "Download Dagenham Park CofE School data" with caption "Dagenham Park CofE School data"

@Javascript:disabled
Scenario Outline: Data downloads 'Download school data' - page should contain three links
	Given Establishment "136028" exists:
		"""
		{
			"name": "Dagenham Park CofE School"
		}
		"""
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
				"id": "kts-school-ks2-pupil",
				"source": "KTS",
				"scope": "School",
				"dataSetType": "KeyStage2",
				"label": "Key stage 2 (KS2)",
				"filePathPattern": "School/{urn}/{year}/{filetype}/ks2_pupil_{version}.{filetype}"
			}
		]
		"""
	And blob storage file School/136028/2022/csv/ks2_pupil_final.csv exists in downloads-kts container:
		"""
		Column A,Column B,Column C
		1,2,3
		"""
	When I navigate to /my-schools/136028/download-data/select-format/?selectedYear=2022&selectedFiles=kts-136028-ks2-2022-final-school&selectedFiles=asp-136028-ks2-2022-provisional-school
	Then the element "[data-testid="select-format-description"]" should have the text content "The data included in your download is the pupil level / aggregated data for your school."
	And the available download formats should be:
		| text                | href                                                                                                                                                                |
		| Data in CSV format  | /my-schools/136028/download-data/download-as-zip/?fileType=CSV&selectedFiles=kts-136028-ks2-2022-final-school&selectedFiles=asp-136028-ks2-2022-provisional-school  |
		| Data in XLSX format | /my-schools/136028/download-data/download-as-zip/?fileType=XLSX&selectedFiles=kts-136028-ks2-2022-final-school&selectedFiles=asp-136028-ks2-2022-provisional-school |
		| Data in TSV format  | /my-schools/136028/download-data/download-as-zip/?fileType=TSV&selectedFiles=kts-136028-ks2-2022-final-school&selectedFiles=asp-136028-ks2-2022-provisional-school  |


@Javascript:disabled
Scenario Outline: Data downloads 'Download school data' - Download other dates link should link back to first step
	Given Establishment "136028" exists:
		"""
		{
			"name": "Dagenham Park CofE School"
		}
		"""
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
				"id": "kts-school-ks2-pupil",
				"source": "KTS",
				"scope": "School",
				"dataSetType": "KeyStage2",
				"label": "Key stage 2 (KS2)",
				"filePathPattern": "School/{urn}/{year}/{filetype}/ks2_pupil_{version}.{filetype}"
			}
		]
		"""
	And blob storage file School/136028/2022/csv/ks2_pupil_final.csv exists in downloads-kts container:
		"""
		Column A,Column B,Column C
		1,2,3
		"""
	When I navigate to /my-schools/136028/download-data/select-format/?selectedYear=2022&selectedFiles=kts-136028-ks2-2022-final-school
	Then the element "[data-testid="available-downloads-other-dates"]" should have the href "/my-schools/136028/download-data/"

@Javascript:disabled
Scenario Outline: My Schools users should see Download data card on School landing page
	Given Establishment "123456" exists:
		"""
		{
		    "name": "Test School",
		    "localAuthority": {
		        "code": "301"
		    },
		    "multiAcademyTrust": {
		        "uid": 1234
		    },
		    "diocese": {
		        "name": "Test Diocese"
		    }
		}
		"""
	And Content Template "school-landing-page" exists:
		"""
			{
			    "Views": [    
			        {
			            "ViewId": "Card",
			            "ViewContent": {
			                "AuthorizationPolicy": "NamedData",
			                "Title": "Download data",
			                "LinkUrl": "download-data/",
			                "Text": "Download data for Analyse school performance and Key to success."
			            }
			        },
			        {
			            "ViewId": "Card",
			            "ViewContent": {
			                "Title": "Other reports",
			                "LinkUrl": "other-reports/",
			                "Text": "View reports on school performance, Ofsted inspections, absence and exclusions and school characteristics."
			            }
			        },
			        {
			            "ViewId": "Card",
			            "ViewContent": {
			                "Title": "Useful links",
			                "LinkUrl": "useful-links/",
			                "Text": "View links to other services and published documents that may be useful."
			            }
			        }
			    ]
			}
		"""
	And I am a <userRole>
	When I navigate to /my-schools/123456/
	Then I should get a 200 response
	And the landing page cards should be:
		| Title         | Url            | Content                                                                                                    |
		| Download data | download-data/ | Download data for Analyse school performance and Key to success.                                           |
		| Other reports | other-reports/ | View reports on school performance, Ofsted inspections, absence and exclusions and school characteristics. |
		| Useful links  | useful-links/  | View links to other services and published documents that may be useful.                                   |

Examples:
	| userRole                                      |
	| LA Named user for Local Authority "301"       |
	| MAT Named user for Multi-Academy Trust "1234" |
	| Diocese Named user for Diocese "Test Diocese" |

@Javascript:disabled
Scenario Outline: My Schools users should not see Download data card on School landing page
	Given Establishment "123456" exists:
		"""
		{
		    "name": "Test School",
		    "localAuthority": {
		        "code": "301"
		    },
		    "multiAcademyTrust": {
		        "uid": 1234
		    },
		    "diocese": {
		        "name": "Test Diocese"
		    }
		}
		"""
	And Content Template "school-landing-page" exists:
		"""
			{
			    "Views": [    
			        {
			            "ViewId": "Card",
			            "ViewContent": {
			                "AuthorizationPolicy": "NamedData",
			                "Title": "Download data",
			                "LinkUrl": "download-data/",
			                "Text": "Download data for Analyse school performance and Key to success."
			            }
			        },
			        {
			            "ViewId": "Card",
			            "ViewContent": {
			                "Title": "Other reports",
			                "LinkUrl": "other-reports/",
			                "Text": "View reports on school performance, Ofsted inspections, absence and exclusions and school characteristics."
			            }
			        },
			        {
			            "ViewId": "Card",
			            "ViewContent": {
			                "Title": "Useful links",
			                "LinkUrl": "useful-links/",
			                "Text": "View links to other services and published documents that may be useful."
			            }
			        }
			    ]
			}
		"""
	And I am a <userRole>
	When I navigate to /my-schools/123456/
	Then I should get a 200 response
	And the landing page cards should be:
		| Title         | Url            | Content                                                                                                    |
		| Other reports | other-reports/ | View reports on school performance, Ofsted inspections, absence and exclusions and school characteristics. |
		| Useful links  | useful-links/  | View links to other services and published documents that may be useful.                                   |

Examples:
	| userRole                                         |
	| LA Unnamed user for Local Authority "301"        |
	| MAT Unnamed user for Multi-Academy Trust "1234"  |
	| MAT Governor user for Multi-Academy Trust "1234" |
	| Diocese Unnamed user for Diocese "Test Diocese"  |
	

@Javascript:disabled
Scenario: Data downloads sub navigation item should not be visible to Unnamed policy users
	Given I am a <Roles> user
	Given Establishment "136028" exists:
		"""
		{
			"name": "Dagenham Park CofE School"
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
		]f
		"""
	And blob storage file School/136028/2022/csv/ks2_pupil_final.csv exists in downloads-kts container:
		"""
		Column A,Column B,Column C
		1,2,3
		"""
	When I navigate to /school/136028/other-reports/
	Then I should get a 200 response
	And the sub-navigation should be:
		| text          | href                          | current |
		| Other reports | /school/136028/other-reports/ | true    |
		| Useful links  | /school/136028/useful-links/  |         |
Examples:
	| Roles          |
	| DfE Unnamed    |
	| Ofsted Unnamed |