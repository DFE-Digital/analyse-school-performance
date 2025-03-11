Feature: DownloadsGetAll

Scenario: Should not accept POST method
	When I send a POST request to /api/downloads
	Then I should get a 405 response
	And the response should be the message "Method not allowed: The HTTP method POST is not allowed."
	And the response should include the header "Allow: GET"

Scenario: Should return BadRequest (400) response if scope parameter is missing
	When I send a GET request to /api/downloads
	Then I should get a 400 response
	And the response should be the message "Bad request: The query parameter "scope" is missing."

Scenario: Should return BadRequest (400) response if scope parameter is empty string
	When I send a GET request to /api/downloads?scope=
	Then I should get a 400 response
	And the response should be the message "Bad request: The query parameter "scope" should not be empty."

Scenario: Should return BadRequest (400) response if scope parameter is invalid
	Given no local authorities exist
	When I send a GET request to /api/downloads?scope=xyz
	Then I should get a 400 response
	And the response should be the message "Bad request: "xyz" is not a valid scope."

Scenario: Should return BadRequest (400) response if scopeId parameter is missing
	When I send a GET request to /api/downloads?scope=LA
	Then I should get a 400 response
	And the response should be the message "Bad request: The query parameter "scopeId" is missing."

Scenario: Should return BadRequest (400) response if scopeId parameter is empty string
	When I send a GET request to /api/downloads?scope=LA&scopeId
	Then I should get a 400 response
	And the response should be the message "Bad request: The query parameter "scopeId" should not be empty."

Scenario: Should return BadRequest (400) response if year parameter is not digits
	When I send a GET request to /api/downloads?scope=LA&scopeId=301&year=xxxx
	Then I should get a 400 response
	And the response should be the message "Bad request: The query parameter "year" should be a whole number greater than or equal to 1."

Scenario: Should return BadRequest (400) response if year parameter is not 4 digits long
	When I send a GET request to /api/downloads?scope=LA&scopeId=301&year=<year>
	Then I should get a 400 response
	And the response should be the message "Bad request: The query parameter "year" must be exactly 4 characters long."
Examples:
	| year  |
	| 123   |
	| 12345 |

Scenario: Should return ServerError (500) response if Downloads config is not valid JSON
	Given local authority Test LA (100) exists
	And blob storage file downloads-config.json exists in config container:
	"""
	Hello
	"""
	When I send a GET request to /api/downloads?scope=LA&scopeId=100
	Then I should get a 500 response
	And the response should be the message "{"ErrorType":"Unexpected","StackTrace":null,"Message":"The configuration file 'downloads-config.json' contained invalid JSON.","MessagePrefix":"Unexpected: "}"

Scenario: Should return ServerError (500) response if Downloads config is empty
	Given local authority Test LA (100) exists
	And blob storage file downloads-config.json exists in config container:
	"""
	[
	]
	"""
	When I send a GET request to /api/downloads?scope=LA&scopeId=100
	Then I should get a 500 response
	And the response should be the message "{"ErrorType":"Unexpected","StackTrace":null,"Message":"The configuration file 'downloads-config.json' was empty.","MessagePrefix":"Unexpected: "}"

Scenario: Should return ServerError (500) response if config source is invalid
	Given local authority Test LA (100) exists
	And blob storage file downloads-config.json exists in config container:
	"""
	[
		{
			"id": "test",
			"source": "XYZ",
			"scope": "LocalAuthority",
			"dataSetType": "test",
			"label": "Test",
			"filePathPattern": "LA/{code}/{year}/{filetype}/file_{version}.{filetype}"
		}
	]
	"""
	When I send a GET request to /api/downloads?scope=LA&scopeId=100
	Then I should get a 500 response
	And the response should be the message "{"ErrorType":"Unexpected","StackTrace":null,"Message":"The downloads source 'XYZ' was not recognised.","MessagePrefix":"Unexpected: "}"

Scenario: LA scope: Should return NotFound (404) response if LA does not exist
	Given no local authorities exist
	And blob storage file downloads-config.json exists in config container:
	"""
	[
		{}
	]
	"""
	When I send a GET request to /api/downloads?scope=LA&scopeId=101
	Then I should get a 404 response
	And the response should be the message "Not found: Could not find Local Authority with code "101"."

Scenario: LA scope: Should return NotFound (404) response if no downloads exist
	Given local authority Test LA (100) exists
	And blob storage file downloads-config.json exists in config container:
	"""
	[
		{
			"id": "test",
			"source": "KTS",
			"scope": "LocalAuthority",
			"dataSetType": "test",
			"label": "Test",
			"filePathPattern": "LA/{code}/{year}/{filetype}/file_{version}.{filetype}"
		}
	]
	"""
	When I send a GET request to /api/downloads?scope=LA&scopeId=100
	Then I should get a 404 response
	And the response should be the message "Not found: There are no downloads available for Local Authority "100"."

