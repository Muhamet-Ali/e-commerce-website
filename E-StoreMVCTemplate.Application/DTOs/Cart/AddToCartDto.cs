using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_StoreMVCTemplate.Application.DTOs.Cart
{
    public class AddToCartDto
    {
        public string UserId { get; set; } = null!;

        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
