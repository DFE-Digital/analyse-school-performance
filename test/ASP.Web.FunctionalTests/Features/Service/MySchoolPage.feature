Feature: My school page

@Javascript:disabled
Scenario: A non-School user should not be able to access the 'My school' page. Instead, they should see a 403 Access not allowed page.
	Given I am a MAT Named user for Multi-Academy Trust "1234"
	When I navigate to /my-school/
	Then I should get a 403 response
	And the page title should be "Access not allowed"
	And the element "h1.govuk-heading-l" should have the text content "Access not allowed"

@Javascript:disabled
Scenario: A user with access to all schools should not be able to access the 'My school' page. Instead, they should see a 403 Access not allowed page.
	Given I am a DfE Named user
	When I navigate to /my-school/
	Then I should get a 403 response
	And the page title should be "Access not allowed"
	And the element "h1.govuk-heading-l" should have the text content "Access not allowed"

@Javascript:disabled
Scenario: School page should display server error page if user's Establishment is not visible
	Given non-visible Establishment "111111" exists:
		"""
		{
			"name": "Thursby Primary School"
		}
		"""
	And I am a School Named user for Establishment "111111"
	When I navigate to /my-school/
	Then I should get a 500 response
	And the page title should be "Sorry, there is a problem with the service"
	And the element "h1.govuk-heading-l" should have the text content "Sorry, there is a problem with the service"
	Then the element "*[data-testid='error-display-message']" should have the text content "Error message: API error: /api/schools/111111 Could not find school with URN "111111"."

@Javascript:disabled
Scenario: School page should display server error page if user's Establishment is deleted
	Given deleted Establishment "111111" exists:
		"""
		{
			"name": "Thursby Primary School"
		}
		"""
	And I am a School Named user for Establishment "111111"
	When I navigate to /my-school/
	Then I should get a 500 response
	And the page title should be "Sorry, there is a problem with the service"
	And the element "h1.govuk-heading-l" should have the text content "Sorry, there is a problem with the service"
	Then the element "*[data-testid='error-display-message']" should have the text content "Error message: API error: /api/schools/111111 Could not find school with URN "111111"."

@Javascript:disabled
Scenario: School page should display server error page if user's Establishment does not exist
	Given Establishment "111111" exists:
		"""
		{
			"name": "Thursby Primary School"
		}
		"""
	And I am a School Named user for Establishment "222222"
	When I navigate to /my-school/
	Then I should get a 500 response
	And the page title should be "Sorry, there is a problem with the service"
	And the element "h1.govuk-heading-l" should have the text content "Sorry, there is a problem with the service"
	And the element "*[data-testid='error-display-message']" should have the text content "Error message: API error: /api/schools/222222 Could not find school with URN "222222"."

@Javascript:disabled
Scenario: School page should be accessible if user's Establishment exists
	Given Establishment "123456" exists:
		"""
		{
			"name": "Hollinswood Primary School"
		}
		"""
	And I am a School Named user for Establishment "123456"
	When I navigate to /my-school/
	Then I should get a 200 response
	Then the page title should be "My school"
	Then the page title should be "My school"
	Then the page subtitle should be "Hollinswood Primary School (URN: 123456)"

