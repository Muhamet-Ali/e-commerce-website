using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_StoreMVCTemplate.Domain.Entities
{
    public class HeaderImages
    {
        public int Id { get; set; }
        public int DisplayOrder { get; set; }
        public string ImageUrl { get; set; }
        public string? Title { get; set; }
        public bool IsActive { get; set; } = true;

    }
}
