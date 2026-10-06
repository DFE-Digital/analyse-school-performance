# GET /local-authorities
Retrieves a paginated list of all local authorities.

    GET /local-authorities?searchTerm={searchTerm}&page={page}&resultsPerPage={resultsPerPage}

[View OpenAPI documentation and try out requests for this endpoint](https://s192d01-func-dev-01.azurewebsites.net/api/swagger/ui#/Local%20Authorities/LocalAuthoritiesGetAll)

## Request parameters

|Parameter | Type | Required? | Other validation |
|-|-|-|-|
|**searchTerm**| String| No | If supplied, should not be empty |

A search term to filter local authorities by - if supplied, the list of local authorities will be filtered to only those that match the search term. This can be either:

* a full LA code e.g. `301`
* part of the name of the local authority (case insensitive)

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
| 404 Not Found | Could not find any local authorities for the given search term |
| 200 OK | At least one result was found |

### Example response body
    {
      "page": 0,
      "resultsPerPage": 0,
      "totalResults": 0,
      "results": [
        {
          "code": "string",
          "name": "string"
        }
      ]
    }
---

# GET /local-authorities/search-suggestions
Provides suggestions for local authorities based on a given search term.

    GET /local-authorities/search-suggestions?searchTerm={searchTerm}
        &maxSuggestions={maxSuggestions}

[View OpenAPI documentation and try out requests for this endpoint](https://s192d01-func-dev-01.azurewebsites.net/api/swagger/ui#/Local%20Authorities/LocalAuthoritiesGetSearchSuggestions)


## Request parameters
|Parameter | Type | Required? | Other validation |
|-|-|-|-|
|**searchTerm**| String| Yes | Must not be empty |

A search term to get suggestions for - the list of suggestions will be filtered to only those that match the search term. This can be either:

* a full or partial LA code e.g. `301` or `30` 
* part of the name of the local authority (case insensitive)

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
| 404 Not Found | No multi-academy trust found for the given UID |
| 200 OK | At least one result was found |

### Example response body
    [
      {
        "code": "string",
        "name": "string"
      }
    ]
---
# GET /local-authorities/{code}
Retrieves details for a specific local authority based on a given code.

    GET /local-authorities/{code}

[View OpenAPI documentation and try out requests for this endpoint](https://s192d01-func-dev-01.azurewebsites.net/api/swagger/ui#/Local%20Authorities/LocalAuthoritiesGetSingle)

## Request parameters

|Parameter | Type | Required? | Other validation |
|-|-|-|-|
|**code**| Int | No | If supplied, must be of length 3 |

The local authority code.

## Response

| Status code | Description |
|-|-|
| 405 Method Not Allowed | Incorrect request method - only `GET` allowed |
| 400 Bad Request | Missing or invalid request parameters |
| 404 Not Found | No local authority found for the given code |
| 200 OK | A local authority was found with the given code |

### Example response body
    {
      "code": "string",
      "name": "string"
    }