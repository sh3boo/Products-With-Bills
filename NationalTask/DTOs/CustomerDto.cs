namespace NationalTask.DTOs
{
    public class CustomerDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public DateTime RegistrationDate { get; set; }
    }

    public class CreateCustomerDto
    {
        public string Name { get; set; } = string.Empty;
    }

    public class CustomerProfileDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public DateTime RegistrationDate { get; set; }
        public int TotalBills { get; set; }
    }
} 