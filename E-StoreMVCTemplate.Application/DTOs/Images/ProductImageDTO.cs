using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_StoreMVCTemplate.Application.DTOs.Images
{
    public class ProductImageDTO
    {
        public int Id { get; set; }
        public string ImageUrl { get; set; }
        public int DisplayOrder { get; set; }

    }
}
