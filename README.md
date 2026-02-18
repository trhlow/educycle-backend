# 🎓 EduCycle Backend API

> **Platform for Trading & Recycling Educational Resources**  
> Built with **.NET 10 (Preview)** and **Clean Architecture** principles.

![.NET](https://img.shields.io/badge/.NET-10.0-purple?style=flat&logo=dotnet)
![Status](https://img.shields.io/badge/Status-Active-success)
![License](https://img.shields.io/badge/License-MIT-blue)

## 📖 Overview

**EduCycle** is a RESTful API backend designed to power a marketplace for students to buy, sell, and exchange educational materials (books, devices, stationery). It ensures secure transactions, user verification, and robust product management.

Key capabilities include:
- **Secure Authentication**: JWT-based auth with Role-Based Access Control (RBAC).
- **Social Login**: Integration with Google, Facebook, and Microsoft.
- **Verification**: Phone number verification via OTP.
- **Transaction Safety**: Escrow-like transaction states (Pending → Completed/Sold).
- **Real-time Interaction**: Message polling and instant notifications.

---

## 🛠️ Tech Stack

- **Framework**: [.NET 10](https://dotnet.microsoft.com/) (ASP.NET Core Web API)
- **Language**: C# 13
- **Database**: SQL Server / Azure SQL
- **ORM**: Entity Framework Core 10.0
- **Authentication**: JWT Bearer, OAuth2 (Social Login)
- **Validation**: FluentValidation
- **Documentation**: Swagger / OpenAPI (Swashbuckle)
- **Utilities**: BCrypt.Net (Hashing)

---

## ✨ Key Features

### 🔐 Authentication & Users
- User Registration & Login (Email/Password).
- **Social Login** (Google, Facebook, Microsoft).
- **Phone Verification** (OTP).
- Role Management: `Admin` vs `User`.
- User Profiles: Manage contact info, avatar, location.

### 📦 Product Management
- **CRUD Operations**: Create, Read, Update, Delete products.
- **Approval Workflow**: Admin approval required for listings.
- **Status Tracking**: Pending, Approved, Rejected, Sold.
- **Image Handling**: Support for multiple product images.
- **Search & Filtering**: Filter by category, price, condition.

### 💸 Transactions & Orders
- **Buy/Sell Flow**: Buyer initiatives transaction → Seller approves → Payment/Exchange.
- **OTP Verification**: Secure handover with transaction-specific OTPs.
- **History**: View past purchases and sales.

### 💬 Communication & Reviews
- **Messaging System**: Chat between buyers and sellers.
- **Reviews**: Rate users and products (1-5 stars) with comments.

---

## 🚀 Getting Started

### Prerequisites
- [.NET 10.0 SDK](https://dotnet.microsoft.com/download/dotnet/10.0) (Preview)
- [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads) (LocalDB or Docker)
- [Visual Studio 2022](https://visualstudio.microsoft.com/) (Preview) or [VS Code](https://code.visualstudio.com/)

### 📥 Installation

1. **Clone the repository**
   ```bash
   git clone https://github.com/trhlow/educycle-backend.git
   cd educycle-backend
   ```

2. **Configure Database**
   Update `appsettings.json` (or `appsettings.Development.json`) with your connection string:
   ```json
   "ConnectionStrings": {
     "DefaultConnection": "Server=.;Database=EduCycleDb;Trusted_Connection=True;TrustServerCertificate=True;"
   }
   ```

3. **Database Migration**
   Apply migrations to create the database schema:
   ```bash
   cd EduCycle.Api
   dotnet ef database update
   ```

4. **Run the API**
   ```bash
   dotnet run
   ```
   The API will be available at `https://localhost:7054` (or configured port).

---

## 📚 API Documentation

Once the application is running, access the **Swagger UI** to explore and test endpoints:

👉 **[https://localhost:7054/swagger](https://localhost:7054/swagger)**

### Key Endpoints
- `POST /api/auth/login`: Authenticate user.
- `POST /api/auth/social-login`: Login with social provider.
- `POST /api/auth/verify-phone`: Verify user phone number.
- `GET /api/products`: Browse available products.
- `POST /api/transactions`: Initiate a purchase.

---

## 📂 Project Structure

The solution follows **Clean Architecture** principles:

```
EduCycle.sln
├── 📂 EduCycle.Api           # Entry point, Controllers, Configuration
├── 📂 EduCycle.Application   # Business Logic, Services, Interfaces, DTOs
├── 📂 EduCycle.Domain        # Entities, Enums, Value Objects
├── 📂 EduCycle.Infrastructure # Data Access, Repositories, External Services
└── 📂 EduCycle.Contracts     # Shared DTOs (if separated)
```

---

## ⚙️ Configuration

Key settings in `appsettings.json`:

| Section | Description |
| :--- | :--- |
| **ConnectionStrings** | Database connection info. |
| **Jwt** | `Key`, `Issuer`, `Audience` for token generation. |
| **Logging** | Log levels for debugging. |

> **Note for Developers**: In `Development` environment, the Admin password is automatically reset to `admin@1` on startup for easier testing.

---

## 🤝 Contributing

1. Fork the repository.
2. Create a feature branch (`git checkout -b feature/amazing-feature`).
3. Commit your changes (`git commit -m 'Add amazing feature'`).
4. Push to the branch (`git push origin feature/amazing-feature`).
5. Open a Pull Request.

---

## 📄 License

Distributed under the MIT License. See `LICENSE` for more information.
