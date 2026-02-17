# Tóm tắt thay đổi

## Cập nhật mật khẩu Admin (17/02/2026)

| Mục | Trước | Sau |
|-----|-------|-----|
| **Admin Email** | `admin@educycle.com` | `admin@educycle.com` |
| **Admin Password** | `admin@admin` | `admin@1` |

### File liên quan

- `ApplicationDbContext.cs` — Seed admin với hash mới
- `20260217150000_UpdateAdminPassword.cs` — Migration cập nhật DB
- `Helpers/PasswordHashGenerator.cs` — Tool tạo BCrypt hash (dev)

### Áp dụng

```bash
cd EduCycle.Api
dotnet ef database update
```

Chi tiết: xem [HUONG_DAN_AP_DUNG_THAY_DOI.md](HUONG_DAN_AP_DUNG_THAY_DOI.md)
