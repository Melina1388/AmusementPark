using Microsoft.AspNetCore.Mvc;

namespace AmusementPark.wb.Controllers
{
    public class ShopBasketController : Controller
    {
        public IActionResult ShopBasket()
        {
            return View();
        }
    }
}
