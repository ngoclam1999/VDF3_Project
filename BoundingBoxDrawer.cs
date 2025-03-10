using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

public class BoundingBoxDrawer
{
    public static void LoadImageAndDrawBoundingBox(PictureBox picBox, string imagePath, int x, int y, int width, int height, float angle)
    {
        if (!System.IO.File.Exists(imagePath))
        {
            MessageBox.Show("Không tìm thấy ảnh!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }

        // Load ảnh gốc
        Image originalImage = Image.FromFile(imagePath);
        Bitmap bitmap = new Bitmap(originalImage);

        using (Graphics g = Graphics.FromImage(bitmap))
        {
            g.SmoothingMode = SmoothingMode.AntiAlias;

            // Tính tọa độ trung tâm của Bounding Box
            float centerX = x + width / 2f;
            float centerY = y + height / 2f;
            float angleRad = angle * (float)Math.PI / 180f;

            // Xác định 4 đỉnh của Bounding Box trước khi xoay
            PointF[] points = new PointF[]
            {
                new PointF(x, y),
                new PointF(x + width, y),
                new PointF(x + width, y + height),
                new PointF(x, y + height)
            };

            // Xoay các điểm quanh tâm
            for (int i = 0; i < points.Length; i++)
            {
                points[i] = RotatePoint(points[i], centerX, centerY, angleRad);
            }

            // Vẽ Bounding Box xoay
            using (Pen pen = new Pen(Color.Red, 2))
            {
                g.DrawPolygon(pen, points);
            }
        }

        // Hiển thị ảnh đã vẽ Bounding Box lên PictureBox
        picBox.Image = bitmap;
        picBox.SizeMode = PictureBoxSizeMode.Zoom;
    }

    private static PointF RotatePoint(PointF point, float cx, float cy, float angleRad)
    {
        float x = point.X - cx;
        float y = point.Y - cy;

        float newX = (float)(Math.Cos(angleRad) * x - Math.Sin(angleRad) * y + cx);
        float newY = (float)(Math.Sin(angleRad) * x + Math.Cos(angleRad) * y + cy);

        return new PointF(newX, newY);
    }
}
