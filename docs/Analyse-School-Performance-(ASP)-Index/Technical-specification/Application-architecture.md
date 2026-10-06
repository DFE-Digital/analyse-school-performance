[[_TOC_]]

# Overview

Here is the ASP 2.0 clean architecture diagram again we discussed in [Development principles](/Analyse-School-Performance-\(ASP\)-Index/Technical-specification/Development-principles):

![image.png](/.attachments/image-2d81c664-12d2-4f1b-945b-82c1815e39d6.png)

Here's a more concrete diagram showing the different projects in the solution and how they depend on each other:

![image.png](/.attachments/image-15ad82c5-cc48-4b4b-a169-f5c68b0a6af8.png)

Note the different interfaces (represented as thin grey rectangles) that reflect a point of abstraction, or an application boundary. Note also that weird long arrow between `ASP.Api.Client.InProcess` and `ASP.Api` - this illustrates that the connection to the API can be swapped out for an in-process connection - we'll come on to that shortly.

# Overview of src projects
![image.png](/.attachments/image-159e14f7-dbc2-4e50-82dd-b6841b6722de.png)

## <span>ASP.Web</span>
This is the ASP web application - the entry point for the user.

## ASP.Web.Components
This project contains the implementations of web components used by the [Page template engine](/Analyse-School-Performance-\(ASP\)-Index/Technical-specification/Content-template-engine/Dataset-report-page-rendering). These are extracted as a separate project with the aim of creating a NuGet package at a later date containing common components that other services can make use of.

## ASP.Web.Core
This contains web-specific classes that aren't necessarily specific to a particular service. The main reason for this existing is due to the extraction of ASP.Web.Components as a separate project. Since this is referenced by ASP.Web in order to include the components within the ASP web application, there needed to be a home for component-related code that needed to be referenced by both projects.

## ASP.Domain
The centre of our "onion", containing our domain objects and the use cases that marshal them.

## ASP.Domain.Repositories
This is where the repositories for the Domain live. The reason they are extracted into a separate project is because although there aren't any explicit dependencies on infrastructure projects, the repositories use LINQ to Objects over a set of DAO objects (LINQ to Cosmos for the Cosmos implementation), and the structure of these objects _is_ an infrastructure concern. In other words the objects themselves and the data fields within them are dictated by the choice of using Cosmos DB and the data pipelines populating them. 

If that were to change, then a new repository project would need to be created with the DAO objects relevant to the new infrastructure (or even a different method than LINQ to Objects would need to be used.)

## ASP.Api
The ASP API (Azure function app) - this is how the web application (and potentially other applications/services) interact with the domain. Each endpoint corresponds to a separate use case.

## ASP.Api.Client
Abstracts the connection between the web application and the API. This can be switched to a HTTP or in-process connection via the `HttpTransportLayer` or `InProcessTransportLayer` implementations of the `ITransportLayer` interface

## ASP.Api.Client.InProcess
The in-process implementation of the API client connection.

## ASP.Infrastructure
Contains the interfaces needed for interacting with Azure resources (e.g. `IDocumentDatabase`, `IBlobStorage`), and also contains the in-memory implementations of these. 

## ASP.Infrastructure.Azure
Contains the Azure implementations of the infrastructure interfaces.

## ASP.Infrastructure.CosmosDbSeeder
This is used to populate Cosmos DB/Blob Storage with a working set of reference data after an environment has been brought up, so the service can function with minimal manual setup - see [Developer setup](/Analyse-School-Performance-\(ASP\)-Index/New-starter-instructions/Developer-setup).

## ASP.Core
Contains cross-cutting concerns common to all projects, e.g. the `Result<T>` object, extension methods on CLR classes, pagination of lists etc.

# In-memory storage
Each storage interface (`IDocumentDatabase`, `IBlobStorage`, `ITableStorage`) has the ability to be swapped out with an in-memory implementation. This effectively allows these interfaces to draw a boundary around the system as a whole (e.g. the API or the web app), allowing the functional tests to exercise as much of the stack as possible (see [Functional tests](/Analyse-School-Performance-\(ASP\)-Index/Technical-specification/Functional-tests)).

The way this is configured, each method of storage has its own set of options (e.g. `DocumentDatabaseOptions`, `BlobStorageOptions` etc) which each correspond to a section of the app settings configuration, e.g.:

```
"DocumentDatabase": {
  "InMemory": true
},
"BloblStorage": {
  "InMemory": true
}
```

