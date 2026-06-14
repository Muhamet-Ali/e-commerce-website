using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_StoreMVCTemplate.Application.DTOs.AppUser
{
    public class UpdateUserDto
    {
        public string  Id { get; set; }
        public string UserName { get; set; }
        public string? UserEmail { get; set; }
        public string PhoneNumber { get; set; }
        public string Password { get; set; }
    }
}

