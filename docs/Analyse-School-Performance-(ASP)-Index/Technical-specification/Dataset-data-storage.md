[**Note:** This functionality is not yet implemented in ASP 2.0]

[[_TOC_]]

For ASP 2.0 we need to think carefully about how to structure the data and make best use of the resources we have, and not make the same mistakes that have been made in the current version of ASP!

There are a few different types of data in ASP, e.g. Establishments, Multi Academy Trusts, and in ASP 2.0 we'll have new things like Page Content Templates and Dataset Definitions (objects that represent the reports and page content templates making up a Dataset, along with where to find the data to drive the page content templates) but these are small in comparison to the Dataset data itself, so this is the logical place to start when thinking about the efficiency of storage and queries in ASP 2.0.

**Aside:** there is a presentation covering most of this content at an overview level available here: [ASP 2.0 Data Structure.pptx](/.attachments/ASP%202.0%20Data%20Structure-119c2cf6-9079-4f97-bf93-839713e311ed.pptx)

## Data dimensions
Dataset data in ASP can be broken down in several ways:

* `DatasetType` (KeyStage1, QLA etc)
* `Scope` (National, LA, School, Pupil) 
* `Year` (2021/22, 2022/23 etc)
* `Version` (Provisional, Revised, Final)

The approach current ASP has taken is to create a new container for *every combination* of the above: `KS1_School_2022_Provisional`, `QLA_National_2023_Final` etc. This is probably the worst possible approach! Here are some reasons I can think of:

1. Querying data coverage (does data exist for each dataset for each year in a given scope?) means a *separate database query for each of the combinations* above (each combination is its own container and CosmosDB does not support cross-container queries)
1. Each of the combinations has so low individual throughput as to not justify the expense of having its own container (minimum 400 RU/s throughput per container)
1. Cosmos DB is unstructured (a container can hold data in any number of different structures - each document is an arbitrary JSON object) so there is no reason different types of data cannot exist in the same container.
1. Even data that's commonly used together is spread across containers (e.g. school, LA, national data for a given dataset are commonly all needed for a single report) - this introduces unnecessary inefficiency as getting this data will require multiple queries and code to join them together (Cosmos DB does not support cross-container joins). Data that is queried together should live together.
1. Data stored in each container is row-level (each document corresponds to a particular row in the original SQL table) which means each query returns multiple documents (sometimes hundreds) - this is not necessarily terrible, but we should think about the use cases, and best practice for certain use cases is to denormalize data (combine objects into a single parent object)

Although granted any other structure we choose to adopt for ASP 2.0 will be better that what we currently have, there are some considerations to take into account:

1. **Containers:** How do we split up the data into containers (if at all?)
2. **Partition keys:** How do we partition the data within each container for efficient querying/throughput?
3. **Denormalization:** How do we structure the data within a partition - do we have individual documents or one big document containing all the data, or somewhere in between?

To answer these we need to look at how the data is commonly used in ASP.

---
## Querying patterns
There are only a few types of Cosmos DB queries in ASP:

### LA/MAT/Diocese level
1. Get LA/national-level data for a given dataset for a specific LA (LA landing page: KS2 and KS4 only)
1. Does LA-level data exist (data coverage) for all datasets/years for a specific LA (downloads)
1. Get LA/national-level data for a given dataset/year for a specific LA (downloads)

### School level

4. Does LA/national/school/pupil-level data exist (data coverage) for each dataset for all years for a specific URN (to work out what reports to display)
1. Get LA/national/school/pupil-level data for a given dataset/year for a specific URN (to display a specific report)
1. Get LA/national/school/pupil-level data for all datasets for a given year for a specific URN (to display a summary report)
1. Does school-level data exist for each dataset/year for a specific URN (downloads)
1. Get LA/national/school/pupil-level data for a given dataset/year for a specific URN (downloads)

---
## Containers
The options as I see them are:
1. One container per combination of `DatasetType` x `Scope` x `Year` x `Version` (current ASP)
2. One container per `DatasetType`
3. One container for all `DatasetTypes`

### 1. One container per `DatasetType` x `Scope` x `Year` x `Version`
#### Advantages 
- Each container is completely isolated, can release one version of a dataset without affecting any other versions of that dataset or any other datasets. If a mistake is made in the release, just create a new empty container and release again, updating any references to point to the new container.

