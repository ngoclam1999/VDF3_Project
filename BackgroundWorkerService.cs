using System;
using System.Collections.Concurrent;
using System.ComponentModel;
using System.Net;
using System.Threading;
using VDF3_Solution3; // Namespace chứa PlcCommunicator
using ActUtlTypeLib;
using EasyModbus;


public class BackgroundWorkerService
{
    public enum PlcConnectionType
    {
        Modbus,
        ActUtl
    }
    private static BackgroundWorkerService _instance;
    private static readonly object _lock = new object();
    private BackgroundWorker _worker;
    private BackgroundWorker _workerReload;

    // Sử dụng interface chung cho PLC connection
    private IPlcConnection _plcConnection;

    private bool _isRunning = false;
    // Dictionary chứa dữ liệu nhận từ UI
    private ConcurrentDictionary<string, object> _dataStore = new ConcurrentDictionary<string, object>();

    // Cấu hình loại kết nối (mặc định là Modbus)
    public PlcConnectionType ConnectionType { get; set; } = PlcConnectionType.Modbus;

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
        if (_isRunning)
        {
            try
            {
                _plcConnection.Disconnect();
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
        // Cố gắng kết nối lại theo chuẩn kết nối được chọn
        while (true)
        {
            try
            {
                string errorMessage;
                if (_plcConnection.Connect(1, out errorMessage))
                {
                    // Khi kết nối thành công, khởi động lại worker chính
                    _worker.RunWorkerAsync();
                    break;
                }
                else
                {
                    UpdateData("DoworkMess", errorMessage);
                }
            }
            catch (Exception ex)
            {
                string errorMsg = $"Mất kết nối, đang cố gắng kết nối lại: {ex.Message}";
                Console.WriteLine(errorMsg);
                UpdateData("DoworkMess", errorMsg);
            }
            Thread.Sleep(200);
        }
    }

    private void Worker_DoWork(object sender, DoWorkEventArgs e)
    {
        _isRunning = true;

        while (!_worker.CancellationPending)
        {
            // Ví dụ: đọc giá trị double từ PLC tại địa chỉ "D4" (hoặc "31" đối với Modbus)
            try
            {
                // Ví dụ: đọc dữ liệu từ PLC
                string AddCurrentX = ConnectionType == PlcConnectionType.Modbus ? "51" : "D4";
                string AddCurrentY = ConnectionType == PlcConnectionType.Modbus ? "53" : "D8";
                string AddCurrentA = ConnectionType == PlcConnectionType.Modbus ? "55" : "D12";
                VariableRobot.CurrentX = (float)(_plcConnection.ReadDouble(AddCurrentX, true));
                VariableRobot.CurrentY = (float)(_plcConnection.ReadDouble(AddCurrentY, true));
                VariableRobot.CurrentU = (float)(_plcConnection.ReadDouble(AddCurrentA, true));
                UpdateData("CurrentX", VariableRobot.CurrentX);
                UpdateData("CurrentY", VariableRobot.CurrentY);
                UpdateData("CurrentA", VariableRobot.CurrentU);

                //_plcConnection.WriteFloat("61", (float)56.35455, true);
                //_plcConnection.WriteFloat("63", (float)5426.3555455, true);
                //_plcConnection.WriteFloat("65", (float)523456.3544355, true);
                if (_dataStore.TryGetValue("CountPoint", out object CountPoint))
                {
                    if ((int)CountPoint > 0)
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
                                _plcConnection.WriteDouble("61", (float)VariableRobot.PointsX[i], true);
                                _plcConnection.WriteDouble("63", (float)VariableRobot.PointsY[i], true);
                                _plcConnection.WriteDouble("65", (float)VariableRobot.PointsA[i], true);
                                //_plcConnection.WriteDouble("61", (double)(56.35455), true);
                                //_plcConnection.WriteDouble("63", (double)(5426.3555455), true);
                                //_plcConnection.WriteDouble("65", (double)(523456.3544355), true);
                            }
                            catch
                            {

                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                string errorMsg = $"Lỗi trong quá trình đọc dữ liệu: {ex.Message}";
                Console.WriteLine(errorMsg);
                UpdateData("DoworkMess", errorMsg);
                _isRunning = false;
                break;
            }

            Thread.Sleep(50);
        }
    }

    public void Start()
    {
        // Tùy thuộc vào loại kết nối được chọn, khởi tạo đối tượng tương ứng
        if (ConnectionType == PlcConnectionType.Modbus)
        {
            if (_dataStore.TryGetValue("IpAddress", out object sIp) && IsValidIP(sIp.ToString()))
            {
                _plcConnection = new ModbusPlcConnection(sIp.ToString());
                UpdateData("DoworkMess", "Kết nối thành công Modbus");
            }
            else
            {
                UpdateData("DoworkMess", "Địa chỉ IP không hợp lệ hoặc chưa nhập");
                return;
            }
        }
        else if (ConnectionType == PlcConnectionType.ActUtl)
        {
            string errorMessage;
            _plcConnection = new PlcCommunicator();
            _plcConnection.Connect(1, out errorMessage);
            UpdateData("DoworkMess", errorMessage);
        }

        if (!_worker.IsBusy)
            _worker.RunWorkerAsync();
    }

    public void Stop()
    {
        if (_worker.IsBusy)
            _worker.CancelAsync();

        _plcConnection?.Disconnect();
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
        return System.Net.IPAddress.TryParse(ip, out _);
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

