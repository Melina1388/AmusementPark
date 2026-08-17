using AmusementPark.Application.DTOs;
using AmusementPark.Application.Interfaces;
using AmusementPark.Domain.Entities;

namespace AmusementPark.Application.Services
{
    /// <summary>
    /// سرویس انجام عملیات پرداخت.
    ///
    /// این کلاس:
    /// 1. اطلاعات پرداخت را بررسی می‌کند.
    /// 2. مبلغ واقعی سبد را دریافت می‌کند.
    /// 3. کد پیگیری ایجاد می‌کند.
    /// 4. Transaction ایجاد می‌کند.
    /// 5. Transaction را ثبت می‌کند.
    ///
    /// این کلاس هیچ اطلاعی از MVC، Session یا View ندارد.
    /// </summary>
    public class PaymentService : IPaymentService
    {
        private readonly IShopBasketService
            _shopBasketService;

        private readonly ITransactionService
            _transactionService;
        private readonly ITicketService
    _ticketService;


        public PaymentService(
      IShopBasketService shopBasketService,
      ITransactionService transactionService,
      ITicketService ticketService)
        {
            _shopBasketService =
                shopBasketService;

            _transactionService =
                transactionService;

            _ticketService =
                ticketService;
        }

        public PaymentResultDto ProcessPayment(
            PaymentRequestDto request)
        {
            ValidateRequest(request);


            // مبلغ واقعی از سبد خرید گرفته می‌شود.
            // بنابراین کاربر نمی‌تواند مبلغ فرم را دستکاری کند.

            decimal totalAmount =
                _shopBasketService.GetTotal();


            if (totalAmount <= 0)
            {
                throw new InvalidOperationException(
                    "Shopping basket is empty.");
            }


            string cleanCardNumber =
                NormalizeCardNumber(
                    request.CardNumber);


            string trackingCode =
                GenerateTrackingCode();


            Transaction transaction =
                new Transaction
                {
                    PlayerID = request.PlayerId,

                    // شماره کامل کارت ذخیره نمی‌شود.
                    CardNum =
                        MaskCardNumber(
                            cleanCardNumber),

                    TotalPrice =
                        totalAmount,

                    TrackingNum =
                        trackingCode
                };


            int transactionId =
                _transactionService.Add(
                    transaction);

            List<ShopBasketItemDto> basketItems =
    _shopBasketService.GetBasket();

            foreach (ShopBasketItemDto item in basketItems)
            {
                for (int i = 0; i < item.Quantity; i++)
                {
                    Ticket ticket = new Ticket
                    {
                        PlayerID = request.PlayerId,

                        GameID = item.GameID,

                        TransactionID = transactionId,

                        IsUsed = "Unused"
                    };

                    _ticketService.Add(ticket);
                }
            }
            _shopBasketService.Clear();
            return new PaymentResultDto
            {
                TransactionId =
                    transactionId,

                TrackingCode =
                    trackingCode,

                CardHolderName =
                    request.CardHolderName,

                MaskedCardNumber =
                    transaction.CardNum ?? "****",

                TotalAmount =
                    totalAmount
            };
        }


        private static void ValidateRequest(
            PaymentRequestDto request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(
                    nameof(request));
            }


            string cardNumber =
                NormalizeCardNumber(
                    request.CardNumber);


            if (cardNumber.Length != 16 ||
                !cardNumber.All(char.IsDigit))
            {
                throw new ArgumentException(
                    "Card number must contain 16 digits.");
            }


            if (string.IsNullOrWhiteSpace(
                    request.CardHolderName))
            {
                throw new ArgumentException(
                    "Card holder name is required.");
            }


            if (string.IsNullOrWhiteSpace(
                    request.Expiry))
            {
                throw new ArgumentException(
                    "Card expiry is required.");
            }


            if (string.IsNullOrWhiteSpace(
                    request.CVV) ||
                !request.CVV.All(char.IsDigit) ||
                (request.CVV.Length != 3 &&
                 request.CVV.Length != 4))
            {
                throw new ArgumentException(
                    "CVV is invalid.");
            }
        }


        private static string NormalizeCardNumber(
            string cardNumber)
        {
            return (cardNumber ?? string.Empty)
                .Replace(" ", "")
                .Trim();
        }


        private static string MaskCardNumber(
            string cardNumber)
        {
            if (cardNumber.Length < 4)
            {
                return "****";
            }


            return
                "**** **** **** "
                + cardNumber[^4..];
        }


        private static string GenerateTrackingCode()
        {
            return
                DateTime.UtcNow
                    .ToString("yyyyMMddHHmmss")
                +
                Random.Shared.Next(
                    1000,
                    9999);
        }
    }
}