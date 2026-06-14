using E_StoreMVCTemplate.Application.DTOs.Order;
using E_StoreMVCTemplate.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Security.Claims;
using System.Security.Claims;
namespace E_StoreMVCTemplate.Web.Controllers
{
    public class OrderController : Controller
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpPost]
        public async Task<IActionResult> AddOrder(string Address)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            await _orderService.AddAsync(userId, Address);
            TempData["SuccessMessage"] = "Your order has been placed successfully.";
            return RedirectToAction("Index","Home");
        }

        public async Task<IActionResult> GetOrders()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var orders=await _orderService.GetUserAllAsync(userId);
            return View(orders);
        }

    }
}
