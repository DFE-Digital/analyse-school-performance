### Setting Up Local Development

1. Create a `local.settings.json` file in the root of the project.
2. Add the following structure to the file:

```json
{
  "IsEncrypted": false,
  "Values": {
    "AzureWebJobsStorage": "UseDevelopmentStorage=true",
    "DocumentDatabase:PrimaryKey": "<Replace with your Cosmos DB Primary Key>",
    "BlobStorage:PrimaryKey": "<Replace with your Blob Storage Primary Key>"
  }
}