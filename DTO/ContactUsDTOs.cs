using System.ComponentModel.DataAnnotations;

namespace MedinaApi.DTO
{
    public class GetContactUsDto
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        [Phone]
        public string PhoneNumber { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        public string Message { get; set; }
    }
    public class AddContactUsDto
    {
        public string FullName { get; set; }
        [Phone]
        public string PhoneNumber { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        public string Message { get; set; }
    }

}
