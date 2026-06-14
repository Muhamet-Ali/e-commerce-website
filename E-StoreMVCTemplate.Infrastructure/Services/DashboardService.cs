using E_StoreMVCTemplate.Application.DTOs.Dashboard;
using E_StoreMVCTemplate.Application.Interfaces;
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
    public class DashboardService : IDashboardService
    {
        private readonly AppDbContext _context;

        public DashboardService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<AdminDashboardDto> GetAdminDashboardAsync()
        {
            return new AdminDashboardDto
            {
                ProductCount = await _context.Products.CountAsync(),
                ActiveProductCount = await _context.Products.CountAsync(x => x.IsActive),
                OrderCount = await _context.Orders.CountAsync(),
                PendingOrderCount = await _context.Orders.CountAsync(x => x.Status == OrderStatus.Pending),
                ConfirmedOrderCount = await _context.Orders.CountAsync(x => x.Status == OrderStatus.Confirmed),
                UserCount = await _context.Users.CountAsync(),
                ReviewCount = await _context.Reviews.CountAsync(),
                TotalRevenue = await _context.Orders
                    .Where(x => x.Status == OrderStatus.Confirmed)
                    .SumAsync(x => (decimal?)x.TotalAmount) ?? 0,
                RecentOrders = await _context.Orders
                    .Include(x => x.User)
                    .OrderByDescending(x => x.CreatedAt)
                    .Take(5)
                    .Select(x => new DashboardOrderDto
                    {
                        Id = x.Id,
                        UserName = x.User.FullName ?? x.User.UserName,
                        TotalAmount = x.TotalAmount,
                        Status = x.Status.ToString(),
                        CreatedAt = x.CreatedAt
                    })
                    .ToListAsync(),
                LowStockProducts = await _context.Products
                    .Where(x => x.Stock <= 10)
                    .OrderBy(x => x.Stock)
                    .Take(5)
                    .Select(x => new DashboardProductDto
                    {
                        Id = x.Id,
                        Name = x.Name,
                        Stock = x.Stock,
                        Price = x.Price
                    })
                    .ToListAsync()
            };
        }
    }
}
