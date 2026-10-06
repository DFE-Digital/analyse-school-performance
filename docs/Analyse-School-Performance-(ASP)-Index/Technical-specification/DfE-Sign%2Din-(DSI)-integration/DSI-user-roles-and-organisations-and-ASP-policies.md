[[_TOC_]]

### 1. Overview of Test Roles & Organisations Created within DSI

The DfE Sign-in (DSI) includes a variety of roles and organisations to manage access and permissions effectively. Roles are defined with specific codes, names, and access levels. These roles include but are not limited to:
*   **DfE Unnamed**
*   **DfE Named**
*   **Diocese Unnamed**
*   **Diocese Named**
*   **LA Unnamed**
*   **LA Named**
*   **MAT Unnamed**
*   **MAT Named**
*   **MAT Governor**
*   **School Unnamed**
*   **School Named**
*   **School Governor**
*   **Super Admin**
*   **Training Unnamed**

* * *

### 2. Overview of Different Policies and Authorization Handlers within ASP

ASP.NET Core uses policies and authorization handlers to manage access control. Policies are defined in the `Policy` class and are associated with specific roles. The following policies are implemented:
*   **Any**
*   **AccessToSearch**
*   **AccessToMyLocalAuthority**
*   **AccessToMySchool**
*   **AccessToMySchools**
*   **AccessToMyLaSchools**
*   **AccessToMyMatSchools**
*   **AccessToAllSchools**
*   **AccessToGenericSchool**
*   **AccessToAllLocalAuthorities**
*   **AccessToMyDioceseSchools**
*   **AccessToGuidance**
*   **AccessToEditPages**
*   **AdminOnly**
*   **NamedData**

#### 2.1 Where Policies Are Used

Policies are applied to controllers and actions using the `[Authorize]` attribute. For example:
*   The `GenericSchoolController` uses the **AccessToGenericSchool** policy to restrict access to users with the appropriate role.
*   The **NamedData** policy is used for actions requiring elevated permissions, such as downloading sensitive data.

* * *

### 3. How These Are Tested within the Functional Test Framework

The functional test framework includes comprehensive tests to validate the behavior of policies and roles. The `AuthorizationStepDefinitions` class defines test steps for various scenarios, such as:
*   Testing access for logged-in users with specific roles.
*   Validating access to resources based on policies.
*   Ensuring fallback policies require authentication.
*   Simulating different user roles and verifying access control.

#### 3.1 TestClaimsProvider Setup

The `TestClaimsProvider` class is used to simulate user claims during testing. It allows setting roles, names, and other claims to mimic real-world scenarios. This ensures that the functional tests cover all possible combinations of roles and policies.