Feature: DownloadsGetPackage


Scenario: Should not accept POST method
	When I send a POST request to /api/downloads/package
	Then I should get a 405 response
	And the response should be the message "Method not allowed: The HTTP method POST is not allowed."
	And the response should include the header "Allow: GET"

Scenario: Should return BadRequest (400) response if parameters are missing
	When I send a GET request to <url>
	Then I should get a 400 response
	And the response should be the message "Bad request: The query parameter "<missingParameter>" is missing."
Examples:
	| url                                                                  | missingParameter |
	| /api/downloads/package?scope=LA&scopeId=123456&fileType=csv          | downloadIds      |
	| /api/downloads/package?scope=LA&scopeId=123456&downloadIds=test      | fileType         |
	| /api/downloads/package?&scopeId=123456&fileType=csv&downloadIds=test | scope            |

Scenario: Should return BadRequest (400) response if scopeId parameter is missing
	When I send a GET request to /api/downloads/package?scope=<scope>&fileType=CSV&downloadIds=test-999-2024
	Then I should get a 400 response
	And the response should be the message "Bad request: The query parameter "scopeId" is missing."
Examples:
	| scope  |
	| School |
	| school |
	| LA     |
	| la     |

Scenario: Should return BadRequest (400) response if scopeId parameter is empty
	When I send a GET request to /api/downloads/package?scope=<scope>&scopeId=&fileType=CSV&downloadIds=test-999-2024
	Then I should get a 400 response
	And the response should be the message "Bad request: The query parameter "scopeId" should not be empty."
Examples:
	| scope  |
	| School |
	| school |
	| LA     |
	| la     |

Scenario: Should return BadRequest (400) response if parameters are empty
    When I send a GET request to <url>
    Then I should get a 400 response
    And the response should be the message "Bad request: The query parameter "<emptyParameter>" should not be empty."
Examples:
	| url                                                                        | emptyParameter |
	| /api/downloads/package?scope=LA&scopeId=123456&fileType=csv&downloadIds=   | downloadIds    |
	| /api/downloads/package?scope=LA&scopeId=123456&fileType=&downloadIds=test  | fileType       |
	| /api/downloads/package?scope=&scopeId=123456&fileType=csv&downloadIds=test | scope          |

Scenario: Should return BadRequest (400) response if fileType parameter is not supported
	When I send a GET request to /api/downloads/package?scope=LA&scopeId=123456&fileType=<fileType>
	Then I should get a 400 response
	And the response should be the message "Bad request: "<fileType>" is not a valid fileType."
Examples:
	| fileType |
	| png      |
	| docx     |

Scenario: Should return BadRequest (400) response if scopeId parameter is invalid URN
	When I send a GET request to /api/downloads/package?scope=School&scopeId=xyz&fileType=csv&downloadIds=test-999-2024
	Then I should get a 400 response
	And the response should be the message "Bad request: "xyz" is not a valid school URN."

Scenario: Should return BadRequest (400) response if scopeId parameter is invalid LA code
	When I send a GET request to /api/downloads/package?scope=LA&scopeId=xyz&fileType=csv&downloadIds=test-999-2024
	Then I should get a 400 response
	And the response should be the message "Bad request: "xyz" is not a valid LA code."

Scenario: Should return BadRequest (400) response if scopeId parameter is non-existent school URN
    Given blob storage file downloads-config.json exists in config container:
    """
    [
        {
            "id": "test",
            "filePathPattern": "test",
            "source" : "KTS"
        }
    ]
    """
    And Establishment "111111" exists:
    """
    {
        "name": "Test School"
    }
    """
    When I send a GET request to /api/downloads/package?scope=School&scopeId=222222&fileType=CSV&downloadIds=test-999-2024
    Then I should get a 400 response 
    And the response should be the message "Bad request: School with URN "222222" does not exist."

