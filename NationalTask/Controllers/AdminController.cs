using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NationalTask.Repository;

namespace NationalTask.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly IBillRepository _billRepository;

        public AdminController(IBillRepository billRepository)
        {
            _billRepository = billRepository;
        }

        public async Task<IActionResult> AllBills()
        {
            var bills = await _billRepository.GetAllAsync();
            return View(bills);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteBill(int id)
        {
            await _billRepository.DeleteAsync(id);
            return RedirectToAction("AllBills");
        }
    }
} 