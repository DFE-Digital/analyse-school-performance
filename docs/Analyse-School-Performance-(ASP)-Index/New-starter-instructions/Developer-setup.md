# Introduction   
ASP V2.0   
  
# Prerequisites  
Before getting started, ensure you have the following installed on your machine:   
  
- [Node.js](https://nodejs.org/)  
- [npm](https://www.npmjs.com/)  
  
  
# Getting Started  
To install the required node packages, you must run the below command within `ASP.Web`  
```  
npm install  
```  
  
# Running the application in development mode  
There are two npm scripts that you can use to run the application in development mode.  
  
```  
npm run build-dev  
npm run watch  
```  
  
`build-dev` will run webpack in development mode and output un-minified CSS and JS to `wwwroot\assets`.  
`watch` will run webpack in development and watch for changes to SCSS and JS files in Styles and Scripts folders. This allows you to see style and javascript changes in the browser as you make them in application codebase.  
  
**Note:**  
If you want to also see any updates you make to HTML files reflected in the browser automatically, you will need to use Visual Studio's 'Hot Reload' feature.   
  
To improve the 'Hot Reload' experience, you can activate the 'Hot Reload on save' feature from the 'Hot Reload' settings. If you also install the 'Auto file save' extension you can automate saving and hence automatically trigger 'Hot Reload'.  
  
# Build the application CSS, JS and other assets for production  
The following npm script will run webpack and output minified CSS and JS to `wwwroot\assets`.  
  
`npm run build-prod`  
  
# Functional tests using Reqnroll
The `ASP.Web.FunctionalTests` and `ASP.Api.FunctionalTests` projects need the `Reqnroll for Visual Studio 2022` extension to edit and run the Reqnroll tests from the Visual Studio test runner. For more info see here: [Setup an IDE for Reqnroll - Reqnroll Documentation](https://docs.reqnroll.net/latest/installation/setup-ide.html)  
  
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
  
## Functional test modes  
Functional test projects can be switched between Development mode and Integration Test mode.  
  
Configuration is in `test.runsettings` in the functional test project root:  
  
```  
<RunSettings>  
  <RunConfiguration>      <EnvironmentVariables>          <!-- List of environment variables we want to set-->          <ASP_Test_Mode>Development</ASP_Test_Mode>      </EnvironmentVariables>  </RunConfiguration></RunSettings>  
```  
  
### ASP_Test_Mode  
| value |function|  
|-|-|  
|Development|tests are run using an in-memory database (fast running to enable development with quick feedback)|  
|Integration|tests are run using a real database (slow but exercises the real database connection code)|  
  

### Test Database and Storage Configuration

Test configurations are managed through multiple settings files:
*   `appsettings.api.test.json` and `appsettings.api.test.secrets.json` for API tests
*   `appsettings.web.test.json` and `appsettings.web.test.secrets.json` for Web tests

#### In-Memory vs Real Storage

Both API and Web configurations support in-memory and real storage options for:
*   Document Database (Cosmos DB)
*   Blob Storage
*   Table Storage (Web only)
By default, all storage types are set to use in-memory implementations:
```  
    "DocumentDatabase": {
        "InMemory": true,
        "EndpointUri": null,
        "PrimaryKey": null,
        "DatabaseId": "test"
    }
```  

#### Using Real Storage Services

To use real storage services instead of in-memory implementations:
1.  Create a secrets file from the template:
    *   For API: Copy `appsettings.api.test.secrets.template.json` to `appsettings.api.test.secrets.json`
    *   For Web: Copy `appsettings.web.test.secrets.template.json` to `appsettings.web.test.secrets.json`
2.  Set the appropriate `InMemory` flag to `false` and provide the required credentials:
    
```  
    "DocumentDatabase": {
        "InMemory": false,
        "EndpointUri": "<Your Test Cosmos DB Endpoint>",
        "PrimaryKey": "<Your Test Cosmos DB Key>"
    }
``` 

#### Test Database Considerations

*   The database ID defaults to `"test"` which should be dedicated to integration tests
*   Integration tests can be run during development, but care should be taken when multiple test runs occur simultaneously
*   To avoid conflicts between developers, each developer should consider using their own test database instance
*   For CI/CD pipelines, it's recommended to create and destroy a unique test database for each pipeline run to prevent conflicts between simultaneous builds

#### Additional Web Test Settings

The web test configuration includes additional settings:
*   `Api.InProcess`: Controls whether the API runs in-process
*   `DsiOidc.Enabled`: Controls OIDC authentication
*   `ErrorHandling.ForceProductionErrorPage`: Controls error page behavior
Remember to never commit the actual secrets files (`*.secrets.json`) to source control. These should be managed securely and locally per environment.
 

### Running Local API

#### Configuration Files Setup

1.  **Local Settings** Create a `local.settings.json` file with:
    ``` 
        {
            "IsEncrypted": false,
            "Values": {
                "ASPNETCORE_ENVIRONMENT": "Local",
                "AzureWebJobsStorage": "UseDevelopmentStorage=true",
                "FUNCTIONS_WORKER_RUNTIME": "dotnet-isolated"
            }
        }
    ```      
    
2.  **API Secrets**
    *   Copy `appsettings.api.secrets.template.json` to `appsettings.api.secrets.json`
    *   Update with your development environment credentials:
    ``` 
        {
            "BlobStorage": {
                "InMemory": false,
                "StorageAccountName": "s192d01strdev",
                "PrimaryKey": "<Your Blob Storage Primary Key>"
            },
            "DocumentDatabase": {
                "InMemory": false,
                "EndpointUri": "https://s192d01-cdb-dev.documents.azure.com:443/",
                "PrimaryKey": "<Your Cosmos DB Primary Key>"
            }
        }
        
    ``` 

#### Configuration Behavior

The project is configured to handle configuration files as follows:
*   `appsettings.api.json`: Always copied to output and publish directories
*   `appsettings.api.secrets.json`:
    *   Copied to output directory if it exists
    *   Never copied to publish directory
*   `appsettings.api.secrets.template.json`: Never copied to output or publish
*   `local.settings.json`:
    *   Copied to output directory
    *   Never copied to publish directory

#### Document Database Configuration

The API uses Cosmos DB with the following container structure:
  ``` 
    "Containers": {
        "content": {
            "ContainerName": "content",
            "PartitionKey": "/contentId"
        },
        "establishments": {
            "ContainerName": "establishments",
            "PartitionKey": "/urn"
        },
        "local-authorities": {
            "ContainerName": "local-authorities",
            "PartitionKey": "/code"
        },
        "multi-academy-trusts": {
            "ContainerName": "multi-academy-trusts",
            "PartitionKey": "/id"
        }
    }
 ```    

#### Configuration Sources

Settings are populated from different sources based on the environment:
*   **Local Development**:
    *   Settings marked as "local only" come from `appsettings.api.secrets.json`
    *   Required for local development
*   **Deployment**:
    *   Settings marked as "deployed only" come from pipeline variable groups (s192d01-group and s192t01-group)
    *   Not needed for local development
*   **Both**:
    *   Settings marked as "local, deployed" come from both sources

#### Additional Notes

*   The default database ID is set to "asp"
*   Storage services (Blob and Document DB) use real instances by default (`"InMemory": false`)
*   Error stack traces are disabled by default
*   When running locally, no "x-functions-key" header is required as the Authorization level is 'Anonymous'
#### Remember:
*   Never commit `appsettings.api.secrets.json` to source control
*   Keep your local settings secure and separate from the template files
*   The application uses Azure Key Vault in deployed environments, but this is not required for local development
  
  
## Troubleshooting Local API Project Setup  
If you encounter a "There is no functions runtime available that matches the version specified" error while attempting to run the API project locally, follow these steps to resolve the issue:  
  
- Navigate to Tools -> Options -> Projects & Solutions -> Azure Functions in Visual Studio.  
- Click on the "Check for updates" button.  
- Allow Visual Studio some time to update. Note that the update process may take a while, and Visual Studio may crash during this time.  
- Once the update is complete, attempt to run the API project again.


* * *

## Running the ASP.Web Project Locally
-----------------------------------

The **ASP.Web** project is configured to connect to the **ASP.Api** project when running locally. Depending on the `InProcess` value in the configuration, you may need to run both projects locally for the web application to function correctly.

* * *

### Prerequisites

1.  **ASP.Api and ASP.Web Projects**  
    Whether both projects need to be running locally depends on the `InProcess` value in the `Api` section of the configuration:
    *   **`InProcess: true`**: The **ASP.Web** project uses an in-process transport layer to communicate directly with the API function objects in memory. In this case, only the **ASP.Web** project needs to be running.
    *   **`InProcess: false`**: The **ASP.Web** project uses an HTTP transport layer to communicate with the **ASP.Api** project over HTTP. In this case, both **ASP.Api** and **ASP.Web** need to be running locally.
    By default, the `InProcess` value is set to `false` for local development, which means **both the ASP.Api and ASP.Web projects need to be running locally** for the web application to function correctly.
    
2.  **Local Configuration**  
    Ensure the `appsettings.web.secrets.json` file is created and populated with the required local configuration values. Use the `appsettings.web.secrets.template.json` file as a reference.
    
3.  **Startup Projects**  
    Configure the solution to run both **ASP.Api** and **ASP.Web** as startup projects if `InProcess` is set to `false`.
    

* * *

### Configuration

#### appsettings.web.json

The `appsettings.web.json` file contains the default configuration for the **ASP.Web** project. For local development, some values are overridden by `appsettings.web.secrets.json`.
Key configuration values include:
*   **API Endpoint**: The `EndpointBaseUrl` in the `Api` section points to the local **ASP.Api** project (`http://localhost:7116`).
*   **InProcess**: Determines whether the **ASP.Web** project communicates with the API in-process or over HTTP.
*   **Blob Storage**: Configured to use local or development storage.
*   **Document Database**: Points to the Cosmos DB Emulator or a development Cosmos DB instance.
*   **DSI OIDC**: Configures authentication for the web application.

#### appsettings.web.secrets.json

Create a `appsettings.web.secrets.json` file in the root of the **ASP.Web** project. This file should contain sensitive information and local overrides. Use the `appsettings.web.secrets.template.json` file as a reference.
Example `appsettings.web.secrets.json`:

    {
      "Api": {
        "InProcess": false,
        "EndpointBaseUrl": "http://localhost:7116"
      },
      "BlobStorage": {
        "InMemory": false,
        "StorageAccountName": "s192d01strdev",
        "PrimaryKey": "<Replace with Blob Storage Primary Key for s192d01strdev>"
      },
      "DocumentDatabase": {
        "InMemory": false,
        "EndpointUri": "https://localhost:8081/",
        "PrimaryKey": "<Replace with Cosmos DB Emulator Primary Key>"
      },
      "DsiOidc": {
        "Audience": "ASP2",
        "ClientId": "ASP2",
        "Issuer": "https://test-oidc.signin.education.gov.uk:443",
        "MetadataAddress": "https://test-oidc.signin.education.gov.uk/.well-known/openid-configuration",
        "ProfileUrl": "https://test-profile.signin.education.gov.uk/",
        "RedirectUrlAfterSignout": "https://test-services.signin.education.gov.uk/my-services",
        "ServiceId": "A66FF5F3-2D89-4698-99EC-61AB3F51F31A",
        "ClientSecret": "<Replace with DSI OIDC client secret for ASP2>"
      },
      "DsiPublicApi": {
        "AuthorizationUrl": "https://test-api.signin.education.gov.uk/",
        "ClientId": "ASP2",
        "ClientSecret": "<Replace with DSI Public API client secret for ASP2>"
      }
    }
    

* * *

### Setting Up Multiple Startup Projects

To run both **ASP.Api** and **ASP.Web** locally (when `InProcess` is set to `false`):
1.  **Open Solution Properties**
    *   Right-click on the solution in Visual Studio and select **Properties**.
2.  **Set Multiple Startup Projects**
    *   In the **Startup Project** section, select **Multiple startup projects**.
    *   Set the **Action** for both **ASP.Api** and **ASP.Web** to **Start**.
3.  **Save and Run**
    *   Save the changes and press **F5** or click the **Start** button to run both projects.

Configure the startup projects in the solution properties to be multiple startup projects as below:

![web-running-locally-image.png](/docs/.attachments/web-running-locally-image-d43b878f-f720-457a-a8bc-ef0cf1d5e3fd.png)

* * *

### How the ASP.Web Project Connects to ASP.Api

The **ASP.Web** project connects to the **ASP.Api** project using the `EndpointBaseUrl` specified in the `appsettings.web.secrets.json` file. By default, this is set to `http://localhost:7116`, which is the port the **ASP.Api** project runs on locally.

#### InProcess Configuration and Transport Layer

The `InProcess` value in the `Api` section of the configuration determines the transport layer used by the **ASP.Web** project to communicate with the **ASP.Api** project:
*   **`InProcess: true`**  
    If `InProcess` is set to `true`, the **ASP.Web** project uses an **in-process transport layer** to communicate directly with the API function objects in memory. This bypasses the need for an HTTP connection, and only the **ASP.Web** project needs to be running.
    
*   **`InProcess: false`**  
    If `InProcess` is set to `false`, the **ASP.Web** project uses an **HTTP transport layer** to communicate with the API over HTTP. In this case, both **ASP.Api** and **ASP.Web** need to be running locally.
   
* * *

### Example Workflow for Running Locally

1.  **Start the Cosmos DB Emulator**  
    If the **ASP.Api** project uses Cosmos DB, ensure the Cosmos DB Emulator is running locally. Use the default connection string:
    
        AccountEndpoint=https://localhost:8081/;AccountKey=C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==
        
    
2.  **Run Both Projects (if `InProcess` is false)**  
    Start both **ASP.Api** and **ASP.Web** using the multiple startup projects configuration.
    
3.  **Access the Web Application**  
    Open a browser and navigate to the **ASP.Web** project’s URL:
    
        https://localhost:7155
        
    
4.  **Test API Integration**  
    Verify that the **ASP.Web** project can successfully communicate with the **ASP.Api** project. For example:
    *   Perform a search or other operation in the web application.
    *   Check the logs to ensure the API is being called correctly.

* * *

### Troubleshooting

1.  **API Connection Issues**
    *   Ensure the `EndpointBaseUrl` in `appsettings.web.secrets.json` matches the URL of the running **ASP.Api** project.
    *   Verify that the **ASP.Api** project is running and accessible.
2.  **Cosmos DB Emulator Issues**
    *   Ensure the emulator is running and the connection string in `appsettings.web.secrets.json` is correct.
    *   Check the emulator logs for errors.
3.  **Transport Layer Issues**
    *   If `InProcess` is set to `true`, ensure the in-process API configuration is correctly set up.
    *   If `InProcess` is set to `false`, verify the HTTP transport layer is configured correctly.

* * *

### Summary

To run the **ASP.Web** project locally:
1.  Configure `appsettings.web.secrets.json` with the required local values.
2.  Depending on the `InProcess` value:
    *   If `InProcess` is `true`, only the **ASP.Web** project needs to be running.
    *   If `InProcess` is `false` (default), both **ASP.Api** and **ASP.Web** need to be running locally.
3.  Start the Cosmos DB Emulator if required.
4.  Run the solution and verify that the web application connects to the API successfully.
By following these steps, you can develop and test the **ASP.Web** project locally with full integration with the **ASP.Api** project.



## CosmosDbSeeder with the Cosmos DB Emulator and Blob Storage for Local Development
This project provides a robust solution for extracting data from a source Cosmos DB, processing it, and seeding it into a target Cosmos DB. Additionally, it supports uploading a specific configuration file (`downloads-config.json`) to Azure Blob Storage.

### Setting Up Cosmos DB Emulator Configuration

The Cosmos DB Emulator provides a local environment that emulates the Azure Cosmos DB service, allowing you to develop and test without connecting to a live database.

#### Prerequisites

1.  **Install the Cosmos DB Emulator**  
    Download and install the Azure Cosmos DB Emulator from the [official documentation](https://learn.microsoft.com/en-us/azure/cosmos-db/emulator).
    
2.  **Start the Emulator**  
    Once installed, start the emulator. By default, it runs on `https://localhost:8081`.
    
3.  **Verify the Emulator is Running**  
    Open a browser and navigate to `https://localhost:8081/_explorer/index.html`. This will open the Cosmos DB Emulator Data Explorer, where you can view and manage your local Cosmos DB instance.
    
4.  **Default Emulator Connection String**  
    Use the following connection string to connect to the emulator:
    
        AccountEndpoint=https://localhost:8081/;AccountKey=C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==
        
    

#### Configuring the Emulator in `appsettings.secrets.json`

To use the emulator for local development, update the `TargetDatabase.ConnectionString` in your `appsettings.secrets.json` file as follows:

    {
      "CosmosDb": {
        "SourceDatabase": {
          "ConnectionString": "AccountEndpoint=https://<SOURCE_ACCOUNT>.documents.azure.com:443/;AccountKey=<SOURCE_KEY>;"
        },
        "TargetDatabase": {
          "ConnectionString": "AccountEndpoint=https://localhost:8081/;AccountKey=C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw=="
        }
      }
    }
    

* * *

### Data Storage Configuration for Cosmos DB

The application stores extracted data in a configurable location before seeding it into the target Cosmos DB. The data is organized into folders corresponding to Cosmos DB containers, with each folder containing JSON files for the documents in that container.

#### Updated Data Folder Structure

The data folder structure is as follows:

    data/
    ├── content/
    │   ├── content.json
    ├── establishments/
    │   ├── establishments.json
    │   ├── synthetic-establishments.json
    ├── local-authorities/
    │   ├── local-authorities.json
    ├── multi-academy-trusts/
    │   ├── multi-academy-trusts.json
    

#### Explanation of the Structure

*   **`data/`**: The root folder for all extracted data.
*   **`content/`**: Contains the `content.json` file, which stores documents from the `content` container.
*   **`establishments/`**: Contains:
    *   `establishments.json`: Stores documents from the `establishments` container.
    *   `synthetic-establishments.json`: Stores synthetic or test data for the `establishments` container.
*   **`local-authorities/`**: Contains the `local-authorities.json` file, which stores documents from the `local-authorities` container.
*   **`multi-academy-trusts/`**: Contains the `multi-academy-trusts.json` file, which stores documents from the `multi-academy-trusts` container.

#### Configuring the Data Path

You can specify the data folder path in the configuration file. The path can be either absolute or relative to the solution directory.
1.  **Absolute Path**:
    
        {
          "Container": {
            "DataPath": "C:\\MyApp\\data"
          }
        }
        
    
2.  **Relative Path**:
    
        {
          "Container": {
            "DataPath": "data"
          }
        }
        
    

#### Important Notes

*   If you provide a relative path, it will be resolved against the solution directory where the `.sln` file is located.
*   Ensure that the specified data folder exists. The application will throw an exception if the folder cannot be found.
*   Each JSON file represents a collection of documents extracted from the corresponding Cosmos DB container.

* * *

### Blob Storage Functionality

The project includes functionality to upload a single configuration file (`downloads-config.json`) to the `config` container in Azure Blob Storage.

#### Key Features

1.  **Upload Specific File**  
    The `BlobStorageService` uploads only the `downloads-config.json` file to the `config` container in Blob Storage.
    
2.  **Automatic Container Creation**  
    If the `config` container does not exist in Blob Storage, it will be created automatically.
    
3.  **Detailed Logging**  
    Logs are provided for each stage of the upload process, including successful uploads and errors.
    

#### Configuration

The Blob Storage functionality is controlled via the `BlobStorage` section in the configuration files:
*   **appsettings.json**:
    
        "BlobStorage": {
          "Enabled": false
        }
        
    
*   **appsettings.secrets.json** (for local development):
    
        {
          "BlobStorage": {
            "ConnectionString": "DefaultEndpointsProtocol=https;AccountName=<ACCOUNT_NAME>;AccountKey=<ACCOUNT_KEY>;EndpointSuffix=core.windows.net",
            "Enabled": true
          }
        }
        
    

* * *

### Updated Configuration Files

#### appsettings.json

    {
      "CosmosDb": {
        "SourceDatabase": {
          "ConnectionString": null, // Override in appsettings.secrets.json
          "DatabaseId": "asp",
          "ConnectionMode": "Gateway",
          "ConsistencyLevel": "Session"
        },
        "TargetDatabase": {
          "ConnectionString": null, // Override in appsettings.secrets.json
          "DatabaseId": "asp",
          "ConnectionMode": "Gateway",
          "ConsistencyLevel": "Session"
        }
      },
      "Extraction": {
        "Enabled": false,
        "CleanupBeforeExtract": false,
        "ExcludeProperties": [
          "_rid",
          "_self",
          "_etag",
          "_attachments",
          "_ts"
        ]
      },
      "BlobStorage": {
        "Enabled": false
      },
      "Logging": {
        "LogLevel": {
          "Default": "Information",
          "Microsoft": "Warning",
          "System": "Warning"
        }
      }
    }
    

#### appsettings.secrets.template.json

    {
      "CosmosDb": {
        "SourceDatabase": {
          "ConnectionString": "<YOUR_CONNECTION_STRING>"
        },
        "TargetDatabase": {
          "ConnectionString": "<YOUR_CONNECTION_STRING>"
        }
      },
      "BlobStorage": {
        "ConnectionString": "<YOUR_CONNECTION_STRING>"
      }
    }
    

* * *

### Running the Application

The application can be run in two ways:

#### 1. From Visual Studio

*   Open the solution in Visual Studio.
*   Set the startup project to the `ASP.Infrastructure.Azure.CosmosDbSeeder` project.
*   Press **F5** or click the "Start" button.
*   The console window will display progress and any errors.

#### 2. From Command Line

Navigate to the project directory (`ASP.Infrastructure.Azure.CosmosDbSeeder`) and run:

    dotnet run
    

* * *

### Important Notes

*   **Blob Storage Connection String**: Ensure the connection string in `appsettings.secrets.json` is valid and points to your Blob Storage account.
*   **Error Handling**: If any errors occur during the upload process or data extraction, they will be logged, and the application will stop processing.
*   **File Location**: The `downloads-config.json` file must exist in the `blobs/config` directory relative to the solution root.
*   **Data Folder**: Ensure the data folder specified in the configuration exists before running the application.
By following these steps, you can set up and use the Cosmos DB Emulator, Blob Storage functionality, and data storage configuration for local development, ensuring a smooth and isolated testing environment.