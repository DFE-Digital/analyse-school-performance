Feature: Table component


@Javascript:disabled
Scenario: Table content should display html correctly with a caption
	Given page content "help-test" exists:
		"""
		{
			"Views": [
				{
					"ViewId": "Table",
					"ViewContent": {
						"Id": "test",
						"Caption": "Caption here",
						"Headings": [
							"Header A",
							"Header B"
						],
						"Rows": [
							[
								"Cell A",
								"Cell B"
							],
						]
					},
				}
			]
		}
		"""
	When I navigate to /help/test
	Then I should get a 200 response
	And the element "#test" should have the tag name "table"
	And the element "#test caption" should have the text content "Caption here"
	And the elements "#test thead tr" should total 1
	And the elements "#test thead tr th" should all have the class "govuk-table__header"
	And the elements "#test thead tr th" should have the text contents:
	   | Text     |
	   | Header A |
	   | Header B |
	And the elements "#test tbody tr" should total 1
	And the elements "#test tbody tr td" should all have the class "govuk-table__cell"
	And the elements "#test tbody tr td" should have the text contents:
	   | Text   |
	   | Cell A |
	   | Cell B |

@Javascript:disabled
Scenario: Table caption should be hidden when Caption property is null
	Given page content "help-test" exists:
		"""
		{
			"Views": [
				{
					"ViewId": "Table",
					"ViewContent": {
						"Id": "test",
						"Caption": null,
						"Headings": [
						],
						"Rows": [
						]
					},
				}
			]
		}
		"""
	When I navigate to /help/test
	Then I should get a 200 response
	And the element "#test caption" should not exist

@Javascript:disabled
Scenario: Table caption should be hidden when Caption property is empty
	Given page content "help-test" exists:
		"""
		{
			"Views": [
				{
					"ViewId": "Table",
					"ViewContent": {
						"Id": "test",
						"Caption": "",
						"Headings": [
						],
						"Rows": [
						]
					},
				}
			]
		}
		"""
	When I navigate to /help/test
	Then I should get a 200 response
	And the element "#test caption" should not exist

@Javascript:disabled
Scenario: Table content should display html correctly with headings and rows
	Given page content "help-test" exists:
		"""
		{
			"Views": [
				{
					"ViewId": "Table",
					"ViewContent": {
						"Id": "test",
						"Headings": [
							"Header A",
							"Header B"
						],
						"Rows": [
							[
								"Cell A",
								"Cell B"
							],
							[
								"Cell C",
								"Cell D"
							]
						]
					},
				}
			]
		}
		"""
	When I navigate to /help/test
	Then I should get a 200 response
	And the elements "#test thead tr" should total 1
	And the elements "#test thead tr th" should have the text contents:
	   | Text     |
	   | Header A |
	   | Header B |
	And the elements "#test tbody tr" should total 2
	And the elements "#test tbody tr:nth-child(1) td" should have the text contents:
	   | Text   |
	   | Cell A |
	   | Cell B |
	And the elements "#test tbody tr:nth-child(2) td" should have the text contents:
	   | Text   |
	   | Cell C |
	   | Cell D |

@Javascript:disabled
Scenario: Table content should display html correctly with more row columns than heading columns
	Given page content "help-test" exists:
		"""
		{
			"Views": [
				{
					"ViewId": "Table",
					"ViewContent": {
						"Id": "test",
						"Headings": [
							"Header A",
						],
						"Rows": [
							[
								"Cell A",
								"Cell B"
							],
							[
								"Cell C",
								"Cell D"
							]
						]
					},
				}
			]
		}
		"""
	When I navigate to /help/test
	Then I should get a 200 response
	And the elements "#test thead tr" should total 1
	And the elements "#test thead tr th" should have the text contents:
	   | Text     |
	   | Header A |
	   |          |
	And the elements "#test tbody tr" should total 2
	And the elements "#test tbody tr:nth-child(1) td" should have the text contents:
	   | Text   |
	   | Cell A |
	   | Cell B |
	And the elements "#test tbody tr:nth-child(2) td" should have the text contents:
	   | Text   |
	   | Cell C |
	   | Cell D |

@Javascript:disabled
Scenario: Table content should display html correctly with more heading columns than rows columns
	Given page content "help-test" exists:
		"""
		{
			"Views": [
				{
					"ViewId": "Table",
					"ViewContent": {
						"Id": "test",
						"Headings": [
							"Header A",
							"Header B",
							"Header C"
						],
						"Rows": [
							[
								"Cell A",
							]
						]
					},
				}
			]
		}
		"""
	When I navigate to /help/test
	Then I should get a 200 response
	And the elements "#test thead tr" should total 1
	And the elements "#test thead tr th" should have the text contents:
	   | Text  |
	   | Header A |
	   | Header B |
	   | Header C |
	And the elements "#test tbody tr" should total 1
	And the elements "#test tbody tr td" should total 3
	And the elements "#test tbody tr td" should have the text contents:
	   | Text   |
	   | Cell A |
	   |        |
	   |        |

