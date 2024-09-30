Feature: GetAvailableSchoolDownloads

 Scenario: Should return BadRequest (400) response if school URN parameter is missing
    When I send a GET request to /api/GetAvailableSchoolDownloads
    Then I should get a 400 response
    And the response should be the message "Bad request: The parameter "urn" is missing."

 Scenario: Should return BadRequest (400) response if school URN parameter is not 6 characters
    When I send a GET request to /api/GetAvailableSchoolDownloads?urn=<urn>
    Then I should get a 400 response
    And the response should be the message "Bad request: The parameter "urn" must be exactly 6 characters long."

Examples: 
    | urn     |
    | 12345   |
    | 1234567 |

 Scenario: Should return BadRequest (400) response if school URN parameter is not all digits
    When I send a GET request to /api/GetAvailableSchoolDownloads?urn=<urn>
    Then I should get a 400 response
    And the response should be the message "Bad request: The parameter "urn" must contain only digits."

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

Scenario Outline: Should return 200 response with school download data without year parameter
    When I send a GET request to /api/GetAvailableSchoolDownloads?urn=<urn>
    Then I should get a 200 response
    And the response should be an object containing these properties excluding null:
        """
        {
            "Urn": "123456",
            "Downloads": [
                {
                    "Id": "kts-123456-phonics-2022-final-pupil",
                    "Label": "Phonics pupil data",
                    "Source": "Key to success",
                    "Year": 2022,
                    "DatasetType": "Phonics",
                    "Version": "Final"
                },
                {
                    "Id": "kts-123456-phonics-2023-final-pupil",
                    "Label": "Phonics pupil data",
                    "Source": "Key to success",
                    "Year": 2023,
                    "DatasetType": "Phonics",
                    "Version": "Final"
                },
                {
                    "Id": "kts-123456-phonics-2024-provisional-pupil",
                    "Label": "Phonics pupil data",
                    "Source": "Key to success",
                    "Year": 2024,
                    "DatasetType": "Phonics",
                    "Version": "Provisional"
                },
                {
                    "Id": "asp-123456-phonics-2024-revised-pupil",
                    "Label": "Phonics pupil data",
                    "Source": "Analyse school performance",
                    "Year": 2024,
                    "DatasetType": "Phonics",
                    "Version": "Revised"
                },
                {
                    "Id": "kts-123456-ks2-2022-final-school",
                    "Label": "Key stage 2 school data",
                    "Source": "Key to success",
                    "Year": 2022,
                    "DatasetType": "Key stage 2",
                    "Version": "Final"
                },
                {
                    "Id": "kts-123456-ks2-2023-final-school",
                    "Label": "Key stage 2 school data",
                    "Source": "Key to success",
                    "Year": 2023,
                    "DatasetType": "Key stage 2",
                    "Version": "Final"
                },
                {
                    "Id": "kts-123456-ks2-2024-revised-school",
                    "Label": "Key stage 2 school data",
                    "Source": "Key to success",
                    "Year": 2024,
                    "DatasetType": "Key stage 2",
                    "Version": "Revised"
                },
                {
                    "Id": "asp-123456-ks2-2022-provisional-school",
                    "Label": "Key stage 2 school data",
                    "Source": "Analyse school performance",
                    "Year": 2022,
                    "DatasetType": "Key stage 2",
                    "Version": "Provisional"
                },
                {
                    "Id": "asp-123456-ks2-2023-provisional-school",
                    "Label": "Key stage 2 school data",
                    "Source": "Analyse school performance",
                    "Year": 2023,
                    "DatasetType": "Key stage 2",
                    "Version": "Provisional"
                },
                {
                    "Id": "kts-123456-ks4-2022-final-pupil",
                    "Label": "Key stage 4 pupil data",
                    "Source": "Key to success",
                    "Year": 2022,
                    "DatasetType": "Key stage 4",
                    "Version": "Final"
                },
                {
                    "Id": "kts-123456-ks4-2023-revised-pupil",
                    "Label": "Key stage 4 pupil data",
                    "Source": "Key to success",
                    "Year": 2023,
                    "DatasetType": "Key stage 4",
                    "Version": "Revised"
                },
                {
                    "Id": "asp-123456-ks4-2022-final-pupil",
                    "Label": "Key stage 4 pupil data",
                    "Source": "Analyse school performance",
                    "Year": 2022,
                    "DatasetType": "Key stage 4",
                    "Version": "Final"
                },
                {
                    "Id": "asp-123456-ks4-2023-final-pupil",
                    "Label": "Key stage 4 pupil data",
                    "Source": "Analyse school performance",
                    "Year": 2023,
                    "DatasetType": "Key stage 4",
                    "Version": "Final"
                },
                {
                    "Id": "asp-123456-ks4-2024-provisional-pupil",
                    "Label": "Key stage 4 pupil data",
                    "Source": "Analyse school performance",
                    "Year": 2024,
                    "DatasetType": "Key stage 4",
                    "Version": "Provisional"
                }
            ]
        }
        """

