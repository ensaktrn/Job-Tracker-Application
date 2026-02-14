# JobTracker Application API

A multi-user job tracking backend built with **ASP.NET Core**, **Clean Architecture**, and **PostgreSQL**.

## 🚀 Features

- JWT Authentication (Access + Refresh Tokens)
- Refresh Token Rotation
- Logout (Token Revocation)
- Shared Companies & Job Postings
- User-Scoped Job Applications
- Pagination & Filtering
- Clean Architecture Structure
- Swagger Documentation

---

## 🧱 Architecture

The project follows Clean Architecture principles:

Domain -> Application -> Infrastructure -> API
- **Domain**: Entities & business rules  
- **Application**: Use cases, DTOs, interfaces  
- **Infrastructure**: EF Core, Identity, repositories  
- **API**: Controllers, middleware, authentication  

---

## 🔐 Authentication Flow

1. Register / Login  
2. Receive `accessToken` + `refreshToken`  
3. Use Bearer token for API calls  
4. Refresh when expired  
5. Logout → refresh token revoked  

---

## 📦 Tech Stack

- .NET (ASP.NET Core)
- Entity Framework Core
- PostgreSQL
- ASP.NET Identity
- JWT Bearer Auth
- Swagger

---

## 🌍 Multi-User Model

### Shared
- Companies
- Job Postings  
(All users can see them, with `CreatedByEmail` attribution.)

### Private
- Job Applications  
(Each user only sees and manages their own applications.)

---

## 🛠 Run Locally

### 1. Configure database connection

Update: src/JobTracker.Api/appsetings.Development.json

### 2. Apply migrations

dotnet ef database update
--project src/JobTracker.Infrastructure
--startup-project src/JobTracker.Api


### 3. Run the API

dotnet run --project src/JobTracker.Api

Swagger available at: http://localhost:xxxx/Swagger


---

## 📌 Status

Backend complete and ready for frontend integration.

---

## 📄 License

MIT
