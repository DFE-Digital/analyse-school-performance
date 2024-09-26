Feature: GetAvailableLADownloads
 
 Scenario: Should not accept POST method
    When I send a POST request to /api/GetAvailableLADownloads
    Then I should get a 405 response
    And the response should be the message "Method not allowed: The HTTP method POST is not allowed."
    And the response should include the header "Allow: GET"

 Scenario: Should return BadRequest (400) response if searchTerm parameter is missing
    When I send a GET request to /api/GetAvailableLADownloads
    Then I should get a 400 response
    And the response should be the message "Bad request: The parameter "laCode" is missing."

Scenario: Should return BadRequest (400) response if searchTerm parameter is empty string
    When I send a GET request to /api/GetAvailableLADownloads?laCode=
    Then I should get a 400 response
    And the response should be the message "Bad request: The parameter "laCode" should not be empty."

Scenario Outline: Should return 200 response with laCode reports response
    When I send a GET request to /api/GetAvailableLADownloads?laCode=123
    Then I should get a 200 response
    And the response should be an object containing these properties excluding null:
    """
    {
        "LaCode": "123",
        "Downloads": [
            {
                "Id": "kts-123-phonics-la-2022-final-pupil",
                "Name": "Phonics LA pupil data",
                "DownloadSource": "Key to success",
                "Year": 2022,
                "DatasetType": "Phonics",
                "ReleaseVersion": "Final"
            },
            {
                "Id": "kts-123-phonics-la-2023-final-pupil",
                "Name": "Phonics LA pupil data",
                "DownloadSource": "Key to success",
                "Year": 2023,
                "DatasetType": "Phonics",
                "ReleaseVersion": "Final"
            },
            {
                "Id": "kts-123-phonics-la-2024-provisional-pupil",
                "Name": "Phonics LA pupil data",
                "DownloadSource": "Key to success",
                "Year": 2024,
                "DatasetType": "Phonics",
                "ReleaseVersion": "Provisional"
            },
            {
                "Id": "asp-123-phonics-la-2024-revised-pupil",
                "Name": "Phonics LA pupil data",
                "DownloadSource": "Analyse school performance",
                "Year": 2024,
                "DatasetType": "Phonics",
                "ReleaseVersion": "Revised"
            },
            {
                "Id": "kts-123-ks2-la-2022-final",
                "Name": "Key stage 2 LA data",
                "DownloadSource": "Key to success",
                "Year": 2022,
                "DatasetType": "Key stage 2",
                "ReleaseVersion": "Final"
            },
            {
                "Id": "kts-123-ks2-la-2023-final",
                "Name": "Key stage 2 LA data",
                "DownloadSource": "Key to success",
                "Year": 2023,
                "DatasetType": "Key stage 2",
                "ReleaseVersion": "Final"
            },
            {
                "Id": "kts-123-ks2-la-2024-revised",
                "Name": "Key stage 2 LA data",
                "DownloadSource": "Key to success",
                "Year": 2024,
                "DatasetType": "Key stage 2",
                "ReleaseVersion": "Revised"
            },
            {
                "Id": "asp-123-ks2-la-2022-provisional",
                "Name": "Key stage 2 LA data",
                "DownloadSource": "Analyse school performance",
                "Year": 2022,
                "DatasetType": "Key stage 2",
                "ReleaseVersion": "Provisional"
            },
            {
                "Id": "asp-123-ks2-la-2023-provisional",
                "Name": "Key stage 2 LA data",
                "DownloadSource": "Analyse school performance",
                "Year": 2023,
                "DatasetType": "Key stage 2",
                "ReleaseVersion": "Provisional"
            },
            {
                "Id": "kts-123-ks4-la-2022-final-pupil",
                "Name": "Key stage 4 LA pupil data",
                "DownloadSource": "Key to success",
                "Year": 2022,
                "DatasetType": "Key stage 4",
                "ReleaseVersion": "Final"
            },
            {
                "Id": "kts-123-ks4-la-2023-revised-pupil",
                "Name": "Key stage 4 LA pupil data",
                "DownloadSource": "Key to success",
                "Year": 2023,
                "DatasetType": "Key stage 4",
                "ReleaseVersion": "Revised"
            },
            {
                "Id": "asp-123-ks4-la-2022-final-pupil",
                "Name": "Key stage 4 LA pupil data",
                "DownloadSource": "Analyse school performance",
                "Year": 2022,
                "DatasetType": "Key stage 4",
                "ReleaseVersion": "Final"
            },
            {
                "Id": "asp-123-ks4-la-2023-final-pupil",
                "Name": "Key stage 4 LA pupil data",
                "DownloadSource": "Analyse school performance",
                "Year": 2023,
                "DatasetType": "Key stage 4",
                "ReleaseVersion": "Final"
            },
            {
                "Id": "asp-123-ks4-la-2024-provisional-pupil",
                "Name": "Key stage 4 LA pupil data",
                "DownloadSource": "Analyse school performance",
                "Year": 2024,
                "DatasetType": "Key stage 4",
                "ReleaseVersion": "Provisional"
            }
        ]
    }
    """
