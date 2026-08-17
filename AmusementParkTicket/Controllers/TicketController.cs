using System.Reflection.Metadata.Ecma335;
using Microsoft.AspNetCore.Mvc;

namespace AmusementPark.wb.Controllers
{
    public class TicketController : Controller
    {

        public IActionResult Ticket()
        {
            return View();
        }
    }
}
