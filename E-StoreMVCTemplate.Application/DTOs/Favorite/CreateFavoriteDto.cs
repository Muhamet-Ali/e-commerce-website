using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_StoreMVCTemplate.Application.DTOs.Favorite
{
    public class CreateFavoriteDto
    {
        public string UserId { get; set; } = null!;
        public int ProductId { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    }
}