Scenario: LA scope: Should return NotFound (404) response if no downloads exist for the given year
	Given local authority Test LA (100) exists
	And blob storage file downloads-config.json exists in config container:
	"""
	[
		{
			"id": "kts-la-ks2-pupil",
			"source": "KTS",
			"scope": "LocalAuthority",
			"dataSetType": "KeyStage2",
			"label": "KS2 pupil",
			"filePathPattern": "LA/{code}/{year}/{filetype}/ks2_pupil_{version}.{filetype}"
		}
	]
	"""
	And blob storage file LA/100/2022/csv/ks2_pupil_provisional.csv exists in downloads-kts container:
	"""
	Column A,Column B,Column C
	1,2,3
	"""
	And blob storage file LA/100/2023/csv/ks2_pupil_provisional.csv exists in downloads-kts container:
	"""
	Column A,Column B,Column C
	1,2,3
	"""
	And blob storage file LA/100/2024/csv/ks2_pupil_provisional.csv exists in downloads-kts container:
	"""
	Column A,Column B,Column C
	1,2,3
	"""
	When I send a GET request to /api/downloads?scope=LA&scopeId=100&year=2000
	Then I should get a 404 response
	And the response should be the message "Not found: There are no downloads available for Local Authority "100" for the year 2000."

Scenario: LA scope: Should return NotFound (404) response if files exist at School level but not LocalAuthority
	Given local authority Test LA (100) exists
	And blob storage file downloads-config.json exists in config container:
	"""
	[
		{
			"id": "kts-school-ks2-pupil",
			"source": "KTS",
			"scope": "School",
			"dataSetType": "KeyStage2",
			"label": "KS2 pupil",
			"filePathPattern": "School/{urn}/{year}/{filetype}/ks2_pupil_{version}.{filetype}"
		},
		{
			"id": "kts-la-ks2-pupil",
			"source": "KTS",
			"scope": "LocalAuthority",
			"dataSetType": "KeyStage2",
			"label": "KS2 pupil",
			"filePathPattern": "LA/{code}/{year}/{filetype}/ks2_pupil_{version}.{filetype}"
		}
	]
	"""
	And blob storage file School/123456/2024/csv/ks2_pupil_provisional.csv exists in downloads-kts container:
	"""
	Column A,Column B,Column C
	1,2,3
	"""
	When I send a GET request to /api/downloads?scope=LA&scopeId=100
	Then I should get a 404 response
	And the response should be the message "Not found: There are no downloads available for Local Authority "100"."

Scenario: LA scope: Should return NotFound (404) response if files exist at LocalAuthority level but not for LA
	Given local authority Test LA (100) exists
	And blob storage file downloads-config.json exists in config container:
	"""
	[
		{
			"source": "KTS",
			"scope": "LocalAuthority",
			"dataSetType": "KeyStage2",
			"label": "KS2 pupil",
			"filePathPattern": "LA/{code}/{year}/{filetype}/ks2_pupil_{version}.{filetype}"
		}
	]
	"""
	And blob storage file LA/101/2024/csv/ks2_pupil_provisional.csv exists in downloads-kts container:
	"""
	Column A,Column B,Column C
	1,2,3
	"""
	And blob storage file LA/102/2024/csv/ks2_pupil_provisional.csv exists in downloads-kts container:
	"""
	Column A,Column B,Column C
	1,2,3
	"""
	When I send a GET request to /api/downloads?scope=LA&scopeId=100
	Then I should get a 404 response
	And the response should be the message "Not found: There are no downloads available for Local Authority "100"."

Scenario: LA scope: Should return NotFound (404) response if files exist for LA but don't match config filepath pattern
	Given local authority Test LA (100) exists
	And blob storage file downloads-config.json exists in config container:
	"""
	[
		{
			"id": "kts-la-ks2-pupil",
			"source": "KTS",
			"scope": "LocalAuthority",
			"dataSetType": "KeyStage2",
			"label": "KS2 pupil",
			"filePathPattern": "LA/{code}/{year}/{filetype}/ks2_pupil_{version}.{filetype}"
		}
	]
	"""
	And blob storage file LA/100/2024/csv/ks4_pupil_provisional.csv exists in downloads-kts container:
	"""
	Column A,Column B,Column C
	1,2,3
	"""
	And blob storage file LA/100/2024/csv/ks2_pupil_x_provisional.csv exists in downloads-kts container:
	"""
	Column A,Column B,Column C
	1,2,3
	"""
	When I send a GET request to /api/downloads?scope=LA&scopeId=100
	Then I should get a 404 response
	And the response should be the message "Not found: There are no downloads available for Local Authority "100"."

