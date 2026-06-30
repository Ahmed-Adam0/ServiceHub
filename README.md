# ServiceHub

[![Build Status](https://img.shields.io/badge/build-passing-brightgreen.svg)](#)
[![Framework](https://img.shields.io/badge/.NET-10.0-blue.svg)](https://dotnet.microsoft.com/)
[![Architecture](https://img.shields.io/badge/Architecture-Onion%20%2F%20Clean-orange.svg)](#architecture)
[![License](https://img.shields.io/badge/License-MIT-green.svg)](LICENSE)

An enterprise-grade service marketplace platform (similar to Fiverr/Khamsat) designed to connect customers with professional vendors. It is built using modern ASP.NET Core principles, focusing on clean separation of concerns, scalability, and domain-driven practices.

---

## 🎨 Logo Placeholder

```
  ██████  ███████ ██████  ██    ██ ██  ██████ ███████ ██   ██ ██    ██ ██████  
 ██       ██      ██   ██ ██    ██ ██ ██      ██      ██   ██ ██    ██ ██   ██ 
 ███████  █████   ██████  ██    ██ ██ ██      █████   ███████ ██    ██ ██████  
      ██  ██      ██   ██  ██  ██  ██ ██      ██      ██   ██ ██    ██ ██   ██ 
 ███████  ███████ ██   ██   ████   ██  ██████ ███████ ██   ██  ██████  ██████  
```
*An Enterprise Service Marketplace Solution*

---

## 📖 Project Description

**ServiceHub** is a production-ready, multi-tenant digital marketplace. It provides the infrastructure necessary for freelancers, professionals, and agencies (**Vendors**) to list specialized service packages, and for clients (**Customers**) to browse, purchase, track, and review these services.

### Business Problem
Modern micro-work and freelance platforms face severe issues regarding:
* Loose decoupling of business workflows (causing transaction errors).
* Tightly-coupled system architectures that do not scale under load spikes (e.g., promotional campaigns).
* Insecure identity models and data leaks.
* Poor SEO indexability for public freelancer listings.

### Project Vision
ServiceHub addresses these problems by separating concern spaces. We build a high-performance **Web API** optimized for rich Client Single Page Applications (SPAs) and mobile devices, coupled with a server-rendered **MVC portal** focused on search visibility and administrative management. The core engine runs on a robust, database-agnostic domain layer that prioritizes business rules over framework dependencies.

---

## ✨ Features

### Current Features
* **Scaffolded Onion Architecture Layout**: Verified clean build structure across Core, Presentation, and Infrastructure layers.
* **Pre-configured Solution References**: Strictly enforced inward-flowing references.
* **Repository Placeholders**: Organized workspace boundaries for DI setup.
* **Git Repository Files**: Configured `.gitignore` keeping builds, local secrets, and node binaries out of commit trees.

### Planned Features
* **Service Listings**: Dynamic category navigation, search filtering, and custom pricing packages (Basic, Standard, Premium).
* **Order Workspace**: Collaboration sandbox where customers and vendors share files and project requirements.
* **Secured Payments**: Multi-currency processing utilizing Stripe and PayPal integrations with refund mechanisms.
* **Real-time Notifications & Chat**: Instant notifications and direct client-vendor chats.
* **Wallet Management**: Ledger tracking deposits, payments, pending balances, and withdrawals.

### Future Enhancements
* **AI-Assisted Moderation**: Automating verification checks on proposals and portfolio images.
* **Localized Escrows**: Handling multi-jurisdiction tax and invoicing requirements.

---

## 🏗️ Architecture

ServiceHub is engineered using **Onion Architecture** and **Clean Architecture** patterns. This design keeps the business logic at the core of the application, completely isolated from databases, UIs, and external services.

```
                  ┌─────────────────────────────────────┐
                  │          SmartMarketplace.API       │
                  │          SmartMarketplace.MVC       │
                  └──────────────────┬──────────────────┘
                                     │ (References)
                                     ▼
                  ┌─────────────────────────────────────┐
                  │    SmartMarketplace.Infrastructure  │
                  └──────────────────┬──────────────────┘
                                     │ (References)
                                     ▼
                  ┌─────────────────────────────────────┐
                  │    SmartMarketplace.Application     │
                  └──────────────────┬──────────────────┘
                                     │ (References)
                                     ▼
                  ┌─────────────────────────────────────┐
                  │      SmartMarketplace.Domain        │
                  └─────────────────────────────────────┘
```

### Why Onion Architecture?
1. **Loose Coupling**: Business rules do not depend directly on database access methods or frameworks.
2. **Testability**: Because the core depends on interfaces rather than implementations, we can mock database engines and external gateways with ease.
3. **Flexibility**: We can swap SQL Server for PostgreSQL or switch mail providers without altering a single business rule.

### Project Separation Details

#### 💻 SmartMarketplace.Domain (Core)
The innermost project of the system containing business invariants. It defines Domain Entities, Value Objects, Domain Events, Enums, and Repository contract interfaces. It references no external NuGet libraries.

#### ⚙️ SmartMarketplace.Application (Core)
Implements the system's use cases by orchestrating domain behaviors. It defines application service implementations, DTOs, Mapping profiles, and Request validators.

#### 🔌 SmartMarketplace.Infrastructure (Infrastructure)
Implements interfaces declared by the Application/Domain cores. It houses Entity Framework Core setups (`DbContext`), Migrations, Caching registries (Redis), Identity configurations, and external clients (Email senders, SMS APIs, Payment SDKs).

#### 🛡️ SmartMarketplace.API (Presentation)
Exposes REST endpoints that serve data in JSON format. It is consumed by the Angular SPA, mobile devices, and external API consumers.

#### 🎛️ SmartMarketplace.MVC (Presentation)
A server-side rendered application responsible for high-SEO landing pages, public listings, and the Administrative Dashboard.

---

## 🛠️ Technology Stack

| Technology | Category | Description | Status |
| :--- | :--- | :--- | :--- |
| **.NET 10** | Runtime | Execution environment for backend web applications | Configured |
| **ASP.NET Core Web API** | Transport | High-performance JSON endpoint delivery | Configured |
| **ASP.NET Core MVC** | UI Engine | Server-side rendered views for Admin/SEO | Configured |
| **EF Core 10** | ORM | Database-to-object mapping and query evaluation | Configured |
| **SQL Server** | Database | Relational database engine for primary storage | Planned |
| **ASP.NET Identity** | Security | Standardized membership, user management, and roles | Planned |
| **JWT** | Security | Stateless claims transfer token for API authorization | Planned |
| **AutoMapper** | Utility | Mapping entities to data transfer shapes | Configured |
| **FluentValidation** | Verification | Strongly-typed checking of payload constraints | Planned |
| **SignalR** | Real-Time | Real-time push updates for notifications and messaging | Configured |
| **Redis** | Caching | In-memory distributed caching provider | Planned |
| **Docker** | DevOps | Containerization of the API, MVC, and Cache components | Planned |
| **Angular** | SPA | Rich client dashboard user interface | Planned |

---

## 📂 Folder Structure

The repository structure matches standard Onion layers:

```
ServiceHub/
│
├── SmartMarketplace.sln                  # Solution Metadata Configuration
├── .gitignore                            # Excluded file registry
├── README.md                             # Technical overview document
├── LICENSE                               # MIT License parameters
│
├── SmartMarketplace.API/                 # Web API Project (REST Endpoints)
│   ├── Controllers/
│   │   └── PlaceholderController.cs
│   ├── Contracts/
│   │   ├── Requests/
│   │   └── Responses/
│   ├── Middlewares/
│   ├── Filters/
│   ├── Extensions/
│   ├── DependencyInjection/
│   └── Swagger/
│
├── SmartMarketplace.Application/         # Application Core Layer (Use Cases)
│   ├── Constants/
│   │   └── Placeholder.cs
│   ├── DTOs/
│   │   └── PlaceholderDto.cs
│   ├── ExternalServices/
│   │   └── IExternalService.cs
│   ├── IRepositories/
│   │   └── IRepository.cs
│   ├── IServices/
│   │   └── IService.cs
│   ├── Mapping/
│   │   └── MappingProfile.cs
│   ├── Options/
│   │   └── PlaceholderOptions.cs
│   └── Services/
│       └── PlaceholderService.cs
│
├── SmartMarketplace.Domain/              # Domain Core Layer (Business Rules)
│   ├── Entities/
│   │   └── PlaceholderEntity.cs
│   └── Enums/
│       └── PlaceholderEnum.cs
│
├── SmartMarketplace.Infrastructure/      # Infrastructure Layer (Data & Integrations)
│   ├── AppDbContext/
│   │   └── PlaceholderDbContext.cs
│   ├── Data/
│   │   └── PlaceholderData.cs
│   ├── Identity/
│   │   └── PlaceholderIdentity.cs
│   ├── Localization/
│   │   └── PlaceholderLocalization.cs
│   ├── Migrations/
│   │   └── PlaceholderMigration.cs
│   ├── ProgramService/
│   │   └── ServiceRegistration.cs
│   ├── Repositories/
│   │   └── PlaceholderRepository.cs
│   ├── Services/
│   │   └── PlaceholderInfrastructureService.cs
│   └── SignalR/
│       └── PlaceholderHub.cs
│
└── SmartMarketplace.MVC/                 # MVC Presentation Project (Admin Panel & SEO)
    ├── Areas/
    │   └── PlaceholderArea.cs
    ├── Controllers/
    │   └── PlaceholderController.cs
    ├── Filters/
    │   └── PlaceholderFilter.cs
    ├── Helpers/
    │   └── PlaceholderHelper.cs
    ├── Models/
    │   └── PlaceholderModel.cs
    ├── Views/
    │   └── .gitkeep
    └── wwwroot/
        └── .gitkeep
```

---

## 📐 Design Principles

* **SOLID**: Enforced class-level structure (Single Responsibility, Open-Closed, Liskov Substitution, Interface Segregation, Dependency Inversion).
* **Clean Code**: Emphasizing readability, self-documenting naming structures, and minimizing unnecessary code comment clutter.
* **Separation of Concerns**: Presentation (delivery format), Infrastructure (framework bindings), Application (business workflow flow), and Domain (business data logic) are kept completely distinct.
* **Dependency Injection**: Injecting abstractions using .NET's built-in IoC container, avoiding hard dependency instantiations.
* **Repository Pattern**: Wrapping data access methods to simplify domain usage and mock out dependencies during unit testing.
* **Specification Pattern**: Decoupling conditional queries into reusable class parameters within the Domain layer.

---

## 🔒 Authentication Flow

We utilize a robust hybrid security structure:

<details>
<summary><b>Click to expand authentication details</b></summary>

### 🔑 ASP.NET Core Identity & Database Security
We use ASP.NET Core Identity to manage logins, role definitions, password hashing, and user tracking tables.
* Business tables do not directly link to Identity profiles. We define matching references (`UserId` keys) to prevent business models from leaking authentication details.

### 🌐 JWT Tokens for API (Angular & Mobile Clients)
* **Access Tokens**: Short-lived (e.g., 15 minutes) JWTs carrying user IDs, roles, and granular permissions.
* **Refresh Tokens**: Long-lived, database-checked, cryptographically secure strings used to request new Access Tokens.

### 🍪 Cookie Authentication for MVC
* MVC admin controllers use standard secure HTTP-Only cookies to protect dashboard pages.

### 👥 Roles & Permissions
* **System Roles**: `Administrator`, `Vendor`, `Customer`.
* **Claims-Based Permissions**: Granular authorization policies matching specific views and routes (e.g., `CanApproveService`, `CanWithdrawBalance`).

</details>

---

## 📦 Planned Modules

* **Customers**: Profile settings, purchase invoice tracking, dashboard tracking.
* **Vendors**: Professional descriptions, portfolio items, earnings ledger, proposals interface.
* **Services**: Marketplace package listings containing requirements, delivery windows, and price points.
* **Categories**: Hierarchical tags (e.g., *Software Development*, *Copywriting*).
* **Orders**: Interactive work boards where requirements are tracked and delivered.
* **Payments**: Escrow system receiving funds, holding balances, and executing vendor withdrawals.
* **Reviews**: Post-delivery feedback rating vendors on response times, service quality, and deadlines.
* **Notifications**: SignalR based real-time alerts.
* **Chat**: Instant messaging interfaces for clients and freelancers.
* **Wallet**: Accounts ledger holding cash balances, processing payments, and withdrawals.
* **Coupons**: Discount code systems.
* **Reports**: System performance charts detailing monthly transactions, disputes, and active service volumes.
* **Dashboard**: Key metric snapshots for active orders.
* **Admin Panel**: Control center for category updates, dispute resolution, and security management.

---

## 🗄️ Database Strategy

We use **SQL Server** as our central relational database. Database tables are managed using **Entity Framework Core Code First** design:
* All database mapping configurations (Primary Keys, Foreign Keys, Index constraints) are defined using EF Core **Fluent API** within `Infrastructure/Persistence/Configurations` folder files, keeping domain entity files free from database attributes.
* **Migrations** will be generated and executed sequentially after the domain and identity models are finalized.

---

## 🔌 API Documentation

API endpoints are mapped, compiled, and tested using **Swagger (OpenAPI)**. The swagger configuration registers bearer-auth requirements, allowing developers to execute JWT-secured operations directly inside the interactive browser dashboard.

---

## 🖥️ MVC Responsibilities

The `SmartMarketplace.MVC` project handles:
* The administrator controls, user accounts review, and category creation.
* Search-Engine-Optimized (SEO) public pages containing services directory, vendor listings, and blog portals.

---

## 🅰️ Angular Interface

The client-facing user interfaces (e.g., Customer Dashboard, Vendor workspace) will be built using **Angular**. It communicates with `SmartMarketplace.API` by passing secure JWT authorization headers.

---

## 🚀 Getting Started

### Prerequisites
* [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0)
* [SQL Server (Express or LocalDB)](https://www.microsoft.com/sql-server/)
* [Git](https://git-scm.com/)

### 1. Clone the Repository
```bash
git clone <repository-url-placeholder>
cd ServiceHub
```

### 2. Restore NuGet Packages
```bash
dotnet restore
```

### 3. Build the Solution
```bash
dotnet build
```

### 4. Run the API Project
```bash
dotnet run --project SmartMarketplace.API
```
*Navigating to `http://localhost:<port>/swagger` opens the OpenAPI Swagger playground.*

### 5. Run the MVC Project
```bash
dotnet run --project SmartMarketplace.MVC
```

---

## 🗺️ Roadmap

- [x] Solution Architecture & Onion Layers Setup
- [x] Project Folder Tree Scaffolding
- [ ] Domain Model Design
- [ ] Database Configurations & Entity mappings
- [ ] ASP.NET Identity & JWT Setup
- [ ] Repository Implementations & EF Core Context
- [ ] Application Services & Use Cases
- [ ] Web API & Middleware Implementations
- [ ] Admin Portal & Razor MVC Layouts
- [ ] Angular Frontend Integration
- [ ] Docker Containerization
- [ ] Deployment Configuration

---

## 🤝 Contributing

We welcome community contributions. Please follow these guidelines:
1. **Fork** the repository and create your feature branch (`git checkout -b feature/amazing-feature`).
2. Write clean code conforming to the style conventions.
3. Commit your changes and verify that the solution builds with no errors.
4. Open a **Pull Request** detailing your modifications.

---

## 📄 License

This project is licensed under the MIT License. See [LICENSE](LICENSE) for details.

---

## 👤 Author

**[Your Name / Username]**
* Technical Role: Lead Backend / Full-Stack Engineer
* Portfolio: `[https://portfolio.placeholder]`

---

## 📬 Contact

* **LinkedIn**: `[https://linkedin.com/in/placeholder]`
* **Email**: `[email.placeholder@example.com]`
* **GitHub**: `[https://github.com/placeholder]`
