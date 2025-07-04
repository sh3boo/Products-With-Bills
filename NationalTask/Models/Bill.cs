using System.ComponentModel.DataAnnotations;

namespace NationalTask.Models
{
    public class Bill
    {
        public int Id { get; set; }
        
        [Required]
        public int CustomerId { get; set; }
        
        [Required]
        public int Code { get; set; }
        
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        
        public virtual Customer Customer { get; set; } = null!;
        public virtual ICollection<BillDetail> BillDetails { get; set; } = new List<BillDetail>();
    }
} 