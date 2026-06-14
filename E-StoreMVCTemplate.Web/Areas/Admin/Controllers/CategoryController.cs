using E_StoreMVCTemplate.Application.DTOs.Category;
using E_StoreMVCTemplate.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace E_StoreMVCTemplate.Web.Areas.Admin.Controllers
{
    [Area("Admin")]

    public class CategoryController : Controller
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        public async Task<IActionResult> Index()
        {
            var items = await _categoryService.GetAllAsync();
            return View(items);
        }


        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateCategoryDto dto)
        {
            await _categoryService.AddAsync(dto);
            return RedirectToAction("Index");
        }


        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            var item = await _categoryService.GetByIdAsync(id);
            var dto = new UpdateCategoryDto
            {
                Id = item.Id,
                Name = item.Name,
                DisplayOrder = item.DisplayOrder
            };
            return View(dto);
        }
        [HttpPost]
        public async Task<IActionResult> Update(UpdateCategoryDto dto)
        {
            await _categoryService.UpdateAsync(dto);
            return RedirectToAction("Index");
        }


        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            await _categoryService.DeleteAsync(id);
            return RedirectToAction("Index");
        }


        [HttpGet]
        public async Task<IActionResult> Detail(int id)
        {
            var item = await _categoryService.GetByIdAsync(id);
            return View(item);
        }


    }
}
