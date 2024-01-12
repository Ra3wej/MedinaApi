namespace MedinaApi.DTO
{
    public class ImageUploadDTO
    {
        public bool Uploaded { get; set; }
        public bool WasEmpty { get; set; } = false;
        public string Name { get; set; }
    }
    public class UploadFileDto
    {
        public string? FileName { get; set; }
    }
    public class UploadImageDTO
    {
        public IFormFile file { get; set; }
    }
}
