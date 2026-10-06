## ASP Worker
- Current ASP has these functions, which do we still need?
  - [ ] ComputeAspDataCoverage_queue
  - [ ] ComputeAspDataCoverage_timer
  - [ ] ConfigureAzureSearch
  - [ ] ConvertGiasXml2Json
  - [ ] EstablishmentImport
  - [ ] NotifySearchIndexerFailure
  - [ ] ProcessKtsFileUpload
  - [ ] QueueKtsFileUpload
  - [ ] RebuildAzureSearch
  - [ ] RebuildMatList
  - [ ] ProcessGiasExtract
- New functions for ASP 2.0
  - [ ] Summary report generation
    - [ ] Research: Manual or automatic trigger?
    - [ ] POC
## ASP API

- [ ] Will API be a separate function app to the above functions?
- [ ] Research: Automated testing
 
---
# Cognitive search
* Current ASP uses Cognitive Search to index Establishments container in Cosmos DB
  - [ ] Do we still need it?
  - [ ] How would it work if Establishments move to Blob storage?

---
# Redis cache
* Current ASP caches at the query level
- [ ] Will we need Redis in future if queries are efficient (Redis is expensive)
- [ ] Page level caching instead of query level?
  - Certain aspects of ASP are dynamic (e.g. QLA) so can't be cached, but everything else is rendered dynamically from static data, predictably from URL + QueryString so could be cached?
