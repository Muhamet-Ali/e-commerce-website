using E_StoreMVCTemplate.Application.DTOs.SubCategory;
using E_StoreMVCTemplate.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace E_StoreMVCTemplate.Web.Areas.Admin.Controllers
{
    [Area("Admin")]

    public class SubCategoryController : Controller
    {
        private readonly ISubCategoryService _subCategoryService;
        private readonly ICategoryService _categoryService;

        public SubCategoryController(ISubCategoryService subCategoryService, ICategoryService categoryService)
        {
            _subCategoryService = subCategoryService;
            _categoryService = categoryService;
        }

        public async Task<IActionResult> Index()
        {
            var items = await _subCategoryService.GetAllAsync();
            return View(items);
        }


        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ViewBag.Categories = await _categoryService.GetAllAsync();
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateSubCategoryDto dto)
        {
            await _subCategoryService.AddAsync(dto);
            return RedirectToAction("Index");
        }


        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            var item = await _subCategoryService.GetByIdAsync(id);
            var dto = new UpdateSubCategoryDto
            {
                Id = item.Id,
                Name = item.Name,
                CategoryId = item.CategoryId
            };
            ViewBag.Categories = await _categoryService.GetAllAsync();
            return View(dto);
        }
        [HttpPost]
        public async Task<IActionResult> Update(UpdateSubCategoryDto dto)
        {
            await _subCategoryService.UpdateAsync(dto);
            return RedirectToAction("Index");
        }


        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            await _subCategoryService.DeleteAsync(id);
            return RedirectToAction("Index");
        }


        [HttpGet]
        public async Task<IActionResult> Detail(int id)
        {
            var item = await _subCategoryService.GetByIdAsync(id);
            return View(item);
        }


    }
}
