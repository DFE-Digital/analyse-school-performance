The Paragraph component provides the ability to paragraphs to content templates. 

[View functional tests for this component](https://dev.azure.com/dfe-ssp/s192-Analyse-School-Performance%20%28ASP%29/_apps/hub/techtalk.techtalk-specflow-plus.techtalk.specflow.plus.hub#/document/5d35bfe282df171235bc3358af9dd39c795f85c7/feature/e49a92764807ec2f63e90ffaf3996e89)

# JSON structure

The component JSON structure is as follows:

```
{
    "ViewId": "Paragraph",
    "ViewContent": {
        "IsLarge": <boolean>,
        "Text": <string>
    }
}
```

Paragraphs are styled according to the [Paragraphs section of the GOV.UK Design System](https://design-system.service.gov.uk/styles/paragraphs/).

## IsLarge property
*Value: `true` or `false`*

The *IsLarge* property provides an option for a larger size of paragraph. Example of a larger vs. a normal size of paragraph:
![image.png](/docs/.attachments/image-707d4402-a1c5-4fd7-a704-a6c8166f4114.png)

If the property is missing or set to null or any other value other than `true` or `false`, it will default to being a normal size paragraph.

A paragraph with `IsLarge` set to true will render as a `<p class="govuk-body-l">`, otherwise it will render as `<p class="govuk-body">`

## Text property
*Value: any string*

The *Text* property defines the text for the paragraph. If the property is missing or set to null, it will default to the text `"Paragraph text"`.

Within the *Text* property A small subset of markdown syntax is supported:

| Text| Displayed as|
|-|-|
| `**bold text**` or `__bold text__` | **bold text** |
| `*italic text*` or `_italic text_` | *italic text* |
| `[link text](https://google.com)`|[link text](https://google.com)|

Combinations of these are also supported, e.g.:

| Text| Displayed as|
|-|-|
| `**_bold italic text_**` or `*__bold italic text__*` etc. | **_bold text_** |
| `[**bold** _italic_ link text](https://google.com)`|[**bold** _italic_ link text](https://google.com)|


# Editing
Paragraphs can be edited within the page using the paragraph component editor:
![image.png](/docs/.attachments/image-bb56e75a-faf0-4f43-a6eb-b9d20c1dac75.png)

## Is Large field
*Checkbox*

Any other value defaults to `unchecked`. Populates `IsLarge` property with `true` or `false`

## Text field
*Textbox, any value*

Populates `Text` property with string value. 