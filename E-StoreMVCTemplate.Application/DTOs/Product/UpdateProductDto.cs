using E_StoreMVCTemplate.Application.DTOs.Images;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_StoreMVCTemplate.Application.DTOs.Product
{
    public class UpdateProductDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; }
        public decimal Price { get; set; }

        public int Stock { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public int CategoryId { get; set; }
        public int? SubCategoryId { get; set; }

        public List<ProductImageDTO> ExistingImages { get; set; } = new List<ProductImageDTO>();
        public List<int> RemovedImageIds { get; set; } = new List<int>();
        public List<UpdateImageProductDto> Images { get; set; } = new List<UpdateImageProductDto>();
    }
}