Examples: 
    | urn    |
    | 123456 |

Scenario Outline: Should return 200 response with school download data with year parameter filter 
    When I send a GET request to /api/GetAvailableSchoolDownloads?urn=<urn>&year=2023
    Then I should get a 200 response
    And the response should be an object containing these properties excluding null:
        """
        {
            "Urn": "123456",
            "Year": 2023,
            "Downloads": [
                {
                    "Id": "kts-123456-phonics-2023-final-pupil",
                    "Label": "Phonics pupil data",
                    "Source": "Key to success",
                    "Year": 2023,
                    "DatasetType": "Phonics",
                    "Version": "Final"
                },
                {
                    "Id": "kts-123456-ks2-2023-final-school",
                    "Label": "Key stage 2 school data",
                    "Source": "Key to success",
                    "Year": 2023,
                    "DatasetType": "Key stage 2",
                    "Version": "Final"
                },
                {
                    "Id": "asp-123456-ks2-2023-provisional-school",
                    "Label": "Key stage 2 school data",
                    "Source": "Analyse school performance",
                    "Year": 2023,
                    "DatasetType": "Key stage 2",
                    "Version": "Provisional"
                },
                {
                    "Id": "kts-123456-ks4-2023-revised-pupil",
                    "Label": "Key stage 4 pupil data",
                    "Source": "Key to success",
                    "Year": 2023,
                    "DatasetType": "Key stage 4",
                    "Version": "Revised"
                },
                {
                    "Id": "asp-123456-ks4-2023-final-pupil",
                    "Label": "Key stage 4 pupil data",
                    "Source": "Analyse school performance",
                    "Year": 2023,
                    "DatasetType": "Key stage 4",
                    "Version": "Final"
                }
            ]
        }
        """

Examples:
  | urn    |
  | 123456 |
  
Scenario: Should return BadRequest (400) response if year parameter is not a number
    When I send a GET request to /api/GetAvailableSchoolDownloads?urn=123456&year=xxxx
    Then I should get a 400 response
    And the response should be the message "Bad request: The parameter "year" should be a whole number greater than or equal to 1."
    
Scenario: Should return BadRequest (400) response if year parameter is not 4 characters
    When I send a GET request to /api/GetAvailableSchoolDownloads?urn=123456&year=<year>
    Then I should get a 400 response
    And the response should be the message "Bad request: The parameter "year" must be exactly 4 characters long."
    
Examples:
  | year  |
  | 123   |
  | 12345 |   
  
Scenario: Should return NotFound (404) response if no downloads exist for the given year
    When I send a GET request to /api/GetAvailableSchoolDownloads?urn=123456&year=2000
    Then I should get a 404 response
    And the response should be the message "Not found: there are no downloads available for Establishment "123456" for the given year."     