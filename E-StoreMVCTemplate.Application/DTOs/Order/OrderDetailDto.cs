using E_StoreMVCTemplate.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_StoreMVCTemplate.Application.DTOs.Order
{
    public class OrderDetailDto
    {
        public int Id { get; set; }
        public decimal TotalAmount { get; set; }
        public OrderStatus Status { get; set; }
        public string Address { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string? UserEmail { get; set; }
        public string? UserPhoneNumber { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<OrderItemDetailDto> Items { get; set; } = new();
    }
}
