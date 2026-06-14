using E_StoreMVCTemplate.Application.DTOs.Discount;
using E_StoreMVCTemplate.Application.DTOs.Images;
using E_StoreMVCTemplate.Application.DTOs.Product;
using E_StoreMVCTemplate.Application.Interfaces;
using E_StoreMVCTemplate.Domain.Entities;
using E_StoreMVCTemplate.Infrastructure.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_StoreMVCTemplate.Infrastructure.Services
{
    public class DiscountService:IDiscountService
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;
        public DiscountService(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        public async Task AddAsync(CreateDiscountDto dto)
        {
            string? fileNameOnly = null;

            // 🔥 IMAGE SAVE
            if (dto.File != null)
            {
                var fileName = $"{Guid.NewGuid()}{Path.GetExtension(dto.File.FileName)}";
                var savePath = Path.Combine(_env.WebRootPath, "images", "discounts", fileName);

                Directory.CreateDirectory(Path.GetDirectoryName(savePath)!);

                using (var stream = new FileStream(savePath, FileMode.Create))
                {
                    await dto.File.CopyToAsync(stream);
                }

                fileNameOnly = fileName;
            }

            // 🔥 DISCOUNT CREATE
            var discount = new Discountcs
            {
                Name = dto.Name,
                Rate = dto.Rate,
                StartDate = dto.StartDate,
                EndDate = dto.EndDate,
                IsActive = dto.IsActive,
                ImageUrl = fileNameOnly // ❗ BURASI EKSİKTİ
            };

            await _context.Discounts.AddAsync(discount);
            await _context.SaveChangesAsync();

            // 🔥 PRODUCT BAĞLAMA
            if (dto.ProductIds != null && dto.ProductIds.Any())
            {
                var products = await _context.Products
                    .Where(p => dto.ProductIds.Contains(p.Id))
                    .ToListAsync();

                foreach (var product in products)
                {
                    product.DiscountId = discount.Id;
                    product.DiscountPrice=product.Price - (product.Price * discount.Rate / 100);
                }

                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteAsync(int id)
        {
            var discount = await _context.Discounts
                .FirstOrDefaultAsync(d => d.Id == id);

            if (discount == null) return;

            var products = await _context.Products
                .Where(p => p.DiscountId == id)
                .ToListAsync();

            foreach (var product in products)
            {
                product.DiscountId = null;
            }

            _context.Discounts.Remove(discount);
            await _context.SaveChangesAsync();
        }
        
        public async Task<List<GetDiscountForUserDto>> GetActiveDiscountsForUserAsync()
        {
            var now = DateTime.UtcNow;

            return await _context.Discounts
                .Where(d => d.IsActive && d.StartDate <= now && d.EndDate >= now)
                .Select(d => new GetDiscountForUserDto
                {
                    Id = d.Id,
                    Name = d.Name,
                    ImageUrl = d.ImageUrl,
                    Rate = d.Rate,
                    StartDate = d.StartDate,
                    EndDate = d.EndDate
                })
                .ToListAsync();
        }

        public async Task<List<GetDiscountListDto>> GetAllAsync()
        {
            return await _context.Discounts
                .Select(d => new GetDiscountListDto
                {
                    Id = d.Id,
                    Name = d.Name,
                    ImageUrl = d.ImageUrl,
                    Rate = d.Rate,
                    StartDate = d.StartDate,
                    EndDate = d.EndDate,
                    IsActive = d.IsActive,
                    ProductCount = d.Products.Count()
                })
                .ToListAsync();
        }

        public async Task<GetDiscountByIdDto?> GetByIdAsync(int id)
        {
            return await _context.Discounts
                .Where(d => d.Id == id)
                .Select(d => new GetDiscountByIdDto
                {
                    Id = d.Id,
                    Name = d.Name,
                    ImageUrl = d.ImageUrl,
                    Rate = d.Rate,
                    StartDate = d.StartDate,
                    EndDate = d.EndDate,
                    IsActive = d.IsActive,

                    Products = d.Products.Select(p => new GetProductListDto
                    {
                        Id = p.Id,
                        Name = p.Name,
                        Price = p.Price,
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

                    }).ToList()
                })
                .FirstOrDefaultAsync();
        }

        public async Task UpdateAsync(UpdateDiscountDto dto)
        {
            var discount = await _context.Discounts
                .Include(d => d.Products)
                .FirstOrDefaultAsync(d => d.Id == dto.Id);

            if (discount == null) return;

            if (dto.File != null)
            {
                var fileName = $"{Guid.NewGuid()}{Path.GetExtension(dto.File.FileName)}";
                var savePath = Path.Combine(_env.WebRootPath, "images", "discounts", fileName);

                Directory.CreateDirectory(Path.GetDirectoryName(savePath)!);

                using (var stream = new FileStream(savePath, FileMode.Create))
                {
                    await dto.File.CopyToAsync(stream);
                }

                discount.ImageUrl = fileName; // sadece isim
            }

            discount.Name = dto.Name;
            discount.Rate = dto.Rate;
            discount.StartDate = dto.StartDate;
            discount.EndDate = dto.EndDate;
            discount.IsActive = dto.IsActive;

            var oldProducts = await _context.Products
                .Where(p => p.DiscountId == discount.Id)
                .ToListAsync();

            foreach (var product in oldProducts)
            {
                product.DiscountId = null;
            }

            if (dto.ProductIds != null && dto.ProductIds.Any())
            {
                var newProducts = await _context.Products
                    .Where(p => dto.ProductIds.Contains(p.Id))
                    .ToListAsync();

                foreach (var product in newProducts)
                {
                    product.DiscountId = discount.Id;
                }
            }

            await _context.SaveChangesAsync();
        }
    
    }
}
