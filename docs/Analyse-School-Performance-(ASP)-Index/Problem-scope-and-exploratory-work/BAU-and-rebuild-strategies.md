How will we carry out BAU alongside the new build? It seems likely that we won't have a complete rewrite ready by the time we need to do a new data release at the end of July so we will need a strategy for this. Here are some options to consider:

# Option 1: Complete rewrite
In this approach we continue working on both systems, applying data releases to the old ASP and making sure new ASP gets the same updates. Once new ASP is complete (including updates since the start of build) we switch over.

## Pros:
* Free to build out the navigation and the look and feel of components in the way that makes the most sense and is GDS compliant

## Cons:

* Users only have access to new ASP (with better navigation, nicer look and feel, GDS compliance etc) once it is complete
* Double the amount of development work for each release (applying releases to both systems)
* Double the data work - preparing releases for both the new ASP and the old ASP (data releases on old ASP are time-consuming and tedious which is one of the reasons for building a new system)
* Extra effort and communication to co-ordinate the changes across both systems
* Double the testing effort, greater risk of bugs
* Major development effort spent on the old system that will be thrown away when the new system is turned on
* All legacy code has to be rewritten in the new system, or features dropped

# Option 2: Side-by-side development
In this approach we start building new ASP and as soon as it is has a minimal subset of functionality that is ready for production, we release it. We only release data on new ASP and keep old ASP around for users to access data for historic years.

## Pros:
* Free to build out the navigation and the look and feel of components in the way that makes the most sense and is GDS compliant
* Users don't have to wait until new ASP is feature-complete before getting benefits of new system (better navigation, nicer look and feel, GDS compliance etc)
* No duplication of effort, data releases can be on new ASP only which will be easier for development and for Data Ops

## Cons:
* Users have to navigate two systems to get the data they want (could help mitigate this by providing links from old ASP to reports on new ASP)
* Each organisation will have to manage DSI logins for both applications (DSI can initially duplicate users from old ASP)

# Option 3: Strangler Fig
> Incrementally migrate a legacy system by gradually replacing specific pieces of functionality with new applications and services. As features from the legacy system are replaced, the new system eventually replaces all of the old system's features, strangling the old system and allowing you to decommission it.

Source (recommended reading): https://learn.microsoft.com/en-us/azure/architecture/patterns/strangler-fig

Here's how this approach would work:
* Keep old ASP running but any new data releases are done in new ASP. 
* Implement a "Strangler Facade" that sits on front of both applications and routes requests either to the old system or the new system. 
* Look and feel of new ASP kept closely similar to old ASP so that users don't necessarily know which system they have been routed to.
* Focus development work on new functionality and replacing only those aspects of the old system that are necessary for the new functioning of the new system. 
* Once core functionality is complete, legacy functionality on a piece by piece basis can either be converted, turned off, or replaced with static files, until old ASP can be deleted.

## Pros:
* Users don't have to navigate two systems to get the data they want
* Users don't have to wait until new ASP is complete before getting benefits of new system (better navigation, nicer look and feel, GDS compliance etc)
* No duplication of effort, data releases can be on new ASP only which will be easier for development and for Data Ops

## Cons:
* Because both systems are pretending to be one system, look and feel and navigation of new ASP would be constrained not to be too different from old ASP (meaning some of the benefits of a rebuild would be lessened e.g. better look and feel/navigation)
* Extra complexity of Strangler Facade to route between systems (see below)
* Extra complexity of keeping navigation consistent across both systems, i.e. old ASP needing to render a navigation menu that includes reports implemented in new ASP and vice versa (see below)
* Extra complexity of implementing Data Coverage across both systems so that old ASP needs to know what schools have data for new ASP reports and vice versa (see below)
* Extra complexity of creating a single sign-on across both systems (will DSI even allow this?)

## Technical considerations
There are a number of technical considerations to take into account with this approach. It is more complex than a straight rebuild (for example the Strangler Facade) and it puts constraints on the new build that would not exist with a straight rebuild, such as keeping the look and feel and navigation structure the same to ensure consistency across the two systems.

Areas of uncertainty where further research needed **[Research]**, some of these could impact the feasibility of this approach.

### Look and feel
The look and feel of new ASP could be different to old ASP, as long as the amount of difference was not too jarring for users. New components could be developed to be GDS compliant, although would it be an issue if the system contained both GDS-compliant and non-GDS-compliant components?

