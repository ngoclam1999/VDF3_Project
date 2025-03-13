using System;
using System.Collections.Concurrent;
using System.ComponentModel;
using System.Net;
using System.Threading;
using EasyModbus;
using static OpenCvSharp.FileStorage;

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
        _worker.RunWorkerCompleted += _worker_RunWorkerCompleted; ;
    }

    private void _worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
    {
   
        if (_isRunning)
        {
            _modbusClient.Disconnect();
            _worker.CancelAsync();
        }
        else
        {
            UpdateData("DoworkMess", "Mất kết nối");
            _workerReload = new BackgroundWorker();
            _workerReload.WorkerSupportsCancellation = true;
            _workerReload.DoWork += _workerReload_DoWork;
            _workerReload.RunWorkerCompleted += _workerReload_RunWorkerCompleted;
            _workerReload.RunWorkerAsync();
        }

    }

    private void _workerReload_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
    {
        UpdateData("DoworkMess", "Mất kết nối, thành công kết nối lại");
    }

    private void _workerReload_DoWork(object sender, DoWorkEventArgs e)
    {
        while (_modbusClient.Connected)
        {
            try
            {
                _modbusClient.Connect();
                if (_modbusClient.Connected)
                {
                    _worker.RunWorkerAsync();
                    break;
                }
            }
            catch (Exception ex)
            {
                UpdateData("DoworkMess", "Mất kết nối, đang thực hiện kết nối lại");
            }
        }
    }

    private void Worker_DoWork(object sender, DoWorkEventArgs e)
    {
        _isRunning = true;

        while (!_worker.CancellationPending)
        {
            do
            {
                if (_dataStore.TryGetValue("IpAddress", out object sIp))
                {
                    if(!IsValidIP(sIp.ToString()))
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

                if(_modbusClient == null)
                {
                    _modbusClient = new ModbusClient();
                    // cấu hình mặc định modbus
                    _modbusClient.IPAddress = sIp.ToString();
                    _modbusClient.Port = Convert.ToInt32(502);
                    _modbusClient.ConnectionTimeout = Convert.ToInt32(5000);
                    _modbusClient.Connect();
                }
                
            }
            while (!_modbusClient.Connected);
            UpdateData("DoworkMess", "Kết nối Modbus thành công");

            try
            {
                if (_dataStore.TryGetValue("InputData", out object inputData)) // Kiểm tra nếu có dữ liệu từ UI
                {
                    //Xử lý dữ liệu nhận được từ UI và gửi đi qua modbus
                }

                VariableRobot.RbtMode = _modbusClient.ReadHoldingRegisters(187, 1)[0];
                UpdateData("RbtMode", VariableRobot.RbtMode);

                Thread.Sleep(100); // Giảm tải CPU
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi: {ex.Message}");
                _isRunning = false;
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

