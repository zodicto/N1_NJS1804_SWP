namespace ODTLearning.Helpers
{
    public interface IImageLibrary
    {
        public Task<bool> UploadImage(IFormFile file);
        public Task<bool> DeleteImage(string fileName);
        public string GetImanges(string fileName) => $@"{Directory.GetCurrentDirectory()}/wwwroot/Images/{fileName}";
    }
}
