Feature: La Downloads
 
 Scenario: Should not accept POST method
    When I send a POST request to /api/GetAvailableLaDownloads
    Then I should get a 405 response
    And the response should be the message "Method not allowed: The HTTP method POST is not allowed."
    And the response should include the header "Allow: GET"

 Scenario: Should return BadRequest (400) response if searchTerm parameter is missing
    When I send a GET request to /api/GetAvailableLaDownloads
    Then I should get a 400 response
    And the response should be the message "Invalid: The parameter "laCode" is missing."

Scenario: Should return BadRequest (400) response if searchTerm parameter is empty string
    When I send a GET request to /api/GetAvailableLaDownloads?laCode=
    Then I should get a 400 response
    And the response should be the message "Invalid: The parameter "laCode" should not be empty."

Scenario Outline: Should return 200 response with laCode reports response
    When I send a GET request to /api/GetAvailableLaDownloads?laCode=301
    Then I should get a 200 response
    And the response should be an object containing these properties excluding null:
        """
        {
    "LaCode": "301",
    "Downloads": [
        {
            "Id": "kts-phonics-la-pupil-2022-final",
            "Name": "Phonics LA pupil data",
            "DownloadSource": "Key to success",
            "Year": "2022",
            "DatasetType": "Phonics",
            "ReleaseVersion": "Final"
        },
        {
            "Id": "kts-phonics-la-pupil-2023-final",
            "Name": "Phonics LA pupil data",
            "DownloadSource": "Key to success",
            "Year": "2023",
            "DatasetType": "Phonics",
            "ReleaseVersion": "Final"
        },
        {
            "Id": "kts-phonics-la-pupil-2024-provisional",
            "Name": "Phonics LA pupil data",
            "DownloadSource": "Key to success",
            "Year": "2024",
            "DatasetType": "Phonics",
            "ReleaseVersion": "Provisional"
        },
        {
            "Id": "asp-phonics-la-pupil-2024-revised",
            "Name": "Phonics LA pupil data",
            "DownloadSource": "Analyse school performance",
            "Year": "2024",
            "DatasetType": "Phonics",
            "ReleaseVersion": "Revised"
        },
        {
            "Id": "kts-ks2-la-2022-final",
            "Name": "Key stage 2 LA data",
            "DownloadSource": "Key to success",
            "Year": "2022",
            "DatasetType": "Key stage 2",
            "ReleaseVersion": "Final"
        },
        {
            "Id": "kts-ks2-la-2023-final",
            "Name": "Key stage 2 LA data",
            "DownloadSource": "Key to success",
            "Year": "2023",
            "DatasetType": "Key stage 2",
            "ReleaseVersion": "Final"
        },
        {
            "Id": "kts-ks2-la-2024-revised",
            "Name": "Key stage 2 LA data",
            "DownloadSource": "Key to success",
            "Year": "2024",
            "DatasetType": "Key stage 2",
            "ReleaseVersion": "Revised"
        },
        {
            "Id": "asp-ks2-la-2022-provisional",
            "Name": "Key stage 2 LA data",
            "DownloadSource": "Analyse school performance",
            "Year": "2022",
            "DatasetType": "Key stage 2",
            "ReleaseVersion": "Provisional"
        },
        {
            "Id": "asp-ks2-la-2023-provisional",
            "Name": "Key stage 2 LA data",
            "DownloadSource": "Analyse school performance",
            "Year": "2023",
            "DatasetType": "Key stage 2",
            "ReleaseVersion": "Provisional"
        },
        {
            "Id": "kts-ks4-la-pupil-2022-final",
            "Name": "Key stage 4 LA pupil data",
            "DownloadSource": "Key to success",
            "Year": "2022",
            "DatasetType": "Key stage 4",
            "ReleaseVersion": "Final"
        },
        {
            "Id": "kts-ks4-la-pupil-2023-revised",
            "Name": "Key stage 4 LA pupil data",
            "DownloadSource": "Key to success",
            "Year": "2023",
            "DatasetType": "Key stage 4",
            "ReleaseVersion": "Revised"
        },
        {
            "Id": "asp-ks4-la-pupil-2022-final",
            "Name": "Key stage 4 LA pupil data",
            "DownloadSource": "Analyse school performance",
            "Year": "2022",
            "DatasetType": "Key stage 4",
            "ReleaseVersion": "Final"
        },
        {
            "Id": "asp-ks4-la-pupil-2023-final",
            "Name": "Key stage 4 LA pupil data",
            "DownloadSource": "Analyse school performance",
            "Year": "2023",
            "DatasetType": "Key stage 4",
            "ReleaseVersion": "Final"
        },
        {
            "Id": "asp-ks4-la-pupil-2024-provisional",
            "Name": "Key stage 4 LA pupil data",
            "DownloadSource": "Analyse school performance",
            "Year": "2024",
            "DatasetType": "Key stage 4",
            "ReleaseVersion": "Provisional"
        }
    ]
}
        """
