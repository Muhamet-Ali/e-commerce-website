using E_StoreMVCTemplate.Application.DTOs.AppUser;
using E_StoreMVCTemplate.Application.DTOs.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_StoreMVCTemplate.Application.Interfaces
{
    public interface IAuthService
    {
        
        Task <List<GetUserListDto>> GetAllAsync();
        Task<GetUserByIdDto> GetByIdAsync(string id);
        Task UpdateAsync(UpdateUserDto dto);

        Task<string> LoginAsync(LoginDto dto);
        Task RegisterAsync(RegisterDto dto);
        Task LogoutAsync();
    }
}
