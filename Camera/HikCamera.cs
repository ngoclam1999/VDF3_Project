
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Net;
using System.Threading;
using System.Windows;
using MvCameraControl;

public class Hikcamera : CameraBase
{
    private readonly DeviceTLayerType enumTLayerType = DeviceTLayerType.MvGigEDevice | DeviceTLayerType.MvUsbDevice
        | DeviceTLayerType.MvGenTLGigEDevice | DeviceTLayerType.MvGenTLCXPDevice | DeviceTLayerType.MvGenTLCameraLinkDevice | DeviceTLayerType.MvGenTLXoFDevice;

    private List<IDeviceInfo> deviceInfoList = new List<IDeviceInfo>();
    private IDevice device = null;

    private bool isGrabbing = false;
    private Thread receiveThread = null;
    private IFrameOut frameForSave;
    private readonly object saveImageLock = new object();

    public Hikcamera(string name, string connectionInfo)
    {
        Name = name;
        ConnectionInfo = connectionInfo;
        Status = CameraStatus.Disconnected;
        ErrorCode = 0;

        SDKSystem.Initialize();
        RefreshDeviceList();
    }

    public override void Start(int index)
    {
        if (deviceInfoList.Count == 0)
        {
            throw new Exception("No device available.");
        }

        IDeviceInfo deviceInfo = deviceInfoList[index]; // Chọn camera đầu tiên

        try
        {
            device = DeviceFactory.CreateDevice(deviceInfo);
            int result = device.Open();
            if (result != MvError.MV_OK)
            {
                Status = CameraStatus.Error;
                ErrorCode = result;
                throw new Exception("Open Device fail!");
            }

            // Cấu hình camera

            device.Parameters.SetEnumValueByString("AcquisitionMode", "Continuous");
            device.Parameters.SetEnumValueByString("TriggerSource", "Software");
            device.Parameters.SetEnumValueByString("TriggerMode", "Off");

            Status = CameraStatus.Connected;
        }
        catch (Exception ex)
        {
            Status = CameraStatus.Error;
            ErrorCode = -1;
            throw new Exception("Error starting camera: " + ex.Message);
        }
    }
    public void StartLive()
    {
        if (device == null) throw new Exception("Device not connected.");
        isGrabbing = true;
        receiveThread = new Thread(ReceiveThreadProcess);
        receiveThread.Start();
        device.StreamGrabber.StartGrabbing();
    }

    public void StopLive()
    {
        isGrabbing = false;
        receiveThread?.Join();
        device.StreamGrabber.StopGrabbing();
    }

    public override void Stop()
    {
        if (isGrabbing)
        {
            StopGrabbing();
        }

        if (device != null)
        {
            device.Close();
            device.Dispose();
            device = null;
            Status = CameraStatus.Disconnected;
        }
    }

    public override Bitmap Capture()
    {
        if (!isGrabbing)
        {
            StartGrabbing();
        }

        // Chờ nhận hình ảnh
        lock (saveImageLock)
        {
            if (frameForSave != null)
            {
                Bitmap bitmap = frameForSave.Image.ToBitmap();
                return bitmap;
            }
        }

        return null; // Nếu không có hình ảnh
    }

    public override void SetExposure(int exposure)
    {
        device.Parameters.SetEnumValue("ExposureAuto", 0); // Tắt tự động
        device.Parameters.SetFloatValue("ExposureTime", exposure);
    }

    public override void SetBrightness(int brightness)
    {
        device.Parameters.SetEnumValue("GainAuto", 0); // Tắt tự động
        device.Parameters.SetFloatValue("Gain", brightness);
    }

    public override void SetGain(float gain)
    {
        device.Parameters.SetEnumValue("GainAuto", 0); // Tắt tự động
        device.Parameters.SetFloatValue("Gain", gain);
    }

    private void StartGrabbing()
    {
        isGrabbing = true;
        receiveThread = new Thread(ReceiveThreadProcess);
        receiveThread.Start();

        int result = device.StreamGrabber.StartGrabbing();
        if (result != MvError.MV_OK)
        {
            isGrabbing = false;
            throw new Exception("Start Grabbing Fail!");
        }
    }

