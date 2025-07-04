using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NationalTask.DTOs;
using NationalTask.Models;
using NationalTask.Reports;
using NationalTask.Reports.Sub;
using NationalTask.Repository;
using System.Security.Claims;

namespace NationalTask.Controllers
{
    [Authorize]
    public class CustomerController : Controller
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IProductRepository _productRepository;
        private readonly IBillRepository _billRepository;
        private readonly UserManager<IdentityUser> _userManager;

        public CustomerController(
            ICustomerRepository customerRepository,
            IProductRepository productRepository,
            IBillRepository billRepository,
            UserManager<IdentityUser> userManager)
        {
            _customerRepository = customerRepository;
            _productRepository = productRepository;
            _billRepository = billRepository;
            _userManager = userManager;
        }

        public async Task<IActionResult> Profile()
        {
            var customerId = await GetCurrentCustomerId();
            if (customerId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var profile = await _customerRepository.GetProfileByIdAsync(customerId.Value);
            if (profile == null)
            {
                return NotFound();
            }

            return View(profile);
        }

        public async Task<IActionResult> CreateBill()
        {
            var customerId = await GetCurrentCustomerId();
            if (customerId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var products = await _productRepository.GetAllAsync();
            ViewBag.Products = products;
            ViewBag.CustomerId = customerId.Value;

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateBill(CreateBillViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var createBillDto = new CreateBillDto
                    {
                        CustomerId = model.CustomerId,
                        BillDetails = model.BillDetails.Select(bd => new CreateBillDetailDto
                        {
                            ProductId = bd.ProductId,
                            Quantity = bd.Quantity
                        }).ToList()
                    };

                    var bill = await _billRepository.CreateAsync(createBillDto);
                    return RedirectToAction("BillDetails", new { id = bill.Id });
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Error creating bill: " + ex.Message);
                }
            }

            var products = await _productRepository.GetAllAsync();
            ViewBag.Products = products;
            return View(model);
        }

        public async Task<IActionResult> MyBills()
        {
            var customerId = await GetCurrentCustomerId();
            if (customerId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var bills = await _billRepository.GetByCustomerIdAsync(customerId.Value);
            return View(bills);
        }

        public async Task<IActionResult> BillDetails(int id)
        {
            var bill = await _billRepository.GetByIdAsync(id);
            if (bill == null)
            {
                return NotFound();
            }

            if (User.IsInRole("Admin"))
            {
                return View(bill);
            }

            var customerId = await GetCurrentCustomerId();
            if (customerId != bill.CustomerId)
            {
                return Forbid();
            }

            return View(bill);
        }

        [HttpGet]
        public async Task<IActionResult> PrintBill(int id)
        {
            CustomerBillReport CB = new CustomerBillReport();
            CB.Parameters["Bfilter"].Value = id;
            return View(CB);
        }

        private async Task<int?> GetCurrentCustomerId()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return null;

            var claims = await _userManager.GetClaimsAsync(user);
            var customerIdClaim = claims.FirstOrDefault(c => c.Type == "CustomerId");
            
            if (customerIdClaim != null && int.TryParse(customerIdClaim.Value, out int customerId))
            {
                return customerId;
            }
            return null;
        }
    }

    public class CreateBillViewModel
    {
        public int CustomerId { get; set; }
        public List<BillDetailViewModel> BillDetails { get; set; } = new List<BillDetailViewModel>();
    }

    public class BillDetailViewModel
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; } = 1;
    }
} 