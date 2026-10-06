[**Note:** This functionality is not yet implemented in ASP 2.0]

[[_TOC_]]

ASP effectively has two types of data accessible to the user:

* "Current" data - i.e. the last year's worth of data
* "Historical" data - anything else, e.g. summary reports

For ASP 2.0 MVP we need to resolve some ambiguities about current behaviour in order to implement it as efficiently as possible.

Note that these behaviours are purely for ASP 2.0 MVP, we can change these in the future for ASP 2.0 proper.

# Top navigation
![TopNav.png](/.attachments/TopNav-06474e5b-0f16-4c1f-a3e3-b090e894ea4c.png)

* The top navigation will show all the Data Sets ***applicable*** to an establishment - i.e. only show the `Key Stage 2`  tab if it is a primary school
  * Each Establishment has a set of flags specifying what school years it covers: 

        IsPrimary
        IsJuniorSchool
        IsMiddleSchool
        IsSecondary
        IsPost16

  * Each Data Set will define which of these flags it is applicable to as part of its metadata, and the navigation will show only those tabs for Data Sets that match
* If a Data Set is not ***available*** for an Establishment (i.e. data for that Data Set does not exist for the establishment for the current year) then the tab will be greyed out or have some other way of indicating the data is not available (see the `Phonics` tab above)
  * Data Set availability will be determined by querying the actual Data Set data for the Establishment URN - either by executing a query against the data directly or against a metadata table
  * If using a metadata table this will be updated whenever Data Set data is imported into ASP (or Data Set data is updated/corrected)
* `All reports` will be replaced by a tab called `Summary reports` or `Historical reports` - this will  be a page containing just the list of School Performance Summary reports available for the establishment:

  ![Summary reports.png](/.attachments/Summary%20reports-3b3df1cd-5e4e-4cb9-8c86-8e1fd5fb53f0.png)
* Reports that currently sit within `All reports` will be moved under their own Data Set tab, e.g. `Absence and Exclusions` and `School characteristics` (maybe with shortened names so they don't take up too much space in the navigation)
* **Note:** need to think about where KS4 Destinations will sit, it is released separately from the rest of Key Stage 4 and is for a different school year (2021, whereas KS4 release is 2022)

# Current data

## Definition
"Current data" in ASP is defined as the ***most recently released year of data for each Data Set***. Note that this is different to *all of the following*:

* "data for the current year"
  * Obviously we are interested in school years which start in Autumn and end in Summer, so we talk about data for e.g. 2021/22
* "data for the current school year"
  * Some of the data we release is for the school year that is currently in session, e.g. QLA data became available during the Summer term. however most of the data that is released is for the previous school year as by the time the data is processed the next year has started
* "data for the previous school year"
  * As mentioned above, some of the data is for the current school year
* "data about the preceding calendar year period"
  * KS4 Destinations data was *released* in May 2023, but it is *about* pupils who left school in 2021 - it has 2021 in the URL: https://asp-dev-web.azurewebsites.net/2021/Report/Destinations/146635
* "data released over the preceding calendar year period"
  * Not all data is released every year (e.g. we missed some during Covid) so the most current data for a Data Set may be more than a year old
* "the latest data released for an Establishment"
  * When an Establishment closes the latest data available for that Establishment will gradually become historical data as newer releases are published for those Data Sets
  * When an Establishment changes type or is merged, a new Establishment record is created with a new URN, and any future data is released against the new URN. This means that the old Establishment is effectively closed, and the previous point applies

## Implications
In order to know what Data Set data to show in the navigation for a given Establishment, ASP needs to know:
  1. What Data Sets are applicable to the Establishment? (covered above)
  2. For each of those Data Sets, what is the most recently released year of data?
  3. For each of those Data Set/Year combinations, is there data available for the Establishment?

In order to establish (2), ASP could calculate this dynamically by querying Data Set data to find the latest year for each Data Set for *any* Establishment, or it could keep some extra metadata to keep track of the latest year that was released for each Data Set.

# Historical data
Any data not falling under the definition of "current data" above is therefore "historical data" and is treated differently:

* Users are not able to view interactive reports for historical data in the same way as current data e.g. 
  * filtering reports and charts
  * switching between chart and table views
  * changing the axes and data on scatter plots, filtering by UPN à la QLA
* Users will be provided with a static view of such data - in the form of summary reports
* Users will not be able to view historical data for a current report just by changing the URL to point to a different year
  * This is allowed in current ASP but is a bug and should not be replicated for ASP 2.0 MVP
* Establishments that have closed, changed type or merged will only show data for as long as the most recently released data for that Establishment coincides with the Current Data for those Data Sets
  * This means that when an Establishment is closed, initially it will show the latest Data Sets, but over time those will disappear as later data for those Data Sets gets released, until there is no data to display at all

# Summary reports
Currently in ASP, summary reports are generated dynamically using the old report code and data. We do not with to replicate this design for a number of reasons:

* Old data has to be kept in the database increasing costs and decreasing efficiency of data queries
* Old report code has to be kept around in the application, increasing the amount of code that needs to be maintained and increasing the risk of bugs
* Summary reports are static views of the historical reports, and so do not need to be dynamically rendered if the reports themselves could be cached in some way
* Replicating this design would mean we would need to re-implement all the historical reports for all the Data Sets, which would involve:
  * Converting all the historical data into the new data structure used by ASP 2.0
  * Converting all the historical reports into the new page template structure for ASP 2.0
  * Implementing components for all graphs and tables in their various historical forms
  * Implementing static views for these components

  all just to replicate the static reports that already exist in current ASP.

Instead, summary reports will be implemented as cached HTML versions of the summary reports currently in ASP. This will involve:

* Creating a web scraper tool to walk through all the summary reports in current ASP for all years for all Establishments, extract the HTML and CSS, convert graphs into images, and save them all into blob storage
* Creating a summary report viewer that will load a report from blob storage based on URL and embed it into the page
  * HTML and CSS will need to be isolated from the rest of the application in order not to conflict with current report styles, so reports should be embedded in an `<iframe>` or as a web component with shadow DOM
* Creating a process in ASP 2.0 MVP where once a Data Set is released it is cached in the same way and added to the summary report for the current year
  * There needs to be a page or view or process that compiles the reports for the current data for each Establishment into a cached version to be saved into blob storage
  * There needs to be a way to define which reports from each Data Set end up in the summary report, and to define a consistent ordering of Data Sets within the summary report
  * Each report and component will need a static view in order for a static version of it to be cached
  * Each time a Data Set is released the summary report for the current year for all Establishments will need to be re-cached to add the new Data Set to it

# Retention period

Historical data in ASP 2.0 MVP will be exclusively limited to summary reports, and so when a new version of a Data Set is released, since we have cached the reports in the summary report for that year, the previous version of that data can be deleted, and also the page templates that defined those reports.

We should probably be careful and keep the data and templates around for a period just in case there are bugs that we need to fix, and we need to regenerate the summary report, but after that period of time we should be free to delete the old data and templates.

We should also have a retention period for summary reports, to indicate how many years' worth of summary reports we keep around.

Here are the proposed retention periods:

* Data and templates for current data: **1 year**
* Summary reports: **5 years**