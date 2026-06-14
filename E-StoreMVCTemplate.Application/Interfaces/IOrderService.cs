using E_StoreMVCTemplate.Application.DTOs.Order;
using E_StoreMVCTemplate.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_StoreMVCTemplate.Application.Interfaces
{
    public interface IOrderService
       
    {
        Task<List<OrderListDto>> GetAllAsync();
        Task<List<GetUserOrdersDto>> GetUserAllAsync(string UserId);
        Task<OrderDetailDto> GetByIdAsync(int id);

        Task AddAsync(string userId, string address);
        Task DeleteAsync(int id);
        Task ConfirmAsync(int id);
        Task<int> GetPendingCountAsync();

    }
}
