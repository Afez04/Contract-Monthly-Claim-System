# Contract Monthly Claim System (CMCS)

## Overview
The Contract Monthly Claim System (CMCS) is a C# WPF desktop application built for the PROG6212 Portfolio of Evidence (POE).  
It replaces the manual monthly-claim process used by Independent Contractor (IC) lecturers and introduces a faster, digital, and more accurate workflow.

The system now includes:
- Database integration  
- Secure login  
- Role-based access  
- Auto-calculation of claim totals  
- Report generation  
- HR management tools  

## Features

### Lecturer
- Submit monthly claims with:
  - Module name  
  - Module code  
  - Hours worked  
  - Auto-loaded hourly rate  
  - Supporting documents (.pdf, .docx, .xlsx)  
- The system automatically calculates the total amount.  
- View past submissions and see if they are **Pending**, **Approved**, or **Rejected**.

### Coordinator
- View lecturer-submitted claims.  
- Approve or reject claims.  
- Check hours, totals, and correctness.  
- Pass valid claims to the Manager.


### Manager
- Review and finalise claims approved by the Coordinator.  
- Approve or reject claims with comments.  
- Monitor all claim activity in the system.

### HR
- Add, edit, or delete users (Lecturers, Coordinators, Managers).  
- Set hourly rates for lecturers.  
- Generate PDF claim summary reports.  
- View claim overview with totals, approved and rejected counts.

## Technology Stack

| Component | Description |
|----------|-------------|
| **Language** | C# |
| **Framework** | .NET (WPF Desktop Application) |
| **Database** | SQL Server using Entity Framework Core |
| **PDF Generation** | iTextSharp |
| **Version Control** | GitHub |
| **UI Styling** | Material Design & custom WPF styles |

## HR Login Details
Username:Andiswa39
Password:Phewa#778P
