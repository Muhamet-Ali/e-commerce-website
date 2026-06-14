using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_StoreMVCTemplate.Application.DTOs.SubCategory
{
    public class GetSubCateogryListDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int ProductCount { get; set; }
        public int CateogryId { get; set; }
        public string CateogryName { get; set; }
    }
}
