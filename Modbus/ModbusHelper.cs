using EasyModbus;
using VDF3_Solution3;
using System;
using System.IO.Ports;
using System.Threading;
using System.Threading.Tasks;
using System.Net.NetworkInformation;
using System.Windows.Forms;
using System.IO;
using System.Net.Sockets;

namespace VDF3_Solution3
{
    public class ModbusHelper
    {
        private ModbusClient modbusClient;

        // Sự kiện
        public event EventHandler<string> OnConnected;
        public event EventHandler<string> OnDisconnected;
        public event EventHandler<string> OnErrorOccurred;
        public event EventHandler<int[]> OnDataRead;
        public event EventHandler<string> OnDataWritten;
        public event EventHandler<bool[]> OnCoilsRead;
        public event EventHandler<string> OnCoilsWritten;

        #region Constructor

        // Constructor cho Modbus TCP
        public ModbusHelper(string ipAddress, int port)
        {
            modbusClient = new ModbusClient(ipAddress, port);
            modbusClient.ConnectionTimeout = 5000; // 10 seconds timeout
        }

        #endregion

        #region Kết nối và tự động kết nối lại
        public async Task<bool> ConnectAsync()
        {
            try
            {
                if (!NetworkInterface.GetIsNetworkAvailable())
                {
                    OnErrorOccurred?.Invoke(this, "Network is not available. Please check the cable or connection.");
                    MessageBox.Show("Network is unavailable. Please check your cable or connection.", "Connection error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }

                if (modbusClient.Connected)
                {
                    modbusClient.Disconnect();
                }

                await Task.Run(() => modbusClient.Connect());
                OnConnected?.Invoke(this, "Connected successfully.");
                return true; // Kết nối thành công
            }
            catch (Exception ex)
            {
                OnErrorOccurred?.Invoke(this, $"Connection failed: {ex.Message}");
                MessageBox.Show($"Connection failed: {ex.Message}", "Connection error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false; // Kết nối thất bại
            }
        }

        // Ngắt kết nối
        public void Disconnect()
        {
            try
            {
                if (modbusClient.Connected)
                {
                    modbusClient.Disconnect();
                    SystemCongfig.ModbusRunning = false;
                    OnDisconnected?.Invoke(this, "Disconnected successfully.");
                }
            }
            catch (Exception ex)
            {
                OnErrorOccurred?.Invoke(this, $"Disconnection error: {ex.Message}");
            }
        }

        // Kiểm tra kết nối
        public bool IsConnected()
        {
            if (modbusClient == null)
            {
                // Handle the null case, maybe log a message or return false
                return false;
            }

            return modbusClient.Connected;
        }

        // Tự động kết nối lại khi mất kết nối
        public async Task AutoReconnectAsync()
        {
            FrSetting fr = new FrSetting();
            int retryCount = 0;
            const int maxRetries = 5;
            const int retryDelay = 1000; // 5 seconds

            // Lần đầu thử kết nối
            if (!await ConnectAsync())
            {
                OnErrorOccurred?.Invoke(this, "Initial connection failed. Will attempt to reconnect.");
            }

            // Thử kết nối lại tối đa 5 lần
            while (!modbusClient.Connected && retryCount < maxRetries)
            {
                retryCount++;
                //fr.($"Retrying connection... Attempt {retryCount} of {maxRetries}");
                await Task.Delay(retryDelay);

                if (await ConnectAsync())
                {
                    //fr.LogMessage("Reconnected successfully.");
                    SystemCongfig.ModbusRunning = true;
                    return;
                }
            }

            // Nếu hết lần thử mà vẫn không kết nối được
            if (!modbusClient.Connected)
            {
                //fr.LogMessage("Failed to reconnect after multiple attempts.");
                OnErrorOccurred?.Invoke(this, "Failed to reconnect after multiple attempts.");
            }
        }

        #endregion

        #region Đọc và ghi dữ liệu Modbus

        // Đọc một Coil
        public async Task<bool> ReadSingleCoilAsync(int address)
        {
            try
            {
                if (!modbusClient.Connected)
                    throw new Exception("Not connected to Modbus server.");

                // Đọc coil
                bool[] data = await Task.Run(() =>
                {
                    return modbusClient.ReadCoils(address, 1);
                });

                return data[0];
            }
            catch (Exception ex)
            {
                OnErrorOccurred?.Invoke(this, $"Error reading single coil: {ex.Message}");
                throw;
            }
        }

        // Đọc nhiều Coil
        public async Task<bool[]> ReadMultipleCoilsAsync(int startingAddress, int numberOfCoils)
        {
            try
            {
                if (!modbusClient.Connected)
                    throw new Exception("Not connected to Modbus server.");

                // Đọc nhiều coil
                bool[] data = await Task.Run(() =>
                {
                    return modbusClient.ReadCoils(startingAddress, numberOfCoils);
                });

                // Kích hoạt sự kiện OnCoilsRead
                OnCoilsRead?.Invoke(this, data);
                return data;
            }
            catch (Exception ex)
            {
                OnErrorOccurred?.Invoke(this, $"Error reading multiple coils: {ex.Message}");
                throw;
            }
        }

        // Ghi một Coil
        public async Task WriteSingleCoilAsync(int address, bool value)
        {
            try
            {
                if (!modbusClient.Connected)
                    throw new Exception("Not connected to Modbus server.");

                // Ghi coil
                await Task.Run(() =>
                {
                    modbusClient.WriteSingleCoil(address, value);
                });

                // Kích hoạt sự kiện OnCoilsWritten
                OnCoilsWritten?.Invoke(this, $"Wrote value {value} to coil at address {address}.");
            }
            catch (SocketException ex)
            {
                // Xử lý lỗi SocketException
                Console.WriteLine($"Lỗi socket: {ex.Message}");
            }
            catch (IOException ex)
            {
                // Xử lý lỗi IOException
                Console.WriteLine($"Lỗi IO: {ex.Message}");
            }
            catch (Exception ex)
            {
                OnErrorOccurred?.Invoke(this, $"Error writing single coil: {ex.Message}");
                throw;
            }
        }

        // Ghi nhiều Coil
        public async Task WriteMultipleCoilsAsync(int startingAddress, bool[] values)
        {
            try
            {
                if (!modbusClient.Connected)
                    throw new Exception("Not connected to Modbus server.");

                // Ghi nhiều coil
                await Task.Run(() =>
                {
                    modbusClient.WriteMultipleCoils(startingAddress, values);
                });

                // Kích hoạt sự kiện OnCoilsWritten
                OnCoilsWritten?.Invoke(this, $"Wrote {values.Length} coils starting at address {startingAddress}.");
            }
            catch (SocketException ex)
            {
                // Xử lý lỗi SocketException
                Console.WriteLine($"Lỗi socket: {ex.Message}");
            }
            catch (IOException ex)
            {
                // Xử lý lỗi IOException
                Console.WriteLine($"Lỗi IO: {ex.Message}");
            }
            catch (Exception ex)
            {
                OnErrorOccurred?.Invoke(this, $"Error writing multiple coils: {ex.Message}");
                throw;
            }
        }

        // Đọc nhiều thanh ghi
        public async Task<int[]> ReadMultipleRegistersAsync(int startingAddress, int numberOfRegisters)
        {
            try
            {
                if (!modbusClient.Connected)
                    throw new Exception("Not connected to Modbus server.");

                int[] data = await Task.Run(() => modbusClient.ReadHoldingRegisters(startingAddress, numberOfRegisters));

                OnDataRead?.Invoke(this, data);
                return data;
            }
            catch (Exception ex)
            {
                OnErrorOccurred?.Invoke(this, $"Error reading registers: {ex.Message}");
                throw;
            }
        }

        // Đọc một thanh ghi
        public async Task<int> ReadSingleRegisterAsync(int address)
        {
            int[] data;
            try
            {
                if (!modbusClient.Connected)
                {
                    throw new Exception("Not connected to Modbus server.");
                }
                else
                {
                    data = await Task.Run(() =>
                    modbusClient.ReadHoldingRegisters(address, 1));
                }
                OnDataRead?.Invoke(this, data);
                return data[0];
            }

            catch (Exception ex)
            {
                OnErrorOccurred?.Invoke(this, $"Error reading single register: {ex.Message}");
                throw;
            }
        }

        // Ghi một thanh ghi
        public async Task WriteSingleRegisterAsync(int address, int value)
        {
            try
            {
                if (!modbusClient.Connected)
                    throw new Exception("Not connected to Modbus server.");

                await Task.Run(() => modbusClient.WriteSingleRegister(address, value));

                OnDataWritten?.Invoke(this, $"Wrote value {value} to register {address}.");
            }
            catch (SocketException ex)
            {
                // Xử lý lỗi SocketException
                Console.WriteLine($"Lỗi socket: {ex.Message}");
            }
            catch (IOException ex)
            {
                // Xử lý lỗi IOException
                Console.WriteLine($"Lỗi IO: {ex.Message}");
            }
            catch (Exception ex)
            {
                OnErrorOccurred?.Invoke(this, $"Error writing single register: {ex.Message}");
                throw;
            }
        }

        // Ghi nhiều thanh ghi
        public async Task WriteMultipleRegistersAsync(int startingAddress, int[] values)
        {
            try
            {
                if (!modbusClient.Connected)
                    throw new Exception("Not connected to Modbus server.");

                await Task.Run(() => modbusClient.WriteMultipleRegisters(startingAddress, values));

                OnDataWritten?.Invoke(this, $"Wrote {values.Length} registers starting at address {startingAddress}.");
            }
            catch (SocketException ex)
            {
                // Xử lý lỗi SocketException
                Console.WriteLine($"Lỗi socket: {ex.Message}");
            }
            catch (IOException ex)
            {
                // Xử lý lỗi IOException
                Console.WriteLine($"Lỗi IO: {ex.Message}");
            }
            catch (Exception ex)
            {
                OnErrorOccurred?.Invoke(this, $"Error writing multiple registers: {ex.Message}");
                throw;
            }
        }

        #endregion

        #region Chuyển đổi dữ liệu Modbus

        public int ConvertRegistersToInt(int[] registers) => ModbusClient.ConvertRegistersToInt(registers);

        public float ConvertRegistersToFloat(int[] registers) => ModbusClient.ConvertRegistersToFloat(registers);

        public int[] ConvertIntToRegisters(int value) => ModbusClient.ConvertIntToRegisters(value);

        public int[] ConvertFloatToRegisters(float value) => ModbusClient.ConvertFloatToRegisters(value);

        #endregion
    }
}
