using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;

public class Transform
{
    // Ma trận biến đổi
    public Matrix Matrix { get; private set; } = new Matrix();

    // Constructor
    public Transform()
    {
        Matrix = new Matrix();
    }

    // Reset ma trận về trạng thái ban đầu
    public void Reset()
    {
        Matrix.Reset();
    }

    // Áp dụng phép dịch chuyển (translate)
    public void Translate(float dx, float dy)
    {
        Matrix.Translate(dx, dy);
    }

    // Áp dụng phép xoay (rotate)
    public void Rotate(float angle, float centerX, float centerY)
    {
        Matrix.Translate(centerX, centerY); // Dịch chuyển đến tâm
        Matrix.Rotate(angle);               // Xoay
        Matrix.Translate(-centerX, -centerY); // Dịch chuyển trở lại
    }

    // Áp dụng phép scale
    public void Scale(float scaleX, float scaleY, float centerX, float centerY)
    {
        Matrix.Translate(centerX, centerY); // Dịch chuyển đến tâm
        Matrix.Scale(scaleX, scaleY);       // Scale
        Matrix.Translate(-centerX, -centerY); // Dịch chuyển trở lại
    }

    // Áp dụng ma trận biến đổi lên Graphics
    public void Apply(Graphics g)
    {
        g.Transform = Matrix;
    }
}

