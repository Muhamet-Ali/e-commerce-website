using E_StoreMVCTemplate.Application.DTOs.Images;
using E_StoreMVCTemplate.Application.DTOs.SubCategory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_StoreMVCTemplate.Application.DTOs.Category
{
    public class GetCategoryListDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int DisplayOrder { get; set; }
        public int ProductCount { get; set; }

        public List<GetSubForCategory> SubCategory { get; set; }

    }
}
