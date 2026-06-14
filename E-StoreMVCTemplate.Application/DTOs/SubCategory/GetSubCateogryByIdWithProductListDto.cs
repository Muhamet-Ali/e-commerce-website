using E_StoreMVCTemplate.Application.DTOs.Product;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_StoreMVCTemplate.Application.DTOs.SubCategory
{
    public class GetSubCateogryByIdWithProductListDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int ProductCount { get; set; }

        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public List<GetProductListDto> Products { get; set; }
    }
}
