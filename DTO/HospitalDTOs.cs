namespace MedinaApi.DTO
{
    public class UpdateHospitalDto
    {
        public int Id { get; set; }
        public string HospitalName { get; set; }
    }
    public class AddHospitalDto
    {
        public string HospitalName { get; set; }
    }
}
