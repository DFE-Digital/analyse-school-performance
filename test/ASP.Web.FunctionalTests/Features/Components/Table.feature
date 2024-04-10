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
	Then the element "#test" should have the outer HTML:
		"""
		<table class="govuk-table" id="test">
			<caption class="govuk-table__caption govuk-table__caption--m">Caption here</caption>
			<thead class="govuk-table__head">
				<tr class="govuk-table__row">
					 <th scope="col" class="govuk-table__header">Header A</th>
					 <th scope="col" class="govuk-table__header">Header B</th>
				</tr>
			</thead>
			<tbody class="govuk-table__body">
				<tr class="govuk-table__row">
					<td class="govuk-table__cell">Cell A</td>
					<td class="govuk-table__cell">Cell B</td>
				</tr>
			</tbody>
		</table>
		"""


@Javascript:disabled
Scenario: Table content should display html correctly when a caption is null
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
	Then the element "#test" should have the outer HTML:
		"""
		<table class="govuk-table" id="test">
			<thead class="govuk-table__head">
				<tr class="govuk-table__row">
					 <th scope="col" class="govuk-table__header">Header A</th>
					 <th scope="col" class="govuk-table__header">Header B</th>
				</tr>
			</thead>
			<tbody class="govuk-table__body">
				<tr class="govuk-table__row">
					<td class="govuk-table__cell">Cell A</td>
					<td class="govuk-table__cell">Cell B</td>
				</tr>
			</tbody>
		</table>
		"""


