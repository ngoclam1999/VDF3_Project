using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Threading;
using Basler.Pylon;

namespace VDF3_Solution3
{
    public class BaslerCamera
    {
        private Camera camera = null;
        private PixelDataConverter converter = new PixelDataConverter();
        private bool isGrabbing = false;
        private Thread liveThread = null;
        private Bitmap currentFrame = null;
        private readonly object frameLock = new object();

        // Thuộc tính bổ sung (có thể dùng để lưu tên, thông tin kết nối,…)
        public string Name { get; set; }
        public string ConnectionInfo { get; set; }
        public int ErrorCode { get; private set; }
        public bool IsConnected => camera != null && camera.IsOpen;

        public BaslerCamera(string name, string connectionInfo)
        {
            Name = name;
            ConnectionInfo = connectionInfo;
            ErrorCode = 0;
        }

        #region Các hàm cơ bản

        /// <summary>
        /// Lấy danh sách tên thân thiện của các camera Basler khả dụng.
        /// </summary>
        public List<string> RefreshDeviceList()
        {
            List<string> deviceNames = new List<string>();
            List<ICameraInfo> allCameras = CameraFinder.Enumerate();
            foreach (ICameraInfo info in allCameras)
            {
                string friendlyName = info[CameraInfoKey.FriendlyName];
                deviceNames.Add(friendlyName);
            }
            return deviceNames;
        }

        /// <summary>
        /// Mở kết nối tới camera theo chỉ số trong danh sách thiết bị.
        /// </summary>
        public void Open(int index)
        {
            List<ICameraInfo> allCameras = CameraFinder.Enumerate();
            if (allCameras.Count == 0)
            {
                throw new Exception("No device available.");
            }
            if (index < 0 || index >= allCameras.Count)
            {
                throw new Exception("Invalid device index.");
            }
            ICameraInfo selectedCameraInfo = allCameras[index];
            try
            {
                camera = new Camera(selectedCameraInfo);
                camera.Open();
                // Cấu hình chế độ Acquisition về Continuous nếu có thể
                if (camera.Parameters.Contains(PLCamera.AcquisitionMode) &&
                    camera.Parameters[PLCamera.AcquisitionMode].CanSetValue("Continuous"))
                {
                    camera.Parameters[PLCamera.AcquisitionMode].SetValue("Continuous");
                }
                // Tắt trigger nếu có
                if (camera.Parameters.Contains(PLCamera.TriggerMode))
                {
                    camera.Parameters[PLCamera.TriggerMode].SetValue("Off");
                }
            }
            catch (Exception ex)
            {
                ErrorCode = -1;
                throw new Exception("Error opening camera: " + ex.Message);
            }
        }

        /// <summary>
        /// Đóng kết nối và giải phóng tài nguyên camera.
        /// </summary>
        public void Close()
        {
            StopLive();
            if (camera != null)
            {
                if (camera.IsOpen)
                {
                    camera.Close();
                }
                camera.Dispose();
                camera = null;
            }
        }

        /// <summary>
        /// Chụp một khung hình đơn và trả về dưới dạng Bitmap.
        /// </summary>
        public Bitmap Capture()
        {
            if (camera == null || !camera.IsOpen)
                throw new Exception("Camera not open.");

            try
            {
                camera.StreamGrabber.Start(1, GrabStrategy.OneByOne, GrabLoop.ProvidedByStreamGrabber);
                IGrabResult grabResult = camera.StreamGrabber.RetrieveResult(5000, TimeoutHandling.ThrowException);
                if (grabResult.GrabSucceeded)
                {
                    Bitmap bitmap = new Bitmap(grabResult.Width, grabResult.Height, PixelFormat.Format32bppRgb);
                    BitmapData bmpData = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height),
                                                           ImageLockMode.ReadWrite,
                                                           bitmap.PixelFormat);
                    converter.OutputPixelFormat = PixelType.BGRA8packed;
                    IntPtr ptrBmp = bmpData.Scan0;
                    converter.Convert(ptrBmp, bmpData.Stride * bitmap.Height, grabResult);
                    bitmap.UnlockBits(bmpData);
                    grabResult.Dispose();
                    return bitmap;
                }
                else
                {
                    string errorMsg = grabResult.ErrorDescription;
                    grabResult.Dispose();
                    throw new Exception("Failed to capture image: " + errorMsg);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error capturing image: " + ex.Message);
            }
        }

