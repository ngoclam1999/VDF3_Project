using Microsoft.Win32;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

public class CalibrationService
{
    private CalibrationData _calibrationData;

    // Constructor khởi tạo với dữ liệu mặc định hoặc rỗng
    public CalibrationService()
    {
        _calibrationData = new CalibrationData
        {
            CalibrationPoint = new List<CalibrationPoint>(),
            homographyMatrix = new HomographyMatrix(),
            Register = new Register(),
            imageTemplate = new ImageTemplate()
        };
    }

    // Lấy danh sách điểm hiệu chuẩn
    public List<CalibrationPoint> GetCalibrationPoints()
    {
        return _calibrationData.CalibrationPoint;
    }

    // Cập nhật danh sách điểm hiệu chuẩn
    public void UpdateCalibrationPoints(List<CalibrationPoint> points)
    {
        _calibrationData.CalibrationPoint = points;
    }

    // Lấy đối tượng HomographyMatrix
    public HomographyMatrix GetHomographyMatrix()
    {
        return _calibrationData.homographyMatrix;
    }

    // Cập nhật HomographyMatrix
    public void UpdateHomographyMatrix(HomographyMatrix hm)
    {
        _calibrationData.homographyMatrix = hm;
    }

    // Lấy đối tượng Register
    public Register GetRegister()
    {
        return _calibrationData.Register;
    }

    // Cập nhật Register
    public void UpdateRegister(Register reg)
    {
        _calibrationData.Register = reg;
    }

    // Hàm cập nhật imageTemplate từ một Image
    public void UpdateTemplateImage(Image image)
    {
        if (image == null)
            throw new ArgumentNullException(nameof(image));

        using (MemoryStream ms = new MemoryStream())
        {
            // Lưu ảnh dưới định dạng PNG (có thể đổi sang định dạng khác nếu cần)
            image.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
            // Chuyển đổi byte[] sang chuỗi Base64 và gán vào _base64Temlate
            _calibrationData.imageTemplate.Base64Template = Convert.ToBase64String(ms.ToArray());
        }
    }

    // Hàm lấy Image từ imageTemplate
    public Image GetTemplateImage()
    {
        // Nếu chuỗi Base64 rỗng hoặc null thì trả về null
        if (string.IsNullOrEmpty(_calibrationData.imageTemplate.Base64Template))
            return null;

        // Chuyển đổi chuỗi Base64 về mảng byte
        byte[] imageBytes = Convert.FromBase64String(_calibrationData.imageTemplate.Base64Template);
        using (MemoryStream ms = new MemoryStream(imageBytes))
        {
            return Image.FromStream(ms);
        }
    }

    // Serialize dữ liệu hiệu chuẩn sang chuỗi JSON
    public string SaveToJson()
    {
        return JsonConvert.SerializeObject(_calibrationData, Formatting.Indented);
        
    }

    // Load dữ liệu hiệu chuẩn từ chuỗi JSON
    public void LoadFromJson(string json)
    {
        _calibrationData = JsonConvert.DeserializeObject<CalibrationData>(json);
        
    }
}
public class CalibrationPoint
{
    public float x { get; set; }
    public float y { get; set; }
}

public class HomographyMatrix
{
    public float R11 { get; set; }
    public float R12 { get; set; }
    public float Tx { get; set; }
    public float R21 { get; set; }
    public float R22 { get; set; }
    public float Ty { get; set; }
}

public class Register
{
    public int MessFormRbt { get; set; }
    public int ModeRbt { get; set; }
    public int CurrentX { get; set; }
    public int CurrentY { get; set; }
    public int CurrentZ { get; set; }
    public int CurrentU { get; set; }
}

public class ImageTemplate
{
    public string Base64Template { get; set; }
}

public class CalibrationData
{
    public List<CalibrationPoint> CalibrationPoint { get; set; }
    public HomographyMatrix homographyMatrix { get; set; }
    public Register Register { get; set; }
    public ImageTemplate imageTemplate { get; set; }
}

