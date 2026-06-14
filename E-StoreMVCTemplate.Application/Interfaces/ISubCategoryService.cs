using E_StoreMVCTemplate.Application.DTOs.Category;
using E_StoreMVCTemplate.Application.DTOs.SubCategory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_StoreMVCTemplate.Application.Interfaces
{
    public interface ISubCategoryService:IGenericService
        <GetSubCateogryListDto, GetSubCateogryByIdWithProductListDto, CreateSubCategoryDto, UpdateSubCategoryDto>
    {




    }
}
