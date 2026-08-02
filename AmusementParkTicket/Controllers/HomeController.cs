using Microsoft.AspNetCore.Mvc;

namespace AmusementPark.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Home()
        {
            return View();
        }
    }
}
