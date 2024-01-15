using System.Text.Json.Serialization;

namespace MedinaApi.Models
{
    public class PatientResults
    {
        public int Id { get; set; }
        public int PatientId { get; set; }
        [JsonIgnore]
        public Patient Patient { get; set; }
        public string FileUrl { get; set; }
        public DateTime CreatedAt { get; set; }= DateTime.Now;
    }
}
