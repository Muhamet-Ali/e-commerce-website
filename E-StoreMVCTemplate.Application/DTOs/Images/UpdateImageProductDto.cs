using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_StoreMVCTemplate.Application.DTOs.Images
{
    public class UpdateImageProductDto
    {
        public int? Id { get; set; } // varsa mevcut image
        public IFormFile? File { get; set; } // yeni foto gelirse
        public int DisplayOrder { get; set; }
    }
}
