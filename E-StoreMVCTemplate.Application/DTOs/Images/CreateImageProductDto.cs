using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_StoreMVCTemplate.Application.DTOs.Images
{
    public class CreateImageProductDto
    {
        public IFormFile File { get; set; } = null!;
        public int DisplayOrder { get; set; }
    }
}
