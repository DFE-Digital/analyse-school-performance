[[_TOC_]]

Going into the ASP 2.0 build, we knew we were not going to get the test resources we needed. BAU is a very test-intensive process, and the existing testers we had for BAU were required to be split across both projects. So the developers were going to have to take on the vast majority of the testing effort.

Manual testing was not going to be feasible, so we needed a really robust set of automated tests. In fact, this was one of the goals of the rebuild, as at the start of the project, BAU had hardly any automated tests and testing was mostly a manual process. Over time, UI tests were slowly being added by the test team using Selenium, but there were very few developer tests and the ones that did exist had little actual value, and adding tests to an already existing application is very hard, since this would require fundamental architectural changes which would need tests already in place to catch any regressions.

# Requirements for functional tests
So drawing on some of the BDD principles mentioned in [Development principles](/Analyse-School-Performance-\(ASP\)-Index/Technical-specification/Development-principles), we wanted tests that:

1. would exercise as much of the code as possible
2. would be as fast as possible
5. would be stable (pass or fail predictably) and robust (not fragile)
3. would cover the potentially complex front-end logic of the page template rendering on the front end (including interactive components, complex data modules driving report pages and populating components etc.)
2. would cover the core business logic of the system
1. we could potentially define in advance as Acceptance Criteria for stories

## 1. Code coverage

This was a really important requirement, since we had to assume we would not get the test resource we needed for the project, we needed to make sure as much of the code as possible was covered by tests. Unit tests around individual pieces of code are important and necessary, but just as many bugs are surfaced from the interplay of these self-contained pieces of code, when populating the domain from the database, or when interacting with the domain/API in the UI.

Integration tests however are famously slow as they test the complete application including a real database, real instances of integrated services such as APIs, and use a real web browser (for web applications). These integration tests are also very important and necessary and must not be forgotten but we need to find a good balance that is somewhere between the two extremes of small unit tests and large integration tests.

Part of the problem in finding the right level for tests is defining the boundaries of the system under test. What is under our control and what is outside of our control? What is "our system" and what are 3rd party dependencies? Another way of thinking about it could be what are the "inputs" and "outputs" of the "system" - i.e. if we want to treat the system we want to test as a "black box" and throw certain inputs at it and check if we receive certain outputs. 

For the API we might consider as inputs:
* HTTP requests
* Data being read from a database/other storage
  * This could include network or database errors
* Responses from 3rd party systems
  * This could include network or application errors

For the API outputs we might consider such things as:
* Successful HTTP responses
* Failure HTTP responses
* Application exceptions
* Requests to 3rd party systems
* Data being written to a database/other storage
* Log messages

For the web application the above also apply but might also include as inputs:
* User interactions with the HTML ODM
* Responses from the API

And outputs:
* Changes to the HTML DOM
* Requests to the API

This leads us to draw abstraction boundaries around our system along the lines of where it meets the "real world" - a "real" database, a "real" filesystem, a "real" network, a "real" web browser. This then allows us to mock out the boundaries of the system and simulate these inputs and outputs:
* an in-memory database that can have data inserted into it as part of test setup
* a mock HTTP transport layer that we can pass HTTP requests/responses to and simulate network errors
* a mock implementation of a 3rd party system we can control and inspect

This also leads us to discover certain aspects of our system that we may not have considered that are integral to the core functionality of the service and also need to be covered by functional tests:
* Network and database errors (does the system handle them gracefully)
* Validation and parsing of HTTP requests (including JSON deserialization)
* Validation of data retrieved from the database (especially if the database is populated by a separate system or process)
* User interactions with the HTML DOM (including with JavaScript turned on or off)

## 2. Speed

Fast tests are key if they are to be run regularly as part of a developer's workflow. The faster the tests they are, the less of a mental block they will be to run, and the likelier they will be run after any change to check for regressions. Anything slower than something on the order of a few seconds causes interruptions to a developer's flow and creates obstacles to productivity.

What is the main bottleneck for speed? Interacting with the "real world", especially something that involves a network request or a file system, these are many orders of magnitude slower than most internal operations of our system. So if we can mock out these "real world" elements we should be in a very good position. 

## 3. Stability and robustness

For tests to be most useful, they must be:
* stable: they pass or fail predictably and repeatably (for example not being dependent on unpredictable elements such as the order the tests are executed, or certain hardware configurations etc.)
* robust: not fragile, a change the the code that does not change the functionality under test should not cause the test to fail (for example changes to the presentation layer or refactoring of code internal to the system)

Stability is hard to achieve with integration tests, as the system under test is inherently a "real world" system. 

