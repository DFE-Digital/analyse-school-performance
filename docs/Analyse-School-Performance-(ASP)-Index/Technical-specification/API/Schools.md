# GET /schools/
Retrieves a list of schools within a specified scope.

    GET /schools?searchTerm={searchTerm}&scope={scope}&scopeId={scopeId}&page={page}
        &resultsPerPage={resultsPerPage}

[View OpenAPI documentation and try out requests for this endpoint](https://s192d01-func-dev-01.azurewebsites.net/api/swagger/ui#/Schools/SchoolsGetAll)

## Request parameters

|Parameter | Type | Required? | Other validation |
|-|-|-|-|
|**searchTerm**| String| No | If supplied, should not be empty |

A search term to filter schools by - if supplied, the list of schools will be filtered to only those that match the search term. This can be either:

* a full URN e.g. `123456`
* a full LAESTAB code e.g. `894/2200` or `8942200` (with or without the forward slash), or either the LA part (`894`) or the ESTAB part (`2200`)
* part of the name or address of the establishment (case insensitive)
* full postcode of the establishment, or either the first part or the second part of the postcode (case insensitive)

|Parameter | Type | Required? | Other validation |
|-|-|-|-|
|**scope**| String | No |  Must be either `LA` or `MAT` or `Diocese` <br> Case insensitive | 
|**scopeId**| String | Yes - if **scope** is supplied |  If supplied, should be a valid LA code, MAT UID or Diocese name |

Used to filter the list of schools to only those schools that belong to the specified LA, MAT or Diocese. If supplied, **scope** is the scope of the search, i.e. `LA`, `MAT` or `Diocese`, and **scopeId** is the identifier for the selected scope, i.e. the LA code, MAT UID or Diocese name.

|Parameter | Type | Required? | Other validation |
|-|-|-|-|
|**page**| Int | No | If supplied, should be greater than zero |
|**resultsPerPage**| Int | No | If supplied, should be greater than zero |

Used for pagination of the results: 
* **page** is the page number (defaults to 1 if not supplied)
* **resultsPerPage** is the number of results in each page (defaults to 50 if not supplied)


## Response

| Status code | Description |
|-|-|
| 405 Method Not Allowed | Incorrect request method - only `GET` allowed |
| 400 Bad Request | Missing or invalid request parameters |
| 404 Not Found | Could not find any schools within the specified scope matching the given search term |
| 200 OK | At least one result was found |

### Example response body
```
{
  "page": 0,
  "resultsPerPage": 0,
  "totalResults": 0,
  "results": [
    {
      "urn": "string",
      "name": "string",
      "educationPhase": "string",
      "address": "string",
      "laestab": "string"
    }
  ]
}
```
---
# GET /schools/search-suggestions

Retrieves a list of school search suggestions within a specified scope based on a search term.

    GET /schools/search-suggestions?searchTerm={searchTerm}&scope={scope}&scopeId={scopeId}
        &maxSuggestions={maxSuggestions}

[View OpenAPI documentation and try out requests for this endpoint](https://s192d01-func-dev-01.azurewebsites.net/api/swagger/ui#/Schools/SchoolsGetSearchSuggestions)

## Request parameters
|Parameter | Type | Required? | Other validation |
|-|-|-|-|
|**searchTerm**| String| Yes | Must not be empty |

A search term to get suggestions for - the list of suggestions will be filtered to only those that match the search term. This can be either:

* a full or partial URN e.g. `123456` or `234` 
* a full or partial LAESTAB code e.g. `894/2200` or `94/2` or `422` (with or without the forward slash)
* part of the name or address of the establishment (case insensitive)
* part of the postcode of the establishment (case insensitive)

|Parameter | Type | Required? | Other validation |
|-|-|-|-|
|**scope**| String | No |  Must be either `LA` or `MAT` or `Diocese` <br> Case insensitive | 
|**scopeId**| String | Yes - if **scope** is supplied |  If supplied, should be a valid LA code, MAT UID or Diocese name |

Used to filter the scope of the search - if supplied, **scope** is the scope of the search, i.e. `LA`, `MAT` or `Diocese`, and **scopeId** is the identifier for the selected scope, i.e. the LA code, MAT UID or Diocese name. Filters the search to only those schools that belong to the specified LA, MAT or Diocese.

|Parameter | Type | Required? | Other validation |
|-|-|-|-|
|**maxSuggestions**| Int | No | If supplied, should be greater than zero |

If supplied, specifies the maximum number of search suggestions to return. Defaults to 10 if not supplied.

## Response

| Status code | Description |
|-|-|
| 405 Method Not Allowed | Incorrect request method - only `GET` allowed |
| 400 Bad Request | Missing or invalid request parameters |
| 404 Not Found | Could not find any schools within the specified scope matching the given search term |
| 200 OK | At least one result was found |

### Example response body
    [
      {
        "urn": "string",
        "name": "string",
        "address": "string",
        "laestab": "string"
      }
    ]

---
# GET /schools/{urn}
Retrieves details for a specific school based on a given URN.

    GET /api/schools/{urn}

[View OpenAPI documentation and try out requests for this endpoint](https://s192d01-func-dev-01.azurewebsites.net/api/swagger/ui#/Schools/SchoolsGetSingle)

## Request parameters

|Parameter | Type | Required? | Other validation |
|-|-|-|-|
|**urn**| Int | Yes| Must be of length 6 |

The URN of the school.

## Response

| Status code | Description |
|-|-|
| 405 Method Not Allowed | Incorrect request method - only `GET` allowed |
| 400 Bad Request | Missing or invalid request parameters |
| 404 Not Found | No school found for the given URN |
| 200 OK | A school was found with the given URN |

### Example response body

    {
      "urn": "string",
      "laestab": "string",
      "name": "string",
      "address": "string",
      "educationPhase": "string",
      "establishmentType": "string",
      "gender": "string",
      "headTeacher": "string",
      "ageRange": "string",
      "religiousDenomination": "string",
      "admissionsPolicy": "string",
      "resourcedProvisionType": "string",
      "diocese": "string",
      "noOfPupils": "string",
      "multiAcademyTrust": "string",
      "localAuthority": {
        "code": "string",
        "name": "string"
      }
    }

---

# GET /schools/{urn}/access
Determines whether a school is accessible within a specified scope.

    GET /api/schools/{urn}/access?scope={scope}&scopeId={scopeId}

[View OpenAPI documentation and try out requests for this endpoint](https://s192d01-func-dev-01.azurewebsites.net/api/swagger/ui#/Schools/SchoolsGetAccess)

## Request parameters

|Parameter | Type | Required? | Other validation |
|-|-|-|-|
|**urn**| Int | Yes| Must be of length 6 |

The URN of the school.

|Parameter | Type | Required? | Other validation |
|-|-|-|-|
|**scope**| String | No |  Must be either `LA` or `MAT` or `Diocese` <br> Case insensitive | 
|**scopeId**| String | Yes - if **scope** is supplied |  If supplied, should be a valid LA code, MAT UID or Diocese name |

Used to determine the schools's accessibility - if supplied, **scope** is the scope of the search, i.e. `LA`, `MAT` or `Diocese`, and **scopeId** is the identifier for the selected scope, i.e. the LA code, MAT UID or Diocese name. Checks whether the school is accessible from within the specified LA, MAT or Diocese. If not supplied, the schools is accessible by default.

## Response

| Status code | Description |
|-|-|
| 405 Method Not Allowed | Incorrect request method - only `GET` allowed |
| 400 Bad Request | Missing or invalid request parameters |
| 404 Not Found | No school found for the given URN |
| 200 OK | A school was found with the given URN |

### Example response body

    {
      "isAccessibleInScope": true,
      "isAccessibleViaLinkedSchools": true
    }

---
# GET /schools/{urn}/linked-schools
Retrieves linked schools for a specific school based on a given URN.

    GET /schools/{urn}/linked-schools

[View OpenAPI documentation and try out requests for this endpoint](https://s192d01-func-dev-01.azurewebsites.net/api/swagger/ui#/Schools/SchoolsGetLinkedSchools)

Whenever an establishment changes significantly, e.g. changes name, becomes a different type of establishment, merges with another establishment or splits off into different establishments, one or more new establishment records are created, and links created between the establishments in both directions. For example if two establishments merge to create one new establishment, the two merged establishments will contain a link to the new establishment, and the new establishment will contain links back to the merged establishments.  

Whenever a new establishment is created, any future data releases are attached to the new URN, so in ASP we need to give the user a way to view multiple establishment records in order to see the data that's reported against the different URNs it may have been given. The user needs to know if the establishment record they are looking at links to any other establishment records, what the type of link is, and have a way to navigate to the linked records.

The purpose of this endpoint is to encapsulate the functionality of identifying the links that are relevant to ASP, structuring them in a way that allows easy display in the UI, and providing a friendly description for each link.

## Request parameters

|Parameter | Type | Required? | Other validation |
|-|-|-|-|
|**urn**| Int | Yes| Must be of length 6 |

The URN of the school.

## Response

| Status code | Description |
|-|-|
| 405 Method Not Allowed | Incorrect request method - only `GET` allowed |
| 400 Bad Request | Missing or invalid request parameters |
| 404 Not Found | No school found for the given URN |
| 200 OK | A school was found with the given URN |

### Example response body
```
[
  {
    "date": "string",
    "linkType": {
      "code": "string",
      "name": "string"
    },
    "establishments": [
      {
        "urn": "string",
        "name": "string"
      }
    ],
    "description": "string"
  }
]
```