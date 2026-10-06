#Setting up a new environment
There are several steps involved in the creation of an environment and each step will be the responsibility of a different profession

##Infrastructure engineering responsibilities

- Update and deploy IaC (Infrastructure as Code) to create new environment resources on Azure
- Update ASP2.0 [code deployment pipeline](https://dev.azure.com/dfe-ssp/s192-Analyse-School-Performance%20(ASP)/_git/asp?path=/deploy-pipeline.yml)




##Software engineering responsibilities
- Setup DSI for new environment
--DSI environments are listed [here]([Environments](/Analyse-School-Performance-\(ASP\)-Index/Architecture-&-Infrastructure/Environments))
--Further details on what's required to configure ASP2.0 for DSI are here ([DfE Sign-in (DSI) Integration](/Analyse-School-Performance-\(ASP\)-Index/Technical-specification/DfE-Sign%2Din-\(DSI\)-integration))
- Update variable group in Azure DevOps
--Each ASP 2.0 code deployment stage has an associated set of configuration settings which are read in during deployment and added to the web app and function app service on Azure. These variable groups are [here](https://dev.azure.com/dfe-ssp/s192-Analyse-School-Performance%20(ASP)/_library?itemType=VariableGroups)  
--Making changes to the variable groups requires ADO Admin access. 
- Deploy ASP2.0 codebase
--The codebase is deployed as part of the continuous deployment process. However, deployments to Test and higher environments requires approval.





##Data engineering responsibilities
- Add data to Cosmos DB




##Azure permissions
See the section on Privileged Identity Management ([Accessing Restricted Resources via Azure's Privileged Identity Management (PIM Request)](/Analyse-School-Performance-\(ASP\)-Index/Architecture-&-Infrastructure/Environments/Accessing-Restricted-Resources-via-Azure's-Privileged-Identity-Management-\(PIM-Request\))) for details on how to access the resources in Test and higher environments