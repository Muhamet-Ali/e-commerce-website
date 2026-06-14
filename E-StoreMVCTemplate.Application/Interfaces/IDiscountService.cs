using E_StoreMVCTemplate.Application.DTOs.Discount;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_StoreMVCTemplate.Application.Interfaces
{
    public interface IDiscountService
    {
        Task<List<GetDiscountListDto>> GetAllAsync();
        Task<GetDiscountByIdDto?> GetByIdAsync(int id);
        Task AddAsync(CreateDiscountDto dto);
        Task UpdateAsync(UpdateDiscountDto dto);
        Task DeleteAsync(int id);

        Task<List<GetDiscountForUserDto>> GetActiveDiscountsForUserAsync();
    }
}
