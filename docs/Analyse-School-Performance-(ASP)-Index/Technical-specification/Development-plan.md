[[_TOC_]]

# Feature mapping
In 2023 the ASP team sat down and compiled a list of all the functionality of ASP BAU. They then identified each individual feature and produced T-shirt sizing estimates for each. This work is available as [ASP Frontend Matrix.xlsx](https://educationgovuk.sharepoint.com/:x:/s/ASP/Eb_26HLpCNtPvEYj3O0WmWkBtCwp72V9Yj3xyXE7G5jFTg) - see the `Features` tab for the list of features with their T-shirt sizing, and `Estimations` for a breakdown of the total effort needed for the whole project.

After this, [a feature map was created in LucidSpark](https://lucid.app/lucidspark/90f5310b-161d-47e0-bdbb-a8105558e55b/edit?beaconFlowId=9A1FB1069CC32CE5&invitationId=inv_be2964e3-83be-46f3-bfad-4149354d894f&page=0_0#), the aim of this being to lay out all the features showing a logical path for development, starting with simpler features and building on those to the more complex functionality:

![image.png](/.attachments/image-bfffb0dc-5676-43aa-aeb6-73d3fcd5a2ba.png)

In this diagram, features that built on previous features are connected with arrows such that `Feature A` -> `Feature B` means that Feature B is dependent on Feature A, or that  Feature A needs to be built first, then Feature B. This then allows us to identify what features can be built in parallel, and identify a critical path for development (shown as the thick set of arrows)

**Note:** the following symbols on a feature card indicate the completion of the feature at the time of writing (March 2025)
 - ✅ the feature is complete
 - ⌛ the feature is partially implemented.

Zooming in on this diagram we can see several distinct areas of functionality.

## Web application and static content pages
![image.png](/.attachments/image-fac596c0-45fc-46cc-a4c6-975ccfa79df1.png)
These features are concerned with getting a web application up and running that serves static content pages. These content pages should be data-driven and rendered using the page template engine. 

In order to render the various types of content present in the static content pages, components for the template engine will need to be developed, such as paragraphs, links, lists, tables. These are represented by the light blue cards under each feature.

There is also a need for the static content pages to be editable, and site news articles to be created and managed. These are represented by the darker purple feature cards. There will eventually need to be an admin tool that can provide a user-friendly way to edit these pages, which will probably be a self-contained web application and should also provide versioning of pages. Until that is implemented the static content pages will need to be edited directly in the database.

## Banners
![image.png](/.attachments/image-b8d3a36b-81a7-4c39-906c-216444b8ef70.png)

Banners are a way of communicating with users using the service, and should either be displayed permanently at the top of the page, or be able to be dismissed by the user. Banners are either purely informational (e.g. a pinned site news article) or provide some functionality (such as accepting the terms of use or the use of analytics cookies)

## Summary reports and data downloads
![image.png](/.attachments/image-bc7124da-9d0a-4be3-8806-90e24c483abc.png)

ASP BAU contains summary reports which are dynamically generated. In ASP 2.0, the idea is to remove the distinction between summary reports and reports for the current year, instead allowing the user to view the same report over historical years by means of a year dropdown or similar navigation method. However, there still needs to be a way of viewing historical reports (released before ASP 2.0 goes live), which will be achieved by scraping the summary reports from BAU for each school, and saving them as static HTML files into blob storage.

Data downloads will also need a similar approach - data download files from the release of ASP 2.0 onwards can be generated as each dataset is released, but historical files will need to be extracted from the BAU service and imported into blob storage for use by ASP 2.0.

## School/LA pages and user roles
![image.png](/.attachments/image-820a5962-d33a-4f7c-a014-56f6e9ee7f02.png)

Showing the different pages required for the different user roles. **Note:** more pages were needed after enhancements were made to the UX prototype, such as My schools and the Generic school page.

## Data set reports
![image.png](/.attachments/image-1348b604-661f-425f-8475-079275e64f78.png)

Each of the data sets in BAU are arranged in ascending order of complexity, showing the different components required for the report pages.

# Epics estimation

Once the features above were mapped, they were divided into epics and re-estimated by the dev and test teams, giving an overall timeline for development: [Epics estimation.xlsx](https://educationgovuk.sharepoint.com/:x:/r/sites/ASP/Shared%20Documents/ASP%202.0/Preparation/Epics%20estimation.xlsx?d=w89f499c542dc4e10947a7f170ecbbe31&csf=1&web=1&e=3fsbHc)

# Phased development

After development had already been well underway for several months, the Programme Board decided to move away from a like-for-like replacement of ASP BAU to a phased development plan, the goal of the first phase being the absolute minimum set of features that could be released as a viable service to replace ASP BAU. This required a complete rethink of the project plan from a logical progression of features building from simple to more complex, to a progression of feature sets from most essential to least essential. 

The decision was made that the most essential feature would be the ability to download the raw data as and when it was released, and the ability to view it in report pages presented as tables or charts would be a secondary feature to be developed at a later date. At least if the user had access to the raw data they could import it into Excel and create their own charts if needed.

The phased development plan is outlined in [ASP 2.0 Minimal MVP](https://educationgovuk.sharepoint.com/:w:/r/sites/ASP/Shared%20Documents/ASP%202.0/Preparation/ASP%202.0%20Minimal%20MVP.docx?d=wec5e004708ac4adb85ec5db194946fc1&csf=1&web=1&e=ygDRpT). This is reproduced below with an indication of the completion state of each feature at the time of writing (March 2025):
 - ✅ the feature is complete
 - ⌛ the feature is partially implemented.

## Absolute minimum Minimum Viable MVP Product (Phase 1) 
- **Home page** ✅
- **Help content pages (read only)** ✅
- **DfE Sign In integration/RBAC** ✅
  - tied in to existing BAU service? Is this possible? 
    *[New service created in DSI]*
- **Downloads (KTS, ASP – MTC, Phonics, QLA only)** ✅
- **Accept terms of use** ✅ 
- Auditing (?) – write only 
- Link to satisfaction survey 

## Must-have features (Phase 2) 
- **School search** ✅
- **School landing page** ✅
- School dataset report pages (only unnamed data view?) 
  - Phonics 
  - MTC 
  - QLA Y6 
  - QLA Y7 
- **LA search** ✅
- **LA landing page (no reports)** ✅
- **My schools (LA/MTC/Diocese/Governor)** ✅
- Admin tool (add/remove/edit JSON documents from certain containers) 
- Publishing/versioning of content pages/dataset reports 
## Nice to haves (Phase 3)
- **DSI integration with separate service (ported/transformed users/roles)** ⌛
  *[DSI integration completed, ported users/roles not implemented]*
- **Linked schools** ✅
- **Named/unnamed data access** ⌛
  *[Named data access implemented for downloads]*
- Analytics 
  - **Accept analytics cookies** ✅
- Site news 
- Banners 
- Pinned news 
- Page content editing UI 
- School dataset report pages 
  - KS2 
  - KS4 
  - KS4 destinations 
  - School characteristics 
  - Absence/exclusions 
- Historical reports 
  - Generation of static files 
  - Static file viewer in UI 
- Dataset year picker (will need after first year) 
- Download PDF 
- Download as CSV 
- Test school/training data 
## Future 2.0 Functionality (Phase 4)
- Admin tool UI for report pages 
- User-defined reports (?) 

# UX enhancements
Although development is technically at the end of Phase 1, you may notice that certain elements of Phases 2 & 3 have also been completed. This was due to some enhancements that were made to the UX of the service. Whereas in BAU the Downloads section was part of the site header, it was felt that a better user experience would be to include the downloads as part of the school page or local authority page. This required the school and local authority pages to be built, including navigating and search functionality. 

This also allowed the DSI integration and role-based access control to have something to work with, as without these pages there would be nothing for users without access to downloads to see. With these pages, every user would be able to see details of the school(s) in their remit and navigate between them, with users having access to Named data also being able to download data.

In order for this all to be delivered as part of Phase 1, certain features such as auditing would have to be postponed to a future phase.