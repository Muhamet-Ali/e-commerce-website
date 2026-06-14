using E_StoreMVCTemplate.Application.DTOs.Reviews;
using E_StoreMVCTemplate.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace E_StoreMVCTemplate.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ReviewController : Controller
    {
        private readonly IReviewService _reviewService;

        public ReviewController(IReviewService reviewService)
        {
            _reviewService = reviewService;
        }

        public async Task<IActionResult> Index()
        {
            var items = await _reviewService.GetAllAsync();
            return View(items);
        }

        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            var review = await _reviewService.GetByIdAsync(id);
            if (review == null)
            {
                TempData["ErrorMessage"] = "Review not found.";
                return RedirectToAction(nameof(Index));
            }

            return View(review);
        }

        [HttpPost]
        public async Task<IActionResult> Update(UpdateReviewDto dto)
        {
            await _reviewService.UpdateAsync(dto);
            TempData["SuccessMessage"] = "Review has been updated.";
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            await _reviewService.DeleteAsync(id);
            TempData["SuccessMessage"] = "Review has been deleted.";
            return RedirectToAction(nameof(Index));
        }
    }
}
