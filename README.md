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

## ✨ Giới Thiệu Luồng Hoạt Động (Workflow Overview)

Hệ thống được thiết kế để phục vụ ba nhóm người dùng chính, mỗi nhóm có các luồng hoạt động riêng biệt, đảm bảo quá trình vận hành trạm sạc diễn ra xuyên suốt và hiệu quả.

### 1. Luồng Hoạt Động của Tài Xế (EV Driver Workflow)

Luồng này tập trung vào trải nghiệm của người dùng khi tìm kiếm, sử dụng dịch vụ sạc, và thanh toán.

* **Đăng nhập & Quản lý Tài khoản**: Tài xế đăng nhập qua email, SĐT, hoặc mạng xã hội. Quản lý thông tin cá nhân, xe, và lịch sử giao dịch.
* **Tìm kiếm & Đặt chỗ**:
    * Xem **Bản đồ trạm sạc** theo vị trí, công suất, trạng thái (trống/đang dùng), loại cổng sạc, tốc độ sạc, giá cả.
    * **Đặt chỗ/Hủy chỗ** sạc để đảm bảo có vị trí khi đến.
* **Thực hiện Sạc**:
    * Sử dụng **QR code** hoặc ứng dụng để bắt đầu/kết thúc sạc.
    * Theo dõi trạng thái sạc (SOC %), thời gian còn lại, chi phí, và nhận thông báo khi sạc đầy.
* **Thanh toán & Hậu mãi**:
    * Thanh toán theo **kWh**, theo thời gian, hoặc **gói thuê bao**.
    * Thanh toán trực tuyến qua e-wallet, banking,....
    * Nhận hóa đơn điện tử.
    * Xem lịch sử và phân tích cá nhân, báo cáo chi phí sạc hàng tháng, và thống kê thói quen sạc.

### 2. Luồng Hoạt Động của Nhân viên Trạm sạc (Charging Station Staff Workflow)

Luồng này đảm bảo các hoạt động tại trạm diễn ra suôn sẻ và giải quyết các sự cố.

* **Quản lý Sạc tại trạm**: Quản lý việc **khởi động/hoặc dừng phiên sạc** và **ghi nhận thanh toán** tại chỗ cho xe.
* **Theo dõi & Báo cáo**:
    * Theo dõi **thời gian/trạng thái điểm sạc** (online/offline, công suất).
    * Báo cáo các sự cố kỹ thuật tại trạm sạc.

### 3. Luồng Hoạt Động của Quản trị viên (Admin Workflow)

Luồng tập trung vào việc giám sát, vận hành hệ thống toàn diện, quản lý người dùng, và phân tích kinh doanh.

* **Quản lý Hệ thống & Điểm sạc**:
    * Theo dõi **tình trạng toàn bộ trạm sạc** (online/offline, công suất).
    * **Điều khiển từ xa** các hoạt động của trạm: hoạt động/dừng.
* **Quản lý Người dùng & Dịch vụ**:
    * Quản lý khách hàng cá nhân/doanh nghiệp.
    * Tạo/Quản lý **gói thuê bao** (trả trước, trả sau, hội viên VIP).
    * Phân quyền nhân viên trạm sạc.
* **Báo cáo & Thống kê**:
    * Theo dõi **doanh thu** theo trạm, khu vực, thời gian.
    * Báo cáo **tần suất sử dụng trạm**, giờ cao điểm.
