using E_StoreMVCTemplate.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_StoreMVCTemplate.Application.DTOs.Order
{
    public class UpdateOrderDto
    {
        public int Id { get; set; }
        public OrderStatus Status { get; set; }
    }
}
