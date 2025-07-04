using Microsoft.EntityFrameworkCore;
using NationalTask.Data;
using NationalTask.DTOs;
using NationalTask.Models;

namespace NationalTask.Repository
{
    public class BillRepository : IBillRepository
    {
        private readonly ApplicationDbContext _context;

        public BillRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<BillDto>> GetAllAsync()
        {
            var bills = await _context.Bills
                .Include(b => b.Customer)
                .Include(b => b.BillDetails)
                .ThenInclude(bd => bd.Product)
                .ToListAsync();

            return bills.Select(b => new BillDto
            {
                Id = b.Id,
                CustomerId = b.CustomerId,
                CustomerName = b.Customer.Name,
                CustomerRegistrationDate = b.Customer.RegistrationDate,
                Code = b.Code,
                CreatedDate = b.CreatedDate,
                TotalPrice = b.BillDetails.Sum(bd => bd.TotalPrice),
                BillDetails = b.BillDetails.Select(bd => new BillDetailDto
                {
                    Id = bd.Id,
                    ProductId = bd.ProductId,
                    ProductName = bd.Product.Name,
                    Quantity = bd.Quantity,
                    UnitPrice = bd.UnitPrice,
                    TotalPrice = bd.TotalPrice
                }).ToList()
            });
        }

        public async Task<BillDto?> GetByIdAsync(int id)
        {
            var bill = await _context.Bills
                .Include(b => b.Customer)
                .Include(b => b.BillDetails)
                .ThenInclude(bd => bd.Product)
                .FirstOrDefaultAsync(b => b.Id == id);

            if (bill == null) return null;

            return new BillDto
            {
                Id = bill.Id,
                CustomerId = bill.CustomerId,
                CustomerName = bill.Customer.Name,
                CustomerRegistrationDate = bill.Customer.RegistrationDate,
                Code = bill.Code,
                CreatedDate = bill.CreatedDate,
                TotalPrice = bill.BillDetails.Sum(bd => bd.TotalPrice),
                BillDetails = bill.BillDetails.Select(bd => new BillDetailDto
                {
                    Id = bd.Id,
                    ProductId = bd.ProductId,
                    ProductName = bd.Product.Name,
                    Quantity = bd.Quantity,
                    UnitPrice = bd.UnitPrice,
                    TotalPrice = bd.TotalPrice
                }).ToList()
            };
        }

        public async Task<IEnumerable<BillDto>> GetByCustomerIdAsync(int customerId)
        {
            var bills = await _context.Bills
                .Include(b => b.Customer)
                .Include(b => b.BillDetails)
                .ThenInclude(bd => bd.Product)
                .Where(b => b.CustomerId == customerId)
                .ToListAsync();

            return bills.Select(b => new BillDto
            {
                Id = b.Id,
                CustomerId = b.CustomerId,
                CustomerName = b.Customer.Name,
                CustomerRegistrationDate = b.Customer.RegistrationDate,
                Code = b.Code,
                CreatedDate = b.CreatedDate,
                TotalPrice = b.BillDetails.Sum(bd => bd.TotalPrice),
                BillDetails = b.BillDetails.Select(bd => new BillDetailDto
                {
                    Id = bd.Id,
                    ProductId = bd.ProductId,
                    ProductName = bd.Product.Name,
                    Quantity = bd.Quantity,
                    UnitPrice = bd.UnitPrice,
                    TotalPrice = bd.TotalPrice
                }).ToList()
            });
        }

        public async Task<BillDto> CreateAsync(CreateBillDto createBillDto)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var nextCode = await GetNextBillCodeAsync();

                var bill = new Bill
                {
                    CustomerId = createBillDto.CustomerId,
                    Code = nextCode,
                    CreatedDate = DateTime.Now
                };

                _context.Bills.Add(bill);
                await _context.SaveChangesAsync();

                var products = await _context.Products
                    .Where(p => createBillDto.BillDetails.Select(bd => bd.ProductId).Contains(p.Id))
                    .ToDictionaryAsync(p => p.Id, p => p);

                var billDetails = createBillDto.BillDetails.Select(bd => new BillDetail
                {
                    BillId = bill.Id,
                    ProductId = bd.ProductId,
                    Quantity = bd.Quantity,
                    UnitPrice = products[bd.ProductId].Price
                }).ToList();

                _context.BillDetails.AddRange(billDetails);
                await _context.SaveChangesAsync();

                await transaction.CommitAsync();

                return await GetByIdAsync(bill.Id) ?? throw new InvalidOperationException("Failed to retrieve created bill");
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var bill = await _context.Bills.FindAsync(id);
            if (bill == null) return false;

            _context.Bills.Remove(bill);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Bills.AnyAsync(b => b.Id == id);
        }

        public async Task<int> GetNextBillCodeAsync()
        {
            var maxCode = await _context.Bills.MaxAsync(b => (int?)b.Code) ?? 0;
            return maxCode + 1;
        }
    }
} 