Feature: DownloadAsZipFile

Scenario: Should not accept POST method
	When I send a POST request to /api/DownloadAsZipFile
	Then I should get a 405 response
	And the response should be the message "Method not allowed: The HTTP method POST is not allowed."
	And the response should include the header "Allow: GET"

Scenario: Should return BadRequest (400) response if parameters are missing
	When I send a GET request to <url>
	Then I should get a 400 response
	And the response should be the message "Bad request: The parameter "<missingParameter>" is missing."
Examples:
	| url                                      | missingParameter |
	| /api/DownloadAsZipFile?fileType=csv      | downloadIds      |
	| /api/DownloadAsZipFile?&downloadIds=test | fileType         |

Scenario: Should return BadRequest (400) response if file type is invalid	
	When I send a GET request to /api/DownloadAsZipFile?fileType=<fileType>&downloadId=abc
	Then I should get a 400 response
	And the response should be the message "Bad request: "<fileType>" is not a valid "fileType"."
Examples:
	| fileType |
	| xlsx     |
	| png      |
	| docx     |

Scenario: Should return BadRequest (400) response if parameters are empty
	When I send a GET request to <url>
	Then I should get a 400 response
	And the response should be the message "Bad request: The parameter "<missingParameterValue>" should not be empty."
Examples:
	| url                                              | missingParameterValue |
	| /api/DownloadAsZipFile?fileType=csv&downloadIds= | downloadIds           |
	| /api/DownloadAsZipFile?fileType=&downloadIds=abc | fileType              |

Scenario: Should return a zip file containing 1 file when 1 downloadId is provided
    # When DownloadAsZip endpoint is implemented properly, set up files in blob storage, e.g.:
    # Given blob storage file test.csv exists in ASP container:
    # """
    # Column1,Column2,Column3
    # 1,2,3
    # A,B,C
    # """
    Given the current time is 2024/10/14 12:34:56
	When I send a GET request to /api/DownloadAsZipFile?fileType=csv&downloadIds=abcd
	Then I should get a 200 response
	And the response should be a ZIP file download with filename 20241014_123456_asp_download.zip
    And the ZIP file download should contain 1 file
    And the ZIP file download should contain the file abcd.csv with contents:
    """
    Id,Name,Value
    abcd,Test Name,123
    abcd,Another Name,456
    """

Scenario: Should return a zip file containing 2 files when 2 downloadIds are provided
    # When DownloadAsZip endpoint is implemented properly, set up files in blob storage, e.g.:
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
    Given the current time is 2024/10/14 12:34:56
	When I send a GET request to /api/DownloadAsZipFile?fileType=csv&downloadIds=abcd&downloadIds=xyz
	Then I should get a 200 response
	And the response should be a ZIP file download with filename 20241014_123456_asp_download.zip
    And the ZIP file download should contain 2 files
    And the ZIP file download should contain the file abcd.csv with contents:
    """
    Id,Name,Value
    abcd,Test Name,123
    abcd,Another Name,456
    """
    And the ZIP file download should contain the file xyz.csv with contents:
    """
    Id,Name,Value
    xyz,Test Name,123
    xyz,Another Name,456
    """