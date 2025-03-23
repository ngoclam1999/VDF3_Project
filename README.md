<!DOCTYPE html>
<html lang="vi">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>VDF3 Project - README</title>
    <style>
        body {
            font-family: Arial, sans-serif;
            line-height: 1.6;
            margin: 0;
            padding: 20px;
            background-color: #f4f4f4;
            color: #333;
        }
        .container {
            max-width: 900px;
            margin: 0 auto;
            background: white;
            padding: 20px;
            border-radius: 10px;
            box-shadow: 0px 0px 10px rgba(0, 0, 0, 0.1);
        }
        h1, h2, h3 {
            color: #007BFF;
        }
        code {
            background: #eee;
            padding: 2px 5px;
            border-radius: 4px;
            font-family: Consolas, monospace;
        }
        pre {
            background: #222;
            color: #fff;
            padding: 10px;
            border-radius: 5px;
            overflow-x: auto;
        }
        a {
            color: #007BFF;
            text-decoration: none;
        }
        a:hover {
            text-decoration: underline;
        }
        .footer {
            text-align: center;
            margin-top: 20px;
            font-size: 14px;
            color: #666;
        }
    </style>
</head>
<body>

<div class="container">
    <h1>VDF3 Project</h1>
    
    <h2>Giới thiệu</h2>
    <p><strong>VDF3 Project</strong> là ứng dụng kết nối với camera công nghiệp qua chuẩn truyền thông <strong>Gig-E</strong> và điều khiển robot thông qua <strong>Modbus TCP/IP</strong>. Ứng dụng hỗ trợ:</p>
    <ul>
        <li><strong>Kết nối camera công nghiệp:</strong> Nhận dữ liệu hình ảnh từ camera Gig-E.</li>
        <li><strong>Kết nối robot:</strong> Điều khiển robot qua Modbus TCP/IP.</li>
        <li><strong>Calibration:</strong> Hiệu chỉnh vị trí giữa camera và robot.</li>
        <li><strong>Chế độ tự động:</strong> Phát hiện đối tượng, lấy tọa độ (X, Y) và góc xoay.</li>
        <li><strong>Tích hợp AI:</strong> Kết nối API qua FastAPI để xử lý hình ảnh và chạy AI.</li>
    </ul>

    <h2>Yêu cầu hệ thống</h2>
    <ul>
        <li>Windows 10/11 (64-bit)</li>
        <li>.NET Framework 4.8+</li>
        <li>OpenCvSharp hoặc EmguCV</li>
        <li>FastAPI để chạy API</li>
        <li>Camera hỗ trợ chuẩn Gig-E</li>
        <li>Robot có giao tiếp Modbus TCP/IP</li>
    </ul>

    <h2>Cài đặt</h2>
    <h3>1. Clone repository</h3>
    <pre><code>git clone https://github.com/ngoclam1999/VDF3_Project.git</code></pre>

    <h3>2. Cài đặt thư viện</h3>
    <p>Python (FastAPI và AI Model):</p>
    <pre><code>pip install fastapi uvicorn opencv-python numpy</code></pre>

    <p>C# (qua NuGet):</p>
    <pre><code>Install-Package OpenCvSharp4
Install-Package Modbus-TCP</code></pre>

    <h3>3. Cấu hình kết nối</h3>
    <p>Cập nhật IP của camera và robot trong file cấu hình.</p>

    <h2>Cách sử dụng</h2>
    <h3>1. Chạy API xử lý ảnh</h3>
    <pre><code>uvicorn api:app --host 0.0.0.0 --port 8000</code></pre>

    <h3>2. Chạy ứng dụng C#</h3>
    <ul>
        <li>Mở Visual Studio và build project.</li>
        <li>Chạy ứng dụng để kết nối camera và robot.</li>
    </ul>

    <h3>3. Sử dụng chế độ tự động</h3>
    <ul>
        <li>Camera quét đối tượng.</li>
        <li>Tính toán tọa độ (X, Y) và góc xoay.</li>
        <li>Gửi dữ liệu đến robot để thực hiện thao tác.</li>
    </ul>

    <h2>Đóng góp</h2>
    <p>Nếu muốn đóng góp, hãy fork repository, tạo branch mới và gửi pull request.</p>

    <h2>Liên hệ</h2>
    <ul>
        <li><strong>Tác giả:</strong> <a href="https://github.com/ngoclam1999" target="_blank">ngoclam1999</a></li>
        <li><strong>Email:</strong> [Email của bạn]</li>
        <li><strong>License:</strong> MIT</li>
    </ul>

    <div class="footer">
        <p>&copy; 2025 VDF3 Project. Mọi quyền được bảo lưu.</p>
    </div>
</div>

</body>
</html>
