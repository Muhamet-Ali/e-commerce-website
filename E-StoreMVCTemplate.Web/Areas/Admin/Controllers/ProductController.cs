using E_StoreMVCTemplate.Application.DTOs.Images;
using E_StoreMVCTemplate.Application.DTOs.Product;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using E_StoreMVCTemplate.Application.Interfaces;
using E_StoreMVCTemplate.Infrastructure.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace E_StoreMVCTemplate.Web.Areas.Admin.Controllers
{
    [Area("Admin")]

    public class ProductController : Controller
    {
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;
        private readonly ISubCategoryService _subCategoryService;
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public ProductController(
            IProductService productService,
            ICategoryService categoryService,
            ISubCategoryService subCategoryService,
            AppDbContext context,
            IWebHostEnvironment env)
        {
            _productService = productService;
            _categoryService = categoryService;
            _subCategoryService = subCategoryService;
            _context = context;
            _env = env;
        }

        public async Task<IActionResult> Index()
        {
            var items=await _productService.GetAllAsync();
            return View(items);
        }


        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ViewBag.Categories = await _categoryService.GetCategAndSub();
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateProductDto dto)
        {
            await _productService.AddAsync(dto);
            TempData["SuccessMessage"] = "Product has been created.";
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            var product = await _productService.GetByIdAsync(id);
            if (product == null)
            {
                TempData["ErrorMessage"] = "Product not found.";
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Categories = await _categoryService.GetCategAndSub();

            var dto = new UpdateProductDto
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                Stock = product.Stock,
                IsActive = product.IsActive,
                CategoryId = product.CategoryId,
                SubCategoryId = product.SubCategoryId,
                ExistingImages = product.Images ?? new List<ProductImageDTO>()
            };

            return View(dto);
        }

        [HttpPost]
        public async Task<IActionResult> Update(UpdateProductDto dto, List<IFormFile> Files)
        {
            if (Files != null && Files.Any())
            {
                dto.Images = Files
                    .Where(f => f != null && f.Length > 0)
                    .Select((f, idx) => new UpdateImageProductDto
                    {
                        File = f,
                        DisplayOrder = idx
                    })
                    .ToList();
            }
            else
            {
                dto.Images = new List<UpdateImageProductDto>();
            }

            await _productService.UpdateAsync(dto);
            TempData["SuccessMessage"] = "Product has been updated.";
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            await _productService.DeleteAsync(id);
            TempData["SuccessMessage"] = "Product has been deleted.";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> DeleteImage(int id, int productId)
        {
            var image = await _context.ProductImages.FirstOrDefaultAsync(i => i.Id == id);
            if (image == null)
                throw new Exception("Image not found.");

            var path = Path.Combine(_env.WebRootPath, "images", "products", image.ImageUrl);
            if (System.IO.File.Exists(path))
                System.IO.File.Delete(path);

            _context.ProductImages.Remove(image);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Image has been deleted.";
            return RedirectToAction(nameof(Update), new { id = productId });
        }
    }
}
