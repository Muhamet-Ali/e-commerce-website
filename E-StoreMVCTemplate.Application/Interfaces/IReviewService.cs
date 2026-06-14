using E_StoreMVCTemplate.Application.DTOs.Reviews;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_StoreMVCTemplate.Application.Interfaces
{
    public interface IReviewService
    {
        Task AddAsync(CreateReviewsDto dto);
        Task<List<GetProductReviews>> GetProductReviews(int Id);
        Task DeleteAsync(int id);
        Task<List<GetReviewsDto>> GetAllAsync();
        Task<UpdateReviewDto?> GetByIdAsync(int id);
        Task UpdateAsync(UpdateReviewDto dto);
    }
}
