This component is used to render cards on the homepage and LA/school landing pages, e.g. the **Phonics**, **Key stage 1**, **Multiplication table check (MTC)** etc. cards here:

![image.png](/docs/.attachments/image-a86019ad-bf20-40c7-9ba2-d157c88914a5.png)

# JSON structure

The component JSON structure is as follows:

```
{
    "ViewId": "Card",
    "ViewContent": {
        "Title": <string>,
        "LinkUrl": <string>
        "Text": <string>
    }
}
```

## Title property
*Value: any string*

The *Title* property defines the heading text of the card. If the property is missing or set to null, the title will not be displayed. (**Note:** this should be fixed to show a sensible default value instead)

## LinkUrl property
*Value: any string*

The *LinkUrl* property provides an optional URL for the heading to link to. If the property is missing or null, the link will not be displayed.

## Text property
*Value: any string*

The *Text* property contains the text content of the card as a string. If the property is missing or set to null, the text content will not be displayed.

Markdown syntax is not supported (**Note:** this should probably be enhanced to include markdown support, see [List & ListItem](/Analyse-School-Performance-\(ASP\)-Index/Technical-specification/Content-template-engine/Generic-components/List-&-ListItem))

## Example card with title, link and text
```
{
    "ViewId": "Card",
    "ViewContent": {
        "Title": "A sample card",
        "LinkUrl": "https://mysamplecard.com",
        "Text": "This is the text contained within the card",
    }
}
```

This produces the following HTML:
```
<div class="app-card">
    <div class="app-card-container">
        <h2 class="govuk-heading-m">
            <a href="https://mysamplecard.com" class="app-card-link govuk-link govuk-link--no-visited-state">
                A sample card
            </a>
        </h2>
        <p class="govuk-body">
            This is the text contained within the card
        </p>
    </div>
</div>
```