Scenario: LA scope: Should return KTS downloads for LA for all matching years
	Given local authority Test LA (100) exists
	And blob storage file downloads-config.json exists in config container:
	"""
	[
		{
			"id": "kts-la-ks2-pupil",
			"source": "KTS",
			"scope": "LocalAuthority",
			"dataSetType": "KeyStage2",
			"label": "KS2 pupil",
			"filePathPattern": "LA/{code}/{year}/{filetype}/ks2_pupil_{version}.{filetype}"
		}
	]
	"""
	And blob storage file LA/100/2023/csv/ks2_pupil_provisional.csv exists in downloads-kts container:
	"""
	Column A,Column B,Column C
	1,2,3
	"""
	And blob storage file LA/100/2024/csv/ks2_pupil_provisional.csv exists in downloads-kts container:
	"""
	Column A,Column B,Column C
	1,2,3
	"""
	When I send a GET request to /api/downloads?scope=LA&scopeId=100
	Then I should get a 200 response
	And the response should be an object containing these properties:
	"""
	{
		"Downloads": [
			{
				"Id": "kts-la-ks2-pupil-100-2023-provisional",
				"Source": "Key to success",
				"Label": "KS2 pupil",
				"DatasetType": "Key stage 2 (KS2)",
				"Year": 2023,
				"Version": "Provisional"
			},
			{
				"Id": "kts-la-ks2-pupil-100-2024-provisional",
				"Source": "Key to success",
				"Label": "KS2 pupil",
				"DatasetType": "Key stage 2 (KS2)",
				"Year": 2024,
				"Version": "Provisional"
			},
		]
	}
	"""

Scenario: LA scope: Should ignore any non-matching files
	Given local authority Test LA (100) exists
	And blob storage file downloads-config.json exists in config container:
	"""
	[
		{
			"id": "kts-la-ks2-pupil",
			"source": "KTS",
			"scope": "LocalAuthority",
			"dataSetType": "KeyStage2",
			"label": "KS2 pupil",
			"filePathPattern": "LA/{code}/{year}/{filetype}/ks2_pupil_{version}.{filetype}"
		}
	]
	"""
	And blob storage file LA/100/2024/csv/ks2_pupil_provisional.csv exists in downloads-kts container:
	"""
	Column A,Column B,Column C
	1,2,3
	"""
	And blob storage file Hello-there/ignore-me.csv exists in downloads-kts container:
	"""
	Column A,Column B,Column C
	1,2,3
	"""
	When I send a GET request to /api/downloads?scope=LA&scopeId=100
	Then I should get a 200 response
	And the response should be an object containing these properties:
	"""
	{
		"Downloads": [
			{
				"Id": "kts-la-ks2-pupil-100-2024-provisional",
				"Source": "Key to success",
				"Label": "KS2 pupil",
				"DatasetType": "Key stage 2 (KS2)",
				"Year": 2024,
				"Version": "Provisional"
			}
		]
	}
	"""

Scenario: LA scope: Should ignore different filetypes within LA for the same year
	Given local authority Test LA (100) exists
	And blob storage file downloads-config.json exists in config container:
	"""
	[
		{
			"id": "kts-la-ks2-pupil",
			"source": "KTS",
			"scope": "LocalAuthority",
			"dataSetType": "KeyStage2",
			"label": "KS2 pupil",
			"filePathPattern": "LA/{code}/{year}/{filetype}/ks2_pupil_{version}.{filetype}"
		}
	]
	"""
	And blob storage file LA/100/2023/csv/ks2_pupil_provisional.csv exists in downloads-kts container:
	"""
	Column A,Column B,Column C
	1,2,3
	"""
	And blob storage file LA/100/2023/tsv/ks2_pupil_provisional.tsv exists in downloads-kts container:
	"""
	Column A\tColumn B\tColumn C
	1\t2\t3
	"""
	And blob storage file LA/100/2024/csv/ks2_pupil_provisional.csv exists in downloads-kts container:
	"""
	Column A,Column B,Column C
	1,2,3
	"""
	And blob storage file LA/100/2024/tsv/ks2_pupil_provisional.tsv exists in downloads-kts container:
	"""
	Column A\tColumn B\tColumn C
	1\t2\t3
	"""
	When I send a GET request to /api/downloads?scope=LA&scopeId=100
	Then I should get a 200 response
	And the response should be an object containing these properties:
	"""
	{
		"Downloads": [
			{
				"Id": "kts-la-ks2-pupil-100-2023-provisional",
				"Source": "Key to success",
				"Label": "KS2 pupil",
				"DatasetType": "Key stage 2 (KS2)",
				"Year": 2023,
				"Version": "Provisional"
			},
			{
				"Id": "kts-la-ks2-pupil-100-2024-provisional",
				"Source": "Key to success",
				"Label": "KS2 pupil",
				"DatasetType": "Key stage 2 (KS2)",
				"Year": 2024,
				"Version": "Provisional"
			}
		]
	}
	"""

