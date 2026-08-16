using AmusementPark.Application.Interfaces;
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


        public ShopBasketController(
            IShopBasketService shopBasketService)
        {
            _shopBasketService =
                shopBasketService;
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