#### Disadvantages
- Any time you need to know what data is available for a given organisation you have to perform a separate query for each container and collate the results. And the more containers you have the longer this takes and the more expensive it is

#### Neutral
- As each `DatasetType`/`Scope`/`Year`/`Version` combination is a separate container, this requires some way of knowing which container corresponds to which combination. This could be done by convention, e.g. the name of the container is structured in a predefined way, or by configuration (the way it is done in current ASP is by hardcoding the container names in code, so that whenever a new container is needed the code has to be updated)

### 2. One container per `DatasetType`
#### Advantages
- Like (1), but only each `DatasetType` is isolated. This allows the data within each dataset to be easily queryable, like finding the latest year/version for a given URN
- Logical separation of datasets so can be easier to reason about/find data
- Reduced cost compared to (1) - throughput is much more expensive than storage, so although the same amount of data is stored in both (1) and (2), it will be much cheaper to have fewer containers, especially if throughput is low (as it generally is on ASP)

#### Disadvantages
- Similar to (1), but not as bad, any query that involves multiple datasets has to involve a separate query for each dataset and collation of the results in code
* Have to be careful when releasing a dataset year/version not to overwrite or add to an existing one - would be difficult to correct. Possible mitigations: 
  * Use auto-generated timestamp field to identify documents that were created on a particular release
  * Enforce [Insert-only role-based access](https://joonasw.net/view/access-data-in-cosmos-db-with-managed-identities) to release pipeline database user? 

#### Neutral
- Like (1), something needs to know which container represents which DataSet - this is best done by convention on the container name
- Each document must have `Year`, `Version`, `Scope` properties defined with predefined values, ideally sortable in a logical way (e.g. if the values for Version are `Provisional`, `Revised`, `Final` this won't work as their sort order would be alphabetical. Better values might be `1` for provisional, `2` for revised and `3` for final.

### 3. One container for all DatasetTypes
#### Advantages
- Can now do a single query to get data coverage across all datasets for a given URN, e.g. 
`SELECT DISTINCT c.DataSetType, c.Year, c.Version FROM c WHERE c.Scope = 'School' AND c.URN = 123456`
- For a low-throughput scenario running costs will be even lower than (2) if we have a single container

#### Disadvantages
- All dataset data now lives in the same container, so similar to (2) except scope of something going wrong is wider

#### Neutral
- Similar to (2) each document must have `DatasetType`, `Year`, `Version`, `Scope` properties defined with predefined values
---
## Partitions
Each container in CosmosDB is split up into partitions. **Physical partitions** are physical locations where the data is stored, containing up to 50GB each. Physical partitions are controlled by CosmosDB and we don't have access to them. However we can define **logical partitions** which can be  are distinct sets of data within the container up to 20GB each, and physical partitions can contain one or more logical partitions. Logical partitions are identified by a **partition key**, which is just a property path on the documents within the container. Every document should have this property defined, and it can't be updated once a document is created, or changed at the container level.

Partition keys should:
1. have a high cardinality (a wide range of possible values)
1. have an even spread of values across this range
1. naturally separate the data into sets that logically live together

The reason for (1) and (2) is to avoid **hot partitions** which are partitions that are accessed significantly more than others. Throughput on a container is divided equally across physical partitions, so if there is one partition that is accessed more frequently, this might lead to it hitting the rate limit or causing the whole container to be autoscaled upwards, even if the other partitions are well below the limit.

The reason for (3) is that queries that are scoped within a logical partition are much more efficient than queries that span logical partitions. This is because the partition key is used to identify which physical partition the data is sitting on, and if this can't be inferred from the query then a **fanout** query has to be made which executes the query against all the physical partitions and joins the results together. This will [likely be slower and will almost certainly use more RU's]( https://learn.microsoft.com/en-us/azure/cosmos-db/nosql/query-metrics#partitioning-and-partition-keys)

So with this in mind let's look at a few possible partition keys - assuming we're going with the one container for all DatasetTypes approach.

### Possibility 1: `/id`?
One obvious candidate for the partition key is the document id property. This is a GUID that is automatically created by CosmosDB if it's not already defined on the document being inserted. It has a wide range of values, and has an even spread across that range so would create very evenly balanced partitions. This would effectively create a partition for each document. This is great for load balancing but not so great for queries as every query would be a fanout query touching every physical partition.

### Possibility 2: `/URN`?
When thinking about partition keys, it's useful to examine the most common queries as this will allow us to identify logical groups of data. Looking at the queries in the *Querying patterns* section above, by far the most frequent queries will be:

1. Get data coverage (whether the latest data exists for each `DatasetType`) for a given URN
1. Get data for a specific `DatasetType` for a given URN

It looks like URN is a logical choice for partition key based on this. However, not all documents in the container have a URN. The URN field exists on pupil-level and school-level data but not LA- or national-level. We're on the right track but not quite there yet.

### Possibility 3: Some kind of composite field?
We could create a new field on each document (maybe called `partitionKey`?) that had an appropriate value for each of the `Pupil`, `School`, `LA` and `National` scoped documents - so for `Pupil` and `School` it would be the URN, for `LA` it would be the LA Code, and for `National` it could be `National` or `N`.

This gives us a big benefit - both `School` and `Pupil` scoped data are now in the same partition, so we can execute a single query to bring back both types of data: `SELECT c.* FROM c WHERE c.partitionKey = 123456` will bring back both school and pupil documents. Even better, we know there will only be one `School` document in this set, so we can order the query so we always get that one first and handle it separately in code: `SELECT c.* FROM c WHERE c.partitionKey = `123456` ORDER BY c.Scope DESC` (`'School'` > `'Pupil'`)

This is looking good but we still need `LA` and `National` data. We can include those in the query but like this: `SELECT c.* FROM c WHERE c.partitionKey IN ('123456', '301', 'N') ORDER BY c.Scope DESC` - we'd have to choose appropriate values for `Scope` here, so that `National`, `LA` and `School` documents come first in the results and we can siphon them off in code. 

However, the above query will be a cross-partition (fanout) query and we'd really like to avoid those if possible.

### Possibility 4: Back to `/URN`?
What if we were to create separate copies of the `LA` and `National` documents for each URN? Then we can use `URN` as our partition key after all. Now our query `SELECT c.* FROM c WHERE c.partitionKey = 123456` will be a single-partition query that brings back all the documents from all 4 scopes at once.

The cost for this is extra storage for the duplicate documents, and extra work for the data import pipeline to create the duplicates. But the advantage is our most frequently used queries are now single-partition queries with a much reduced cost in RUs - and throughput is vastly more expensive than storage. *(insert numbers here)*

In addition, for other systems this would create extra work of keeping all the duplicates in sync when they are updated (e.g. using the Change Feed to detect changes and update the relevant documents), in ASP we never update the data, so we only have to create the duplicates once on import and never need to worry about them again!

### Possibility 5: Back to `/partitionKey`?
OK so have we covered all cases? Not quite - querying by URN isn't the only use case. Looking back at our *Querying patterns* we also need to get `LA`-level data for the LA landing page and LA downloads. How should we do that? Well we could create a query to get the LA and National documents: `SELECT c.* FROM c WHERE (c.Scope = 'LA' AND c.Code = '301') OR c.Scope = 'N'` - this will work but it will be a bit messy, as it will bring back all the duplicate documents we created in the last section - plus it will be a cross-partition query. Can we do better?

Yes - create more duplicates! Instead of using `URN` as the partition key, bring back the custom `partitionKey` field. Then create duplicates of the `LA` and `National` documents for each school and LA and have the `partitionKey` be set to either the URN or the LA code. 

Now we can have 
- a `School`-level query: `SELECT * FROM c WHERE c.partitionKey = '123456'`
- and an `LA`-level query: `SELECT * FROM c WHERE c.partitionKey = '301'`

Both of these will be single-partition queries so will be very efficient, and we only use up a tiny bit of extra storage for all the LA/National-level duplicates. 

My recommendation would be to use this partition strategy, combined with having all the dataset data in a single container. Thoughts? Opinions? Rage?

### Further thoughts on the partition key field

1. It might be better to rename the `partitionKey` field to a more meaningful name that reflects the data it contains, such as `urnOrLaCode` - I'm not sure whether this is better or whether keeping it as `partitionKey` makes its purpose more obvious when inspecting the data.

2. The URN or LA Code fields tend to be integers in the original data, however the recommendation is for partition keys to be strings. I suggest having it as a string although this will require converting the value to string when populating the partition key field as part of the data import process

3. It might be useful to further distinguish school and LA values by adding a prefix to the partition key (e.g. `"school-"` or `"la-"`, so we'd get `"school-123456"` or `"la-301"`) - this is not necessary and might create more work for little gain, especially as URNs and LA codes do not share any values (URNs are 6-digit and LA codes are 3-digit)

4. A more interesting problem to think about is that often the URN field is named differently for different types of record. For example the container `exclusions_school_2021` contains a number of documents for each school, with different field prefixes for each document, so the URN field could be one of the following:
   * `FIXED_NUM_SCH_URN` 
   * `FIXED_PERCENT_SCH_URN`
   * `HEADCOUNTS_SCH_URN`
   * `ONE_OR_MORE_FIXED_NUM_SCH_URN`
   * `ONE_OR_MORE_FIXED_PERCENT_SCH_URN`
   * `PERM_NUM_SCH_URN`
   * `PERM_PERCENT_SCH_URN`
   * `TWO_OR_MORE_FIXED_NUM_SCH_URN`
   * `TWO_OR_MORE_FIXED_PERCENT_SCH_URN`
  
   This means that currently in ASP to get all the data for a school we have to hard-code these field names into a query, for example: 

    `SELECT * FROM c WHERE c.FIXED_NUM_SCH_URN = 102850 OR c.FIXED_PERCENT_SCH_URN = 102850 OR c.HEADCOUNTS_SCH_URN = 102850 OR c.ONE_OR_MORE_FIXED_NUM_SCH_URN = 102850 OR c.ONE_OR_MORE_FIXED_PERCENT_SCH_URN = 102850 OR c.PERM_NUM_SCH_URN = 102850 OR c.PERM_PERCENT_SCH_URN = 102850 OR c.TWO_OR_MORE_FIXED_NUM_SCH_URN = 102850 OR c.TWO_OR_MORE_FIXED_PERCENT_SCH_URN = 102850`

    Populating a single partition key field from all these different fields will make querying the data much easier to reason about, more consistent, and will result in the queries themselves being much more efficient. This will require adding extra logic into the data processing pipeline to know that these specific fields need to be looked for - maybe this could be done by finding any field containing the substring `"_URN"`?

    Another option would be not creating the prefixes in the first place. I think these were created in order to identify the source data file of each record, but this could be better done by a discriminator field on the data to indicate the same thing? More research needed into the purpose and use of these field prefixes.

### Further reading
If you want to explore more about partition keys and partitioning strategy check out these resources:

- [Partitioning and horizontal scaling in Azure Cosmos DB (learn.microsoft.com)](https://learn.microsoft.com/en-us/azure/cosmos-db/partitioning-overview)
- [Partition Key Best Practices in Cosmos DB (mssqltips.com)](https://www.mssqltips.com/sqlservertip/7406/cosmos-db-partition-key-best-practices/)
- [Partition Strategy | Azure Cosmos DB Essentials Season 2 (youtube)](https://youtu.be/QLgK8yhKd5U) (7:48) - this one is really good
- [Best practices for Azure Cosmos DB: Data modeling Partitioning and RUs (youtube)](https://www.youtube.com/watch?v=bQBeTeYUrR8) (1:01:43) - also really good, goes into depth about RUs, partition keys and structuring data

---
## Data structure
Now we've identified a possible container and partition strategy for the data (hoping everyone agrees with my reasoning process above!) we can finally think about the structure of the data. Up to this point I've been assuming that the data will be stored as a document representing each row in the source tables in SQL Server. But that is only one possibility. At the other extreme, we could have a single document for each partition, so for each school or LA we have a single parent document containing child documents for each of the `National`, `LA`, `School` and `Pupil` data. Or we could have somewhere in between - maybe `National` and `LA` data should have their own documents, but `School` and `Pupil` data should be bundled together in one document?

Which is best? It's a tradeoff between the following:
- Document size
- Data import pipeline effort
- Ease of understanding of data structure for whoever is compiling the reports
- Best practices for updates

### Document size
Cosmos DB has a maximum document size of 2MB. If we were to bundle all the `Pupil`, `School`, `LA`, and `National` level data into one document for each school, this is going to be the document size we need to think about (`LA` documents will be much smaller than this as it won't include school or pupil level data) - so how much does all this add up to for each data set, and what would be the largest document size?

After querying the total size of all the pupil records for each school from all the datasets we can see that the pupil records for the largest school total 2.69 MB. This means that bundling pupil data for each school into the same document isn't going to work for all datasets. So at least for some datasets, pupil and school records should be kept as separate documents. However it would be best to be consistent across datasets and treat them all in the same way.

Another thing that suggests we're on the right track with the pupil data is that the QLA Year 7 custom view needs to find pupils by UPN across all schools. This is much easier to do if each pupil has their own document and can be queried independently, rather than bundled into the document for their school.

What about the `School`, `LA` and `National` level data? With all of these we can also have multiple documents, for instance in Key Stage 2 the statistics are broken down by SUBJECTID and PUPILTYPECODE and we have a separate document for each of these combinations. However the amount of data for all of these combined falls well below the 2 MB limit - for Key Stage 2 we have up to 27 documents for each school, 4 documents for each LA and 154 documents total for National - so I think it makes sense to bundle these together into the `School` level document, so that each school document would look like this:

```
{
  "id": "797515e1-481d-458a-a1f0-d86bd9e35ff3",
  "URN": 123456,
  "URNOrLACode": "123456", // partition key
  
  "schoolLevelData": [
    {
      "URN": 123456,
      "SUBJECTID": 9982,
      "PUPILTYPECODE": "ALL",
      ... // other School fields
    },
    {
      "URN": 123456,
      "SUBJECTID": 9982,
      "PUPILTYPECODE": "CLA or FSM",
      ... // other School fields
    },
    {
      "URN": 123456,
      "SUBJECTID": 9985,
      "PUPILTYPECODE": "DisadvHigh",
      ... // other School fields
    },
    ... // other School records
  ],

  "laLevelData": [
    {
      "LA": 894,
      "SUBJECTID": 9982,
      "PUPILTYPECODE": "ALL",
      ... // other LA fields
    },
    {
      "LA": 894,
      "SUBJECTID": 9984,
      "PUPILTYPECODE": "ALL",
      ... // other LA fields
    },
    {
      "LA": 894,
      "SUBJECTID": 9985,
      "PUPILTYPECODE": "ALL",
      ... // other LA fields
    },
    ... // other LA records
  ],

  "nationalLevelData": [
    {
      "SUBJECTID": 9982,
      "PUPILTYPECODE": "ALL",
      ... // other National fields
    },
    {
      "SUBJECTID": 9982,
      "PUPILTYPECODE": "M",
      ... // other National fields
    },
    {
      "SUBJECTID": 9982,
      "PUPILTYPECODE": "F",
      ... // other National fields
    }
    ... // other National records
  ]
}
```

What we're doing here is effectively denormalizing the `School`, `LA` and `National` data for each school document. We can do a similar thing with the LA documents and denormalize `National` data into each LA document, so we'd create a single document for each LA, and combine all the `LA` and `National` documents into it, something like this:

```
{
  "id": "a52c4391-f2b9-4d10-844e-b22c57dbf141",
  "LA": 894,
  "URNOrLACode": "894", // partition key

  "laLevelData": [
    {
      "LA": 894,
      "SUBJECTID": 9982,
      "PUPILTYPECODE": "ALL",
      ... // other LA fields
    },
    {
      "LA": 894,
      "SUBJECTID": 9984,
      "PUPILTYPECODE": "ALL",
      ... // other LA fields
    },
    {
      "LA": 894,
      "SUBJECTID": 9985,
      "PUPILTYPECODE": "ALL",
      ... // other LA fields
    },
    ... // other LA records
  ],

  "nationalLevelData": [
    {
      "SUBJECTID": 9982,
      "PUPILTYPECODE": "ALL",
      ... // other National fields
    },
    {
      "SUBJECTID": 9982,
      "PUPILTYPECODE": "M",
      ... // other National fields
    },
    {
      "SUBJECTID": 9982,
      "PUPILTYPECODE": "F",
      ... // other National fields
    }
    ... // other National records
  ]
}
```

### Data import pipeline
How much work is it to merge documents together and how much processing time does it add to the import process? How important this is depends on how much data resource we have and how extensive the changes are. I'll discuss in its own section later.

### Ease of understanding of data structure
Is the person creating the page templates more familiar with the original SQL Server data or is it more intuitive to think about parent and child JSON documents? 

This aspect is not important as we're already using Cosmos DB so whoever is writing the reports will need to know the difference between how Cosmos represents data vs. the original data structure in SQL Server. In addition, within the data processor pipeline of each report definition in ASP v2 we can structure the data how we like after the fact, so the data as surfaced to the report won't depend on the structure in Cosmos.

### Best practices for updates
As mentioned in the "Best practices" video listed above, the way data is updated is a factor in how data should be structured - it's good practice to denormalize data as children of a parent object, unless the number of child items is potentially unlimited (e.g. customer orders) in which case they should be separate documents, as we don't want to be updating a document that is constantly increasing in size. 

We don't have to think about this aspect however because ASP data is never updated.

---
## Casing
Best practice for casing of field names within Cosmos documents is `camelCase`. This is at odds with the dataset data in its original form on SQL Server which uses `UPPERCASE`. In addition the proof-of-concept work for the content templates uses `TitleCase` (as these fields are driving C# properties or used in `.cshtml` pages)

There are two options:
1. Keep the fields as `UPPERCASE` for consistency with the original SQL Server data
2. Convert the fields to `camelCase` to conform to Cosmos best practice

With option (2) there are some potential problems. The word boundaries within the field names are are inconsistent and often not obvious to an automated process, e.g. `SPELLINGMARK_AVG` and `SPELLINGMARKGROUP1` - how would we convert these to camelCase? Obviously the ideal form for these fields would be `spellingMarkAvg` and `spellingMarkGroup1` but other than manually hardcoding these transformations for every possible field name, or having some machine intelligence detect word boundaries, there is no way this could be done automatically.

So it seems we would have to leave these field names as `UPPERCASE`. What about fields like `id` and the partition key field? The `id` field cannot be changed, it is required to be `camelCase` (i.e. essentially lowercase) in order to be detected as an ID field by Cosmos. I suggest that since the partition key field is generated for ASP and is not part of the original data, it should be in `camelCase` (so `partitionKey` or `urnOrLACode`) to distinguish it from the original data fields and to align with Cosmos-generated fields like `_ts`.

I would also suggest that other fields like `schoolLevelData`, `laLevelData` and `nationalLevelData` that we discussed creating in the *Document Size* section above should also be `camelCase` for the same reason. So we'd end up with a document like this:

```
{
  "id": "797515e1-481d-458a-a1f0-d86bd9e35ff3",
  "urnOrLACode": "123456", // partition key
  "URN": 123456,
  
  "schoolLevelData": [
    {
      "URN": 123456,
      "SUBJECTID": 9982,
      "PUPILTYPECODE": "ALL",
      ... // other School fields
    },
    {
      "URN": 123456,
      "SUBJECTID": 9982,
      "PUPILTYPECODE": "CLA or FSM",
      ... // other School fields
    },
    {
      "URN": 123456,
      "SUBJECTID": 9985,
      "PUPILTYPECODE": "DisadvHigh",
      ... // other School fields
    },
    ... // other School records
  ],

  "laLevelData": [
    {
      "LA": 894,
      "SUBJECTID": 9982,
      "PUPILTYPECODE": "ALL",
      ... // other LA fields
    },
    {
      "LA": 894,
      "SUBJECTID": 9984,
      "PUPILTYPECODE": "ALL",
      ... // other LA fields
    },
    {
      "LA": 894,
      "SUBJECTID": 9985,
      "PUPILTYPECODE": "ALL",
      ... // other LA fields
    },
    ... // other LA records
  ],

  "nationalLevelData": [
    {
      "SUBJECTID": 9982,
      "PUPILTYPECODE": "ALL",
      ... // other National fields
    },
    {
      "SUBJECTID": 9982,
      "PUPILTYPECODE": "M",
      ... // other National fields
    },
    {
      "SUBJECTID": 9982,
      "PUPILTYPECODE": "F",
      ... // other National fields
    }
    ... // other National records
  ]
}
```

---
## Changes to data import pipeline for v2
So assuming everyone is on board with the suggestions above, the changes to the data import pipeline for v2 would be as follows:

1. Import all datasets into a single container. This would involve setting the following fields on all documents created as part of the import process:
   * `dataSetType`: set to `"KeyStage1"`, `"QLA"` etc. (these would need to be pre-defined values for all datasets)
   * `year`: set to `2022`, `2023` etc.
   * `version`: set to `"Provisional"`, `"Revised"`, `"Final"` / `"P"`, `"R"`, `"F"` - or values that are orderable in order to find the latest version e.g. `1` for provisional, `2` for revised, `3` for final (the version names will give the wrong order when sorted alphabetically)
   * `scope`: set to `School`, `LA` or `Pupil` - this is to distinguish the type of document when querying (`National` values are bundled into the top-level school or LA documents)
   * `urnOrLACode`: set to the value of the `URN` field (for school and pupil documents) or `LA` field (for LA documents) converted to `string`
     * This also requires the import process to know which type of document it is importing and where to find the correct value for the partition key from each type of document
     * This should also take into account the different names the URN field could have as discussed in the *Partitions* section above

2. Add copies of all the `School`, `National` and `LA` level data into a single document for each school, and copies of all the `LA` and `National` level data into a single document for each LA, in the form discussed above.

---
## Data coverage
To find the latest dataset for a school we can now run a single query:

```
SELECT 
  c.dataSetType, 
  MAX(CONCAT(ToString(c.year), ".", ToString(c.version))) AS yearVersion 
FROM c 
WHERE c.scope = 'School' AND c.partitionKey = '123456' 
GROUP BY c.dataSetType
```

This produces results like this:

```
[
    {
        "dataSetType": "KeyStage2",
        "yearVersion": "2023.1"
    },
    {
        "dataSetType": "KeyStage1",
        "yearVersion": "2022.3"
    },
    {
        "dataSetType": "Phonics",
        "yearVersion": "2022.2"
    }
]
```


What's going on here? 

We've chosen the `version` field to be an `int` where 1 = `Provisional`, 2 = `Revised` and 3 = `Final`. This allows us to sort the versions numerically and find the latest one. We're joining that with the `year` field to allow us to sort by the year/version combination, and grouping by `dataSetType` to get the latest of these for each data set.

We could have done something like `MAX(c.year + (c.version / 10))` which would give us the same result, but we would be limited to having version numbers of less than 10, and string concatenation isn't that much more expensive.

Another alternative would be to create a field to store this year/version value and just populate it when creating the data, rather than computing it on the fly, but the cost to do the concatenation is tiny.

# Blob Storage
Given that most of the data we store in Cosmos never changes, we could save costs dramatically by storing the data in blob storage instead. 

This would be implemented with something like the following structure:
```
dataset-data/
  School/
    100001/
      2022/
        phonics-provisional.json
        phonics-revised.json
        phonics-final.json
        phonics-pupil-provisional.json
        phonics-pupil-revised.json
        phonics-pupil-final.json
        ks2-provisional.json
        ks2-pupil-provisional.json
        ...
      2023/
      2024/
    100002/
    100003/
  LA/
    101/
      2022/
        phonics-provisional.json
        phonics-revised.json
        phonics-final.json
        phonics-pupil-provisional.json
        phonics-pupil-revised.json
        phonics-pupil-final.json
        ks2-provisional.json
        ks2-pupil-provisional.json
        ...
      2023/
      2024/
    102/
    103/
```

The structure can be explained as follows:
* A separate folder for each School/LA
* A separate folder within each of the above folders for each year
* Within each of the above folders, each dataset will be split into two documents: one containing the pupil data for that dataset (if any), and one for the school-, LA-, and national-level data for that dataset - in the structure outlined above
* In addition, a separate document will be generated for each of the above documents for the provisional, revised and final releases.

The reason the dataset is split into two documents is that the pupil data is often very large and is not always needed on any given page. So rather than have the pupil data contained within the same document as the other data where it will have to be retrieved even when it isn't needed, incurring extra bandwidth, it should be stored in a separate document where if it is needed for a specific report page an extra network call can be made to request it in that case.