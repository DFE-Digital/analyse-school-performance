# Infrastructure Repository

This repository contains the infrastructure as code for our ASP environment. 
The ARM templates have been designed to be reusable across all environments. All templates are parameterized, and values are dynamically passed from pipelines to enhance reusability.
**Key improvements:**
*   **Clarity:** Rephrased for better flow and conciseness.
*   **Grammar:** Corrected grammatical errors.
*   **Conciseness:** Removed redundant phrases.
*   **Professionalism:** Maintained a professional tone.

**Repository Link:** [s192-Analyse-School-Performance (ASP) - Repos](https://dfe-ssp.visualstudio.com/_git/s192-Analyse-School-Performance%20(ASP))

## Table of Contents

* [1. ARM Templates](#1-arm-templates)
    * [1.1. App Service](#1.1-app-service)
    * [1.2. App Service Plan](#1.2-app-service-plan)
    * [1.3. Cosmos DB](#1.3-cosmos-db)
    * [1.4. Data Factory](#1.4-data-factory)
    * [1.5. Function App](#1.5-function-app)
    * [1.6. Key Vault](#1.6-key-vault)
    * [1.7. Storage Account](#1.7-storage-account)
* [2. Azure DevOps Pipelines](#2-azure-devops-pipelines)
    * [2.1. YAML Pipelines](#2.1-yaml-pipelines) 
        * [2.1.1. Sandbox Environment](#2.1.1-sandbox-environment)
        * [2.1.2. Dev Environment](#2.1.2-dev-environment)
        * [2.1.3. Staging Environment](#2.1.3-staging-environment)
    * [2.2. Templating](#2.2-templating)
    * [2.3. Variables](#2.3-variables) 

## 1. ARM Templates

This section outlines the ARM templates used to provision the infrastructure for our ASP.NET environment.

### 1.1. App Service
* [AppService - Repos](https://dfe-ssp.visualstudio.com/_git/s192-Analyse-School-Performance%20(ASP)?path=/templates/AppService)

### 1.2. App Service Plan
* [AppServicePlan - Repos](https://dfe-ssp.visualstudio.com/_git/s192-Analyse-School-Performance%20(ASP)?path=/templates/AppServicePlan)

### 1.3. Cosmos DB
* [CosmosDB - Repos](https://dfe-ssp.visualstudio.com/_git/s192-Analyse-School-Performance%20(ASP)?path=/templates/CosmosDB)

### 1.4. Data Factory
* [DataFactory - Repos](https://dfe-ssp.visualstudio.com/_git/s192-Analyse-School-Performance%20(ASP)?path=/templates/DataFactory)

### 1.5. Function App
* [FunctionApp - Repos](https://dfe-ssp.visualstudio.com/_git/s192-Analyse-School-Performance%20(ASP)?path=/templates/FunctionApp)

### 1.6. Key Vault
* [KeyVault - Repos](https://dfe-ssp.visualstudio.com/_git/s192-Analyse-School-Performance%20(ASP)?path=/templates/KeyVault)

### 1.7. Storage Account
* [StorageAccount - Repos](https://dfe-ssp.visualstudio.com/_git/s192-Analyse-School-Performance%20(ASP)?path=/templates/StorageAccount)

## 2. Azure DevOps Pipelines

This section details the Azure DevOps pipelines used to deploy the infrastructure.
[azure-pipeline-1.yml - Repos](https://dfe-ssp.visualstudio.com/_git/s192-Analyse-School-Performance%20(ASP)?path=/azure-pipeline-1.yml)

### 2.1. YAML Pipelines

* **2.1.1. Sandbox Environment** 
* **2.1.2. Dev Environment** 
* **2.1.3. Staging Environment** 

### 2.2. Templating

* [deployment-stage1.yml - Repos](https://dfe-ssp.visualstudio.com/_git/s192-Analyse-School-Performance%20(ASP)?path=/pipelines/deployment-stage1.yml)

### 2.3. Variables

* [variables - Repos](https://dfe-ssp.visualstudio.com/_git/s192-Analyse-School-Performance%20(ASP)?path=/pipelines/variables)