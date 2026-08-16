using AmusementPark.Application.DTOs;
using AmusementPark.Application.Interfaces;
using AmusementPark.Domain.Entities;
using Application.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace AmusementPark.wb.Controllers
{
    /// <summary>
    /// Controller مربوط به نمایش و مدیریت سبد خرید.
    /// </summary>
    public class ShopBasketController : Controller
    {
        private readonly IShopBasketService
      _shopBasketService;

        private readonly IGameService
            _gameService;


        public ShopBasketController(
     IShopBasketService shopBasketService,
     IGameService gameService)
        {
            _shopBasketService =
                shopBasketService;

            _gameService =
                gameService;
        }

        [HttpPost]
        public IActionResult SelectTicket(
     [FromBody] SelectTicketRequestDto request)
        {
            if (request == null || request.GameId <= 0)
            {
                return BadRequest(new
                {
                    success = false,
                    message = "اطلاعات بلیت نامعتبر است."
                });
            }

            int quantity = request.Quantity;

            if (quantity < 0)
            {
                return BadRequest(new
                {
                    success = false,
                    message = "تعداد بلیت نامعتبر است."
                });
            }

            bool alreadyExists =
                _shopBasketService.ContainsGame(
                    request.GameId);

            // ============================================
            // اگر تعداد صفر شده، بازی را حذف کن
            // ============================================

            if (quantity == 0)
            {
                _shopBasketService.RemoveItem(
                    request.GameId);

                return Ok(new
                {
                    success = true,
                    isNewItem = false,
                    quantity = 0
                });
            }

            // ============================================
            // اگر بازی قبلاً در سبد هست،
            // فقط تعداد را مستقیم تنظیم کن
            // ============================================

            if (alreadyExists)
            {
                _shopBasketService.SetQuantity(
                    request.GameId,
                    quantity);

                return Ok(new
                {
                    success = true,
                    isNewItem = false,
                    quantity = quantity
                });
            }

            // ============================================
            // بازی هنوز در سبد نیست
            // ============================================

            Game? game =
                _gameService.GetById(
                    request.GameId);

            if (game == null)
            {
                return NotFound(new
                {
                    success = false,
                    message = "بازی پیدا نشد."
                });
            }

            ShopBasketItemDto item =
                new ShopBasketItemDto
                {
                    GameID =
                        game.GameID!.Value,

                    GameName =
                        game.GameName,

                    AmusementName =
                        game.AmusementName,

                    GamePrice =
                        game.GamePrice ?? 0,

                    Quantity =
                        quantity
                };

            _shopBasketService.AddItem(item);

            return Ok(new
            {
                success = true,

                // فقط این حالت باید پیام
                // «بلیت با موفقیت ثبت شد»
                // را نمایش دهد.
                isNewItem = true,

                quantity = quantity
            });
        }

        // ============================================
        // نمایش سبد خرید
        // ============================================

        [HttpGet]
        public IActionResult ShopBasket()
        {
            var basket =
                _shopBasketService.GetBasket();


            return View(basket);
        }


        // ============================================
        // افزایش تعداد
        // ============================================

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Increase(
            int gameId)
        {
            _shopBasketService
                .IncreaseQuantity(gameId);


            return RedirectToAction(
                nameof(ShopBasket));
        }


        // ============================================
        // کاهش تعداد
        // ============================================

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Decrease(
            int gameId)
        {
            _shopBasketService
                .DecreaseQuantity(gameId);


            return RedirectToAction(
                nameof(ShopBasket));
        }


        // ============================================
        // حذف محصول
        // ============================================

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Remove(
            int gameId)
        {
            _shopBasketService
                .RemoveItem(gameId);


            return RedirectToAction(
                nameof(ShopBasket));
        }
    }
}