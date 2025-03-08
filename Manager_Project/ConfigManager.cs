using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Xml.Linq;
using Newtonsoft.Json;

public static class ConfigManager
{
    private static ConfigItem _config;
    private static readonly object _lock = new object();

    public static ConfigItem Config
    {
        get
        {
            if (_config == null)
            {
                lock (_lock)
                {
                    if (_config == null)
                    {
                        LoadConfig("config.json");
                    }
                }
            }
            return _config;
        }
    }

    /// <summary>
    /// Đọc file JSON và tải cấu hình
    /// </summary>
    public static void LoadConfig(string filePath)
    {
        if (!File.Exists(filePath))
        {
            throw new FileNotFoundException($"File not found: {filePath}");
        }

        string jsonContent = File.ReadAllText(filePath);
        _config = JsonConvert.DeserializeObject<ConfigItem>(jsonContent);
    }

    /// <summary>
    /// Lưu cấu hình hiện tại vào file JSON
    /// </summary>
    public static void SaveConfig(string filePath)
    {
        lock (_lock)
        {
            string jsonContent = JsonConvert.SerializeObject(_config, Formatting.Indented);
            File.WriteAllText(filePath, jsonContent);
        }
    }

    /// <summary>
    /// Truy xuất thông số từ từ điển
    /// </summary>
    public static T GetParameter<T>(Dictionary<string, T> set, string name)
    {
        if (set == null)
        {
            throw new ArgumentNullException(nameof(set), "Parameter set is null.");
        }

        return set.TryGetValue(name, out var param) ? param : default;
    }

    /// <summary>
    /// Truy xuất thông số từ từ điển lồng
    /// </summary>
    public static T GetNestedParameter<T>(Dictionary<string, Dictionary<string, T>> set, string groupName, string paramName)
    {
        if (set == null)
        {
            throw new ArgumentNullException(nameof(set), "Parameter set is null.");
        }

        return set.TryGetValue(groupName, out var group) && group.TryGetValue(paramName, out var param) ? param : default;
    }

    /// <summary>
    /// Cập nhật thông số trong từ điển
    /// </summary>
    public static void UpdateParameter<T>(Dictionary<string, T> set, string name, T updatedParam)
    {
        if (set == null)
        {
            throw new ArgumentNullException(nameof(set), "Parameter set is null.");
        }

        set[name] = updatedParam;
    }

    /// <summary>
    /// Cập nhật thông số trong từ điển lồng
    /// </summary>
    public static void UpdateNestedParameter<T>(Dictionary<string, Dictionary<string, T>> set, string groupName, string paramName, T updatedParam)
    {
        if (set == null)
        {
            throw new ArgumentNullException(nameof(set), "Parameter set is null.");
        }

        if (!set.ContainsKey(groupName))
        {
            set[groupName] = new Dictionary<string, T>();
        }

        set[groupName][paramName] = updatedParam;
    }

