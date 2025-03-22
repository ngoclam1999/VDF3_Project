using System;
using System.Collections.Concurrent;
using System.ComponentModel;
using System.Net;
using System.Threading;
using EasyModbus;

public class BackgroundWorkerService
{
    private static BackgroundWorkerService _instance;
    private static readonly object _lock = new object();
    private BackgroundWorker _worker;
    private BackgroundWorker _workerReload;
    private ModbusClient _modbusClient;
    private bool _isRunning = false;
    // Dictionary chứa dữ liệu nhận từ UI
    private ConcurrentDictionary<string, object> _dataStore = new ConcurrentDictionary<string, object>();

    // Event để cập nhật UI
    public event Action<string, object> OnDataUpdated;

    public static BackgroundWorkerService Instance
    {
        get
        {
            if (_instance == null)
            {
                lock (_lock)
                {
                    if (_instance == null)
                        _instance = new BackgroundWorkerService();
                }
            }
            return _instance;
        }
    }

    private BackgroundWorkerService()
    {
        _worker = new BackgroundWorker();
        _worker.WorkerSupportsCancellation = true;
        _worker.DoWork += Worker_DoWork;
        _worker.RunWorkerCompleted += _worker_RunWorkerCompleted;
    }

    private void _worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
    {
        // Nếu _isRunning vẫn true, nghĩa là có yêu cầu dừng worker
        if (_isRunning)
        {
            try
            {
                _modbusClient.Disconnect();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi khi ngắt kết nối: {ex.Message}");
            }
            _worker.CancelAsync();
        }
        else
        {
            UpdateData("DoworkMess", "Mất kết nối");
            // Khởi tạo workerReload mới để cố gắng kết nối lại
            _workerReload = new BackgroundWorker();
            _workerReload.WorkerSupportsCancellation = true;
            _workerReload.DoWork += _workerReload_DoWork;
            _workerReload.RunWorkerCompleted += _workerReload_RunWorkerCompleted;
            _workerReload.RunWorkerAsync();
        }
    }

    private void _workerReload_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
    {
        UpdateData("DoworkMess", "Kết nối lại thành công");
    }

    private void _workerReload_DoWork(object sender, DoWorkEventArgs e)
    {
        // Sửa lại điều kiện: lặp lại cho đến khi _modbusClient kết nối thành công
        while (!_modbusClient.Connected)
        {
            try
            {
                // Cố gắng kết nối lại
                _modbusClient.Connect();
                if (_modbusClient.Connected)
                {
                    // Khi kết nối thành công, khởi động lại worker chính
                    _worker.RunWorkerAsync();
                    break;
                }
            }
            catch (Exception ex)
            {
                // Cập nhật thông báo lỗi chi tiết và in ra console
                string errorMsg = $"Mất kết nối, đang cố gắng kết nối lại: {ex.Message}";
                Console.WriteLine(errorMsg);
                UpdateData("DoworkMess", errorMsg);
                // Tạm dừng 2 giây trước khi thử lại để tránh loop quá nhanh
                Thread.Sleep(2000);
            }
        }
    }
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

