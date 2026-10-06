## RBAC
  - DSI integration
    - [ ] Research: new DSI API
    - [ ] Rearrange meeting with DSI team (Jane Ludlow)
    - [ ] Research: mocking for acceptance tests
    - [ ] Research: adding users/roles
    - [ ] Research: how to export users/roles from current ASP to ASP 2.0?
    - [ ] Should this be part of the core application or just in the web app?
   - User could belong to multiple orgs - need to get org for RBAC
   - Set up different roles for each user per org
   - Services and roles
   - Org - can identify LA/School from DSI org?
   - Each org has fields set up (from GIAS): URN, UID, UPIN, UKPRN
   - Configuration
   - Policies (filters roles available to approver to assign to user within DSI)
   - Conditions: "category IS Local Authority"
Diocese: "organisation IS (60 values)"
   - OIDC?
   - Sign out from DSI when sign out of ASP? No because single sign on.
   - RBAC at report level? (view, anonymised/named etc)
## Data processing modules
  - [ ] ASP-specific modules
    - [ ] Pupil Characteristics
    - [ ] Pupil Characteristics Filters
    - [ ] QLA User URNs
    - [ ] QLA Processor
    - [ ] Key Stage 2?
    - [ ] Key Stage 4?
  - [ ] Generic modules
    - [ ] query
    - [ ] where
    - [ ] select
    - [ ] single
    - [ ] count
    - [ ] average
    - [ ] percentage
    - [ ] case-when
    - [ ] any others?
  - [ ] Error handling
    - Want to see errors in html in place of pages/components
  - [ ] Data interpolation
  - [ ] Research: Pipeline syntax in Dataset report definition
  - [ ] POC
## Data coverage
  - [ ] Query or reverse lookup?
## Auditing
  - [ ] Does this belong in Core application or Web UI?
  - [ ] What granularity of user actions are we tracking?
    - Currently logs each URL visited by the user
  - [ ] Research: How to view/query the data once logged?
  - [ ] Consent management?
## Code architecture
  - Domain Driven Design?
    - Repository 
      - [ ] LINQ vs SQL?
      - [ ] JSON casing
      - [ ] LINQ serialization issue
  - [ ] Exceptions vs result objects?
