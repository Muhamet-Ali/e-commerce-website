using E_StoreMVCTemplate.Application.Interfaces;
using E_StoreMVCTemplate.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;


namespace E_StoreMVCTemplate.Web.ViewComponents
{
    public class CategoryMenuViewComponent : ViewComponent
    {
        private readonly ICategoryService _categoryService;
        private readonly ICartService _cartService;
        private readonly AppDbContext _context;

        public CategoryMenuViewComponent(ICategoryService categoryService, ICartService cartService, AppDbContext context)
        {
            _categoryService = categoryService;
            _cartService = cartService;
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var categories = await _categoryService.GetAllAsync();

            ViewBag.CartCount = 0;

            if (User.Identity != null && User.Identity.IsAuthenticated)
            {
                var userId = UserClaimsPrincipal.FindFirstValue(ClaimTypes.NameIdentifier);

                if (!string.IsNullOrEmpty(userId))
                {
                    ViewBag.CartCount = await _context.CartItems
                        .CountAsync(x => x.Cart.UserId == userId);

                }
            }

            return View(categories);
        }
    }
}
