using E_StoreMVCTemplate.Application.DTOs.Order;
using E_StoreMVCTemplate.Application.Interfaces;
using E_StoreMVCTemplate.Domain.Entities;
using E_StoreMVCTemplate.Domain.Enums;
using E_StoreMVCTemplate.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_StoreMVCTemplate.Infrastructure.Services
{
    public class OrderService : IOrderService
    {
        private readonly AppDbContext _context;

        public OrderService(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(string userId, string address)
        {
            var cart = await _context.Carts
                .Include(c => c.CartItems)
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (cart == null || !cart.CartItems.Any()) return;

            var order = new Order
            {
                Addres = address,
                UserId = userId,
                CreatedAt = DateTime.UtcNow,
                Status = OrderStatus.Pending,
                OrderItems = new List<OrderItem>()
            };

            decimal total = 0;

            foreach (var item in cart.CartItems)
            {
                var product = await _context.Products
                    .FirstOrDefaultAsync(p => p.Id == item.ProductId);

                var orderItem = new OrderItem
                {
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    UnitPrice = product.DiscountPrice??product.Price
                };

                order.OrderItems.Add(orderItem);

                total += (product.DiscountPrice ?? product.Price) * item.Quantity;
            }

            order.TotalAmount = total;

            await _context.Orders.AddAsync(order);

            // 🔥 SEPETİ TEMİZLE
            _context.CartItems.RemoveRange(cart.CartItems);
            _context.Carts.Remove(cart);

            await _context.SaveChangesAsync();
        }
        public async Task DeleteAsync(int id)
        {
            var Order = await _context.Orders.Include(o => o.OrderItems).FirstOrDefaultAsync();
            _context.Orders.Remove(Order);
            await _context.SaveChangesAsync();
        }

        public async Task<List<OrderListDto>> GetAllAsync()
        {
            return await _context.Orders.Include(o => o.OrderItems).Include(o => o.User)
                .Select(o => new OrderListDto
                {
                    Id = o.Id,
                    TotalAmount = o.TotalAmount,
                    Status = o.Status,
                    CreatedAt = o.CreatedAt,
                    UserId = o.UserId,
                    UserName = o.User.FullName,
                }).ToListAsync();
        }

        public async Task<OrderDetailDto> GetByIdAsync(int id)
        {
            return await _context.Orders
                .Include(o => o.User)
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Product)
                .Where(o => o.Id == id)
                .Select(o => new OrderDetailDto
                {
                    Id = o.Id,
                    TotalAmount = o.TotalAmount,
                    Status = o.Status,
                    Address = o.Addres,
                    UserId = o.UserId,
                    UserName = o.User.UserName,
                    UserEmail = o.User.Email,
                    UserPhoneNumber = o.User.PhoneNumber,
                    CreatedAt = o.CreatedAt,
                    Items = o.OrderItems.Select(oi => new OrderItemDetailDto
                    {
                        Id = oi.Id,
                        ProductId = oi.ProductId,
                        ProductName = oi.Product.Name,
                        Quantity = oi.Quantity,
                        UnitPrice = oi.UnitPrice,
                    }).ToList()
                })
                .FirstOrDefaultAsync();
        }

        public async Task<List<GetUserOrdersDto>> GetUserAllAsync(string UserId)
        {
            return await _context.Orders.Where(o => o.UserId == UserId)
                .Select(o => new GetUserOrdersDto
                {
                    Id = o.Id,
                    UserId = o.UserId,
                    TotalAmount = o.TotalAmount,
                    Status = o.Status,
                    CreatedAt = o.CreatedAt,
                }).ToListAsync();


        }

        public async Task ConfirmAsync(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order == null) return;

            order.Status = OrderStatus.Confirmed;
            await _context.SaveChangesAsync();
        }

        public async Task<int> GetPendingCountAsync()
        {
            return await _context.Orders.CountAsync(o => o.Status == OrderStatus.Pending);
        }
    }
}
