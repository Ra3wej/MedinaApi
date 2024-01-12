using System.ComponentModel.DataAnnotations;

namespace MedinaApi.Models
{
    public class LanguageSet
    {
        [Key]
        public int Id { get; set; }
        public string English { get; set; }
        public string Kurdish { get; set; }
        public string Arabic { get; set; }
        public bool IsActive { get; set; } = true;
    }
}