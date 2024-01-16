namespace MedinaApi.Models
{
    public class RecentTreatments
    {
        public int Id { get; set; }
        public int PatientId { get; set; }
        public Patient Patient { get; set; }
        public int DoctorId { get; set; }
        public Doctors Doctor { get; set; }
        public int HospitalId { get; set; }
        public Hospital Hospital { get; set; }
        public int ChronicDiasesId { get; set; }
        public ChronicDiases ChronicDiases { get; set; }
        public DateTime CreatedAt { get; set; }= DateTime.Now;
    }
}
