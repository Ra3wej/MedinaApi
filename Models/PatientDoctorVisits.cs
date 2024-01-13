namespace MedinaApi.Models
{
    public class PatientDoctorVisits
    {
        public int Id { get; set; }
        public int DoctorId { get; set; }
        public Doctors Doctor { get; set; }
        public int PatientId { get; set; }
        public Patient Patient { get; set; }
        public DateTime CreatedAt { get; set; }= DateTime.Now;
    }
}
