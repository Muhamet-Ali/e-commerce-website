using E_StoreMVCTemplate.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_StoreMVCTemplate.Application.DTOs.Order
{
    public class GetUserOrdersDto
    {

        public int  Id { get; set; }
        public string UserId { get; set; } = null!;
        public decimal TotalAmount { get; set; }
        public OrderStatus Status { get; set; } = OrderStatus.Pending;
        public DateTime CreatedAt { get; set; }
    }
}
