using Microsoft.Win32;
using Newtonsoft.Json;
using System.Collections.Generic;

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
            Register = new Register()
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

public class CalibrationData
{
    public List<CalibrationPoint> CalibrationPoint { get; set; }
    public HomographyMatrix homographyMatrix { get; set; }
    public Register Register { get; set; }
}
