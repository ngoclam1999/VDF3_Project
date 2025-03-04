using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using OpenCvSharp;
using OpenCvSharp.Extensions;

namespace ImageProcessingLib
{
    public class ImageProcessor
    {
        private Mat originalMat;
        private string imagePath;
        private ROI selectedROI;
        private List<System.Drawing.Point> candidatePoints = new List<System.Drawing.Point>();
        private Dictionary<RotatedRect, double> detectedScores = new Dictionary<RotatedRect, double>();
        public List<RotatedRect> DetectedRegions => detectedScores.Keys.ToList();
        public Dictionary<RotatedRect, double> DetectedScores => new Dictionary<RotatedRect, double>(detectedScores);



        public ImageProcessor(string imagePath)
        {
            this.imagePath = imagePath;
            originalMat = Cv2.ImRead(imagePath);
        }
        public Bitmap GetImage()
        {
            return BitmapConverter.ToBitmap(originalMat.Clone());
        }
        public Bitmap FindMatchingRegions(ROI roi, double threshold = 0.5, int mergeDistance = 20)
        {
            originalMat = Cv2.ImRead(imagePath); // Load lại ảnh gốc
            candidatePoints.Clear();
            detectedScores.Clear();

            Mat resultImage = originalMat.Clone();

            using (Mat roiMat = new Mat(originalMat, roi.ToRect()))
            using (Mat result = new Mat())
            {
                Cv2.MatchTemplate(originalMat, roiMat, result, TemplateMatchModes.CCoeffNormed);
                for (int y = 0; y < result.Rows; y+=5)
                {
                    for (int x = 0; x < result.Cols; x+=5)
                    {
                        double similarity = result.At<float>(y, x);
                        if (similarity >= threshold)
                        {
                            candidatePoints.Add(new System.Drawing.Point(x, y));
                        }
                    }
                }
            }

            // Giữ lại các điểm gần nhau có điểm số cao nhất
            candidatePoints = MergeClosePoints(candidatePoints, mergeDistance);

            foreach (var point in candidatePoints)
            {
                RotatedRect bestRegion = default;
                double bestScore = 0;

                for (int angle = 0; angle <= 360; angle += 5)
                {
                    using (Mat rotatedRoi = RotateImage(originalMat, roi, angle))
                    using (Mat result = new Mat())
                    {
                        Cv2.MatchTemplate(originalMat, rotatedRoi, result, TemplateMatchModes.CCoeffNormed);

                        Rect searchArea = new Rect(point.X - 50, point.Y - 50, 100, 100);
                        searchArea = searchArea & new Rect(0, 0, originalMat.Cols, originalMat.Rows);

                        using (Mat searchResult = new Mat(result, searchArea))
                        {
                            double minVal, maxVal;
                            OpenCvSharp.Point minLoc, maxLoc;
                            Cv2.MinMaxLoc(searchResult, out minVal, out maxVal, out minLoc, out maxLoc);

                            if (maxVal > bestScore)
                            {
                                bestScore = maxVal;
                                bestRegion = new RotatedRect(
                                    new Point2f(point.X + maxLoc.X, point.Y + maxLoc.Y),
                                    new Size2f(roi.Width, roi.Height), angle);
                            }
                        }
                    }
                }

                if (bestScore > threshold)
                {
                    detectedScores[bestRegion] = bestScore;
                }
            }

            // Vẽ kết quả lên ảnh
            foreach (var region in detectedScores)
            {
                Cv2.Polylines(resultImage, new List<IEnumerable<OpenCvSharp.Point>>
                {
                    region.Key.Points().Select(p => new OpenCvSharp.Point((int)p.X, (int)p.Y))
                }, true, Scalar.Red, 2);

                string scoreText = $"{region.Value:0.00}";
                Cv2.PutText(resultImage, scoreText, new OpenCvSharp.Point((int)region.Key.Center.X, (int)region.Key.Center.Y),
                            HersheyFonts.HersheySimplex, 0.5, Scalar.Blue, 2);
            }

            return BitmapConverter.ToBitmap(resultImage);
        }

        private Mat RotateImage(Mat src, ROI roi, double angle)
        {
            Mat dst = new Mat();
            using (Mat roiMat = new Mat(src, roi.ToRect()))
            {
                Point2f center = new Point2f(roiMat.Cols / 2, roiMat.Rows / 2);
                Mat rotationMatrix = Cv2.GetRotationMatrix2D(center, angle, 1.0);
                Cv2.WarpAffine(roiMat, dst, rotationMatrix, roiMat.Size());
            }
            return dst;
        }

        private List<System.Drawing.Point> MergeClosePoints(List<System.Drawing.Point> points, int mergeDistance)
        {
            List<System.Drawing.Point> mergedPoints = new List<System.Drawing.Point>();

            while (points.Count > 0)
            {
                var current = points[0];
                var closePoints = points.Where(p =>
                    Math.Sqrt(Math.Pow(p.X - current.X, 2) + Math.Pow(p.Y - current.Y, 2)) <= mergeDistance).ToList();

                var bestPoint = closePoints.First();
                mergedPoints.Add(bestPoint);
                points.RemoveAll(p => closePoints.Contains(p));
            }

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
