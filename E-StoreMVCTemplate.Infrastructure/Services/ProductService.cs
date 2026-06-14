using E_StoreMVCTemplate.Application.DTOs.Images;
using E_StoreMVCTemplate.Application.DTOs.Product;
using E_StoreMVCTemplate.Application.DTOs.Reviews;
using E_StoreMVCTemplate.Application.Interfaces;
using E_StoreMVCTemplate.Domain.Entities;
using E_StoreMVCTemplate.Infrastructure.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace E_StoreMVCTemplate.Infrastructure.Services
{
   
    public class ProductService : IProductService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public ProductService(IHttpContextAccessor httpContextAccessor, AppDbContext context, IWebHostEnvironment env)
        {
            _httpContextAccessor = httpContextAccessor;
            _context = context;
            _env = env;
        }

        public async Task AddAsync(CreateProductDto dto)
        {
            var product = new Product
            {
                Name = dto.Name,
                Description = dto.Description,
                Price = dto.Price,
                Stock = dto.Stock,
                IsActive = dto.IsActive,
                CreatedAt = dto.CreatedAt,
                CategoryId = dto.CategoryId,
                SubCategoryId = dto.SubCategoryId
            };

            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();

            if (dto.Images != null && dto.Images.Any())
            {
                var newImages = new List<ProductImages>();

                foreach (var imageDto in dto.Images)
                {
                    if (imageDto.File == null) continue;

                    var fileName = $"{Guid.NewGuid()}{Path.GetExtension(imageDto.File.FileName)}";
                    var savePath = Path.Combine(_env.WebRootPath, "images", "products", fileName);

                    Directory.CreateDirectory(Path.GetDirectoryName(savePath)!);

                    using (var stream = new FileStream(savePath, FileMode.Create))
                    {
                        await imageDto.File.CopyToAsync(stream);
                    }

                    newImages.Add(new ProductImages
                    {
                        ProductId = product.Id,
                        ImageUrl = fileName,
                        DisplayOrder = imageDto.DisplayOrder
                    });
                }

                await _context.ProductImages.AddRangeAsync(newImages);
                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteAsync(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null) return;

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
        }

        public async Task<List<GetProductListDto>> GetAllAsync()
        {
            return await _context.Products.Select(p => new GetProductListDto
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                Price = p.Price,
                Stock = p.Stock,
                DiscountPrice = p.DiscountPrice,
                DiscountId = p.DiscountId,
                DiscountName=p.Discountcs.Name,
                CategoryId = p.CategoryId,
                CategoryName = p.Category.Name,
                FavoriteCount = p.Favorites.Count,
                Images = p.Images.Select(i => new ProductImageDTO
                {
                    ImageUrl = i.ImageUrl
                }).ToList(),
            }).ToListAsync();
        }

        public async Task<GetProductByIdDto> GetByIdAsync(int id)
        {

        var userId = _httpContextAccessor.HttpContext.User
            .FindFirstValue(ClaimTypes.NameIdentifier);

            return await _context.Products
                .Where(p => p.Id == id)
                .Select(p => new GetProductByIdDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    Description = p.Description,
                    Price = p.Price,
                    DiscountPrice = p.DiscountPrice,
                    Stock = p.Stock,
                    IsActive = p.IsActive,
                    DiscountId = p.DiscountId,
                    DiscountName = p.Discountcs != null ? p.Discountcs.Name : null,
                    CategoryId = p.CategoryId,
                    CategoryName = p.Category.Name,
                    SubCategoryId = p.SubCategoryId,
                    SubCategoryName = p.SubCategory != null ? p.SubCategory.Name : null,
                    FavoriteCount = p.Favorites.Count,
                    IsFavorite = p.Favorites.Any(f => f.UserId == userId),
                    Reviews = p.Reviews.Select(r => new GetProductReviews
                    {
                        Id = r.Id,
                        Comment = r.Comment,
                        Rating = r.Rating,
                        CreatedAt = r.CreatedAt
                    }).ToList(),
                    Images = p.Images.Select(i => new ProductImageDTO
                    {
                        ImageUrl = i.ImageUrl
                    }).ToList()
                })
                .FirstOrDefaultAsync();
        }

        public async Task<List<GetPopulerProductList>> GetPopulerProductListAsync()
        {
            return await _context.Products
                .Select(p => new
                {
                    Product = p,
                    TotalSold = _context.OrderItems
                        .Where(oi => oi.ProductId == p.Id)
                        .Sum(oi => (int?)oi.Quantity) ?? 0
                })
                .Where(x => x.TotalSold > 0)
                .OrderByDescending(x => x.TotalSold)
                .Take(10)
                .Select(x => new GetPopulerProductList
                {
                    Id = x.Product.Id,
                    Name = x.Product.Name,
                    Description = x.Product.Description,
                    Price = x.Product.Price,
                    DiscountPrice = x.Product.DiscountPrice,
                    Stock = x.Product.Stock,
                    DiscountId = x.Product.DiscountId,
                    DiscountName = x.Product.Discountcs != null ? x.Product.Discountcs.Name : null,
                    CategoryId = x.Product.CategoryId,
                    CategoryName = x.Product.Category != null ? x.Product.Category.Name : null,
                    FavoriteCount = x.Product.Favorites.Count,
                    Images = x.Product.Images
                        .OrderBy(i => i.DisplayOrder)
                        .Select(i => new ProductImageDTO
                        {
                            Id = i.Id,
                            ImageUrl = i.ImageUrl,
                            DisplayOrder = i.DisplayOrder,
                        }).ToList()
                })
                .ToListAsync();
        }

        public async Task<List<GetProductByCategoryDTO>> GetProductByCategoryAsync(int categoryId)
        {
            return await _context.Products
                .Where(p => p.CategoryId == categoryId)
                .Select(p => new GetProductByCategoryDTO
                {
                    Id = p.Id,
                    Name = p.Name,
                    Description = p.Description,
                    Price = p.Price,
                    DiscountPrice = p.DiscountPrice,
                    DiscountId = p.DiscountId,
                    DiscountName = p.Discountcs != null ? p.Discountcs.Name : null,
                    FavoriteCount = p.Favorites.Count,
                    Images = p.Images.Select(i => new ProductImageDTO
                    {
                        ImageUrl = i.ImageUrl
                    }).ToList()
                })
                .ToListAsync();
        }

        public async Task<List<GetProductBySubCategoryDTO>> GetProductBySubCategoryAsync(int subCategoryId)
        {
            return await _context.Products
                .Where(p => p.SubCategoryId == subCategoryId)
                .Select(p => new GetProductBySubCategoryDTO
                {
                    Id = p.Id,
                    Name = p.Name,
                    Description = p.Description,
                    Price = p.Price,
                    DiscountPrice = p.DiscountPrice,
                    DiscountId = p.DiscountId,
                    DiscountName = p.Discountcs != null ? p.Discountcs.Name : null,
                    FavoriteCount = p.Favorites.Count,
                    Images = p.Images.Select(i => new ProductImageDTO
                    {
                        ImageUrl = i.ImageUrl
                    }).ToList()
                })
                .ToListAsync();
        }

        public async Task<List<GetRelatedProductDto>> GetRelatedProductAsync(int productId)
        {
            var product = await _context.Products
                .FirstOrDefaultAsync(x => x.Id == productId);

            if (product == null)
                return new List<GetRelatedProductDto>();

            List<Product> relatedProducts;

            if (product.SubCategoryId.HasValue)
            {
                relatedProducts = await _context.Products.Include(x => x.Images)
                    .Where(x => x.Id != productId && x.SubCategoryId == product.SubCategoryId)
                    .Take(4)
                    .ToListAsync();
            }
            else
            {
                relatedProducts = await _context.Products.Include(x => x.Images)
                    .Where(x => x.Id != productId && x.CategoryId == product.CategoryId)
                    .Take(4)
                    .ToListAsync();
            }

            return relatedProducts.Select(x => new GetRelatedProductDto
            {
                Id = x.Id,
                Name = x.Name,
                Price = x.Price,
                Images = x.Images
                    .OrderBy(i => i.DisplayOrder)
                    .Take(1)
                    .Select(i => new ProductImageDTO
                    {
                        Id = i.Id,
                        ImageUrl = i.ImageUrl,
                        DisplayOrder = i.DisplayOrder
                    })
                    .ToList()
            }).ToList();
        }

        public async Task UpdateAsync(UpdateProductDto dto)
        {
            var product = await _context.Products
                .Include(p => p.Images)
                .FirstOrDefaultAsync(p => p.Id == dto.Id);

            if (product == null)
                throw new Exception("Product not found.");

            product.Name = dto.Name;
            product.Description = dto.Description;
            product.Price = dto.Price;
            product.Stock = dto.Stock;
            product.IsActive = dto.IsActive;
            product.CategoryId = dto.CategoryId;
            product.SubCategoryId = dto.SubCategoryId;

            if (dto.RemovedImageIds != null && dto.RemovedImageIds.Any())
            {
                var toRemove = product.Images
                    .Where(i => dto.RemovedImageIds.Contains(i.Id))
                    .ToList();

                foreach (var oldImage in toRemove)
                {
                    var oldPath = Path.Combine(_env.WebRootPath, "images", "products", oldImage.ImageUrl);
                    if (File.Exists(oldPath))
                        File.Delete(oldPath);
                }

                _context.ProductImages.RemoveRange(toRemove);
            }

            if (dto.Images != null && dto.Images.Any())
            {
                var maxOrder = product.Images
                    .Where(i => dto.RemovedImageIds == null || !dto.RemovedImageIds.Contains(i.Id))
                    .Select(i => (int?)i.DisplayOrder)
                    .Max() ?? -1;

                var newImages = new List<ProductImages>();
                foreach (var imageDto in dto.Images)
                {
                    if (imageDto.File == null || imageDto.File.Length == 0) continue;

                    var fileName = $"{Guid.NewGuid()}{Path.GetExtension(imageDto.File.FileName)}";
                    var savePath = Path.Combine(_env.WebRootPath, "images", "products", fileName);

                    Directory.CreateDirectory(Path.GetDirectoryName(savePath)!);

                    using (var stream = new FileStream(savePath, FileMode.Create))
                    {
                        await imageDto.File.CopyToAsync(stream);
                    }

                    maxOrder++;
                    newImages.Add(new ProductImages
                    {
                        ProductId = dto.Id,
                        ImageUrl = fileName,
                        DisplayOrder = maxOrder
                    });
                }

                await _context.ProductImages.AddRangeAsync(newImages);
            }

            await _context.SaveChangesAsync();
        }
    }


}
