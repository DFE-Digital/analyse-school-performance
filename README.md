# Introduction 
ASP V2.0 

# Prerequisites
Before getting started, ensure you have the following installed on your machine: 

- [Node.js](https://nodejs.org/)
- [npm](https://www.npmjs.com/)


# Getting Started
To generate design system css/js/assets, you must run the below commands within `ASP.Web`
```
npm install
npm run build
```

# Acceptance tests using SpecFlow
The `ASP.AcceptanceTests` project needs the `SpecFlow for Visual Studio 2022` extension to edit and run the SpecFlow tests from the Visual Studio test runner. The version in the Visual Studio Marketplace doesn't support .NET 8 yet, but there is an out-of-band release that supports it, [available here](https://github.com/SpecFlowOSS/SpecFlow.VS/releases/tag/v2022.1.93-net8) (download and run the `.vsix` file.)

## Acceptance test modes
Acceptance test projects can be switched between Development mode and Integration Test mode.

Configuration is in ASP.AcceptanceTests\test.runsettings:

<RunSettings>
  <RunConfiguration>
      <EnvironmentVariables>
          <!-- List of environment variables we want to set-->
          <ASP_Test_Mode>Development</ASP_Test_Mode>
      </EnvironmentVariables>
  </RunConfiguration>
</RunSettings>
ASP_Test_Mode
value	function
Development	tests are run using an in-memory database (fast running to enable development with quick feedback)
Integration	tests are run using a real database (slow but exercises the real database connection code)
Test database config
Test database is configured using appsettings.Test.json and appsettings.Test.local.json.

appsettings.Test.json is a replica of appsettings.json in the ASP.Web project, containing this section:

    "RepositoryOptions": {
        "EndpointUri": null,
        "PrimaryKey": null,
        "DatabaseId": "test",
        "Containers": [ ... ]
        ...
     }
DatabaseId defaults to test which should be a database completely dedicated to integration tests. Integration tests can (and should) be run as part of development to catch errors but care should be taken as if two test runs are happening at the same time it will cause the tests to fail.

appsettings.Test.local.json should be created locally to point to the test database on dev. One way to avoid conflicting test runs could be if each developer has their own test database on dev and wire up the local config to point to that.

The intention is that these are run on a dedicated database on CI build - suggest a unique database is created/destroyed on each pipeline run so as to avoid issues when multiple builds are triggered simultaneously.