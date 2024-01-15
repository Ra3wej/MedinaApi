using MedinaApi.Models;

namespace MedinaApi.DTO
{
    public class AddPatientResultsDto
    {
        public int PatientId { get; set; }
        public string FileUrl { get; set; }
    }
    public class GetPatientResultsDto
    {
        public int Id { get; set; }
        public int PatientId { get; set; }
        public string FileUrl { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
