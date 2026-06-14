using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_StoreMVCTemplate.Application.DTOs.Discount
{
    public class UpdateDiscountDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public decimal Rate { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsActive { get; set; }
        public IFormFile? File { get; set; }


        public List<int>? ProductIds { get; set; }
    }
}
