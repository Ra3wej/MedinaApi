namespace MedinaApi.DTO
{
    public class UpdateChronicDiasesDto
    {
        public int Id { get; set; }
        public string DiasesName { get; set; }
    }
    public class AddChronicDiasesDto
    {
        public string DiasesName { get; set; }
    }
    public class AddChronicDiasesForPatientDto
    {
        public int DiasesId { get; set; }
        public string? UserNationalid { get; set; }
        public string? Passport { get; set; }
    }
}
