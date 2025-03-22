using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

public static class SystemCongfig
{
    /// <summary>
    /// 
    /// </summary>
    public static string PresentFilePath;
    /// <summary>
    /// 
    /// </summary>
    public static bool _directControl = false;
    /// <summary>
    /// Đường dẫn chương trình đang mở hiện tại
    /// </summary>
    public static string ProjectName;
    /// <summary>
    /// Id của Project
    /// </summary>
    public static string ProjectId;
    /// <summary>
    /// 
    /// </summary>
    public static bool Unlock = false;
    /// <summary>
    /// 
    /// </summary>
    public static bool DebugMode = false;
    /// <summary>
    /// Trạng thái kết nối từ App với thiết bị
    /// </summary>
    public static bool ModbusRunning;
    /// <summary>
    /// Lưu đường dẫn khi mở 1 project bất kỳ
    /// </summary>
   
    public static int addr_ProjectId = 100;
    /// <summary>
    /// Auto/Manu mode
    /// </summary>
    public static int addr_SystemMode = 102;
   
    /// <summary>
    /// Robot control register
    /// </summary>
    public static int addrMW_Control = 31;
    /// <summary>
    /// User defined control register
    /// </summary>
    public static int addrMW_UserControllDefined = 32;
    /// <summary>
    /// Set robot program register
    /// </summary>
    public static int addrMW_SetProgram = 33;
    /// <summary>
    /// Set speed of robot program register
    /// </summary>
    public static int addrMW_SetSPeed = 34;
    /// <summary>
    /// Set accelation of robot register
    /// </summary>
    public static int addrMW_SetAccel = 35;
    /// <summary>
    /// Set decellation of robot register
    /// </summary>
    public static int addrMW_SetDeccel = 36;
    /// <summary>
    /// Set lower limit object register
    /// </summary>
    public static int addrMW_MinObj = 40;

    /// <summary>
    /// Set X coordinate of robot register
    /// </summary>
    public static int addrMW_XCoordinate = 50;
    /// <summary>
    /// Set Y coordinate of robot register
    /// </summary>
    public static int addrMW_YCoordinate = 51;
    /// <summary>
    /// Set X coordinate of robot register
    /// </summary>
    public static int addrMW_ZCoordinate = 52;
    /// <summary>
    /// Set U Angle of robot register
    /// </summary>
    public static int addrMW_UAngle = 53;

    /// <summary>
    /// Robot status register
    /// </summary>
    public static int addrIW_RobotStatus0 = 31;
    /// <summary>
    /// User status register
    /// </summary>
    public static int addrIW_RobotStatus1 = 32;
    /// <summary>
    /// Current program of robot register
    /// </summary>
    public static int addrIW_CurProgram = 33;
    /// <summary>
    /// Current speed of robot program register
    /// </summary>
    public static int addrIW_CurSPeed = 34;
    /// <summary>
    /// Current accelation of robot register
    /// </summary>
    public static int addrIW_CurAccel = 35;
    /// <summary>
    /// Current decellation of robot register
    /// </summary>
    public static int addrIW_CurDeccel = 36;
    /// <summary>
    /// Current remaining object register
    /// </summary>
    public static int addrIW_ObjRemain = 40;
    /// <summary>
    /// Current error code register
    /// </summary>
    public static int addrIW_ErrCode = 50;
}
