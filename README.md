# 🏥 Clinic Management – Hệ thống quản lý phòng khám

Ứng dụng web quản lý phòng khám xây dựng bằng **ASP.NET Core MVC (.NET 8)** và **SQL Server**. Hệ thống có 3 vai trò – **Quản trị viên**, **Bác sĩ**, **Bệnh nhân** – hỗ trợ đặt lịch khám, khám bệnh, lưu hồ sơ bệnh án và thống kê.

## ✨ Tính năng

### 👤 Bệnh nhân
- Đăng ký / đăng nhập, cập nhật hồ sơ cá nhân
- Xem danh sách bác sĩ, lọc theo chuyên khoa hoặc tìm theo tên
- Đặt lịch khám: chọn chuyên khoa → bác sĩ → ngày → khung giờ còn trống (tải bằng AJAX)
- Xem và hủy lịch hẹn (chỉ được hủy trước ít nhất 1 ngày)
- Xem lịch sử khám và chi tiết hồ sơ bệnh án (triệu chứng, chẩn đoán, lời dặn)

### 🩺 Bác sĩ
- Xem lịch hẹn của mình, lọc theo trạng thái
- Khám bệnh: ghi triệu chứng, chẩn đoán, lời dặn → tự tạo hồ sơ bệnh án
- Xem lịch sử khám của từng bệnh nhân
- Tạo hồ sơ bệnh nhân mới và đặt lịch hộ bệnh nhân
- Cập nhật hồ sơ cá nhân, ảnh đại diện, đổi mật khẩu

### 🛠️ Quản trị viên
- Dashboard tổng quan và danh sách lịch hẹn trong ngày
- Quản lý **bác sĩ** (thêm / sửa / xóa, tải ảnh), **chuyên khoa**, **bệnh nhân** (có tìm kiếm)
- Quản lý **lịch hẹn**: tạo, sửa, đổi trạng thái, xóa, lọc theo trạng thái / ngày / bác sĩ
- **Thống kê**: biểu đồ lịch hẹn theo tháng (12 tháng gần nhất) và theo bác sĩ (Chart.js)

### 📋 Quy tắc nghiệp vụ
- Trạng thái lịch hẹn: `Đã đặt` → `Đã khám` hoặc `Đã hủy`
- Khung giờ khám: 08:00, 09:00, 10:00, 11:00, 13:00, 14:00, 15:00, 16:00, 17:00
- Không cho đặt lịch trong quá khứ và không cho trùng giờ của cùng một bác sĩ (lịch đã hủy không tính)
- Không xóa được bác sĩ hoặc bệnh nhân đã có lịch hẹn, và chuyên khoa còn bác sĩ thuộc về

## 🧰 Công nghệ

| Thành phần | Công nghệ |
|---|---|
| Backend | ASP.NET Core MVC, .NET 8 |
| ORM / CSDL | Entity Framework Core 8, SQL Server |
| Xác thực | Cookie Authentication, phân quyền theo Role |
| Giao diện | Razor Views, Bootstrap 5.3, Font Awesome 6, jQuery |
| Biểu đồ | Chart.js 4 |

## 📁 Cấu trúc thư mục

```
ClinicManagement/
├── Controllers/    # Account, Home, Admin, Doctor, Patient
├── Models/         # User, Doctor, Specialty, Appointment, MedicalRecord
│   └── ViewModels/ # Login, Register, BookAppointment, Examination, ...
├── Data/           # ApplicationDbContext, DbInitializer (dữ liệu mẫu)
├── Migrations/     # EF Core migrations
├── Views/          # Razor views theo từng controller
├── wwwroot/        # CSS, ảnh bác sĩ
├── Program.cs      # Cấu hình ứng dụng
└── appsettings.json
```

## 🚀 Cài đặt và chạy

### Yêu cầu
- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- SQL Server (bản Express / Developer / LocalDB đều được)
- Visual Studio 2022 hoặc VS Code (tùy chọn)

### Các bước

```bash
# 1. Clone repo
git clone <URL-repo-của-bạn>
cd <tên-thư-mục>

# 2. Kiểm tra chuỗi kết nối trong appsettings.json (xem bên dưới)

# 3. Chạy ứng dụng
dotnet run
```

Khi khởi động, ứng dụng **tự động tạo database `QLPK`, chạy migration và nạp dữ liệu mẫu**, nên không cần chạy thêm lệnh nào cho CSDL.

Mặc định ứng dụng chạy tại:
- https://localhost:52889
- http://localhost:52890

### Chuỗi kết nối

Mặc định trong `appsettings.json`:

```json
"DefaultConnection": "Server=localhost;Database=QLPK;Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=true"
```

Nếu dùng SQL Express hoặc LocalDB, đổi `Server=localhost` thành `Server=.\\SQLEXPRESS` hoặc `Server=(localdb)\\MSSQLLocalDB`. Nếu dùng tài khoản SQL (không dùng Windows Authentication), thay `Trusted_Connection=True` bằng `User Id=...;Password=...`.

Nếu muốn tạo DB thủ công:

```bash
dotnet ef database update
```

## 🔑 Tài khoản dùng thử

Dữ liệu mẫu gồm 5 chuyên khoa, 10 bác sĩ, 50 bệnh nhân cùng lịch hẹn và hồ sơ khám ngẫu nhiên.

| Vai trò | Tên đăng nhập | Mật khẩu |
|---|---|---|
| Quản trị viên | `admin` | `admin123` |
| Bác sĩ | `bacsi01` … `bacsi10` | `123456` |
| Bệnh nhân | `patient01` … `patient50` | `123456` |

> ⚠️ Đây chỉ là tài khoản demo. Hãy đổi hoặc xóa khi triển khai thật.

## ⚠️ Hạn chế đã biết

- Mật khẩu hiện **lưu dạng văn bản thường** (chưa băm). Khi triển khai thực tế nên dùng `PasswordHasher` hoặc ASP.NET Core Identity.
- Chưa có kiểm thử tự động.
- Ảnh bác sĩ được lưu trong `wwwroot/img/doctors`, chưa có giới hạn loại / dung lượng file tải lên.

## 🗺️ Hướng phát triển

- Băm mật khẩu / chuyển sang ASP.NET Core Identity
- Gửi email hoặc SMS nhắc lịch hẹn
- Quản lý đơn thuốc, hóa đơn và thanh toán
- Xuất hồ sơ bệnh án ra PDF
- Viết unit test cho các quy tắc đặt lịch

## 📄 Giấy phép

Dự án phục vụ mục đích học tập. Bạn có thể thêm file `LICENSE` (ví dụ MIT) nếu muốn công khai cho người khác sử dụng.
