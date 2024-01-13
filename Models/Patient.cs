using System.ComponentModel.DataAnnotations;
using System.Data;

namespace MedinaApi.Models
{
    public class Patient
    {

        [Key]
        public int Id { get; set; }
        public Guid GuidKey { get; set; }=Guid.NewGuid();
        public string? NationalCardId { get; set; }
        public string? PassportId { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string SecondName { get; set; }
        [Required]
        public string ThirdName { get; set; }

        [Required]
        public string PhoneNumber { get; set; }
        public string? FamilyPhoneNumber { get; set; }
        public string CountryCode { get; set; } = "+964";
        public DateTime BirthDate { get; set; }
        public byte Gender { get; set; }
        public bool IsActive { get; set; } = true;
        public bool Deceased { get; set; } = true;
        public DateTime CreatedAt { get; set; }=DateTime.Now;
    }
}
