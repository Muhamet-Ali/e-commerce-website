using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_StoreMVCTemplate.Domain.Entities
{
    public class Product
    {
        public int Id { get; set; } 
        public string Name { get; set; } = null!;
        public string Description { get; set; }
        public decimal Price { get; set; }
        public decimal? DiscountPrice { get; set; } // Soru işareti null olabilir demek
        public int Stock { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        public int? DiscountId { get; set; }
        public Discountcs? Discountcs { get; set; }

        public int CategoryId { get; set; }
        public Category Category { get; set; }

        public int? SubCategoryId { get; set; }
        public SubCategory SubCategory { get; set; }

        public ICollection<ProductImages> Images { get; set; } = new List<ProductImages>();
        public ICollection<Review> Reviews { get; set; } = new List<Review>();
        public ICollection<Favorite> Favorites { get; set; } = new List<Favorite>();

    }
}
