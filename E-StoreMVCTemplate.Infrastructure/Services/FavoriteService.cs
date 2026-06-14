using E_StoreMVCTemplate.Application.DTOs.Favorite;
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
    public class FavoriteService : IFavoriteService
    {
        private readonly AppDbContext _context;

        public FavoriteService(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(CreateFavoriteDto dto)
        {
            //user product
            var Favorite = new Favorite
            {
                UserId = dto.UserId,
                ProductId = dto.ProductId,

            };
            _context.Favorites.Add(Favorite);
            await _context.SaveChangesAsync();

        }

        public async Task DeleteAsync(int id ,string userId)
        {
            var Favorite = await _context.Favorites
                .FirstOrDefaultAsync(f => f.ProductId == id && f.UserId==userId);
            if (Favorite == null) { return; }
            _context.Favorites.Remove(Favorite);
            await _context.SaveChangesAsync();
        }

        public async Task<List<GetUserFavoriteListDto>> GetFavoriteUserAsync(string UserId)
        {
            return await _context.Favorites
                .Where(f => f.UserId == UserId)
                .Select(f => new GetUserFavoriteListDto
                {
                    FavoriteId = f.Id,
                    ProductId = f.ProductId,
                    ProductName = f.Product.Name,
                    ProductImageUrl = f.Product.Images
                        .Select(i => i.ImageUrl)
                        .FirstOrDefault(),
                    ProductPrice = f.Product.Price,
                    CreatedAt = f.CreatedAt,
                })
                .ToListAsync();
        }
    }
}

