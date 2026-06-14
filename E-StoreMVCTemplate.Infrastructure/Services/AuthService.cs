using E_StoreMVCTemplate.Application.DTOs.AppUser;
using E_StoreMVCTemplate.Application.DTOs.Auth;
using E_StoreMVCTemplate.Application.Interfaces;
using E_StoreMVCTemplate.Domain.Entities;
using E_StoreMVCTemplate.Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
namespace E_StoreMVCTemplate.Infrastructure.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly AppDbContext _context;

        public AuthService(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, AppDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
        }

        public async Task<List<GetUserListDto>> GetAllAsync()
        {
            return await _context.Users.Include(u => u.Orders)
                  .Select(x => new GetUserListDto
                  {
                      Id = x.Id,
                      FullName = x.UserName,
                      Email = x.Email,
                      PhoneNumber = x.PhoneNumber,
                      OrderCount = x.Orders.Count(),
                  })
                  .ToListAsync();
        }

        public async Task<GetUserByIdDto> GetByIdAsync(string id)
        {
            var user = await _context.Users
                .Where(x => x.Id == id)
                .Select(x => new GetUserByIdDto
                {
                    Id = x.Id,
                    FullName = x.UserName,
                    Email = x.Email,
                    Address = x.Address,
                    PhoneNumber = x.PhoneNumber,
                })
                .FirstOrDefaultAsync();

            if (user == null)
            {
                throw new Exception("Kullanici bulunamadi");
            }

            return user;
        }

        public async Task<string> LoginAsync(LoginDto dto)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(dto.Email) || string.IsNullOrWhiteSpace(dto.Password))
                    throw new ApplicationException("Email and password are required.");

                var user = await _userManager.FindByEmailAsync(dto.Email);
                if (user == null)
                    throw new ApplicationException("Invalid email or password.");

                var check = await _signInManager.PasswordSignInAsync(user, dto.Password, false, true);

                if (check.IsLockedOut)
                    throw new ApplicationException("Your account is temporarily locked due to too many failed attempts. Please try again later.");

                if (check.IsNotAllowed)
                    throw new ApplicationException("Login is not allowed for this account. Please confirm your email or contact support.");

                if (!check.Succeeded)
                    throw new ApplicationException("Invalid email or password.");

                return "Login successful.";
            }
            catch (ApplicationException)
            {
                throw;
            }
            catch (Exception)
            {
                throw new ApplicationException("An unexpected error occurred during login. Please try again.");
            }
        }

        public async Task RegisterAsync(RegisterDto dto)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(dto.UserName))
                    throw new ApplicationException("Username is required.");

                if (string.IsNullOrWhiteSpace(dto.Email))
                    throw new ApplicationException("Email is required.");

                if (string.IsNullOrWhiteSpace(dto.Password))
                    throw new ApplicationException("Password is required.");

                var existingByEmail = await _userManager.FindByEmailAsync(dto.Email);
                if (existingByEmail != null)
                    throw new ApplicationException("This email is already registered.");

                var existingByName = await _userManager.FindByNameAsync(dto.UserName);
                if (existingByName != null)
                    throw new ApplicationException("This username is already taken.");

                var user = new AppUser
                {
                    UserName = dto.UserName,
                    Email = dto.Email,
                    PhoneNumber = dto.PhoneNumber,
                };

                var result = await _userManager.CreateAsync(user, dto.Password);
                if (!result.Succeeded)
                {
                    var errors = string.Join(" ", result.Errors.Select(e => e.Description));
                    throw new ApplicationException(string.IsNullOrWhiteSpace(errors)
                        ? "Account could not be created."
                        : errors);
                }
            }
            catch (ApplicationException)
            {
                throw;
            }
            catch (Exception)
            {
                throw new ApplicationException("An unexpected error occurred during registration. Please try again.");
            }
        }

        public async Task LogoutAsync()
        {
            try
            {
                await _signInManager.SignOutAsync();
            }
            catch (Exception)
            {
                throw new ApplicationException("An error occurred while logging out. Please try again.");
            }
        }

        public async Task UpdateAsync(UpdateUserDto dto)
        {
            var user = await _userManager.FindByIdAsync(dto.Id);

            if (user == null)
            {
                throw new Exception("Kullanici bulunamadi");
            }

            user.UserName = dto.UserName;
            user.Email = dto.UserEmail;
            user.PhoneNumber = dto.PhoneNumber;

            var result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded)
            {
                throw new Exception("Kullanici guncellenemedi");
            }
        }
    }
}
