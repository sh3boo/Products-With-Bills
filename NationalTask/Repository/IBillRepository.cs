using NationalTask.DTOs;

namespace NationalTask.Repository
{
    public interface IBillRepository
    {
        Task<IEnumerable<BillDto>> GetAllAsync();
        Task<BillDto?> GetByIdAsync(int id);
        Task<IEnumerable<BillDto>> GetByCustomerIdAsync(int customerId);
        Task<BillDto> CreateAsync(CreateBillDto createBillDto);
        Task<bool> DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
        Task<int> GetNextBillCodeAsync();
    }
} 