    private void StopGrabbing()
    {
        isGrabbing = false;
        receiveThread.Join();

        int result = device.StreamGrabber.StopGrabbing();
        if (result != MvError.MV_OK)
        {
            throw new Exception("Stop Grabbing Fail!");
        }
    }

    private void ReceiveThreadProcess()
    {
        while (isGrabbing)
        {
            IFrameOut frameOut;
            int nRet = device.StreamGrabber.GetImageBuffer(1000, out frameOut);
            if (nRet == MvError.MV_OK)
            {
                lock (saveImageLock)
                {
                    frameForSave = frameOut.Clone() as IFrameOut;
                }
                device.StreamGrabber.FreeImageBuffer(frameOut);
            }
            else
            {
                Thread.Sleep(5); // Nếu có lỗi, đợi một chút trước khi thử lại
            }
        }
    }

    public List<string> RefreshDeviceList()
    {
        string deviceName;
        int nRet = DeviceEnumerator.EnumDevices(enumTLayerType, out deviceInfoList);
        if (nRet != MvError.MV_OK)
        {
            throw new Exception("Enumerate devices fail!");
        }

        List<string> deviceNames = new List<string>();

        // Tạo danh sách tên thiết bị
        foreach (var deviceInfo in deviceInfoList)
        {
            if (string.IsNullOrEmpty(deviceInfo.UserDefinedName))
            {
                deviceName = deviceInfo.ModelName+ " (" + deviceInfo.SerialNumber + ")";
            }
            else
            {
                deviceName = deviceInfo.TLayerType + ": "
                           + deviceInfo.UserDefinedName
                           + " (" + deviceInfo.SerialNumber + ")";
            }

            deviceNames.Add(deviceName);
        }

        return deviceNames;
    }

