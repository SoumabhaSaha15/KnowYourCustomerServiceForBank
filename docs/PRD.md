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
  CREATE TABLE [dbo].[Users] (
    [UserId] int NOT NULL IDENTITY(1,1),
    [FullName] nvarchar(100) NOT NULL,
    [Email] nvarchar(254) NOT NULL,
    [PhoneNumber] nvarchar(20),
    [Address] nvarchar(255),
    [Password] nvarchar(256) NOT NULL,
    [OnboardingStatus] nvarchar(max) NOT NULL,
    [DateOfBirth] date,
    [UserRole] nvarchar(max) NOT NULL,
    [IsActive] bit NOT NULL,
    [CreatedAt] datetime2(7) NOT NULL,
    [UpdatedAt] datetime2(7) NOT NULL,
    PRIMARY KEY ([UserId])
  );

  CREATE TABLE [dbo].[SessionCache] (
    [Id] nvarchar(449) NOT NULL,
    [Value] varbinary(max) NOT NULL,
    [ExpiresAtTime] datetimeoffset(7) NOT NULL,
    [SlidingExpirationInSeconds] bigint,
    [AbsoluteExpiration] datetimeoffset(7),
    PRIMARY KEY ([Id])
  );

  CREATE TABLE [dbo].[AuditLogs] (
    [AuditLogId] int NOT NULL IDENTITY(1,1),
    [UserId] int NOT NULL,
    [Action] nvarchar(max) NOT NULL,
    [Remarks] nvarchar(256) NOT NULL,
    [CreatedAt] datetime2(7) NOT NULL,
    [UpdatedAt] datetime2(7) NOT NULL,
    PRIMARY KEY ([AuditLogId])
  );

  CREATE TABLE [dbo].[__EFMigrationsHistory] (
    [MigrationId] nvarchar(150) NOT NULL,
    [ProductVersion] nvarchar(32) NOT NULL,
    PRIMARY KEY ([MigrationId])
  );

  CREATE TABLE [dbo].[RiskProfiles] (
    [RiskProfileId] int NOT NULL IDENTITY(1,1),
    [UserId] int NOT NULL,
    [RiskLevel] nvarchar(max) NOT NULL,
    [Score] float(53),
    [CreatedAt] datetime2(7) NOT NULL,
    [UpdatedAt] datetime2(7) NOT NULL,
    PRIMARY KEY ([RiskProfileId])
  );

  CREATE TABLE [dbo].[Accounts] (
    [AccountId] int NOT NULL IDENTITY(1,1),
    [AccountType] nvarchar(max) NOT NULL,
    [AccountStatus] nvarchar(max) NOT NULL,
    [UserId] int NOT NULL,
    [CreatedAt] datetime2(7) NOT NULL,
    [UpdatedAt] datetime2(7) NOT NULL,
    PRIMARY KEY ([AccountId])
  );

  CREATE TABLE [dbo].[Documents] (
    [DocumentId] int NOT NULL IDENTITY(1,1),
    [UserId] int NOT NULL,
    [DocumentType] nvarchar(max) NOT NULL,
    [FilePath] nvarchar(max) NOT NULL,
    [DocumentVerificationStatus] nvarchar(max) NOT NULL,
    [CreatedAt] datetime2(7) NOT NULL,
    [UpdatedAt] datetime2(7) NOT NULL,
    PRIMARY KEY ([DocumentId])
  );


  ALTER TABLE [dbo].[AuditLogs]
  ADD CONSTRAINT [FK_AuditLogs_Users_UserId]
  FOREIGN KEY ([UserId]) 
  REFERENCES [dbo].[Users]([UserId])
  ON DELETE NO ACTION
  ON UPDATE NO ACTION;



  ALTER TABLE [dbo].[RiskProfiles]
  ADD CONSTRAINT [FK_RiskProfiles_Users_UserId]
  FOREIGN KEY ([UserId]) 
  REFERENCES [dbo].[Users]([UserId])
  ON DELETE NO ACTION
  ON UPDATE NO ACTION;



  ALTER TABLE [dbo].[Accounts]
  ADD CONSTRAINT [FK_Accounts_Users_UserId]
  FOREIGN KEY ([UserId]) 
  REFERENCES [dbo].[Users]([UserId])
  ON DELETE NO ACTION
  ON UPDATE NO ACTION;



  ALTER TABLE [dbo].[Documents]
  ADD CONSTRAINT [FK_Documents_Users_UserId]
  FOREIGN KEY ([UserId]) 
  REFERENCES [dbo].[Users]([UserId])
  ON DELETE NO ACTION
  ON UPDATE NO ACTION;
```
