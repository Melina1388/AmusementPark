using AmusementPark.Application.DTOs;
using AmusementPark.Application.Interfaces;
using AmusementPark.wb.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace AmusementPark.wb.Controllers
{
  
    /// Controller مربوط به صفحات پرداخت.
    ///
    /// Controller فقط مسئول ارتباط HTTP با View و Service است.
    /// منطق کسب‌وکار داخل Application قرار دارد.
  
    public class PaymentController : Controller
    {
        private readonly IShopBasketService
            _shopBasketService;

        private readonly IPaymentService
            _paymentService;


        public PaymentController(
            IShopBasketService shopBasketService,
            IPaymentService paymentService)
        {
            _shopBasketService =
                shopBasketService;

            _paymentService =
                paymentService;
        }


        // ============================================
        // نمایش صفحه پرداخت
        // ============================================

        [HttpGet]
        public IActionResult Payment()
        {
            decimal totalAmount =
                _shopBasketService.GetTotal();


            if (totalAmount <= 0)
            {
                return RedirectToAction(
                    "ShopBasket",
                    "ShopBasket");
            }


            PaymentViewModel model =
                new PaymentViewModel
                {
                    TotalAmount =
                        totalAmount
                };


            return View(model);
        }


        // ============================================
        // انجام پرداخت
        // ============================================

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Payment(
            PaymentViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.TotalAmount =
                    _shopBasketService.GetTotal();

                return View(model);
            }


            // PlayerId در LoginController پروژه شما
            // با همین کلید داخل Session ذخیره می‌شود.

            string? playerIdValue =
                HttpContext.Session.GetString(
                    "PlayerId");


            if (!int.TryParse(
                    playerIdValue,
                    out int playerId))
            {
                return RedirectToAction(
                    "Login",
                    "Login");
            }


            try
            {
                PaymentRequestDto request =
                    new PaymentRequestDto
                    {
                        PlayerId =
                            playerId,

                        CardNumber =
                            model.CardNumber,

                        CardHolderName =
                            model.CardHolderName,

                        Expiry =
                            model.Expiry,

                        CVV =
                            model.CVV
                    };


                PaymentResultDto result =
                    _paymentService.ProcessPayment(
                        request);


                // پرداخت موفق بوده،
                // بنابراین سبد خرید خالی می‌شود.

                _shopBasketService.Clear();


                SuccessPaymentViewModel successModel =
                    new SuccessPaymentViewModel
                    {
                        Id =
                            result.TransactionId,

                        TrackingCode =
                            result.TrackingCode,

                        CardHolderName =
                            result.CardHolderName,

                        MaskedCardNumber =
                            result.MaskedCardNumber,

                        TotalAmount =
                            result.TotalAmount
                    };


                return View(
                    "SuccessPayment",
                    successModel);
            }
            catch (ArgumentException ex)
            {
                ModelState.AddModelError(
                    string.Empty,
                    ex.Message);

                model.TotalAmount =
                    _shopBasketService.GetTotal();

                return View(model);
            }
            catch (InvalidOperationException ex)
            {
                ModelState.AddModelError(
                    string.Empty,
                    ex.Message);

                model.TotalAmount =
                    _shopBasketService.GetTotal();

                return View(model);
            }
        }
    }
}