        /// <summary>
        /// Hàm alias cho Capture() để dễ sử dụng.
        /// </summary>
        public Bitmap ReadImage()
        {
            return Capture();
        }

        /// <summary>
        /// Kích hoạt trigger phần mềm nếu camera hỗ trợ.
        /// </summary>
        public void Trigger()
        {
            if (camera == null || !camera.IsOpen)
                throw new Exception("Camera not open.");

            if (camera.Parameters.Contains(PLCamera.TriggerSoftware))
            {
                camera.Parameters[PLCamera.TriggerSoftware].Execute();
            }
            else
            {
                throw new Exception("Camera does not support software trigger.");
            }
        }

        /// <summary>
        /// Bắt đầu chế độ live view (acquisition liên tục) bằng cách khởi chạy một luồng riêng.
        /// </summary>
        public void StartLive()
        {
            if (camera == null || !camera.IsOpen)
                throw new Exception("Camera not open.");

            isGrabbing = true;
            liveThread = new Thread(LiveGrabThread);
            liveThread.Start();
            camera.StreamGrabber.Start(GrabStrategy.OneByOne, GrabLoop.ProvidedByStreamGrabber);
        }

        /// <summary>
        /// Dừng chế độ live view.
        /// </summary>
        public void StopLive()
        {
            isGrabbing = false;
            if (liveThread != null && liveThread.IsAlive)
            {
                liveThread.Join();
            }
            if (camera != null && camera.StreamGrabber.IsGrabbing)
            {
                camera.StreamGrabber.Stop();
            }
        }

