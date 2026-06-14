using E_StoreMVCTemplate.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace E_StoreMVCTemplate.Web.Controllers
{
    public class SubCategoryController : Controller
    {
        private readonly ISubCategoryService _subCategoryService;

        public SubCategoryController(ISubCategoryService subCategoryService)
        {
            _subCategoryService = subCategoryService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> GetSubCategoryWithProduct(int id)
        {
            var subCategroy= await _subCategoryService.GetByIdAsync(id);
            return View(subCategroy);
        }


    }
}
