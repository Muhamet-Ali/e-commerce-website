using E_StoreMVCTemplate.Domain.Enums;

namespace E_StoreMVCTemplate.Domain.Entities
{
    public class Payment
    {
        public int Id { get; set; }
        public string UserId { get; set; } = null!;
        public AppUser User { get; set; } = null!;
        public int CartId { get; set; }
        public Cart Cart { get; set; } = null!;
        public decimal Amount { get; set; }
        public string Currency { get; set; } = "TRY";
        public PaymentStatus Status { get; set; } = PaymentStatus.Pending;
        public string ConversationId { get; set; } = null!;
        public string? Token { get; set; }
        public string? IyzicoPaymentId { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public DateTime? PaidDate { get; set; }
    }
}
