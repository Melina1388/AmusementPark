namespace AmusementPark.Application.DTOs
{
    /// <summary>
    /// نتیجه موفقیت‌آمیز عملیات پرداخت.
    /// </summary>
    public class PaymentResultDto
    {
        public int TransactionId { get; set; }

        public string TrackingCode { get; set; }
            = string.Empty;

        public string CardHolderName { get; set; }
            = string.Empty;

        public string MaskedCardNumber { get; set; }
            = string.Empty;

        public decimal TotalAmount { get; set; }
    }
}