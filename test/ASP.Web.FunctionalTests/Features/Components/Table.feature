Feature: Table component

The Table component provides the ability to add tables to content templates.

For more information please view the [technical specification for this component](https://dev.azure.com/dfe-ssp/s192-Analyse-School-Performance%20%28ASP%29/_wiki/wikis/s192-Analyse-School-Performance-%28ASP%29.wiki/14453/Table).

@Javascript:disabled
Scenario: Table content should display html correctly with a caption
	Given a content template contains the component:
		"""
		{
			"ViewId": "Table",
			"ViewContent": {
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
		"""
	When I view the component on the page
	Then there should be no errors
	And the component outer element should have the tag name "table"
	And the element "caption" within the component should have the text content "Caption here"
	And the elements "thead tr" within the component should total 1
	And the elements "thead tr th" within the component should all have the class "govuk-table__header"
	And the elements "thead tr th" within the component should have the text contents:
		| Text     |
		| Header A |
		| Header B |
	And the elements "tbody tr" within the component should total 1
	And the elements "tbody tr td" within the component should all have the class "govuk-table__cell"
	And the elements "tbody tr td" within the component should have the text contents:
		| Text   |
		| Cell A |
		| Cell B |

@Javascript:disabled
Scenario: Table caption should be hidden when Caption property is null
	Given a content template contains the component:
		"""
		{
			"ViewId": "Table",
			"ViewContent": {
				"Caption": null,
				"Headings": [
				],
				"Rows": [
				]
			},
		}
		"""
	When I view the component on the page
	Then there should be no errors
	And the element "caption" within the component should not exist

@Javascript:disabled
Scenario: Table caption should be hidden when Caption property is empty
	Given a content template contains the component:
		"""
		{
			"ViewId": "Table",
			"ViewContent": {
				"Caption": "",
				"Headings": [
				],
				"Rows": [
				]
			},
		}
		"""
	When I view the component on the page
	Then there should be no errors
	And the element "caption" within the component should not exist

@Javascript:disabled
Scenario: Table content should display html correctly with headings and rows
	Given a content template contains the component:
		"""
		{
			"ViewId": "Table",
			"ViewContent": {
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
			}
		}
		"""
	When I view the component on the page
	Then there should be no errors
	And the elements "thead tr" within the component should total 1
	And the elements "thead tr th" within the component should have the text contents:
		| Text     |
		| Header A |
		| Header B |
	And the elements "tbody tr" within the component should total 2
	And the elements "tbody tr:nth-child(1) td" within the component should have the text contents:
		| Text   |
		| Cell A |
		| Cell B |
	And the elements "tbody tr:nth-child(2) td" within the component should have the text contents:
		| Text   |
		| Cell C |
		| Cell D |

@Javascript:disabled
Scenario: Table content should display html correctly with headings and rows when using markdown
	Given a content template contains the component:
		"""
		{
			"ViewId": "Table",
			"ViewContent": {
				"Headings": [
					"Bold Text",
		                  "Italic Text",
		                  "Link Text"
				],
				"Rows": [
					[
						"**bold text**",
		                "*italic text*"
					],
					[
						"__bold text__",
		                "_italic text_"
					],
					[
		                "",
		                "",
		                "[link text](https://google.com)"
		            ]
				]
			}
		}
		"""
	When I view the component on the page
	Then there should be no errors
	And the elements "thead tr" within the component should total 1
	And the elements "thead tr th" within the component should have the text contents:
		| Text        |
		| Bold Text   |
		| Italic Text |
		| Link Text   |
	And the elements "tbody tr" within the component should total 3
	And the elements "tbody tr:nth-child(1) td" within the component should have the text contents:
		| Text        |
		| bold text   |
		| italic text |
		|             |
	And the elements "tbody tr:nth-child(2) td" within the component should have the text contents:
		| Text        |
		| bold text   |
		| italic text |
		|             |
	And the elements "tbody tr:nth-child(3) td" within the component should have the text contents:
		| Text      |
		|           |
		|           |
		| link text |

@Javascript:disabled
Scenario: Table content should display html correctly with more row columns than heading columns
	Given a content template contains the component:
		"""
		{
			"ViewId": "Table",
			"ViewContent": {
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
		"""
	When I view the component on the page
	Then there should be no errors
	And the elements "thead tr" within the component should total 1
	And the elements "thead tr th" within the component should have the text contents:
		| Text     |
		| Header A |
		|          |
	And the elements "tbody tr" within the component should total 2
	And the elements "tbody tr:nth-child(1) td" within the component should have the text contents:
		| Text   |
		| Cell A |
		| Cell B |
	And the elements "tbody tr:nth-child(2) td" within the component should have the text contents:
		| Text   |
		| Cell C |
		| Cell D |

@Javascript:disabled
Scenario: Table content should display html correctly with more heading columns than rows columns
	Given a content template contains the component:
		"""
		{
			"ViewId": "Table",
			"ViewContent": {
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
		"""
	When I view the component on the page
	Then there should be no errors
	And the elements "thead tr" within the component should total 1
	And the elements "thead tr th" within the component should have the text contents:
		| Text     |
		| Header A |
		| Header B |
		| Header C |
	And the elements "tbody tr" within the component should total 1
	And the elements "tbody tr td" within the component should total 3
	And the elements "tbody tr td" within the component should have the text contents:
		| Text   |
		| Cell A |
		|        |
		|        |

@Javascript:disabled
Scenario: Table content should display html correctly with no Rows specified
	Given a content template contains the component:
		"""
		{
			"ViewId": "Table",
			"ViewContent": {
				"Headings": [
					"Header A",
					"Header B"
				]
			},
		}
		"""
	When I view the component on the page
	Then there should be no errors
	And the elements "thead tr" within the component should total 1
	And the elements "thead tr th" within the component should have the text contents:
		| Text     |
		| Header A |
		| Header B |
	And the elements "tbody tr" within the component should total 1
	And the elements "tbody tr td" within the component should total 2
	And the elements "tbody tr td" within the component should have the text contents:
		| Text |
		|      |
		|      |

@Javascript:disabled
Scenario: Table content should display html correctly with no Headings specified
	Given a content template contains the component:
		"""
		{
			"ViewId": "Table",
			"ViewContent": {
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
			}
		}
		"""
	When I view the component on the page
	Then there should be no errors
	And the elements "thead tr" within the component should total 1
	And the elements "thead tr th" within the component should have the text contents:
		| Text |
		|      |
		|      |
	And the elements "tbody tr" within the component should total 2
	And the elements "tbody tr:nth-child(1) td" within the component should have the text contents:
		| Text   |
		| Cell A |
		| Cell B |
	And the elements "tbody tr:nth-child(2) td" within the component should have the text contents:
		| Text   |
		| Cell C |
		| Cell D |

@Javascript:disabled
Scenario: Table content should display html correctly with empty rows
	Given a content template contains the component:
		"""
		{
			"ViewId": "Table",
			"ViewContent": {
				"Headings": [
					"Header A",
					"Header B"
				],
				"Rows": [
					[],
					[]
				]
			}
		}
		"""
	When I view the component on the page
	Then there should be no errors
	And the elements "thead tr" within the component should total 1
	And the elements "thead tr th" within the component should have the text contents:
		| Text     |
		| Header A |
		| Header B |
	And the elements "tbody tr" within the component should total 2
	And the elements "tbody tr:nth-child(1) td" within the component should have the text contents:
		| Text |
		|      |
		|      |
	And the elements "tbody tr:nth-child(2) td" within the component should have the text contents:
		| Text |
		|      |
		|      |

@Javascript:disabled
Scenario: Table content should display html correctly with empty headings
	Given a content template contains the component:
		"""
		{
			"ViewId": "Table",
			"ViewContent": {
				"Headings": [
						
				],
				"Rows": [
					[
						"Cell A",
						"Cell B"
					]
				]
			}
		}
		"""
	When I view the component on the page
	Then there should be no errors
	And the elements "thead tr" within the component should total 1
	And the elements "thead tr th" within the component should have the text contents:
		| Text |
		|      |
		|      |
	And the elements "tbody tr" within the component should total 1
	And the elements "tbody tr td" within the component should have the text contents:
		| Text   |
		| Cell A |
		| Cell B |

@Javascript:disabled
Scenario: Table content should display html correctly with Row having null value
	Given a content template contains the component:
		"""
		{
			"ViewId": "Table",
			"ViewContent": {
				"Headings": [
					"Header A",
					"Header B"
				],
				"Rows": [
					[
						null
					],
				]
			}
		}
		"""
	When I view the component on the page
	Then there should be no errors
	And the elements "thead tr" within the component should total 1
	And the elements "thead tr th" within the component should have the text contents:
		| Text     |
		| Header A |
		| Header B |
	And the elements "tbody tr" within the component should total 1
	And the elements "tbody tr td" within the component should have the text contents:
		| Text |
		|      |
		|      |

@Javascript:disabled
Scenario: Table content should display html correctly with Row as null
	Given a content template contains the component:
		"""
		{
			"ViewId": "Table",
			"ViewContent": {
				"Headings": [
					"Header A",
					"Header B"
				],
				"Rows": null
			}
		}
		"""
	When I view the component on the page
	Then there should be no errors
	And the elements "thead tr" within the component should total 1
	And the elements "thead tr th" within the component should have the text contents:
		| Text     |
		| Header A |
		| Header B |
	And the elements "tbody tr" within the component should total 1
	And the elements "tbody tr td" within the component should have the text contents:
		| Text |
		|      |
		|      |

@Javascript:disabled
Scenario: Table content should display html correctly with Headings as null
	Given a content template contains the component:
		"""
		{
			"ViewId": "Table",
			"ViewContent": {
				"Headings": null,
				"Rows": [
					[
						"Cell A",
						"Cell B"
					]
				]
			}
		}
		"""
	When I view the component on the page
	Then there should be no errors
	And the elements "thead tr" within the component should total 1
	And the elements "thead tr th" within the component should have the text contents:
		| Text |
		|      |
		|      |
	And the elements "tbody tr" within the component should total 1
	And the elements "tbody tr td" within the component should have the text contents:
		| Text   |
		| Cell A |
		| Cell B |

@Javascript:disabled
Scenario: Table content should display html correctly with Headings having null value
	Given a content template contains the component:
		"""
		{
			"ViewId": "Table",
			"ViewContent": {
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
		"""
	When I view the component on the page
	Then there should be no errors
	And the elements "thead tr" within the component should total 1
	And the elements "thead tr th" within the component should have the text contents:
		| Text |
		|      |
		|      |
	And the elements "tbody tr" within the component should total 1
	And the elements "tbody tr td" within the component should have the text contents:
		| Text   |
		| Cell A |
		| Cell B |

@Javascript:disabled
Scenario: Table content should display first column as header when FirstCellAsHeader is true
	Given a content template contains the component:
		"""
		{
			"ViewId": "Table",
			"ViewContent": {
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
			}
		}
		"""
	When I view the component on the page
	Then there should be no errors
	And the elements "thead tr th" within the component should all have the class "govuk-table__header"
	And the elements "tbody tr td:nth-child(1)" within the component should all have the class "govuk-table__header"
	And the elements "tbody tr td:nth-child(2)" within the component should all have the class "govuk-table__cell"

@Javascript:disabled
Scenario: Table content should not display first column as header when FirstCellAsHeader is false
	Given a content template contains the component:
		"""
		{
			"ViewId": "Table",
			"ViewContent": {
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
			}
		}
		"""
	When I view the component on the page
	Then there should be no errors
	And the elements "thead tr th" within the component should all have the class "govuk-table__header"
	And the elements "tbody tr td" within the component should all have the class "govuk-table__cell"

@Javascript:disabled
Scenario: Table content should not display first column as header when FirstCellAsHeader is missing
	Given a content template contains the component:
		"""
		{
			"ViewId": "Table",
			"ViewContent": {
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
			}
		}
		"""
	When I view the component on the page
	Then there should be no errors
	And the elements "thead tr th" within the component should all have the class "govuk-table__header"
	And the elements "tbody tr td" within the component should all have the class "govuk-table__cell"

@Javascript:disabled
Scenario: Table content should not display first column as header when FirstCellAsHeader is not a boolean value
	Given a content template contains the component:
		"""
		{
			"ViewId": "Table",
			"ViewContent": {
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
			}
		}
		"""
	When I view the component on the page
	Then there should be no errors
	And the elements "thead tr th" within the component should all have the class "govuk-table__header"
	And the elements "tbody tr td" within the component should all have the class "govuk-table__cell"

Examples:
	| Value  |
	| "true" |
	| 12345  |
	| null   |