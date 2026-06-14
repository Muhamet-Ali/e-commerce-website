using E_StoreMVCTemplate.Application.DTOs.Images;
using E_StoreMVCTemplate.Application.DTOs.Product;
using E_StoreMVCTemplate.Application.DTOs.SubCategory;
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
    public class SubService : ISubCategoryService
    {
        private readonly AppDbContext _context;

        public SubService(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(CreateSubCategoryDto dto)
        {
            var SubCategory = new SubCategory
            {
                Name = dto.Name,
                CategoryId = dto.CategoryId,
            };
            _context.SubCategories.Add(SubCategory);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var subCategory = await _context.SubCategories.Where(s => s.Id == id).FirstOrDefaultAsync();
            if (subCategory == null)
                return;
            _context.SubCategories.Remove(subCategory);
            await _context.SaveChangesAsync();

        }

        public async Task<List<GetSubCateogryListDto>> GetAllAsync()
        {
            return await _context.SubCategories.Select(s => new GetSubCateogryListDto
            {
                Id = s.Id,
                Name = s.Name,
                ProductCount = s.Products.Count,
                CateogryId = s.CategoryId,
                CateogryName = s.Category.Name,

            }).ToListAsync();
        }

        public async Task<GetSubCateogryByIdWithProductListDto> GetByIdAsync(int id)
        {
            return await _context.SubCategories.Where(s => s.Id == id)
                .Select(s => new GetSubCateogryByIdWithProductListDto
                {
                    Id = s.Id,
                    Name = s.Name,
                    ProductCount = s.Products.Count,
                    CategoryId = s.CategoryId,
                    CategoryName = s.Category.Name,
                    Products = s.Products.Select(p => new GetProductListDto
                    {
                        Id = p.Id,
                        Name = p.Name,
                        Price = p.Price,
                        Stock = p.Stock,
                        Description = p.Description,
                        DiscountId = p.DiscountId,
                        DiscountPrice = p.DiscountPrice,
                        CategoryId = p.CategoryId,
                        CategoryName = p.Category.Name,
                        FavoriteCount = p.Favorites.Count,
                        Images = p.Images.Select(i => new ProductImageDTO
                        {
                            ImageUrl = i.ImageUrl
                        }).ToList(),

                    }).ToList(),
                }).FirstOrDefaultAsync();
        }

        public async Task UpdateAsync(UpdateSubCategoryDto dto)
        {
            var sub = await _context.SubCategories.FirstOrDefaultAsync(s => s.Id == dto.Id);
            if (sub == null)
                return;

            sub.Name = dto.Name;
            sub.CategoryId = dto.CategoryId;

            await _context.SaveChangesAsync();
        }
    }
}
