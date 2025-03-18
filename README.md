# Introduction 
ASP v2.0 

# Prerequisites
Before getting started, ensure you have the following installed on your machine: 

- [Node.js](https://nodejs.org/)
- [npm](https://www.npmjs.com/)
- [Visual Studio 2022](https://visualstudio.microsoft.com/vs/) (or higher)
- [.NET 8.0 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0) (or higher)

# Getting Started
To install the required node packages, you must run the below command within `ASP.Web`
```
npm install
```

# Running the ASP web application locally
## Building the front-end assets
There are two npm scripts that you can use to run the application in development mode.

```
npm run build-dev
npm run watch
```

`build-dev` will run webpack in development mode and output un-minified CSS and JS to `wwwroot\assets`.

`watch` will run webpack in development and watch for changes to SCSS and JS files in `Styles` and `Scripts` folders. This allows you to see style and Javascript changes in the browser as you make them in application codebase.

The npm scripts above must be run from within the `src/ASP.Web` folder. To make this less annoying there are two batch files in the repository root (`web-build.bat` and `web-watch.bat`) that run from the root folder and execute the corresponding npm script:

  ```
  C:\code\asp>web-build
  C:\code\asp>web-watch
  ```

  Equivalent bash scripts are provided for running in git bash terminal:

  ```
  $ ./web-build
  $ ./web-watch
  ```

If you want to also see any updates you make to HTML files reflected in the browser automatically, you will need to use Visual Studio's 'Hot Reload' feature. 

  To improve the 'Hot Reload' experience, you can activate the 'Hot Reload on save' feature from the 'Hot Reload' settings. If you also install the 'Auto file save' extension you can automate saving and hence automatically trigger 'Hot Reload'.

## Building the front-end assets for production
The following npm script will run webpack and output minified CSS and JS to `wwwroot\assets`:

```
npm run build-prod
```

## App settings
To run `ASP.Web` locally, you'll have to add the appropriate app settings by creating `.secrets.json` files - **do not check these in!** There should be a `.gitignore` entry for these files to ensure they are not accidentally added.

In ASP.Web there should be an `appsettings.web.json` file, and an `appsettings.web.secrets.template.json` file. Simply copy the template file as `appsettings.web.secrets.json` and fill in the missing values, referring to the `appsettings.web.json` file as a guide for which values are needed for local development.

For example here is an extract from `appsettings.web.json`:
```
{
    // Null settings values will be populated as indicated by the comments as folllows
    // local only:      populated from appsettings.web.secrets.json                                                     (required for local development)
    // deployed only:   populated during deployment from pipeline variable groups s192d01-group and s192t01-group       (not needed for local development)
    // local, deployed: populated from appsettings.web.secrets.json and during deployment from pipeline variable groups (required for local development)
    
    "DocumentDatabase": {
        "InMemory": false,
        "EndpointUri": null,             // local, deployed
        "PrimaryKey": null,              // local only
        "ManagedIdentityClientId": null, // deployed only
        "DatabaseId": "asp",
        "Containers": {
            "content": {
                "ContainerName": "content",
                "PartitionKey": "/contentId"
            },
            "establishments": {
                "ContainerName": "establishments",
                "PartitionKey": "/urn"
            },
            ...
        }
    }
}
```

Note the null values for `DocumentDatabase.EndpointUri`, `DocumentDatabase.PrimaryKey` and `DocumentDatabase.ManagedIdentityClientId`. These are simply placeholders that will need to be populated either from `appsettings.web.secrets.json` or by the deployment pipeline from the appropriate variable group when deploying the application to the Dev or Test environment.

Note also the comments indicating that `DocumentDatabase.PrimaryKey` is only needed for local development, and `DocumentDatabase.ManagedIdentityClientId` is only needed when deploying the application.

Here's the corresponding section of the `appsettings.web.secrets.json` file:
```
{
    "DocumentDatabase": {
        "InMemory": false,
        "EndpointUri": "https://s192d01-cdb-dev.documents.azure.com:443/",
        "PrimaryKey": "<Replace with Cosmos DB Primary Key for s192d01-cdb-dev>"
    }
}
```

# Running the API function app locally
## App settings
Similar to the web application, in order to run the API locally the appropriate app settings will need to be added. Inside `ASP.Api` there should be an `appsettings.api.json` file and an `appsettings.api.secrets.template.json` file that can be copied and modified to create the `appsettings.api.secrets.json` file needed for local development.

## Functions key
When sending requests to the API function app hosted on the Dev or Test environments, the `x-functions-key` header must be included in every request. The value for this header can be obtained from the function app in Azure Portal by navigating to Functions -> App keys -> Host keys (all functions).

When sending requests to the function app running locally, there is no need to include the `x-functions-key` header, this is due to the Authorization level being 'Anonymous' when running locally.

## Troubleshooting Local API Project Setup
If you encounter a "There is no functions runtime available that matches the version specified" error while attempting to run the API project locally, follow these steps to resolve the issue:

- Navigate to Tools -> Options -> Projects & Solutions -> Azure Functions in Visual Studio.
- Click on the "Check for updates" button.
- Allow Visual Studio some time to update. Note that the update process may take a while, and Visual Studio may crash during this time.
- Once the update is complete, attempt to run the API project again.

# Functional tests using SpecFlow
The `ASP.Web.FunctionalTests` and `ASP.Api.FunctionalTests` projects need the `SpecFlow for Visual Studio 2022` extension to edit and run the SpecFlow tests from the Visual Studio test runner. The version in the Visual Studio Marketplace doesn't support .NET 8 yet, but there is an out-of-band release that supports it, [available here](https://github.com/SpecFlowOSS/SpecFlow.VS/releases/tag/v2022.1.93-net8) (download and run the `.vsix` file.)

**Note:** As of December 2024, SpecFlow is no longer supported, so functional tests will need to be migrated to an equivalent framework, e.g. [Reqnroll](https://docs.reqnroll.net/latest/guides/migrating-from-specflow.html)

## Javascript enabled/disabled
The `ASP.Web.FunctionalTests` spin up the `ASP.Web` web application in a test host using ASP.NET Core's `WebApplicationFactory`. This allows us to interact with the HTML on the page to fully test the behaviour of the application and components. The web application must be functional when Javascript is disabled and when it is enabled, so in order to test the functionality in each circumstance a different web driver must be used.

Each test scenario that interacts with the web application must specify which web driver to use for the scenario, which is done by adding the `@Javascript:disabled` or `@Javascript:enabled` attribute. This tells SpecFlow to use either the `AngleSharp` or `Playwright` web driver. `AngleSharp` is the preferred web driver for tests that do not require Javascript as tests that use it are quicker and require less initializing time. `Playwright` is a browser-based automation tool that spins up a full web browser, so while this allows testing of the Javascript and CSS on the page this has an overhead, so should only be used when necessary.

## Playwright setup
Before the Javascript tests can be run, the Playwright browsers must be installed locally. To do this, run the PowerShell command `pwsh bin/Debug/net8.0/playwright.ps1 install` from within the `test\ASP.Web.FunctionalTests` directory (the solution must be built first). See https://playwright.dev/dotnet/docs/intro for more detailed instructions.

## Test runner playlist to exclude Javascript tests
As the Javascript tests take longer to run, a playlist exists dynamically excludes all the `@Javascript:enabled` tests from the test run. This is just to make it easier to skip those tests if they are not needed. To use this playlist go to the `Test Explorer` in Visual Studio, and click `Open Playlist File`, then select the file `test\Exclude Javascript tests.playlist`. This opens the playlist in a separate Test Explorer view which can be run separately.

## Generating reports locally

You can generate the SpecFlow LivingDoc test html page locally by using the `livingdoc` CLI tool.
To install this run the following from the repository root folder:

```
dotnet tool install --global --configfile NuGet-ToolInstall.config SpecFlow.Plus.LivingDoc.CLI
```
Then once installed you can run `livingdoc feature-folder` with a test project root like so:
```
livingdoc feature-folder test/ASP.Web.FunctionalTests
```
Documentation for this is [here](https://docs.specflow.org/projects/specflow-livingdoc/en/latest/LivingDocGenerator/CLI/livingdoc-feature-folder.html)

## Using a real database

The test web application is configured using `appsettings.web.test.json` and `appsettings.web.test.secrets.json` (or `appsettings.api.settings.test.json` and `appsettings.api.test.secrets.json` for the API) - the `appsettings.web.test.json` file overrides values from `appsettings.web.json` in the ASP.Web project.

By default, `appsettings.web.test.json` is configured to set the document database and blob storage to use in-memory implementations: 

```
{
    ...
    "BlobStorage": {
        "InMemory": true,           // Override in appsettings.web.test.secrets.json to use real blob storage
        "StorageAccountName": null, // Override in appsettings.web.test.secrets.json to use real blob storage
        "PrimaryKey": null          // Override in appsettings.web.test.secrets.json to use real blob storage
    },
    "DocumentDatabase": {
        "InMemory": true,           // Override in appsettings.web.test.secrets.json to use real database
        "EndpointUri": null,        // Override in appsettings.web.test.secrets.json to use real database
        "PrimaryKey": null,         // Override in appsettings.web.test.secrets.json to use real database
        "DatabaseId": "test"
    }
    ...
}
```

These can be configured to use a real database/blob storage instance by overriding these in `appsettings.web.test.secrets.json`, e.g.:

```
{
    ...
    "BlobStorage": {
        "InMemory": false,
        "StorageAccountName": "<Replace with test Blob Storage account name>",
        "PrimaryKey": "<Replace with test Blob Storage Primary Key>"
    },
    "DocumentDatabase": {
        "InMemory": false,
        "EndpointUri": "<Replace with test Cosmos DB Endpoint URI>",
        "PrimaryKey": "<Replace with test Cosmos DB Primary Key>"
    }
    ...
}
```

`DocumentDatabase:DatabaseId` defaults to `"test"` which should be a database completely dedicated to integration tests. Integration tests can (and should) be run as part of development to catch errors but care should be taken as if two test runs are happening at the same time it will cause the tests to fail. One way to avoid conflicting test runs could be if each developer configures `appsettings.web.test.secrets.json` to point to their own dedicated test database on dev, or an instance of Azure Cosmos Db Emulator.

The intention is that these are run on a dedicated database on CI build - suggest a unique database is created/destroyed on each pipeline run so as to avoid issues when multiple builds are triggered simultaneously.