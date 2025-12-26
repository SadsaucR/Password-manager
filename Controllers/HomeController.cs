using Microsoft.AspNetCore.Mvc;

namespace Password_manager.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Main()
        {
            return View();
        }
    }
}
