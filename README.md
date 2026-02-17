# ?? EduCycle Backend API

N?n t?ng trao ??i sách & tài li?u h?c t?p dành cho sinh viên.  
Backend REST API xây d?ng b?ng **ASP.NET Core (.NET 10)** theo ki?n trúc Clean Architecture.

---

## ?? M?c l?c

- [Tech Stack](#-tech-stack)
- [Ki?n trúc d? án](#-ki?n-trúc-d?-án)
- [Cài ??t & Ch?y](#-cài-??t--ch?y)
- [C?u hình](#-c?u-hình)
- [API Endpoints](#-api-endpoints)
- [Authentication & Authorization](#-authentication--authorization)
- [Admin — Duy?t s?n ph?m](#-admin--duy?t-s?n-ph?m)
- [Validation](#-validation)
- [Database Schema](#-database-schema)
- [Seed Data](#-seed-data)
- [Error Response Format](#-error-response-format)
- [Development Rules](#-development-rules)

---

## ?? Tech Stack

| Công ngh? | Version | Mô t? |
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

## ?? Ki?n trúc d? án

```
EduCycle.Api/
??? Controllers/               # API Controllers
?   ??? AuthController.cs
?   ??? ProductsController.cs
?   ??? CategoriesController.cs
?   ??? TransactionsController.cs
?   ??? ReviewsController.cs
?   ??? AdminController.cs
??? Application/
?   ??? Interfaces/            # Service interfaces
?   ??? Services/              # Business logic
?   ??? Validators/            # FluentValidation validators
??? Contracts/                 # Request / Response DTOs
?   ??? Auth/
?   ??? Products/
?   ??? Categories/
?   ??? Transactions/
?   ??? Reviews/
?   ??? Messages/
?   ??? Admin/
??? Domain/
?   ??? Entities/              # EF Core entities
?   ??? Enums/                 # Role, ProductStatus, TransactionStatus
??? Infrastructure/
?   ??? Data/                  # ApplicationDbContext
?   ??? Repositories/          # Data access layer
?   ??? Authentication/        # JWT token generator
??? Common/Extensions/         # Custom exceptions
??? Middleware/                 # Exception handling middleware
??? Migrations/                # EF Core migrations
```

---

## ?? Cài ??t & Ch?y

### Yêu c?u

- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- SQL Server ho?c LocalDB

### Các b??c

```bash
# 1. Clone repository
git clone https://github.com/trhlow/educycle-backend.git
cd educycle-backend/EduCycle.Api

# 2. C?u hình connection string (xem ph?n C?u hình bên d??i)

# 3. Ch?y migration
dotnet ef database update

# 4. Ch?y ?ng d?ng
dotnet run
```

Swagger UI m?c ??nh: **http://localhost:5000/swagger**

---

## ? C?u hình

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

## ?? API Endpoints

### ?? Auth — `/api/auth`

| Method | Endpoint | Auth | Mô t? |
|---|---|---|---|
| POST | `/api/auth/register` | ? | ??ng ký tài kho?n m?i |
| POST | `/api/auth/login` | ? | ??ng nh?p, nh?n JWT token |

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

### ?? Products — `/api/products`

| Method | Endpoint | Auth | Mô t? |
|---|---|---|---|
| GET | `/api/products` | ? | Danh sách s?n ph?m **?ã duy?t** (Approved) |
| GET | `/api/products/{id}` | ? | Chi ti?t s?n ph?m |
| POST | `/api/products` | ?? User | ??ng bán s?n ph?m (status = Pending) |
| PUT | `/api/products/{id}` | ?? Owner | C?p nh?t s?n ph?m (reset ? Pending) |
| DELETE | `/api/products/{id}` | ?? Owner | Xóa s?n ph?m |
| GET | `/api/products/mine` | ?? User | S?n ph?m c?a tôi (m?i status) |
| GET | `/api/products/pending` | ?? Admin | S?n ph?m ch? duy?t |
| GET | `/api/products/admin/all` | ?? Admin | T?t c? s?n ph?m (m?i status) |
| PATCH | `/api/products/{id}/approve` | ?? Admin | Duy?t s?n ph?m |
| PATCH | `/api/products/{id}/reject` | ?? Admin | T? ch?i s?n ph?m |

**Product Response:**
```json
{
  "id": "guid",
  "name": "Giáo trình Toán cao c?p",
  "description": "Sách m?i 95%...",
  "price": 50000,
  "imageUrl": "https://...",
  "imageUrls": ["https://...", "https://..."],
  "category": "Giáo Trình",
  "categoryName": "Giáo Trình",
  "condition": "Nh? m?i (95%)",
  "contactNote": "G?p t?i th? vi?n...",
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
  "name": "Giáo trình Toán cao c?p",
  "category": "Giáo Trình",
  "condition": "Nh? m?i (95%)",
  "price": 50000,
  "description": "Sách m?i 95%, không ghi chú...",
  "contactNote": "G?p t?i th? vi?n tr??ng",
  "imageUrl": "https://...",
  "imageUrls": ["https://...", "https://..."]
}
```

---

### ?? Categories — `/api/categories`

| Method | Endpoint | Auth | Mô t? |
|---|---|---|---|
| GET | `/api/categories` | ? | Danh sách danh m?c |
| GET | `/api/categories/{id}` | ? | Chi ti?t danh m?c |
| POST | `/api/categories` | ?? Admin | T?o danh m?c |
| PUT | `/api/categories/{id}` | ?? Admin | C?p nh?t danh m?c |
| DELETE | `/api/categories/{id}` | ?? Admin | Xóa danh m?c |

---

### ?? Transactions — `/api/transactions`

**Status flow:**
```
Pending ? Accepted ? Meeting ? Completed
       ? Rejected
       ? Cancelled
       ? AutoCompleted (timeout)
```

| Method | Endpoint | Auth | Mô t? |
|---|---|---|---|
| POST | `/api/transactions` | ?? User | T?o yêu c?u mua |
| GET | `/api/transactions` | ?? User | T?t c? giao d?ch |
| GET | `/api/transactions/mine` | ?? User | Giao d?ch c?a tôi (buyer ho?c seller) |
| GET | `/api/transactions/{id}` | ?? User | Chi ti?t giao d?ch |
| PATCH | `/api/transactions/{id}/status` | ?? User | C?p nh?t tr?ng thái |
| POST | `/api/transactions/{id}/otp` | ?? User | T?o mã OTP xác nh?n g?p m?t |
| POST | `/api/transactions/{id}/verify-otp` | ?? User | Xác minh mã OTP |
| POST | `/api/transactions/{id}/confirm` | ?? User | Xác nh?n ?ã nh?n hàng |

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

### ?? Messages — `/api/transactions/{id}/messages`

| Method | Endpoint | Auth | Mô t? |
|---|---|---|---|
| GET | `/api/transactions/{id}/messages` | ?? User | L?ch s? chat c?a giao d?ch |
| POST | `/api/transactions/{id}/messages` | ?? User | G?i tin nh?n |

**Message Response:**
```json
{
  "id": "guid",
  "transactionId": "guid",
  "senderId": "guid",
  "senderName": "nguyenvana",
  "content": "B?n ?i mai g?p ? th? vi?n nhé!",
  "createdAt": "2025-01-01T00:00:00Z"
}
```

---

### ? Reviews — `/api/reviews`

| Method | Endpoint | Auth | Mô t? |
|---|---|---|---|
| POST | `/api/reviews` | ?? User | Vi?t ?ánh giá |
| GET | `/api/reviews` | ? | T?t c? ?ánh giá |
| GET | `/api/reviews/{id}` | ?? User | Chi ti?t ?ánh giá |
| GET | `/api/reviews/product/{productId}` | ? | ?ánh giá theo s?n ph?m |
| GET | `/api/reviews/transaction/{transactionId}` | ? | ?ánh giá theo giao d?ch |
| DELETE | `/api/reviews/{id}` | ?? Owner | Xóa ?ánh giá |

---

### ?? Admin — `/api/admin`

| Method | Endpoint | Auth | Mô t? |
|---|---|---|---|
| GET | `/api/admin/stats` | ?? Admin | Dashboard th?ng kê t?ng quan |
| GET | `/api/admin/users` | ?? Admin | Danh sách t?t c? ng??i dùng |

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

## ?? Authentication & Authorization

### JWT Flow

1. G?i `POST /api/auth/login` ? nh?n `token`
2. G?i token trong header: `Authorization: Bearer <token>`
3. Swagger UI h? tr? nh?p token tr?c ti?p (nút ?? Authorize)

### Roles

| Role | Quy?n |
|---|---|
| **User** | ??ng bán s?n ph?m, mua hàng, chat, ?ánh giá, qu?n lý s?n ph?m / giao d?ch c?a mình |
| **Admin** | Toàn b? quy?n User + Duy?t / t? ch?i s?n ph?m + CRUD danh m?c + Xem th?ng kê + Qu?n lý ng??i dùng |

---

## ?? Admin — Duy?t s?n ph?m

### Lu?ng ho?t ??ng

```
User ??ng s?n ph?m ??? Status: Pending
                            ?
                    Admin xem /products/pending
                            ?
                   ???????????????????
                   ?                 ?
          PATCH /approve       PATCH /reject
          Status: Approved     Status: Rejected
                   ?
                   ?
        Hi?n th? trên GET /products (public)
```

- **User ??ng bán** ? s?n ph?m có status `Pending`
- **Ch? s?n ph?m `Approved`** m?i hi?n th? cho public (`GET /api/products`)
- **Admin duy?t** ? `PATCH /api/products/{id}/approve`
- **Admin t? ch?i** ? `PATCH /api/products/{id}/reject`
- **User s?a s?n ph?m** ? status reset v? `Pending` (c?n duy?t l?i)

---

## ? Validation

S? d?ng **FluentValidation** t? ??ng validate request:

| Request | Rules |
|---|---|
| `RegisterRequest` | Username 3–50 ký t?, Email h?p l?, Password 6–100 ký t? |
| `LoginRequest` | Email h?p l?, Password b?t bu?c |
| `CreateProductRequest` | Name b?t bu?c (5–150 ký t?), Price > 0 & ? 10.000.000, Description ? 20 ký t? |
| `UpdateProductRequest` | Name b?t bu?c (5–150 ký t?), Price > 0 & ? 10.000.000, Description ? 20 ký t? |
| `CreateCategoryRequest` | Name b?t bu?c (max 100) |
| `CreateReviewRequest` | ProductId b?t bu?c, Rating 1–5, Content b?t bu?c (max 1000) |
| `CreateTransactionRequest` | ProductId b?t bu?c, SellerId b?t bu?c, Amount > 0 |
| `UpdateTransactionStatusRequest` | Status h?p l? |

---

## ?? Database Schema

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
| Category | nvarchar | Tên danh m?c (string) |
| Condition | nvarchar | Tình tr?ng sách |
| ContactNote | nvarchar | Ghi chú liên h? |
| CategoryId | int | FK ? Categories, Nullable |
| UserId | GUID | FK ? Users |
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
| ProductId | GUID | FK ? Products (NoAction) |
| BuyerId | GUID | FK ? Users (NoAction) |
| SellerId | GUID | FK ? Users (NoAction) |
| Amount | decimal(18,2) | |
| Status | nvarchar(20) | Pending / Accepted / Meeting / Completed / AutoCompleted / Rejected / Cancelled / Disputed |
| OtpCode | nvarchar | Nullable |
| OtpExpiresAt | datetime | Nullable |
| CreatedAt | datetime | |

### Reviews

| Column | Type | Ghi chú |
|---|---|---|
| Id | GUID | PK |
| UserId | GUID | FK ? Users (NoAction) |
| ProductId | GUID | FK ? Products (Cascade) |
| Rating | int | 1–5 |
| Content | nvarchar | |
| CreatedAt | datetime | |

### Messages

| Column | Type | Ghi chú |
|---|---|---|
| Id | GUID | PK |
| TransactionId | GUID | FK ? Transactions (Cascade) |
| SenderId | GUID | FK ? Users (NoAction) |
| Content | nvarchar | |
| CreatedAt | datetime | |

---

## ?? Seed Data

Migration t? ??ng t?o d? li?u m?c ??nh:

### Tài kho?n Admin

| Field | Value |
|---|---|
| Email | `admin@educycle.com` |
| Password | `admin@1` |
| Role | Admin |

### Danh m?c m?c ??nh

| Id | Name |
|---|---|
| 1 | Giáo Trình |
| 2 | Sách Chuyên Ngành |
| 3 | Tài Li?u Ôn Thi |
| 4 | D?ng C? H?c T?p |
| 5 | Ngo?i Ng? |
| 6 | Khác |

---

## ?? Error Response Format

T?t c? l?i tr? v? cùng format:

```json
{
  "success": false,
  "message": "Error description",
  "errors": []
}
```

| HTTP Code | Exception | Khi nào |
|---|---|---|
| 400 | `BadRequestException` | Request không h?p l? |
| 400 | `ValidationException` | FluentValidation fail |
| 401 | `UnauthorizedException` | Sai credentials / không có quy?n |
| 404 | `NotFoundException` | Không tìm th?y resource |
| 500 | `Exception` | L?i server không xác ??nh |

---

## ?? Development Rules

- Feature-based branching (`feature/xxx`, `fix/xxx`)
- Hoàn thành feature ? commit ? push ? r?i m?i làm feature ti?p
- Không commit tr?c ti?p vào `main`
- Merge vào `dev` tr??c khi lên `main`

---

## ?? Frontend ? Backend API Mapping

| Frontend (endpoints.js) | Backend API | Status |
|---|---|---|
| `authApi.register(data)` | `POST /api/auth/register` | ? |
| `authApi.login(data)` | `POST /api/auth/login` | ? |
| `productsApi.getAll(params)` | `GET /api/products` | ? |
| `productsApi.getById(id)` | `GET /api/products/{id}` | ? |
| `productsApi.create(data)` | `POST /api/products` | ? |
| `productsApi.update(id, data)` | `PUT /api/products/{id}` | ? |
| `productsApi.delete(id)` | `DELETE /api/products/{id}` | ? |
| `productsApi.getMyProducts()` | `GET /api/products/mine` | ? |
| `categoriesApi.getAll()` | `GET /api/categories` | ? |
| `categoriesApi.create(data)` | `POST /api/categories` | ? |
| `categoriesApi.update(id, data)` | `PUT /api/categories/{id}` | ? |
| `categoriesApi.delete(id)` | `DELETE /api/categories/{id}` | ? |
| `transactionsApi.getAll()` | `GET /api/transactions` | ? |
| `transactionsApi.getMyTransactions()` | `GET /api/transactions/mine` | ? |
| `transactionsApi.getById(id)` | `GET /api/transactions/{id}` | ? |
| `transactionsApi.create(data)` | `POST /api/transactions` | ? |
| `transactionsApi.updateStatus(id, data)` | `PATCH /api/transactions/{id}/status` | ? |
| `transactionsApi.generateOtp(id)` | `POST /api/transactions/{id}/otp` | ? |
| `transactionsApi.verifyOtp(id, data)` | `POST /api/transactions/{id}/verify-otp` | ? |
| `transactionsApi.confirmReceipt(id)` | `POST /api/transactions/{id}/confirm` | ? |
| `messagesApi.getByTransaction(id)` | `GET /api/transactions/{id}/messages` | ? |
| `messagesApi.send(id, data)` | `POST /api/transactions/{id}/messages` | ? |
| `reviewsApi.getAll()` | `GET /api/reviews` | ? |
| `reviewsApi.create(data)` | `POST /api/reviews` | ? |
| `reviewsApi.delete(id)` | `DELETE /api/reviews/{id}` | ? |
| `reviewsApi.getByTransaction(id)` | `GET /api/reviews/transaction/{id}` | ? |
| `reviewsApi.getByProduct(id)` | `GET /api/reviews/product/{id}` | ? |

---

## ?? License

MIT
