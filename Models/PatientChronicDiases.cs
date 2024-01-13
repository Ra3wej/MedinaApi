namespace MedinaApi.Models
{
    public class PatientChronicDiases
    {
        public int Id { get; set; }
        public int ChronicDiaseId { get; set; }
        public ChronicDiases ChronicDiase { get; set; }
        public int PatientId { get; set; }
        public Patient Patient { get; set; }
    }
}
