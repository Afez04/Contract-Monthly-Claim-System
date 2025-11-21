# Rationale_Database.md  

## 1. Introduction  
The database for the **Contract Monthly Claim System (CMCS)** was designed to support the workflow of lecturers, coordinators, and managers. It ensures that claims are properly tracked, reviewed, and verified. The goal of this design is to maintain **data consistency, accountability, and separation of responsibilities** across different user roles.  



## 2. Entity Justification  

- **Lecturer Table**  
  Stores lecturer details (name, email, module information). Each claim must be tied to a lecturer.  

- **Coordinator Table**  
  Contains coordinators who are responsible for reviewing claims submitted by lecturers.  

- **Manager Table**  
  Contains managers who verify and finalize claims after they have been reviewed by coordinators.  

- **Claims Table**  
  The core of the system. It records module details, hours worked, total amount, status (Pending, Approved, Rejected), and who reviewed or verified it.  

- **SupportingDocuments Table**  
  Stores uploaded files such as timesheets or evidence of work. These documents are linked to claims to ensure fair verification.  



## 3. Relationships  

- **Lecturer → Claims**: One lecturer can submit many claims.  
- **Coordinator → Claims**: Each claim must be reviewed by one coordinator.  
- **Manager → Claims**: Each claim must be verified by one manager after coordinator review.  
- **Claims → SupportingDocuments**: A claim may have zero or many supporting documents.  

This structure ensures that every claim is fully traceable from submission to final verification.  



## 4. Assumptions  

1. Lecturers, coordinators, and managers are stored in **separate tables** to simplify access control.  
2. Claim statuses are limited to: `"Pending"`, `"Approved"`, `"Rejected"`.  
3. Each claim is always linked to **exactly one lecturer and one module**.  
4. Supporting documents are optional but strongly encouraged for accountability.  


## 5. Constraints  

1. Claims must always reference a valid lecturer ID.  
2. A claim cannot be marked as `"Approved"` unless it was reviewed by a coordinator.  
3. Only coordinators and managers are allowed to change claim statuses.  
4. Claims must always have a valid module code and total amount calculated.  
5. Data duplication is minimized by using **foreign keys** instead of repeating information.  


## 6. Normalization  

The database is normalized to **3rd Normal Form (3NF)**:  
- Lecturer details are stored only once in the Lecturer table.  
- Claim records reference lecturers, coordinators, and managers by ID rather than duplicating data.  
- Supporting documents reference claims using foreign keys, ensuring proper linkage without redundancy.  



## 7. Conclusion  

The database structure mirrors the workflow of the CMCS: **Lecturers → Coordinators → Managers**. By using separate tables, foreign keys, and status tracking, the design guarantees **data integrity, security, and accountability**. It ensures that each claim follows a transparent process from submission to approval or rejection.  
