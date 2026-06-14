namespace E_StoreMVCTemplate.Application.DTOs.Payment
{
    public class StartPaymentDto
    {
        public string UserId { get; set; } = null!;
        public string Address { get; set; } = null!;
        public string CallbackUrl { get; set; } = null!;
        public string IpAddress { get; set; } = null!;
    }
}
