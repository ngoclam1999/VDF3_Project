using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public static class VariableRobot
{
    /// <summary>
    /// Notice status from robots
    /// </summary>
    public static int MessFormRbt;
    /// <summary>
    /// Robot's mode
    /// </summary>
    public static int RbtMode;
    /// <summary>
    /// The coordinate of the X axis of the current robot
    /// </summary>
    public static float CurrentX;
    /// <summary>
    /// The coordinate of the Y axis of the current robot
    /// </summary>
    public static float CurrentY;
    /// <summary>
    /// The coordinate of the Z axis of the current robot
    /// </summary>
    public static float CurrentZ;
    /// <summary>
    /// The axis angle A of the current robot
    /// </summary>
    public static float CurrentU;
    //------------------------------------------------------------
    /* MA TRẬN CHUYỂN ĐỔI CÓ DẠNG
     * [R11, R12, Tx]
     * [R21, R22, Ty]
     * [  0,   0,  1]
     * TÍNH TOÁN TỌA ĐỘ
     * VÍ DỤ VỊ TRÍ CAMERA LÀ [Xc] , VÀ GÓC XOAY CAMERA XÁC ĐỊNH ĐƯỢC LÀ THETAc
     *                        [Yc]
     *                        [1 ]
     *  TÍNH TỌA ĐỘ ROBOT                      
     * [Xr]   [R11, R12, Tx]   [Xc]
     * [Yr] = [R21, R22, Ty] * [Yc]
     * [ 1]   [  0,   0,  1]   [ 1]
     *TỌA ĐỘ TRỤC X
     * => Xr = R11 * Xc + R12 * Yc + Tx
     *TỌA ĐỘ TRỤC Y
     * => Yr = R21 * Xc + R22 * Yc + Ty
     * TÍNH GÓC XOAY ROBOT TỪ GOC CAMERA THETAc
     * => THETAr = THETAc + ANTAN2(R21/R11)
     */

    /// <summary>
    /// Tính tọa độ robot từ ma trận chuyển đổi
    /// R11: 0,0
    /// double x_r = R11 * x_c + R12 * y_c + Tx;
    /// </summary>
    public static double R11;
    /// <summary>
    /// R12: 0,1
    /// double x_r = R11 * x_c + R12 * y_c + Tx;
    /// </summary>
    public static double R12;
    /// <summary>
    /// Tx: 0,2
    /// double x_r = R11 * x_c + R12 * y_c + Tx;
    /// </summary>
    public static double Tx;
    /// <summary>
    /// R21: 1,0
    /// double y_r = R21 * x_c + R22 * y_c + Ty;
    /// </summary>
    public static double R21;
    /// <summary>
    /// R22: 1,1
    /// double y_r = R21 * x_c + R22 * y_c + Ty;
    /// </summary>
    public static double R22;
    /// <summary>
    /// Ty: 1,2
    /// double y_r = R21 * x_c + R22 * y_c + Ty;
    /// </summary>
    public static double Ty;
    /// <summary> 
    /// Góc xoay của robot tính ra theta_r = theta_c + Math.Atan2(R21, R11) * (180 / Math.PI) (đơn vị độ)
    /// </summary>
    public static double theta_r;

}