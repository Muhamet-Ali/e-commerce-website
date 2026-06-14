using E_StoreMVCTemplate.Application.DTOs.Payment;
using E_StoreMVCTemplate.Application.Interfaces;
using E_StoreMVCTemplate.Domain.Enums;
using E_StoreMVCTemplate.Infrastructure.Data;
using Iyzipay;
using Iyzipay.Model;
using Iyzipay.Request;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Globalization;

namespace E_StoreMVCTemplate.Infrastructure.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;

        public PaymentService(AppDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task<PaymentFormDto> StartPaymentAsync(StartPaymentDto dto)
        {
            var cart = await _context.Carts
                .Include(c => c.User)
                .Include(c => c.CartItems)
                    .ThenInclude(ci => ci.Product)
                        .ThenInclude(p => p.Category)
                .Include(c => c.CartItems)
                    .ThenInclude(ci => ci.Product)
                        .ThenInclude(p => p.SubCategory)
                .FirstOrDefaultAsync(c => c.UserId == dto.UserId);

            if (cart == null || !cart.CartItems.Any())
                throw new Exception("Cart is empty.");

            var amount = cart.CartItems.Sum(x => x.UnitPrice * x.Quantity);
            var conversationId = Guid.NewGuid().ToString();

            var payment = new E_StoreMVCTemplate.Domain.Entities.Payment
            {
                UserId = dto.UserId,
                CartId = cart.Id,
                Amount = amount,
                Currency = Currency.TRY.ToString(),
                Status = PaymentStatus.Pending,
                ConversationId = conversationId,
                CreatedDate = DateTime.UtcNow
            };

            await _context.Payments.AddAsync(payment);
            await _context.SaveChangesAsync();

            var request = new CreateCheckoutFormInitializeRequest
            {
                Locale = Locale.TR.ToString(),
                ConversationId = conversationId,
                Price = FormatPrice(amount),
                PaidPrice = FormatPrice(amount),
                Currency = Currency.TRY.ToString(),
                BasketId = cart.Id.ToString(),
                PaymentGroup = PaymentGroup.PRODUCT.ToString(),
                CallbackUrl = dto.CallbackUrl,
                Buyer = CreateBuyer(cart.User, dto),
                ShippingAddress = CreateAddress(cart.User, dto.Address),
                BillingAddress = CreateAddress(cart.User, dto.Address),
                BasketItems = cart.CartItems.Select(x => new BasketItem
                {
                    Id = x.ProductId.ToString(),
                    Name = x.Product.Name,
                    Category1 = x.Product.Category?.Name ?? "General",
                    Category2 = x.Product.SubCategory?.Name ?? "General",
                    ItemType = BasketItemType.PHYSICAL.ToString(),
                    Price = FormatPrice(x.UnitPrice * x.Quantity)
                }).ToList()
            };

            var result = await CheckoutFormInitialize.Create(request, CreateOptions());

            if (result.Status != "success")
            {
                payment.Status = PaymentStatus.Failed;
                await _context.SaveChangesAsync();
                throw new Exception(result.ErrorMessage ?? "Payment form could not be initialized.");
            }

            payment.Token = result.Token;
            await _context.SaveChangesAsync();

            return new PaymentFormDto
            {
                PaymentId = payment.Id,
                ConversationId = payment.ConversationId,
                Token = result.Token,
                CheckoutFormContent = result.CheckoutFormContent,
                PaymentPageUrl = result.PaymentPageUrl
            };
        }

        public async Task<PaymentCallbackDto> ConfirmPaymentAsync(string token)
        {
            var payment = await _context.Payments.FirstOrDefaultAsync(x => x.Token == token);

            if (payment == null)
                throw new Exception("Payment not found.");

            var request = new RetrieveCheckoutFormRequest
            {
                Locale = Locale.TR.ToString(),
                ConversationId = payment.ConversationId,
                Token = token
            };

            var result = await CheckoutForm.Retrieve(request, CreateOptions());
            var isSuccess = result.Status == "success" && result.PaymentStatus == "SUCCESS";

            payment.Status = isSuccess ? PaymentStatus.Success : PaymentStatus.Failed;
            payment.IyzicoPaymentId = result.PaymentId;
            payment.PaidDate = isSuccess ? DateTime.UtcNow : null;

            await _context.SaveChangesAsync();

            return new PaymentCallbackDto
            {
                PaymentId = payment.Id,
                Status = payment.Status,
                IyzicoPaymentId = payment.IyzicoPaymentId,
                ErrorMessage = isSuccess ? null : result.ErrorMessage
            };
        }

        private Options CreateOptions()
        {
            return new Options
            {
                ApiKey = _configuration["Iyzipay:ApiKey"],
                SecretKey = _configuration["Iyzipay:SecretKey"],
                BaseUrl = _configuration["Iyzipay:BaseUrl"]
            };
        }

        private static Buyer CreateBuyer(E_StoreMVCTemplate.Domain.Entities.AppUser user, StartPaymentDto dto)
        {
            var names = (user.FullName ?? user.UserName ?? "Customer").Split(' ', StringSplitOptions.RemoveEmptyEntries);

            return new Buyer
            {
                Id = user.Id,
                Name = names.FirstOrDefault() ?? "Customer",
                Surname = names.Skip(1).FirstOrDefault() ?? "User",
                Email = user.Email ?? "customer@example.com",
                GsmNumber = user.PhoneNumber ?? "+905555555555",
                IdentityNumber = "11111111111",
                RegistrationAddress = dto.Address,
                Ip = dto.IpAddress,
                City = "Istanbul",
                Country = "Turkey",
                ZipCode = "34000"
            };
        }

        private static Address CreateAddress(E_StoreMVCTemplate.Domain.Entities.AppUser user, string address)
        {
            return new Address
            {
                ContactName = user.FullName ?? user.UserName ?? "Customer User",
                City = "Istanbul",
                Country = "Turkey",
                Description = address,
                ZipCode = "34000"
            };
        }

        private static string FormatPrice(decimal price)
        {
            return price.ToString("0.00", CultureInfo.InvariantCulture);
        }
    }
}
