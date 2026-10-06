When an environment (e.g. dev or test environment) is brought online for the first time, a developer with access to the Azure Portal and the appropriate subscription (`s192-analyse-school-performance-development` or `s192-analyse-school-performance-test`) will need to manually set up a few things.

**Note:** the final stage of this process requires raising CIP requests to grant access from the Managed Identity to the storage resources (see the **Managed Identity** section below). Please be aware this process may take several days, so remember to factor this in to the time needed to get the environment fully up and running.

# Function app AzureWebJobsStorage setting
Before the function app deployment pipeline can be run, and before the functions key can be retrieved, there is one app setting in the function app that must be manually updated: `AzureWebJobsStorage`. 

The value for this setting is the connection string for the storage account found in Azure Portal: Storage Account `s192d01strdev` (for the dev environment) or `s192t01strtest` (for the test environment) > Security + networking > Access keys > key1 > Connection string

This needs to be set in Azure Portal: Function app `s192d01-func-dev-01` (for the dev environment) or `s192t01-func-test-01` (for the test environment) > Settings > Environment variables > App settings > `AzureWebJobsStorage`.

Once this is set the deployment pipeline should run successfully and the Functions > App keys section of the function app will function correctly.

# Variable groups
Next in order for the web application service and the API function app to be configured correctly, the application settings must be updated. The best way to do this is to populate the appropriate variable group to be used by the deployment pipeline, as then whenever the web application or the API are deployed, the app settings for each will be overwritten with the correct values from the variable group.

To find the appropriate variable group to update go to Pipelines > Library in ADO:

![image.png](/docs/.attachments/image-add1e3ff-4dee-4c2c-bf08-1998e1d0cdcc.png)

Choose the appropriate variable group (either `s192d01-group` or `s192t01-group`, the `-data` groups are for the data pipelines):

![image.png](/docs/.attachments/image-b2f2f100-5fea-46fc-bc4e-39743cd25326.png)

The keys that will need updating are as follows:

![image.png](/docs/.attachments/image-3baa726e-bb75-4fa1-8f26-e3108085b4f3.png)
![image.png](/docs/.attachments/image-3e8ad8cb-53a9-4ea8-bd7d-4c6c0115b678.png)

|Key|What it's for|Where to find it|
|-|-|-|
|Api:FunctionsKey|For the web app to connect to the API function app via HTTP|In Azure Portal, either the function app `s192d01-func-dev-01` (if setting up the dev environment) or `s192t01-func-test-01` (if setting up the test environment), then Functions > App keys > Host keys (all functions) > default
|APPINSIGHTS_INSTRUMENTATIONKEY|For the web app and API function app to be able to log to Application Insights|In Azure Portal, Application Insights instance `s192d01-appi-dev-01` or `s192t01-appi-test-01` > Overview > Instrumentation Key| 
|APPLICATIONINSIGHTS_CONNECTION_STRING|For the web app and API function app to be able to log to Application Insights|In Azure Portal, Application Insights instance `s192d01-appi-dev-01` or `s192t01-appi-test-01` > Overview > Connection String|
|AzureWebJobsStorage|Connection string for blob storage used by the API function app for normal operations ([more information](https://learn.microsoft.com/en-us/azure/azure-functions/functions-app-settings#azurewebjobsstorage))|In Azure Portal, Storage Account `s192d01strdev` or `s192t01strtest` > Security + networking > Access keys > key1 > Connection string|
|BlobStorage:ManagedIdentityClientId|Client ID of the Managed Identity. Used by the API function app (and also by the web application if using the in-process API)|In Azure Portal, Managed Identity `s192d01-uami-01` or `s192t01-uami-01` > Overview > Client ID|
|DocumentDatabase:ManagedIdentityClientId|Client ID of the Managed Identity. Used by the API function app (and also by the web application if using the in-process API)|In Azure Portal, Managed Identity `s192d01-uami-01` or `s192t01-uami-01` > Overview > Client ID|
|TableStorage:ManagedIdentityClientId|Client ID of the Managed Identity. Used by the web application only to log errors|In Azure Portal, Managed Identity `s192d01-uami-01` or `s192t01-uami-01` > Overview > Client ID|

# (Key Vault) - remove?
Certain secrets need to be added to the Key Vault in Azure Portal, Key vault `s192d01-kv-dev` or `s192t01-kv-test` > Objects > Secrets

You may see a message saying you are unauthorized to view these contents:
![==image_0==.jpeg](/docs/.attachments/==image_0==-3e94d3c9-01cd-4379-b2c1-c123f4272011.jpeg) 

In that case you will need to go to Access policies > `+ Create` and do the following:
1. On the Permissions tab, select Configure from a template > `Key & Secret Management`
2. On the Principal tab, enter your email address
3. On the Application (optional) tab, leave empty
4. On the Review + Create tab click `Create`

You should now be able to view and edit Keys and Secrets.

Back in Objects > Secrets click `+ Generate/Import` and create the following secrets:

| Name | Value | Notes |
|-|-|-|
|s192-asp-cosmos-key|

# Managed Identity
The final process needed in order to get the environment in a functioning state is for the Managed Identity to be given access to the resources needed to run the web app and the function app: Azure Cosmos DB and Azure Blob Storage. 

Unfortunately this process requires manually raising a CIP request and may take several days to be executed. More information about what specific requests are needed is available in the **infrastructure documentation [here]**