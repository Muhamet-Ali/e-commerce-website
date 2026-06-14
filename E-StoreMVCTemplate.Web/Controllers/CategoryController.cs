using E_StoreMVCTemplate.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace E_StoreMVCTemplate.Web.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> GetCategoryWithProduct(int id)
        {
            var categ=await _categoryService.GetByIdAsync(id);
            return View(categ);
        }





    }
}
