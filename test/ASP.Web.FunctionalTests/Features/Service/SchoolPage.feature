Feature: School Page
 
@Javascript:disabled
Scenario: School page should contain seven app card container element
    Given page content "school-landing-page" exists:
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
    And establishment "123456" exists:
    """
    {
        "name": "Hollinswood Primary School",
        "urn": "123456"
    }
    """
    When I navigate to /school/123456/
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
Scenario: School page should be accessible when provided urn
  Given establishment "123456" exists:
		"""
		{
            "name": "Hollinswood Primary School",
            "urn": "123456"
        }
		"""
    When I navigate to /school/123456
	Then I should get a 200 response
	Then the page title should be "My school | Analyse school performance"
	Then the element "h1.govuk-heading-xl" should have the text content "My school" 
    Then the element "h1.govuk-heading-l" should have the outer HTML:
       """
       <h1 class="govuk-heading-l"> Hollinswood Primary School
            <span style="font-weight:400;">(URN: 123456)</span>
        </h1>
       """

@Javascript:disabled
Scenario: School page should throw page not found if Establishment is deleted
  Given establishment "112123" exists:
		"""
		{
            "name": "Thursby Primary School",
            "urn": "112123",
            "isDeleted": true
        }
		"""
    When I navigate to /school/112123
    Then I should get a 404 response
    Then the element "*[data-testid='error-display-message']" should have the text content "Error message: Establishment 112123 has been deleted."

@Javascript:disabled
Scenario: School page should contain a school details disclosure element
    Given establishment "123456" exists:
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
            "id": "123456",
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
            },
            "urn": "123456",
        }
		"""
    When I navigate to /school/123456
    Then the element ".govuk-details" should have the outer HTML: 
    """
     <details id="app-school-page-details" class="govuk-details">
        <summary class="govuk-details__summary">
            <span class="govuk-details__summary-text">
                <span id="app-school-page-details-state-closed" class="app-school-details-closed">Show</span>
                <span id="app-school-page-details-state-open" class="app-school-details-open">Hide</span>
                school details
            </span>
        </summary>
        <div class="govuk-details__text">
            <dl class="govuk-summary-list">
                <div class="govuk-summary-list__row">
                    <dt class="govuk-summary-list__key app-school-details-summary-heading">
                        Detail type
                    </dt>
                    <dd class="govuk-summary-list__value app-school-details-summary-heading">
                        School details
                    </dd>
                </div>
                <div class="govuk-summary-list__row">
                    <dt id="app-school-details-address-key" class="govuk-summary-list__key app-school-details-summary-key">
                        Address
                    </dt>
                    <dd id="app-school-details-address-value" class="govuk-summary-list__value">
                        Dale Acre Way, Telford, TF3 2EP
                    </dd>
                </div>
                <div class="govuk-summary-list__row">
                    <dt id="app-school-details-school-type-key" class="govuk-summary-list__key app-school-details-summary-key">
                        School type
                    </dt>
                    <dd id="app-school-details-school-type-value" class="govuk-summary-list__value">
                        Community school
                    </dd>
                </div>
                <div class="govuk-summary-list__row">
                    <dt class="govuk-summary-list__key app-school-details-summary-key">
                        Education phase
                    </dt>
                    <dd class="govuk-summary-list__value">
                        Primary
                    </dd>
                </div>
                <div class="govuk-summary-list__row">
                    <dt class="govuk-summary-list__key app-school-details-summary-key">
                        Gender of entry
                    </dt>
                    <dd class="govuk-summary-list__value">
                        Mixed
                    </dd>
                </div>
                <div class="govuk-summary-list__row">
                    <dt class="govuk-summary-list__key app-school-details-summary-key">
                        Ofsted rating
                    </dt>
                    <dd class="govuk-summary-list__value">
                            <span class="app-ofsted-colour-rating app-ofsted-rating-2">
                                2
                            </span>
                            <span>
                                Good <wbr /> |
                                <a href="https://reports.ofsted.gov.uk/inspection-reports/find-inspection-report/provider/ELS/123456" target="_blank" class="govuk-link">
                                    Ofsted report
                                </a>
                            </span>
                                <span class="app-ofsted-last-inspection">Inspected 22 January 2020</span>
                    </dd>
                </div>
                <div class="govuk-summary-list__row">
                    <dt class="govuk-summary-list__key app-school-details-summary-key">
                        Local authority
                    </dt>
                    <dd class="govuk-summary-list__value">
                        Telford and Wrekin
                    </dd>
                </div>
                <div class="govuk-summary-list__row">
                    <dt class="govuk-summary-list__key app-school-details-summary-key">
                        Headteacher / Principal
                    </dt>
                    <dd class="govuk-summary-list__value">
                        Mrs Kath Osborne
                    </dd>
                </div>
                <div class="govuk-summary-list__row">
                    <dt class="govuk-summary-list__key app-school-details-summary-key">
                        Age range
                    </dt>
                    <dd class="govuk-summary-list__value">
                        3 to 11
                    </dd>
                </div>
                <div class="govuk-summary-list__row">
                    <dt class="govuk-summary-list__key app-school-details-summary-key">
                        Religious character
                    </dt>
                    <dd class="govuk-summary-list__value">
                        Does not apply
                    </dd>
                </div>
                <div class="govuk-summary-list__row">
                    <dt class="govuk-summary-list__key app-school-details-summary-key">
                        Admissions policy
                    </dt>
                    <dd class="govuk-summary-list__value">
                        Not applicable
                    </dd>
                </div>
                <div class="govuk-summary-list__row">
                    <dt class="govuk-summary-list__key app-school-details-summary-key">
                        SEN unit or resourced provision
                    </dt>
                    <dd class="govuk-summary-list__value">
                        Not recorded
                    </dd>
                </div>
                <div class="govuk-summary-list__row">
                    <dt class="govuk-summary-list__key app-school-details-summary-key">
                        Number of pupils
                    </dt>
                    <dd class="govuk-summary-list__value">
                        404
                    </dd>
                </div>
            </dl>
        </div>
    </details>
    """

@Javascript:disabled
Scenario: Details disclosure element text should read 'Show school details' when closed
    Given establishment "123456" exists:
	"""
	{
        "name": "Hollinswood Primary School",
        "urn": "123456",
    }
	"""
    When I navigate to /school/123456
    Then the element "#app-school-page-details-state-closed" should have the text content "Show"
  
@Javascript:disabled
Scenario: School page should display page not found page if School URN is invalid
    Given establishment "112123" exists:
    """
    {
        "name": "Thursby Primary School",
        "urn": "112123",
        "isDeleted": false
    }
    """
    When I navigate to /school/112
    Then I should get a 404 response
    And the page title should be "Page not found – ASP – GOV.UK | Analyse school performance"
    And the element "h1.govuk-heading-l" should have the text content "Page not found"
    And the element "*[data-testid='address-typing-instruction']" should have the text content "If you typed the web address, check it is correct."
    And the element "*[data-testid='address-pasting-instruction']" should have the text content "If you pasted the web address, check you copied the entire address."
    And the element "*[data-testid='error-display-message']" should exist
    And the element "*[data-testid='error-display-message']" should have the text content "Error message: 404 Error: Could not find object with id "112" and partition key "112" in container "establishments"."