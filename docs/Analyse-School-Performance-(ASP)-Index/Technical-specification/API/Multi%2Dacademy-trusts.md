# GET /multi-academy-trusts/{uid}
Retrieves details for a specific multi-academy trust based on a given UID.

    GET /multi-academy-trusts/{uid}

[View OpenAPI documentation and try out requests for this endpoint](https://s192d01-func-dev-01.azurewebsites.net/api/swagger/ui#/Multi-Academy%20Trusts/MultiAcademyTrustsGetSingle)

## Request parameters

|Parameter | Type | Required? | Other validation |
|-|-|-|-|
|**uid**| Int | No | If supplied, must be of length 4 |

The UID of the multi-academy trust.

## Response

| Status code | Description |
|-|-|
| 405 Method Not Allowed | Incorrect request method - only `GET` allowed |
| 400 Bad Request | Missing or invalid request parameters |
| 404 Not Found | No multi-academy trust found for the given UID |
| 200 OK | At least one result was found |

### Example response body
    {
      "uid": "string",
      "name": "string"
    }