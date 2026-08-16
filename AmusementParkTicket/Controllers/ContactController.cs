using Microsoft.AspNetCore.Mvc;

namespace AmusementPark.wb.Controllers
{
    public class ContactController : Controller
    {

        public IActionResult Contact()
        {
            return View();
        }
    }
}
