[[_TOC_]]


# Purpose

Non-functional requirements (NFRs), also known as quality attributes or system qualities, that are a critical part of software and system development. 

# 1. Scalability 
The application should be able to handle a growing amount of data and users over time. Ensure that it can scale horizontally to accommodate increased load.

**_Metrics:_** 
Requests per Second (RPS) - Ensure the application can handle a specific number of RPS, and monitor it as usage grows. Aim for scalability by adding resources as needed to maintain performance.

# 2. Performance 
The reports and dashboards should load quickly, even with extensive data. Consider using caching mechanisms and optimizing database queries for efficient data retrieval.

**_Metrics:_** 
Page Load Time - Aim for fast page load times, typically **_under 2 seconds_**, to provide a responsive user experience even with extensive data.

# 3. Security
##Data Security
Implement robust data encryption both in transit and at rest. Ensure user data privacy and compliance with data protection regulations.
##Authentication and Authorization
Implement strong user authentication and authorization mechanisms to control access to sensitive information.
##Security Patching
Regularly update and patch all components of the application to protect against security vulnerabilities.

**_Metrics:_** 
Security Vulnerabilities - Regularly scan for and fix security vulnerabilities. Aim for zero critical vulnerabilities by using tools like [Microsoft Defender for Cloud](https://azure.microsoft.com/en-gb/products/defender-for-cloud/).

# 4. Availability and Reliability
Ensure high availability with minimal downtime. Implement failover mechanisms and disaster recovery plans to minimize service interruptions.

**_Metrics:_** 
1. Uptime Percentage - Aim for at least 99.9% uptime (three nines) to minimize downtime and ensure availability.
2. Mean Time Between Failures (MTBF) - Aim for an MTBF of at least 10,000 hours.

# 5. Data Backup and Recovery
Regularly back up user data and provide a reliable mechanism for data recovery in case of accidental data loss.
**_Metrics:_** 
Recovery Time Objective (RTO) - Set a low RTO to minimize data loss and downtime in case of data recovery events, typically measured in minutes.

# 6. Compliance with Internal Policies
Adhere to internal organizational policies and guidelines related to IT infrastructure and security.
**_Metrics:_** 
Compliance Audit Pass Rate - Achieve a 100% pass rate during compliance audits with relevant regulations i.e. [GDPR](https://www.itgovernance.co.uk/gdpr-compliance-checklist) - General Data Protection Regulation .

# 7. Disaster Recovery and Geographic Redundancy: 
Consider deploying the application across multiple geographic regions for redundancy and disaster recovery.
**_Metrics:_** 
Failover Time between Regions - Minimize failover time between geographic regions to ensure seamless redundancy.


# 8. Load Testing
Conduct load testing to ensure the application can handle peak usage without degradation in performance. Identify bottlenecks and optimize accordingly.
**_Metrics:_** 
Concurrent Users during Load Test - Determine the maximum number of concurrent users your application can handle while maintaining acceptable response times.

# 9. Monitoring and Logging
Implement robust monitoring and logging systems to track application performance, user activities, and potential security incidents.
**_Metrics:_** 
Log Analysis Time - Ensure logs are analysed promptly. Aim for log analysis within minutes to detect and respond to issues quickly.

# 10. User Experience (UX)
Ensure a user-friendly interface with intuitive navigation. Conduct usability testing to refine the user experience.

**_Metrics:_** 
User Satisfaction (e.g., NPS - Net Promoter Score) - Aim for a high NPS score (above 70) to indicate a positive user experience.

_*TODO*_ : understand from Chris if we can maintain an interactive poll dashboard.

# 11. Response Time
Define acceptable response times for different actions within the application, such as generating reports or loading dashboards, and ensure they are met.

**_Metrics:_** 
Response Time Distribution (e.g., 95th percentile) - Define response time targets for various actions and ensure that the majority of responses meet these targets.

# 12. Cost Optimization
Monitor and optimize cloud resource usage to control costs. Use auto-scaling and cost-effective cloud services where applicable.

**_Metrics:_** 
Cost per User or Cost per Transaction - Monitor and reduce the cost per user or transaction to optimize cloud resource spending.

# 13. Comprehensive Testing
Thoroughly test the application for _performance_, _security_, and _compatibility_ with various browsers and devices.

**_Metrics:_** 
Test Coverage Percentage - Aim for high test coverage (above 80%) to ensure thorough testing of the application.


# 14. Scalable Database
Choose a database solution that can handle the aggregation of extensive user data across years efficiently.

**_Metrics:_** 
Database Query Response Time
Keep database query response times within acceptable limits, typically measured in milliseconds.

# 15. User Training and Support
Provide adequate training and support resources for internal users to ensure they can make the most of the application.

**_Metrics:_** 
User Support Response Time (Tool for tracking e.g. service now) - Aim for quick response times to user inquiries or issues ( ETA e.g., within 1 hour).

# 16. Browser Compatibility
Ensure the application is compatible with a wide range of web browsers and stays up-to-date with evolving web standards.

**_Metrics:_** 
Browser Compatibility Score : Aim for a high compatibility score (e.g., 95% compatibility) across popular web browsers.

# 17. Accessibility 
Make the application accessible to users with disabilities in compliance with accessibility standards like WCAG.

**_Metrics:_** 
Accessibility Violations: Minimize accessibility violations to ensure compliance with WCAG guidelines.


# 18. Resource Efficiency 
Optimize resource usage to minimize the environmental footprint of the cloud-based infrastructure.

**_Metrics:_** 
Resource Utilization Efficiency : Optimize resource utilization to reduce environmental impact. Measure energy usage and carbon emissions.

# 19. Data Retention
Storing data for specific periods as per legal or operational requirements.
**_Metrics:_** 
1. Retention Period: Define data retention periods (e.g., user logs for 1 year).
2. Data Volume: Set limits to control data growth (e.g., 10% annually).
3. Data Aging: Measure data obsolescence rates.

# 20. Data Archival
Moving data to secondary storage for long-term retention.
**_Metrics:_** 
1. Archival Frequency: Specify how often data is moved (e.g., nightly).
2. Archival Efficiency: Optimize resource usage.
3. Data Retrieval Time: Ensure timely data access (e.g., < 1 hour).

# 21. Data Purging
Permanently removing unnecessary data.
**_Metrics:_** 
1. Purging Triggers: Define criteria for data deletion (e.g., data older than 7 years).
2. Purging Frequency: Schedule purging operations.
3. Data Deletion Efficiency: Optimize resource usage.
4. Data Recovery: Establish recovery procedures.
