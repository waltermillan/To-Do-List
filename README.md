# 📝 To-DoList Project

A to-do list is a time management tool used to list all tasks or activities that need to be completed. It serves as a reminder of what needs to be done, allowing users to organize their work or personal lives more effectively.

---

## 📅 Changelog

- **06/04/2025**:
  - **Backend/Frontend/Database**: Added entity-relationship diagram and Users table. Backend: Cleaned up code, added User entity. Frontend: Added login module, improved UI rendering. Executed unit tests: Ran `ng test` to execute unit tests defined in the `*.spec.ts` files, using frameworks like Jasmine and the Karma test runner.
- **14/03/2025**:
  - **Backend/Frontend/Database**: Corrected database table names to plural form, cleaned up frontend code.
- **27/02/2025**:
  - **Backend/Frontend**: Removed comments, improved frontend code, fixed various bugs.
- **26/02/2025**:
  - **Frontend**: Fixed API to follow RESTful conventions and ensured that routes use proper pluralization. Corrected frontend URL invocations.
- **03/02/2025**:
  - **Frontend**: Updated frontend, added audio listening functionality and speech-to-text conversion.
- **02/02/2025**:
  - **Backend**: Implemented Factory design pattern to create task instances. Added Serilog for logging in ASP.NET Core applications.
  - **Backend**: Added `done` field to Task model.
- **31/01/2025**:
  - **Backend**: Added unit tests.
- **30/01/2025**:
- **29/01/2025**: Initial release — Backend, Frontend, and Database.

---

## 🎯 Objective

Practice using .NET (C#) / SQL Server and Angular (TypeScript) / Design Patterns / Onion Architecture. Gain familiarity with Angular forms. Use an API to convert speech to text — specifically the Web Speech API, which enables browsers to perform real-time speech recognition.

### Technologies:

- **.NET (C#)** and **SQL Server**
- **Angular (TypeScript)**
- **Design Patterns**
- **Onion Architecture**

---

## 🚀 Features

### 🔧 Backend

Implements design patterns: BaseEntity, Repository, UnitOfWork, and Factory (for task instance creation).

- Structured using **Onion Architecture**
- Utilizes several **Design Patterns**:
  - `BaseEntity`
  - `UnitOfWork`
  - `Repository` (for database access)
  - `DTO` (Data Transfer Object)
  - `Singleton` (for backend configuration)
  - `Factory` (for task instance creation)

- **Key Libraries**:
  - **Encryption**:
    - `System.Security.Cryptography` (AES-256)
  - **Logging**:
    - `Serilog`
    - `Serilog.Extensions.Logging`
    - `Serilog.Sinks.File`
  - **ORM**:
    - `Microsoft.EntityFrameworkCore.SqlServer`
    - `Microsoft.EntityFrameworkCore.Tools`
  - **UI**:
    - `@angular/material` 18.2.14
    - `@angular/cdk` 18.2.14

---

### 💻 Frontend

- Built with **Angular 18.0.2 / 18.2.14**
- Features:
  - Reactive Forms
  - `AuthService` and HTTP Interceptors
  - Modular architecture
  - Creation of services and models
  - Custom Pipes and Shared Modules
  - Angular Material for UI components and popups

---

### 🗄️ Database

- Uses **MariaDB**, deployed via **Docker Desktop**
- Includes:
  - Entity-Relationship Diagram written for SQL Server (ERD)
  - Sample data insertion scripts (`.sql`)
  - **DDL scripts** (for schema creation)
  - **DML scripts** (for sample data insertion)

---

## 🧪 Installation

### ✅ Prerequisites

Ensure the following tools are installed on your machine:

- [.NET SDK 9.0.200](https://dotnet.microsoft.com/)
- [SQL Express](https://www.microsoft.com/es-es/sql-server/sql-server-downloads)
- [Node.js + npm](https://nodejs.org/) (for the frontend)
- [Postman 11.44.3](https://www.postman.com/downloads/)

---

### 🔧 Setup Steps

1. Clone the repository:
    ```bash
    git clone https://github.com/waltermillan/To-Do-List.git
    ```

2. Follow the setup video guide:
    - [Version 1 - Display Version](https://youtu.be/478V9e3bG60)

3. Complete the remaining setup steps as described in the project documentation.

---

## 📄 License

This project is licensed under the [MIT License](LICENSE).
