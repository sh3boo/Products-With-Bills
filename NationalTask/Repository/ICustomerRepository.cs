using NationalTask.DTOs;
using NationalTask.Models;

namespace NationalTask.Repository
{
    public interface ICustomerRepository
    {
        Task<IEnumerable<CustomerDto>> GetAllAsync();
        Task<CustomerDto?> GetByIdAsync(int id);
        Task<CustomerProfileDto?> GetProfileByIdAsync(int id);
        Task<CustomerDto> CreateAsync(CreateCustomerDto createCustomerDto);
        Task<CustomerDto?> UpdateAsync(int id, CreateCustomerDto updateCustomerDto);
        Task<bool> DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
    }
} 