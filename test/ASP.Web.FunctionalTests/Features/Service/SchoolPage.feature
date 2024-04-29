Feature: School Page

@Javascript:disabled
Scenario: School page should contain a school details disclosure element
  Given establishment details "123456" exists:
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
                                <span class="app-ofsted-last-inspection">Inspected 01 January 0001</span>
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
  When I navigate to /school/123456
  Then the element "#app-school-page-details-state-closed" should have the text content "Show"

