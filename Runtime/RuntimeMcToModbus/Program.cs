using System;
using System.Threading;
using EasyModbus;
using ActUtlTypeLib;
using System.IO.Ports;
using System.Runtime.InteropServices;

namespace PLCtoModbusServer_Console
{
    class Program
    {
        // Khai báo đối tượng PLC sử dụng ActUtlType
        static ActUtlType plc = new ActUtlType();

        // Khai báo Modbus Server từ EasyModbus
        static ModbusServer modbusServer = new ModbusServer();

        // Biến điều khiển vòng lặp cập nhật dữ liệu
        static bool isRunning = true;

        static void Main(string[] args)
        {
            // --- 1. KẾT NỐI VỚI PLC ---
            plc.ActLogicalStationNumber = 1;  // Số Logical station, điều chỉnh theo PLC của bạn
            short ret = (short)plc.Open();
            if (ret != 0)
            {
                Console.WriteLine("PLC connection error. Error code: " + ret);
                return;
            }
            Console.WriteLine("Successful PLC connection.");

            // --- 2. DỰNG MODBUS SERVER ---
            modbusServer.Port = 502;  // Cổng mặc định của Modbus TCP
            modbusServer.Listen();
            Console.WriteLine("Modbus Server is running on port 502.");


            // --- 3. KHỞI ĐỘNG LUỒNG CẬP NHẬT DỮ LIỆU PLC ---
            Thread plcThread = new Thread(UpdatePLCDataLoop);
            plcThread.IsBackground = true;
            plcThread.Start();

            Console.WriteLine("Runtime application is running hidden. Press Enter to escape ...");
            Console.ReadLine();

            // Dừng ứng dụng
            isRunning = false;
            plcThread.Join();
            //plc.Close();
            modbusServer.StopListening();
            Console.WriteLine("The application has stopped.");
        }

