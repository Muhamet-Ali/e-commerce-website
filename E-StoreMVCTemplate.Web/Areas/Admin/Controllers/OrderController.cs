using E_StoreMVCTemplate.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace E_StoreMVCTemplate.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class OrderController : Controller
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        public async Task<IActionResult> Index()
        {
            var items = await _orderService.GetAllAsync();
            return View(items);
        }

        [HttpGet]
        public async Task<IActionResult> Detail(int id)
        {
            var order = await _orderService.GetByIdAsync(id);
            if (order == null)
            {
                TempData["ErrorMessage"] = "Order not found.";
                return RedirectToAction(nameof(Index));
            }

            return View(order);
        }

        [HttpGet]
        public async Task<IActionResult> Confirm(int id)
        {
            await _orderService.ConfirmAsync(id);
            TempData["SuccessMessage"] = "Order has been confirmed.";
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            await _orderService.DeleteAsync(id);
            TempData["SuccessMessage"] = "Order has been deleted.";
            return RedirectToAction(nameof(Index));
        }
    }
}