@Javascript:disabled
Scenario: Table content should display html correctly with no Rows specified
	Given page content "help-test" exists:
		"""
		{
			"Views": [
				{
					"ViewId": "Table",
					"ViewContent": {
						"Id": "test",
						"Headings": [
							"Header A",
							"Header B"
						]
					},
				}
			]
		}
		"""
	When I navigate to /help/test
	Then I should get a 200 response
	And the elements "#test thead tr" should total 1
	And the elements "#test thead tr th" should have the text contents:
	   | Text     |
	   | Header A |
	   | Header B |
	And the elements "#test tbody tr" should total 1
	And the elements "#test tbody tr td" should total 2
	And the elements "#test tbody tr td" should have the text contents:
	   | Text |
	   |      |
	   |      |

@Javascript:disabled
Scenario: Table content should display html correctly with no Headings specified
	Given page content "help-test" exists:
		"""
		{
			"Views": [
				{
					"ViewId": "Table",
					"ViewContent": {
						"Id": "test",
						"Rows": [
							[
								"Cell A",
								"Cell B"
							],
							[
								"Cell C",
								"Cell D"
							]
						]
					},
				}
			]
		}
		"""
	When I navigate to /help/test
	Then I should get a 200 response
	And the elements "#test thead tr" should total 1
	And the elements "#test thead tr th" should have the text contents:
	   | Text |
	   |      |
	   |      |
	And the elements "#test tbody tr" should total 2
	And the elements "#test tbody tr:nth-child(1) td" should have the text contents:
	   | Text   |
	   | Cell A |
	   | Cell B |
	And the elements "#test tbody tr:nth-child(2) td" should have the text contents:
	   | Text   |
	   | Cell C |
	   | Cell D |

@Javascript:disabled
Scenario: Table content should display html correctly with empty rows
	Given page content "help-test" exists:
		"""
		{
			"Views": [
				{
					"ViewId": "Table",
					"ViewContent": {
						"Id": "test",
						"Headings": [
							"Header A",
							"Header B"
						],
						"Rows": [
							[],
							[]
						]
					},
				}
			]
		}
		"""
	When I navigate to /help/test
	Then I should get a 200 response
	And the elements "#test thead tr" should total 1
	And the elements "#test thead tr th" should have the text contents:
	   | Text     |
	   | Header A |
	   | Header B |
	And the elements "#test tbody tr" should total 2
	And the elements "#test tbody tr:nth-child(1) td" should have the text contents:
	   | Text |
	   |      |
	   |      |
	And the elements "#test tbody tr:nth-child(2) td" should have the text contents:
	   | Text |
	   |      |
	   |      |

@Javascript:disabled
Scenario: Table content should display html correctly with empty headings
	Given page content "help-test" exists:
		"""
		{
			"Views": [
				{
					"ViewId": "Table",
					"ViewContent": {
						"Id": "test",
						"Headings": [
						
						],
						"Rows": [
							[
								"Cell A",
								"Cell B"
							]
						]
					},
				}
			]
		}
		"""
	When I navigate to /help/test
	Then I should get a 200 response
	And the elements "#test thead tr" should total 1
	And the elements "#test thead tr th" should have the text contents:
	   | Text |
	   |      |
	   |      |
	And the elements "#test tbody tr" should total 1
	And the elements "#test tbody tr td" should have the text contents:
	   | Text   |
	   | Cell A |
	   | Cell B |

@Javascript:disabled
Scenario: Table content should display html correctly with Row having null value
	Given page content "help-test" exists:
		"""
		{
			"Views": [
				{
					"ViewId": "Table",
					"ViewContent": {
						"Id": "test",
						"Headings": [
							"Header A",
							"Header B"
						],
						"Rows": [
							[
								null
							],
						]
					},
				}
			]
		}
		"""
	When I navigate to /help/test
	Then I should get a 200 response
	And the elements "#test thead tr" should total 1
	And the elements "#test thead tr th" should have the text contents:
	   | Text     |
	   | Header A |
	   | Header B |
	And the elements "#test tbody tr" should total 1
	And the elements "#test tbody tr td" should have the text contents:
	   | Text |
	   |      |
	   |      |

@Javascript:disabled
Scenario: Table content should display html correctly with Row as null
	Given page content "help-test" exists:
		"""
		{
			"Views": [
				{
					"ViewId": "Table",
					"ViewContent": {
						"Id": "test",
						"Headings": [
							"Header A",
							"Header B"
						],
						"Rows": null
					},
				}
			]
		}
		"""
	When I navigate to /help/test
	Then I should get a 200 response
	And the elements "#test thead tr" should total 1
	And the elements "#test thead tr th" should have the text contents:
	   | Text     |
	   | Header A |
	   | Header B |
	And the elements "#test tbody tr" should total 1
	And the elements "#test tbody tr td" should have the text contents:
	   | Text |
	   |      |
	   |      |

