using E_StoreMVCTemplate.Application.DTOs.Category;
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
    public class CategoryService : ICategoryService
    {
        private readonly AppDbContext _context;

        public CategoryService(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(CreateCategoryDto dto)
        {

            var Category = new Category
            {
                Name = dto.Name,
                DisplayOrder = dto.DisplayOrder,
            };
            _context.Categories.Add(Category);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var category = await _context.Categories.FirstOrDefaultAsync(c => c.Id == id);
            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();

        }
        public async Task<List<GetCategoryListDto>> GetAllAsync()
        {
            return await _context.Categories
         .OrderByDescending(c => c.DisplayOrder)
         .Select(c => new GetCategoryListDto
         {
             Id = c.Id,
             Name = c.Name,
             DisplayOrder = c.DisplayOrder,
             ProductCount = c.Products.Count,

             SubCategory = c.SubCategories
                 .Select(sc => new GetSubForCategory
                 {
                     Id = sc.Id,
                     Name = sc.Name
                 }).ToList()
         })
         .ToListAsync();
        }

        public async Task<List<GetCategoryWithSub>> GetCategAndSub()
        {
            return await _context.Categories.Include(c => c.SubCategories)
                .Select(c => new GetCategoryWithSub
                {
                    Id = c.Id,
                    Name = c.Name,
                    subCategories = c.SubCategories.Select(sub => new GetSubCateogryListDto
                    {
                        Id = sub.Id,
                        Name = sub.Name,

                    }).ToList(),

                }).ToListAsync();
        }
        public async Task<GetCategoryByIdWithProductDto> GetByIdAsync(int id)
        {


            return await _context.Categories.Where(c => c.Id == id)
                .Select(c => new GetCategoryByIdWithProductDto
                {
                    Id = c.Id,
                    Name = c.Name,
                    DisplayOrder = c.DisplayOrder,
                    ProductCount = c.Products.Count,
                    Products = c.Products.Select(p => new GetProductListDto
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

        public async Task UpdateAsync(UpdateCategoryDto dto)
        {
            var Category = await _context.Categories.FirstOrDefaultAsync(c => c.Id == dto.Id);
            if (Category == null)
                return;

            Category.Name = dto.Name;
            Category.DisplayOrder = dto.DisplayOrder;

            await _context.SaveChangesAsync();
        }
    }
}
