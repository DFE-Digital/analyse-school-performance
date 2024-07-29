Feature: GetAvailableSchoolDownloads

 Scenario: Should return BadRequest (400) response if school URN parameter is missing
    When I send a GET request to /api/GetAvailableSchoolDownloads
    Then I should get a 400 response
    And the response should be the message "Invalid: The parameter "urn" is missing."

 Scenario: Should return BadRequest (400) response if school URN parameter is not 6 characters
    When I send a GET request to /api/GetAvailableSchoolDownloads?urn=<urn>
    Then I should get a 400 response
    And the response should be the message "Invalid: The parameter "urn" must be exactly 6 characters long."

Examples: 
    | urn     |
    | 12345   |
    | 1234567 |

 Scenario: Should return BadRequest (400) response if school URN parameter is not all digits
    When I send a GET request to /api/GetAvailableSchoolDownloads?urn=<urn>
    Then I should get a 400 response
    And the response should be the message "Invalid: The parameter "urn" must contain only digits."

Examples: 
    | urn    |
    | 12345a |
    | 12345= |
    | 12345- |
    | abcdef |
    | -12345 |
    | ////// |

Scenario: Should not accept POST method
    When I send a POST request to /api/GetAvailableSchoolDownloads?urn=123456
    Then I should get a 405 response
    And the response should be the message "Method not allowed: The HTTP method POST is not allowed."
    And the response should include the header "Allow: GET"

Scenario Outline: Should return 200 response with school download data
    When I send a GET request to /api/GetAvailableSchoolDownloads?urn=<urn>
    Then I should get a 200 response
    And the response should be an object containing these properties excluding null:
        """
        {
            "Urn": "123456",
            "DownloadItems": [
                {
                    "Id": "kts-phonics-pupil-123456-2022-final",
                    "Name": "Phonics pupil data",
                    "DownloadSource": "Key to success",
                    "Year": "2022",
                    "DatasetType": "Phonics",
                    "ReleaseVersion": "Final"
                },
                {
                    "Id": "kts-phonics-pupil-123456-2023-final",
                    "Name": "Phonics pupil data",
                    "DownloadSource": "Key to success",
                    "Year": "2023",
                    "DatasetType": "Phonics",
                    "ReleaseVersion": "Final"
                },
                {
                    "Id": "kts-phonics-pupil-123456-2024-provisional",
                    "Name": "Phonics pupil data",
                    "DownloadSource": "Key to success",
                    "Year": "2024",
                    "DatasetType": "Phonics",
                    "ReleaseVersion": "Provisional"
                },
                {
                    "Id": "asp-phonics-pupil-123456-2024-revised",
                    "Name": "Phonics pupil data",
                    "DownloadSource": "Analyse school performance",
                    "Year": "2024",
                    "DatasetType": "Phonics",
                    "ReleaseVersion": "Revised"
                },
                {
                    "Id": "kts-ks2-school-123456-2022-final",
                    "Name": "Key stage 2 school data",
                    "DownloadSource": "Key to success",
                    "Year": "2022",
                    "DatasetType": "Key stage 2",
                    "ReleaseVersion": "Final"
                },
                {
                    "Id": "kts-ks2-school-123456-2023-final",
                    "Name": "Key stage 2 school data",
                    "DownloadSource": "Key to success",
                    "Year": "2023",
                    "DatasetType": "Key stage 2",
                    "ReleaseVersion": "Final"
                },
                {
                    "Id": "kts-ks2-school-123456-2024-revised",
                    "Name": "Key stage 2 school data",
                    "DownloadSource": "Key to success",
                    "Year": "2024",
                    "DatasetType": "Key stage 2",
                    "ReleaseVersion": "Revised"
                },
                {
                    "Id": "asp-ks2-school-123456-2022-provisional",
                    "Name": "Key stage 2 school data",
                    "DownloadSource": "Analyse school performance",
                    "Year": "2022",
                    "DatasetType": "Key stage 2",
                    "ReleaseVersion": "Provisional"
                },
                {
                    "Id": "asp-ks2-school-123456-2023-provisional",
                    "Name": "Key stage 2 school data",
                    "DownloadSource": "Analyse school performance",
                    "Year": "2023",
                    "DatasetType": "Key stage 2",
                    "ReleaseVersion": "Provisional"
                },
                {
                    "Id": "kts-ks4-pupil-123456-2022-final",
                    "Name": "Key stage 4 pupil data",
                    "DownloadSource": "Key to success",
                    "Year": "2022",
                    "DatasetType": "Key stage 4",
                    "ReleaseVersion": "Final"
                },
                {
                    "Id": "kts-ks4-pupil-123456-2023-revised",
                    "Name": "Key stage 4 pupil data",
                    "DownloadSource": "Key to success",
                    "Year": "2023",
                    "DatasetType": "Key stage 4",
                    "ReleaseVersion": "Revised"
                },
                {
                    "Id": "asp-ks4-pupil-123456-2022-final",
                    "Name": "Key stage 4 pupil data",
                    "DownloadSource": "Analyse school performance",
                    "Year": "2022",
                    "DatasetType": "Key stage 4",
                    "ReleaseVersion": "Final"
                },
                {
                    "Id": "asp-ks4-pupil-123456-2023-final",
                    "Name": "Key stage 4 pupil data",
                    "DownloadSource": "Analyse school performance",
                    "Year": "2023",
                    "DatasetType": "Key stage 4",
                    "ReleaseVersion": "Final"
                },
                {
                    "Id": "asp-ks4-pupil-123456-2024-provisional",
                    "Name": "Key stage 4 pupil data",
                    "DownloadSource": "Analyse school performance",
                    "Year": "2024",
                    "DatasetType": "Key stage 4",
                    "ReleaseVersion": "Provisional"
                }
            ]
        }
        """

Examples: 
    | urn    |
    | 123456 |
