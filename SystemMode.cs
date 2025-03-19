using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public static class SystemMode
{
    /// <summary>
    /// 
    /// </summary>
    public static string PresentImagePath;
    /// <summary>
    /// 
    /// </summary>
    public static string PresentTemplatePath;
    /// <summary>
    /// 
    /// </summary>
    public static Image PresentImage;
    /// <summary>
    /// 
    /// </summary>
    public static Image ImgTemplate;
    /// <summary>
    /// 0: Tool Not use
    /// 1: Vacum
    /// 2: Gripper
    /// </summary>
    public static int ToolMode;
    /// <summary>
    /// Diameter
    /// </summary>
    public static int VacumPad_Diameter;
    /// <summary>
    /// distance when gripper is close
    /// </summary>
    public static int GripDistance_Close;
    /// <summary>
    /// distance when gripper is open
    /// </summary>
    public static int GripDistance_Open;
    /// <summary>
    /// Grip thickness
    /// </summary>
    public static int Grip_Thickness;


}
