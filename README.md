# Phần mềm quản lý trạm sạc xe điện (EV Charging Station Management System)

[![C#](https://img.shields.io/badge/Language-C%23-239120?style=for-the-badge&logo=c-sharp)](https://docs.microsoft.com/en-us/dotnet/csharp/)
[![Database](https://img.shields.io/badge/Database-SQL%20Server-CC2927?style=for-the-badge&logo=microsoft-sql-server)](https://www.microsoft.com/en-us/sql-server)

## 🔑 Tài Khoản Đăng Nhập Thử Nghiệm

Bạn có thể sử dụng các tài khoản dưới đây để đăng nhập và kiểm tra các chức năng của hệ thống. Đây là các tài khoản đã được gán **Role** và **Policy** khác nhau trong hệ thống **ASP.NET Core Identity**.

| Vai trò (Role) | Tên đăng nhập (Username) | Mật khẩu (Password) |
| :--- | :--- | :--- |
| **Quản trị viên** | `admin@example.com` | `admin123` |
| **Nhân viên** | `staff@example.com` | `123456` |
| **Tài xế** | `alice@example.com` | `123456` |
---

## 🚀 Hướng Dẫn Khởi Chạy (Local Setup)

1.  **Clone Repository:**
    ```bash
    git clone [Địa chỉ Repository của bạn]
    cd EV_DMS
    ```
2.  **Cấu hình Database:**
    * Mở file `appsettings.json` và cập nhật chuỗi kết nối (`ConnectionString`) để trỏ đến SQL Server cục bộ của bạn.
3.  **Chạy Ứng dụng:**
    * Mở giải pháp (.sln) bằng Visual Studio và nhấn **F5**, hoặc chạy lệnh:
        ```bash
        dotnet run
        ```
4.  Truy cập vào địa chỉ hiển thị (thường là `https://localhost:7062/`).
