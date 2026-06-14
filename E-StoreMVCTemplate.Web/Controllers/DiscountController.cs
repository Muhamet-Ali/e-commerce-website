using E_StoreMVCTemplate.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace E_StoreMVCTemplate.Web.Controllers
{
    public class DiscountController : Controller
    {
        private readonly IDiscountService _discountService;

        public DiscountController(IDiscountService discountService)
        {
            _discountService = discountService;
        }

        public async Task<IActionResult> GetDiscountProducts(int id)
        {
            var prd=await _discountService.GetByIdAsync(id);
            return View(prd);
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
