Feature: School Other reports page

@Javascript:disabled
Scenario Outline: Other reports page should be accessible when valid urn is provided (My school page)
	Given establishment Dagenham Park CofE School (136028) exists
	And I am a School Named user for Establishment 136028
	When I navigate to /my-school/other-reports/
	Then I should get a 200 response
	And the page title should be "Other reports"
	And the sub-navigation should be:
		| Link Text     | Url                       | Current Page |
		| Download data | /my-school/download-data/ |              |
		| Other reports | /my-school/other-reports/ | true         |
		| Useful links  | /my-school/useful-links/  |              |

@Javascript:disabled
Scenario Outline: Other reports page should be accessible when valid urn is provided (Generic school page)
	Given establishment Dagenham Park CofE School (136028) exists
	And I am a DfE Named user
	When I navigate to /school/136028/other-reports/
	Then I should get a 200 response
	And the page title should be "Other reports"
	And the sub-navigation should be:
		| Link Text     | Url                           | Current Page |
		| Download data | /school/136028/download-data/ |              |
		| Other reports | /school/136028/other-reports/ | true         |
		| Useful links  | /school/136028/useful-links/  |              |

@Javascript:disabled
Scenario Outline: Other reports page should be accessible when valid urn is provided (My schools > School page)
	Given establishment Dagenham Park CofE School (136028) exists in local authority 301
	And local authority Test LA (301) exists
	And I am a LA Named user for Local Authority 301
	When I navigate to /my-schools/136028/other-reports/
	Then I should get a 200 response
	And the page title should be "Other reports"
	And the sub-navigation should be:
		| Link Text     | Url                               | Current Page |
		| Download data | /my-schools/136028/download-data/ |              |
		| Other reports | /my-schools/136028/other-reports/ | true         |
		| Useful links  | /my-schools/136028/useful-links/  |              |
  
@Javascript:disabled
Scenario Outline: LA user should not be able to access the Other Reports page of a school with a valid URN outside their Local Authority (My schools > School page)
	Given establishment Dagenham Park CofE School (136028) exists in local authority 302
	And local authority Test LA (301) exists
	And I am a LA Named user for Local Authority 301
	When I navigate to /my-schools/136028/other-reports/
	Then I should get a 403 response
	And the page title should be "Access not allowed"
	And the element "h1.govuk-heading-l" should have the text content "Access not allowed"
	
@Javascript:disabled
Scenario: Other reports page should show the accordion component when javascript disabled
	Given establishment Dagenham Park CofE School (136028) exists in local authority 301
	And I am a <User>
	When I navigate to <Path>/other-reports
	Then I should get a 200 response
	And the element "[data-testid='accordion-default-heading-1']" should have the text content "Ofsted inspection data summary reports"
	And the element "[data-testid='accordion-default-heading-2']" should have the text content "School performance summary"
	And the element "[data-testid='accordion-default-heading-3']" should have the text content "Absence and exclusions"
	And the element "[data-testid='accordion-default-heading-4']" should have the text content "School characteristics"
Examples: 
	| User                                         | Path           |
	| School Named user for Establishment 136028   | /my-school     |
	| School Unnamed user for Establishment 136028 | /my-school     |
	| DfE Named user                               | /school/136028 |
	| DfE Unnamed user                             | /school/136028 |

@Javascript:enabled
Scenario: Other reports page should show the accordion component when javascript enabled
	Given Content Template "school-other-reports-ofsted" exists:
	"""
	{
		"Views": [
			{
				"ViewId": "Paragraph",
				"ViewContent": {
					"Id": "ofsted-visit-service",
					"Size": "m",
					"Text": "For the latest IDSR, [visit the Ofsted IDSR service](https://idsr.ofsted.gov.uk/idsr/{URN}/){target=\"_blank\"}"
				}
			}
		]
	}
	"""
	And establishment Dagenham Park CofE School (136028) exists
	And I am a <User>
	When I navigate to <Path>/other-reports
	Then I should get a 200 response
	And the element "[data-testid='accordion-default-heading-1']" should have the text content "Ofsted inspection data summary reports, Show"
	And the element "[data-testid='accordion-default-heading-2']" should have the text content "School performance summary, Show"
	And the element "[data-testid='accordion-default-heading-3']" should have the text content "Absence and exclusions, Show"
	And the element "[data-testid='accordion-default-heading-4']" should have the text content "School characteristics, Show"
	And the element "#ofsted-visit-service a" should have the href "https://idsr.ofsted.gov.uk/idsr/136028/"
Examples: 
	| User                                         | Path           |
	| School Named user for Establishment 136028   | /my-school     |
	| School Unnamed user for Establishment 136028 | /my-school     |
	| DfE Named user                               | /school/136028 |
	| DfE Unnamed user                             | /school/136028 |