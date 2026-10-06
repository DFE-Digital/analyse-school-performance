
## TO REVIEW Some simple examples from CSCP

- https://agilefactory.visualstudio.com/SPT/_workitems/edit/59939
- https://agilefactory.visualstudio.com/SPT/_workitems/edit/59604
- https://agilefactory.visualstudio.com/SPT/_workitems/edit/59682

# Bug Template

## Results PASS✅ or FAIL❌

## Describe the problem in a few words
When I log in as a ... 


## Steps to reproduce 
 
  * what user are you logged in as? 
  * what URL are you trying to access? 
1. 
2.
3.

## How should this be fixed?
?

## Current results including screenshots and links
?

## Environment

<Environment tested in goes here> e.g dev
<Versioned artifacts from deployment pipeline e.g pipeline link here> 

---

# Bug priority

Use below query, ranked by severity to review bugs

- TODO create query?
- Removes `Done` or `Removed` items.

# Bug raising

When raising bugs, we follow the below approach;

1. Copy and paste the `Bug Template` above and paste it into the `Description` of the raised bug. Fill out the template 

2. Relate any stories that the bug originates from

3. Determine the severity of the bug. There is a section when raising a bug in Azure DevOps titled `Severity` see below. 
    - `Critical` bugs are **brought into the current sprint immediately, and the team notified**.
    - All other bugs are taken to 3 amigos/refinement/bug triage to determine priority

  Some examples of `Critical` include

- Pages not loading
- Functionality failing (e.g Search auto-complete)

4. Notify at least 1 BA, Developer, Tester on the team through @NameOfPerson in the comments of the bug. If it's a `Critical` bug use the Stand-up chat.


---

# Bug fixing and retesting

1. Developer comments what the fix was, where the change was applied. where it can be retested.
2. Developer assigns the bug to a tester to retest
3. Tester retests the fix, writes any automated tests to catch problem in future, and moves to BA for business sign-off

---
