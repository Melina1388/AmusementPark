using System.ComponentModel.DataAnnotations;

namespace AmusementPark.wb.ViewModels
{
    public class PaymentViewModel
    {
        public decimal TotalAmount { get; set; }


        [Required(
            ErrorMessage =
                "شماره کارت را وارد کنید.")]
        public string CardNumber { get; set; }
            = string.Empty;


        [Required(
            ErrorMessage =
                "نام دارنده کارت را وارد کنید.")]
        public string CardHolderName { get; set; }
            = string.Empty;


        [Required(
            ErrorMessage =
                "تاریخ انقضا را وارد کنید.")]
        public string Expiry { get; set; }
            = string.Empty;


        [Required(
            ErrorMessage =
                "CVV2 را وارد کنید.")]
        public string CVV { get; set; }
            = string.Empty;
    }
}