@Javascript:disabled
Scenario: Table content should display html correctly with Headings as null
	Given page content "help-test" exists:
		"""
		{
			"Views": [
				{
					"ViewId": "Table",
					"ViewContent": {
						"Id": "test",
						"Headings": null,
						"Rows": [
							[
								"Cell A",
								"Cell B"
							]
						]
					},
				}
			]
		}
		"""
	When I navigate to /help/test
	Then I should get a 200 response
	And the elements "#test thead tr" should total 1
	And the elements "#test thead tr th" should have the text contents:
	   | Text |
	   |      |
	   |      |
	And the elements "#test tbody tr" should total 1
	And the elements "#test tbody tr td" should have the text contents:
	   | Text   |
	   | Cell A |
	   | Cell B |

@Javascript:disabled
Scenario: Table content should display html correctly with Headings having null value
	Given page content "help-test" exists:
		"""
		{
			"Views": [
				{
					"ViewId": "Table",
					"ViewContent": {
						"Id": "test",
						"Headings": [
							null
						],
						"Rows": [
							[
								"Cell A",
								"Cell B"
							]
						]
					},
				}
			]
		}
		"""
	When I navigate to /help/test
	Then I should get a 200 response
	And the elements "#test thead tr" should total 1
	And the elements "#test thead tr th" should have the text contents:
	   | Text |
	   |      |
	   |      |
	And the elements "#test tbody tr" should total 1
	And the elements "#test tbody tr td" should have the text contents:
	   | Text   |
	   | Cell A |
	   | Cell B |

@Javascript:disabled
Scenario: Table content should display first column as header when FirstCellAsHeader is true
	Given page content "help-test" exists:
		"""
		{
			"Views": [
				{
					"ViewId": "Table",
					"ViewContent": {
						"Id": "test",
						"Headings": [
							"Header A",
							"Header B"
						],
						"Rows": [
							[
								"Cell A",
								"Cell B"
							],
							[
								"Cell C",
								"Cell D"
							]
						],
						"FirstCellAsHeader": true
					},
				}
			]
		}
		"""
	When I navigate to /help/test
	Then I should get a 200 response
	And the elements "#test thead tr th" should all have the class "govuk-table__header"
	And the elements "#test tbody tr td:nth-child(1)" should all have the class "govuk-table__header"
	And the elements "#test tbody tr td:nth-child(2)" should all have the class "govuk-table__cell"

@Javascript:disabled
Scenario: Table content should not display first column as header when FirstCellAsHeader is false
	Given page content "help-test" exists:
		"""
		{
			"Views": [
				{
					"ViewId": "Table",
					"ViewContent": {
						"Id": "test",
						"Headings": [
							"Header A",
							"Header B"
						],
						"Rows": [
							[
								"Cell A",
								"Cell B"
							],
							[
								"Cell C",
								"Cell D"
							]
						],
						"FirstCellAsHeader": false
					},
				}
			]
		}
		"""
	When I navigate to /help/test
	Then I should get a 200 response
	And the elements "#test thead tr th" should all have the class "govuk-table__header"
	And the elements "#test tbody tr td" should all have the class "govuk-table__cell"

@Javascript:disabled
Scenario: Table content should not display first column as header when FirstCellAsHeader is missing
	Given page content "help-test" exists:
		"""
		{
			"Views": [
				{
					"ViewId": "Table",
					"ViewContent": {
						"Id": "test",
						"Headings": [
							"Header A",
							"Header B"
						],
						"Rows": [
							[
								"Cell A",
								"Cell B"
							],
							[
								"Cell C",
								"Cell D"
							]
						]
					},
				}
			]
		}
		"""
	When I navigate to /help/test
	Then I should get a 200 response
	And the elements "#test thead tr th" should all have the class "govuk-table__header"
	And the elements "#test tbody tr td" should all have the class "govuk-table__cell"

@Javascript:disabled
Scenario: Table content should not display first column as header when FirstCellAsHeader is not a boolean value
	Given page content "help-test" exists:
		"""
		{
			"Views": [
				{
					"ViewId": "Table",
					"ViewContent": {
						"Id": "test",
						"Headings": [
							"Header A",
							"Header B"
						],
						"Rows": [
							[
								"Cell A",
								"Cell B"
							],
							[
								"Cell C",
								"Cell D"
							]
						],
						"FirstCellAsHeader": <Value>
					},
				}
			]
		}
		"""
	When I navigate to /help/test
	Then I should get a 200 response
	And the elements "#test thead tr th" should all have the class "govuk-table__header"
	And the elements "#test tbody tr td" should all have the class "govuk-table__cell"

Examples:
	| Value  |
	| "true" |
	| 12345  |
	| null   |