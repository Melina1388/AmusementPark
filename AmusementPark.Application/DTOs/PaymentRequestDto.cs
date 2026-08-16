namespace AmusementPark.Application.DTOs
{
    /// <summary>
    /// اطلاعات مورد نیاز برای انجام پرداخت.
    ///
    /// این DTO به View یا MVC وابسته نیست.
    /// </summary>
    public class PaymentRequestDto
    {
        public int PlayerId { get; set; }

        public string CardNumber { get; set; }
            = string.Empty;

        public string CardHolderName { get; set; }
            = string.Empty;

        public string Expiry { get; set; }
            = string.Empty;

        public string CVV { get; set; }
            = string.Empty;
    }
}