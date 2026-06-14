using E_StoreMVCTemplate.Application.DTOs.Cart;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_StoreMVCTemplate.Application.Interfaces
{
    public interface ICartService 
    {
        Task AddAsync(AddToCartDto dto);
        Task<List<CartListDto>> GetAllAsync();
        Task<CartListDto?> GetByCartIdAsync(int cartId);
        Task<List<CartByIdDto>> GetByUserIdAsync(string userId);
        Task RemoveItemFromCartAsync(int cartId, int productId);
        Task ClearCartAsync(string userId);

        Task PlusQuantity(int cartId, int productId);
        Task MinusQuantity(int cartId, int productId);

    }
}