Scenario: Should return BadRequest (400) response if scopeId parameter is non-existent LA code
    Given Local Authority "111" exists:
    """
    {
        "name": "Test LA"
    }
    """
    When I send a GET request to /api/downloads/package?scope=LA&scopeId=222&fileType=CSV&downloadIds=test-999-2024
    Then I should get a 400 response 
    And the response should be the message "Bad request: Local Authority with Code "222" does not exist."

Scenario Outline: CSV, XLSX and TSV fileTypes should be supported (case insensitive)
    Given blob storage file downloads-config.json exists in config container:
    """
    [
        {
            "id": "test",
            "filePathPattern": "test.csv",
            "source" : "KTS"
        }
    ]
    """
    And Establishment "111111" exists:
    """
    {
        "name": "Test School"
    }
    """
    And blob storage file test.csv exists in downloads-kts container:
    """
    test
    """
    When I send a GET request to /api/downloads/package?scope=School&scopeId=111111&fileType=<fileType>&downloadIds=test-111111-2024
    Then I should get a <statusCode> response
Examples:
    | fileType | statusCode |
    | csv      | 200        |
    | CSV      | 200        |
    | tsv      | 200        |
    | TSV      | 200        |
    | xlsx     | 200        |
    | XLSX     | 200        |
    | ABC      | 400        |
    | XYZ      | 400        |

Scenario Outline: Should return BadRequest (400) response if downloadId is not in the correct format (school scope)
    Given blob storage file downloads-config.json exists in config container:
    """
    [
        { "id": "abc" },
        { "id": "def" }
    ]
    """
    And Establishment "111111" exists:
    """
    {
        "name": "Test School"
    }
    """ 
    When I send a GET request to /api/downloads/package?scope=School&scopeId=111111&fileType=CSV&downloadIds=<invalidDownloadId>
    Then I should get a 400 response
    And the response should be the message "Bad request: Download ID: <invalidDownloadId> is not in the format "{download-config.id}-{identifier}-{year}[-{version}]".""
Examples:
    | invalidDownloadId |
    | 111111-2024-final |
    | test-111111-final |
    | test-111111-2024- |
    | 111111-2024-final |
    | 11111-2024-final  |
    | 111111-202-final  |
    | abc-2024-final    |
    | 111111-abc-final  |
    | xyz               |
    | ---               |

Scenario Outline: Should return BadRequest (400) response if downloadId is not in the correct format (LA scope)
    Given blob storage file downloads-config.json exists in config container:
    """
    [
        { "id": "abc" },
        { "id": "def" }
    ]
    """
    And Local Authority "111" exists:
    """
    {
        "name": "Test LA"
    }
    """ 
    When I send a GET request to /api/downloads/package?scope=LA&scopeId=111&fileType=CSV&downloadIds=<invalidDownloadId>
    Then I should get a 400 response
    And the response should be the message "Bad request: Download ID: <invalidDownloadId> is not in the format "{download-config.id}-{identifier}-{year}[-{version}]".""
Examples:
    | invalidDownloadId |
    | 111-2024-final    |
    | test-111-final    |
    | test-111-2024-    |
    | 111-2024-final    |
    | abc-2024-final    |
    | 111-abc-final     |
    | xyz               |
    | ---               |

Scenario: Should return ServerError (500) response if downloads-config.json file does not exist
    Given no files exist in blob storage
    And Establishment "123456" exists:
    """
    {
        "name": "Test School 1"
    }
    """ 
    When I send a GET request to /api/downloads/package?scope=School&scopeId=123456&fileType=CSV&downloadIds=test
    Then I should get a 500 response
    And the response should be the message "{"ErrorType":"Unexpected","StackTrace":null,"Message":"Blob storage file \"downloads-config.json\" does not exist in container \"config\".","MessagePrefix":"Unexpected: "}"

