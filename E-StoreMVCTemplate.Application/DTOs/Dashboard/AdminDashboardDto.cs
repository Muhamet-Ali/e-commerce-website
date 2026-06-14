using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_StoreMVCTemplate.Application.DTOs.Dashboard
{
    public class AdminDashboardDto
    {
        public int ProductCount { get; set; }
        public int ActiveProductCount { get; set; }
        public int OrderCount { get; set; }
        public int PendingOrderCount { get; set; }
        public int ConfirmedOrderCount { get; set; }
        public int UserCount { get; set; }
        public int ReviewCount { get; set; }
        public decimal TotalRevenue { get; set; }
        public List<DashboardOrderDto> RecentOrders { get; set; } = new List<DashboardOrderDto>();
        public List<DashboardProductDto> LowStockProducts { get; set; } = new List<DashboardProductDto>();
    }

    public class DashboardOrderDto
    {
        public int Id { get; set; }
        public string? UserName { get; set; }
        public decimal TotalAmount { get; set; }
        public string Status { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
    }

    public class DashboardProductDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public int Stock { get; set; }
        public decimal Price { get; set; }
    }
}
