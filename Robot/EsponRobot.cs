using System;
using EasyModbus;

namespace VDF3_Solution3
{
    public class ModbusPlcConnection : IPlcConnection
    {
        private ModbusClient _modbusClient;

        public ModbusPlcConnection(string ipAddress, int port = 502, int connectionTimeout = 5000)
        {
            _modbusClient = new ModbusClient(ipAddress, port)
            {
                ConnectionTimeout = connectionTimeout
            };
        }

        public bool Connect(int stationNumber, out string errorMessage)
        {
            try
            {
                _modbusClient.Connect();
                if (_modbusClient.Connected)
                {
                    errorMessage = "Kết nối Modbus thành công.";
                    return true;
                }
                else
                {
                    errorMessage = "Không thể kết nối Modbus.";
                    return false;
                }
            }
            catch (Exception ex)
            {
                errorMessage = $"Lỗi kết nối Modbus: {ex.Message}";
                return false;
            }
        }

        public void Disconnect()
        {
            if (_modbusClient.Connected)
                _modbusClient.Disconnect();
        }


        public double ReadDouble(string startAddress, bool isLittleEndian = true)
        {
            // Ví dụ: startAddress là số địa chỉ thanh ghi (ví dụ "31")
            int address = int.Parse(startAddress);
            double registers = ModbusClient.ConvertRegistersToFloat(_modbusClient.ReadInputRegisters(address, 2));

            return registers;
        }

        public void WriteDouble(string startAddress, double value, bool isLittleEndian = true)
        {
            int address = int.Parse(startAddress);
            _modbusClient.WriteMultipleRegisters(address, ConvertFloatToRegisters((float)value));
            Console.WriteLine(address);
            Console.WriteLine((float)value);
        }

        public void WriteFloat(string startAddress, float value, bool isLittleEndian = true)
        {
            int address = int.Parse(startAddress);
            _modbusClient.WriteMultipleRegisters(address, ModbusClient.ConvertFloatToRegisters(value));
        }

        public int ReadInt(string startAddress)
        {
            int address = int.Parse(startAddress);
            int registers = ModbusClient.ConvertRegistersToInt(_modbusClient.ReadInputRegisters(address, 2));
            return registers;
        }

        public void WriteInt(string startAddress, int value)
        {
            int address = int.Parse(startAddress);
            _modbusClient.WriteMultipleRegisters(address, ModbusClient.ConvertIntToRegisters(value));
        }


        // Các hàm convert cho Modbus (theo thứ tự thanh ghi Modbus)

        static int[] ConvertFloatToRegisters(float value)
        {
            // Lấy byte array từ float (mặc định theo định dạng little-endian)
            byte[] bytes = BitConverter.GetBytes((float)Math.Round(value, 6));

            // Nếu thiết bị Modbus yêu cầu dữ liệu theo Big-Endian, cần đảo ngược thứ tự byte:
            int reg1 = (bytes[3] << 8) | bytes[2]; // Thanh ghi đầu tiên
            int reg2 = (bytes[1] << 8) | bytes[0]; // Thanh ghi thứ hai

            // Nếu thiết bị sử dụng little-endian, đổi ngược lại:
            // int reg1 = (bytes[1] << 8) | bytes[0];
            // int reg2 = (bytes[3] << 8) | bytes[2];

            return new int[] { reg1, reg2 };
        }

        public void Dispose()
        {
            Disconnect();
            _modbusClient = null;
        }
    }
}
