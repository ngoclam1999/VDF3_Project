**VDF3 Project**

**Giới thiệu**

VDF3 Project là một ứng dụng hỗ trợ kết nối với camera công nghiệp qua chuẩn truyền thông Gig-E và điều khiển robot thông qua Modbus TCP/IP. Ứng dụng này có các chức năng chính như:

Kết nối camera công nghiệp: Nhận dữ liệu hình ảnh từ camera Gig-E.

Kết nối robot: Điều khiển và trao đổi dữ liệu với robot qua Modbus TCP/IP.

Calibration giữa robot và camera: Hiệu chỉnh vị trí giữa camera và robot để đảm bảo độ chính xác.

Chế độ tự động: Tự động phát hiện đối tượng, xác định tọa độ (X, Y) và góc xoay.

Tích hợp AI: Kết nối với API qua FastAPI để xử lý hình ảnh và chạy mô hình AI.

**Yêu cầu hệ thống**

Hệ điều hành: Windows 10/11 (64-bit)

.NET Framework 4.8 trở lên

OpenCvSharp hoặc EmguCV (tùy theo phiên bản sử dụng)

FastAPI để chạy API xử lý hình ảnh

Camera hỗ trợ chuẩn Gig-E

Robot có giao tiếp Modbus TCP/IP

**Cài đặt**

Clone repository:

git clone https://github.com/ngoclam1999/VDF3_Project.git

Cài đặt thư viện cần thiết:

Đối với Python (FastAPI và AI Model):

pip install fastapi uvicorn opencv-python numpy

Đối với C# (thông qua NuGet):

Install-Package OpenCvSharp4
Install-Package Modbus-TCP

**Cấu hình kết nối:**

Cấu hình IP của camera và robot trong file cấu hình.

Đảm bảo API xử lý hình ảnh đang chạy trước khi kết nối.

**Cách sử dụng**

Khởi động API xử lý ảnh:

uvicorn api:app --host 0.0.0.0 --port 8000

**Chạy ứng dụng C#:**

Mở Visual Studio và build project.

Chạy ứng dụng để kết nối với camera và robot.

Camera quét và phát hiện đối tượng.

Hệ thống tính toán tọa độ X, Y và góc quay của đối tượng.

Robot nhận dữ liệu và thực hiện thao tác.

**Đóng góp**

Nếu bạn muốn đóng góp, hãy fork repository, tạo branch mới và gửi pull request.

**Liên hệ**

Tác giả: ngoclam1999

Email: [Email của bạn]

License: MIT

Nếu có bất kỳ vấn đề nào, vui lòng tạo issue trên GitHub để được hỗ trợ!