Scenario: LA scope: Should return user-friendly text for different dataset types
	Given local authority Test LA (100) exists
	And blob storage file downloads-config.json exists in config container:
	"""
	[
		{
			"id": "test-asp",
			"source": "ASP",
			"scope": "LocalAuthority",
			"dataSetType": "<dataSetType>",
			"label": "Test",
			"filePathPattern": "LA/{code}/{year}/test/file_{version}.{filetype}"
		},
		{
			"id": "test-kts",
			"source": "KTS",
			"scope": "LocalAuthority",
			"dataSetType": "<dataSetType>",
			"label": "Test",
			"filePathPattern": "LA/{code}/{year}/{filetype}/file_{version}.{filetype}"
		}
	]
	"""
	And blob storage file LA/100/2024/test/file_provisional.csv exists in downloads-asp container:
	"""
	Column A,Column B,Column C
	1,2,3
	"""
	And blob storage file LA/100/2024/csv/file_provisional.csv exists in downloads-kts container:
	"""
	Column A,Column B,Column C
	1,2,3
	"""
	When I send a GET request to /api/downloads?scope=LA&scopeId=100
	Then I should get a 200 response
	And the response should be an object containing these properties:
	"""
	{
		"Downloads": [
			{
				"Id": "test-asp-100-2024-provisional",
				"Source": "Analyse school performance",
				"Label": "Test",
				"DatasetType": "<userFriendlyDataSetType>",
				"Year": 2024,
				"Version": "Provisional"
				},
			{
				"Id": "test-kts-100-2024-provisional",
				"Source": "Key to success",
				"Label": "Test",
				"DatasetType": "<userFriendlyDataSetType>",
				"Year": 2024,
				"Version": "Provisional"
			}
		]
	}
	"""
Examples:
	| dataSetType           | userFriendlyDataSetType          |
	| KeyStage2             | Key stage 2 (KS2)                |
	| KeyStage4             | Key stage 4 (KS4)                |
	| Post16                | 16-18                            |
	| QLA                   | QLA (year 6 only)                |
	| SchoolCharacteristics | School characteristics           |
	| Absence               | Absence                          |
	| Exclusions            | Exclusions                       |
	| MTC                   | Multiplication table check (MTC) |
	| Phonics               | Phonics                          |

Scenario: LA scope: Should return user-friendly text for different versions
	Given local authority Test LA (100) exists
	And blob storage file downloads-config.json exists in config container:
	"""
	[
		{
			"id": "test-asp",
			"source": "ASP",
			"scope": "LocalAuthority",
			"dataSetType": "KeyStage2",
			"label": "Test",
			"filePathPattern": "LA/{code}/{year}/test/file_{version}.{filetype}"
		},
		{
			"id": "test-kts",
			"source": "KTS",
			"scope": "LocalAuthority",
			"dataSetType": "KeyStage2",
			"label": "Test",
			"filePathPattern": "LA/{code}/{year}/{filetype}/file_{version}.{filetype}"
		}
	]
	"""
	And blob storage file LA/100/2024/test/file_<version>.csv exists in downloads-asp container:
	"""
	Column A,Column B,Column C
	1,2,3
	"""
	And blob storage file LA/100/2024/csv/file_<version>.csv exists in downloads-kts container:
	"""
	Column A,Column B,Column C
	1,2,3
	"""
	When I send a GET request to /api/downloads?scope=LA&scopeId=100
	Then I should get a 200 response
	And the response should be an object containing these properties:
	"""
	{
		"Downloads": [
			{
				"Id": "test-asp-100-2024-<versionDashed>",
				"Source": "Analyse school performance",
				"Label": "Test",
				"DatasetType": "Key stage 2 (KS2)",
				"Year": 2024,
				"Version": "<userFriendlyVersion>"
			},
			{
				"Id": "test-kts-100-2024-<versionDashed>",
				"Source": "Key to success",
				"Label": "Test",
				"DatasetType": "Key stage 2 (KS2)",
				"Year": 2024,
				"Version": "<userFriendlyVersion>"
			}
		]
	}
	"""
Examples:
	| version                 | versionDashed           | userFriendlyVersion      |
	| provisional             | provisional             | Provisional              |
	| provisional_without_cla | provisional-without-cla | Provisional, without CLA |
	| provisional_with_cla    | provisional-with-cla    | Provisional, with CLA    |
	| revised                 | revised                 | Revised                  |
	| revised_without_cla     | revised-without-cla     | Revised, without CLA     |
	| revised_with_cla        | revised-with-cla        | Revised, with CLA        |
	| final                   | final                   | Final                    |
	| final_without_cla       | final-without-cla       | Final, without CLA       |
	| final_with_cla          | final-with-cla          | Final, with CLA          |

Scenario: LA scope: Should ignore different file type casing
	Given local authority Test LA (100) exists
	And blob storage file downloads-config.json exists in config container:
	"""
	[
		{
			"id": "kts-la-ks2-pupil",
			"source": "KTS",
			"scope": "LocalAuthority",
			"dataSetType": "KeyStage2",
			"label": "KS2 pupil",
			"filePathPattern": "LA/{code}/{year}/{filetype}/ks2_pupil_{version}.{filetype}"
		}
	]
	"""
	And blob storage file LA/100/2024/csv/ks2_pupil_provisional.<fileType> exists in downloads-kts container:
	"""
	Column A,Column B,Column C
	1,2,3
	"""
	When I send a GET request to /api/downloads?scope=LA&scopeId=100
	Then I should get a 200 response
	And the response should be an object containing these properties:
	"""
	{
		"Downloads": [
			{
				"Id": "kts-la-ks2-pupil-100-2024-provisional",
				"Source": "Key to success",
				"Label": "KS2 pupil",
				"DatasetType": "Key stage 2 (KS2)",
				"Year": 2024,
				"Version": "Provisional"
			}
		]
	}
	"""
