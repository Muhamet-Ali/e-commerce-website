using E_StoreMVCTemplate.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace E_StoreMVCTemplate.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IProductService _productService;

        public HomeController(IProductService productService)
        {
            _productService = productService;
        }

        public async Task<IActionResult> Index()
        {
            var Product=await _productService.GetPopulerProductListAsync();
            return View(Product);
        }
    }
}
