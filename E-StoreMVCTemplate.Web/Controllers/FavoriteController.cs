using E_StoreMVCTemplate.Application.DTOs.Favorite;
using E_StoreMVCTemplate.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace E_StoreMVCTemplate.Web.Controllers
{
    [Authorize]
    public class FavoriteController : Controller
    {
        private readonly IFavoriteService _favoriteService;
        public FavoriteController(IFavoriteService favoriteService)
        {
            _favoriteService = favoriteService;
        }

        public async Task<IActionResult> AddFavorite(int ProductId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            await _favoriteService.AddAsync(new CreateFavoriteDto
            {
                ProductId = ProductId,
                UserId = userId
            });

            return RedirectToAction("Detail", "Product", new { id = ProductId });
        }

        public async Task<IActionResult> RemoveFavorite(int ProductId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            await _favoriteService.DeleteAsync(ProductId,userId);
            return RedirectToAction("Detail", "Product", new { id = ProductId });
        }

        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var favorites=await _favoriteService.GetFavoriteUserAsync(userId);

            return View(favorites);
        }
    }
}