Examples:
	| fileType |
	| csv      |
	| CSV      |
	| CsV      |

Scenario: LA scope: Should return KTS downloads filtered by year
	Given local authority Test LA (100) exists
	And blob storage file downloads-config.json exists in config container:
	"""
	[
		{
			"id": "kts-la-ks2-pupil",
			"source": "KTS",
			"scope": "LocalAuthority",
			"dataSetType": "KeyStage2",
			"label": "KS2 pupil",
			"filePathPattern": "LA/{code}/{year}/{filetype}/ks2_pupil_{version}.{filetype}"
		}
	]
	"""
	And blob storage file LA/100/2022/csv/ks2_pupil_provisional.csv exists in downloads-kts container:
	"""
	Column A,Column B,Column C
	1,2,3
	"""
	And blob storage file LA/100/2023/csv/ks2_pupil_provisional.csv exists in downloads-kts container:
	"""
	Column A,Column B,Column C
	1,2,3
	"""
	And blob storage file LA/100/2024/csv/ks2_pupil_provisional.csv exists in downloads-kts container:
	"""
	Column A,Column B,Column C
	1,2,3
	"""
	When I send a GET request to /api/downloads?scope=LA&scopeId=100&year=2023
	Then I should get a 200 response
	And the response should be an object containing these properties:
	"""
	{
		"Downloads": [
			{
				"Id": "kts-la-ks2-pupil-100-2023-provisional",
				"Source": "Key to success",
				"Label": "KS2 pupil",
				"DatasetType": "Key stage 2 (KS2)",
				"Year": 2023,
				"Version": "Provisional"
			}
		]
	}
	"""

Scenario: LA scope: Should return ASP downloads filtered by year
	Given local authority Test LA (100) exists
	And blob storage file downloads-config.json exists in config container:
	"""
	[
		{
			"id": "asp-la-ks2-la",
			"source": "ASP",
			"scope": "LocalAuthority",
			"dataSetType": "KeyStage2",
			"label": "KS2 LA",
			"filePathPattern": "LA/{code}/{year}/ks2/ks2_la.{filetype}"
		}
	]
	"""
	And blob storage file LA/100/2022/ks2/ks2_la.csv exists in downloads-asp container:
	"""
	Column A,Column B,Column C
	1,2,3
	"""
	And blob storage file LA/100/2023/ks2/ks2_la.csv exists in downloads-asp container:
	"""
	Column A,Column B,Column C
	1,2,3
	"""
	And blob storage file LA/100/2024/ks2/ks2_la.csv exists in downloads-asp container:
	"""
	Column A,Column B,Column C
	1,2,3
	"""
	When I send a GET request to /api/downloads?scope=LA&scopeId=100&year=2023
	Then I should get a 200 response
	And the response should be an object containing these properties:
	"""
	{
		"Downloads": [
			{
				"Id": "asp-la-ks2-la-100-2023",
				"Source": "Analyse school performance",
				"Label": "KS2 LA",
				"DatasetType": "Key stage 2 (KS2)",
				"Year": 2023,
				"Version": null
			}
		]
	}
	"""

Scenario: LA scope: Should return latest KTS and ASP versions
	Given local authority Test LA (100) exists
	And blob storage file downloads-config.json exists in config container:
	"""
	[
		{
			"id": "asp-la-mtc-pupil",
			"source": "ASP",
			"scope": "LocalAuthority",
			"dataSetType": "MTC",
			"label": "Test",
			"filePathPattern": "LA/{code}/{year}/MTC/mtc_pupil.{filetype}"
		},
		{
			"id": "kts-la-ks2-pupil",
			"source": "KTS",
			"scope": "LocalAuthority",
			"dataSetType": "KeyStage2",
			"label": "Test",
			"filePathPattern": "LA/{code}/{year}/{filetype}/ks2_pupil_{version}.{filetype}"
		}
	]
	"""
	And blob storage file LA/100/2024/mtc/mtc_pupil.csv exists in downloads-asp container:
	"""
	Column A,Column B,Column C
	1,2,3
	"""
	And blob storage file LA/100/2024/csv/ks2_pupil_provisional.csv exists in downloads-kts container:
	"""
	Column A,Column B,Column C
	1,2,3
	"""
	And blob storage file LA/100/2024/csv/ks2_pupil_revised.csv exists in downloads-kts container:
	"""
	Column A,Column B,Column C
	1,2,3
	"""
	When I send a GET request to /api/downloads?scope=LA&scopeId=100
	Then I should get a 200 response
	And the response should be an object containing these properties:
	"""
	{
		"Downloads": [
			{
				"Id": "asp-la-mtc-pupil-100-2024",
				"Source": "Analyse school performance",
				"Label": "Test",
				"DatasetType": "Multiplication table check (MTC)",
				"Year": 2024,
				"Version": null
			},
			{
				"Id": "kts-la-ks2-pupil-100-2024-revised",
				"Source": "Key to success",
				"Label": "Test",
				"DatasetType": "Key stage 2 (KS2)",
				"Year": 2024,
				"Version": "Revised"
			}
		]
	}
	"""

