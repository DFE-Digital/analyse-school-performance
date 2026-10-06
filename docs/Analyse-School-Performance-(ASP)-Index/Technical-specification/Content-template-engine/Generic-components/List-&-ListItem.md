The List and ListItem components provide the ability to add unordered lists to content templates.

[View functional tests for this component](https://dfe-ssp.visualstudio.com/s192-Analyse-School-Performance%20(ASP)/_apps/hub/techtalk.techtalk-specflow-plus.techtalk.specflow.plus.hub#/document/5d35bfe282df171235bc3358af9dd39c795f85c7/feature/ecd1c72223616372b9ebd9098cc69a06)

# JSON structure

A `List` contains a collection of `ListItem`s (via its `ChildViews` property), and each `ListItem` has some text (via `ViewContent.Text`), and can optionally contain another `List` (also via its `ChildViews` property).

The component JSON structure is as follows:

```
{
    "ViewId": "List",
    "ViewContent": {},
    "ChildViews": [
        {
            "ViewId": "ListItem",
            "ViewContent": {
                "Text": "List item 1"
            }
        },
        {
            "ViewId": "ListItem",
            "ViewContent": {
                "Text": "List item 2"
            },
            "ChildViews": [
                {
                    "ViewId": "List",
                    "ViewContent": {},
                    "ChildViews": [
                        {
                            "ViewId": "ListItem",
                            "ViewContent": {
                                "Text": "Nested list item 1"
                            }
                        },
                        {
                            "ViewId": "ListItem",
                            "ViewContent": {
                                "Text": "Nested list item 2"
                            }
                        }
                    ]
            ]
        },
        {
            "ViewId": "ListItem",
            "ViewContent": {
                "Text": "List item 3"
            }
        }
    ]
}
```

This will produce a list like this:

* List item 1
* List item 2
  * Nested list item 1
  * Nested list item 2
* List item 3

**Notes:**
* List item 2 contains a list, which contains more list items. These list items could potentially each contain a list which could contain more list items, etc.
* List component does not have any `ViewContent` properties. the `ViewContent` property for lists is an empty object, but this property can be omitted without any issues.
* ChildViews can technically contain any component - e.g. `ViewId` could be set to `Table` or `Paragraph` or any other component. There is currently nothing in place to catch this. The expectation is that `List` components will only ever contain `ListItem` components and `ListItem` components will only ever contain `List` components. For anything else the behaviour is undefined.

Lists are styled according to the [Bulleted lists section of the GOV.UK Design System](https://design-system.service.gov.uk/styles/lists/).

# List
## Text property
*Value: any string*

The *Text* property contains the text content of the list item as a string. The text string supports a small subset of markdown syntax:

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

# Editing
Tables can be edited within the page using the list component editor:
![list edit](/.attachments/image-865dbd5b-665f-425a-af4c-7608c07fc9a4.png)

Note that only the text content of list items can be edited currently. List items cannot be added or deleted or moved. This is functionality that will be implemented at a later date.

## Text
*Textbox, any value*

Populates `Text` property with string value.