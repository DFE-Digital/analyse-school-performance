
| **Topic** | **Current Setup (T1)** | **CIP Setup** |
|--|--|--|
| Virtual Network | Shared Virtual Network​ This restricts visibility and limits design choices. |  Isolated Virtual Network ​This allows for greater options (i.e., Private Endpoints, Application Gateway) and visibility|
| Access and Permissions | Direct access is managed by DfE Cyber Security using Azure Active Directory groups |Access is managed by the service team using Azure Active Directory and Privileged Identity Management  |
|Infrastructure as Code  | Currently, no Infrastructure as Code is being used for resources within the Portal. |Infrastructure as Code will be used for all deployments.  |
| Deployment of Resources |Due to restricted access, requests will need to be raised with other teams (Infrastructure Operations, DfE Cyber Security) to deploy resources.   |Access is less restricted, allowing for the deploying and amendment of resources. Some Networking and Microsoft Entra ID changes will still need to be raised with other teams.  |

 