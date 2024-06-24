using ODTLearning.Models;

namespace ODTLearning.Helpers
{
    public class ImageLibrary
    {
        public async Task<ApiResponse<object>> UploadImage(IFormFile file)
        {
            if (file.Length > 0)
            {
                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Images", file.FileName);

                using (var stream = System.IO.File.Create(path))
                {
                    await file.CopyToAsync(stream);
                }

                if (File.Exists(path))
                {
                    return new ApiResponse<object>
                    {
                        Success = true,
                        Message = "Thành công"
                    };
                }

                new ApiResponse<object>
                {
                    Success = false,
                    Message = "Không tồn tại file đường dẫn"
                };
            }
            
            return new ApiResponse<object>
            {
                Success = false,
                Message = "Không tìm thấy file"
            };
        }

        public async Task<bool> DeleteImage(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
            {
                return false;
            }

            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Images", fileName);

            if (!File.Exists(path))
            {
                return false;
            }

            try
            {
                File.Delete(path);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }


        public string GetImanges(string fileName) => $@"{Directory.GetCurrentDirectory()}/wwwroot/Images/{fileName}";

        //public string GetImanges(string fileName) => Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Images", fileName).ToString();

        //public IFormFile GetImanges(string fileName)
        //{
        //    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Images", fileName);
        //    var imageBytes = System.IO.File.ReadAllBytes(filePath);
        //    var contentType = GetImageContentType(filePath);
        //    return (IFormFile) File(imageBytes, contentType);
        //}

        //private string GetImageContentType(string path)
        //{
        //    var ext = Path.GetExtension(path).ToLowerInvariant();
        //    return ext switch
        //    {
        //        ".jpg" or ".jpeg" => "image/jpeg",
        //        ".png" => "image/png",
        //        ".gif" => "image/gif",
        //        _ => "application/octet-stream",
        //    };
        //}
    }
}
