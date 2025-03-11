Feature: School Useful links page

@Javascript:disabled
Scenario Outline: School Useful links page should be accessible when valid urn is provided (My school page)
	Given establishment Dagenham Park CofE School (136028) exists
	And Content Template "school-useful-links" exists:
	"""
	{
		"Views": [
			{
				"ViewId": "Link",
				"ViewContent": {
					"Id": "department-for-education", 
					"Size": "m",
					"Url": "https://www.gov.uk/government/organisations/department-for-education",
					"Text": "department-for-education"
				}
			 }
		 ]
	}
	"""
	And I am a School Named user for Establishment 136028
	When I navigate to /my-school/useful-links/
	Then I should get a 200 response
	And the page title should be "Useful links"
	And the sub-navigation should be:
		| Link Text     | Url                       | Current Page |
		| Download data | /my-school/download-data/ |              |
		| Other reports | /my-school/other-reports/ |              |
		| Useful links  | /my-school/useful-links/  | true         |

@Javascript:disabled
Scenario Outline: School Useful links page should be accessible when valid urn is provided (Generic school page)
	Given establishment Dagenham Park CofE School (136028) exists
	And Content Template "school-useful-links" exists:
	"""
	{
		"Views": [
			{
				"ViewId": "Link",
				"ViewContent": {
					"Id": "department-for-education", 
					"Size": "m",
					"Url": "https://www.gov.uk/government/organisations/department-for-education",
					"Text": "department-for-education"
				}
			 }
		 ]
	}
	"""
	And I am a DfE Named user
	When I navigate to /school/136028/useful-links/
	Then I should get a 200 response
	And the page title should be "Useful links"
	And the sub-navigation should be:
		| Link Text     | Url                           | Current Page |
		| Download data | /school/136028/download-data/ |              |
		| Other reports | /school/136028/other-reports/ |              |
		| Useful links  | /school/136028/useful-links/  | true         |

@Javascript:disabled
Scenario Outline: School Useful links page should be accessible when valid urn is provided (My schools > School page)
	Given establishment Dagenham Park CofE School (136028) exists in local authority 301
	And local authority Test LA (301) exists
	And Content Template "school-useful-links" exists:
	"""
	{
		"Views": [
			{
				"ViewId": "Link",
				"ViewContent": {
					"Id": "department-for-education", 
					"Size": "m",
					"Url": "https://www.gov.uk/government/organisations/department-for-education",
					"Text": "department-for-education"
				}
			 }
		 ]
	}
	"""
	And I am a LA Named user for Local Authority 301
	When I navigate to /my-schools/136028/useful-links/
	Then I should get a 200 response
	And the page title should be "Useful links"
	And the sub-navigation should be:
		| Link Text     | Url                               | Current Page |
		| Download data | /my-schools/136028/download-data/ |              |
		| Other reports | /my-schools/136028/other-reports/ |              |
		| Useful links  | /my-schools/136028/useful-links/  | true         |

@Javascript:disabled
Scenario Outline: LA user should not be able to access the School Useful links page of a school with a valid URN outside their Local Authority (My schools > School page)
	Given establishment Dagenham Park CofE School (136028) exists in local authority 302
	And local authority Test LA (301) exists
	And Content Template "school-useful-links" exists:
	"""
	{
		"Views": [
			{
				"ViewId": "Link",
				"ViewContent": {
					"Id": "department-for-education", 
					"Size": "m",
					"Url": "https://www.gov.uk/government/organisations/department-for-education",
					"Text": "department-for-education"
				}
			 }
		 ]
	}
	"""
	And I am a LA Named user for Local Authority 301
	When I navigate to /my-schools/136028/useful-links/
	Then I should get a 403 response
	And the page title should be "Access not allowed"
	And the element "h1.govuk-heading-l" should have the text content "Access not allowed"