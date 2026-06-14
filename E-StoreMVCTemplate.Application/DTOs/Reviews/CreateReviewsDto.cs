using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_StoreMVCTemplate.Application.DTOs.Reviews
{
    public class CreateReviewsDto
    {
        public int Rating { get; set; }  // 1-5
        public string? Comment { get; set; }
        public bool IsApproved { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public int ProductId { get; set; }

        public string UserId { get; set; } = null!;
    }
}
