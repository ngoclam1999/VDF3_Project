using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;
using VDF3_Solution3; // Dùng BoundingBox từ dự án chính


public class ApiClient
{
    private static readonly HttpClient client = new HttpClient();
   
    public static async Task<string> MatchTemplate(PictureBox _image, PictureBox template, float threshold = 0.4f)
    {
        using (var content = new MultipartFormDataContent())
        {
            // Chuyển ảnh thành byte[]
            byte[] imageBytes = ImageToByteArray(_image.Image);
            byte[] templateBytes = ImageToByteArray(template.Image);

            var imageContent = new ByteArrayContent(imageBytes);
            var templateContent = new ByteArrayContent(templateBytes);

            imageContent.Headers.ContentType = new MediaTypeHeaderValue("image/jpeg");
            templateContent.Headers.ContentType = new MediaTypeHeaderValue("image/jpeg");

            content.Add(imageContent, "image", "image.jpg");
            content.Add(templateContent, "template", "template.jpg");
            content.Add(new StringContent(threshold.ToString()), "threshold");

            try
            {
                HttpResponseMessage response = await client.PostAsync("http://192.168.2.5:8000/api/match-template/?threshold="+threshold.ToString(), content);
                response.EnsureSuccessStatusCode();
                string jsonResponse = await response.Content.ReadAsStringAsync();
                Console.WriteLine(jsonResponse);
                return jsonResponse;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi gửi ảnh: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
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

