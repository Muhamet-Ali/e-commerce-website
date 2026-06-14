using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_StoreMVCTemplate.Application.DTOs.Cart
{
    public class CartListDto
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string? UserName { get; set; }
        public string? UserEmail { get; set; }
        public string? UserPhoneNumber { get; set; }
        public List<CartItemDto> Items { get; set; } = new();
        public decimal GrandTotal => Items.Sum(x => x.TotalPrice);
    }
}
