using E_StoreMVCTemplate.Application.DTOs.Cart;
using E_StoreMVCTemplate.Domain.Entities;
using E_StoreMVCTemplate.Infrastructure.Data;
using E_StoreMVCTemplate.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;

namespace Tests
{
    public class CartServiceTest
    {
        private AppDbContext CreateContext()
        {
            var option = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

            return new AppDbContext(option);
        }

        [Fact]
        public async Task AddAsync_WhenCartDoesNotExist_ShouldCreateCartAndItem()
        {
            await using var context = CreateContext();

            var service = new CartService(context);

            var product = new Product
            {
                Id = 1,
                Name = "productDeneme",
                Description = "Desc.Deneme",
                Price = 100,
                DiscountPrice = null
            };

            context.Products.Add(product);
            await context.SaveChangesAsync();

            var dto = new AddToCartDto
            {
                UserId = "user-1",
                ProductId = 1,
                Quantity = 2
            };

            await service.AddAsync(dto);
        }

        [Fact]
        public async Task AddAsync_WhenProductAlreadyExists_ShouldIncreaseQuantity()
        {
            // Arrange
            await using var context = CreateContext();

            var product = new Product
            {
                Id = 1,
                Name = "productDeneme2",
                Description = "Desc.Deneme2",
                Price = 100
            };

            var cart = new Cart
            {
                Id = 1,
                UserId = "user-1"
            };

            var cartItem = new CartItem
            {
                CartId = 1,
                ProductId = 1,
                Quantity = 2,
                UnitPrice = 100
            };

            context.Products.Add(product);
            context.Carts.Add(cart);
            context.CartItems.Add(cartItem);
            await context.SaveChangesAsync();

            var service = new CartService(context);

            var dto = new AddToCartDto
            {
                UserId = "user-1",
                ProductId = 1,
                Quantity = 3
            };

            // Act
            await service.AddAsync(dto);

            // Assert
            var items = await context.CartItems.ToListAsync();
            var updatedItem = items.Single();

            Assert.Single(items);
            Assert.Equal(5, updatedItem.Quantity);
            Assert.Equal(100, updatedItem.UnitPrice);
        }

    }
}