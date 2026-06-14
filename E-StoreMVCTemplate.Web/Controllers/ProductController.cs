using E_StoreMVCTemplate.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace E_StoreMVCTemplate.Web.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        public async Task<IActionResult> Detail(int Id)
        {
            var product=await _productService.GetByIdAsync(Id);
            return View(product);
        }

        public IActionResult Index()
        {
            return View();
        }




    }
}
