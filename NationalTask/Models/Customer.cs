using System.ComponentModel.DataAnnotations;

namespace NationalTask.Models
{
    public class Customer
    {
        public int Id { get; set; }
        
        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;
        
        public DateTime RegistrationDate { get; set; } = DateTime.Now;
        
        public virtual ICollection<Bill> Bills { get; set; } = new List<Bill>();
    }
} 