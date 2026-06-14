using E_StoreMVCTemplate.Application.DTOs.Payment;
using E_StoreMVCTemplate.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace E_StoreMVCTemplate.Web.Controllers
{
    [Authorize]
    public class PaymentController : Controller
    {
        private readonly IPaymentService _paymentService;

        public PaymentController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        [HttpPost]
        public async Task<IActionResult> Checkout(string Address)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var dto = new StartPaymentDto
            {
                UserId = userId,
                Address = Address,
                CallbackUrl = Url.Action(nameof(Callback), "Payment", null, Request.Scheme),
                IpAddress = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "127.0.0.1"
            };

            var paymentForm = await _paymentService.StartPaymentAsync(dto);
            return View(paymentForm);
        }

        [HttpPost]
        public async Task<IActionResult> Callback(string token)
        {
            var result = await _paymentService.ConfirmPaymentAsync(token);
            return View("Result", result);
        }
    }
}
