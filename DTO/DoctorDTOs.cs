using System.Text.Json.Serialization;

namespace MedinaApi.DTO
{
    public class UpdateDoctorDto
    {
        public Guid Id { get; set; }
        public string? NationalCardId { get; set; }
        public string? PassportId { get; set; }
        public string FirstName { get; set; }
        public string SecondName { get; set; }
        public string ThirdName { get; set; }
        public DateTime BirthDate { get; set; }
        public byte Gender { get; set; }
        public bool IsActive { get; set; }
        public bool Deceased { get; set; }
    }

    public class AddDoctorDto
    {
        public string? NationalCardId { get; set; }
        public string? PassportId { get; set; }
        [JsonRequired]
        public string FirstName { get; set; }
        [JsonRequired]
        public string SecondName { get; set; }
        [JsonRequired]
        public string ThirdName { get; set; }
        public DateTime BirthDate { get; set; }
        [JsonRequired]
        public byte Gender { get; set; }
    }

    public class GetDoctorDetailDto
    {
        public Guid Id { get; set; }
        public string? NationalCardId { get; set; }
        public string? PassportId { get; set; }
        public string FirstName { get; set; }
        public string SecondName { get; set; }
        public string ThirdName { get; set; }

        public DateTime BirthDate { get; set; }
        public byte Gender { get; set; }
        public bool IsActive { get; set; }
        public bool Deceased { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