Scenario: LA scope: Download configs for different sources that use the same file path pattern should match up correctly
	Given local authority Test LA (100) exists
	And blob storage file downloads-config.json exists in config container:
	"""
	[
		{
			"id": "asp-la-duplicate-file",
			"source": "ASP",
			"scope": "LocalAuthority",
			"dataSetType": "KeyStage2",
			"label": "Test",
			"filePathPattern": "LA/{code}/{year}/{filetype}/file_{version}.{filetype}"
		},
		{
			"id": "kts-la-duplicate-file",
			"source": "KTS",
			"scope": "LocalAuthority",
			"dataSetType": "KeyStage2",
			"label": "Test",
			"filePathPattern": "LA/{code}/{year}/{filetype}/file_{version}.{filetype}"
		}
	]
	"""
	And blob storage file LA/100/2024/csv/file_revised.csv exists in downloads-asp container:
	"""
	Column A,Column B,Column C,Source
	1,2,3,ASP
	"""
	And blob storage file LA/100/2024/csv/file_final.csv exists in downloads-kts container:
	"""
	Column A,Column B,Column C,Source
	1,2,3,KTS
	"""
	When I send a GET request to /api/downloads?scope=LA&scopeId=100
	Then I should get a 200 response
	And the response should be an object containing these properties:
	"""
	{
		"Downloads": [
			{
				"Id": "asp-la-duplicate-file-100-2024-revised",
				"Source": "Analyse school performance",
				"Label": "Test",
				"DatasetType": "Key stage 2 (KS2)",
				"Year": 2024,
				"Version": "Revised"
			},
			{
				"Id": "kts-la-duplicate-file-100-2024-final",
				"Source": "Key to success",
				"Label": "Test",
				"DatasetType": "Key stage 2 (KS2)",
				"Year": 2024,
				"Version": "Final"
			}
		]
	}
	"""

Scenario: LA scope: Should still return results if downloads missing in once source
	Given local authority Test LA (100) exists
	And blob storage file downloads-config.json exists in config container:
	"""
	[
		{
			"id": "asp-la-duplicate-file",
			"source": "ASP",
			"scope": "LocalAuthority",
			"dataSetType": "KeyStage2",
			"label": "Test",
			"filePathPattern": "LA/{code}/{year}/{filetype}/file_{version}.{filetype}"
		},
		{
			"id": "kts-la-duplicate-file",
			"source": "KTS",
			"scope": "LocalAuthority",
			"dataSetType": "KeyStage2",
			"label": "Test",
			"filePathPattern": "LA/{code}/{year}/{filetype}/file_{version}.{filetype}"
		}
	]
	"""
	And blob storage file LA/100/2024/csv/file_revised.csv exists in downloads-asp container:
	"""
	Column A,Column B,Column C,Source
	1,2,3,ASP
	"""
	When I send a GET request to /api/downloads?scope=LA&scopeId=100
	Then I should get a 200 response
	And the response should be an object containing these properties:
	"""
	{
		"Downloads": [
			{
				"Id": "asp-la-duplicate-file-100-2024-revised",
				"Source": "Analyse school performance",
				"Label": "Test",
				"DatasetType": "Key stage 2 (KS2)",
				"Year": 2024,
				"Version": "Revised"
			}
		]
	}
	"""

Scenario: School scope: Should return NotFound (404) response if School does not exist
  Given establishment Test School (123456) exists
  And blob storage file downloads-config.json exists in config container:
  """
  [
    {}
  ]
  """
  When I send a GET request to /api/downloads?scope=School&scopeId=360158
  Then I should get a 404 response 
  And the response should be the message "Not found: Could not find school with URN "360158"."

Scenario: School scope: Should return NotFound (404) response if no downloads exist
  Given establishment Test School (123456) exists
  And blob storage file downloads-config.json exists in config container:
  """
  [
    {
      "id": "kts-school-ks2-pupil",
      "source": "KTS",
      "scope": "School",
      "dataSetType": "KeyStage2",
      "label": "KS2 pupil",
      "filePathPattern": "School/{urn}/{year}/{filetype}/ks2_pupil_{version}.{filetype}"
    }
  ]
  """
  When I send a GET request to /api/downloads?scope=School&scopeId=123456
  Then I should get a 404 response 
  And the response should be the message "Not found: There are no downloads available for School "123456"."

Scenario: School scope: Should return NotFound (404) response if files exist at LA level but not School
Given establishment Test School (123456) exists
  And blob storage file downloads-config.json exists in config container:
  """
  [
    {
      "id": "kts-school-ks2-pupil",
      "source": "KTS",
      "scope": "School",
      "dataSetType": "KeyStage2",
      "label": "KS2 pupil",
      "filePathPattern": "School/{urn}/{year}/{filetype}/ks2_pupil_{version}.{filetype}"
    },
    {
      "id": "kts-la-ks2-pupil",
      "source": "KTS",
      "scope": "LocalAuthority",
      "dataSetType": "KeyStage2",
      "label": "KS2 pupil",
      "filePathPattern": "LA/{laCode}/{year}/{filetype}/ks2_pupil_{version}.{filetype}"
    }
  ]
  """
  And blob storage file LA/100/2024/csv/ks2_pupil_provisional.csv exists in downloads-kts container:
  """
  Column A,Column B,Column C
  1,2,3
  """
  When I send a GET request to /api/downloads?scope=School&scopeId=123456
  Then I should get a 404 response
  And the response should be the message "Not found: There are no downloads available for School "123456"."