The default implementation is the in-memory version, which is implemented using a singleton instance of `MemoryStore` (which is just a wrapper around a `Dictionary<string, Dictionary<TKey, MemoryStoreItem<TKey>>>`, where the outer dictionary key value is the container name, and the inner dictionary is a mapping from id -> object)

This is fine for running functional tests, however if a connection to a real store is needed, more settings values need to be given:

```
"DocumentDatabase": {
  "InMemory": false,
  "EndpointUri": "https://s192d01-cdb-dev.documents.azure.com:443/",
  "PrimaryKey": "<REDACTED>"

},
"BloblStorage": {
  "InMemory": false,
  "StorageAccountName": "s192d01strdev",
  "PrimaryKey": "<REDACTED>"
}
```
## Distinction between repository and infrastructure interfaces
Infrastructure interfaces as discussed above are abstractions over a connection to a storage provider e.g. Azure Cosmos DB or Azure Blob Storage, where the interface is hopefully generic enough to allow an implementation for another storage provider. The main reason for these is to allow a real implementation to be swapped with an in-memory implementation for testing. These interfaces are contained in `ASP.Infrastructure`.

Repository interfaces (e.g. `ISchoolRepository`, `ILocalAuthorityRepository`) are slightly different. These are defined in `ASP.Domain`, and define a boundary between the Domain Objects (which encapsulate the business logic functionality) and the Domain Repositories (whose role is to fetch complete Domain Objects from a store). As mentioned above, the implementations for these are found in `ASP.Domain.Repositories` and encapsulate LINQ queries over DAO objects.

Using LINQ is what provides a lot of the value of the in-memory implementation, because the same LINQ queries defined in the repositories can be either executed against a list of objects in memory within the `MemoryStore`, or can be converted to SQL via the .NET Cosmos client using LINQ to Cosmos.

Admittedly there may be cases in which certain LINQ queries execute successfully in memory but fail when converted into SQL, however these cases will hopefully be caught by integration tests, and as most queries are relatively simple, this arrangement works very well for the most part.

## Testing with real databases

Although the in-memory implementations are intended for use in the functional tests due to speed constraints, this is only really an issue when running the tests locally. In fact, it might be more useful to use a real database for running tests in CI/CD pipelines. The way the test framework is structured, it is very easy to configure the same suite of tests that used an in-memory database for local tests, to use a real database for CI/CD tests, just by setting the `InMemory` flag to false in the configuration and supplying the appropriate connection strings.

The original intention was for this to actually be the case for the CI/CD tests but this was never implemented.

# In-process API
In a similar vein, the web application needs a connection to the API, but this connection is abstracted by the `ITransportLayer` interface. This allows there to be two implementations: 
* a HTTP implementation which sends HTTP requests to a physical endpoint, and 
* an in-process implementation which creates instances of the API function classes in-process and sends mock HTTP requests to those function objects directly.

There are two advantages to having the in-process implementation:
1. It allows for much faster tests as a real API does not need to be spun up (which would incur large setup costs in terms of time for each test run), and means each API request is a function call rather than a network request
2. It allows the production environment to be much more performant, as the API is effectively hosted within the application service, and API calls do not incur the network latency if it were a separate function app. See the image below for an illustration of difference in the request flow between the in-process or HTTP implementations:

![image.png](/.attachments/image-bcdbd4fb-7daf-486b-8e9b-54d1723ace92.png)

Note that it's the `ITransportLayer` interface which is either implemented using `InProcessTransportLayer` in `ASP.Api.Client.InProcess` or `HttpTransportLayer` in `ASP.Api.Client`.

As for the in-memory implementations, the in-process API can be turned on or off by setting the `InProcess` flag in app settings. This time the HTTP implementation is the default one:

    "Api": {
        "InProcess": false,
        "EndpointBaseUrl": "http://localhost:7116",
        "FunctionsKey": "<REDACTED>"
    }

And to turn the in-process implementation on, set `InProcess` to true:

    "Api": {
        "InProcess": true
    }

## Testing with a real API
Similar to the **Testing with real databases** section above, although the tests are configured to use the in-process API for locally running tests, this can (and probably should) be switched over to use the real API for the CI/CD tests. This would require a bit of thought as the API would need to be hosted somewhere in order to receive HTTP requests. [Aasim Ahmed](https://www.microsoft365.com/search/overview?pp=6759c6a7-7a60-43d7-a108-c6a3302331a3%40fad277c9-c60a-4da1-b5f3-b3b8b34a82f9%7cAasim.AHMED%40EDUCATION.GOV.UK&auth=2) was experimenting with using Docker to host a containerised web application and/or database to use for integration tests, this seemed like a promising approach.