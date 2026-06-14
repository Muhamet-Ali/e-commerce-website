using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_StoreMVCTemplate.Application.DTOs.HeaderImage
{
    public class CreateHeadeImagesDto
    {
        public int DisplayOrder { get; set; }
        public string? Title { get; set; }
        public IFormFile File { get; set; } = null!;

    }
}
