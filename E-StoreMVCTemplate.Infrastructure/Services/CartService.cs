using E_StoreMVCTemplate.Application.DTOs.Cart;
using E_StoreMVCTemplate.Application.Interfaces;
using E_StoreMVCTemplate.Domain.Entities;
using E_StoreMVCTemplate.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace E_StoreMVCTemplate.Infrastructure.Services
{
    public class CartService : ICartService
    {
        private readonly AppDbContext _context;

        public CartService(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(AddToCartDto dto)
        {
            var cart = await _context.Carts
                .Include(c => c.CartItems)
                .FirstOrDefaultAsync(c => c.UserId == dto.UserId);

            if (cart == null)
            {
                cart = new Cart { UserId = dto.UserId };
                await _context.Carts.AddAsync(cart);
                await _context.SaveChangesAsync();
            }

            var item = await _context.CartItems
                .FirstOrDefaultAsync(ci => ci.ProductId == dto.ProductId && ci.CartId == cart.Id);

            if (item != null)
            {
                item.Quantity += dto.Quantity;
                await _context.SaveChangesAsync();
            }
            else
            {
                var product = await _context.Products
                    .FirstOrDefaultAsync(p => p.Id == dto.ProductId);

                if (product == null) return;

                var cartItem = new CartItem
                {
                    CartId = cart.Id,
                    ProductId = dto.ProductId,
                    Quantity = dto.Quantity,
                    UnitPrice = product.DiscountPrice ?? product.Price,
                };

                await _context.CartItems.AddAsync(cartItem);
                await _context.SaveChangesAsync();
            }
        }

        public async Task ClearCartAsync(string userId)
        {
            var cart = await _context.Carts
                .Include(c => c.CartItems)
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (cart == null) return;

            _context.CartItems.RemoveRange(cart.CartItems);
            _context.Carts.Remove(cart);
            await _context.SaveChangesAsync();
        }

        public async Task<List<CartListDto>> GetAllAsync()
        {
            return await _context.Carts
                .Include(c => c.User)
                .Include(c => c.CartItems)
                    .ThenInclude(ci => ci.Product)
                .Select(c => new CartListDto
                {
                    Id = c.Id,
                    UserId = c.UserId,
                    UserName = c.User.UserName,
                    UserEmail = c.User.Email,
                    UserPhoneNumber = c.User.PhoneNumber,
                    Items = c.CartItems.Select(ci => new CartItemDto
                    {
                        Id = ci.Id,
                        ProductId = ci.ProductId,
                        ProductName = ci.Product.Name,
                        Quantity = ci.Quantity,
                        UnitPrice = ci.UnitPrice,
                    }).ToList()
                }).ToListAsync();
        }

        public async Task<CartListDto?> GetByCartIdAsync(int cartId)
        {
            return await _context.Carts
                .Include(c => c.User)
                .Include(c => c.CartItems)
                    .ThenInclude(ci => ci.Product)
                .Where(c => c.Id == cartId)
                .Select(c => new CartListDto
                {
                    Id = c.Id,
                    UserId = c.UserId,
                    UserName = c.User.UserName,
                    UserEmail = c.User.Email,
                    UserPhoneNumber = c.User.PhoneNumber,
                    Items = c.CartItems.Select(ci => new CartItemDto
                    {
                        Id = ci.Id,
                        ProductId = ci.ProductId,
                        ProductName = ci.Product.Name,
                        Quantity = ci.Quantity,
                        UnitPrice = ci.UnitPrice,
                    }).ToList()
                })
                .FirstOrDefaultAsync();
        }

        public async Task<List<CartByIdDto>> GetByUserIdAsync(string userId)
        {
            var cart = await _context.Carts
                .Include(c => c.CartItems)
                    .ThenInclude(ci => ci.Product)
                        .ThenInclude(p => p.Images)
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (cart == null) return new List<CartByIdDto>();

            return cart.CartItems.Select(ci => new CartByIdDto
            {
                cartId = ci.Cart.Id,
                CartItemId = ci.Id,
                ProductId = ci.Product.Id,
                ProductName = ci.Product.Name,
                ImageUrl = ci.Product.Images
                    .OrderBy(i => i.DisplayOrder)
                    .FirstOrDefault()?.ImageUrl ?? string.Empty,
                Quantity = ci.Quantity,
                UnitPrice = ci.UnitPrice,
            }).ToList();
        }

        public async Task RemoveItemFromCartAsync(int cartId, int productId )
        {
            var item = await _context.CartItems
                .FirstOrDefaultAsync(c => c.Cart.Id == cartId && c.ProductId == productId);

            if (item == null) return;
            _context.CartItems.Remove(item);

            await _context.SaveChangesAsync();
        }

        public async Task MinusQuantity(int cartId, int productId)
        {
            var element = await _context.CartItems
                 .Where(c => c.Cart.Id == cartId && c.ProductId == productId).FirstOrDefaultAsync();

            element.Quantity--;
            if (element.Quantity <= 0)
                _context.CartItems.Remove(element);

            await _context.SaveChangesAsync();
        }

        public async Task PlusQuantity(int cartId, int productId)
        {
            var element = await _context.CartItems
                .Where(c => c.Cart.Id == cartId && c.ProductId == productId).FirstOrDefaultAsync();

            element.Quantity++;
            await _context.SaveChangesAsync();
        }
    }
}