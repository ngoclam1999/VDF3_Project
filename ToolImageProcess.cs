using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using OpenCvSharp;
using OpenCvSharp.Extensions;

namespace ImageProcessingLib
{
    public class ImageProcessor
    {
        private Mat originalMat;
        private string imagePath;
        private List<System.Drawing.Point> candidatePoints = new List<System.Drawing.Point>();
        private Dictionary<RotatedRect, double> detectedScores = new Dictionary<RotatedRect, double>();
        public List<RotatedRect> DetectedRegions => detectedScores.Keys.ToList();
        public Dictionary<RotatedRect, double> DetectedScores => new Dictionary<RotatedRect, double>(detectedScores);

        public ImageProcessor(string imagePath)
        {
            this.imagePath = imagePath;
            originalMat = Cv2.ImRead(imagePath);
        }

        public Bitmap GetImage() => BitmapConverter.ToBitmap(originalMat.Clone());

        public Bitmap FindMatchingRegions(ROI roi, double threshold = 0.4, int mergeDistance = 20)
        {
            originalMat = Cv2.ImRead(imagePath);
            candidatePoints.Clear();
            detectedScores.Clear();
            Mat resultImage = originalMat.Clone();

            using (Mat roiMat = new Mat(originalMat, roi.ToRect()))
            using (Mat result = new Mat())
            {
                Cv2.MatchTemplate(originalMat, roiMat, result, TemplateMatchModes.CCoeffNormed);

                for (int y = 0; y < result.Rows; y += Math.Max(5, roi.Height / 10))
                {
                    for (int x = 0; x < result.Cols; x += Math.Max(5, roi.Width / 10))
                    {
                        double similarity = result.At<float>(y, x);
                        if (similarity >= threshold)
                        {
                            candidatePoints.Add(new System.Drawing.Point(x, y));
                        }
                    }
                }
            }

            candidatePoints = MergeClosePoints(candidatePoints, mergeDistance);

            Parallel.ForEach(candidatePoints, point =>
            {
                RotatedRect bestRegion = default;
                double bestScore = 0;

                for (int angle = 0; angle <= 180; angle += 10)
                {
                    using (Mat rotatedRoi = RotateImage(originalMat, roi, angle))
                    using (Mat result = new Mat())
                    {
                        Cv2.MatchTemplate(originalMat, rotatedRoi, result, TemplateMatchModes.CCoeffNormed);

                        Rect searchArea = new Rect(10, 10, 100, 100) & new Rect(0, 0, originalMat.Cols, originalMat.Rows);

                        using (Mat searchResult = new Mat(result, searchArea))
                        {
                            Cv2.MinMaxLoc(searchResult, out _, out double maxVal, out _, out OpenCvSharp.Point maxLoc);

                            if (maxVal > bestScore)
                            {
                                bestScore = maxVal;
                                bestRegion = new RotatedRect(
                                    new Point2f(point.X + maxLoc.X, point.Y + maxLoc.Y),
                                    new Size2f(roi.Width, roi.Height), angle);
                            }
                        }
                    }

                    if (bestScore > threshold)
                    {
                        lock (detectedScores)
                        {
                            detectedScores[bestRegion] = bestScore;
                        }
                    }
                }
            });
            
            foreach (var region in detectedScores)
            {
                Cv2.Polylines(resultImage, new List<IEnumerable<OpenCvSharp.Point>>
                {
                    region.Key.Points().Select(p => new OpenCvSharp.Point((int)p.X, (int)p.Y))
                }, true, Scalar.Red, 2);

                Cv2.PutText(resultImage, $"{region.Value:0.00}", new OpenCvSharp.Point((int)region.Key.Center.X, (int)region.Key.Center.Y),
                            HersheyFonts.HersheySimplex, 0.5, Scalar.Blue, 2);
            }

            return BitmapConverter.ToBitmap(resultImage);
        }

        private Mat RotateImage(Mat src, ROI roi, double angle)
        {
            // Trích xuất ROI từ ảnh gốc
            Mat roiMat = new Mat(src, roi.ToRect());

            // Tạo một ảnh vuông đủ lớn để chứa ROI xoay
            int maxDim = (int)Math.Ceiling(Math.Sqrt(roi.Width * roi.Width + roi.Height * roi.Height));
            Mat paddedRoi = new Mat(new OpenCvSharp.Size(maxDim, maxDim), roiMat.Type(), Scalar.Black);

            // Tính toán vị trí đặt ROI vào giữa ảnh mới
            int xOffset = (maxDim - roi.Width) / 2;
            int yOffset = (maxDim - roi.Height) / 2;
            roiMat.CopyTo(new Mat(paddedRoi, new Rect(xOffset, yOffset, roi.Width, roi.Height)));

            // Xoay ảnh quanh tâm của nó
            Point2f center = new Point2f(maxDim / 2f, maxDim / 2f);
            Mat rotationMatrix = Cv2.GetRotationMatrix2D(center, angle, 1.0);
            Mat rotatedRoi = new Mat();
            Cv2.WarpAffine(paddedRoi, rotatedRoi, rotationMatrix, paddedRoi.Size(), InterpolationFlags.Linear, BorderTypes.Transparent);

            // Cắt ROI đã xoay về đúng kích thước ban đầu
            int cropX = (maxDim - roi.Width) / 2;
            int cropY = (maxDim - roi.Height) / 2;
            return new Mat(rotatedRoi, new Rect(cropX, cropY, roi.Width, roi.Height));
        }


        private List<System.Drawing.Point> MergeClosePoints(List<System.Drawing.Point> points, int mergeDistance)
        {
            if (points.Count == 0) return new List<System.Drawing.Point>();

            points = points.OrderBy(p => p.X).ThenBy(p => p.Y).ToList();
            List<System.Drawing.Point> mergedPoints = new List<System.Drawing.Point>();
            System.Drawing.Point current = points[0];

            foreach (var point in points.Skip(1))
            {
                if (Math.Abs(point.X - current.X) <= mergeDistance && Math.Abs(point.Y - current.Y) <= mergeDistance)
                {
                    continue;
                }
                mergedPoints.Add(current);
                current = point;
            }
            mergedPoints.Add(current);
            return mergedPoints;
        }
    }

    public class ROI
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }

        public ROI(int x, int y, int width, int height)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
        }

        public Rect ToRect() => new Rect(X, Y, Width, Height);
    }
}
