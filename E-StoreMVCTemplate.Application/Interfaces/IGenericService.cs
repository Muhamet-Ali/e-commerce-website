using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using E_StoreMVCTemplate.Domain.Entities;

namespace E_StoreMVCTemplate.Application.Interfaces
{
    public interface IGenericService<TListDto, TDetailDto, TCreateDto, TUpdateDto>
    {
        Task<List<TListDto>> GetAllAsync();
        Task<TDetailDto> GetByIdAsync(int id);
        Task AddAsync(TCreateDto dto);
        Task UpdateAsync(TUpdateDto dto);
        Task DeleteAsync(int id);
    }
}
