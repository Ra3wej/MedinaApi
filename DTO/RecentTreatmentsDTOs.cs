using MedinaApi.Models;

namespace MedinaApi.DTO
{
    public class AddRecentTreatmentsDTOs
    {
        public int PatientId { get; set; }
        public int DoctorId { get; set; }
        public int HsopitalId { get; set; }
        public int ChronicDiasesId { get; set; }
    }
    public class UpdateRecentTreatmentsDTOs
    {
        public int Id { get; set; }
        public int PatientId { get; set; }
        public int DoctorId { get; set; }
        public int HsopitalId { get; set; }
        public int ChronicDiasesId { get; set; }
    }
}
