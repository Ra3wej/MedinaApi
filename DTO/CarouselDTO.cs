namespace MedinaApi.DTO
{
    public class GetCarouselDto
    {
        public string FileUrl { get; set; }
        public short Type { get; set; }
        public int Id { get; set; }
    }
    public class GetCarouselDashboardDto
    {
        public int Id { get; set; }
        public string FileUrl { get; set; }
        public short Type { get; set; }
        public bool IsActive { get; set; }
    }
    public class AddCarouselDto
    {
        public IFormFile File { get; set; }
        public short Type { get; set; }
    }
    public class UpdateCarouselDto
    {
        public int Id { get; set; }
        public IFormFile File { get; set; }

    }
}
