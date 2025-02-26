# Cosmos DB Data Extraction and Seeding

This project provides a robust solution for extracting data from a source Cosmos DB, processing it, and seeding it into a target Cosmos DB. The process is fully configurable via `appsettings.json` and includes features such as data cleanup, property exclusion, and detailed logging.

## Features

- **Data Extraction**: Extracts data from source Cosmos DB containers based on configurable IDs or queries.
- **Data Seeding**: Seeds data into target Cosmos DB containers after extraction.
- **Configurable Cleanup**: Cleans up data folders before extraction if enabled in the configuration.
- **Property Exclusion**: Excludes specific properties (e.g., `_rid`, `_etag`) from extracted documents.
- **Detailed Logging**: Provides comprehensive logs for all stages of the process, including cleanup, extraction, and seeding.

---

## Configuration

The application is configured using `appsettings.json`. Below is an example configuration:

```json
{
  "CosmosDb": {
    "SourceDatabase": {
      "ConnectionString": "<YOUR_CONNECTION_STRING>",
      "DatabaseId": "asp",
      "ConnectionMode": "Gateway",
      "ConsistencyLevel": "Session"
    },
    "TargetDatabase": {
      "ConnectionString": "<YOUR_CONNECTION_STRING>",
      "DatabaseId": "asp_test",
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
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft": "Warning",
      "System": "Warning"
    }
  }
}
```
## Using appsettings.local.json 

For security and flexibility, do not hardcode sensitive information like connection strings in appsettings.json. Instead, create a file named appsettings.local.json in the root of the  project and specify the connection strings there.

### Example appsettings.local.json:

```json
{
  "CosmosDb": {
    "SourceDatabase": {
      "ConnectionString": "AccountEndpoint=https://<SOURCE_ACCOUNT>.documents.azure.com:443/;AccountKey=<SOURCE_KEY>;"
    },
    "TargetDatabase": {
      "ConnectionString": "AccountEndpoint=https://<TARGET_ACCOUNT>.documents.azure.com:443/;AccountKey=<TARGET_KEY>;"
    }
  }
}
```

## Data Storage Configuration

The application stores data files in a configurable location. You can specify the data folder path in two ways:

1. **Absolute Path**: Provide a full path to the desired location
   ```json
   {
     "Container": {
       "DataPath": "C:\\MyApp\\data\\container-data"
     }
   }
   ```
2. **Relative Path**: Provide a path relative to the solution directory.
   ```json
     {
       "Container": {
         "DataPath": "data\\container-data"
       }
     }
   ```
### Important Notes
 - If you provide a relative path, it will be resolved against the solution directory where the .sln file is located.
 - Ensure that the specified data folder exists.
 - The application will throw an exception if the configuration file or data folder cannot be found. 

## Running the Application

The application can be run in two ways:

### 1. From Visual Studio
- Open the solution in Visual Studio
- Set the startup project to the 'ASP.Infrastructure.Azure.CosmosDbSeeder' project
- Press F5 or click the "Start" button
- The console window will display progress and any errors

### 2. From Command Line
Navigate to the project directory (ASP.Infrastructure.Azure.CosmosDbSeeder) and run:
```bash
dotnet run
```