Scenario: School scope: Should return NotFound (404) response if files exist at School level but not for given School
  Given establishment Test School (123456) exists
  And blob storage file downloads-config.json exists in config container:
  """
  [
    {
      "id": "kts-school-ks2-pupil",
      "source": "KTS",
      "scope": "School",
      "dataSetType": "KeyStage2",
      "label": "KS2 pupil",
      "filePathPattern": "School/{urn}/{year}/{filetype}/ks2_pupil_{version}.{filetype}"
    }
  ]
  """
  And blob storage file School/345678/2024/csv/ks2_pupil_provisional.csv exists in downloads-kts container:
  """
  Column A, Column B, Column C
  1,2,3
  """
  When I send a GET request to /api/downloads?scope=School&scopeId=123456
  Then I should get a 404 response 
  And the response should be the message "Not found: There are no downloads available for School "123456"."

Scenario: School scope: Should return NotFound (404) response if files exist for School but don't match config filepath pattern
  Given establishment Test School (123456) exists
  And blob storage file downloads-config.json exists in config container:
  """
  [
    {
      "id": "kts-school-ks2-pupil",
      "source": "KTS",
      "scope": "School",
      "dataSetType": "KeyStage2",
      "label": "KS2 pupil",
      "filePathPattern": "School/{urn}/{year}/{filetype}/ks2_pupil_{version}.{filetype}"
    }
  ]
  """
  And blob storage file School/123456/2024/csv/ks4_pupil_provisional.csv exists in downloads-kts container:
  """
  Column A, Column B, Column C
  1,2,3
  """
  And blob storage file School/123456/2024/csv/ks2_pupil_x_provisional.csv exists in downloads-kts container:
  """
  Column A, Column B, Column C
  1,2,3
  """
  When I send a GET request to /api/downloads?scope=School&scopeId=123456
  Then I should get a 404 response 
  And the response should be the message "Not found: There are no downloads available for School "123456"."

Scenario: School scope: Should return KTS downloads for School for all matching years
  Given establishment Test School (123456) exists
  And blob storage file downloads-config.json exists in config container:
  """
  [
    {
      "id": "kts-school-ks2-pupil",
      "source": "KTS",
      "scope": "School",
      "dataSetType": "KeyStage2",
      "label": "KS2 pupil",
      "filePathPattern": "School/{urn}/{year}/{filetype}/ks2_pupil_{version}.{filetype}"
    }
  ]
  """
  And blob storage file School/123456/2023/csv/ks2_pupil_provisional.csv exists in downloads-kts container:
  """
  Column A, Column B, Column C
  1,2,3
  """
  And blob storage file School/123456/2024/csv/ks2_pupil_provisional.csv exists in downloads-kts container:
  """
  Column A, Column B, Column C
  1,2,3
  """
  When I send a GET request to /api/downloads?scope=School&scopeId=123456
  Then I should get a 200 response 
  And the response should be an object containing these properties:
  """
  {
    "Downloads": [
      {
        "Id": "kts-school-ks2-pupil-123456-2023-provisional",
  		"Source": "Key to success",
        "Label": "KS2 pupil",
        "DatasetType": "Key stage 2 (KS2)",
  		"Year": 2023,
  		"Version": "Provisional"
      },
      {
        "Id": "kts-school-ks2-pupil-123456-2024-provisional",
  		"Source": "Key to success",
        "Label": "KS2 pupil",
        "DatasetType": "Key stage 2 (KS2)",
  		"Year": 2024,
  		"Version": "Provisional"
      },
    ]
  }
  """

Scenario: School scope: Should ignore different filetypes within School for the same year
  Given establishment Test School (123456) exists
  And blob storage file downloads-config.json exists in config container:
  """
  [
    {
      "id": "kts-school-ks2-pupil",
      "source": "KTS",
      "scope": "School",
      "dataSetType": "KeyStage2",
      "label": "KS2 pupil",
      "filePathPattern": "School/{urn}/{year}/{filetype}/ks2_pupil_{version}.{filetype}"
    }
  ]
  """
  And blob storage file School/123456/2024/csv/ks2_pupil_provisional.csv exists in downloads-kts container:
  """
  Column A, Column B, Column C
  1,2,3
  """
  And blob storage file School/123456/2024/tsv/ks2_pupil_provisional.tsv exists in downloads-kts container:
  """
  Column A\tColumn B\tColumn C
  1\t2\t3
  """
  When I send a GET request to /api/downloads?scope=School&scopeId=123456
  Then I should get a 200 response 
  And the response should be an object containing these properties:
  """
  {
    "Downloads": [
      {
        "Id": "kts-school-ks2-pupil-123456-2024-provisional",
  		"Source": "Key to success",
        "Label": "KS2 pupil",
        "DatasetType": "Key stage 2 (KS2)",
  		"Year": 2024,
  		"Version": "Provisional"
      }
    ]
  }
  """

