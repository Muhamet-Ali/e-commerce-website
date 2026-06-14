using E_StoreMVCTemplate.Application.DTOs.Cart;
using E_StoreMVCTemplate.Application.Interfaces;
using E_StoreMVCTemplate.Infrastructure.Data;
using E_StoreMVCTemplate.Web.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace E_StoreMVCTemplate.Web.Controllers
{
    [Authorize]
    public class CartController : Controller
    {
        private readonly ICartService _cartService;
        private readonly AppDbContext _context;

        public CartController(ICartService cartService, AppDbContext context)
        {
            _cartService = cartService;
            _context = context;
        }

        public async Task<IActionResult> ViewCart()
        {
            var userId=User.FindFirstValue(ClaimTypes.NameIdentifier);
            var cart= await _cartService.GetByUserIdAsync(userId);
            return View(cart);
        }
        public async Task<IActionResult> AddToCart(int productId, int quantity)
        {
            var UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var dto = new AddToCartDto
            { ProductId = productId, Quantity = quantity, UserId = UserId };

            await _cartService.AddAsync(dto);

            return RedirectToAction("Index", "Home");
        }
        public async Task<IActionResult> MinusQuantity(int ProductId,int cartId)
        {
            await _cartService.MinusQuantity(cartId, ProductId);
            return RedirectToAction("ViewCart");
        }
        public async Task<IActionResult> PlusQuantity(int ProductId,int cartId)
        {
            await _cartService.PlusQuantity(cartId, ProductId);
            return RedirectToAction("ViewCart");
        }

        public async Task<IActionResult> RemoveItem (int ProductId, int cartId)
        {
            await _cartService.RemoveItemFromCartAsync(cartId, ProductId);
            return RedirectToAction("ViewCart");
        }
        public async Task<IActionResult> ClearCart()
        {
            var UserId=User.FindFirstValue(ClaimTypes.NameIdentifier);
            await _cartService.ClearCartAsync(UserId);
            return RedirectToAction("Index", "Home");
        }

    }
}
