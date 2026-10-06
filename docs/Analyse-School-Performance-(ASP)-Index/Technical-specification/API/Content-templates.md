# GET /content-templates
Retrieves all published content templates.

    GET /content-templates

[View OpenAPI documentation and try out requests for this endpoint](https://s192d01-func-dev-01.azurewebsites.net/api/swagger/ui#/Content%20Templates/ContentTemplatesGetAll)

## Request parameters
No parameters.

## Response

| Status code | Description |
|-|-|
| 405 Method Not Allowed | Incorrect request method - only `GET` allowed |
| 400 Bad Request | Missing or invalid request parameters |
| 404 Not Found | Could not find any published content templates |
| 200 OK | A list of all published content templates |

### Example response body

    [
      {
        "isPublished": true,
        "pageTitle": "string",
        "pageContent": {},
        "views": [
          {
            "viewId": "string",
            "viewContent": {},
            "viewModel": {},
            "childViews": [
              null
            ]
          }
        ]
      }
    ]

--- 

# GET /content-templates/{id}
Retrieves details for a specific content template based on a given ID.

    GET /content-templates/{id}?revision={revision}

[View OpenAPI documentation and try out requests for this endpoint](https://s192d01-func-dev-01.azurewebsites.net/api/swagger/ui#/Content%20Templates/ContentTemplatesGetSingle)

A content template has a base template and a set of revisions. The base template and revisions are the same document structure each having an ID and a content ID. The base template is created first (in an unpublished state) with the ID the same as the content ID. 

New revisions can be created by copying an existing revision with a new ID but the same content ID (in an unpublished state). Any revision can be published, but there can only be one published revision per content template. Published revisions cannot be updated - the only way to update a published content template is to create a new revision with the updated content and set that to be the published revision. **Note:** publishing/unpublishing revisions via the API is not yet implemented.

The revision can be the same as the content ID in which case this is the base template.

If a revision isn't provided:
* If the base template with ID `{id}` doesn't exist, returns a NotFound error
*  If the base template with ID `{id}` exists and is unpublished, returns a NotFound error
*  If the base template with ID `{id}` exists and is published, returns the base template object

If a revision is provided:
*  If the Base Template with ID `{id}` doesn't exist, returns a NotFound error
*  If the revision doesn't exist, returns a NotFound error
*  If the revision exists and is unpublished, returns the revision object
*  If the revision exists and is published, returns the revision object

## Request parameters
|Parameter | Type | Required? | Other validation |
|-|-|-|-|
|**id**| String | Yes| Must not be empty |

The ID of the content template.

|Parameter | Type | Required? | Other validation |
|-|-|-|-|
|**revision**| String | No | Must not be empty |

The ID of a particular revision of the content template.

## Response

| Status code | Description |
|-|-|
| 405 Method Not Allowed | Incorrect request method - only `GET` allowed |
| 400 Bad Request | Missing or invalid request parameters |
| 404 Not Found | Content template not found for the given ID and revision |
| 200 OK | Details of the specified content template |

### Example response body

    {
      "isPublished": true,
      "pageTitle": "string",
      "pageContent": {},
      "views": [
        {
          "viewId": "string",
          "viewContent": {},
          "viewModel": {},
          "childViews": [
            null
          ]
        }
      ]
    }

---

# POST /content-templates/{id}
Updates the content template/revision with the given ID, creating it if it doesn't already exist.

    GET /content-templates/{id}?revision={revision}

[View OpenAPI documentation and try out requests for this endpoint](https://s192d01-func-dev-01.azurewebsites.net/api/swagger/ui#/Content%20Templates/ContentTemplatesUpdateSingle)

A content template has a base template and a set of revisions. The base template and revisions are the same document structure each having an ID and a content ID. The base template is created first (in an unpublished state) with the ID the same as the content ID. 

New revisions can be created by copying an existing revision with a new ID but the same content ID (in an unpublished state). Any revision can be published, but there can only be one published revision per content template. Published revisions cannot be updated - the only way to update a published content template is to create a new revision with the updated content and set that to be the published revision. **Note:** publishing/unpublishing revisions via the API is not yet implemented.

The revision can be the same as the content ID in which case this is the base template.

If a revision isn't provided:
* If the base template with ID `{id}` doesn't exist, creates the unpublished base template
* If the base template with ID `{id}` exists and is unpublished, updates it
* If the base template with ID `{id}` exists and is published, returns a NotAllowed error (published revisions can't be updated)

If a revision is provided:
* If the base template with ID `{id}` doesn't exist, returns a NotFound error
* If the revision doesn't exist, creates an unpublished revision with ID `{revision}`
* If the revision exists and is unpublished, updates it
* If the revision exists and is published, returns a NotAllowed error (published revisions can't be updated)

## Request parameters
|Parameter | Type | Required? | Other validation |
|-|-|-|-|
|**id**| String | Yes| Must not be empty |

The ID of the content template.

|Parameter | Type | Required? | Other validation |
|-|-|-|-|
|**revision**| String | No | Must not be empty |

The ID of a particular revision of the content template.

| | Type | Required? | Other validation |
|-|-|-|-|
|**request body**| String | Yes | Must not be empty <br> Must be valid JSON for a content template |

The content template details to update, example:

    {
      "pageTitle": "string",
      "pageContent": {},
      "views": [
        {
          "viewId": "string",
          "viewContent": {},
          "viewModel": {},
          "childViews": [
            null
          ]
        }
      ]
    }

## Response

| Status code | Description |
|-|-|
| 405 Method Not Allowed | Incorrect request method - only `POST` allowed |
| 400 Bad Request | Missing or invalid request parameters |
| 404 Not Found | Content template not found for the given ID (if revision supplied) |
| 200 OK | Content template/revision successfully created/updated |

### Example response body

    (empty)