using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_StoreMVCTemplate.Domain.Entities
{
    public class ProductImages
    {
        public int Id { get; set; }
        public string ImageUrl { get; set; }
        public int DisplayOrder { get; set; }
        public int ProductId { get; set; }

        public Product Products { get; set; }

    }
}