    public (string Manufacturer, string ModelName, string SerialNumber, string version, string UserDefinedName) GetDeviceInfo(string selectedDeviceName)
    {
        int nRet = DeviceEnumerator.EnumDevices(enumTLayerType, out deviceInfoList);
        if (nRet != MvError.MV_OK)
        {
            throw new Exception("Enumerate devices fail!");
        }

        foreach (var deviceInfo in deviceInfoList)
        {
            string deviceName = string.IsNullOrEmpty(deviceInfo.UserDefinedName)
                ? $"{deviceInfo.ModelName} ({deviceInfo.SerialNumber})"
                : $"{deviceInfo.TLayerType}: {deviceInfo.UserDefinedName} ({deviceInfo.SerialNumber})";

            if (deviceName == selectedDeviceName)
            {
                return (deviceInfo.ManufacturerName,
                        deviceInfo.ModelName,
                        deviceInfo.SerialNumber,
                        deviceInfo.DeviceVersion,
                        deviceInfo.UserDefinedName);
            }
        }

        // Trả về giá trị mặc định nếu không tìm thấy thiết bị
        return ("N/A", "N/A", "N/A", "N/A", "N/A");
    }
    /*
    public void ForceIp(string ip, string subnetMask, string gateway, int deviceIndex)
    {
        if (deviceInfoList.Count == 0 || deviceIndex < 0 || deviceIndex >= deviceInfoList.Count)
        {
            throw new Exception("No device available or invalid device index.");
        }

        if (!IPAddress.TryParse(ip, out IPAddress clsIpAddr) || !IPAddress.TryParse(subnetMask, out IPAddress clsSubMask) || !IPAddress.TryParse(gateway, out IPAddress clsDefaultWay))
        {
            throw new Exception("Invalid IP, subnet mask, or gateway.");
        }

        long nIp = IPAddress.NetworkToHostOrder(clsIpAddr.Address);
        long nSubMask = IPAddress.NetworkToHostOrder(clsSubMask.Address);
        long nDefaultWay = IPAddress.NetworkToHostOrder(clsDefaultWay.Address);

        var deviceInfo = deviceInfoList[deviceIndex];
        device = DeviceFactory.CreateDevice(deviceInfo);
        IGigEDevice gigeDevice = device as IGigEDevice;

        if (gigeDevice == null)
        {
            throw new Exception("Device is not a GigE device.");
        }

        try
        {
            // Kiểm tra nếu thiết bị có thể truy cập
            bool accessible = DeviceEnumerator.IsDeviceAccessible(deviceInfo, DeviceAccessMode.AccessExclusive);
            if (accessible)
            {
                int ret = gigeDevice.SetIpConfig(IpConfigType.Static);
                if (ret != MvError.MV_OK)
                {
                    throw new Exception("Set IP config failed.");
                }
            }

            int forceIpResult = gigeDevice.ForceIp((uint)(nIp >> 32), (uint)(nSubMask >> 32), (uint)(nDefaultWay >> 32));
            if (forceIpResult != MvError.MV_OK)
            {
                throw new Exception("Force IP failed.");
            }
        }
        finally
        {
            gigeDevice.Dispose();
            device = null;
        }
    }
    */
    public void ForceIp(string ip, string subnetMask, string gateway, int deviceIndex)
    {
        if (deviceInfoList.Count == 0)
        {
            MessageBox.Show("No Device");
            return;
        }

        // ch:IP转换 | en:IP conversion
        IPAddress clsIpAddr;
        if (false == IPAddress.TryParse(ip, out clsIpAddr))
        {
            MessageBox.Show("Please enter correct IP");
            return;
        }
        long nIp = IPAddress.NetworkToHostOrder(clsIpAddr.Address);

        // ch:掩码转换 | en:Mask conversion
        IPAddress clsSubMask;
        if (false == IPAddress.TryParse(subnetMask, out clsSubMask))
        {
            MessageBox.Show("Please enter correct IP");
            return;
        }
        long nSubMask = IPAddress.NetworkToHostOrder(clsSubMask.Address);

        // ch:网关转换 | en:Gateway conversion
        IPAddress clsDefaultWay;
        if (false == IPAddress.TryParse(gateway, out clsDefaultWay))
        {
            MessageBox.Show("Please enter correct IP");
            return;
        }
        long nDefaultWay = IPAddress.NetworkToHostOrder(clsDefaultWay.Address);

        if (deviceInfoList == null || deviceInfoList.Count == 0 || deviceInfoList[deviceIndex] == null)
        {
            return;
        }

        int ret = MvError.MV_OK;
        // ch:创建设备 | en:Create device

        device = DeviceFactory.CreateDevice(deviceInfoList[deviceIndex]);
        IGigEDevice gigeDevice = device as IGigEDevice;
        // ch:判断设备IP是否可达 | en: If device ip is accessible
        bool accessible = DeviceEnumerator.IsDeviceAccessible(deviceInfoList[deviceIndex], DeviceAccessMode.AccessExclusive);
        if (accessible)
        {


            ret = gigeDevice.SetIpConfig(IpConfigType.Static);
            if (MvError.MV_OK != ret)
            {
                MessageBox.Show("Set Ip config fail");
                gigeDevice.Dispose();
                device = null;
                return;
            }

            ret = gigeDevice.ForceIp((uint)(nIp >> 32), (uint)(nSubMask >> 32), (uint)(nDefaultWay >> 32));
            if (MvError.MV_OK != ret)
            {
                MessageBox.Show("ForceIp fail");
                gigeDevice.Dispose();
                device = null;
                return;
            }
        }
        else
        {
            ret = gigeDevice.ForceIp((uint)(nIp >> 32), (uint)(nSubMask >> 32), (uint)(nDefaultWay >> 32));
            if (MvError.MV_OK != ret)
            {
                MessageBox.Show("ForceIp fail");
                gigeDevice.Dispose();
                device = null;
                return;
            }
            gigeDevice.Dispose();

            IDeviceInfo deviceInfo = deviceInfoList[deviceIndex];
            IGigEDeviceInfo gigeDevInfo = deviceInfo as IGigEDeviceInfo;

            uint nIp1 = ((gigeDevInfo.NetExport & 0xff000000) >> 24);
            uint nIp2 = ((gigeDevInfo.NetExport & 0x00ff0000) >> 16);
            uint nIp3 = ((gigeDevInfo.NetExport & 0x0000ff00) >> 8);
            uint nIp4 = (gigeDevInfo.NetExport & 0x000000ff);
            string netExportIp = nIp1.ToString() + "." + nIp2.ToString() + "." + nIp3.ToString() + "." + nIp4.ToString();
            //ch:需要重新创建句柄，设置为静态IP方式进行保存 | en:  Need to recreate the handle and set it to static IP mode for saving
            //ch: 创建设备 | en: Create device
            device = DeviceFactory.CreateDeviceByIp(ip, netExportIp);
            if (null == device)
            {
                MessageBox.Show("Create handle fail");
                return;
            }
            gigeDevice = device as IGigEDevice;
            ret = gigeDevice.SetIpConfig(IpConfigType.Static);
            if (MvError.MV_OK != ret)
            {
                MessageBox.Show("Set Ip config fail");
                gigeDevice.Dispose();
                device = null;
                return;
            }
        }
    }    

