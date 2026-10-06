The Table component provides the ability to add tables to content templates. 

[View functional tests for this component](https://dev.azure.com/dfe-ssp/s192-Analyse-School-Performance%20%28ASP%29/_apps/hub/techtalk.techtalk-specflow-plus.techtalk.specflow.plus.hub#/document/5d35bfe282df171235bc3358af9dd39c795f85c7/feature/3d69300ac3107f51cdbef64f35396694)

# JSON structure

The component JSON structure is as follows:

```
{
    "ViewId": "Table",
    "ViewContent": {
        "Caption": <string>,
        "Headings": <array of <string>>,
        "Rows": <array of <array of <string>>>,
        "FirstCellAsHeader": <bool>
    }
}
```

Tables are styled according to the [Table section of the GOV.UK Design System](https://design-system.service.gov.uk/components/table/).

## Caption property
*Value: any string*

The *Caption* property provides an optional caption above the table. If the property is missing or null, the caption will not be displayed.

### Example table with caption
```
{
    "ViewId": "Table",
    "ViewContent": {
        "Caption": "This is the caption",
        "Headings": [ "Heading" ]
        "Rows": [ [ "Row" ] ],
        "FirstCellAsHeader": false
    }
}
```

This generates the following HTML:
```
<table class="govuk-table">
    <caption class="govuk-table__caption govuk-table__caption--m">This is the caption</caption>
    <thead class="govuk-table__head">
        <tr class="govuk-table__row">
            <th scope="col" class="govuk-table__header">Heading</th>
        </tr>
    </thead>
    <tbody class="govuk-table__body">
        <tr class="govuk-table__row">
            <td class="govuk-table__cell">Row</td>
        </tr>
    </tbody>
</table>
```

Which displays the following in the UI:
![table with caption](/docs/.attachments/image-8d8aaff3-f8f3-4abc-a77a-54c63d666c34.png)

## Headings property
*Value: array of string values*

The *Headings* property is an array representing a list of column headings for the table.

If the property is missing, null or an empty array, the table will display with no headings.

### Example table with headings
```
{
    "ViewId": "Heading",
    "ViewContent": {
        "Headings": [
            "Column 1",
            "Column 2"
        ],
        "Rows": []
    }
}
```
This generates the following HTML:
```
<table class="govuk-table">
    <caption class="govuk-table__caption govuk-table__caption--m">This is the caption</caption>
    <thead class="govuk-table__head">
        <tr class="govuk-table__row">
            <th scope="col" class="govuk-table__header">Column 1</th>
            <th scope="col" class="govuk-table__header">Column 2</th>
        </tr>
    </thead>
    <tbody class="govuk-table__body">
    </tbody>
</table>
```
Which displays the following in the UI:
![image.png](/docs/.attachments/image-721009ba-8d8f-4ff5-80e1-4c4bffcecbc8.png)

## Rows property
*Value: array of (array of string values)*

The *Rows* property is a nested array, the outer array is a list of rows in the table, and each row is a list of values representing each table cell within the row.

If the property is missing, null or an empty array, the table will display with no rows.

### Example table with rows
```
{
    "ViewId": "Heading",
    "ViewContent": {
        "Headings": [
        ],
        "Rows": [
            [
                "Row 1 Column 1",
                "Row 1 Column 2"
            ],
            [
                "Row 2 Column 1",
                "Row 2 Column 2"
            ]
        ]
    }
}
```
This generates the following HTML:
```
<table class="govuk-table">
    <caption class="govuk-table__caption govuk-table__caption--m">This is the caption</caption>
    <thead class="govuk-table__head">
        <tr class="govuk-table__row">
            <th scope="col" class="govuk-table__header"></th>
            <th scope="col" class="govuk-table__header"></th>
        </tr>
    </thead>
    <tbody class="govuk-table__body">
        <tr class="govuk-table__row">
            <td class="govuk-table__cell">Row 1 Column 1</td>
            <td class="govuk-table__cell">Row 1 Column 2</td>
        </tr>
        <tr class="govuk-table__row">
            <td class="govuk-table__cell">Row 2 Column 1</td>
            <td class="govuk-table__cell">Row 2 Column 2</td>
        </tr>
    </tbody>
</table>
```
Which displays the following in the UI:

![image.png](/docs/.attachments/image-53c9338d-2c50-4a3c-a4b5-efea8c2a2225.png)

**Note:** the number of headers always defaults to the maximum number of cells in each row. This is true of cells, so if there are an inconsistent number of cells in each row, the component will make sure each row has the same number of cells as the biggest row. This is to prevent styling issues with missing cells. For example given these rows:

```
"Rows": [
    [
        "Row 1 Column 1"
    ],
    [
        "Row 2 Column 1",
        "Row 2 Column 2",
        "Row 2 Column 3"
    ],
    [
        "Row 3 Column 1",
        "Row 3 Column 2"
    ]
]
```
the number of cells in each row will fill out to the number of cells in the biggest row:
```
<tbody class="govuk-table__body">
    <tr class="govuk-table__row">
        <td class="govuk-table__cell">Row 1 Column 1</td>
        <td class="govuk-table__cell"></td>
        <td class="govuk-table__cell"></td>
    </tr>
    <tr class="govuk-table__row">
        <td class="govuk-table__cell">Row 2 Column 1</td>
        <td class="govuk-table__cell">Row 2 Column 2</td>
        <td class="govuk-table__cell">Row 2 Column 3</td>
    </tr>
    <tr class="govuk-table__row">
        <td class="govuk-table__cell">Row 3 Column 1</td>
        <td class="govuk-table__cell">Row 3 Column 2</td>
        <td class="govuk-table__cell"></td>
    </tr>
</tbody>
```

Each table cell string supports a small subset of markdown syntax:

| Text| Displayed as|
|-|-|
| `**bold text**` or `__bold text__` | **bold text** |
| `*italic text*` or `_italic text_` | *italic text* |
| `[link text](https://google.com)`|[link text](https://google.com)|

Combinations of these are also supported, e.g.:

| Text | Displayed as|
|-|-|
| `**_bold italic text_**` or `*__bold italic text__*` etc. | **_bold text_** |
| `[**bold** _italic_ link text](https://google.com)`|[**bold** _italic_ link text](https://google.com)|


## FirstCellAsHeader property
*Value: boolean (true or false)*

The *FirstCellAsHeader* property is a boolean, if set to true, will transform the first cell in each row, to a header.

If the property is missing, null, empty or a different value, the table will default to a normal cell.


# Editing
Tables can be edited within the page using the table component editor:
![table edit](/docs/.attachments/image-cafcbe33-c0b8-4850-bc37-b152a04d710b.png)

## Heading Type field
*Dropdown, values `"H2"` or `"H3"`*

Any other value defaults to `"H2"`. Populates `HeadingType` property with `"h2"` or `"h3"`

## Caption field
*Textbox, any value*

Populates `Caption` property with string value.

## Headings field
*Textarea, JSON array of string values*

Populates `Headings` property with string value.

## Rows field
*Textarea, JSON array of array of string values*

Populates `Rows` property with string value.