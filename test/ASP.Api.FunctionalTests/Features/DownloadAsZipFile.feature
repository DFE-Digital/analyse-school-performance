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

Scenario: Should return a 200 response when valid fileType and downloadIds are provided
	When I send a GET request to <url>
	Then I should get a 200 response
	And the ZIP file should contain <csvCount> CSV files with at least 3 rows each
Examples:
	| url                                                                  | csvCount |
	| /api/DownloadAsZipFile?fileType=csv&downloadIds=abcd                 | 1        |
	| /api/DownloadAsZipFile?fileType=csv&downloadIds=abcd&downloadIds=xyz | 2        |

Scenario: Should return a 200 response and have the correct headers
	When I send a GET request to /api/DownloadAsZipFile?fileType=csv&downloadIds=abcd
	Then I should get a 200 response
	And the response should include the header "Content-Type: application/zip"
	And the ZIP file name should follow the expected format