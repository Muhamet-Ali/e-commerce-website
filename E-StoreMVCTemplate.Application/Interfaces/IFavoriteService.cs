using E_StoreMVCTemplate.Application.DTOs.Favorite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_StoreMVCTemplate.Application.Interfaces
{
    public interface IFavoriteService
    {
        Task AddAsync(CreateFavoriteDto dto);
        Task DeleteAsync(int id,string userId);
        Task<List<GetUserFavoriteListDto>> GetFavoriteUserAsync(string UserId );
    }
}