Scenario Outline: Should return 200 response with LA download data with year parameter filter 
    When I send a GET request to /api/GetAvailableLADownloads?laCode=123&year=2023
    Then I should get a 200 response
    And the response should be an object containing these properties excluding null:
    """
    {
        "LaCode": "123",
        "Year": 2023,
        "Downloads": [
            {
                "Id": "kts-123-phonics-la-2023-final-pupil",
                "Name": "Phonics LA pupil data",
                "DownloadSource": "Key to success",
                "Year": 2023,
                "DatasetType": "Phonics",
                "ReleaseVersion": "Final"
            },
            {
                "Id": "kts-123-ks2-la-2023-final",
                "Name": "Key stage 2 LA data",
                "DownloadSource": "Key to success",
                "Year": 2023,
                "DatasetType": "Key stage 2",
                "ReleaseVersion": "Final"
            },
            {
                "Id": "asp-123-ks2-la-2023-provisional",
                "Name": "Key stage 2 LA data",
                "DownloadSource": "Analyse school performance",
                "Year": 2023,
                "DatasetType": "Key stage 2",
                "ReleaseVersion": "Provisional"
            },
            {
                "Id": "kts-123-ks4-la-2023-revised-pupil",
                "Name": "Key stage 4 LA pupil data",
                "DownloadSource": "Key to success",
                "Year": 2023,
                "DatasetType": "Key stage 4",
                "ReleaseVersion": "Revised"
            },
            {
                "Id": "asp-123-ks4-la-2023-final-pupil",
                "Name": "Key stage 4 LA pupil data",
                "DownloadSource": "Analyse school performance",
                "Year": 2023,
                "DatasetType": "Key stage 4",
                "ReleaseVersion": "Final"
            }
        ]
    }
    """
    
Scenario: Should return BadRequest (400) response if year parameter is not a number
    When I send a GET request to /api/GetAvailableLADownloads?laCode=301&year=xxxx
    Then I should get a 400 response
    And the response should be the message "Bad request: The parameter "year" should be a whole number greater than or equal to 1."

Scenario: Should return BadRequest (400) response if year parameter is not 4 characters
    When I send a GET request to /api/GetAvailableLADownloads?laCode=301&year=<year>
    Then I should get a 400 response
    And the response should be the message "Bad request: The parameter "year" must be exactly 4 characters long."

Examples:
  | year |
  | 123  |
  | 12345 |  
  
Scenario: Should return NotFound (404) response if no downloads exist for the given year
    When I send a GET request to /api/GetAvailableLADownloads?laCode=301&year=2000
    Then I should get a 404 response
    And the response should be the message "Not found: there are no downloads available for Local Authority "301" for the given year."    