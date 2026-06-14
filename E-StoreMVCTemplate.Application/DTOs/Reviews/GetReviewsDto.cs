using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_StoreMVCTemplate.Application.DTOs.Reviews
{
    public class GetReviewsDto
    {
        public int Id { get; set; }
        public int Rating { get; set; }  // 1-5
        public string? Comment { get; set; }
        public bool IsApproved { get; set; } = false;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public int ProductId { get; set; }
        public string ProductName { get; set; } = null!;

        public string? UserId { get; set; }
        public string? UserName { get; set; }

    }
}
