The Heading component provides the ability to add h2 and h3 tags to content templates. 

[View functional tests for this component](https://dev.azure.com/dfe-ssp/s192-Analyse-School-Performance%20%28ASP%29/_apps/hub/techtalk.techtalk-specflow-plus.techtalk.specflow.plus.hub#/document/5d35bfe282df171235bc3358af9dd39c795f85c7/feature/e72adc1f6a58b8a5d4baa0bf41f49783)

# JSON structure

The component JSON structure is as follows:

```
{
    "ViewId": "Heading",
    "ViewContent": {
        "HeadingType": "h2" | "h3",
        "Caption": <string>,
        "Text": <string>,
        "LinkUrl": <string>
    }
}
```

Headings are styled according to the [Headings section of the GOV.UK Design System](https://design-system.service.gov.uk/styles/headings/).

## HeadingType property
*Value: `"h2"` or `"h3"`*

The *HeadingType* property provides two levels of headings, `h2` and `h3`. `h1` headings are not allowed as they are used for the title of the page.

If the property is missing or set to null or any other value other than `"h2"` or `"h3"`, it will default to being a `h2`.

### Example h2 heading
```
{
    "ViewId": "Heading",
    "ViewContent": {
        "HeadingType": "h2",
        "Text": "This is a h2 heading"
    }
}
```

This generates the following HTML:
```
<h2 class="govuk-heading-l">
    This is a h2 heading
</h2>
```

Which displays the following in the UI:
![h2](https://dfe-ssp.visualstudio.com/eb62f5e3-e9f9-48e4-b1ad-1299fcc97149/_apis/git/repositories/64c84fc6-b50c-4733-b588-1cf324205824/Items?path=/.attachments/image-584d6265-a71e-465b-b2a9-80573eece685.png&download=false&resolveLfs=true&%24format=octetStream&api-version=5.0-preview.1&sanitize=true&versionDescriptor.version=wikiMaster)


### Example h3 heading
------------------
```
{
    "ViewId": "Heading",
    "ViewContent": {
        "HeadingType": "h3",
        "Text": "This is a h3 heading"
    }
}
```

This generates the following HTML:
```
<h3 class="govuk-heading-m">
    This is a h3 heading
</h3>
```

Which displays the following in the UI:
![h3](https://dfe-ssp.visualstudio.com/eb62f5e3-e9f9-48e4-b1ad-1299fcc97149/_apis/git/repositories/64c84fc6-b50c-4733-b588-1cf324205824/Items?path=/.attachments/image-7b0ff59d-2cf0-4453-a1a2-00e8a8c4f879.png&download=false&resolveLfs=true&%24format=octetStream&api-version=5.0-preview.1&sanitize=true&versionDescriptor.version=wikiMaster)

## Text property
*Value: any string*

The *Text* property defines the text for the heading. If the property is missing or set to null, it will default to the text `"Heading text"`.


## Caption property
*Value: any string*

The *Caption* property provides an optional caption above the heading. If the property is missing or null, the caption will not be displayed.

### Example h2 heading with caption
```
{
    "ViewId": "Heading",
    "ViewContent": {
        "HeadingType": "h2",
        "Caption": "This is the caption",
        "Text": "This is a h2 heading with caption"
    }
}
```

This generates the following HTML:
```
<h2 class="govuk-heading-l">
    <span class="govuk-caption-l">This is the caption</span>
    This is a h2 heading with caption
</h2>
```

Which dispays the following in the UI:
![h2 with caption](https://dfe-ssp.visualstudio.com/eb62f5e3-e9f9-48e4-b1ad-1299fcc97149/_apis/git/repositories/64c84fc6-b50c-4733-b588-1cf324205824/Items?path=/.attachments/image-eac1faec-b87d-4e91-add7-bf57845f133b.png&download=false&resolveLfs=true&%24format=octetStream&api-version=5.0-preview.1&sanitize=true&versionDescriptor.version=wikiMaster)

### Example h3 heading with caption
```
{
    "ViewId": "Heading",
    "ViewContent": {
        "HeadingType": "h3",
        "Caption": "This is the caption",
        "Text": "This is a h3 heading with caption"
    }
}
```

This generates the following HTML:
```
<h3 class="govuk-heading-m">
    <span class="govuk-caption-m">This is the caption</span>
    This is a h2 heading with caption
</h3>
```

Which displays the following in the UI:
![h3 with caption](https://dfe-ssp.visualstudio.com/eb62f5e3-e9f9-48e4-b1ad-1299fcc97149/_apis/git/repositories/64c84fc6-b50c-4733-b588-1cf324205824/Items?path=/.attachments/image-21205301-35bd-4eda-b2af-fec443bc5834.png&download=false&resolveLfs=true&%24format=octetStream&api-version=5.0-preview.1&sanitize=true&versionDescriptor.version=wikiMaster)

## LinkUrl property
*Value: any string*

The *LinkUrl* property provides an optional URL for the heading to link to. If the property is missing or null, the link will not be displayed.

### Example h2 heading with link
```
{
    "ViewId": "Heading",
    "ViewContent": {
        "HeadingType": "h2",
        "Text": "This is a h2 heading with link",
        "LinkUrl": "https://google.com",
    }
}
```

This generates the following HTML:
```
<h2 class="govuk-heading-l">
    <a href="https://google.com" class="govuk-link">
        This is a h2 heading with link
    </a>
</h2>
```

Which displays the following in the UI:
![h2 with link](https://dfe-ssp.visualstudio.com/eb62f5e3-e9f9-48e4-b1ad-1299fcc97149/_apis/git/repositories/64c84fc6-b50c-4733-b588-1cf324205824/Items?path=/.attachments/image-88322869-4869-4186-85a2-b3dd97547221.png&download=false&resolveLfs=true&%24format=octetStream&api-version=5.0-preview.1&sanitize=true&versionDescriptor.version=wikiMaster)

### Example h3 heading with link
```
{
    "ViewId": "Heading",
    "ViewContent": {
        "HeadingType": "h3",
        "Text": "This is a h3 heading with link",
        "LinkUrl": "https://google.com",
    }
}
```

This generates the following HTML:
```
<h3 class="govuk-heading-l">
    <a href="https://google.com" class="govuk-link">
        This is a h3 heading with link
    </a>
</h3>
```

Which displays the following in the UI:
![h3 with link](https://dfe-ssp.visualstudio.com/eb62f5e3-e9f9-48e4-b1ad-1299fcc97149/_apis/git/repositories/64c84fc6-b50c-4733-b588-1cf324205824/Items?path=/.attachments/image-109c2b90-4bf9-4c7a-a9c9-afe981f93480.png&download=false&resolveLfs=true&%24format=octetStream&api-version=5.0-preview.1&sanitize=true&versionDescriptor.version=wikiMaster)

### Example with caption and link:
![image.png](/.attachments/image-3ae1326f-3fb4-4a0f-ac1f-aa268eaa840b.png)

# Editing
Headings can be edited within the page using the heading component editor:
![Heading edit](https://dfe-ssp.visualstudio.com/eb62f5e3-e9f9-48e4-b1ad-1299fcc97149/_apis/git/repositories/64c84fc6-b50c-4733-b588-1cf324205824/Items?path=/.attachments/image-853dafb3-92ed-4f94-95f3-847e6160fee6.png&download=false&resolveLfs=true&%24format=octetStream&api-version=5.0-preview.1&sanitize=true&versionDescriptor.version=wikiMaster)

## Heading Type field
*Dropdown, values `"H2"` or `"H3"`*

Any other value defaults to `"H2"`. Populates `HeadingType` property with `"h2"` or `"h3"`

## Caption field
*Textbox, any value*

Populates `Caption` property with string value.

## Text field
*Textbox, any value*

Populates `Text` property with string value.

## Link URL field
*Textbox, any value*

Populates `LinkURL` property with string value.