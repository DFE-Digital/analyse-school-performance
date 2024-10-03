Feature: Blob storage example tests

Scenario: If file is created then it should exist
    Given blob storage file School/123456/2024/test.json exists in ASP container:
    """
    Hello this is the file
    """
    Then blob storage file School/123456/2024/test.json should exist in ASP container:
    """
    Hello this is the file
    """