using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;
using VDF3_Solution3; // Dùng BoundingBox từ dự án chính


public class ApiClient
{
    private static readonly HttpClient client = new HttpClient()
    {
        Timeout = TimeSpan.FromMinutes(5)
    };

    /// <summary>
    /// Gửi ảnh lên server để nhận về bounding box dạng json
    /// </summary>
    /// <param name="_image"></param>
    /// <param name="template"></param>
    /// <param name="threshold"></param>
    /// <returns></returns>
    public static async Task<string> MatchTemplate(Image _image, Image template, float threshold = 0.4f)
    {
        using (var content = new MultipartFormDataContent())
        {
            // Chuyển ảnh thành byte[]
            byte[] imageBytes = ImageToByteArray(_image);
            byte[] templateBytes = ImageToByteArray(template);

            //var imageContent = new ByteArrayContent(File.ReadAllBytes(_image));
            //var templateContent = new ByteArrayContent(File.ReadAllBytes(template));

            var imageContent = new ByteArrayContent(imageBytes);
            var templateContent = new ByteArrayContent(templateBytes);

            imageContent.Headers.ContentType = new MediaTypeHeaderValue("image/jpeg");
            templateContent.Headers.ContentType = new MediaTypeHeaderValue("image/jpeg");

            content.Add(imageContent, "image", "image.jpg");
            content.Add(templateContent, "template", "template.jpg");
            content.Add(new StringContent(threshold.ToString()), "threshold");

            try
            {
                HttpResponseMessage response = await client.PostAsync("http://192.168.0.5:8000/api/match-template/?threshold="+threshold.ToString(), content);
                response.EnsureSuccessStatusCode();
                string jsonResponse = await response.Content.ReadAsStringAsync();
                Console.WriteLine(jsonResponse);
                return jsonResponse;
            }
            catch (TaskCanceledException ex)
            {
                MessageBox.Show("Lỗi timeout: Server không phản hồi hoặc kết nối bị gián đoạn.\n" + ex.Message,
                                "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
            catch (HttpRequestException ex)
            {
                MessageBox.Show("Lỗi HTTP: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi không xác định: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }

        }

    }

    public static async Task<(List<List<int>>, float[,], float, float, List<float>)> SendCalibrationRequest(Image image, string jsonString)
    {
        // Địa chỉ FastAPI endpoint
        string url = "http://192.168.0.5:8000/api/calibrate/";

        using (HttpClient client = new HttpClient())
        using (MultipartFormDataContent form = new MultipartFormDataContent())
        {
            try
            {
                // Chuyển ảnh thành byte[]
                byte[] imageBytes = ImageToByteArray(image);
                var imageContent = new ByteArrayContent(imageBytes);
                imageContent.Headers.ContentType = new MediaTypeHeaderValue("image/jpeg");

                // Chuyển chuỗi JSON thành StringContent
                var jsonContent = new StringContent(jsonString, Encoding.UTF8, "application/json");

                // Thêm dữ liệu vào form theo định dạng của cURL:
                // - Key "file" với file ảnh, tên file có thể là "CalibPlate.jpg"
                // - Key "coordinates_file" với JSON, tên file có thể là "test.json"
                form.Add(imageContent, "file", "CalibPlate.jpg");
                form.Add(jsonContent, "coordinates_file", "test.json");

                // Gửi yêu cầu POST
                HttpResponseMessage response = await client.PostAsync(url, form);

                if (response.IsSuccessStatusCode)
                {
                    string responseString = await response.Content.ReadAsStringAsync();
                    JObject jsonData = JObject.Parse(responseString);

                    // Lấy danh sách tọa độ 9 điểm (camera_points)
                    JArray pointsArray = (JArray)jsonData["camera_points"];
                    List<List<int>> cameraPoints = pointsArray.ToObject<List<List<int>>>();

                    // Lấy ma trận homography 3x3
                    JArray homographyArray = (JArray)jsonData["homography_matrix"];
                    float[,] homographyMatrix = new float[3, 3];

                    for (int i = 0; i < 3; i++)
                    {
                        JArray row = (JArray)homographyArray[i];
                        for (int j = 0; j < 3; j++)
                        {
                            homographyMatrix[i, j] = row[j].ToObject<float>();
                        }
                    }

                    // Lấy sai số từng điểm
                    JArray errorsArray = (JArray)jsonData["reprojection_errors"]["errors_per_point"];
                    List<float> errorsPerPoint = errorsArray.ToObject<List<float>>();

                    // Lấy sai số trung bình và lớn nhất
                    float meanError = jsonData["reprojection_errors"]["mean_error"].ToObject<float>();
                    float maxError = jsonData["reprojection_errors"]["max_error"].ToObject<float>();

                    return (cameraPoints, homographyMatrix, maxError, meanError, errorsPerPoint);
                }
                else
                {
                    string errorResponse = await response.Content.ReadAsStringAsync();
                    MessageBox.Show($"Error {response.StatusCode}:\n{errorResponse}", "Server Response", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return (null, null, 0, 0, null);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Request failed: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return (null, null, 0, 0, null);
            }
        }
    }

    // Chuyển Image thành byte[]
    private static byte[] ImageToByteArray(Image image)
    {
        using (var ms = new MemoryStream())
        {
            image.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg); // Lưu ảnh dưới định dạng JPEG
            return ms.ToArray();
        }
    }

}
public class BoundingBoxData
{
    public int Count { get; set; }
    public List<Transform> Matches { get; set; } // Sử dụng BoundingBox từ VDF3_Solution3
}

