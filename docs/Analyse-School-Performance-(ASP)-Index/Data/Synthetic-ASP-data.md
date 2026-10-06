The following datasets have been set up for generating synthetic data:-
- MTC
- QLA
- Phonics

Each of the data sets has a dataspec that can be updated and changed to generated the test data output.  The dataspecs can be found here:- [data - Microsoft Azure](https://portal.azure.com/#view/Microsoft_Azure_Storage/ContainerMenuBlade/~/overview/storageAccountId/%2Fsubscriptions%2F212193cd-152c-4621-97d1-85cb63f025b4%2FresourceGroups%2Fs192d01-dev%2Fproviders%2FMicrosoft.Storage%2FstorageAccounts%2Fs192d01stradfdev/path/data/etag/%220x8DCAA4B768C3688%22/defaultEncryptionScope/%24account-encryption-key/denyEncryptionScopeOverride~/false/defaultId//publicAccessVal/None)  

![image.png](/docs/.attachments/image-f56dce17-1911-44aa-affd-944254e9cabe.png)

The **data_type_lookup.xlsx** file is used during the synthetic data generation to easily switch between the different data types.  Any new data specs created will need to also be added to this document

The **synthetic-urn-lookup.xlsx** is used the dev environment to map the live URNs to the dummy URNs created by the GIAS team for the front to log in with. This is used during the establishment data import process to restrict the establishment's container in dev to only those we need.

![image.png](/docs/.attachments/image-63ed0247-5541-4769-a90f-72eceb8ca3f6.png)

## Databricks
Before you can run any code you will need to have access to the dev databricks on ADA resource **s101d01-dbr-edap-02**.

To generate the data open this notebook - [synthetic_data - Databricks](https://adb-2220072380334347.7.azuredatabricks.net/editor/notebooks/573295273506505?o=2220072380334347#command/573295273506512)

At the top there are 2 drop down menus for selecting the data type and the number of dummy rows of data you require. The data will be output to the following folder

![image.png](/docs/.attachments/image-8f3b7db4-82c7-4660-8717-0989c9e0bbb9.png)