    public (string IP, string SubnetMask, string Gateway, string Range) GetDeviceNetworkInfo(int deviceIndex)
    {
        if (deviceInfoList == null || deviceInfoList.Count == 0 || deviceIndex < 0 || deviceIndex >= deviceInfoList.Count)
        {
            throw new Exception("Invalid device index or no devices available.");
        }

        IDeviceInfo deviceInfo = deviceInfoList[deviceIndex];
        IGigEDeviceInfo gigeDevInfo = deviceInfo as IGigEDeviceInfo;

        if (gigeDevInfo == null)
        {
            throw new Exception("Selected device is not a GigE device.");
        }

        // Net IP range
        uint nNetIp1 = (gigeDevInfo.NetExport & 0xFF000000) >> 24;
        uint nNetIp2 = (gigeDevInfo.NetExport & 0x00FF0000) >> 16;
        uint nNetIp3 = (gigeDevInfo.NetExport & 0x0000FF00) >> 8;
        string range = $"{nNetIp1}.{nNetIp2}.{nNetIp3}.0 ~ {nNetIp1}.{nNetIp2}.{nNetIp3}.255";

        // Current IP
        uint ip1 = (gigeDevInfo.CurrentIp & 0xFF000000) >> 24;
        uint ip2 = (gigeDevInfo.CurrentIp & 0x00FF0000) >> 16;
        uint ip3 = (gigeDevInfo.CurrentIp & 0x0000FF00) >> 8;
        uint ip4 = gigeDevInfo.CurrentIp & 0x000000FF;
        string ip = $"{ip1}.{ip2}.{ip3}.{ip4}";

        // Subnet Mask
        uint sm1 = (gigeDevInfo.CurrentSubNetMask & 0xFF000000) >> 24;
        uint sm2 = (gigeDevInfo.CurrentSubNetMask & 0x00FF0000) >> 16;
        uint sm3 = (gigeDevInfo.CurrentSubNetMask & 0x0000FF00) >> 8;
        uint sm4 = gigeDevInfo.CurrentSubNetMask & 0x000000FF;
        string subnetMask = $"{sm1}.{sm2}.{sm3}.{sm4}";

        // Gateway
        uint gw1 = (gigeDevInfo.DefultGateWay & 0xFF000000) >> 24;
        uint gw2 = (gigeDevInfo.DefultGateWay & 0x00FF0000) >> 16;
        uint gw3 = (gigeDevInfo.DefultGateWay & 0x0000FF00) >> 8;
        uint gw4 = gigeDevInfo.DefultGateWay & 0x000000FF;
        string gateway = $"{gw1}.{gw2}.{gw3}.{gw4}";

        return (ip, subnetMask, gateway, range);
    }

}
