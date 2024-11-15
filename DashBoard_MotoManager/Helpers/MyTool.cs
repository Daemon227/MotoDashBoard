using Microsoft.Build.Framework;
using System.Text;

    namespace DashBoard_MotoManager.Helpers
    {
        public class MyTool
        {
            public static string GenarateRandomKey(int length = 25)
            {
                var partern = @"1234567890qwertyuiopasdfghjklzxcvbnm@!?QWERTYUIOPLKJHGFDAZXCVBNM";
                var sb = new StringBuilder();
                var rd = new Random();
                for (int i = 0; i < length; i++)
                {
                    sb.Append(partern[rd.Next(0, partern.Length)]);
                }
                return sb.ToString();
            }

        /*public static string UploadHinh(IFormFile file, string folder)
        {
            try
            {
                if(file != null)
                {
                    var fullPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "img", folder, file.FileName);
                    using (var myfile = new FileStream(fullPath, FileMode.CreateNew))
                    {
                        file.CopyTo(myfile);
                    }
                    return file.FileName;
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
        }*/

            public static string UploadHinh(IFormFile file, string folder)
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

            public static IFormFile ConvertFileToIFormFile(string fileName, string folder)
        {
            string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "img", folder, fileName);
            // Kiểm tra nếu file tồn tại
            if (!File.Exists(filePath))
            {
                return null;
            }

            // Đọc dữ liệu từ file và tạo IFormFile
            var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            var memoryStream = new MemoryStream();

            fileStream.CopyTo(memoryStream);
            fileStream.Close();

            memoryStream.Position = 0; // Đặt vị trí về đầu stream

            // Lấy tên file từ đường dẫn
            var contentType = "application/octet-stream"; // Hoặc định dạng chính xác như "image/jpeg" nếu là ảnh

            // Tạo đối tượng IFormFile từ MemoryStream
            IFormFile formFile = new FormFile(memoryStream, 0, memoryStream.Length, null, fileName)
            {
                Headers = new HeaderDictionary(),
                ContentType = contentType
            };
            return formFile;
        }
        }
    }
