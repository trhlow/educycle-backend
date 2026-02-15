# 🎓 EduCycle Backend API

Nền tảng trao đổi sách & tài liệu học tập dành cho sinh viên.  
Backend REST API xây dựng bằng **ASP.NET Core (.NET 10)** theo kiến trúc Clean Architecture.

---

## 📋 Mục lục

- [Tech Stack](#-tech-stack)
- [Kiến trúc dự án](#-kiến-trúc-dự-án)
- [Cài đặt & Chạy](#-cài-đặt--chạy)
- [Cấu hình](#-cấu-hình)
- [API Endpoints](#-api-endpoints)
- [Authentication & Authorization](#-authentication--authorization)
- [Validation](#-validation)
- [Database Schema](#-database-schema)
- [Seed Data](#-seed-data)
- [Error Response Format](#-error-response-format)
- [Development Rules](#-development-rules)

---

## 🛠 Tech Stack

| Công nghệ | Version | Mô tả |
|---|---|---|
| .NET | 10.0 | Runtime & SDK |
| ASP.NET Core | 10.0 | Web API framework |
| Entity Framework Core | 10.0.2 | ORM + Code-First migrations |
| SQL Server | LocalDB / SQL Server | Relational database |
| JWT Bearer | 10.0.2 | Authentication |
| FluentValidation | 11.3.1 | Request validation |
| BCrypt.Net-Next | 4.0.3 | Password hashing |
| Swashbuckle | 8.1.1 | Swagger UI |

---

## 📁 Kiến trúc dự án

```
EduCycle.Api/
├── Controllers/               # API Controllers
│   └── AuthController.cs
├── Contracts/                 # Request / Response DTOs
│   └── Auth/
│       ├── LoginRequest.cs
│       └── RegisterRequest.cs
├── Domain/
│   ├── Entities/              # EF Core entities
│   │   ├── User.cs
│   │   ├── Product.cs
│   │   ├── Category.cs
│   │   ├── Transaction.cs
│   │   ├── Review.cs
│   │   └── Message.cs
│   └── Enums/
│       └── TransactionStatus.cs
├── Infrastructure/
│   └── Data/                  # ApplicationDbContext + Factory
│       ├── EduCycleDbContext.cs
│       └── EduCycleDbContextFactory.cs
└── Migrations/                # EF Core migrations
```

---

## 🚀 Cài đặt & Chạy

### Yêu cầu

- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- SQL Server hoặc LocalDB

### Các bước

```bash
# 1. Clone repository
git clone https://github.com/trhlow/educycle-backend.git
cd educycle-backend/EduCycle.Api

# 2. Cấu hình connection string (xem phần Cấu hình bên dưới)

# 3. Chạy migration
dotnet ef database update

# 4. Chạy ứng dụng
dotnet run
```

Swagger UI mặc định: **http://localhost:5171/swagger**

---

## ⚙ Cấu hình

File `appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\MSSQLLocalDB;Database=EduCycleDb;Trusted_Connection=True;TrustServerCertificate=True"
  },
  "Jwt": {
    "Key": "your-super-secret-key-at-least-32-characters",
    "Issuer": "EduCycle.Api",
    "Audience": "EduCycle.Client",
    "ExpireMinutes": 120
  }
}
```

---

## 📡 API Endpoints

### 🔑 Auth — `/api/auth`

| Method | Endpoint | Auth | Mô tả |
|---|---|---|---|
| POST | `/api/auth/register` | ❌ | Đăng ký tài khoản mới |
| POST | `/api/auth/login` | ❌ | Đăng nhập, nhận JWT token |

---

## 🔐 Authentication & Authorization

### JWT Flow

1. Gọi `POST /api/auth/login` → nhận `token`
2. Gửi token trong header: `Authorization: Bearer <token>`
3. Swagger UI hỗ trợ nhập token trực tiếp (nút 🔓 Authorize)

---

## 🗄 Database Schema

### Users

| Column | Type | Ghi chú |
|---|---|---|
| Id | int | PK |
| Email | nvarchar | Unique |
| FullName | nvarchar | |
| PasswordHash | nvarchar | BCrypt |
| AvgRating | double | Default 0 |

### Products

| Column | Type | Ghi chú |
|---|---|---|
| Id | int | PK |
| Title | nvarchar | |
| Description | nvarchar | |
| Price | decimal | |
| SellerId | int | FK → Users |
| CategoryId | int | FK → Categories |
| IsAvailable | bool | Default true |

### Categories

| Column | Type | Ghi chú |
|---|---|---|
| Id | int | PK, Identity |
| Name | nvarchar | |

### Transactions

| Column | Type | Ghi chú |
|---|---|---|
| Id | int | PK |
| ProductId | int | FK → Products |
| BuyerId | int | FK → Users (NoAction) |
| SellerId | int | FK → Users (NoAction) |
| Status | enum | Pending / Accepted / Completed / Cancelled |

### Reviews

| Column | Type | Ghi chú |
|---|---|---|
| Id | int | PK |
| ReviewerId | int | FK → Users (NoAction) |
| ProductId | int | FK → Products (Cascade) |
| Rating | int | |
| Content | nvarchar | |

### Messages

| Column | Type | Ghi chú |
|---|---|---|
| Id | int | PK |
| TransactionId | int | FK → Transactions (Cascade) |
| SenderId | int | FK → Users (NoAction) |
| Content | nvarchar | |
| CreatedAt | datetime | |

---

## 🌱 Seed Data

Migration tự động tạo dữ liệu mặc định:

### Danh mục mặc định

| Id | Name |
|---|---|
| 1 | Giáo trình đại cương |
| 2 | Chuyên ngành CNTT |
| 3 | Kinh tế - Quản trị |
| 4 | Ngoại ngữ |
| 5 | Khác |

---

## 📝 Development Rules

- Feature-based branching (`feature/xxx`, `fix/xxx`)
- Hoàn thành feature → commit → push → rồi mới làm feature tiếp
- Không commit trực tiếp vào `main`
- Merge vào `dev` trước khi lên `main`

---

## 📄 License

MIT