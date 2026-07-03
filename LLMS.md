# Project Prompt Context: Banking User Onboarding & KYC Compliance Management System

You are an expert full-stack developer assisting with a decoupled web application implementation for the "Banking User Onboarding & KYC Compliance Management System.

## Technical Stack & Developer Environment
* **IDE/Package Manager:** VS Code using `pnpm` workspace / monorepo structure.
* **Backend:** ASP.NET Core Web API  (Generated via the standard Visual Studio template) targeting .NET 8.0+.
* **Database & ORM:** Microsoft SQL Server  with Entity Framework Core.
* **Frontend:** React (SPA) utilizing:
  * **Routing:** `@tanstack/react-router`
  * **UI & Styling:** Material UI (MUI) combined with Tailwind CSS.
  * **Forms & Validation:** `react-hook-form` with `zod` for schema validation.
  * **HTTP Client:** `axios` for API consumption.

---

## 1. System Overview & Architecture
This system enables digital user onboarding, identity verification, automated risk scoring, account approval workflows, and compliance auditing. It is divided into five core functional modules:
1. User Registration & Profile Creation 
2. KYC Document Upload & Verification 
3. Risk Assessment & User Scoring 
4. Account Creation & Approval Workflow 
5. Compliance Monitoring & Audit Trail Management 

---

## 2. API Endpoints (ASP.NET Web API) & Frontend Mapping

### 2.1 User Registration & Profile Creation
* **Backend API:** `UserController` 
  * `POST /api/user` -> `RegisterUser([FromBody] RegisterDto dto)` 
  * `PUT /api/user/{id}` -> `UpdateUserProfile(int id, [FromBody] UpdateDto dto)` 
  * `GET /api/user/{id}` -> `GetUserDetails(int id)` 
* **Frontend Route:** `/user/register`, `/user/profile`
* **Form State:** `react-hook-form` validated via a `zod` schema enforcing names, emails, and phone formatting.

### 2.2 KYC Document Upload & Verification
* **Backend API:** `KycController` 
  * `POST /api/kyc/upload` -> `UploadDocument([FromForm] UploadDto dto)` 
  * `POST /api/kyc/verify` -> `VerifyDocument([FromBody] VerifyDto dto)` 
  * `GET /api/kyc/user/{userId}` -> `GetKycDocuments(int userId)` 
* **Frontend Route:** `/kyc/upload`, `/kyc/verify`
* **UI Component:** MUI file upload drag-and-drop element.

### 2.3 Risk Assessment & User Scoring
* **Backend API:** `RiskController` 
  * `POST /api/risk/calculate` -> `CalculateRiskScore([FromBody] CalculateDto dto)` 
  * `GET /api/risk/user/{userId}` -> `GetRiskResult(int userId)` 
* **Frontend Route:** `/admin/risk-assessment`

### 2.4 Account Creation & Approval Workflow
* **Backend API:** `AccountController` 
  * `POST /api/account` -> `CreateAccount([FromBody] CreateAccountDto dto)` 
  * `POST /api/account/{id}/approve` -> `ApproveAccount(int id)` 
  * `POST /api/account/{id}/reject` -> `RejectAccount(int id)` 
  * `GET /api/account/{id}` -> `GetAccountDetails(int id)` 
* **Frontend Route:** `/dashboard/accounts`, `/admin/approvals`

### 2.5 Compliance Monitoring & Audit Trail
* **Backend API:** `ComplianceController` 
  * `POST /api/compliance/log` -> `LogComplianceEvent([FromBody] LogDto dto)` 
  * `GET /api/compliance/report` -> `GetComplianceReport()` 
* **Frontend Route:** `/admin/audit-logs`

---

## 3. Database Schema & Core Entities (SQL Server)

```sql
CREATE TABLE User (
    userId INT IDENTITY(1,1) PRIMARY KEY,
    fullName VARCHAR(100),
    email VARCHAR(100),
    mobileNumber VARCHAR(20),
    address VARCHAR(255),
    dateOfBirth DATE,
    onboardingStatus VARCHAR(20) CHECK (onboardingStatus IN ('NEW', 'IN_PROGRESS', 'COMPLETED'))
);

CREATE TABLE KycDocument (
    documentId INT IDENTITY(1,1) PRIMARY KEY,
    userId INT,
    documentType VARCHAR(50),
    filePath VARCHAR(255),
    verificationStatus VARCHAR(20) CHECK (verificationStatus IN ('PENDING', 'VERIFIED', 'REJECTED')),
    FOREIGN KEY (userId) REFERENCES User(userId)
);

CREATE TABLE RiskProfile (
    riskId INT IDENTITY(1,1) PRIMARY KEY,
    userId INT,
    score INT,
    riskLevel VARCHAR(20) CHECK (riskLevel IN ('LOW', 'MEDIUM', 'HIGH')),
    FOREIGN KEY (userId) REFERENCES User(userId)
);

CREATE TABLE Account (
    accountId INT IDENTITY(1,1) PRIMARY KEY,
    userId INT,
    accountType VARCHAR(50),
    accountStatus VARCHAR(20) CHECK (accountStatus IN ('PENDING_APPROVAL', 'ACTIVE', 'REJECTED')),
    createdDate DATE,
    FOREIGN KEY (userId) REFERENCES User(userId)
);

CREATE TABLE AuditLog (
    logId INT IDENTITY(1,1) PRIMARY KEY,
    userId INT,
    action VARCHAR(100),
    status VARCHAR(50),
    remarks VARCHAR(255),
    timestamp DATETIME,
    FOREIGN KEY (userId) REFERENCES User(userId)
);