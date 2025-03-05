using MvCameraControl;
using System;
using System.Drawing; // Để sử dụng Bitmap

public enum CameraStatus
{
    Disconnected,
    Connected,
    Error
}

public abstract class CameraBase
{
    public string Name { get; set; } // Tên camera
    public string ConnectionInfo { get; set; } // Thông tin kết nối (URL, ID, IP, v.v.)
    public CameraStatus Status { get; protected set; } // Trạng thái kết nối
    public int ErrorCode { get; protected set; } // Mã lỗi

    public abstract void Start(int index); // Bắt đầu kết nối
    public abstract void Stop();  // Dừng kết nối
    public abstract Bitmap Capture(); // Chụp ảnh hoặc lấy khung hình và trả về Bitmap
    public abstract void SetExposure(Int32 exposure);
    public abstract void SetBrightness(Int32 brigness);
    public abstract void SetGain(float gain);
}
