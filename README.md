# 🎓 EduCycle Backend API

Nền tảng trao đổi tài liệu / sản phẩm học tập dành cho sinh viên.  
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
- [Unit Tests](#-unit-tests)
- [Seed Data](#-seed-data)
- [Error Response Format](#-error-response-format)
- [Development Rules](#-development-rules)

---

## 🛠 Tech Stack

| Công nghệ | Version |
|---|---|
| .NET | 10.0 |
| Entity Framework Core | 10.0.2 |
| SQL Server | LocalDB / SQL Server |
| JWT Bearer Authentication | 10.0.2 |
| FluentValidation | 11.3.1 |
| BCrypt.Net-Next | 4.0.3 |
| Swashbuckle (Swagger) | 8.1.1 |
| xUnit + Moq | Testing |

---

## 📁 Kiến trúc dự án

```
EduCycle.Api/
├── Controllers/             # API Controllers
│   ├── AuthController.cs
│   ├── ProductsController.cs
│   ├── CategoriesController.cs
│   ├── TransactionsController.cs
│   └── ReviewsController.cs
├── Application/
│   ├── Interfaces/          # Service interfaces
│   ├── Services/            # Business logic
│   └── Validators/          # FluentValidation validators
├── Contracts/               # Request / Response DTOs
│   ├── Auth/
│   ├── Products/
│   ├── Categories/
│   ├── Transactions/
│   └── Reviews/
├── Domain/
│   ├── Entities/            # EF Core entities
│   └── Enums/               # Role, TransactionStatus
├── Infrastructure/
│   ├── Authentication/      # JWT token generator
│   ├── Data/                # DbContext + Seed
│   └── Repositories/        # Data access layer
├── Common/Extensions/       # Custom exceptions
├── Middleware/               # Global exception handling
└── Migrations/              # EF Core migrations

EduCycle.Tests/
└── Services/                # Unit tests (Auth + Product)
```

---

## 🚀 Cài đặt & Chạy

### Yêu cầu

- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- SQL Server (LocalDB hoặc SQL Server)

### Bước 1 – Clone

```bash
git clone https://github.com/trhlow/educycle-backend.git
cd educycle-backend/EduCycle.Api
```

### Bước 2 – Cấu hình Connection String

Chỉnh `appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=.;Database=EduCycleDb;Trusted_Connection=True;TrustServerCertificate=True"
  }
}
```

### Bước 3 – Chạy Migration

```bash
dotnet ef database update
```

### Bước 4 – Chạy API

```bash
dotnet run
```

Truy cập Swagger UI: **https://localhost:{port}/swagger**

---

## ⚙ Cấu hình

`appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=.;Database=EduCycleDb;Trusted_Connection=True;TrustServerCertificate=True"
  },
  "Jwt": {
    "Key": "YOUR_SECRET_KEY_AT_LEAST_32_CHARS",
    "Issuer": "EduCycle",
    "Audience": "EduCycleUsers"
  }
}
```

---

## 📡 API Endpoints

### Auth

| Method | Endpoint | Auth | Mô tả |
|---|---|---|---|
| POST | `/api/auth/register` | ❌ | Đăng ký tài khoản |
| POST | `/api/auth/login` | ❌ | Đăng nhập, trả về JWT |

### Products

| Method | Endpoint | Auth | Mô tả |
|---|---|---|---|
| POST | `/api/products` | 🔒 User | Tạo sản phẩm |
| GET | `/api/products` | ❌ | Danh sách tất cả sản phẩm |
| GET | `/api/products/{id}` | ❌ | Chi tiết sản phẩm |
| PUT | `/api/products/{id}` | 🔒 Owner | Cập nhật sản phẩm (chủ sở hữu) |
| DELETE | `/api/products/{id}` | 🔒 Owner | Xóa sản phẩm (chủ sở hữu) |

### Categories

| Method | Endpoint | Auth | Mô tả |
|---|---|---|---|
| POST | `/api/categories` | 🔒 Admin | Tạo danh mục |
| GET | `/api/categories` | ❌ | Danh sách danh mục |
| GET | `/api/categories/{id}` | ❌ | Chi tiết danh mục |
| PUT | `/api/categories/{id}` | 🔒 Admin | Cập nhật danh mục |
| DELETE | `/api/categories/{id}` | 🔒 Admin | Xóa danh mục |

### Transactions

| Method | Endpoint | Auth | Mô tả |
|---|---|---|---|
| POST | `/api/transactions` | 🔒 User | Tạo giao dịch |
| GET | `/api/transactions` | 🔒 User | Danh sách giao dịch |
| GET | `/api/transactions/{id}` | 🔒 User | Chi tiết giao dịch |
| PATCH | `/api/transactions/{id}/status` | 🔒 User | Cập nhật trạng thái |

**Transaction Status:** `Pending` → `Accepted` → `Completed` | `Cancelled`

### Reviews

| Method | Endpoint | Auth | Mô tả |
|---|---|---|---|
| POST | `/api/reviews` | 🔒 User | Viết đánh giá |
| GET | `/api/reviews` | ❌ | Danh sách đánh giá |
| GET | `/api/reviews/{id}` | 🔒 User | Chi tiết đánh giá |
| DELETE | `/api/reviews/{id}` | 🔒 Owner | Xóa đánh giá (chủ sở hữu) |

### Health Check

| Method | Endpoint | Mô tả |
|---|---|---|
| GET | `/health` | Kiểm tra API hoạt động |

---

## 🔐 Authentication & Authorization

### JWT Flow

1. Gọi `POST /api/auth/login` → nhận `token`
2. Gửi token trong header: `Authorization: Bearer <token>`
3. Swagger UI hỗ trợ nhập token trực tiếp (nút 🔓 Authorize)

### Roles

| Role | Quyền |
|---|---|
| **User** | CRUD sản phẩm / review của mình, tạo giao dịch |
| **Admin** | Toàn bộ quyền User + CRUD danh mục |

### Policies

- `AdminOnly` – Chỉ Admin
- `UserOrAdmin` – User hoặc Admin

---

## ✅ Validation

Sử dụng **FluentValidation** tự động validate request:

| Request | Rules |
|---|---|
| `RegisterRequest` | Username 3–50 ký tự, Email hợp lệ, Password 6–100 ký tự |
| `LoginRequest` | Email hợp lệ, Password bắt buộc |
| `CreateProductRequest` | Name bắt buộc (max 200), Price > 0 |
| `UpdateProductRequest` | Name bắt buộc (max 200), Price > 0 |
| `CreateCategoryRequest` | Name bắt buộc (max 100) |
| `CreateReviewRequest` | ProductId bắt buộc, Rating 1–5, Content bắt buộc (max 1000) |
| `CreateTransactionRequest` | SellerId bắt buộc, Amount > 0 |
| `UpdateTransactionStatusRequest` | Status: Pending / Accepted / Completed / Cancelled |

---

## 🗄 Database Schema

### Users

| Column | Type | Ghi chú |
|---|---|---|
| Id | GUID | PK |
| Username | nvarchar | |
| Email | nvarchar | |
| PasswordHash | nvarchar | BCrypt |
| Role | nvarchar(20) | User / Admin |
| CreatedAt | datetime | |

### Products

| Column | Type | Ghi chú |
|---|---|---|
| Id | GUID | PK |
| Name | nvarchar | |
| Description | nvarchar | Nullable |
| Price | decimal(18,2) | |
| ImageUrl | nvarchar | Nullable |
| CategoryId | int | FK → Categories, Nullable, SetNull on delete |
| UserId | GUID | FK → Users |
| CreatedAt | datetime | |

### Categories

| Column | Type | Ghi chú |
|---|---|---|
| Id | int | PK, Identity |
| Name | nvarchar | |

### Transactions

| Column | Type | Ghi chú |
|---|---|---|
| Id | GUID | PK |
| BuyerId | GUID | FK → Users (NoAction) |
| SellerId | GUID | FK → Users (NoAction) |
| Amount | decimal(18,2) | |
| Status | nvarchar(20) | Pending / Accepted / Completed / Cancelled |
| CreatedAt | datetime | |

### Reviews

| Column | Type | Ghi chú |
|---|---|---|
| Id | GUID | PK |
| UserId | GUID | FK → Users (NoAction) |
| ProductId | GUID | FK → Products (Cascade) |
| Rating | int | 1–5 |
| Content | nvarchar | |
| CreatedAt | datetime | |

---

## 🧪 Unit Tests

```bash
cd EduCycle.Tests
dotnet test
```

**16 tests** – tất cả pass:

| Test Class | Tests | Mô tả |
|---|---|---|
| `AuthServiceTests` | 5 | Register OK, email trùng, login đúng / sai, user not found |
| `ProductServiceTests` | 11 | Create, GetById (found / not found), GetAll (có / rỗng), Update (owner / not owner / not found), Delete (owner / not owner / not found) |

---

## 🌱 Seed Data

Migration tự động tạo tài khoản Admin:

| Field | Value |
|---|---|
| Email | `admin@educycle.com` |
| Password | `Admin@123` |
| Role | Admin |

---

## 🔧 Error Response Format

Tất cả lỗi trả về cùng format:

```json
{
  "success": false,
  "message": "Error description",
  "errors": []
}
```

| HTTP Code | Exception | Khi nào |
|---|---|---|
| 400 | `BadRequestException` | Request không hợp lệ |
| 400 | `ValidationException` | FluentValidation fail |
| 401 | `UnauthorizedException` | Sai credentials / không có quyền |
| 404 | `NotFoundException` | Không tìm thấy resource |
| 500 | `Exception` | Lỗi server không xác định |

---

## 📝 Development Rules

- Feature-based branching (`feature/xxx`, `fix/xxx`)
- Hoàn thành feature → commit → push → rồi mới làm feature tiếp
- Không commit trực tiếp vào `main`
- Merge vào `dev` trước khi lên `main`

---

## 📄 License

MIT