        /// <summary>
        /// Luồng grab ảnh cho live view: mỗi khung hình grab được chuyển sang Bitmap và lưu vào currentFrame.
        /// </summary>
        private void LiveGrabThread()
        {
            while (isGrabbing)
            {
                try
                {
                    IGrabResult grabResult = camera.StreamGrabber.RetrieveResult(1000, TimeoutHandling.Return);
                    if (grabResult != null && grabResult.GrabSucceeded)
                    {
                        Bitmap bitmap = new Bitmap(grabResult.Width, grabResult.Height, PixelFormat.Format32bppRgb);
                        BitmapData bmpData = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height),
                                                               ImageLockMode.ReadWrite,
                                                               bitmap.PixelFormat);
                        converter.OutputPixelFormat = PixelType.BGRA8packed;
                        IntPtr ptrBmp = bmpData.Scan0;
                        converter.Convert(ptrBmp, bmpData.Stride * bitmap.Height, grabResult);
                        bitmap.UnlockBits(bmpData);
                        lock (frameLock)
                        {
                            currentFrame?.Dispose();
                            currentFrame = bitmap;
                        }
                    }
                    grabResult?.Dispose();
                }
                catch (Exception)
                {
                    // Ghi log lỗi nếu cần
                }
            }
        }

        /// <summary>
        /// Lấy ảnh live view mới nhất.
        /// </summary>
        public Bitmap GetLiveImage()
        {
            lock (frameLock)
            {
                return currentFrame != null ? (Bitmap)currentFrame.Clone() : null;
            }
        }

        #endregion

        #region Các hàm bổ sung

        /// <summary>
        /// Lấy thông tin thiết bị: Vendor, Model, Firmware.
        /// </summary>
        public string GetDeviceInformation()
        {
            if (camera == null || !camera.IsOpen)
                throw new Exception("Camera not open.");

            string vendor = camera.Parameters[PLCamera.DeviceVendorName].GetValue();
            string model = camera.Parameters[PLCamera.DeviceModelName].GetValue();
            string firmware = camera.Parameters[PLCamera.DeviceFirmwareVersion].GetValue();

            return $"Vendor: {vendor}\nModel: {model}\nFirmware: {firmware}";
        }

        /// <summary>
        /// Cấu hình AOI (Area of Interest): đưa OffsetX, OffsetY về giá trị tối thiểu và đặt Width, Height.
        /// </summary>
        public bool ConfigureAOI(int width, int height)
        {
            if (camera == null || !camera.IsOpen)
                throw new Exception("Camera not open.");

            bool success = true;
            if (camera.Parameters[PLCamera.OffsetX].IsWritable)
                success &= camera.Parameters[PLCamera.OffsetX].TrySetToMinimum();
            if (camera.Parameters[PLCamera.OffsetY].IsWritable)
                success &= camera.Parameters[PLCamera.OffsetY].TrySetToMinimum();

            success &= camera.Parameters[PLCamera.Width].TrySetValue(width, IntegerValueCorrection.Nearest);
            success &= camera.Parameters[PLCamera.Height].TrySetValue(height, IntegerValueCorrection.Nearest);
            return success;
        }

        /// <summary>
        /// Đặt Pixel Format của camera (ví dụ: "Mono8", "BayerRG8", ...).
        /// </summary>
        public bool SetPixelFormat(string pixelFormat)
        {
            if (camera == null || !camera.IsOpen)
                throw new Exception("Camera not open.");
            return camera.Parameters[PLCamera.PixelFormat].TrySetValue(pixelFormat);
        }

        /// <summary>
        /// Lấy Pixel Format hiện tại của camera.
        /// </summary>
        public string GetPixelFormat()
        {
            if (camera == null || !camera.IsOpen)
                throw new Exception("Camera not open.");
            return camera.Parameters[PLCamera.PixelFormat].GetValue();
        }

        /// <summary>
        /// Tắt chế độ tự động gain (GainAuto) nếu có.
        /// </summary>
        public bool SetAutoGainOff()
        {
            if (camera == null || !camera.IsOpen)
                throw new Exception("Camera not open.");
            if (camera.Parameters.Contains(PLCamera.GainAuto))
                return camera.Parameters[PLCamera.GainAuto].TrySetValue(PLCamera.GainAuto.Off);
            return false;
        }

        /// <summary>
        /// Đặt giá trị gain theo tỷ lệ phần trăm của khoảng giá trị cho phép.
        /// Sử dụng GainRaw cho các camera cũ hoặc PLUsbCamera.Gain cho các camera mới.
        /// </summary>
        public bool SetGainPercent(double percent)
        {
            if (camera == null || !camera.IsOpen)
                throw new Exception("Camera not open.");

            if (camera.Parameters.Contains(PLCamera.GainRaw))
            {
                return camera.Parameters[PLCamera.GainRaw].TrySetValuePercentOfRange(percent);
            }
            else if (camera.Parameters.Contains(PLUsbCamera.Gain))
            {
                return camera.Parameters[PLUsbCamera.Gain].TrySetValuePercentOfRange(percent);
            }
            return false;
        }

        /// <summary>
        /// Đặt thời gian phơi sáng theo tỷ lệ phần trăm của khoảng giá trị cho phép.
        /// </summary>
        public bool SetExposurePercent(double percent)
        {
            if (camera == null || !camera.IsOpen)
                throw new Exception("Camera not open.");

            if (camera.Parameters.Contains(PLCamera.ExposureTimeAbs))
            {
                return camera.Parameters[PLCamera.ExposureTimeAbs].TrySetValuePercentOfRange(percent);
            }
            return false;
        }

        /// <summary>
        /// Bật hoặc tắt Gamma nếu tham số có sẵn.
        /// </summary>
        public bool SetGamma(bool enable)
        {
            if (camera == null || !camera.IsOpen)
                throw new Exception("Camera not open.");
            if (camera.Parameters.Contains(PLCamera.GammaEnable))
            {
                return camera.Parameters[PLCamera.GammaEnable].TrySetValue(enable);
            }
            return false;
        }

        /// <summary>
        /// Ví dụ: Lấy giá trị CenterX, đảo ngược (toggle) giá trị đó rồi khôi phục lại.
        /// </summary>
        public bool ToggleCenterX()
        {
            if (camera == null || !camera.IsOpen)
                throw new Exception("Camera not open.");
            bool originalValue = camera.Parameters[PLCamera.CenterX].GetValueOrDefault(false);
            bool toggled = camera.Parameters[PLCamera.CenterX].TrySetValue(!originalValue);
            bool restored = camera.Parameters[PLCamera.CenterX].TrySetValue(originalValue);
            return toggled && restored;
        }

        /// <summary>
        /// Ví dụ: Truy cập một tham số không có trong danh sách chuẩn (ví dụ "BrandNewFeature")
        /// và đặt giá trị của nó về giá trị tối đa.
        /// </summary>
        public bool SetBrandNewFeatureToMaximum(string featureName)
        {
            if (camera == null || !camera.IsOpen)
                throw new Exception("Camera not open.");
            IIntegerParameter param = camera.Parameters[featureName] as IIntegerParameter;
            if (param != null)
            {
                return param.TrySetToMaximum();
            }
            return false;
        }

        #endregion
    }
}