    /// <summary>
    /// Used to update parameters for predefined vibration modes.
    /// </summary>
    /// <param name="name">Vibration name needs updating</param>
    /// <param name="_Amplitude">param:new amplitude</param>
    /// <param name="_Frequency">paramz:new Frequency</param>
    /// <param name="_Duration">param:Duration</param>
    public static void UpdataVibName(string name, int _Amplitude, int _Frequency, int _Duration)
    {
        if (string.IsNullOrEmpty(name))
        {
            MessageBox.Show("Vibration name cannot be empty.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }
        else
        {
            var updateVib = new VibrationParameter
            {
                Amplitude = _Amplitude,
                Frequency = _Frequency,
                Duration = _Duration
            };
            ConfigManager.UpdateParameter(ConfigManager.Config.platform_vibrationSet, name, updateVib);

            //MessageBox.Show($"Vibration '{name}' updated successfully.","Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }

    /// <summary>
    /// Used to update parameters for Black Light modes.
    /// </summary>
    /// <param name="_Mode">Switch, Flash,Off</param>
    /// <param name="_Intensity"></param>
    /// <param name="_Color"></param>
    /// <param name="_Duration"></param>
    public static void UpdataBackLight(string _Mode, int _Intensity, int _Color, int _Duration)
    {
        if (string.IsNullOrEmpty(_Mode))
        {
            MessageBox.Show("BlackLight mode cannot be empty.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }
        else
        {
            var updateBlight = new BacklightParameter
            {
                Intensity = _Intensity,
                Color = _Color,
                Duration = _Duration
            };
            ConfigManager.UpdateParameter(ConfigManager.Config.backlight_vibrationSet, _Mode, updateBlight);

            //MessageBox.Show($"Black Light '{_Mode}' updated successfully.","Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="_Mode"></param>
    /// <param name="_Amplitude"></param>
    /// <param name="_hopperFreq"></param>
    /// <param name="_Duration"></param>
    public static void UpdataAutoClear(string _AutoClearMode, int _Amplitude, int _AutoClearFreq, int _Duration, int _DurationOpen, int _DurationClose)
    {
        if (string.IsNullOrEmpty(_AutoClearMode))
        {
            MessageBox.Show("AutoClear mode cannot be empty.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }
        else
        {
            var updateAutoClear = new AutoClearParameter
            {
                Amplitude = _Amplitude,
                AutoClearFreq = _AutoClearFreq,
                Duration = _Duration,
                DurationOpen =_DurationOpen,
                DurationClose = _DurationClose
            };
            ConfigManager.UpdateParameter(ConfigManager.Config.AutoClear_vibrationSet, _AutoClearMode, updateAutoClear);

            //MessageBox.Show($"Black Light '{_Mode}' updated successfully.","Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }

    /// <summary>
    /// Used to update parameters for predefined customsize vibration modes.
    /// </summary>
    /// <param name="groupName">Name of the customsize vibration to update</param>
    /// <param name="VcmName">Name of the engine to be updated</param>
    /// <param name="_Amplitude">param:new amplitude</param>
    /// <param name="_Frequency">param:new Frequency</param>
    /// <param name="_Phase">param:new Phase</param>
    /// <param name="_Duration">param:Duration</param>
    public static void UpdataVibCustomsize(string groupName,string VcmName, int _Amplitude, int _Frequency,int _Phase, int _Duration)
    {
        if (string.IsNullOrEmpty(groupName))
        {
            MessageBox.Show("Vibration name cannot be empty.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }
        else
        {
            if (string.IsNullOrEmpty(VcmName))
            {
                MessageBox.Show("Vibration name cannot be empty.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else
            {
                var updatedParam = new CustomizedVibrationParameter
                {
                    Amplitude = _Amplitude,
                    Frequency = _Frequency,
                    Duration = _Duration,
                    Phase = _Phase
                };
                ConfigManager.UpdateNestedParameter(ConfigManager.Config.CustomizedVibrationSet, groupName, VcmName, updatedParam);

                //MessageBox.Show($"Customized Vibration '{groupName}' -> '{VcmName}' updated successfully.","Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }    
        }
        
    }

    /// <summary>
    /// Used to update the program sequence set.
    /// </summary>
    /// <param name="SequenceName">Name of the SequenceSet to update</param>
    /// <param name="ActionName">Name of the ActionName in sequence updated</param>
    /// <param name="_ActionType">param:new Action Type</param>
    /// <param name="_Vibtype">paramz:new Vibration Type</param>
    /// <param name="_DwellMode">param:new Stop or not Stop</param>
    /// <param name="_DwellValue">param:Dwell value</param>
    public static void UpdataSequenceSet(string SequenceName, string ActionName, int _ActionType, int _Vibtype, int _DwellMode, int _DwellValue)
    {
        if (string.IsNullOrEmpty(SequenceName))
        {
            MessageBox.Show("Sequence name cannot be empty.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }
        else
        {
            if (string.IsNullOrEmpty(ActionName))
            {
                MessageBox.Show("Action name cannot be empty.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else
            {
                var updatedParam = new SequenceParameter
                {
                    ActionType = _ActionType,
                    VibrationType = _Vibtype,
                    DurationMode = _DwellMode,
                    DurationValue = _DwellValue
                };
                ConfigManager.UpdateNestedParameter(ConfigManager.Config.flexfeeder_sequenceSet, SequenceName, ActionName, updatedParam);
            }
        }

    }

    /// <summary>
    /// Read and show vibration name
    /// </summary>
    /// <param name="VibrationName">param:name Vibration Name</param>
    /// <returns>Amplitude,Frequency,Duration</returns>
    public static (int Amplitude, int Frequency, int Duration)? GetVibration(string VibrationName)
    {
        var param = ConfigManager.GetParameter(ConfigManager.Config.platform_vibrationSet, VibrationName);

        if (param != null)
        {
            return (param.Amplitude, param.Frequency, param.Duration);
        }
        return null; // Trả về null nếu không tìm thấy
    }

    /// <summary>
    /// Read and show Customsize vibration
    /// </summary>
    /// <param name="groupName">param:name group</param>
    /// <param name="VcmName">param:name Vcm</param>
    /// <returns>Amplitude,Frequency,Phase,Duration</returns>
    public static (int Amplitude, int Frequency, int Phase, int Duration)? GetCustomsizeVibration(string groupName, string VcmName)
    {
        var param = ConfigManager.GetNestedParameter(ConfigManager.Config.CustomizedVibrationSet, groupName, VcmName);

        if (param != null)
        {
            return (param.Amplitude, param.Frequency, param.Phase, param.Duration);
        }
        return null; // Trả về null nếu không tìm thấy
    }

    /// <summary>
    /// Read and show BackLight
    /// </summary>
    /// <param name="backLight">param:name BackLight mode </param>
    /// <returns>Intensity,Color,Duration</returns>
    public static (int Intensity, int Color, int Duration)? GetBackLightMode(string backLight)
    {
        var param = ConfigManager.GetParameter(ConfigManager.Config.backlight_vibrationSet, backLight);

        if (param != null)
        {
            return (param.Intensity, param.Color, param.Duration);
        }
        return null; // Trả về null nếu không tìm thấy
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="backLight"></param>
    /// <returns></returns>
    public static (int Amplitude, int AutoClearFreq, int Duration, int DurationOpen, int DurationClose)? GetAutoClearMode(string AutoClearMode)
    {
        var param = ConfigManager.GetParameter(ConfigManager.Config.AutoClear_vibrationSet, AutoClearMode);

        if (param != null)
        {
            return (param.Amplitude, param.AutoClearFreq, param.Duration,param.DurationOpen,param.DurationClose);
        }
        return null; // Trả về null nếu không tìm thấy
    }

    /// <summary>
    /// Read and show Sequence set
    /// </summary>
    /// <param name="sequenceName">param:name sequence set</param>
    /// <param name="actionName">param:name action</param>
    /// <returns>ActionType,VibrationType,Durationmode,DurationValue</returns>
    public static (int ActionType, int VibrationType, int DurationMode, int DurationValue)? GetSequenceSet(string sequenceName, string actionName)
    {
        var param = ConfigManager.GetNestedParameter(ConfigManager.Config.flexfeeder_sequenceSet, sequenceName, actionName);

        if (param != null)
        {
            return (param.ActionType, param.VibrationType, param.DurationMode, param.DurationValue);
        }
        return null; // Trả về null nếu không tìm thấy
    }

    public static void UpdateProjectID(int newProjectID)
    {
        if (Config == null)
        {
            throw new InvalidOperationException("Configuration not loaded.");
        }

        Config.ProjectID = newProjectID;
    }

    public static int GetProjectID()
    {
        if (Config == null)
        {
            throw new InvalidOperationException("Configuration not loaded.");
        }
        return Config.ProjectID;
    }

    public static void CoppyAndSaveAs(string sourceFile, string destinationFile, int _newProjectId)
    {
        try
        {
            if (!File.Exists(sourceFile))
            {
                MessageBox.Show("Source file 'config.json' not found!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Sao chép file config ban đầu
            File.Copy(sourceFile, destinationFile, overwrite: true);

            // Load và cập nhật config
            ConfigManager.LoadConfig(destinationFile);
            ConfigManager.UpdateProjectID(_newProjectId);
            ConfigManager.SaveConfig(destinationFile);
        }
        catch (UnauthorizedAccessException)
        {
            MessageBox.Show("You do not have permission to save to the selected folder.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        catch (IOException ex)
        {
            MessageBox.Show($"An error occurred while saving the file: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
