using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_StoreMVCTemplate.Application.DTOs.Favorite
{
    public class GetUserFavoriteListDto
    {
        public int FavoriteId { get; set; } // Silmek isterse Id lazım olur
        public int ProductId { get; set; }
        public string ProductName { get; set; } // UI'da göstermek için
        public string? ProductImageUrl { get; set; } // Görsel önemli
        public decimal ProductPrice { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
