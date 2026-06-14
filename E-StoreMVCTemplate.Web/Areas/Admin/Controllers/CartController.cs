using E_StoreMVCTemplate.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace E_StoreMVCTemplate.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CartController : Controller
    {
        private readonly ICartService _cartService;

        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }

        public async Task<IActionResult> Index()
        {
            var items = await _cartService.GetAllAsync();
            return View(items);
        }

        [HttpGet]
        public async Task<IActionResult> Detail(int id)
        {
            var cart = await _cartService.GetByCartIdAsync(id);
            if (cart == null)
            {
                TempData["ErrorMessage"] = "Cart not found.";
                return RedirectToAction(nameof(Index));
            }

            return View(cart);
        }
    }
}
