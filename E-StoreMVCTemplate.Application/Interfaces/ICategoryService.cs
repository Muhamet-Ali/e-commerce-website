using E_StoreMVCTemplate.Application.DTOs.Category;
using E_StoreMVCTemplate.Application.DTOs.Discount;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_StoreMVCTemplate.Application.Interfaces
{
    public interface ICategoryService : IGenericService
        <GetCategoryListDto, GetCategoryByIdWithProductDto, CreateCategoryDto, UpdateCategoryDto>
    {
        Task<List<GetCategoryWithSub>> GetCategAndSub();
    }
}