    private void Worker_DoWork(object sender, DoWorkEventArgs e)
    {
        _isRunning = true;

        while (!_worker.CancellationPending)
        {
            // Kiểm tra dữ liệu IP và khởi tạo ModbusClient nếu cần
            if (_dataStore.TryGetValue("IpAddress", out object sIp))
            {
                if (!IsValidIP(sIp.ToString()))
                {
                    UpdateData("DoworkMess", "Địa chỉ IP không hợp lệ");
                    return;
                }
            }
            else
            {
                UpdateData("DoworkMess", "Chưa nhập địa chỉ IP");
                return;
            }

            if (_modbusClient == null)
            {
                _modbusClient = new ModbusClient();
                _modbusClient.IPAddress = sIp.ToString();
                _modbusClient.Port = 502;
                _modbusClient.ConnectionTimeout = 5000;
            }

            // Nếu chưa kết nối, thử kết nối
            if (!_modbusClient.Connected)
            {
                try
                {
                    _modbusClient.Connect();
                    UpdateData("DoworkMess", "Kết nối Modbus thành công");
                }
                catch (Exception ex)
                {
                    string errorMsg = $"Lỗi khi kết nối Modbus: {ex.Message}";
                    Console.WriteLine(errorMsg);
                    UpdateData("DoworkMess", errorMsg);
                    // Nếu không kết nối được, dừng worker để kích hoạt quá trình tái kết nối
                    _isRunning = false;
                    break;
                }
            }

            // Kiểm tra lại kết nối
            if (!_modbusClient.Connected)
            {
                UpdateData("DoworkMess", "Không thể kết nối Modbus");
                _isRunning = false;
                break;
            }

            // Read/Write data to Modbus 

            try
            {
                // Nếu có dữ liệu từ UI, xử lý (ví dụ gửi đi qua modbus)
                if (_dataStore.TryGetValue("InputData", out object inputData))
                {
                    // Xử lý dữ liệu inputData tại đây
                }

                // Đọc dữ liệu từ Modbus
                int[] registerValues = _modbusClient.ReadHoldingRegisters(31,1);
                if (registerValues != null && registerValues.Length > 0)
                {
                    // Giả sử VariableRobot là biến toàn cục hoặc thuộc tính của một lớp khác
                    VariableRobot.RbtMode = registerValues[0];
                    UpdateData("RbtMode", VariableRobot.RbtMode);
                }
                else
                {
                    UpdateData("DoworkMess", "Không nhận được dữ liệu từ Modbus");
                }

                float RobotX = ModbusClient.ConvertRegistersToFloat(_modbusClient.ReadInputRegisters(51, 2));

                if (RobotX != null)
                {
                    // Giả sử VariableRobot là biến toàn cục hoặc thuộc tính của một lớp khác
                    VariableRobot.CurrentX = RobotX;
                    UpdateData("CurrentX", VariableRobot.CurrentX);
                   
                }
                else
                {
                    UpdateData("DoworkMess", "Không nhận được dữ liệu từ Modbus");
                }

                float RobotY = ModbusClient.ConvertRegistersToFloat(_modbusClient.ReadInputRegisters(53, 2));
                if (RobotY != null)
                {
                    // Giả sử VariableRobot là biến toàn cục hoặc thuộc tính của một lớp khác
                    VariableRobot.CurrentY = RobotY;
                    UpdateData("CurrentY", VariableRobot.CurrentY);
                }
                else
                {
                    UpdateData("DoworkMess", "Không nhận được dữ liệu từ Modbus");
                }

                

                if (_dataStore.TryGetValue("CountPoint", out object CountPoint))
                {
                    
                   if((int)CountPoint > 0)
                   {
                        for (int i = 0; i < Math.Min((int)CountPoint, 15); i++)
                        {

                            if (_dataStore.TryGetValue($"PointX{i + 1}", out object pointX))
                                VariableRobot.PointsX[i] = (float)pointX;

                            if (_dataStore.TryGetValue($"PointY{i + 1}", out object pointY))
                                VariableRobot.PointsY[i] = (float)pointY;

                            if (_dataStore.TryGetValue($"Angle{i + 1}", out object pointA))
                                VariableRobot.PointsA[i] = (float)pointA;
                            
                            try
                            {
                                _modbusClient.WriteSingleRegister(58, (int)CountPoint);
                                _modbusClient.WriteMultipleRegisters(60+i*6, ConvertFloatToRegisters(VariableRobot.PointsX[i]));
                                _modbusClient.WriteMultipleRegisters(60 + i*6+2, ConvertFloatToRegisters(VariableRobot.PointsY[i]));
                                _modbusClient.WriteMultipleRegisters(60 + i * 6 + 4, ConvertFloatToRegisters(VariableRobot.PointsA[i]));
                            }
                            catch
                            {

                            }
                        }
                    }    
                    
                }
                else
                {
                   //UpdateData("DoworkMess", "Không nhận được dữ liệu gữi về sever");
                    //return;
                }


                // Giảm tải CPU
                Thread.Sleep(50);
            }
            catch (Exception ex)
            {
                string errorMsg = $"Lỗi trong quá trình xử lý: {ex.Message}";
                Console.WriteLine(errorMsg);
                UpdateData("DoworkMess", errorMsg);
                _isRunning = false;
                break;
            }
        }
    }

    public void Start()
    {
        if (!_worker.IsBusy)
            _worker.RunWorkerAsync();
    }

    public void Stop()
    {
        if (_worker.IsBusy)
            _worker.CancelAsync();

        if(_modbusClient.Connected)
            _modbusClient.Disconnect();
    }

    public void UpdateData(string key, object value)
    {
        _dataStore[key] = value;
        OnDataUpdated?.Invoke(key, value);
    }

    public object GetData(string key)
    {
        _dataStore.TryGetValue(key, out object value);
        return value;
    }

    static bool IsValidIP(string ip)
    {
        return IPAddress.TryParse(ip, out _);
    }
}

public class InvokeService
{
    public static void SendData(string key, object value)
    {
        BackgroundWorkerService.Instance.UpdateData(key, value);
    }

    public static object GetData(string key)
    {
        return BackgroundWorkerService.Instance.GetData(key);
    }
}
