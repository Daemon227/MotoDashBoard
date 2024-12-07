using Microsoft.Build.Framework;
using System.Text;

    namespace DashBoard_MotoManager.Helpers
    {
        public class MyTool
        {

            public static string GenarateRandomKey(int length = 25)
            {
                var partern = @"1234567890qwertyuiopasdfghjklzxcvbnmQWERTYUIOPLKJHGFDAZXCVBNM";
                var sb = new StringBuilder();
                var rd = new Random();
                for (int i = 0; i < length; i++)
                {
                    sb.Append(partern[rd.Next(0, partern.Length)]);
                }
                return sb.ToString();
            }

            public static string UploadImage(IFormFile file, string folder)
        {
            try
            {
                if (file != null)
                {
                    // Lấy tên file và thêm một dấu thời gian để tạo sự khác biệt
                    var fileName = Path.GetFileNameWithoutExtension(file.FileName);
                    var fileExtension = Path.GetExtension(file.FileName);
                    var uniqueFileName = $"{fileName}_{DateTime.Now.ToString("yyyyMMddHHmmssfff")}{fileExtension}";

                    var fullPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "img", folder, uniqueFileName);

                    // Kiểm tra và tạo thư mục nếu nó chưa tồn tại
                    var directory = Path.GetDirectoryName(fullPath);
                    if (!Directory.Exists(directory))
                    {
                        Directory.CreateDirectory(directory);
                    }
                    // Lưu file
                    using (var myfile = new FileStream(fullPath, FileMode.Create))
                    {
                        file.CopyTo(myfile);
                    }
                    return uniqueFileName;
                }
                else
                {
                    return "anhnull";
                }
            }
            catch (Exception ex)
            {
                // Ghi log lỗi tại đây nếu cần
                return string.Empty;
            }
        }

            public static void DeleteImg(string fileName,string folder) 
        {
            try
            {
                if (fileName != null)
                {
                    var imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "img", folder, fileName);
                    if (System.IO.File.Exists(imagePath))
                    {
                        System.IO.File.Delete(imagePath); // Xóa ảnh
                    }
                    else return;
                }
            }
            catch (Exception ex) 
            {
                throw new Exception("Lỗi khi xóa ảnh: " + ex.Message, ex);
            }
        }

            
        }
    }
