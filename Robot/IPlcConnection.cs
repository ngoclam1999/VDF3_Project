using System;

public interface IPlcConnection : IDisposable
{

    bool Connect(int stationNumber, out string errorMessage);
    void Disconnect();
    double ReadDouble(string startAddress, bool isLittleEndian = true);
    void WriteDouble(string startAddress, double value, bool isLittleEndian = true);
    void WriteFloat(string startAddress, float value, bool isLittleEndian = true);
    int ReadInt(string startAddress);
    void WriteInt(string startAddress, int value);
    // Thêm các phương thức khác nếu cần (ví dụ ReadBool, WriteBool, …)
}
