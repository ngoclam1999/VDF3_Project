using System;
using System.Runtime.InteropServices;
using ActUtlTypeLib;


namespace VDF3_Solution3
{
   
    public class PlcCommunicator : IPlcConnection
    {
        private ActUtlType plc;

        public PlcCommunicator()
        {
            plc = new ActUtlType();
        }

        public bool Connect(int stationNumber, out string errorMessage)
        {
            plc.ActLogicalStationNumber = stationNumber;
            short connectResult = (short)plc.Open();
            if (connectResult == 0)
            {
                errorMessage = "Mở kết nối thành công.";
                return true;
            }
            else
            {
                errorMessage = GetConnectionErrorMessage(connectResult);
                return false;
            }
        }

        private string GetConnectionErrorMessage(short errorCode)
        {
            switch (errorCode)
            {
                case 1:
                    return "Không mở được kết nối.";
                case 2:
                    return "Địa chỉ IP không đúng.";
                case 3:
                    return "Số kết nối vượt quá giới hạn.";
                case 4:
                    return "Cho phép kết nối đang tắt.";
                case 6:
                    return "Không kiểm tra được giá trị trả về.";
                default:
                    return $"Lỗi không xác định: {errorCode}";
            }
        }

        public void Disconnect()
        {
            plc.Close();
        }

        /// <summary>
        /// Đọc giá trị double từ PLC bằng cách ghép 4 thanh ghi 16-bit.
        /// </summary>
        public double ReadDouble(string startAddress, bool isLittleEndian = true)
        {
            int[] registers = new int[4];
            plc.ReadDeviceBlock(startAddress, 4, out registers[0]);
            return RegistersToDouble(registers, isLittleEndian);
        }

        /// <summary>
        /// Ghi giá trị double xuống PLC ở 4 thanh ghi 16-bit.
        /// </summary>
        public void WriteDouble(string startAddress, double value, bool isLittleEndian = true)
        {
            int[] wrData = DoubleToRegisters(value, isLittleEndian);
            int startAddr = int.Parse(startAddress.TrimStart('D', 'd'));
            plc.SetDevice($"D{startAddr}", wrData[0]);
            plc.SetDevice($"D{startAddr + 1}", wrData[1]);
            plc.SetDevice($"D{startAddr + 2}", wrData[2]);
            plc.SetDevice($"D{startAddr + 3}", wrData[3]);
        }

        public void WriteFloat(string startAddress, float value, bool isLittleEndian = true)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="startAddress"></param>
        /// <returns></returns>
        public int ReadInt(string startAddress)
        {
            int[] registers = new int[2];
            plc.ReadDeviceBlock(startAddress, 2, out registers[0]);
            return registers[0];
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="startAddress"></param>
        /// <param name="value"></param>
        public void WriteInt(string startAddress, int value)
        {
            int startAddr = int.Parse(startAddress.TrimStart('D', 'd'));
            plc.SetDevice($"D{startAddr}", value);
        }

        // Phương thức chuyển 4 thanh ghi 16-bit thành số double.
        public static double RegistersToDouble(int[] registers, bool isLittleEndian)
        {
            if (registers == null || registers.Length != 4)
                throw new ArgumentException("Cần 4 thanh ghi 16-bit!");

            byte[] bytes = new byte[8];
            if (isLittleEndian)
            {
                for (int i = 0; i < 4; i++)
                    BitConverter.GetBytes((ushort)registers[i]).CopyTo(bytes, i * 2);
            }
            else
            {
                for (int i = 0; i < 4; i++)
                    BitConverter.GetBytes((ushort)registers[i]).CopyTo(bytes, (3 - i) * 2);
            }
            return BitConverter.ToDouble(bytes, 0);
        }

        public static int[] DoubleToRegisters(double value, bool isLittleEndian)
        {
            byte[] bytes = BitConverter.GetBytes(value);
            int[] registers = new int[4];

            if (isLittleEndian)
            {
                for (int i = 0; i < 4; i++)
                    registers[i] = BitConverter.ToUInt16(bytes, i * 2);
            }
            else
            {
                for (int i = 0; i < 4; i++)
                    registers[i] = BitConverter.ToUInt16(bytes, (3 - i) * 2);
            }

            return registers;
        }

        // Các phương thức bổ sung riêng cho chuẩn ActUtl

        /// <summary>
        /// Đọc giá trị boolean từ PLC tại địa chỉ đã cho.
        /// </summary>
        public bool ReadBool(string address)
        {
            plc.GetDevice(address, out int value);
            return value == 1;
        }

        /// <summary>
        /// Ghi giá trị boolean xuống PLC tại địa chỉ đã cho.
        /// </summary>
        public void WriteBool(string address, bool value)
        {
            plc.SetDevice(address, value ? 1 : 0);
        }

        /// <summary>
        /// Ghi giá trị float (float precision) xuống PLC bằng cách chuyển đổi thành double và ghi qua 4 thanh ghi.
        /// </summary>
        public void WriteFloatPrecision(string startAddress, float value, bool isLittleEndian = true)
        {
            // Chuyển float sang double nếu cần (hoặc giữ nguyên nếu PLC yêu cầu double)
            double doubleValue = value;
            WriteDouble(startAddress, doubleValue, isLittleEndian);
        }

        public void Dispose()
        {
            if (plc != null)
            {
                plc.Close();
                plc = null;
            }
        }
    }
}