Scenario: Should return NotAllowed (403) response if downloadId identifier part is school URN that doesn't exist (School scope)
    Given blob storage file downloads-config.json exists in config container:
    """
    [
        { "id": "test" }
    ]
    """
    Given Establishment "111111" exists:
    """
    {
        "name": "Test School"
    }
    """    
    When I send a GET request to /api/downloads/package?scope=School&scopeId=111111&fileType=CSV&downloadIds=test-111111-2024&downloadIds=test-222222-2024
    Then I should get a 403 response
    And the response should be the message "Not allowed: Identifier "222222" is not accessible within the given scope."

Scenario: Should return NotAllowed (403) response if downloadId identifier part is different school URN to scopeId (School scope)
    Given blob storage file downloads-config.json exists in config container:
    """
    [
        { "id": "test" }
    ]
    """
    And Establishment "111111" exists:
    """
    {
        "name": "Test School 1"
    }
    """    
    And Establishment "222222" exists:
    """
    {
        "name": "Test School 2"
    }
    """    
    When I send a GET request to /api/downloads/package?scope=School&scopeId=111111&fileType=CSV&downloadIds=test-222222-2024
    Then I should get a 403 response
    And the response should be the message "Not allowed: Identifier "222222" is not accessible within the given scope."

Scenario: Should return NotAllowed (403) response if downloadId identifier part is LA code that doesn't exist (LA scope)
    Given blob storage file downloads-config.json exists in config container:
    """
    [
        { "id": "test" }
    ]
    """
    And Local Authority "111" exists:
    """
    {
        "name": "Test LA"
    }
    """    
    When I send a GET request to /api/downloads/package?scope=LA&scopeId=111&fileType=CSV&downloadIds=test-111-2024&downloadIds=test-222-2024
    Then I should get a 403 response
    And the response should be the message "Not allowed: Identifier "222" is not accessible within the given scope."

Scenario: Should return NotAllowed (403) response if downloadId identifier part is different LA code to scopeId (LA scope)
    Given blob storage file downloads-config.json exists in config container:
    """
    [
        { "id": "test" }
    ]
    """
    And Local Authority "111" exists:
    """
    {
        "name": "Test LA 1"
    }
    """    
    And Local Authority "222" exists:
    """
    {
        "name": "Test LA 2"
    }
    """    
    When I send a GET request to /api/downloads/package?scope=LA&scopeId=111&fileType=CSV&downloadIds=test-111-2024&downloadIds=test-222-2024
    Then I should get a 403 response
    And the response should be the message "Not allowed: Identifier "222" is not accessible within the given scope."

Scenario: Should return NotAllowed (403) response if downloadId identifier part is school URN that doesn't exist (LA scope)
    Given blob storage file downloads-config.json exists in config container:
    """
    [
        { "id": "test" }
    ]
    """
    And Local Authority "111" exists:
    """
    {
        "name": "Test LA 1"
    }
    """    
    And Establishment "111111" exists:
    """
    {
        "name": "Test School 1",
        "localAuthority": {
            "code": "111"
        }
    }
    """
    When I send a GET request to /api/downloads/package?scope=LA&scopeId=111&fileType=CSV&downloadIds=test-111111-2024&downloadIds=test-222222-2024
    Then I should get a 403 response
    And the response should be the message "Not allowed: Identifier "222222" is not accessible within the given scope."

Scenario: Should return NotAllowed (403) response if downloadId identifier part is school URN from a different LA (LA scope)
    Given blob storage file downloads-config.json exists in config container:
    """
    [
        { "id": "test" }
    ]
    """
    And Local Authority "111" exists:
    """
    {
        "name": "Test LA 1"
    }
    """    
    And Local Authority "222" exists:
    """
    {
        "name": "Test LA 2"
    }
    """    
    And Establishment "111111" exists:
    """
    {
        "name": "Test School 1",
        "localAuthority": {
            "code": "111"
        }
    }
    """
    And Establishment "222222" exists:
    """
    {
        "name": "Test School 2",
        "localAuthority": {
            "code": "222"
        }
    }
    """
    When I send a GET request to /api/downloads/package?scope=LA&scopeId=111&fileType=CSV&downloadIds=test-111111-2024&downloadIds=test-222222-2024
    Then I should get a 403 response
    And the response should be the message "Not allowed: Identifier "222222" is not accessible within the given scope."

