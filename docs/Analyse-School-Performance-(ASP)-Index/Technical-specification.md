[[_TOC_]]

# High level architecture
The following diagram shows a high level overview of the ASP 2.0 architecture. Dashed boxes/arrows represent components that haven't been implemented yet.

![image.png](/docs/.attachments/image-38a2878a-f2c1-44bf-ab0d-2d0d8712f1de.png)

## ASP Web application service
This is the application service the end user interacts with. Users are authenticated with DfE Sign In (DSI).

## API function app
This is the API that drives the web application, and contains all the business logic.

## Cosmos DB
This contains data that needs to be easily queryable, such as establishments, LA and MAT reference data, and things like the dataset definitions (to be implemented - listing what datasets are available/released for each school/LA)

## Blob storage
This contains the rest of the data that doesn't need querying functionality (e.g. data download files, and the dataset data itself (to be implemented - see [Dataset data storage](/Analyse-School-Performance-\(ASP\)-Index/Technical-specification/Dataset-data-storage)).

## Data Bricks
This is the data pipeline that populates the ASP Cosmos DB and blob storage with production data (or synthetic data for the lower environments). The reference data, e.g. establishments, LAs and MATs are pulled from the GIAS API.

## Table storage
This is mainly used for logging, such as the error log and the audit log (to be implemented)

## Another function app?
There will need to be some feature that 
1. scrapes summary reports as static HTML files from the ASP BAU system and stores them in Blob Storage
2. generates static HTML files from ASP 2.0 datasets as they are released (?) and stores them in Blob Storage - unclear if this will be needed as summary reports may just be a legacy feature of BAU and only need a one-off generation effort

It is also unclear as to whether this scraping and generation functionality will be executed by a function app or data pipeline, or even a simple console app run locally.
