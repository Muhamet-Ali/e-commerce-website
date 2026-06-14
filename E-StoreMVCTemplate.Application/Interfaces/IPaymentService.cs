using E_StoreMVCTemplate.Application.DTOs.Payment;

namespace E_StoreMVCTemplate.Application.Interfaces
{
    public interface IPaymentService
    {
        Task<PaymentFormDto> StartPaymentAsync(StartPaymentDto dto);
        Task<PaymentCallbackDto> ConfirmPaymentAsync(string token);
    }
}
