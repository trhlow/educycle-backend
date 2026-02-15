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
- [Admin — Duyệt sản phẩm](#-admin--duyệt-sản-phẩm)
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
│   ├── AuthController.cs
│   ├── ProductsController.cs
│   ├── CategoriesController.cs
│   ├── TransactionsController.cs
│   ├── ReviewsController.cs
│   └── AdminController.cs
├── Application/
│   ├── Interfaces/            # Service interfaces
│   ├── Services/              # Business logic
│   └── Validators/            # FluentValidation validators
├── Contracts/                 # Request / Response DTOs
│   ├── Auth/
│   ├── Products/
│   ├── Categories/
│   ├── Transactions/
│   ├── Reviews/
│   ├── Messages/
│   └── Admin/
├── Domain/
│   ├── Entities/              # EF Core entities
│   └── Enums/                 # Role, ProductStatus, TransactionStatus
├── Infrastructure/
│   ├── Data/                  # ApplicationDbContext
│   ├── Repositories/          # Data access layer
│   └── Authentication/        # JWT token generator
├── Common/Extensions/         # Custom exceptions
├── Middleware/                 # Exception handling middleware
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

Swagger UI mặc định: **http://localhost:5000/swagger**

---

## ⚙ Cấu hình

File `appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=EduCycleDb;Trusted_Connection=True;"
  },
  "Jwt": {
    "Key": "your-super-secret-key-at-least-32-characters",
    "Issuer": "EduCycle",
    "Audience": "EduCycle"
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

**Login Response:**
```json
{
  "userId": "guid",
  "username": "string",
  "email": "string",
  "token": "jwt-token",
  "role": "User | Admin"
}
```

---

### 📦 Products — `/api/products`

| Method | Endpoint | Auth | Mô tả |
|---|---|---|---|
| GET | `/api/products` | ❌ | Danh sách sản phẩm **đã duyệt** (Approved) |
| GET | `/api/products/{id}` | ❌ | Chi tiết sản phẩm |
| POST | `/api/products` | 🔒 User | Đăng bán sản phẩm (status = Pending) |
| PUT | `/api/products/{id}` | 🔒 Owner | Cập nhật sản phẩm (reset → Pending) |
| DELETE | `/api/products/{id}` | 🔒 Owner | Xóa sản phẩm |
| GET | `/api/products/mine` | 🔒 User | Sản phẩm của tôi (mọi status) |
| GET | `/api/products/pending` | 🔒 Admin | Sản phẩm chờ duyệt |
| GET | `/api/products/admin/all` | 🔒 Admin | Tất cả sản phẩm (mọi status) |
| PATCH | `/api/products/{id}/approve` | 🔒 Admin | Duyệt sản phẩm |
| PATCH | `/api/products/{id}/reject` | 🔒 Admin | Từ chối sản phẩm |

**Product Response:**
```json
{
  "id": "guid",
  "name": "Giáo trình Toán cao cấp",
  "description": "Sách mới 95%...",
  "price": 50000,
  "imageUrl": "https://...",
  "imageUrls": ["https://...", "https://..."],
  "category": "Giáo Trình",
  "categoryName": "Giáo Trình",
  "condition": "Như mới (95%)",
  "contactNote": "Gặp tại thư viện...",
  "sellerId": "guid",
  "sellerName": "nguyenvana",
  "status": "Pending | Approved | Rejected",
  "averageRating": 4.5,
  "reviewCount": 12,
  "createdAt": "2025-01-01T00:00:00Z"
}
```

**Create Product Request:**
```json
{
  "name": "Giáo trình Toán cao cấp",
  "category": "Giáo Trình",
  "condition": "Như mới (95%)",
  "price": 50000,
  "description": "Sách mới 95%, không ghi chú...",
  "contactNote": "Gặp tại thư viện trường",
  "imageUrl": "https://...",
  "imageUrls": ["https://...", "https://..."],
}
```

---

### 🏷 Categories — `/api/categories`

| Method | Endpoint | Auth | Mô tả |
|---|---|---|---|
| GET | `/api/categories` | ❌ | Danh sách danh mục |
| GET | `/api/categories/{id}` | ❌ | Chi tiết danh mục |
| POST | `/api/categories` | 🔒 Admin | Tạo danh mục |
| PUT | `/api/categories/{id}` | 🔒 Admin | Cập nhật danh mục |
| DELETE | `/api/categories/{id}` | 🔒 Admin | Xóa danh mục |

---

### 🔄 Transactions — `/api/transactions`

**Status flow:**
```
Pending → Accepted → Meeting → Completed
       → Rejected
       → Cancelled
       → AutoCompleted (timeout)
```

| Method | Endpoint | Auth | Mô tả |
|---|---|---|---|
| POST | `/api/transactions` | 🔒 User | Tạo yêu cầu mua |
| GET | `/api/transactions` | 🔒 User | Tất cả giao dịch |
| GET | `/api/transactions/mine` | 🔒 User | Giao dịch của tôi (buyer hoặc seller) |
| GET | `/api/transactions/{id}` | 🔒 User | Chi tiết giao dịch |
| PATCH | `/api/transactions/{id}/status` | 🔒 User | Cập nhật trạng thái |
| POST | `/api/transactions/{id}/otp` | 🔒 User | Tạo mã OTP xác nhận gặp mặt |
| POST | `/api/transactions/{id}/verify-otp` | 🔒 User | Xác minh mã OTP |
| POST | `/api/transactions/{id}/confirm` | 🔒 User | Xác nhận đã nhận hàng |

**Transaction Response:**
```json
{
  "id": "guid",
  "buyer": { "id": "guid", "username": "buyer1", "email": "..." },
  "seller": { "id": "guid", "username": "seller1", "email": "..." },
  "product": { "id": "guid", "name": "Sách ABC", "price": 50000, "imageUrl": "..." },
  "amount": 50000,
  "status": "Pending",
  "createdAt": "2025-01-01T00:00:00Z"
}
```

**Create Transaction Request:**
```json
{
  "productId": "guid",
  "sellerId": "guid",
  "amount": 50000
}
```

---

### 💬 Messages — `/api/transactions/{id}/messages`

| Method | Endpoint | Auth | Mô tả |
|---|---|---|---|
| GET | `/api/transactions/{id}/messages` | 🔒 User | Lịch sử chat của giao dịch |
| POST | `/api/transactions/{id}/messages` | 🔒 User | Gửi tin nhắn |

**Message Response:**
```json
{
  "id": "guid",
  "transactionId": "guid",
  "senderId": "guid",
  "senderName": "nguyenvana",
  "content": "Bạn ơi mai gặp ở thư viện nhé!",
  "createdAt": "2025-01-01T00:00:00Z"
}
```

---

### ⭐ Reviews — `/api/reviews`

| Method | Endpoint | Auth | Mô tả |
|---|---|---|---|
| POST | `/api/reviews` | 🔒 User | Viết đánh giá |
| GET | `/api/reviews` | ❌ | Tất cả đánh giá |
| GET | `/api/reviews/{id}` | 🔒 User | Chi tiết đánh giá |
| GET | `/api/reviews/product/{productId}` | ❌ | Đánh giá theo sản phẩm |
| GET | `/api/reviews/transaction/{transactionId}` | ❌ | Đánh giá theo giao dịch |
| DELETE | `/api/reviews/{id}` | 🔒 Owner | Xóa đánh giá |

---

### 🛡 Admin — `/api/admin`

| Method | Endpoint | Auth | Mô tả |
|---|---|---|---|
| GET | `/api/admin/stats` | 🔒 Admin | Dashboard thống kê tổng quan |
| GET | `/api/admin/users` | 🔒 Admin | Danh sách tất cả người dùng |

**Dashboard Stats Response:**
```json
{
  "totalUsers": 150,
  "totalProducts": 320,
  "pendingProducts": 12,
  "totalTransactions": 89,
  "totalRevenue": 4500000
}
```

---

## 🔐 Authentication & Authorization

### JWT Flow

1. Gọi `POST /api/auth/login` → nhận `token`
2. Gửi token trong header: `Authorization: Bearer <token>`
3. Swagger UI hỗ trợ nhập token trực tiếp (nút 🔓 Authorize)

### Roles

| Role | Quyền |
|---|---|
| **User** | Đăng bán sản phẩm, mua hàng, chat, đánh giá, quản lý sản phẩm / giao dịch của mình |
| **Admin** | Toàn bộ quyền User + Duyệt / từ chối sản phẩm + CRUD danh mục + Xem thống kê + Quản lý người dùng |

---

## 🛡 Admin — Duyệt sản phẩm

### Luồng hoạt động

```
User đăng sản phẩm ──→ Status: Pending
                            │
                    Admin xem /products/pending
                            │
                   ┌────────┴────────┐
                   │                 │
          PATCH /approve       PATCH /reject
          Status: Approved     Status: Rejected
                   │
                   ▼
        Hiển thị trên GET /products (public)
```

- **User đăng bán** → sản phẩm có status `Pending`
- **Chỉ sản phẩm `Approved`** mới hiển thị cho public (`GET /api/products`)
- **Admin duyệt** → `PATCH /api/products/{id}/approve`
- **Admin từ chối** → `PATCH /api/products/{id}/reject`
- **User sửa sản phẩm** → status reset về `Pending` (cần duyệt lại)

---

## ✅ Validation

Sử dụng **FluentValidation** tự động validate request:

| Request | Rules |
|---|---|
| `RegisterRequest` | Username 3–50 ký tự, Email hợp lệ, Password 6–100 ký tự |
| `LoginRequest` | Email hợp lệ, Password bắt buộc |
| `CreateProductRequest` | Name bắt buộc (5–150 ký tự), Price > 0 & ≤ 10.000.000, Description ≥ 20 ký tự |
| `UpdateProductRequest` | Name bắt buộc (5–150 ký tự), Price > 0 & ≤ 10.000.000, Description ≥ 20 ký tự |
| `CreateCategoryRequest` | Name bắt buộc (max 100) |
| `CreateReviewRequest` | ProductId bắt buộc, Rating 1–5, Content bắt buộc (max 1000) |
| `CreateTransactionRequest` | ProductId bắt buộc, SellerId bắt buộc, Amount > 0 |
| `UpdateTransactionStatusRequest` | Status hợp lệ |

---

## 🗄 Database Schema

### Users

| Column | Type | Ghi chú |
|---|---|---|
| Id | GUID | PK |
| Username | nvarchar | |
| Email | nvarchar | Unique |
| PasswordHash | nvarchar | BCrypt |
| Role | nvarchar(20) | User / Admin |
| Avatar | nvarchar | Nullable |
| Bio | nvarchar | Nullable |
| CreatedAt | datetime | |

### Products

| Column | Type | Ghi chú |
|---|---|---|
| Id | GUID | PK |
| Name | nvarchar | |
| Description | nvarchar | Nullable |
| Price | decimal(18,2) | |
| ImageUrl | nvarchar | Nullable |
| ImageUrls | nvarchar | JSON array, Nullable |
| Category | nvarchar | Tên danh mục (string) |
| Condition | nvarchar | Tình trạng sách |
| ContactNote | nvarchar | Ghi chú liên hệ |
| CategoryId | int | FK → Categories, Nullable |
| UserId | GUID | FK → Users |
| Status | nvarchar(20) | Pending / Approved / Rejected |
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
| ProductId | GUID | FK → Products (NoAction) |
| BuyerId | GUID | FK → Users (NoAction) |
| SellerId | GUID | FK → Users (NoAction) |
| Amount | decimal(18,2) | |
| Status | nvarchar(20) | Pending / Accepted / Meeting / Completed / AutoCompleted / Rejected / Cancelled / Disputed |
| OtpCode | nvarchar | Nullable |
| OtpExpiresAt | datetime | Nullable |
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

### Messages

| Column | Type | Ghi chú |
|---|---|---|
| Id | GUID | PK |
| TransactionId | GUID | FK → Transactions (Cascade) |
| SenderId | GUID | FK → Users (NoAction) |
| Content | nvarchar | |
| CreatedAt | datetime | |

---

## 🌱 Seed Data

Migration tự động tạo dữ liệu mặc định:

### Tài khoản Admin

| Field | Value |
|---|---|
| Email | `admin@educycle.com` |
| Password | `admin@admin` |
| Role | Admin |

### Danh mục mặc định

| Id | Name |
|---|---|
| 1 | Giáo Trình |
| 2 | Sách Chuyên Ngành |
| 3 | Tài Liệu Ôn Thi |
| 4 | Dụng Cụ Học Tập |
| 5 | Ngoại Ngữ |
| 6 | Khác |

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

## 🔗 Frontend → Backend API Mapping

| Frontend (endpoints.js) | Backend API | Status |
|---|---|---|
| `authApi.register(data)` | `POST /api/auth/register` | ✅ |
| `authApi.login(data)` | `POST /api/auth/login` | ✅ |
| `productsApi.getAll(params)` | `GET /api/products` | ✅ |
| `productsApi.getById(id)` | `GET /api/products/{id}` | ✅ |
| `productsApi.create(data)` | `POST /api/products` | ✅ |
| `productsApi.update(id, data)` | `PUT /api/products/{id}` | ✅ |
| `productsApi.delete(id)` | `DELETE /api/products/{id}` | ✅ |
| `productsApi.getMyProducts()` | `GET /api/products/mine` | ✅ |
| `categoriesApi.getAll()` | `GET /api/categories` | ✅ |
| `categoriesApi.create(data)` | `POST /api/categories` | ✅ |
| `categoriesApi.update(id, data)` | `PUT /api/categories/{id}` | ✅ |
| `categoriesApi.delete(id)` | `DELETE /api/categories/{id}` | ✅ |
| `transactionsApi.getAll()` | `GET /api/transactions` | ✅ |
| `transactionsApi.getMyTransactions()` | `GET /api/transactions/mine` | ✅ |
| `transactionsApi.getById(id)` | `GET /api/transactions/{id}` | ✅ |
| `transactionsApi.create(data)` | `POST /api/transactions` | ✅ |
| `transactionsApi.updateStatus(id, data)` | `PATCH /api/transactions/{id}/status` | ✅ |
| `transactionsApi.generateOtp(id)` | `POST /api/transactions/{id}/otp` | ✅ |
| `transactionsApi.verifyOtp(id, data)` | `POST /api/transactions/{id}/verify-otp` | ✅ |
| `transactionsApi.confirmReceipt(id)` | `POST /api/transactions/{id}/confirm` | ✅ |
| `messagesApi.getByTransaction(id)` | `GET /api/transactions/{id}/messages` | ✅ |
| `messagesApi.send(id, data)` | `POST /api/transactions/{id}/messages` | ✅ |
| `reviewsApi.getAll()` | `GET /api/reviews` | ✅ |
| `reviewsApi.create(data)` | `POST /api/reviews` | ✅ |
| `reviewsApi.delete(id)` | `DELETE /api/reviews/{id}` | ✅ |
| `reviewsApi.getByTransaction(id)` | `GET /api/reviews/transaction/{id}` | ✅ |
| `reviewsApi.getByProduct(id)` | `GET /api/reviews/product/{id}` | ✅ |

---

## 📝 Development Rules

- Feature-based branching (`feature/xxx`, `fix/xxx`)
- Hoàn thành feature → commit → push → rồi mới làm feature tiếp
- Không commit trực tiếp vào `main`
- Merge vào `dev` trước khi lên `main`

---

## 📄 License

MIT