Scenario: School scope: Should return user-friendly text for different dataset types
  Given establishment Test School (123456) exists
And blob storage file downloads-config.json exists in config container:
  """
  [
    {
      "id": "test",
      "source": "KTS",
      "scope": "School",
      "dataSetType": "<dataSetType>",
      "label": "Test",
      "filePathPattern": "School/{urn}/{year}/{filetype}/file_{version}.{filetype}"
    }
  ]
  """
  And blob storage file School/123456/2024/csv/file_provisional.csv exists in downloads-kts container:
  """
  Column A, Column B, Column C
  1,2,3
  """
  When I send a GET request to /api/downloads?scope=School&scopeId=123456
  Then I should get a 200 response 
  And the response should be an object containing these properties:
  """
  {
    "Downloads": [
      {
        "Id": "test-123456-2024-provisional",
  		"Source": "Key to success",
        "Label": "Test",
        "DatasetType": "<userFriendlyDataSetType>",
  		"Year": 2024,
  		"Version": "Provisional"
      }
    ]
  }
  """
Examples:
  | dataSetType           | userFriendlyDataSetType |
  | KeyStage2             | Key stage 2 (KS2)       |
  | KeyStage4             | Key stage 4 (KS4)       |
  | Post16                | 16-18                   |
  | QLA                   | QLA (year 6 only)       |
  | SchoolCharacteristics | School characteristics  |
  | Absence               | Absence                 |
  | Exclusions            | Exclusions              |

Scenario: School scope: Should return user-friendly text for different versions
  Given establishment Test School (123456) exists
  And blob storage file downloads-config.json exists in config container:
  """
  [
    {
      "id": "test",
      "source": "KTS",
      "scope": "School",
      "dataSetType": "KeyStage2",
      "label": "Test",
      "filePathPattern": "School/{urn}/{year}/{filetype}/file_{version}.{filetype}"
    }
  ]
  """
  And blob storage file School/123456/2024/csv/file_<version>.csv exists in downloads-kts container:
  """
  Column A, Column B, Column C
  1,2,3
  """
  When I send a GET request to /api/downloads?scope=School&scopeId=123456
  Then I should get a 200 response 
  And the response should be an object containing these properties:
  """
  {
    "Downloads": [
      {
        "Id": "test-123456-2024-<versionDashed>",
  		"Source": "Key to success",
        "Label": "Test",
        "DatasetType": "Key stage 2 (KS2)",
  		"Year": 2024,
  		"Version": "<userFriendlyVersion>"
      }
    ]
  }
  """
Examples:
  | version                 | versionDashed           | userFriendlyVersion      |
  | provisional             | provisional             | Provisional              |
  | provisional_without_cla | provisional-without-cla | Provisional, without CLA |
  | provisional_with_cla    | provisional-with-cla    | Provisional, with CLA    |
  | revised                 | revised                 | Revised                  |
  | revised_without_cla     | revised-without-cla     | Revised, without CLA     |
  | revised_with_cla        | revised-with-cla        | Revised, with CLA        |
  | final                   | final                   | Final                    |
  | final_without_cla       | final-without-cla       | Final, without CLA       |
  | final_with_cla          | final-with-cla          | Final, with CLA          |

Scenario: School scope: Should filter downloads by year
  Given establishment Test School (123456) exists
  And blob storage file downloads-config.json exists in config container:
  """
  [
    {
      "id": "kts-school-ks2-pupil",
      "source": "KTS",
      "scope": "School",
      "dataSetType": "KeyStage2",
      "label": "KS2 pupil",
      "filePathPattern": "School/{urn}/{year}/{filetype}/ks2_pupil_{version}.{filetype}"
    }
  ]
  """
  And blob storage file School/123456/2022/csv/ks2_pupil_provisional.csv exists in downloads-kts container:
  """
  Column A, Column B, Column C
  1,2,3
  """
  And blob storage file School/123456/2023/csv/ks2_pupil_provisional.csv exists in downloads-kts container:
  """
  Column A, Column B, Column C
  1,2,3
  """
  And blob storage file School/123456/2024/csv/ks2_pupil_provisional.csv exists in downloads-kts container:
  """
  Column A, Column B, Column C
  1,2,3
  """
  When I send a GET request to /api/downloads?scope=School&scopeId=123456&year=2023
  Then I should get a 200 response 
  And the response should be an object containing these properties:
  """
  {
    "Downloads": [
      {
        "Id": "kts-school-ks2-pupil-123456-2023-provisional",
  		"Source": "Key to success",
        "Label": "KS2 pupil",
        "DatasetType": "Key stage 2 (KS2)",
  		"Year": 2023,
  		"Version": "Provisional"
      }
    ]
  }
  """