# Infrastructure CI/CD


- The pipeline is designed to deploy ARM templates using YAML pipelines, with dedicated stages for development and testing.
- In the event of an environmental failure or collapse, the pipelines can be easily retriggered to restore the environment.

 Pipeline Link:  [Pipelines - Runs for s192-Infrastructure-MVP-D01](https://dfe-ssp.visualstudio.com/s192-Analyse-School-Performance%20(ASP)/_build?definitionId=2735)


# Application CI/CD

-  This pipeline is designed to deploy application code to Function Apps and Azure App Services after successful compilation and testing. Deployments follow a linear progression: first to the development environment, then to the test environment.
-  This pipeline is specifically designed to run after the infrastructure pipeline, ensuring that all necessary resources are provisioned before application deployment.

 Pipeline Link: [Pipelines - Runs for S192-ASP-Deploy-Application](https://dfe-ssp.visualstudio.com/s192-Analyse-School-Performance%20(ASP)/_build?definitionId=2752)

#  Sandbox POC Pipeline 
**Note: Not in use**
Infrastructure Pipeline Link:  [Pipelines - Runs for s192-ASP-Infrastructure-POC-D02](https://dfe-ssp.visualstudio.com/s192-Analyse-School-Performance%20(ASP)/_build?definitionId=2515)
Application Pipeline Link:  [Pipelines - Runs for s192-ASP-frontend-application-POC-D02](https://dfe-ssp.visualstudio.com/s192-Analyse-School-Performance%20(ASP)/_build?definitionId=2601)
