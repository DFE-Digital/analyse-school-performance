Within content templates there are two categories of components:

## Generic components 
These are components that could be used by any service that implemented the same templating engine. Examples of these could be:

* Heading
* Paragraph
* Table
* List
* Graphs of various types

The goal for these components would be to consolidate this set of components across all services (ASP, CSCP etc.) and extract them as a shared NuGet package. Currently these live in the `ASP.Web.Components` project which is referenced by `ASP.Web` and the components dynamically added to the web application at runtime.

## Service-specific components
These are components that are too specific to a particular service (such as ASP or CSCP) to be shared. These could be components that encapsulate a specific piece of functionality (e.g. the Pupil Characteristics table or the QLA Y6 UPN filter in ASP). Currently in ASP 2.0 there are two: 

* AcceptTerms (in `ASP.Web/Features/TermsOfUse`) 
* CookiePreferences (in `ASP.Web/Features/AnalyticsTrackingPreferences`)

It could be argued that these are generic enough to be extracted from ASP as shared components, however they both have server-side functionality implemented by separate controllers in their feature folders, and the work of extracting this functionality into `ASP.Web.Components` has not been prioritised as yet. It should just be a matter of moving the controllers into `ASP.Web.Components` and altering the component library registration logic to dynamically add any controllers present in the library.