using System;
using System.IO;
using System.Text;
using Microsoft.AspNetCore.Http;

namespace MyFinalExam.Areas.Admin.HelperAdmin
{
    public class MyUtil
    {
        public static string UploadImage(IFormFile Hinh, string folder)
        {
            try
            {
                var directory = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Hinh", folder);
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                var fullPath = Path.Combine(directory, Hinh.FileName);
                using (var myfile = new FileStream(fullPath, FileMode.CreateNew))
                {
                    Hinh.CopyTo(myfile);
                }
                return Hinh.FileName;
            }
            catch (Exception ex)
            {
                // Xử lý ngoại lệ cụ thể hoặc ghi log
                Console.WriteLine(ex.Message);
                return string.Empty;
            }
        }

        public static string GenerateRamdomKey(int length = 5)
        {
            var pattern = @"qazwsxedcrfvtgbyhnujmiklopQAZWSXEDCRFVTGBYHNUJMIKLOP!";
            var sb = new StringBuilder();
            var rd = new Random();
            for (int i = 0; i < length; i++)
            {
                sb.Append(pattern[rd.Next(0, pattern.Length)]);
            }

            return sb.ToString();
        }
    }
}
