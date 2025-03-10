using System;
using System.Collections.Generic;
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
    /*
    public static async Task<List<VDF3_Solution3.BoundingBox>> MatchTemplate(string imagePath, string templatePath, float threshold = 0.4f)
    {
        
        if (!File.Exists(imagePath) || !File.Exists(templatePath))
        {
            MessageBox.Show("Không tìm thấy file ảnh!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return null;
        }

        using (var content = new MultipartFormDataContent())
        {
            var imageContent = new ByteArrayContent(File.ReadAllBytes(imagePath));
            var templateContent = new ByteArrayContent(File.ReadAllBytes(templatePath));

            imageContent.Headers.ContentType = new MediaTypeHeaderValue("image/jpeg");
            templateContent.Headers.ContentType = new MediaTypeHeaderValue("image/jpeg");

            content.Add(imageContent, "image", Path.GetFileName(imagePath));
            content.Add(templateContent, "template", Path.GetFileName(templatePath));
            content.Add(new StringContent(threshold.ToString()), "threshold");
            Console.WriteLine(content);

            try
            {
                HttpResponseMessage response = await client.PostAsync("http://192.168.2.5:8000/api/match-template/?threshold=0.4", content);
                response.EnsureSuccessStatusCode();
                string jsonResponse = await response.Content.ReadAsStringAsync();
                Console.WriteLine(jsonResponse);
                var data = JsonSerializer.Deserialize<BoundingBoxData>(jsonResponse);
                Console.WriteLine(data);
                return data?.Matches ?? new List<VDF3_Solution3.BoundingBox>();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi gửi ảnh: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }

        }

    }
    */
    public static async Task<string> MatchTemplate(string imagePath, string templatePath, float threshold = 0.6f)
    {

        if (!File.Exists(imagePath) || !File.Exists(templatePath))
        {
            MessageBox.Show("Không tìm thấy file ảnh!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return null;
        }

        using (var content = new MultipartFormDataContent())
        {
            var imageContent = new ByteArrayContent(File.ReadAllBytes(imagePath));
            var templateContent = new ByteArrayContent(File.ReadAllBytes(templatePath));

            imageContent.Headers.ContentType = new MediaTypeHeaderValue("image/jpeg");
            templateContent.Headers.ContentType = new MediaTypeHeaderValue("image/jpeg");

            content.Add(imageContent, "image", Path.GetFileName(imagePath));
            content.Add(templateContent, "template", Path.GetFileName(templatePath));
            content.Add(new StringContent(threshold.ToString()), "threshold");
            Console.WriteLine(content);

            try
            {
                HttpResponseMessage response = await client.PostAsync("http://192.168.2.5:8000/api/match-template/?threshold=0.2", content);
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

}
public class BoundingBoxData
{
    public int Count { get; set; }
    public List<VDF3_Solution3.BoundingBox> Matches { get; set; } // Sử dụng BoundingBox từ VDF3_Solution3
}

