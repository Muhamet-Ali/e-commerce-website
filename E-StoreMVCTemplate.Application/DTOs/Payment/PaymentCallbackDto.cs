using E_StoreMVCTemplate.Domain.Enums;

namespace E_StoreMVCTemplate.Application.DTOs.Payment
{
    public class PaymentCallbackDto
    {
        public int PaymentId { get; set; }
        public PaymentStatus Status { get; set; }
        public string? IyzicoPaymentId { get; set; }
        public string? ErrorMessage { get; set; }
    }
}
