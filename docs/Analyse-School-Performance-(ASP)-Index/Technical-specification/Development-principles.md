[[_TOC_]]

In building ASP 2.0 we wanted to use an application architecture that was easily extensible, testable, and maintainable over the long term. To achieve this we draw on a number of ideas and principles.

# Clean architecture / Hexagonal architecture / Onion architecture

Reading list:
* https://blog.cleancoder.com/uncle-bob/2012/08/13/the-clean-architecture.html
* https://www.freecodecamp.org/news/a-quick-introduction-to-clean-architecture-990c014448d2/
* https://medium.com/idealo-tech-blog/hexagonal-ports-adapters-architecture-e3617bcf00a0
* https://jeffreypalermo.com/2008/07/the-onion-architecture-part-1/

These architectural patterns all share the same fundamental approach in slightly different ways. The main idea is that the **Domain** (or Business Logic / Entities) is the centre of the application, but it doesn't depend on any infrastructure. Instead, the rest of the application should know how to interact with it - this follows the principle of Inversion of Control. The core business logic of the application should be concerned only with modelling the business domain itself, and not know anything about how data is persisted or how to interact with the outside world.

![](https://blog.cleancoder.com/uncle-bob/images/2012-08-13-the-clean-architecture/CleanArchitecture.jpg)

In order to actually perform business actions, domain objects should be marshalled by **Use Cases** (or Domain Services) - the next layer out from the middle of the "onion". Each use case is responsible for a single business action the system needs to perform, and its job is to field a request, retrieve the appropriate domain object/s needed from the relevant **Repository/ies** (or Provider/s), perform the action and return a response. Use cases are not concerned with how the data is stored, how the requests originate or where the responses go. Use cases will need to interact with other elements of the system (e.g. retrieving data, sending emails, writing files etc.) but these interactions are abstracted away by **Interfaces** (or Ports). These interfaces have their **Implementations** (or Adapters) in the Application (or Infrastructure) layer.

![](https://miro.medium.com/v2/resize:fit:700/1*LF3qzk0dgk9kfnplYYKv4Q.png)

Each interface can have multiple implementations or maybe just a single implementation, but the key is that it is the **Application layer** (the skin of the "onion") that decides which implementations are wired up to which interfaces. This allows the use cases to be tested in isolation, free of implementation details. It also allows new implementations to be plugged in to the existing interfaces. However the main advantage this separation of concerns brings is in forcing us to consider what is core business logic and what is an implementation detail. The discipline of imagining how this might work with a different database or file provider or email client ends up creating a much cleaner structure, and helps bring to light any assumptions about the implementation that might otherwise be taken for granted.

Here is a loose representation of the ASP 2.0 architecture, showing the API app and the Web app:

![image.png](/docs/.attachments/image-2d81c664-12d2-4f1b-945b-82c1815e39d6.png)

# API-first development
Reading list:
* https://swagger.io/resources/articles/adopting-an-api-first-approach/
* https://www.postman.com/api-first/#:~:text=API%20design%2Dfirst%20is%20a,create%20a%20better%20developer%20experience.
* https://www.gov.uk/guidance/gds-api-technical-and-data-standards

Another of the key principles of ASP 2.0 is that all business logic should be available via an API. There are a number of reasons for this:

1. It is much easier to start from the beginning with an API that grows with the application than to try to extract an API from an existing application. This makes it much easier to extend the service in future by adding a mobile app, or integrating with an upstream service.
2. Forcing all business logic to be exposed via an API makes us think much more carefully about that specific piece of business logic in isolation, e.g. what is its scope, what are the parameters, likely use cases not just of this service but other services
3. It helps in documenting and visualising the functionality of the service, especially when considering its relationship to other services in a similar area - e.g. what functionality do they have in common and what could be consolidated
4. It encourages better separation of concerns by identifying what is core business logic (and therefore should live behind the API) and what is specific to the platform on which it is hosted (e.g. a web application or mobile app), leading to a cleaner overall design
5. Each element of the business logic having its own API endpoint with a clearly-defined specification allows it to be thoroughly tested in isolation from the rest of the service.
6. Certain performance optimisations now become possible, such as an intermediate caching layer between the API and the web application
7. Better data testing becomes possible by building specific endpoints that expose the raw data in various forms
8. It becomes possible to better parallelise development work by creating a stub endpoint that front-end developers can work from to build out the UI while the back-end developers work on the API

We have not followed all the principles of API-first development outlined in the articles shared above, for example API security (it is currently not a public API) or having API stakeholders. However we have created stub endpoints to enable working on the UI and API in parallel, and have a very comprehensive suite of functional tests around both the API and the web application (see [Functional tests](/Analyse-School-Performance-\(ASP\)-Index/Technical-specification/Functional-tests)

One interesting principle uncovered during development is this: if developers *can* reference the domain from the web application, they *will*! Unfortunately, due to the in-process API implementation (see [Application architecture](/Analyse-School-Performance-\(ASP\)-Index/Technical-specification/Application-architecture)) it was not possible to break the dependency between the web application and the domain, as the web application project indirectly references the API project, which references the domain. So sometimes references to domain code ended up finding their way into the web application.

It may be possible to fully break this dependency this by dynamically loading the in-process API assembly within a separate [AssemblyLoadContext](https://learn.microsoft.com/en-us/dotnet/core/dependency-loading/understanding-assemblyloadcontext) but this needs further research.

See [API](/Analyse-School-Performance-\(ASP\)-Index/Technical-specification/API) for an overview of the API and its endpoints.

# Screaming architecture
Reading list:
* https://blog.cleancoder.com/uncle-bob/2011/09/30/Screaming-Architecture.html
* https://dvmhn07.medium.com/screaming-architecture-letting-your-code-tell-its-story-203de594cf74

ASP 2.0 tries not to have its architecture dictated by the technology it's implemented in. The principle of Screaming Architecture is that by looking at the code structure you should be able to see at a glance what the code does, not what type of framework it uses. This offers several benefits:

1. Comprehensibility - the ability to look at a solution or project and know from looking what each bit should do
2. Findability - the instinct to know where to find a particular piece of code you're looking for
3. Discoverability - code that's related in functionality should be next to each other, so by looking at one piece of code it should be possible to instantly see related functionality

## ASP solution

It should hopefully be possible to see at a glance what each project does and where to look for any particular area of functionality:

![image.png](/docs/.attachments/image-0c0521dd-a23e-43b0-82a5-bae7c359c0a5.png)

## ASP.Web project

Rather than the familiar `Controllers`, `Models`, `Views` folders of MVC, we have `Areas\School` and `Features\Authentication`:

![image.png](/docs/.attachments/image-4864c800-3ded-4a2c-9b33-bc7c58cd7ca3.png)

There is still room for improvement, for instance maybe `Images`, `Scripts` and `Styles` should be grouped together under `StaticContent`, and `Extensions` is a folder containing extension method classes, which could benefit from being grouped according to function rather than type.

### Areas
Using .NET MVC Areas to represent the different physical sections of the website, each with their own sets of controllers and views:

![image.png](/docs/.attachments/image-acb8b077-d5de-4c06-8277-7d8a344155dc.png)

This requires some custom view location formats to allow views to live directly within area folders and for views within area folders to find shared views:

![image.png](/docs/.attachments/image-2d46d97f-63e1-4d4a-8476-d3cdb89ef1b0.png)

### Features
Features are supposed to represent functionality of the web application that is not confined to one physical section, e.g. authentication or search:

![image.png](/docs/.attachments/image-a50c80d4-1f49-4236-bd80-416b1662a0d5.png)

Here we can see the common elements of the School search and LA search pages have been abstracted out into the concept of a Search feature, with its own controllers and views. Also note the `AccountController` lives within `Features\Authentication`, and the `TermsOfUse` feature contains:
* the `TermsOfUseActionFilter` that attaches to each page to check whether the terms of use have been accepted
* the `AcceptTerms` component that is hosted in a content page which contains the button the user clicks to accept the terms
* the controller that button submits to

To achieve this requires a bit more finagling with MVC view locations and conventions, loosely based on code in these articles:
* [ASP.NET Core - Feature Slices for ASP.NET Core MVC | Microsoft Learn](https://learn.microsoft.com/en-us/archive/msdn-magazine/2016/september/asp-net-core-feature-slices-for-asp-net-core-mvc)
* [Feature Folder Structure in ASP.NET Core – Scott Sauber](https://scottsauber.com/2016/04/25/feature-folder-structure-in-asp-net-core/)

## ASP.Core
ASP.Core contains cross cutting concerns that can be shared across all projects. Care has been taken to group the code by functionality rather than by type (e.g. `Helpers`, `Extensions`, `Interfaces` etc.)

![image.png](/docs/.attachments/image-1cb008b0-31a8-4f6d-b65c-43756cb47819.png)

See [Application architecture](/Analyse-School-Performance-\(ASP\)-Index/Technical-specification/Application-architecture) for more of a deep dive into the application structure.

# Behaviour-Driven Development (BDD)

Reading list:
* https://dannorth.net/introducing-bdd/

ASP 2.0 draws heavily from BDD practices when building out our functional tests. For instance:

* Focus on behaviours rather than tests
* The Given/When/Then structure of acceptance criteria
* Acceptance criteria should be executable

We found there were some that were less applicable, such as:
* Acceptance criteria should be written in a common language that business stakeholders and developers both understand
* Acceptance criteria should be written in collaboration between business stakeholders

This is mainly because for a long while this was seen as purely a technical rewrite by both developers and business analysts, but as the project gradually left the path of a like-for-like replacement and more a separate service that should be defined by user needs,  we started to see BAs and developers working together on defining acceptance criteria.

See [Functional tests](/Analyse-School-Performance-\(ASP\)-Index/Technical-specification/Functional-tests) for more justification about our approach and a technical overview of the functional test framework.

# Railway-Oriented Programming

Reading list:
* https://fsharpforfunandprofit.com/posts/recipe-part2/
* https://vimeo.com/113707214

Railway-oriented programming is a term coined by Scott Wlaschin to refer to a flow for error handling within functional Domain-Driven Design.

The key principle is that failure paths are just as valuable and valid as success paths within our business logic and should be modelled explicitly. For example, when requesting an entity with a specific ID from a repository, if it is possible that an entity does not exist with that ID, and the application is expected to handle this case and do something different (e.g. branch off into a different path of logic), then the business logic function should return the failure case as a result of the business operation. 

The result of the business operation should then be an object representing a success (and containing the return value of the operation) or a failure (and containing the error message). The calling code can then handle the failure case, or simply chain the next business logic action onto the successful path, creating a structure similar to a set of points on a railway (hence "Railway-Oriented Programming"), each business logic function having the ability to switch the code from the success path to the error path:

![image.png](/docs/.attachments/image-7346d537-4d7a-4441-a8e9-a03e91667109.png)

See the resources above for more details, or [Error handling and the Result<> object in ASP 2.0](/Analyse-School-Performance-\(ASP\)-Index/Technical-specification/Error-handling-and-the-Result<>-object-in-ASP-2.0) in this wiki for information on how this is implemented in ASP 2.0.

Scott also defined some cases where Railway-Oriented Programming should not be used, which is interesting reading:

* https://fsharpforfunandprofit.com/posts/against-railway-oriented-programming/

