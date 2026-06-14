using E_StoreMVCTemplate.Application.DTOs.HeaderImage;
using E_StoreMVCTemplate.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace E_StoreMVCTemplate.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class HeaderImagesController : Controller
    {
        private readonly IHeadeImagesService _headeImagesService;

        public HeaderImagesController(IHeadeImagesService headeImagesService)
        {
            _headeImagesService = headeImagesService;
        }

        public async Task<IActionResult> Index()
        {
            var images = await _headeImagesService.GetAllAsync();
            return View(images);
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateHeadeImagesDto dto)
        {
            if (dto.File == null || dto.File.Length == 0)
                return RedirectToAction("Index");

            await _headeImagesService.AddAsync(dto);
            return RedirectToAction("Index");
        }


        public async Task<IActionResult> Delete(int id)
        {
            await _headeImagesService.DeleteAsync(id);
            return RedirectToAction("Index");
        }


    }
}
