namespace E_StoreMVCTemplate.Application.DTOs.Payment
{
    public class PaymentFormDto
    {
        public int PaymentId { get; set; }
        public string ConversationId { get; set; } = null!;
        public string Token { get; set; } = null!;
        public string CheckoutFormContent { get; set; } = null!;
        public string? PaymentPageUrl { get; set; }
    }
}
