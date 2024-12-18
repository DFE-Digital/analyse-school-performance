Feature: Blob storage demo tests

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
	And the current time is 2024/10/14 12:34:56
	When I send a GET request to /api/BlobStorageDemoFileDownload?container=ASP&filepath=test.csv
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
	And the current time is 2024/10/14 12:34:56
	When I send a GET request to /api/BlobStorageDemoZipFileDownload?container=ASP&filepath=test.csv
	Then I should get a 200 response
	And the response should be a ZIP file download with filename 20241014_123456_asp_blob_storage_demo.zip
	And the ZIP file download should contain 1 file
	And the ZIP file download should contain the file test.csv with contents:
	"""
	Column1,Column2,Column3
	1,2,3
	A,B,C
	"""