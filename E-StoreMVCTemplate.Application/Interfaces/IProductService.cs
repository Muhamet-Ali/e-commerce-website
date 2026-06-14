using E_StoreMVCTemplate.Application.DTOs.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace E_StoreMVCTemplate.Application.Interfaces
{
    public  interface IProductService : IGenericService
        < GetProductListDto,GetProductByIdDto,CreateProductDto,UpdateProductDto >
    {
        Task<List<GetProductByCategoryDTO>> GetProductByCategoryAsync(int  categoryId);
        Task<List<GetPopulerProductList>> GetPopulerProductListAsync();
        Task<List<GetProductBySubCategoryDTO>> GetProductBySubCategoryAsync(int subCategoryId);
        Task<List<GetRelatedProductDto>> GetRelatedProductAsync(int ProductId);



    }
}
