There are four different aspects of ASP 2.0 that will need to have some sort of versioning to manage them:

* **Code**
Each new release of code e.g. new features or bug fixes will have a different version number, probably using semantic versioning, so we can keep track of what version of the code is deployed to which environment.
* **Data** 
Each dataset (e.g. KeyStage2, QLA) goes through multiple releases, broken down by year (2022, 2023) and version within that year (provisional, revised, final). Each report within ASP needs to be aware of what dataset the data it's using corresponds to, and what year/version of that data.
* **Page content templates**
The page content of each report page in ASP is defined using a page content template, which sets out what components are used on a page and what data fields the components are being populated with. We will want to be able to make copies of these templates, track what changes were made by which users, and revert to a particular version. We will need to have draft and published versions of a given template as we will want to be working on it to update it without affecting the version that's currently live.
* **Datasets** (dataset definitions)
As the page content template engine is a shared service that doesn't have any concept of ASP datasets, we will need a way of wiring up a particular version of a page content template with a particular version of the data that's driving it. Datasets define the report pages that are contained within each dataset on ASP, what page template each report uses and the data to query to populate each report. These also need to be versioned as we will need to work on the next data release (which will include changes to the data and page templates) without affecting the current live version.

This presentation goes step-by-step through a few data releases, showing the sorts of changes that might be made, and how versioning helps this process: [ASP 2.0 Release.pptx](/.attachments/ASP%202.0%20Release-7f9dec12-a580-49f0-9288-fabb8183df58.pptx)

The **Code** and **Data** aspects will have their own approaches to versioning which we won't consider here, but **Page content templates** and **Datasets** are similar enough to be considered together when we decide what data structure to use. Both of these have the following characteristics:

* Each entity (i.e. template or dataset) needs to be uniquely identified
* Each entity needs to have several versions
* Each version needs to be uniquely identified
* Each version should know what entity it's a version of
* Each version should have a timestamp
* There should be only one published version per entity
* Getting the published/latest version for an entity should be optmised for speed/cost as this will be a common operation
* Creating/updating versions is rare and so can be more inefficient

There are two alternatives for the data structure:
* Document versioning pattern
* Same container

## Document versioning pattern
* https://www.mongodb.com/blog/post/building-with-patterns-the-document-versioning-pattern
* https://mensetopera.medium.com/the-document-versioning-pattern-in-azure-cosmos-db-4db140b9b240

This requires two containers, one container for just the published versions of the entities, and one container for the other versions

### Advantages
* Each published version corresponds to a single document within the published versions container, and so querying the published version is very efficient (a single point-read)
* Creating a new draft versions is fast:
  * Just create a new document in the *versions* container with the entity id
* Updating a draft versions is fast:
  * Just update the document for the specific version id in the *versions* container

### Disadvantages
* Requires creating a second container for each entity that needs versioned
* Publishing a version is complex:
  * The document in the *published* container must be replaced with the document from the *versions* container that is going live
  * The previously published document that is being replaced must be moved into the *versions* container
* In addition, the publish operation cannot be transactional, as transactions in Cosmos DB are limited to a single partition within a container, and cannot cross partitions, let alone multiple containers. So if the operation fails midway through the data could be left in an inconsistent state
* Published versions are treated differently to unpublished versions, so extra work would be needed to view the published version of an entity in the same way as an unpublished entity (e.g. in a preview function similar to WordPress)

## Same container
One alternative could be to store all the published and unpublished versions of each entity in the same container. This would need a way of identifying which entity each version belongs to (an "entity id" field as well as an "id" field), and which version is the published one (a simple boolean `isPublished` field)

### Advantages
* Creating a new draft versions is fast:
  * Just create a new document with the entity id
* Updating a draft versions is fast:
  * Just update the document with the specific version id
* Publishing a version is fast:
  * Just set the `isPublished` flag to `false` on the currently published version, and `true` on the new version
* Publish operation can now be transactional, provided all the versions of an entity fall within a single partition in the container (which they should, as they should share the same partition key as the entity)

### Disadvantages
* Getting the published version of an entity can no longer be done with a point-read but must be a query (as it must have the clause `WHERE c.isPublished = true`)

## Transaction or stored procedure?
In order to ensure the publish operation is atomic (i.e. the operation either succeeds completely or fails entirely, so that data is not left in an inconsistent state) there are two options: [transactional batch operations](https://learn.microsoft.com/en-us/azure/cosmos-db/nosql/transactional-batch?tabs=dotnet) or [stored procedures](https://learn.microsoft.com/en-us/azure/cosmos-db/nosql/stored-procedures-triggers-udfs).

### Stored procedures
* Scoped to a single partition within a container
* Written in Javascript
* Created within CosmosDB so will require recreating/updating on target database when deploying

### Transactional batch operations
* Scoped to a single partition within a container
* .NET API support, so can be deployed with the codebase
* The Azure Cosmos DB request size limit constrains the size of the TransactionalBatch payload to not exceed 2 MB, and the maximum execution time is 5 seconds.
* There's a current limit of 100 operations per TransactionalBatch to ensure the performance is as expected and within SLAs
* [Latency reduction of up to 30%](https://devblogs.microsoft.com/cosmosdb/introducing-transactionalbatch-in-the-net-sdk/) compared with stored procedures

So long as we're looking at < 2 MB request payloads and < 100 operations, seems like transactional batch operations are the way to go

## Current implementation
In ASP 2.0 so far only page content templates have been implemented (the `content` container in Cosmos DB) and versioning has been implemented using the `Same container` pattern - each document has an `id` field (the revision id), a `contentId` (the base document id) and an `isPublished` field. The API retrieves either the published version of each template, the published version of a specific template, or a particular revision (see the [Content template API endpoints](/Analyse-School-Performance-\(ASP\)-Index/Technical-specification/API/Content-templates)

The domain logic assumes that there is at most one published revision for each content template, but as the code to publish a revision is not yet implemented, this has been tested by hand-editing the documents directly in the database (or in-memory data setup in the functional tests.) Thus the choice of implementation of the publish operation above is left to the future developer.