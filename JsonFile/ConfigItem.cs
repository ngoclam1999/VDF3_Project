using System.Collections.Generic;

public class ConfigItem
{
    public int ProjectID { get; set; }
   
    public Dictionary<string, VibrationParameter> platform_vibrationSet { get; set; }
    public Dictionary<string, Dictionary<string, CustomizedVibrationParameter>> CustomizedVibrationSet { get; set; }
    public Dictionary<string, AutoClearParameter> AutoClear_vibrationSet { get; set; }

    public Dictionary<string, BacklightParameter> backlight_vibrationSet { get; set; }

    public Dictionary<string, Dictionary<string, SequenceParameter>> flexfeeder_sequenceSet { get; set; }
}