        /// <summary>
        /// Luồng đọc dữ liệu từ PLC và cập nhật vào Modbus Server liên tục.
        /// Trong ví dụ này:
        /// - Đọc 2 thanh ghi bắt đầu từ "D0" (ví dụ: D0, D1) và ánh xạ vào holding registers 1,2.
        /// - Đọc 4 thanh ghi bắt đầu từ "D4" (ví dụ: D4, D5, D6, D7) và chuyển đổi sang kiểu double (nếu cần).
        /// Các giá trị có thể được ánh xạ hoặc mở rộng theo yêu cầu.
        /// </summary>
        static void UpdatePLCDataLoop()
        {
            while (isRunning)
            {
                try
                {
                    // --- ĐỌC DỮ LIỆU TỪ PLC ---
                    // Đọc 2 thanh ghi từ "D0"
                    int[] d0Data = new int[2];
                    int retD0 = plc.ReadDeviceBlock("D0", 2, out d0Data[0]);
                    // Đọc 4 thanh ghi từ "D4"
                    int[] d4Data = new int[4];
                    int retD4 = plc.ReadDeviceBlock("D4", 4, out d4Data[0]);
                    double data = RegistersToDouble(d4Data, true);
                    Console.WriteLine((float)data);
                    int[] DoubleToRe = ModbusClient.ConvertFloatToRegisters((float)data);
                    if (retD0 == 0 && retD4 == 0)
                    {
                        // --- CẬP NHẬT VÀO MODBUS SERVER ---
                        // Ánh xạ dữ liệu D0 vào holding registers 1 và 2
                        if (d0Data != null && d0Data.Length >= 2)
                        {
                            modbusServer.holdingRegisters[1] = (short)d0Data[1];
                            modbusServer.holdingRegisters[2] = (short)d0Data[0];
                        }

                        // Ánh xạ dữ liệu D4 (4 thanh ghi) vào holding registers 3-6
                        if (d4Data != null && d4Data.Length >= 4)
                        {
                            modbusServer.inputRegisters[51] = (short)DoubleToRe[0];
                            modbusServer.inputRegisters[52] = (short)DoubleToRe[1];

                            int[] data1 = ModbusClient.ConvertFloatToRegisters(29.43f);
                            modbusServer.inputRegisters[53] = (short)data1[0];
                            modbusServer.inputRegisters[54] = (short)data1[1];

                            // Nếu cần chuyển đổi 4 thanh ghi thành double (ví dụ: giá trị đo lường)
                            double d4Double = RegistersToDouble(d4Data, true);
                            Console.WriteLine("PLC Read: D0 = {0}, {1}; D4 block (as double) = {2}", d0Data[0], d0Data[1], d4Double);
                        }
                        else
                        {
                            Console.WriteLine("PLC Read: D0 = {0}, {1}", d0Data[0], d0Data[1]);
                        }
                    }
                    else
                    {
                        Console.WriteLine("PLC Reading Error: retD0={0}, retD4={1}", retD0, retD4);
                    }

                    int NumOfValues = (int)modbusServer.holdingRegisters[58];

                    Console.WriteLine($"NumOfValues: {NumOfValues}");
                    for (int i = 0; i < 2; i++)
                    {
                        int[] arrintX = new int[2] { modbusServer.holdingRegisters[60 + i * 6], modbusServer.holdingRegisters[60 + i * 6 +1 ] };
                        float Dx = ModbusClient.ConvertRegistersToFloat(arrintX);
                        Console.WriteLine($"Fx:{Dx}");
                        int[] arrsendX = ModbusClient.ConvertDoubleToRegisters(Dx);
                        plc.SetDevice($"D{100 + i * 12}", arrsendX[0]);
                        plc.SetDevice($"D{100 + i * 12 + 1}", arrsendX[1]);
                        plc.SetDevice($"D{100 + i * 12 + 2}", arrsendX[2]);
                        plc.SetDevice($"D{100 + i * 12 + 3}", arrsendX[3]);
                        
                        int[] arrintY = new int[2] { modbusServer.holdingRegisters[62 + i * 6], modbusServer.holdingRegisters[62 + i * 6 + 1 ] };
                        float Dy = ModbusClient.ConvertRegistersToFloat(arrintY);
                        Console.WriteLine($"Fy:{Dy}");
                        int[] arrsendY = DoubleToRegisters((double)Dy, true);
                        plc.SetDevice($"D{104 + i * 12}", arrsendY[0]);
                        plc.SetDevice($"D{104 + i * 12 + 1}", arrsendY[1]);
                        plc.SetDevice($"D{104 + i * 12 + 2}", arrsendY[2]);
                        plc.SetDevice($"D{104 + i * 12 + 3}", arrsendY[3]);

                        int[] arrintA = new int[2] { modbusServer.holdingRegisters[64 + i * 6], modbusServer.holdingRegisters[64 + i * 6 + 1] };
                        float Da = ModbusClient.ConvertRegistersToFloat(arrintA);
                        Console.WriteLine($"Fa:{Da}");
                        int[] arrsendA = DoubleToRegisters((double)Da, true);
                        plc.SetDevice($"D{108 + i * 12}", arrsendA[0]);
                        plc.SetDevice($"D{108 + i * 12 + 1}", arrsendA[1]);
                        plc.SetDevice($"D{108 + i * 12 + 2}", arrsendA[2]);
                        plc.SetDevice($"D{108 + i * 12 + 3}", arrsendA[3]);
                    }

                }
                catch (Exception ex)
                {
                    Console.WriteLine("Exception: " + ex.Message);
                }

                Thread.Sleep(1000); // Đọc dữ liệu mỗi giây
            }
        }


        /// <summary>
        /// Chuyển 4 thanh ghi (16-bit mỗi thanh ghi) thành giá trị double.
        /// </summary>
        /// <param name="registers">Mảng 4 số nguyên (đại diện cho 4 thanh ghi)</param>
        /// <param name="isLittleEndian">True nếu dữ liệu little-endian</param>
        /// <returns>Giá trị double</returns>
        static double RegistersToDouble(int[] registers, bool isLittleEndian)
        {
            if (registers.Length != 4) throw new ArgumentException("Cần 4 thanh ghi 16-bit!");
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

        /// <summary>
        /// (Tùy chọn) Chuyển đổi giá trị double thành mảng 4 thanh ghi (16-bit)
        /// </summary>
        static int[] DoubleToRegisters(double value, bool isLittleEndian)
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
    }
}
