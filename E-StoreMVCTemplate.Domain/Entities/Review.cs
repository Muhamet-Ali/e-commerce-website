using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_StoreMVCTemplate.Domain.Entities
{
    public class Review
    {

        public int Id { get; set; }
        public int Rating { get; set; }  // 1-5
        public string? Comment { get; set; }
        public bool IsApproved { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public int ProductId { get; set; }
        public Product Product { get; set; } = null!;

        public string UserId { get; set; } = null!;
        public AppUser User { get; set; } = null;


    }
}
