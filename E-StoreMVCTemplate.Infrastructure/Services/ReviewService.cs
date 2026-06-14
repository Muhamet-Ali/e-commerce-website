using E_StoreMVCTemplate.Application.DTOs.Reviews;
using E_StoreMVCTemplate.Application.Interfaces;
using E_StoreMVCTemplate.Domain.Entities;
using E_StoreMVCTemplate.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_StoreMVCTemplate.Infrastructure.Services
{
    public class ReviewService : IReviewService
    {
        private readonly AppDbContext _context;

        public ReviewService(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(CreateReviewsDto dto)
        {
          
            var Review = new Review
            {
                Rating = dto.Rating,
                Comment = dto.Comment,
                ProductId = dto.ProductId,
                UserId = dto.UserId,
            };
            _context.Reviews.Add(Review);    
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var reviwe=await _context.Reviews.FindAsync(id);
            if (reviwe == null) return; // ✅ ekle

            _context.Reviews.Remove(reviwe);
            await _context.SaveChangesAsync();
        }

        public async Task<List<GetReviewsDto>> GetAllAsync()
        {
            return await _context.Reviews.Include(r => r.Product).Include(r => r.User).Select(r => new GetReviewsDto
            {
                Id=r.Id,
                Rating=r.Rating,
                Comment=r.Comment,
                IsApproved=r.IsApproved,
                CreatedAt=r.CreatedAt,
                ProductId=r.ProductId,
                ProductName=r.Product.Name,
                UserId=r.UserId,
                UserName=r.User.UserName,

            }).ToListAsync();
        }

        public async Task<UpdateReviewDto?> GetByIdAsync(int id)
        {
            var review = await _context.Reviews.FindAsync(id);
            if (review == null) return null;

            return new UpdateReviewDto
            {
                Id = review.Id,
                Rating = review.Rating,
                Comment = review.Comment,
                IsApproved = review.IsApproved,
            };
        }

        public async Task UpdateAsync(UpdateReviewDto dto)
        {
            var review = await _context.Reviews.FindAsync(dto.Id);
            if (review == null) return;

            review.Rating = dto.Rating;
            review.Comment = dto.Comment;
            review.IsApproved = dto.IsApproved;

            await _context.SaveChangesAsync();
        }

        public async Task<List<GetProductReviews>> GetProductReviews(int Id)
        {

            return await _context.Reviews.Where(r => r.ProductId == Id).Select(r => new GetProductReviews
            {
                Id=r.Id,
                Rating=r.Rating,
                Comment=r.Comment,
                CreatedAt=r.CreatedAt,
                UserId=r.UserId,
                UserName=r.User.UserName,
            }).ToListAsync();

        }
    }
}
