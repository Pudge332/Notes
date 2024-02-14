using Microsoft.AspNetCore.Mvc;

namespace Notes.WebApi.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Registration()
        {
            return View();
        }

        public IActionResult Login()
        {
            return View();
        }
    }
}
