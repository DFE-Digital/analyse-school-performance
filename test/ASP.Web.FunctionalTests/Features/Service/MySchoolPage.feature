Feature: My school page

@Javascript:disabled
Scenario: A non-School user should not be able to access the 'My school' page. Instead, they should see a 403 Access not allowed page.
    Given I am a MAT Named user for Multi-Academy Trust "1234"
    When I navigate to /my-school/
    Then I should get a 403 response
    And the page title should be "Access not allowed | Analyse school performance"
    And the element "h1.govuk-heading-l" should have the text content "Access not allowed"

@Javascript:disabled
Scenario: A user with access to all schools should not be able to access the 'My school' page. Instead, they should see a 403 Access not allowed page.
    Given I am a DfE Named user
    When I navigate to /my-school/
    Then I should get a 403 response
    And the page title should be "Access not allowed | Analyse school performance"
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
    And the page title should be "Sorry, there is a problem with the service | Analyse school performance"
    And the element "h1.govuk-heading-l" should have the text content "Sorry, there is a problem with the service"
    Then the element "*[data-testid='error-display-message']" should have the text content "Error message: API error: Establishment with URN "111111" is not currently visible."

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
    And the page title should be "Sorry, there is a problem with the service | Analyse school performance"
    And the element "h1.govuk-heading-l" should have the text content "Sorry, there is a problem with the service"
    Then the element "*[data-testid='error-display-message']" should have the text content "Error message: API error: Establishment with URN "111111" has been deleted."

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
    And the page title should be "Sorry, there is a problem with the service | Analyse school performance"
    And the element "h1.govuk-heading-l" should have the text content "Sorry, there is a problem with the service"
    And the element "*[data-testid='error-display-message']" should have the text content "Error message: API error: Could not find Establishment with URN "222222"."

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
	Then the page title should be "My school | Analyse school performance"
	Then the element "h1.govuk-heading-xl" should have the text content "My school" 
    Then the element "h1.govuk-heading-l" should have the outer HTML:
       """
       <h1 data-testid="school-page-school-name" class="govuk-heading-l"> Hollinswood Primary School
            <span style="font-weight:400;">(URN: 123456)</span>
        </h1>
       """

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
            "localAuthority": null,
            "name": "Hollinswood Primary School",
            "noOfPupils": null,
            "ofstedLastInspectionDate": null,
            "ofstedRating": null,
            "religiousDenomination": null,
            "resourcedProvisionType": null
        }
		"""
    And I am a School Named user for Establishment "123456"
    When I navigate to /my-school/
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
    And the element "*[data-testid='school-details-la-value']" should have the text content ""
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
    And I am a School Named user for Establishment "123456"
    When I navigate to /my-school/
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
    And the element "*[data-testid='school-details-la-value']" should have the text content ""
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
        "name": "Hollinswood Primary School"
    }
	"""
    And I am a School Named user for Establishment "123456"
    When I navigate to /my-school/
    Then the element "*[data-testid='school-page-details-state-closed']" should have the text content "Show"