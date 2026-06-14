using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_StoreMVCTemplate.Application.DTOs.Cart
{
    public class CartByIdDto
    {
            public int CartItemId { get; set; }
            public string ProductName { get; set; }
            public string ImageUrl { get; set; }
            public int Quantity { get; set; }
            public decimal UnitPrice { get; set; }
            public int cartId { get; set; }
            public int ProductId { get; set; }

        public decimal SubTotal => UnitPrice * Quantity;
        
    }
}
