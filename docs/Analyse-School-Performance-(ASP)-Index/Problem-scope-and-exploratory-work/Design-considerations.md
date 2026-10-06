# Technical roadmap

## Problems
The ASP 2.0 roadmap can be broadly split into 2 phases:
1. Technical rewrite with minimal functional changes (MVP) 
2. New functionality (ASP 2.0 proper) 

The goal of the MVP is to fix a lot of the technical issues with ASP in order to create a sound basis for BAU and future functionality changes. The technical changes cover three broad areas which are somewhat interconnected:
* Cost
* Efficiency
* Developer-intensive workflow 

### Cost  
* ASP v1 uses Cosmos DB
* Each time a Dataset (Key stage 2, QLA etc) is released, several new Cosmos containers are created
* Since we have data going back to 2014, there are now over 370 Cosmos containers   duplicated across 3 environments
* Very expensive to run - each container has a fixed running cost, even if it is not accessed
* As more containers are added, cost is growing over time
* Data accessed in ASP is heavily weighted towards the latest data - ASP only shows you the latest data available for each Dataset
* Historical data is kept around to render the summary reports - year-based reports containing a selection of reports for each year
* Summary reports are dynamically generated so need the historic data and code kept around in order to render them on request
* No retention   policy on how many years summary reports should go back   for, so all reports are kept

### Efficiency
* ASP is very slow
* When any page   is viewed it needs to work out what the latest data is available for each dataset (Data Coverage) for the requested establishment
* Because the data is split up across so many containers it needs to do a separate query for each container
* Each report dynamically queries Cosmos whenever it needs data - results in duplication of queries, fetching more data than needed
* All queries are cached using Redis but the first time a new dataset is viewed or if the cache is cleared the initial load could be well over 5 seconds
* No partition keys set on containers, so queries are inefficient
* Everything in ASP is dynamically generated from data that doesn't change. Current year reports need to be dynamic as they use filters etc but summary reports don't

### Developer-intensive workflow
* All reports are generated in code, so every time a Dataset is released, or a change needs to be made to the page content, a developer has to make the change
* Every year each time a new Dataset is released, the code needs be duplicated from the previous year
* This is because of the need to generate historical reports on-demand, so code for previous years must be kept unchanged
* Creates a huge maintenance burden as codebase is ever increasing
* Every new Dataset release requires multiple updates to different places - time-consuming and error-prone
* Risk of bugs as could accidentally introduce changes that affect historic reports
* Few automated tests (UI-based automation tests are being built) so any change requires a lot of manual testing

## Proposed solutions
* Use a single Cosmos container for all Dataset data (or even move into storage container)
* Load all data needed for a report up-front so can be batched into as few queries as possible
* If using Cosmos, have an effective partition strategy and use de-normalisation so queries will be as efficient as possible
* Replace developer-heavy workflow with a CMS-type system so that
  1. Data releases/content changes do not need code to be deployed
  2. Data releases/content changes can be done by non-developers
  3. Report pages can be built out of a set of pre-defined components (charts, tables etc) that can be dropped onto the page and wired up to the correct data
  4. Developer input can be limited to:
     * creating new components/data flows
     * making changes to existing components/data flows
     * maintaining the overall functionality of the system
* Instead of historic summary reports being dynamic, convert all reports from previous years into a static form, and delete the underlying report templates and data.
  1. Background process that creates the cache when the current year is complete (triggered manually/automatically)
  2. Would need to create a report for each establishment per year
  3. Possible formats:
     * PDFs - accessibility issues
     * HTML - generate images of graphs and store alongside

## Technical tasks

| Task | Estimate |
|--|--|
| Provision Dev environment|  |
|Provision UAT environment |  |
| Provision Pre-Prod environment |  |
| Provision Prod environment |  |
|Set up build pipelines	 |  |
| Set up data pipelines	 |  |
|  DSI integration (tie into pages/reports?)|  |
| Audit log	|  |
| Helptext system	 |  |
| Dataset definition	 |  |
| Page content template system	 |  |
| UI Ccmponent system |  |
|Unit test framework	|  |
|  Automation test framework	|  |
|  API |  |
| Security configuration | |

	



## NFRs  
* Capacity
* Concurrency
* Data Privacy
* Compliance and Legal
* Integration
* Maintainability
* Performance
* Resilience
* Re-use
* Scalability
* Scheduling
* Security
* Segregation
* Service Continuity
* Sustainability
* Service Operations
* Usability
* Data Retention – Archival and Purging  

[Azure Well-Architected | Microsoft Azure](https://azure.microsoft.com/en-us/solutions/cloud-enablement/well-architected/#Reliability)

## Ways of working
* Sprints?  How do we determine what we do in each sprint?  Kanban ?
* Determine development E2E process.  Design – Approve – Develop – Test – Deploy - Review
* How do we keep in sync with as-is dev team?  If they develop new things?  Maybe we look at their sprint planning to pick up anything that is of interest to us and confirm no impact to our deliverables.
* Approvals for design?  Dev and Architect
* Who does HLD and LLD?  What is included in each document?  Where do store the files?  Wiki, sharepoint, etc 
* Calendar of major milestones and holiday  planning?  When cannot not do releases, etc

## Actions

- [ ] 1. Setup meeting to review ASP 2.0 Plan in Lucid.  Objective is to align agreed high level approach.  Look at owners for each track. Ask GD when do we need to have the plan ready by, as our dependency is having a high level approach agreed , what can be down now with no further input, what depends on what, like env build need NFR to pick correct components or pattern for development.  After plan is published, it will be updated as we move along (flexibility on plan??).
- [ ] 2. Walkthrough of Data changes (specifications received from Policy) year on year.  
- [ ] 3. Document the primary deliverables for MVP (Cost, Efficiency, Developer-intensive workflow) – Objective is to baseline MVP requirements to be used in the high level solution approach. 
- [ ] 4. Confirm environment for use on MVP is T1 or CIP – SaS and StS to talk to CarlMc 
- [ ] 5. Setup meeting to agree on what goes into each sprint of 2.0 MVP . – ALL
- [ ] 6. Document technologies in use and planned to be used – confirm it’s aligned to accepted DfE technology stack (use continue, sunrise tools only, migrate sunset)
- [ ] 7. Determine what’s enough design at this moment for MVP, to begin agile development – Envrionments, infrastructure, etc
