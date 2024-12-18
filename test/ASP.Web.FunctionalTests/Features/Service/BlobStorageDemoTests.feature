Feature: Blob storage demo tests

Background: 
	Given I am a Super Admin user

@Javascript:disabled
Scenario: If file is created then it should exist
	Given blob storage file School/123456/2024/test.json exists in ASP container:
	"""
	Hello this is the file
	"""
	Then blob storage file School/123456/2024/test.json should exist in ASP container:
	"""
	Hello this is the file
	"""

@Javascript:disabled
Scenario: Should be able to download a file from blob storage
	Given blob storage file test.csv exists in ASP container:
	"""
	Column1,Column2,Column3
	1,2,3
	A,B,C
	"""
	And Content Template "blob-storage-demo" exists:
	"""
	{
		"Views": [
			{
				"ViewId": "Paragraph",
				"ViewContent": {
					"Id": "download-link",
					"Text": "[click to download](file-download/?container=ASP&filepath=test.csv)"
				}
			}
		]
	}
	"""
	And the current time is 2024/10/14 12:34:56
	When I navigate to /blob-storage-demo
	And I click the download link "#download-link a"
	Then I should get a 200 response
	And the response should be a CSV file download with filename test.csv
	And the file download should have the contents:
	"""
	Column1,Column2,Column3
	1,2,3
	A,B,C
	"""

@Javascript:disabled
Scenario: Should be able to download a file from blob storage via the BlobStorageDemoFileDownload API endpoint
	Given blob storage file test.csv exists in ASP container:
	"""
	Column1,Column2,Column3
	1,2,3
	A,B,C
	"""
	And Content Template "blob-storage-demo" exists:
	"""
	{
		"Views": [
			{
				"ViewId": "Paragraph",
				"ViewContent": {
					"Id": "download-link",
					"Text": "[click to download](api-file-download/?container=ASP&filepath=test.csv)"
				}
			}
		]
	}
	"""
	And the current time is 2024/10/14 12:34:56
	When I navigate to /blob-storage-demo
	And I click the download link "#download-link a"
	Then I should get a 200 response
	And the response should be a CSV file download with filename test.csv
	And the file download should have the contents:
	"""
	Column1,Column2,Column3
	1,2,3
	A,B,C
	"""

@Javascript:disabled
Scenario: Should be able to download a file from blob storage and then zip it
	Given blob storage file test.csv exists in ASP container:
	"""
	Column1,Column2,Column3
	1,2,3
	A,B,C
	"""
	And Content Template "blob-storage-demo" exists:
	"""
	{
		"Views": [
			{
				"ViewId": "Paragraph",
				"ViewContent": {
					"Id": "download-link",
					"Text": "[click to download](zip-file-download/?container=ASP&filepath=test.csv)"
				}
			}
		]
	}
	"""
	And the current time is 2024/10/14 12:34:56
	When I navigate to /blob-storage-demo
	And I click the download link "#download-link a"
	Then I should get a 200 response
	And the response should be a ZIP file download with filename 20241014_123456_asp_blob_storage_demo.zip
	And the ZIP file download should contain 1 file
	And the ZIP file download should contain the file test.csv with contents:
	"""
	Column1,Column2,Column3
	1,2,3
	A,B,C
	"""

@Javascript:disabled
Scenario: Should be able to download a file from blob storage and then zip it via the BlobStorageDemoZipFileDownload API endpoint
	Given blob storage file test.csv exists in ASP container:
	"""
	Column1,Column2,Column3
	1,2,3
	A,B,C
	"""
	And Content Template "blob-storage-demo" exists:
	"""
	{
		"Views": [
			{
				"ViewId": "Paragraph",
				"ViewContent": {
					"Id": "download-link",
					"Text": "[click to download](api-zip-file-download/?container=ASP&filepath=test.csv)"
				}
			}
		]
	}
	"""
	And the current time is 2024/10/14 12:34:56
	When I navigate to /blob-storage-demo
	And I click the download link "#download-link a"
	Then I should get a 200 response
	And the response should be a ZIP file download with filename 20241014_123456_asp_blob_storage_demo.zip
	And the ZIP file download should contain 1 file
	And the ZIP file download should contain the file test.csv with contents:
	"""
	Column1,Column2,Column3
	1,2,3
	A,B,C
	"""

@Javascript:disabled
Scenario: DownloadAsZipFile API endpoint should return a zip file containing 1 file when 1 downloadId is provided
	# When DownloadAsZipFile endpoint is implemented properly, set up files in blob storage, e.g.:
	# Given blob storage file test.csv exists in ASP container:
	# """
	# Column1,Column2,Column3
	# 1,2,3
	# A,B,C
	# """
	Given Content Template "blob-storage-demo" exists:
	"""
	{
		"Views": [
			{
				"ViewId": "Paragraph",
				"ViewContent": {
					"Id": "download-link",
					"Text": "[click to download](api-download-as-zip-file/?downloadIds=test)"
				}
			}
		]
	}
	"""
	And the current time is 2024/10/14 12:34:56
	When I navigate to /blob-storage-demo
	And I click the download link "#download-link a"
	Then I should get a 200 response
	And the response should be a ZIP file download with filename 20241014_123456_asp_download.zip
	And the ZIP file download should contain 1 file
	And the ZIP file download should contain the file test.csv with contents:
	"""
	Id,Name,Value
	test,Test Name,123
	test,Another Name,456
	"""

@Javascript:disabled
Scenario: DownloadAsZipFile API endpoint should return a zip file containing 2 files when 2 downloadIds are provided
	# When DownloadAsZipFile endpoint is implemented properly, set up files in blob storage, e.g.:
	# Given blob storage file test1.csv exists in ASP container:
	# """
	# Column1,Column2,Column3
	# 1,2,3
	# A,B,C
	# """
	# And blob storage file test2.csv exists in ASP container:
	# """
	# Column1,Column2,Column3
	# 4,5,6
	# D,E,F
	# """
	Given Content Template "blob-storage-demo" exists:
	"""
	{
		"Views": [
			{
				"ViewId": "Paragraph",
				"ViewContent": {
					"Id": "download-link",
					"Text": "[click to download](api-download-as-zip-file/?downloadIds=test1&downloadIds=test2)"
				}
			}
		]
	}
	"""
	And the current time is 2024/10/14 12:34:56
	When I navigate to /blob-storage-demo
	And I click the download link "#download-link a"
	Then I should get a 200 response
	And the response should be a ZIP file download with filename 20241014_123456_asp_download.zip
	And the ZIP file download should contain 2 files
	And the ZIP file download should contain the file test1.csv with contents:
	"""
	Id,Name,Value
	test1,Test Name,123
	test1,Another Name,456
	"""
	And the ZIP file download should contain the file test2.csv with contents:
	"""
	Id,Name,Value
	test2,Test Name,123
	test2,Another Name,456
	"""