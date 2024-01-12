using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MedinaApi.Models
{
    public class DashboardUser
    {

        [Key]
        public int Id { get; set; }
        public string UserName { get; set; }
        [EmailAddress]
        public string EmailAddress { get; set; }
        public string Password { get; set; }
        public string Salt { get; set; }
        public bool IsActive { get; set; } = true;
        public bool IsSuperUser { get; set; }=false;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
    }
}
