using E_StoreMVCTemplate.Application.DTOs.HeaderImage;
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
    public class HeaderImageService : IHeadeImagesService
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public HeaderImageService(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        public async Task AddAsync(CreateHeadeImagesDto dto)
        {
            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(dto.File.FileName)}";
            var savePath = Path.Combine(_env.WebRootPath, "images", "header", fileName);

            Directory.CreateDirectory(Path.GetDirectoryName(savePath)!);

            using (var stream = new FileStream(savePath, FileMode.Create))
            {
                await dto.File.CopyToAsync(stream);
            }

            var headerImage = new HeaderImages
            {
                DisplayOrder = dto.DisplayOrder,
                ImageUrl = fileName,
                Title = dto.Title
            };

            await _context.HeaderImages.AddAsync(headerImage);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var headerImage = await _context.HeaderImages.FirstOrDefaultAsync(x => x.Id == id);
            if (headerImage == null) return;

            _context.HeaderImages.Remove(headerImage);
            await _context.SaveChangesAsync();
        }

        public async Task<List<GetHeadeImagesListDto>> GetAllAsync()
        {
            return await _context.HeaderImages
                .OrderByDescending(h => h.DisplayOrder)
                .Select(h => new GetHeadeImagesListDto
                {
                    Id = h.Id,
                    DisplayOrder = h.DisplayOrder,
                    ImageUrl = h.ImageUrl,
                    Title = h.Title,
                    IsActive = h.IsActive
                }).ToListAsync();
        }

        public Task<GetHeadeImagesByIdDto> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(UpdateHeadeImagesDto dto)
        {
            throw new NotImplementedException();
        }
    }

}
