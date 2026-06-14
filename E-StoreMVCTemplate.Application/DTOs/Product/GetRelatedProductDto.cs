using E_StoreMVCTemplate.Application.DTOs.Images;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_StoreMVCTemplate.Application.DTOs.Product
{
    public class GetRelatedProductDto
    {

        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public decimal Price { get; set; }
        public List<ProductImageDTO> Images { get; set; }

    }
}
