using E_StoreMVCTemplate.Application.DTOs.Reviews;
using E_StoreMVCTemplate.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace E_StoreMVCTemplate.Web.Controllers
{
    [Authorize]
    public class ReviewController : Controller
    {
        private readonly IReviewService _reviewService;

        public ReviewController(IReviewService reviewService)
        {
            _reviewService = reviewService;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateReviewsDto dto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            dto.UserId = userId;

            await _reviewService.AddAsync(dto);

            return RedirectToAction("Detail", "Product", new { id = dto.ProductId });
        }
    }
}
