Disaster recovery is the process of restoring application functionality in the wake of a catastrophic loss.

The nature of the disaster recovery is a business decision that varies from one application to the next. It might be acceptable for some applications to be unavailable or to be partially available with reduced functionality or delayed processing for a period of time. For other applications, any reduced functionality is unacceptable.

In the event of a failure event there are two available options
1. Accept the outage and fix-forward
2. Failover to an alternative region

The choice of option for a given application is based on the business criticality of the application since there are increasing costs and complexity as we move from option 1 (easiest and cheapest) to option 2 (most expensive and complex).
Two DR options have been prepared and will be presented for consideration – one approach that illustrates the High Priority Disaster Recovery Scenario and Medium Priority Disaster Recovery scenario.

##Pattern 1: Full geo-failover

In this scenario a full replica of the ASP BAU/ASP MVP solution is created in a secondary region and traffic manager is setup and configured to implement failover.

This model supports either a hot or cold failover. Steps could be taken to scale back services in the second region to reduce costs.

The solution relies of a second deployment of ASP BAU/ASP MVP to the secondary region and utilises the replicated data layer.

The solution extends the existing designs by adding a Traffic Manager component to the design and then preparing a failover playbook that details how failover will be orchestrated and restoration to the primary region would be achieved.

## Issues:
The solution extents the existing solution by adding a new component to the design
The solution will require further development to prepare a failover and failback playbook
The solution relies on a secondary region outside of West Europe. This is currently blocked by policy although exceptions can be requested



## Pattern 2: Full Redeployment 