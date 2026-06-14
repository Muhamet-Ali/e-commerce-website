using E_StoreMVCTemplate.Application.DTOs.Auth;
using E_StoreMVCTemplate.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace E_StoreMVCTemplate.Web.Controllers
{
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpGet]
        public IActionResult Login(string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginDto dto, string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;

            if (!ModelState.IsValid)
            {
                return View(dto);
            }

            try
            {
                await _authService.LoginAsync(dto);
                TempData["SuccessMessage"] = "Welcome back!";

                if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                {
                    return Redirect(returnUrl);
                }

                return RedirectToAction("Index", "Home");
            }
            catch (ApplicationException ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(dto);
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "An unexpected error occurred. Please try again later.");
                return View(dto);
            }
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterDto dto)
        {
            if (!ModelState.IsValid)
            {
                return View(dto);
            }

            try
            {
                await _authService.RegisterAsync(dto);
                TempData["SuccessMessage"] = "Your account has been created successfully. You can now log in.";
                return RedirectToAction(nameof(Login));
            }
            catch (ApplicationException ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(dto);
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "An unexpected error occurred while creating your account. Please try again later.");
                return View(dto);
            }
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            try
            {
                await _authService.LogoutAsync();
                TempData["SuccessMessage"] = "You have been logged out successfully.";
            }
            catch (ApplicationException ex)
            {
                TempData["ErrorMessage"] = ex.Message;
            }
            catch (Exception)
            {
                TempData["ErrorMessage"] = "An unexpected error occurred while logging out.";
            }

            return RedirectToAction("Index", "Home");
        }

        [Authorize]
        public async Task<IActionResult> GetProfile()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
            {
                TempData["ErrorMessage"] = "Your session has expired. Please log in again.";
                return RedirectToAction(nameof(Login));
            }

            try
            {
                var user = await _authService.GetByIdAsync(userId);
                return View(user);
            }
            catch (Exception)
            {
                TempData["ErrorMessage"] = "Profile could not be loaded.";
                return RedirectToAction("Index", "Home");
            }
        }
    }
}
