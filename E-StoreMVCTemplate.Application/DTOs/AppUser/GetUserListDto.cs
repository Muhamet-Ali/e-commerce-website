using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_StoreMVCTemplate.Application.DTOs.AppUser
{
    public class GetUserListDto
    {
        public string Id { get; set; }
        public string? FullName { get; set; }
        public int OrderCount { get; set; }
        public string Email  { get; set; }
        public string PhoneNumber { get; set; }

    }
}
