namespace NationalTask.DTOs
{
    public class BillDto
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public string CustomerName { get; set; } = string.Empty;
        public DateTime CustomerRegistrationDate { get; set; }
        public int Code { get; set; }
        public DateTime CreatedDate { get; set; }
        public decimal TotalPrice { get; set; }
        public List<BillDetailDto> BillDetails { get; set; } = new List<BillDetailDto>();
    }

    public class CreateBillDto
    {
        public int CustomerId { get; set; }
        public List<CreateBillDetailDto> BillDetails { get; set; } = new List<CreateBillDetailDto>();
    }

    public class BillDetailDto
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice { get; set; }
    }

    public class CreateBillDetailDto
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }
} 