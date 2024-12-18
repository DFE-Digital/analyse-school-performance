Feature: School Useful links page

@Javascript:disabled
Scenario Outline: Other reports page should be accessible when valid urn is provided (My school page)
	Given Establishment "136028" exists:
	"""
	{
		"name": "Dagenham Park CofE School"
	}
	"""
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
	And I am a School Named user for Establishment "136028"
	When I navigate to /my-school/useful-links/
	Then I should get a 200 response
	And the page title should be "Useful links"
	And the sub-navigation should be:
		| text          | href                      | current |
		| Download data | /my-school/download-data/ |         |
		| Other reports | /my-school/other-reports/ |         |
		| Useful links  | /my-school/useful-links/  | true    |

@Javascript:disabled
Scenario Outline: Other reports page should be accessible when valid urn is provided (Generic school page)
	Given Establishment "136028" exists:
	"""
	{
		"name": "Dagenham Park CofE School"
	}
	"""
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
		| text          | href                          | current |
		| Download data | /school/136028/download-data/ |         |
		| Other reports | /school/136028/other-reports/ |         |
		| Useful links  | /school/136028/useful-links/  | true    |

@Javascript:disabled
Scenario Outline: Other reports page should be accessible when valid urn is provided (My schools > School page)
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
	And I am a LA Named user for Local Authority "301"
	When I navigate to /my-schools/136028/useful-links/
	Then I should get a 200 response
	And the page title should be "Useful links"
	And the sub-navigation should be:
		| text          | href                      | current |
		| Download data | /my-schools/136028/download-data/ |         |
		| Other reports | /my-schools/136028/other-reports/ |         |
		| Useful links  | /my-schools/136028/useful-links/  | true    |
