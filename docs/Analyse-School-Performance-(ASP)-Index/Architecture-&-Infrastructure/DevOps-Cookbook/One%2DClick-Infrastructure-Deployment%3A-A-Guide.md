## Recovering Azure Environments

This document outlines the procedures for recovering Azure environments after accidental destruction or deletion.

**Table of Contents**

1. Recovery Phases
2. Infrastructure Recovery
3. Application Deployment
4. Final Configuration

**1. Recovery Phases**

There are two primary methods for recovering an Azure environment:

* **Manual Recovery:** Re-creating the environment manually using the same steps followed during the initial setup.
* **Automated Recovery:** Re-running the existing infrastructure pipeline to provision the required resources.

**2. Infrastructure Recovery**

* **Automated Recovery (Recommended):**
    * Execute the Azure infrastructure pipeline: [Pipelines - Runs for s192-Infrastructure-MVP-D01](https://dfe-ssp.visualstudio.com/s192-Analyse-School-Performance%20(ASP)/_build?definitionId=2735). 
    * This pipeline, based on ARM templates, will provision the necessary resources in Azure under the specified development or test environment.

**3. Application Deployment**

* Once the infrastructure is provisioned, deploy the application code to the newly created resources. 
* Execute the application deployment pipeline: [Pipelines - Runs for S192-ASP-Deploy-Application](https://dfe-ssp.visualstudio.com/s192-Analyse-School-Performance%20(ASP)/_build?definitionId=2752).

**4. Final Configuration**

* Perform any necessary final configuration tasks, such as:
    * Addressing any other pending configuration which developer needed.
    *  Updating the azurewebjob connection string to activate the function app key
    * A CIP request is required to grant IAM roles for the Managed Identity to access both the Storage Account and Cosmos DB.

     

**Note:**

* This document provides a general outline. Specific steps and procedures may vary depending on the complexity of the environment and the nature of the destruction or deletion.
* Regularly review and update the infrastructure and application pipelines to ensure they accurately reflect the current environment configuration and deployment requirements.
* Consider implementing additional recovery mechanisms, such as backups and snapshots, to minimize downtime and facilitate faster recovery.