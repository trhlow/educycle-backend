# Hướng dẫn áp dụng thay đổi

Tài liệu này mô tả các thay đổi gần đây và cách áp dụng chúng vào dự án EduCycle Backend.

---

## Tóm tắt thay đổi

| Mục | Trước | Sau |
|-----|-------|-----|
| **Tài khoản Admin - Email** | `admin@educycle.com` | `admin@educycle.com` (không đổi) |
| **Tài khoản Admin - Mật khẩu** | `admin@admin` | `admin@1` |
| **PasswordHash** | Hash cũ | Hash mới (BCrypt cho `admin@1`) |

---

## Các file đã thay đổi

- `EduCycle.Api/Infrastructure/Data/ApplicationDbContext.cs` — Cập nhật seed admin với password hash mới
- `EduCycle.Api/Migrations/20260217150000_UpdateAdminPassword.cs` — Migration cập nhật password trong database
- `EduCycle.Api/Helpers/PasswordHashGenerator.cs` — Helper tool để tạo BCrypt hash (phục vụ dev, không dùng trong runtime)

---

## Các bước áp dụng thay đổi

### 1. Đảm bảo môi trường sẵn sàng

- Đã cài đặt [.NET 10 SDK](https://dotnet.microsoft.com/download)
- SQL Server / LocalDB đang chạy
- Đã cấu hình connection string trong `appsettings.json`

### 2. Chạy migration

Từ thư mục `EduCycle.Api`:

```bash
dotnet ef database update
```

Hoặc nếu đang ở thư mục gốc dự án:

```bash
cd EduCycle.Api
dotnet ef database update
```

### 3. Xác nhận migration thành công

- Migration `UpdateAdminPassword` sẽ cập nhật `PasswordHash` của user admin (Id: `00000000-0000-0000-0000-000000000001`) trong bảng `Users`
- Nếu database mới (chưa có dữ liệu cũ), seed data trong `ApplicationDbContext` đã bao gồm password mới

### 4. Đăng nhập với tài khoản Admin mới

| Field | Giá trị |
|-------|---------|
| **Email** | `admin@educycle.com` |
| **Password** | `admin@1` |

**Lưu ý:** Đổi mật khẩu admin sau lần đăng nhập đầu tiên trong môi trường production.

---

## Rollback (nếu cần)

Để hoàn tác migration và quay về password cũ:

```bash
dotnet ef database update 20260217000000_<TênMigrationTrướcUpdateAdminPassword>
```

Hoặc kiểm tra danh sách migration:

```bash
dotnet ef migrations list
```

---

## Cập nhật tài liệu khác

- **README.md** (phần Seed Data): Mật khẩu admin đã đổi từ `admin@admin` sang `admin@1`. Cần cập nhật để đồng bộ.

---

## Câu hỏi thường gặp

**Q: Database đã có dữ liệu cũ, migration có ghi đè không?**  
A: Có. Migration chỉ cập nhật `PasswordHash` của user admin (Id cố định), không ảnh hưởng user khác.

**Q: Có cần build lại project không?**  
A: Chạy `dotnet ef database update` sẽ build nếu cần. Để chạy API: `dotnet run`.
