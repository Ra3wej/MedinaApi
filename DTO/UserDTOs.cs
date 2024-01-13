

using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace MedinaApi.DTO
{
    public class UserLogInDto
    {
        public string PhoneNumber { get; set; }
        public string? PassportId { get; set; }
        public string? NationalCardId { get; set; }
    }
    public class PatientLogInResponse
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string SecondName { get; set; }
        public string ThirdName { get; set; }
        public string PhoneNumber { get; set; }
        public string Token { get; set; }
    }
    public class AddPatientDto
    {
        public string? NationalCardId { get; set; }
        public string? PassportId { get; set; }
        [JsonRequired]
        public string FirstName { get; set; }
        [JsonRequired]
        public string SecondName { get; set; }
        [JsonRequired]
        public string ThirdName { get; set; }

        [JsonRequired]
        public string PhoneNumber { get; set; }
        [JsonRequired]
        public string CountryCode { get; set; } = "+964";
        [JsonRequired]
        public DateTime BirthDate { get; set; }
        [JsonRequired]
        public byte Gender { get; set; }
        [JsonRequired]
        public string? FamilyPhoneNumber { get; set; }
    }

    public class GetpatientDetailDto
    {
        public Guid Id { get; set; }
        public string? NationalCardId { get; set; }
        public string? PassportId { get; set; }
        public string FirstName { get; set; }
        public string SecondName { get; set; }
        public string ThirdName { get; set; }

        public string PhoneNumber { get; set; }
        public string CountryCode { get; set; } = "+964";
        public DateTime BirthDate { get; set; }
        public byte Gender { get; set; }
        public bool IsActive { get; set; }
        public bool Deceased { get; set; }
        public DateTime CreatedAt { get; set; }

    }
    //public class GetNormalUserDTO {
    //    public int Id { get; set; }
    //    public string FirstName { get; set; }
    //    public string SecondName { get; set; }
    //    public string ThirdName { get; set; }
    //    public DateTime CreatedAt { get; set; }
    //    public DateTime UpdatedAt { get; set; }

    //    public string PhoneNumber { get; set; }
    //    public bool IsPhoneNumberConfirmed { get; set; }

    //    public DateTime BirthDate { get; set; }

    //    public byte Gender { get; set; }
    //    public bool IsActive { get; set; } = true;

    //    public string? UserImage { get; set; }

    //}
    public class VerifyUserDTO
    {
        public string PhoneNumber { get; set; }
        public string? PassportId { get; set; }
        public string? NationalCardId { get; set; }
        public string OtpCode { get; set; }
    }
    public class AddUserDTO { }
    public class DeleteUserDTO { }
    public class UpdateUserDTO
    {
        public Guid Id { get; set; }
        public string? NationalCardId { get; set; }
        public string? PassportId { get; set; }
        public string FirstName { get; set; }
        public string SecondName { get; set; }
        public string ThirdName { get; set; }
        public string PhoneNumber { get; set; }
        public string CountryCode { get; set; }
        public DateTime BirthDate { get; set; }
        public byte Gender { get; set; }
        public bool IsActive { get; set; }
        public bool Deceased { get; set; }
        public string? FamilyPhoneNumber { get; set; }
    }
}
