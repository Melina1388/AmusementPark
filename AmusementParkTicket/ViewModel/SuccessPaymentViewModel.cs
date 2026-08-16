namespace AmusementPark.wb.ViewModels
{
    public class SuccessPaymentViewModel
    {
        public int Id { get; set; }

        public string TrackingCode { get; set; }
            = string.Empty;

        public string CardHolderName { get; set; }
            = string.Empty;

        public string MaskedCardNumber { get; set; }
            = string.Empty;

        public decimal TotalAmount { get; set; }
    }
}