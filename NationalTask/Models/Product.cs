using System.ComponentModel.DataAnnotations;

namespace NationalTask.Models
{
    public class Product
    {
        public int Id { get; set; }
        
        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;
        
        [Required]
        [Range(0, double.MaxValue)]
        public decimal Price { get; set; }
        
        public virtual ICollection<BillDetail> BillDetails { get; set; } = new List<BillDetail>();
    }
} 