Scenario: Should return NotFound (404) response if single download id supplied containing non-existent download-config id
    Given blob storage file downloads-config.json exists in config container:
    """
    [
        { "id": "abc" },
        { "id": "def" }
    ]
    """
    And Establishment "123456" exists:
    """
    {
        "name": "Test School 1"
    }
    """ 
    When I send a GET request to /api/downloads/package?scope=School&scopeId=123456&fileType=CSV&downloadIds=xyz-123456-2024
    Then I should get a 404 response
    And the response should be the message "Not found: There is no download config with id "xyz"."

Scenario: Should return NotFound (404) response if multiple download ids supplied, one of which contains non-existent download-config id
    Given blob storage file downloads-config.json exists in config container:
    """
    [
        { "id": "abc", "filePathPattern":"", "source":"KTS" },
        { "id": "def", "filePathPattern":"", "source":"KTS" }
    ]
    """
    And Establishment "123456" exists:
    """
    {
        "name": "Test School 1"
    }
    """ 
    When I send a GET request to /api/downloads/package?scope=School&scopeId=123456&fileType=CSV&downloadIds=abc-123456-2024&downloadIds=xyz-123456-2024
    Then I should get a 404 response
    And the response should be the message "Not found: There is no download config with id "xyz"."

Scenario: Should return NotFound (404) response if downloads-config exists but blob storage file matching filePathPattern does not exist
    Given blob storage file downloads-config.json exists in config container:
    """
    [
        {
            "id": "test",
            "filePathPattern": "test.csv",
            "source": "KTS"
        }
    ]
    """
    And Establishment "123456" exists:
    """
    {
        "name": "Test School 1"
    }
    """ 
    And no blob storage files exist in downloads-kts container 
    When I send a GET request to /api/downloads/package?scope=School&scopeId=123456&fileType=CSV&downloadIds=test-123456-2024
    Then I should get a 404 response
    And the response should be the message "Not found: Blob storage file "test.csv" does not exist in container "downloads-kts"."

Scenario: Should zip up file from blob storage matching filePathPattern of downloads-config
    Given blob storage file downloads-config.json exists in config container:
    """
    [
        {
            "id": "test",
            "filePathPattern": "test.csv",
            "source": "KTS"
        }
    ]
    """
    And Establishment "123456" exists:
    """
    {
        "name": "Test School 1"
    }
    """ 
    And blob storage file test.csv exists in downloads-kts container:
    """
    test
    """
    When I send a GET request to /api/downloads/package?scope=School&scopeId=123456&fileType=CSV&downloadIds=test-123456-2024
    Then the response should be a ZIP file download
    And the ZIP file download should contain 1 file
    And the ZIP file download should contain the file test.csv with contents:
    """
    test
    """

Scenario Outline: Should create download filename based on current time
    Given blob storage file downloads-config.json exists in config container:
    """
    [
        {
            "id": "test",
            "source": "KTS",
            "filePathPattern": "test.csv",
            "source": "KTS"
        }
    ]
    """
	And Establishment "111111" exists:
	"""
	{
		"name": "Test School"
	}
	"""
    And blob storage file test.csv exists in downloads-kts container:
    """
    test
    """
    And the current time is <currentTime>
    When I send a GET request to /api/downloads/package?scope=School&scopeId=111111&fileType=CSV&downloadIds=test-111111-2024
    Then the response should be a ZIP file download with filename <filename>
Examples:
    | currentTime         | filename                         |
    | 2024/01/01 01:23:45 | 20240101_012345_download.zip |
    | 2023/12/30 00:00:00 | 20231230_000000_download.zip |

