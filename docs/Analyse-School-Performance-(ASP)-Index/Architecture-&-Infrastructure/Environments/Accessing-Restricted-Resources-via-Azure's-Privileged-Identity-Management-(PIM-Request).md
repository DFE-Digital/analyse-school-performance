#Accessing Restricted Resources via Azure AD Privileged Identity Management (PIM Request)
To access Test, Pre-Production or Production resource groups, team members will need to submit a PIM request.

###Accessing resources in Test

- Go to  [Azure AD Privileged Identity Management](https://portal.azure.com/#view/Microsoft_Azure_PIMCommon/CommonMenuBlade/~/quickStart)
- Select 'Azure Resources'
- Then 'Activate role'

![image.png](/docs/.attachments/image-a05d7645-4f8e-440d-b175-584274356e27.png)

- Select the resource you wish to access

For example, if you needed to access resources within s192-analyse-school-performance-test, select Activate.

![image.png](/docs/.attachments/image-efeffb1c-e645-4a38-89ca-b30fb95f6726.png)

- In the 'Activate - Contributer' pop-up, add a brief Reason and click 'Activate'

**Approval is automatic in Test**. The 'Activate - Contributer' pop-up should update to show that your role as a 'Contributer' has been activated and should will now be able to access Test resources.

![image.png](/docs/.attachments/image-762dc1fc-5087-459f-9f79-26be9a15b4cd.png)

##Accessing resources in Pre-Prod and Prod

Follow the steps above to activate your 'Contributer' role for a Production resources. Activation is not automatic, and will require approval. Approval will be assessed and granted by a team member with the required permissions.

Approvals are time limited and access rights will be automatically revoked after 8 hours.