@Javascript:disabled
Scenario: School page should contain eight app card container elements
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
			},
			{
				"ViewId": "Card",
				"ViewContent": {
					"Id": "app-card-data-downloads",
					"Title": "Data downloads",
					"LinkUrl": "data-downloads",
					"Text": "Download data for Analyse school performance and Key to success"
				}
			}
			]
		}
		"""
	And Establishment "123456" exists:
		"""
		{
			"name": "Hollinswood Primary School"
		}
		"""
	And I am a School Named user for Establishment "123456"
	When I navigate to /my-school/
	Then the element "#app-card-container" class should contain "app-grid-container-four-column"
	And the elements "#app-card-container .app-card" should total 8

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

	And the element "#app-card-data-downloads h2 a" should have the href "data-downloads"
	And the element "#app-card-data-downloads h2 a" should have the text content "Data downloads"
	And the element "#app-card-data-downloads p" should have the text content "Download data for Analyse school performance and Key to success"


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
			},
			"name": "Hollinswood Primary School",
			"noOfPupils": 404,
			"religiousDenomination": {
				"name": "Does not apply",
			},
			"resourcedProvisionType": {
				"name": "Not recorded",
			},
			"laestab": "001/1234",
			"multiAcademyTrust": {
				"uid": "0001",
				"name": "THE DEAN TRUST"
			},
			"diocese": {
				"name": "Diocese of Chelmsford"
			},
		}
		"""
	And I am a School Named user for Establishment "123456"
	When I navigate to /my-school/
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
	And the element "*[data-testid='school-details-laestab-key']" should have the text content "LAESTAB"
	And the element "*[data-testid='school-details-laestab-value']" should have the text content "001/1234"
	And the element "*[data-testid='school-details-mat-key']" should have the text content "Multi-academy trust"
	And the element "*[data-testid='school-details-mat-value']" should have the text content "THE DEAN TRUST"
	And the element "*[data-testid='school-details-diocese-key']" should have the text content "Diocese"
	And the element "*[data-testid='school-details-diocese-value']" should have the text content "Diocese of Chelmsford"

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
			"localAuthority": null,
			"name": "Hollinswood Primary School",
			"noOfPupils": null,
			"religiousDenomination": null,
			"resourcedProvisionType": null,
			"laestab": null,
			"multiAcademyTrust": null,
			"diocese": null
		}
		"""
	And I am a School Named user for Establishment "123456"
	When I navigate to /my-school/
	Then I should get a 200 response
	And the element "*[data-testid='school-page-details-state-closed']" should have the text content "Show"
	And the element "*[data-testid='school-page-details-state-open']" should have the text content "Hide"
	And the element "*[data-testid='school-details-address-key']" should have the text content "Address"
	And the element "*[data-testid='school-details-address-value']" should have the text content "Data not available"
	And the element "*[data-testid='school-details-school-type-key']" should have the text content "School type"
	And the element "*[data-testid='school-details-school-type-value']" should have the text content "Data not available"
	And the element "*[data-testid='school-details-education-key']" should have the text content "Education phase"
	And the element "*[data-testid='school-details-education-value']" should have the text content "Data not available"
	And the element "*[data-testid='school-details-gender-key']" should have the text content "Gender of entry"
	And the element "*[data-testid='school-details-gender-value']" should have the text content "Data not available"
	And the element "*[data-testid='school-details-la-key']" should have the text content "Local authority"
	And the element "*[data-testid='school-details-la-value']" should have the text content "Data not available"
	And the element "*[data-testid='school-details-gender-key']" should have the text content "Gender of entry"
	And the element "*[data-testid='school-details-gender-value']" should have the text content "Data not available"
	And the element "*[data-testid='school-details-principal-key']" should have the text content "Headteacher / Principal"
	And the element "*[data-testid='school-details-principal-value']" should have the text content "Data not available"
	And the element "*[data-testid='school-details-age-key']" should have the text content "Age range"
	And the element "*[data-testid='school-details-age-value']" should have the text content "Data not available"
	And the element "*[data-testid='school-details-religious-key']" should have the text content "Religious character"
	And the element "*[data-testid='school-details-religious-value']" should have the text content "Data not available"
	And the element "*[data-testid='school-details-admission-key']" should have the text content "Admissions policy"
	And the element "*[data-testid='school-details-admission-value']" should have the text content "Data not available"
	And the element "*[data-testid='school-details-provision-key']" should have the text content "SEN unit or resourced provision"
	And the element "*[data-testid='school-details-provision-value']" should have the text content "Data not available"
	And the element "*[data-testid='school-details-pupils-key']" should have the text content "Number of pupils"
	And the element "*[data-testid='school-details-pupils-value']" should have the text content "Data not available"
	And the element "*[data-testid='school-details-laestab-key']" should have the text content "LAESTAB"
	And the element "*[data-testid='school-details-laestab-value']" should have the text content "Data not available"
	And the element "*[data-testid='school-details-mat-key']" should not exist
	And the element "*[data-testid='school-details-mat-value']" should not exist
	And the element "*[data-testid='school-details-diocese-key']" should have the text content "Diocese"
	And the element "*[data-testid='school-details-diocese-value']" should have the text content "Not applicable"

@Javascript:disabled
Scenario: School page should show if values are null case 2
	Given Establishment "123456" exists:
		"""
		{
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
				"name": null,
			},
			"name": null,
			"noOfPupils": null,
			"religiousDenomination": {
				"name": null,
			},
			"resourcedProvisionType": {
				"name": null,
			},
			"laestab": null,
			"multiAcademyTrust": {
				"uid": "0001",
				"name": null
			},
			"diocese": {
				"name": null
			},
		}
		"""
	And I am a School Named user for Establishment "123456"
	When I navigate to /my-school/
	Then I should get a 200 response
	And the element "*[data-testid='school-page-details-state-closed']" should have the text content "Show"
	And the element "*[data-testid='school-page-details-state-open']" should have the text content "Hide"
	And the element "*[data-testid='school-details-address-key']" should have the text content "Address"
	And the element "*[data-testid='school-details-address-value']" should have the text content "Data not available"
	And the element "*[data-testid='school-details-school-type-key']" should have the text content "School type"
	And the element "*[data-testid='school-details-school-type-value']" should have the text content "Data not available"
	And the element "*[data-testid='school-details-education-key']" should have the text content "Education phase"
	And the element "*[data-testid='school-details-education-value']" should have the text content "Data not available"
	And the element "*[data-testid='school-details-gender-key']" should have the text content "Gender of entry"
	And the element "*[data-testid='school-details-gender-value']" should have the text content "Data not available"
	And the element "*[data-testid='school-details-la-key']" should have the text content "Local authority"
	And the element "*[data-testid='school-details-la-value']" should have the text content "Data not available"
	And the element "*[data-testid='school-details-gender-key']" should have the text content "Gender of entry"
	And the element "*[data-testid='school-details-gender-value']" should have the text content "Data not available"
	And the element "*[data-testid='school-details-principal-key']" should have the text content "Headteacher / Principal"
	And the element "*[data-testid='school-details-principal-value']" should have the text content "Data not available"
	And the element "*[data-testid='school-details-age-key']" should have the text content "Age range"
	And the element "*[data-testid='school-details-age-value']" should have the text content "Data not available"
	And the element "*[data-testid='school-details-religious-key']" should have the text content "Religious character"
	And the element "*[data-testid='school-details-religious-value']" should have the text content "Data not available"
	And the element "*[data-testid='school-details-admission-key']" should have the text content "Admissions policy"
	And the element "*[data-testid='school-details-admission-value']" should have the text content "Data not available"
	And the element "*[data-testid='school-details-provision-key']" should have the text content "SEN unit or resourced provision"
	And the element "*[data-testid='school-details-provision-value']" should have the text content "Data not available"
	And the element "*[data-testid='school-details-pupils-key']" should have the text content "Number of pupils"
	And the element "*[data-testid='school-details-pupils-value']" should have the text content "Data not available"
	And the element "*[data-testid='school-details-laestab-key']" should have the text content "LAESTAB"
	And the element "*[data-testid='school-details-laestab-value']" should have the text content "Data not available"
	And the element "*[data-testid='school-details-mat-key']" should not exist
	And the element "*[data-testid='school-details-mat-value']" should not exist
	And the element "*[data-testid='school-details-diocese-key']" should have the text content "Diocese"
	And the element "*[data-testid='school-details-diocese-value']" should have the text content "Not applicable"

@Javascript:disabled
Scenario: Details disclosure element text should read 'Show school details' when closed
	Given Establishment "123456" exists:
		"""
		{
			"name": "Hollinswood Primary School"
		}
		"""
	And I am a School Named user for Establishment "123456"
	When I navigate to /my-school/
	Then the element "*[data-testid='school-page-details-state-closed']" should have the text content "Show"
	
@Javascript:disabled
Scenario Outline: My school page - common page elements
	Given I am a <School> user for Establishment "123456"
	And Establishment "123456" exists:
		"""
		{
			"name": "Hollinswood Primary School"
		}
		"""
	When I navigate to /my-school/
	Then the page title should be "My school"
	And the page subtitle should be "Hollinswood Primary School (URN: 123456)"
	And the breadcrumb trail should be:
		| Link Text | Url |
		| Home      | /   |
Examples:
	| School          |
	| School Named    |
	| School Unnamed  |
	| School Governor |
	
@Javascript:disabled
Scenario: Data downloads 'Dates available for download' page - common page elements
	Given Establishment "136028" exists:
		"""
		{
			"name": "Dagenham Park CofE School"
		}
		"""
	And I am a School Named user for Establishment "136028"
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
	When I navigate to /my-school/download-data
	Then the page title should be "Download data"
	And the breadcrumb trail should be:
		| Link Text     | Url                       |
		| Home          | /                         |
		| My school     | /my-school/               |
		| Download data | /my-school/download-data/ |
	And the sub-navigation should be:
		| Link Text     | Url                       | Current Page |
		| Download data | /my-school/download-data/ | true         |
		| Other reports | /my-school/other-reports/ |              |
		| Useful links  | /my-school/useful-links/  |              |
	And the side navigation should be:
		| Link Text                      | Url                       | Current Page |
		| Dagenham Park CofE School data | /my-school/download-data/ | true         |
	And the sub-page title should be "Dates available for download" with caption "Dagenham Park CofE School data"

@Javascript:disabled
Scenario Outline: Data downloads 'Dates available for download' - page should contain three radio buttons
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
Scenario: Data downloads 'Dates available for download' - when no date is selected and Continue button clicked, should show validation error
	Given Establishment "123456" exists:
		"""
		{
			"name": "Hollinswood Primary School"
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
	And I am a School Named user for Establishment "123456"
	When I navigate to /my-school/download-data/
	And I click the button "*[data-testid='selectedYearSubmit']"
	Then the path should be /my-school/download-data/
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
	And I am a School Named user for Establishment "123456"
	When I navigate to /my-school/download-data/
	And I update the element "#app-available-downloads-dates-2022" to be checked
	And I click the button "*[data-testid='selectedYearSubmit']"
	Then the path should be /my-school/download-data/select-files/?selectedYear=2022

@Javascript:disabled
Scenario: Data downloads "Data files available for download' - common page elements
	Given Establishment "136028" exists:
		"""
		{
			"name": "Dagenham Park CofE School"
		}
		"""
	And I am a School Named user for Establishment "136028"
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
	When I navigate to /my-school/download-data/select-files/?selectedYear=2022
	Then the page title should be "Download data"
	And the breadcrumb trail should be:
		| Link Text                    | Url                       |
		| Home                         | /                         |
		| My school                    | /my-school/               |
		| Download data                | /my-school/download-data/ |
		| Dates available for download | /my-school/download-data/ |
	And the sub-navigation should be:
		| Link Text     | Url                       | Current Page |
		| Download data | /my-school/download-data/ | true         |
		| Other reports | /my-school/other-reports/ |              |
		| Useful links  | /my-school/useful-links/  |              |
	And the side navigation should be:
		| Link Text                      | Url                       | Current Page |
		| Dagenham Park CofE School data | /my-school/download-data/ | true         |
	And the sub-page title should be "Data files available for download" with caption "Dagenham Park CofE School data"

@Javascript:disabled
Scenario Outline: Data downloads 'Data files available for download' - page should contain three checkbox groups
	Given Establishment "136028" exists:
		"""
		{
			"name": "Dagenham Park CofE School"
		}
		"""
	And I am a School Named user for Establishment "136028"
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
	When I navigate to /my-school/download-data/select-files/?selectedYear=2022
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
	And I am a School Named user for Establishment "136028"
	When I navigate to /my-school/download-data/select-files/?selectedYear=2022
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
	And I am a School Named user for Establishment "123456"
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
	When I navigate to /my-school/download-data/select-files/?selectedYear=2022
	And I click the button "*[data-testid='selectedFilesSubmit']"
	Then the path should be /my-school/download-data/select-files/?selectedYear=2022
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
	And I am a School Named user for Establishment "123456"
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
	When I navigate to /my-school/download-data/select-files/?selectedYear=2022
	And I update the element "#app-available-downloads-file-asp-school-ks2-school-123456-2022-provisional" to be checked
	And I click the button "*[data-testid='selectedFilesSubmit']"
	Then the path should be /my-school/download-data/select-format/?selectedYear=2022&selectedFiles=asp-school-ks2-school-123456-2022-provisional

@Javascript:disabled
Scenario: Data downloads 'Download school data' - common page elements
	Given Establishment "136028" exists:
		"""
		{
			"name": "Dagenham Park CofE School"
		}
		"""
	And I am a School Named user for Establishment "136028"
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
	When I navigate to /my-school/download-data/select-format/?selectedYear=2022&selectedFiles=kts-800200-ks2-2022-final-school&selectedFiles=asp-800200-ks2-2022-provisional-school
	Then the page title should be "Download data"
	And the breadcrumb trail should be:
		| Link Text                         | Url                                                      |
		| Home                              | /                                                        |
		| My school                         | /my-school/                                              |
		| Download data                     | /my-school/download-data/                                |
		| Dates available for download      | /my-school/download-data/                                |
		| Data files available for download | /my-school/download-data/select-files/?selectedYear=2022 |
	And the sub-navigation should be:
		| Link Text     | Url                       | Current Page |
		| Download data | /my-school/download-data/ | true         |
		| Other reports | /my-school/other-reports/ |              |
		| Useful links  | /my-school/useful-links/  |              |
	And the side navigation should be:
		| Link Text                      | Url                       | Current Page |
		| Dagenham Park CofE School data | /my-school/download-data/ | true         |
	And the sub-page title should be "Download Dagenham Park CofE School data" with caption "Dagenham Park CofE School data"

@Javascript:disabled
Scenario Outline: Data downloads 'Download school data' - Download other dates link should link back to first step
	Given Establishment "136028" exists:
		"""
		{
			"name": "Dagenham Park CofE School"
		}
		"""
	And I am a School Named user for Establishment "136028"
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
	When I navigate to /my-school/download-data/select-format/?selectedYear=2022&selectedFiles=kts-136028-ks2-2022-final-school
	Then the element "[data-testid="available-downloads-other-dates"]" should have the href "/my-school/download-data/"

@Javascript:disabled
Scenario: School Named user should see Download data card on School landing page
	Given Establishment "123456" exists:
		"""
		{
		    "name": "Test School"
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
	And I am a School Named user for Establishment "123456"
	When I navigate to /my-school/
	Then I should get a 200 response
	And the landing page cards should be:
		| Title         | Url            | Content                                                                                                    |
		| Download data | download-data/ | Download data for Analyse school performance and Key to success.                                           |
		| Other reports | other-reports/ | View reports on school performance, Ofsted inspections, absence and exclusions and school characteristics. |
		| Useful links  | useful-links/  | View links to other services and published documents that may be useful.                                   |
   
@Javascript:disabled
Scenario: School Unnamed/School Governor users should not see Download data card on School landing page
	Given Establishment "123456" exists:
		"""
		{
		    "name": "Test School"
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
	And I am a School Named user for Establishment "123456"
	And I am a <userRole>
	When I navigate to /my-school/
	Then I should get a 200 response
	And the landing page cards should be:
		| Title         | Url            | Content                                                                                                    |
		| Other reports | other-reports/ | View reports on school performance, Ofsted inspections, absence and exclusions and school characteristics. |
		| Useful links  | useful-links/  | View links to other services and published documents that may be useful.                                   |
Examples:
	| userRole                                        |
	| School Unnamed user for Establishment "123456"  |
	| School Governor user for Establishment "123456" |
	
@Javascript:disabled
Scenario Outline: Should return (200) response if the School Named user accesses /my-school/download-data
	Given I am a School Named user for Establishment "123456"
	And Establishment "123456" exists:
		"""
		{
			"name": "Dagenham Park CofE School"
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
	And blob storage file School/123456/2022/csv/ks2_pupil_final.csv exists in downloads-kts container:
		"""
		Column A,Column B,Column C
		1,2,3
		"""
	When I navigate to /my-school/download-data
	Then I should get a 200 response

@Javascript:disabled
Scenario Outline: Should return (403) response if the below mentioned user roles access /my-school/download-data
	Given I am a <userRole>
	And Establishment "123456" exists:
		"""
		{
			"name": "Dagenham Park CofE School"
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
	And blob storage file School/123456/2022/csv/ks2_pupil_final.csv exists in downloads-kts container:
		"""
		Column A,Column B,Column C
		1,2,3
		"""
	When I navigate to /my-school/download-data
	Then I should get a 403 response
Examples:
	| userRole                                         |
	| School Unnamed user for Establishment "123456"   |
	| School Governor user for Establishment "123456"  |
	| LA Named user for Local Authority "301"          |
	| LA Unnamed user for Local Authority "301"        |
	| MAT Named user for Multi-Academy Trust "1234"    |
	| MAT Unnamed user for Multi-Academy Trust "1234"  |
	| MAT Governor user for Multi-Academy Trust "1234" |
	| Diocese Named user for Diocese "Test Diocese"    |
	| Diocese Unnamed user for Diocese "Test Diocese"  |
	| DfE Named user                                   |
	| DfE Unnamed user                                 |
	| Ofsted Unnamed user                              |
	| Super Admin user                                 |

@Javascript:disabled
Scenario Outline: Descriptions for link to multiple establishments with description text
	Given Establishment "100001" exists:
	"""
	{ 
	  "name": "Test School 1",
	  "links": [
	    {
	      "linkedUrn": "100002",
	      "establishedDate": <EstablishedDate>,
	      "linkType": {
	        "code": "<Code>",
	        "name": "<Name>"
	      }
	    },
	    {
	      "linkedUrn": "100003",
	      "establishedDate": <EstablishedDate>,
	      "linkType": {
	        "code": "<Code>",
	        "name": "<Name>"
	      }
	    },
	    {
	      "linkedUrn": "100004",
	      "establishedDate": <EstablishedDate>,
	      "linkType": {
	        "code": "<Code>",
	        "name": "<Name>"
	      }
	    }
	  ]
	}
	"""
	And I am a School Named user for Establishment "100001"
	And Establishment "100002" exists:
	"""
	{ 
	  "name": "Test School 2"
	}
	"""
	And Establishment "100003" exists:
	"""
	{ 
	  "name": "Test School 3"
	}
	"""
	And Establishment "100004" exists:
	"""
	{ 
	  "name": "Test School 4"
	}
	"""
	When I navigate to /my-school/
	Then I should get a 200 response
	And the element "[data-testid="linked-school-description-1"]" should have the text content "<Description>"
	Examples:
	  | Code | Name                                                           | EstablishedDate | Description                                                                                                               |
	  | 1    | Predecessor                                                    | null            | Test School 1 was previously Test School 2, Test School 3 and Test School 4.                                              |
	  | 1    | Predecessor                                                    | "2020-10-01"    | Test School 1 was previously Test School 2, Test School 3 and Test School 4 up until 1 October 2020.                      |
	  | 1F   | Predecessor - Split School                                     | null            | Test School 1 was created as the result of a split from Test School 2, Test School 3 and Test School 4.                   |
	  | 1F   | Predecessor - Split School                                     | "2020-10-01"    | Test School 1 was created as the result of a split from Test School 2, Test School 3 and Test School 4 on 1 October 2020. |
	  | 1I   | Closure                                                        | null            | Test School 1 was previously Test School 2, Test School 3 and Test School 4.                                              |
	  | 1I   | Closure                                                        | "2020-10-01"    | Test School 1 was previously Test School 2, Test School 3 and Test School 4, which closed on 1 October 2020.              |
	  | 1L   | Predecessor - merged                                           | null            | Test School 1 was merged with Test School 2, Test School 3 and Test School 4.                                             |
	  | 1L   | Predecessor - merged                                           | "2020-10-01"    | Test School 1 was merged with Test School 2, Test School 3 and Test School 4 on 1 October 2020.                           |
	  | 2    | Successor                                                      | null            | Test School 1 became Test School 2, Test School 3 and Test School 4.                                                      |
	  | 2    | Successor                                                      | "2020-10-01"    | Test School 1 became Test School 2, Test School 3 and Test School 4 on 1 October 2020.                                    |
	  | 2A   | Expansion                                                      | null            | Test School 1 became Test School 2, Test School 3 and Test School 4.                                                      |
	  | 2A   | Expansion                                                      | "2020-10-01"    | Test School 1 became Test School 2, Test School 3 and Test School 4 on 1 October 2020.                                    |
	  | 2F   | Successor - Split School                                       | null            | Test School 2, Test School 3 and Test School 4 were split off from Test School 1.                                         |
	  | 2F   | Successor - Split School                                       | "2020-10-01"    | Test School 2, Test School 3 and Test School 4 were split off from Test School 1 on 1 October 2020.                       |
	  | 2K   | Result of Amalgamation                                         | null            | Test School 1 was the result of an amalgamation of Test School 2, Test School 3 and Test School 4.                                        |
	  | 2K   | Result of Amalgamation                                         | "2020-10-01"    | Test School 1 was the result of an amalgamation of Test School 2, Test School 3 and Test School 4 on 1 October 2020.                      |
	  | 2O   | Merged - change in age range                                   | null            | Test School 1 was merged with Test School 2, Test School 3 and Test School 4.                                             |
	  | 2O   | Merged - change in age range                                   | "2020-10-01"    | Test School 1 was merged with Test School 2, Test School 3 and Test School 4 on 1 October 2020.                           |
	  | 2P   | Merged - expansion of school capacity                          | null            | Test School 1 was merged with Test School 2, Test School 3 and Test School 4.                                             |
	  | 2P   | Merged - expansion of school capacity                          | "2020-10-01"    | Test School 1 was merged with Test School 2, Test School 3 and Test School 4 on 1 October 2020.                           |
	  | 2Q   | Merged - expansion in school capacity and changer in age range | null            | Test School 1 was merged with Test School 2, Test School 3 and Test School 4.                                             |
	  | 2Q   | Merged - expansion in school capacity and changer in age range | "2020-10-01"    | Test School 1 was merged with Test School 2, Test School 3 and Test School 4 on 1 October 2020.                           |
	  | 6    | Successor - merged                                             | null            | Test School 1 was merged with Test School 2, Test School 3 and Test School 4.                                             |
	  | 6    | Successor - merged                                             | "2020-10-01"    | Test School 1 was merged with Test School 2, Test School 3 and Test School 4 on 1 October 2020.                           |
	  | 6.1  | Predecessor - amalgamated                                      | null            | Test School 1 was the result of an amalgamation of Test School 2, Test School 3 and Test School 4.                                        |
	  | 6.1  | Predecessor - amalgamated                                      | "2020-10-01"    | Test School 1 was the result of an amalgamation of Test School 2, Test School 3 and Test School 4 on 1 October 2020.                      |
	  | 6.2  | Successor - amalgamated                                        | null            | Test School 1 was amalgamated into Test School 2, Test School 3 and Test School 4.                                        |
	  | 6.2  | Successor - amalgamated                                        | "2020-10-01"    | Test School 1 was amalgamated into Test School 2, Test School 3 and Test School 4 on 1 October 2020.                      |
   
@Javascript:disabled
Scenario: Should provide default description if linkType is missing
	Given Establishment "100001" exists:
	"""
	{ 
	  "name": "Test School 1",
	  "links": [
	    {
	      "linkedUrn": "100002"
	    },
	    {
	      "linkedUrn": "100003",
	      "establishedDate": "2020-03-01"
	    }
	  ]
	}
	"""
	And I am a School Named user for Establishment "100001"
	And Establishment "100002" exists:
	"""
	{ 
	  "name": "Test School 2"
	}
	"""
	And Establishment "100003" exists:
	"""
	{ 
	  "name": "Test School 3"
	}
	"""
	When I navigate to /my-school/
	Then I should get a 200 response 
	And the element "[data-testid="linked-school-description-1"]" should have the text content "Test School 1 was linked to Test School 2."
	And the element "[data-testid="linked-school-description-2"]" should have the text content "Test School 1 was linked to Test School 3 on 1 March 2020."
	
@Javascript:disabled
Scenario Outline: Display linked establishment descriptions with links and established dates
	Given Establishment "100001" exists:
	"""
	{ 
	  "name": "Test School 1",
	  "links": [
	    {
	      "linkedUrn": "100002"
	    },
	    {
	      "linkedUrn": "100003",
	      "establishedDate": "2020-03-01"
	    }
	  ]
	}
	"""
	And I am a School Named user for Establishment "100001"
	And Establishment "100002" exists:
	"""
	{ 
	  "name": "Test School 2"
	}
	"""
	And Establishment "100003" exists:
	"""
	{ 
	  "name": "Test School 3"
	}
	"""
	When I navigate to /my-school/
	Then I should get a 200 response
	And the linked schools links should be:
	  | Text Content                                               | Url             |
	  | Test School 1 was linked to Test School 2.                 | /school/100002/ |
	  | Test School 1 was linked to Test School 3 on 1 March 2020. | /school/100003/ |
		