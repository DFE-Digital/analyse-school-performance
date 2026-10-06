## Dataset data
* Currently ASP: Cosmos DB
- [ ] Cosmos DB vs Blob storage?
  - Cosmos DB
    - [x] Research: Single or multiple containers?
    - [x] Research: Partition strategy within container
    - [x] Research: Document structure within partition
    - [x] Document research
    - [x] Meeting with data team
    - [ ] POC
  - Blob storage
    - [ ] Research: Metadata queries
    - [ ] Research: Throughput
    - [ ] Research: Latency
    - [ ] Research: Cost
    - [ ] Document research
    - [ ] Meeting with data team
    - [ ] POC

## Establishments/MATs
* Current ASP: Cosmos DB
- [ ] Cosmos DB vs Blob storage? (need Cognitive search to index)
  - [ ] POC
- [ ] Relational database (use native query capability)

## Dataset definitions
* Current ASP: N/A
- [ ] Cosmos DB vs Blob storage?
  - [x] Cosmos DB
    - [x] POC
  - [ ] Blob storage
    - [ ] POC
- [x] Document versioning
  - [x] Separate vs single container
  - [x] POC

## Page content templates
* Current ASP: N/A
- [ ] Cosmos DB vs Blob storage?
  - [x] Cosmos DB
    - [x] POC
  - [ ] Blob storage
    - [ ] POC
- [x] Document versioning
  - [x] Separate vs single container
  - [x] POC

## User UPNs (QLA)
* Current ASP: Azure table storage
- [ ] Research options

## Audit log
* Current ASP: Azure table storage
- [ ] Research options

## Data downloads
* KTS downloads
* Current ASP: Blob storage
  - [ ] Research options
* ASP downloads
  - Current ASP: Cosmos DB
  - [ ] Research options

## Summary reports
* Current ASP: Dynamically generated
- [ ] Research: Blob storage

## LAs
* Current ASP: Hardcoded list
- [ ] Research: storage options

## Anything else missing
- [ ] Research: Check all data stored in current ASP

## Data retention
- [ ] Find out if there is a DfE data retention policy
- [ ] How long to keep historical data?
  - Suggestion: 5 years
- [ ] When should current (interactive) reports become unavailable?
  - Suggestion: as soon as new release for that DataSet goes live
- [ ] Research: How should old data be cleared down?
  - [ ] Research: Cold storage?
  - [ ] Research: Manual or automatic process?
- [ ] When to clear down previous dataset versions (provisional/revised/final) after a release?

---

* Blob metadata (8KB) https://learn.microsoft.com/en-us/rest/api/storageservices/setting-and-retrieving-properties-and-metadata-for-blob-resources
  * https://learn.microsoft.com/en-us/rest/api/storageservices/list-blobs?tabs=azure-ad
  * https://learn.microsoft.com/en-us/rest/api/storageservices/naming-and-referencing-containers--blobs--and-metadata
  * https://learn.microsoft.com/en-us/rest/api/storageservices/enumerating-blob-resources
  * https://learn.microsoft.com/en-us/azure/storage/common/storage-use-azurite?tabs=visual-studio
* https://learn.microsoft.com/en-us/azure/storage/blobs/storage-performance-checklist#reading-data
  * In general, reading data once is preferable to reading it twice. Consider the example of a web application that has retrieved a 50 MiB blob from the Azure Storage to serve as content to a user. Ideally, the application caches the blob locally to disk and then retrieves the cached version for subsequent user requests.

  * One way to avoid retrieving a blob if it hasn't been modified since it was cached is to qualify the GET operation with a conditional header for modification time. If the last modified time is after the time that the blob was cached, then the blob is retrieved and re-cached. Otherwise, the cached blob is retrieved for optimal performance.
  * https://learn.microsoft.com/en-us/rest/api/storageservices/specifying-conditional-headers-for-blob-service-operations
* https://learn.microsoft.com/en-us/azure/storage/blobs/storage-performance-checklist#capacity-and-transaction-targets
  * If your application hits the transaction target, consider using block blob storage accounts, which are optimized for high transaction rates and low and consistent latency. For more information, see Azure storage account overview.
  * A single blob supports up to 500 requests per second. If you have multiple clients that need to read the same blob and you might exceed this limit, then consider using a block blob storage account. A block blob storage account provides a higher request rate, or I/O operations per second (IOPS).
  * If you have a large number of clients accessing a single blob concurrently, you need to consider both per blob and per storage account scalability targets. The exact number of clients that can access a single blob varies depending on factors such as the number of clients requesting the blob simultaneously, the size of the blob, and network conditions.
  * Alternatively, you can temporarily copy the blob to multiple storage accounts to increase the total IOPS per blob and across storage accounts
* https://learn.microsoft.com/en-us/azure/storage/blobs/storage-performance-checklist#partitioning
  * Blob storage uses a range-based partitioning scheme for scaling and load balancing. Each blob has a partition key comprised of the full blob name (account+container+blob). The partition key is used to partition blob data into ranges. The ranges are then load-balanced across Blob storage.
  * If your application is approaching the scalability targets, then make sure that you're using an exponential backoff for retries.
  * Range-based partitioning means that naming conventions that use lexical ordering (for example, mypayroll, myperformance, myemployees, etc.) or timestamps (log20160101, log20160102, log20160102, etc.) are more likely to result in the partitions being co-located on the same partition server until increased load requires that they're split into smaller ranges. Co-locating blobs on the same partition server enhances performance, so an important part of performance enhancement involves naming blobs in a way that organizes them most effectively.
  * If possible, use blob or block sizes greater than 256 KiB for standard and premium storage accounts. Larger blob or block sizes automatically activate high-throughput block blobs. High-throughput block blobs provide high-performance ingest that isn't affected by partition naming.
  * Examine the naming convention you use for accounts, containers, blobs, tables, and queues. Consider prefixing account, container, or blob names with a three-digit hash using a hashing function that best suits your needs.
* https://learn.microsoft.com/en-us/rest/api/storageservices/understanding-block-blobs--append-blobs--and-page-blobs
* https://learn.microsoft.com/en-us/azure/storage/common/scalability-targets-standard-account