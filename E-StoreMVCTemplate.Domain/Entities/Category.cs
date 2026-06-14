using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_StoreMVCTemplate.Domain.Entities
{
    public class Category
    {
        public int Id { get; set; }
        public  string Name { get; set; }
        public int DisplayOrder { get; set; }
        public ICollection<SubCategory> SubCategories { get; set; } = new List<SubCategory>();
        public ICollection<Product> Products { get; set; } = new List<Product>();

    }
}
