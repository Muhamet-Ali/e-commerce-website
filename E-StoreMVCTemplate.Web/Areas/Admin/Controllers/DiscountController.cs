using E_StoreMVCTemplate.Application.DTOs.Discount;
using E_StoreMVCTemplate.Application.Interfaces;
using E_StoreMVCTemplate.Web.DTOs;
using E_StoreMVCTemplate.Web.DTOs.Admin;
using Microsoft.AspNetCore.Mvc;
using E_StoreMVCTemplate.Application.DTOs;

namespace E_StoreMVCTemplate.Web.Areas.Admin.Controllers
{
    [Area("Admin")]

    public class DiscountController : Controller
    {
        private readonly IDiscountService _discountService;
       private readonly IProductService _productService;

        public DiscountController(IDiscountService discountService, IProductService productService)
        {
            _discountService = discountService;
            _productService = productService;
        }

        public async Task<IActionResult> Index()
        {
            var items=await _discountService.GetAllAsync();

            return View(items);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ViewBag.Products = await _productService.GetAllAsync();

            return View(new CreateDiscountDto
            {
                StartDate = DateTime.Today,
                EndDate = DateTime.Today.AddDays(7),
                IsActive = true
            });
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateDiscountDto dto)
        {
            await _discountService.AddAsync(dto);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int Id)
        {
            await _discountService.DeleteAsync(Id);
            return RedirectToAction("Index");
        }




    }
}
