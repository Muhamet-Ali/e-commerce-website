using E_StoreMVCTemplate.Application.DTOs.SubCategory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_StoreMVCTemplate.Application.DTOs.Category
{
    public  class GetCategoryWithSub
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<GetSubCateogryListDto> subCategories { get; set; }

    }
}