Robustness is hard to achieve when the majority of system tests are unit tests, as this means the tests are now tightly coupled to a particular code structure/architecture. This means the tests cannot serve as regression tests over large structural changes as the tests themselves will need to be changed at the same time as the code they are testing. It also means that any refactoring work will be double the effort, as not just the code but the tests will need to be refactored. Any refactoring should by definition not change the functionality of the code, and so a good suite of tests should function as guard rails to allow significant changes to the codebase to be made efficiently and with confidence.

## 4. Front-end tests

Fortunately, ASP.NET Core MVC provides us with [a mechanism for integration tests](https://learn.microsoft.com/en-us/aspnet/core/test/integration-tests?view=aspnetcore-5.0) which virtually hosts a web application using [WebApplicationFactory](https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.mvc.testing.webapplicationfactory-1?view=aspnetcore-9.0) and simulates the HTTP transport layer. This allows us to spin up a real web application within a test suite, send HTTP requests to it, and receive the responses as HTML, and then inspect those responses with a HTML parser such as [AngleSharp](https://anglesharp.github.io/general/introduction).

If we then combine this with the powerful dependency injection features of .NET Core, we can easily swap out the other "real world" dependencies for in-memory versions, as long as our interfaces are appropriately defined (see [Application architecture](/Analyse-School-Performance-\(ASP\)-Index/Technical-specification/Application-architecture)).

## 5. Business logic tests

We would also like to have a separate suite of tests that exercise the business logic specifically. Since the API is effectively a wrapper for the business logic, and as long as we stick to the principle that every business logic action must go through the API, we can effectively test the business logic directly through testing the API.

Unfortunately, we can't look to the integration test framework for the API tests in the same way as the front-end tests, as there isn't an implementation of WebApplicationFactory for function apps. Luckily, the in-process implementation of the API (see [Application architecture](/Analyse-School-Performance-\(ASP\)-Index/Technical-specification/Application-architecture)) can help us here, as the `InProcessTransportLayer` we created to simulate a HTTP connection to the API can be used by the test framework to send requests to the API functions directly.

## 6. Acceptance criteria

Finally, it would be nice to be able to frame the tests in such a way that they can be understandable by business stakeholders, and even written in collaboration between the developers, testers and business analysts. Ideally we would like to write the tests in advance as acceptance criteria for stories, and maybe even publish the tests as HTML reports that can be viewed by anyone. These tests could then function as an executable specification of the system.

`SpecFlow` provided us with this framework, which allowed us to create the tests in a natural language form using the Given, When, Then structure, and these test steps could have implementations that set up data in the in-memory store, interact with the API/application, and assert on the API responses/HTML on the page. The suite of functional tests along with the test results can then be published to ADO via the [SpecFlow+ LivingDoc page](https://dfe-ssp.visualstudio.com/s192-Analyse-School-Performance%20(ASP)/_apps/hub/techtalk.techtalk-specflow-plus.techtalk.specflow.plus.hub) whenever any code was merged.

# Overview of test projects
![image.png](/docs/.attachments/image-e4466a9a-8073-4bc5-89dd-6ab9e4a1bdf4.png)

ASP 2.0 uses [XUnit](https://xunit.net/) as its test framework, with [Reqnroll](https://reqnroll.net/) for functional tests, [AngleSharp](https://anglesharp.github.io/general/introduction) for parsing the HTML DOM, and [Playwright](https://playwright.dev/) for executing Javascript.

## ASP.Core.UnitTests
Contains unit tests for the `ASP.Core` project

## ASP.Domain.UnitTests
Contains unit tests for the `ASP.Domain` project

## ASP.Web.UnitTests
Contains unit tests for the `ASP.Web` project

## ASP.Web.Core.UnitTests
Contains unit tests for the `ASP.Web.Core` project

## ASP.Test.Core
Contains classes common to all test projects, mainly custom assertions. This depends on  the `xunit.assert.source` package, which allows us to add our own assert methods directly to the main XUnit `Assert` class - [more information](https://xunit.net/docs/nuget-packages-v2#extenders)

## ASP.Test.Reqnroll
Contains Reqnroll step definitions common to both `ASP.Api.FunctionalTests` and `ASP.Web.FunctionalTests`

## <span>ASP.Test.Web</span>
This project is added dynamically to the ASP.Web application when the `ASP.Web` functional tests are run. This allows us to add extra functionality that is only relevant to tests, without polluting the `ASP.Web` project itself with test concerns.

This extra test functionality includes:
* `TestComponent` - a page template component purely for testing that allows us to test the page templating engine without depending on any one specific component to exist in the web application
* `ComponentTestController` - a controller that renders a page template containing a single component, which can be set up to be a different component for each test. This allows individual components to be tested in isolation without depending on them being included in any specific page template
* `ErrorTestController` - a controller that has actions throwing different kinds of errors, used to reliably trigger that particular error type to test the error handling of the application
* `AuthorizationTest` - a controller that has actions set up with different policies, to test the authorization logic

## ASP.Api.FunctionalTests 
Contains the functional tests for `ASP.Api`

## ASP.Web.FunctionalTests
Contains the functional tests for `ASP.Web`

# API functional tests
Here is an example of a typical functional test for the API:
```
Scenario: Should return a 200 response with results and expected pagination for the given resultsPerPage
  Given establishment Primary School 111111 (111111) exists
  And establishment Primary School 222222 (222222) exists
  And establishment Primary School 333333 (333333) exists

  When I send a GET request to /api/schools?resultsPerPage=2

  Then I should get a 200 response
  And the response should be an object containing these properties (ignoring null values):
    """
    {
      "TotalResults": 3,
      "ResultsPerPage": 2,
      "Page": 1,
      "Results": [
        {
          "Urn": "111111",
          "Name": "Primary School 111111"
        },
        {
          "Urn": "222222", 
          "Name": "Primary School 222222"
        }
      ]
    }    
    """
```

**Given:** Sets up 3 establishments in the database (memory store)
**When:** Sends a GET request to the API to get all the schools, with 2 results per page
**Then:** Asserts that the response is successful, and contains the first page of results which is the first two schools

Hopefully this test is fairly self-explanatory.

API functional tests are broken up into a feature (set of tests) for each endpoint.

# Web functional tests
Here is an example of a typical functional test for the web application:
```
@Javascript:disabled
Scenario: Page should show a breadcrumb trail when search returns no results
  Given establishment Some Primary School (111111) exists in local authority 999
  And establishment Some Other Primary School (222222) exists in local authority 999
  And local authority Oxfordshire (999) exists

  When I navigate to /local-authority/999/schools/
  And I update the textbox "#app-field-Search" to have the value "Secondary"
  And I click the button "#searchSubmit"

  Then the path should be /local-authority/999/schools/?search=Secondary
  And the breadcrumb trail should be:
    | Link Text             | Url                           |
    | Home                  | /                             |
    | All local authorities | /local-authorities/           |
    | Oxfordshire           | /local-authority/999/         |
    | All schools           | /local-authority/999/schools/ |
```
**Given:** Sets up 2 establishments in the database (memory store) that have the text "Primary" in the name
**When:** Navigates to the schools page for local authority 999, enters "Secondary" in the search box and presses the search button (this search should return no results)
**Then:** Asserts that the page path is correct, and that the breadcrumb trail contains the correct links text and URL

##Notes
* `@Javascript:disabled` is a tag applied to the test, which indicates which web driver to use to execute the test. In this case the `AngleSharpWebDriver` is used, which uses `AngleSharp` to parse the HTML returned from the (virtual) web server, assert on the HTML DOM and execute limited functionality such as clicking on links and submitting forms. This is all that is needed for most tests as the core functionality of the service is required to work with JavaScript disabled, using standard HTML. 
* If the particular piece of functionality on the front-end is using progressive enhancement to dynamically replace the standard HTML with a interactive client-side functionality, the `@Javascript:enabled` tag should be used. This uses the `PlaywrightWebDriver` instead to execute the test using a real browser within Playwright. The real browser has a Javascript engine so the interactive functionality can be tested. As these tests are much slower, it is recommended that only the tests that explicitly test Javascript fuctionality should be tagged with `@Javascript:enabled`
* The Web functional tests are split into two sections: `Components`, which contains functional tests around individual components, and `Service`, which tests specific pages and functionality within the ASP service itself. `Components` should really be split out into its own functional test project, as it tests the components contained in `ASP.Web.Components` - these are intended to be extracted out as a shared package at a later date, so the functional tests for these components should be extracted out too.

# SpecFlow EOL and Reqnroll
Unfortunately SpecFlow abruptly reached end-of-life in December 2024, and all the repositories and online documentation were deleted. As part of the final process of pausing development on ASP 2.0, the tests were migrated to [Reqnroll](https://reqnroll.net/) which is an open-source reimplementation of SpecFlow. 

This migration was successful, however the only thing that was not possible to fix is the ADO SpecFlow+ LivingDoc extension which fails if the total size of all the tests published to it is greater than 5MB, which is now the case with our functional tests. Unfortunately a Reqnroll implementation of this extension for ADO has yet to be developed, but it does seem like [there is some progress on this](https://github.com/orgs/reqnroll/discussions/68) - maybe by the time ASP 2.0 development is restarted an implementation will be available.