### Hosting
Are old ASP and new ASP going to be hosted in the same tenant? Decision whether old ASP is to be migrated will impact which approach we go with - infra setup/firewall to allow communication between tenancies **[Research]**

### Navigation
Both systems will need to render the same navigation menu including links to pages in both old and new ASP (Is there a way we could delegate this to the Strangler Facade somehow? **[Research]**)

* New ASP will need to have a model of pages in the navigation that don't correspond to pages in new ASP but are handled by old ASP (this could be as simple as having a navigation structure that allows for arbitrary links that aren't tied to controller actions - the Strangler Facade will route those links to old ASP)
* Old ASP will need to accommodate pages in its navigation model that are implemented in new ASP - this is more difficult, the old navigation model (`SchoolPagesModel`) is quite complex and hardcodes a lot of the structure **[Research]**

### All reports page
The All Reports page is a list of all reports available for the current year. Currently defined in the old navigation structure i.e. `SchoolPagesModel` and could point to reports from any year, if that's the latest year for that dataset. The view also hardcodes these reports along with access checks. If `SchoolPagesModel` is adapted to include arbitrary links this should be fairly easy to do the same. 

* This will need to be reimplemented in new ASP and 
* While still implemented in old ASP will need to amend `SchoolPagesModel` and `AllReports` view whenever a report is added in either system
* Once reimplemented in new ASP will need to similarly update whenever a report is added in either system

### Summary reports
Shows a one-page view of selected reports for a given year (without filters, so just showing a single dimension for each report)

* Reports are embedded into the page - if we are allowing some reports to be implemented in new ASP and some in old ASP will need to figure out a way of embedding reports from one system into the other
* Easiest thing would be to replace all Summary Reports with PDF versions - this would remove the need to replicate the old reports on new ASP and would remove any need to embed reports from different systems into the page.
* If this is not possible the next easiest thing would be to not allow reports within a given year to be implemented on different systems (e.g. all 2023 reports will be on new ASP)
* If this is not possible then next easiest would probably be to implement summary report in new ASP and build a way into new ASP of including partial reports from old ASP? **[Research]**

### Cross-year reports
ASP contains reports that are across multiple years. In old ASP each Dataset has all the data it needs in its own container. So if a report needs data over multiple years this data will be included in the container for that Dataset/year. 

In new ASP we would probably want a container to contain multiple years' worth of data, so we will need to think about how to distinguish data over multiple years needed for a report from data for a specific dataset/year.

### Data coverage
Data coverage is code that determines if data is available for an establishment for a given dataset/year combination. 

* Combination of manual and dynamic checks (the dataset/year to check is hardcoded but the check itself then checks the corresponding containers for each dataset/year for the URN in question)
* Used to determine visibility of reports on the page (e.g. summary report)
* Used to determine whether to show/hide pages within the navigation (if there is no data for the current school for that dataset/year)
* Data coverage checks call out to custom services for each dataset/year - if a report is implemented in new ASP these services would need to be implemented for that report (at least just the `HasData()` checks) **[Research]**
* One way of circumventing this could be to create a very simple data coverage table within old ASP to be looked up for new ASP datasets
* Another possibility - network call out to new ASP to get data coverage

### Downloads
There are two types of Downloads in ASP: Key To Success pre-prepared files and Analyse School Performance dynamically-generated files

* Data released in either new or old ASP needs to be available in downloads regardless of whether this is implemented in old or new ASP
* Downloads are defined in `ASP.Core.Services.Downloads.Metadata.Configuration` - defining the container and key fields to query on (exports all fields in the container)
* **Either:** keep downloads in old ASP - reports created in new ASP need to be added to this configuration (assuming new data structure makes sense to download directly), **or:** reimplement downloads in new ASP - requires creating new download definitions in new ASP for all legacy download files 
* Downloads will need to be reimplemented in new ASP anyway so maybe makes most sense to reimplement from the start **[Research]**

### Authentication and RBAC
* Users will need to be automatically logged into both systems - Strangler Facade will need to pass through DSI authentication headers/cookies
* Old ASP uses OWIN external authentication which might not be compatible with new ASP on ASP.NET Core 6.0? **[Research]**
* Old ASP has its own security model which checks which establishments a user has access to - this will need to be reimplemented in new ASP
* If using DSI for future RBAC will need to make sure this returns the same as old ASP for any given establishment/user

### Strangler Facade
* Will need a list of routes available in old and new ASP
* This could be as simple as unless specifically targeted for new ASP, every request goes to old ASP
* Where does login page sit? Facade, old ASP or new ASP? **[Research]**