Scenario: Should zip up multiple files from blob storage based on from different download-configs
    Given blob storage file downloads-config.json exists in config container:
    """
    [
        {
            "id": "test1",
            "source": "KTS",
            "filePathPattern": "test1_file.csv"
        },
        {
            "id": "test2",
            "source": "KTS",
            "filePathPattern": "test2_file.csv"
        }

    ]
    """
    And Establishment "123456" exists:
    """
    {
        "name": "Test School 1"
    }
    """ 
    And blob storage file test1_file.csv exists in downloads-kts container:
    """
    test1
    """
    And blob storage file test2_file.csv exists in downloads-kts container:
    """
    test2
    """
    And the current time is 2024/01/01 01:23:45
    When I send a GET request to /api/downloads/package?scope=School&scopeId=123456&fileType=CSV&downloadIds=test1-123456-2024&downloadIds=test2-123456-2024
    Then the response should be a ZIP file download
    And the ZIP file download should contain 2 files
    And the ZIP file download should contain the file test1_file.csv with contents:
    """
    test1
    """
    And the ZIP file download should contain the file test2_file.csv with contents:
    """
    test2
    """

Scenario: Should zip up multiple files from blob storage based on from different download-configs (one doesn't exist)
    Given blob storage file downloads-config.json exists in config container:
    """
    [
        {
            "id": "test1",
            "source": "KTS",
            "filePathPattern": "test1_file.csv"
        },
        {
            "id": "test2",
            "source": "KTS",
            "filePathPattern": "test2_file.csv"
        }
    ]
    """
    And Establishment "123456" exists:
    """
    {
        "name": "Test School 1"
    }
    """ 
    And blob storage file test1_file.csv exists in downloads-kts container:
    """
    test1
    """
    And the current time is 2024/01/01 01:23:45
    When I send a GET request to /api/downloads/package?scope=School&scopeId=123456&fileType=CSV&downloadIds=test1-123456-2024&downloadIds=test2-123456-2024
    Then I should get a 404 response
    And the response should be the message "Not found: Blob storage file "test2_file.csv" does not exist in container "downloads-kts"."

Scenario: Should be able to download multiple years by replacing {year} in filePathPattern with year from downloadId
    Given blob storage file downloads-config.json exists in config container:
    """
    [
        {
            "id": "test",
            "source": "KTS",
            "filePathPattern": "School/{year}/test.csv"
        }
    ]
    """
	And Establishment "123456" exists:
	"""
	{
		"name": "Test School"
	}
	"""
    And blob storage file School/2022/test.csv exists in downloads-kts container:
    """
    test-2022
    """
    And blob storage file School/2023/test.csv exists in downloads-kts container:
    """
    test-2023
    """
    And blob storage file School/2024/test.csv exists in downloads-kts container:
    """
    test-2024
    """
    When I send a GET request to /api/downloads/package?scope=School&scopeId=123456&fileType=CSV&downloadIds=test-123456-2022&downloadIds=test-123456-2023&downloadIds=test-123456-2024
    Then the response should be a ZIP file download
    And the ZIP file download should contain 3 files
    And the ZIP file download should contain the file School/2022/test.csv with contents:
    """
    test-2022
    """
    And the ZIP file download should contain the file School/2023/test.csv with contents:
    """
    test-2023
    """
    And the ZIP file download should contain the file School/2024/test.csv with contents:
    """
    test-2024
    """

Scenario: Should be able to download multiple years by replacing {year} in filePathPattern with year from downloadId (one doesn't exist)
    Given blob storage file downloads-config.json exists in config container:
    """
    [
        {
            "id": "test",
            "source": "KTS",
            "filePathPattern": "School/{year}/test.csv"
        }
    ]
    """
	And Establishment "123456" exists: 
	"""
		{
		   "name": "Test School"
		}   
	"""
    And blob storage file School/2024/test.csv exists in downloads-kts container:
    """
    test-2024
    """
    When I send a GET request to /api/downloads/package?scope=School&scopeId=123456&fileType=CSV&downloadIds=test-123456-2023&downloadIds=test-123456-2024
    Then I should get a 404 response
    And the response should be the message "Not found: Blob storage file "School/2023/test.csv" does not exist in container "downloads-kts"."

Scenario: Should be able to download multiple school URNs by replacing {urn} in filePathPattern with URN from downloadId
    Given blob storage file downloads-config.json exists in config container:
    """
    [
        {
            "id": "test",
            "source": "KTS",
            "filePathPattern": "School/{urn}/test.csv"
        }
    ]
    """
    And Local Authority "111" exists:
    """
    {
        "name": "Test LA"
    }
    """ 
    And Establishment "111111" exists:
    """
    {
        "name": "Test School 1",
        "localAuthority": {
            "code": "111"
        }
    }
    """
    And Establishment "222222" exists:
    """
    {
        "name": "Test School 2",
        "localAuthority": {
            "code": "111"
        }
    }
    """
    And Establishment "333333" exists:
    """
    {
        "name": "Test School 3",
        "localAuthority": {
            "code": "111"
        }
    }
    """
    And blob storage file School/111111/test.csv exists in downloads-kts container:
    """
    test-111111
    """
    And blob storage file School/222222/test.csv exists in downloads-kts container:
    """
    test-222222
    """
    And blob storage file School/333333/test.csv exists in downloads-kts container:
    """
    test-333333
    """
    When I send a GET request to /api/downloads/package?scope=LA&scopeId=111&fileType=CSV&downloadIds=test-111111-2024&downloadIds=test-222222-2024&downloadIds=test-333333-2024
    Then the response should be a ZIP file download
    And the ZIP file download should contain 3 files
    And the ZIP file download should contain the file School/111111/test.csv with contents:
    """
    test-111111
    """
    And the ZIP file download should contain the file School/222222/test.csv with contents:
    """
    test-222222
    """
    And the ZIP file download should contain the file School/333333/test.csv with contents:
    """
    test-333333
    """

Scenario Outline: Should be able to download multiple versions by replacing {version} in filePathPattern with version from downloadId
    Given blob storage file downloads-config.json exists in config container:
    """
    [
        {
            "id": "test",
            "source": "KTS",
            "filePathPattern": "test_{version}.csv"
        }
    ]
    """
	And Establishment "111111" exists:
	"""
	{
		"name": "Test School"
	}
	"""
    And blob storage file test_<version>.csv exists in downloads-kts container:
    """
    test
    """
    And the current time is 2024/01/01 01:23:45
    When I send a GET request to /api/downloads/package?scope=School&scopeId=111111&fileType=CSV&downloadIds=test-111111-2024-<versionDashed>
    Then I should get a 200 response
    And the response should be a ZIP file download
    And the ZIP file download should contain 1 file
    And the ZIP file download should contain the file test_<version>.csv with contents:
    """
    test
    """
Examples:
    | version                 | versionDashed           |
    | provisional             | provisional             |
    | provisional_without_cla | provisional-without-cla |
    | provisional_with_cla    | provisional-with-cla    |
    | revised                 | revised                 |
    | revised_without_cla     | revised-without-cla     |
    | revised_with_cla        | revised-with-cla        |
    | final                   | final                   |
    | final_without_cla       | final-without-cla       |
    | final_with_cla          | final-with-cla          |

Scenario Outline: Should be able to download multiple versions by replacing {version} in filePathPattern with version from downloadId (file doesn't exist)
    Given blob storage file downloads-config.json exists in config container:
    """
    [
        {
            "id": "test",
            "source": "KTS",
            "filePathPattern": "test_{version}.csv"
        }
    ]
    """
    And Establishment "123456" exists:
	"""
	{
		"name": "Test School"
	}
	"""
    And blob storage file test_revised.csv exists in downloads-kts container:
    """
    test
    """
    And the current time is 2024/01/01 01:23:45
    When I send a GET request to /api/downloads/package?scope=School&scopeId=123456&fileType=CSV&downloadIds=test-123456-2024-revised&downloadIds=test-123456-2024-final-with-cla
    Then I should get a 404 response
    And the response should be the message "Not found: Blob storage file "test_final_with_cla.csv" does not exist in container "downloads-kts"."

Scenario: Should be able to download fileType by replacing {fileType} in filePathPattern with requested fileType (case insensitive)
    Given blob storage file downloads-config.json exists in config container:
    """
    [
        {
            "id": "test",
            "source": "KTS",
            "filePathPattern": "School/{filetype}/test.{filetype}"
        }
    ]
    """
    And Establishment "111111" exists:
    """
    {
        "name": "Test LA 1"
    }
    """   
    And blob storage file School/<fileTypeInBlobStorage>/test.<fileTypeInBlobStorage> exists in downloads-kts container:
    """
    test
    """
    When I send a GET request to /api/downloads/package?scope=School&scopeId=111111&fileType=<requestedFileType>&downloadIds=test-111111-2024
    Then the response should be a ZIP file download
    And the ZIP file download should contain 1 file
    And the ZIP file download should contain the file School/<fileTypeInBlobStorage>/test.<fileTypeInBlobStorage> with contents:
    """
    test
    """
Examples:
    | requestedFileType | fileTypeInBlobStorage |
    | csv               | csv                   |
    | CSV               | csv                   |
    | tsv               | tsv                   |
    | TSV               | tsv                   |
    | xlsx              | xlsx                  |
    | XLSX              | xlsx                  |

Scenario: Should be able to download fileType by replacing {fileType} in filePathPattern with requested fileType (case insensitive) (not found)
    Given blob storage file downloads-config.json exists in config container:
    """
    [
        {
            "id": "test",
            "source": "KTS",
            "filePathPattern": "School/{filetype}/test.{filetype}"
        }
    ]
    """
	And Establishment "111111" exists:
	"""
	{
		"name": "Test LA 1"
	}
	"""
    And blob storage file School/abc/test.abc exists in downloads-kts container:
    """
    test
    """
    When I send a GET request to /api/downloads/package?scope=School&scopeId=111111&fileType=csv&downloadIds=test-111111-2024
    Then I should get a 404 response
    And the response should be the message "Not found: Blob storage file "School/csv/test.csv" does not exist in container "downloads-kts"."

Scenario: Should zip up files from both ASP and KTS containers from blob storage
    Given blob storage file downloads-config.json exists in config container:
    """
    [
        {
            "id": "test-kts",
            "source": "KTS",
            "filePathPattern": "test_kts_file.csv"
        },
        {
            "id": "test-asp",
            "source": "ASP",
            "filePathPattern": "test_asp_file.csv"
        }
    ]
    """
    And Establishment "123456" exists:
    """
    {
        "name": "Test School 1"
    }
    """ 
    And blob storage file test_kts_file.csv exists in downloads-kts container:
    """
    test_kts
    """
    And blob storage file test_asp_file.csv exists in downloads-asp container:
    """
    test_asp
    """
    When I send a GET request to /api/downloads/package?scope=School&scopeId=123456&fileType=CSV&downloadIds=test-kts-123456-2024&downloadIds=test-asp-123456-2024
    Then the response should be a ZIP file download
    And the ZIP file download should contain 2 files
    And the ZIP file download should contain the file test_kts_file.csv with contents:
    """
    test_kts
    """
    And the ZIP file download should contain the file test_asp_file.csv with contents:
    """
    test_asp
    """

Scenario: Should zip up files from both ASP and KTS containers from blob storage (not found)
    Given blob storage file downloads-config.json exists in config container:
    """
    [
        {
            "id": "test-kts",
            "source": "KTS",
            "filePathPattern": "test_kts_file.csv"
        },
        {
            "id": "test-asp",
            "source": "ASP",
            "filePathPattern": "test_asp_file.csv"
        }
    ]
    """
    And Establishment "123456" exists:
    """
    {
        "name": "Test School 1"
    }
    """ 
    And blob storage file test_kts_file.csv exists in downloads-kts container:
    """
    test_kts
    """
    When I send a GET request to /api/downloads/package?scope=School&scopeId=123456&fileType=CSV&downloadIds=test-kts-123456-2024&downloadIds=test-asp-123456-2024
    Then I should get a 404 response
    And the response should be the message "Not found: Blob storage file "test_asp_file.csv" does not exist in container "downloads-asp"."

Scenario: Should be able to handle multiple years from both ASP and KTS containers
    Given blob storage file downloads-config.json exists in config container:
    """
    [
        {
            "id": "test-kts",
            "source": "KTS",
            "filePathPattern": "School/{year}/test-kts.csv"
        },
        {
            "id": "test-asp",
            "source": "ASP",
            "filePathPattern": "School/{year}/test-asp.csv"
        }
    ]
    """
	And Establishment "123456" exists:
	"""
	{
		"name": "Test School"
	}
	"""
    And blob storage file School/2023/test-kts.csv exists in downloads-kts container:
    """
    test-kts-2023
    """
    And blob storage file School/2024/test-kts.csv exists in downloads-kts container:
    """
    test-kts-2024
    """
    And blob storage file School/2023/test-asp.csv exists in downloads-asp container:
    """
    test-asp-2023
    """
    And blob storage file School/2024/test-asp.csv exists in downloads-asp container:
    """
    test-asp-2024
    """
    When I send a GET request to /api/downloads/package?scope=School&scopeId=123456&fileType=CSV&downloadIds=test-kts-123456-2023&downloadIds=test-kts-123456-2024&downloadIds=test-asp-123456-2023&downloadIds=test-asp-123456-2024
    Then the response should be a ZIP file download
    And the ZIP file download should contain 4 files
    And the ZIP file download should contain the file School/2023/test-kts.csv with contents:
    """
    test-kts-2023
    """
    And the ZIP file download should contain the file School/2024/test-kts.csv with contents:
    """
    test-kts-2024
    """
    And the ZIP file download should contain the file School/2023/test-asp.csv with contents:
    """
    test-asp-2023
    """
    And the ZIP file download should contain the file School/2024/test-asp.csv with contents:
    """
    test-asp-2024
    """

 Scenario: Should be able to handle multiple years from both ASP and KTS containers (not found)
    Given blob storage file downloads-config.json exists in config container:
    """
    [
        {
            "id": "test-kts",
            "source": "KTS",
            "filePathPattern": "School/{year}/test-kts.csv"
        },
        {
            "id": "test-asp",
            "source": "ASP",
            "filePathPattern": "School/{year}/test-asp.csv"
        }

    ]
    """
	And Establishment "123456" exists:
	"""
	{
		"name": "Test School"
	}
	"""
    And blob storage file School/2023/test-kts.csv exists in downloads-kts container:
    """
    test-kts-2023
    """
    And blob storage file School/2024/test-kts.csv exists in downloads-kts container:
    """
    test-kts-2024
    """
    And blob storage file School/2023/test-asp.csv exists in downloads-asp container:
    """
    test-asp-2023
    """
    When I send a GET request to /api/downloads/package?scope=School&scopeId=123456&fileType=CSV&downloadIds=test-kts-123456-2023&downloadIds=test-kts-123456-2024&downloadIds=test-asp-123456-2023&downloadIds=test-asp-123456-2024
    Then I should get a 404 response
    And the response should be the message "Not found: Blob storage file "School/2024/test-asp.csv" does not exist in container "downloads-asp"."