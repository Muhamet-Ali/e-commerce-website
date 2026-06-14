using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_StoreMVCTemplate.Application.DTOs.Order
{
    public class CreateOrderDto
    {
        public string Address { get; set; }
        public string UserId { get; set; }
        public List<CreateOrderItemDto> Items { get; set; }
    }
}
