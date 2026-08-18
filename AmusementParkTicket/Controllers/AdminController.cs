using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AmusementPark.wb.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        [HttpGet]
        public IActionResult Admin()
        {
            var userType =
                User.FindFirst("UserType")?.Value;

            if (userType != "True")
            {
                return Forbid();
            }

            return View();
        }
    }
}