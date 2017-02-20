using Microsoft.AspNetCore.Mvc;

namespace Bit.CodingTest.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Error()
        {
            return View();
        }
    }
}
