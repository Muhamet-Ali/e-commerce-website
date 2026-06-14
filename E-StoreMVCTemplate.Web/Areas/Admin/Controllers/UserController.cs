using E_StoreMVCTemplate.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace E_StoreMVCTemplate.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class UserController : Controller
    {
        private readonly IAuthService _authService;
        private readonly IOrderService _orderService;

        public UserController(IAuthService authService, IOrderService orderService)
        {
            _authService = authService;
            _orderService = orderService;
        }

        public async Task<IActionResult> Index()
        {
            var items = await _authService.GetAllAsync();
            return View(items);
        }

        [HttpGet]
        public async Task<IActionResult> Detail(string id)
        {
            var user = await _authService.GetByIdAsync(id);
            if (user == null)
            {
                TempData["ErrorMessage"] = "User not found.";
                return RedirectToAction(nameof(Index));
            }

            user.UserOrders = await _orderService.GetUserAllAsync(id);
            return View(user);
        }
    }
}
