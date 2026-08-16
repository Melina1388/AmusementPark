using AmusementPark.Application.DTOs;
using AmusementPark.Application.Interfaces;
using AmusementPark.wb.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

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

        private readonly IShopBasketPersistenceService
    _shopBasketPersistenceService;

        public PaymentController(
            IShopBasketService shopBasketService,
            IPaymentService paymentService,
            IShopBasketPersistenceService shopBasketPersistenceService)
        {
            _shopBasketService =
                shopBasketService;

            _paymentService =
                paymentService;
            _shopBasketPersistenceService =
    shopBasketPersistenceService;
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


            // PlayerID از Claim احراز هویت خوانده می‌شود.
            // این مقدار از Cookie معتبر Login می‌آید
            // و از Client دریافت نمی‌شود.

            string? playerIdValue =
                User.FindFirstValue(
                    ClaimTypes.NameIdentifier);

            if (!int.TryParse(
                    playerIdValue,
                    out int playerId) ||
                playerId <= 0)
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


                // پرداخت موفق بوده است.
                // بنابراین هم سبد Session و هم نسخه ثبت‌شده
                // در جدول ShopBasket باید پاک شوند.

                _shopBasketPersistenceService
                    .ClearBasket(playerId);

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