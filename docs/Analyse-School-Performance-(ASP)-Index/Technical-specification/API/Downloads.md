# GET /downloads
Retrieves available downloads for a given year within a specified scope.

    GET /downloads?scope={scope}&scopeId={scopeId}&year={year}

[View OpenAPI documentation and try out requests for this endpoint](https://s192d01-func-dev-01.azurewebsites.net/api/swagger/ui#/Downloads/DownloadsGetAll)

## Request parameters

|Parameter | Type | Required? | Other validation |
|-|-|-|-|
|**scope**| String | Yes |  Must be either `School` or `LA` <br> Case insensitive | 
|**scopeId**| String | Yes |  Must be a valid school URN or LA code |

Used to filter the scope of the search - **scope** is the scope of the available downloads e.g. `School` or `LA`, and **scopeId** is the identifier for the selected scope, identifier for the selected scope: either a school URN or LA code. Filters the available downloads to the specified School or LA.

|Parameter | Type | Required? | Other validation |
|-|-|-|-|
|**year**| Int | No |  Must be greater than zero | 

Filters the available downloads to only those for the specified year.

## Response

| Status code | Description |
|-|-|
| 405 Method Not Allowed | Incorrect request method - only `GET` allowed |
| 400 Bad Request | Missing or invalid request parameters |
| 404 Not Found | No available downloads for the given year within the specified scope |
| 200 OK | All available downloads for the given year within the specified scope |

### Example response body
    {
      "year": 0,
      "downloads": [
        {
          "id": "string",
          "label": "string",
          "source": {
            "rawValue": "string"
          },
          "year": 0,
          "datasetType": {
            "rawValue": "string"
          },
          "version": {
            "rawValue": "string",
            "friendlyName": "string",
            "priority": 0
          },
          "baseId": "string"
        }
      ],
      "availableDates": [
        {
          "year": 0,
          "description": "string"
        }
      ]
    }

---

# GET /downloads/package
Creates a ZIP archive of multiple downloads within a specified scope based on a list of download file IDs.

    GET /downloads/package?scope={scope}&scopeId={scopeId}&fileType={fileType}
        &downloadIds={downloadIds}

[View OpenAPI documentation and try out requests for this endpoint](https://s192d01-func-dev-01.azurewebsites.net/api/swagger/ui#/Downloads/DownloadsGetPackage)

## Request parameters

|Parameter | Type | Required? | Other validation |
|-|-|-|-|
|**scope**| String | Yes |  Must be either `School` or `LA` <br> Case insensitive | 
|**scopeId**| String | Yes |  Must be a valid school URN or LA code |

Used to validate the scope of the requested downloads - **scope** is the scope of the available downloads e.g. `School` or `LA`, and **scopeId** is the identifier for the selected scope, identifier for the selected scope: either a school URN or LA code. If any of the supplied download IDs fall outside of this scope, an error is returned.

|Parameter | Type | Required? | Other validation |
|-|-|-|-|
|**fileType**| String | Yes |  Must be either `csv`, `txt` or `xlsx`<br>Case insensitive| 

Type of the files to be downloaded, e.g. `csv`, `txt` or `xlsx`.

|Parameter | Type | Required? | Other validation |
|-|-|-|-|
|**downloadIds**| String | Yes |  Must not be empty<br>Multiple values allowed | 

List of file download IDs to be downloaded as a ZIP.

## Response

| Status code | Description |
|-|-|
| 405 Method Not Allowed | Incorrect request method - only `GET` allowed |
| 400 Bad Request | Missing or invalid request parameters |
| 404 Not Found | No file was found with the requested download ID |
| 200 OK | The ZIP file stream containing the requested files |

### Example response body

    (ZIP file stream)

---