@Javascript:disabled
Scenario: Table content should display html correctly when a caption is empty
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
	Then the element "#test" should have the outer HTML:
		"""
		<table class="govuk-table" id="test">
			<caption class="govuk-table__caption govuk-table__caption--m"></caption>
			<thead class="govuk-table__head">
				<tr class="govuk-table__row">
					 <th scope="col" class="govuk-table__header">Header A</th>
					 <th scope="col" class="govuk-table__header">Header B</th>
				</tr>
			</thead>
			<tbody class="govuk-table__body">
				<tr class="govuk-table__row">
					<td class="govuk-table__cell">Cell A</td>
					<td class="govuk-table__cell">Cell B</td>
				</tr>
			</tbody>
		</table>
		"""



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
	Then the element "#test" should have the outer HTML:
		"""
		<table class="govuk-table" id="test">
			<thead class="govuk-table__head">
				<tr class="govuk-table__row">
					 <th scope="col" class="govuk-table__header">Header A</th>
					 <th scope="col" class="govuk-table__header">Header B</th>
				</tr>
			</thead>
			<tbody class="govuk-table__body">
				<tr class="govuk-table__row">
					<td class="govuk-table__cell">Cell A</td>
					<td class="govuk-table__cell">Cell B</td>
				</tr>
				<tr class="govuk-table__row">
					<td class="govuk-table__cell">Cell C</td>
					<td class="govuk-table__cell">Cell D</td>
				</tr>
			</tbody>
		</table>
		"""


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
	Then the element "#test" should have the outer HTML:
		"""
		<table class="govuk-table" id="test">
			<thead class="govuk-table__head">
				<tr class="govuk-table__row">
					 <th scope="col" class="govuk-table__header">Header A</th>
					 <th scope="col" class="govuk-table__header"></th>
				</tr>
			</thead>
			<tbody class="govuk-table__body">
				<tr class="govuk-table__row">
					<td class="govuk-table__cell">Cell A</td>
					<td class="govuk-table__cell">Cell B</td>
				</tr>
				<tr class="govuk-table__row">
					<td class="govuk-table__cell">Cell C</td>
					<td class="govuk-table__cell">Cell D</td>
				</tr>
			</tbody>
		</table>
		"""


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
	Then the element "#test" should have the outer HTML:
		"""
		<table class="govuk-table" id="test">
			<thead class="govuk-table__head">
				<tr class="govuk-table__row">
					 <th scope="col" class="govuk-table__header">Header A</th>
					 <th scope="col" class="govuk-table__header">Header B</th>
					 <th scope="col" class="govuk-table__header">Header C</th>
				</tr>
			</thead>
			<tbody class="govuk-table__body">
				<tr class="govuk-table__row">
					<td class="govuk-table__cell">Cell A</td>
					<td class="govuk-table__cell"></td>
					<td class="govuk-table__cell"></td>
				</tr>
			</tbody>
		</table>
		"""


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
	Then the element "#test" should have the outer HTML:
		"""
		<table class="govuk-table" id="test">
			<thead class="govuk-table__head">
				<tr class="govuk-table__row">
					 <th scope="col" class="govuk-table__header">Header A</th>
					 <th scope="col" class="govuk-table__header">Header B</th>
				</tr>
			</thead>
			<tbody class="govuk-table__body">
				<tr class="govuk-table__row">
					<td class="govuk-table__cell"></td>
					<td class="govuk-table__cell"></td>
				</tr>
			</tbody>
		</table>
		"""


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
	Then the element "#test" should have the outer HTML:
		"""
		<table class="govuk-table" id="test">
			<thead class="govuk-table__head">
				<tr class="govuk-table__row">
					 <th scope="col" class="govuk-table__header"></th>
					 <th scope="col" class="govuk-table__header"></th>
				</tr>
			</thead>
			<tbody class="govuk-table__body">
				<tr class="govuk-table__row">
					<td class="govuk-table__cell">Cell A</td>
					<td class="govuk-table__cell">Cell B</td>
				</tr>
				<tr class="govuk-table__row">
					<td class="govuk-table__cell">Cell C</td>
					<td class="govuk-table__cell">Cell D</td>
				</tr>
			</tbody>
		</table>
		"""


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
	Then the element "#test" should have the outer HTML:
		"""
		<table class="govuk-table" id="test">
			<thead class="govuk-table__head">
				<tr class="govuk-table__row">
					 <th scope="col" class="govuk-table__header">Header A</th>
					 <th scope="col" class="govuk-table__header">Header B</th>
				</tr>
			</thead>
			<tbody class="govuk-table__body">
				<tr class="govuk-table__row">
					<td class="govuk-table__cell"></td>
					<td class="govuk-table__cell"></td>
				</tr>
				<tr class="govuk-table__row">
					<td class="govuk-table__cell"></td>
					<td class="govuk-table__cell"></td>
				</tr>
			</tbody>
		</table>
		"""


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
	Then the element "#test" should have the outer HTML:
		"""
		<table class="govuk-table" id="test">
			<thead class="govuk-table__head">
				<tr class="govuk-table__row">
					 <th scope="col" class="govuk-table__header"></th>
					 <th scope="col" class="govuk-table__header"></th>
				</tr>
			</thead>
			<tbody class="govuk-table__body">
				<tr class="govuk-table__row">
					<td class="govuk-table__cell">Cell A</td>
					<td class="govuk-table__cell">Cell B</td>
				</tr>
			</tbody>
		</table>
		"""


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
	Then the element "#test" should have the outer HTML:
		"""
		<table class="govuk-table" id="test">
			<thead class="govuk-table__head">
				<tr class="govuk-table__row">
					 <th scope="col" class="govuk-table__header">Header A</th>
					 <th scope="col" class="govuk-table__header">Header B</th>
				</tr>
			</thead>
			<tbody class="govuk-table__body">
				<tr class="govuk-table__row">
					<td class="govuk-table__cell"></td>
					<td class="govuk-table__cell"></td>
				</tr>
			</tbody>
		</table>
		"""


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
	Then the element "#test" should have the outer HTML:
		"""
		<table class="govuk-table" id="test">
			<thead class="govuk-table__head">
				<tr class="govuk-table__row">
					 <th scope="col" class="govuk-table__header">Header A</th>
					 <th scope="col" class="govuk-table__header">Header B</th>
				</tr>
			</thead>
			<tbody class="govuk-table__body">
				<tr class="govuk-table__row">
					<td class="govuk-table__cell"></td>
					<td class="govuk-table__cell"></td>
				</tr>
			</tbody>
		</table>
		"""


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
	Then the element "#test" should have the outer HTML:
		"""
		<table class="govuk-table" id="test">
			<thead class="govuk-table__head">
				<tr class="govuk-table__row">
					 <th scope="col" class="govuk-table__header"></th>
					 <th scope="col" class="govuk-table__header"></th>
				</tr>
			</thead>
			<tbody class="govuk-table__body">
				<tr class="govuk-table__row">
					<td class="govuk-table__cell">Cell A</td>
					<td class="govuk-table__cell">Cell B</td>
				</tr>
			</tbody>
		</table>
		"""


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
	Then the element "#test" should have the outer HTML:
		"""
		<table class="govuk-table" id="test">
			<thead class="govuk-table__head">
				<tr class="govuk-table__row">
					 <th scope="col" class="govuk-table__header"></th>
					 <th scope="col" class="govuk-table__header"></th>
				</tr>
			</thead>
			<tbody class="govuk-table__body">
				<tr class="govuk-table__row">
					<td class="govuk-table__cell">Cell A</td>
					<td class="govuk-table__cell">Cell B</td>
				</tr>
			</tbody>
		</table>
		"""


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
	Then the element "#test" should have the outer HTML:
		"""
		<table class="govuk-table" id="test">
			<thead class="govuk-table__head">
				<tr class="govuk-table__row">
					 <th scope="col" class="govuk-table__header">Header A</th>
					 <th scope="col" class="govuk-table__header">Header B</th>
				</tr>
			</thead>
			<tbody class="govuk-table__body">
				<tr class="govuk-table__row">
					<td class="govuk-table__header">Cell A</td>
					<td class="govuk-table__cell">Cell B</td>
				</tr>
				<tr class="govuk-table__row">
					<td class="govuk-table__header">Cell C</td>
					<td class="govuk-table__cell">Cell D</td>
				</tr>
			</tbody>
		</table>
		"""


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
	Then the element "#test" should have the outer HTML:
		"""
		<table class="govuk-table" id="test">
			<thead class="govuk-table__head">
				<tr class="govuk-table__row">
					 <th scope="col" class="govuk-table__header">Header A</th>
					 <th scope="col" class="govuk-table__header">Header B</th>
				</tr>
			</thead>
			<tbody class="govuk-table__body">
				<tr class="govuk-table__row">
					<td class="govuk-table__cell">Cell A</td>
					<td class="govuk-table__cell">Cell B</td>
				</tr>
				<tr class="govuk-table__row">
					<td class="govuk-table__cell">Cell C</td>
					<td class="govuk-table__cell">Cell D</td>
				</tr>
			</tbody>
		</table>
		"""


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
	Then the element "#test" should have the outer HTML:
		"""
		<table class="govuk-table" id="test">
			<thead class="govuk-table__head">
				<tr class="govuk-table__row">
					 <th scope="col" class="govuk-table__header">Header A</th>
					 <th scope="col" class="govuk-table__header">Header B</th>
				</tr>
			</thead>
			<tbody class="govuk-table__body">
				<tr class="govuk-table__row">
					<td class="govuk-table__cell">Cell A</td>
					<td class="govuk-table__cell">Cell B</td>
				</tr>
				<tr class="govuk-table__row">
					<td class="govuk-table__cell">Cell C</td>
					<td class="govuk-table__cell">Cell D</td>
				</tr>
			</tbody>
		</table>
		"""


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
	Then the element "#test" should have the outer HTML:
		"""
		<table class="govuk-table" id="test">
			<thead class="govuk-table__head">
				<tr class="govuk-table__row">
					 <th scope="col" class="govuk-table__header">Header A</th>
					 <th scope="col" class="govuk-table__header">Header B</th>
				</tr>
			</thead>
			<tbody class="govuk-table__body">
				<tr class="govuk-table__row">
					<td class="govuk-table__cell">Cell A</td>
					<td class="govuk-table__cell">Cell B</td>
				</tr>
				<tr class="govuk-table__row">
					<td class="govuk-table__cell">Cell C</td>
					<td class="govuk-table__cell">Cell D</td>
				</tr>
			</tbody>
		</table>
		"""
Examples:
	| Value  |
	| "true" |
	| 12345  |
	| null   |