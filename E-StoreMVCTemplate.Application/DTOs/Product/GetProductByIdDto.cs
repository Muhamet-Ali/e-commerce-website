using E_StoreMVCTemplate.Application.DTOs.Images;
using E_StoreMVCTemplate.Application.DTOs.Reviews;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_StoreMVCTemplate.Application.DTOs.Product
{
    public class GetProductByIdDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; }
        public decimal Price { get; set; }
        public decimal? DiscountPrice { get; set; } // Soru işareti null olabilir demek
        public int Stock { get; set; }
        public bool IsActive { get; set; }

        public int? DiscountId { get; set; }
        public string ?DiscountName { get; set; }
        public bool IsFavorite { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }

        public int? SubCategoryId { get; set; }
        public string SubCategoryName { get; set; }

        public int FavoriteCount { get; set; }

        public List<GetProductReviews> Reviews { get; set; }
        public List<ProductImageDTO> Images { get; set; }
    }
}
