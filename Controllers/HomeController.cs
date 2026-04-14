using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Password_manager.Models;
using Password_manager.Services.Interface;
using System.Security.Claims;

namespace Password_manager.Controllers
{
    public class HomeController : Controller
    {
        private readonly IAccountService _accountService;
        public HomeController(IAccountService accountService)
        {
            _accountService = accountService;
        }
        //頁面宣告
        public IActionResult Index()
        {
            return View(new LoginVM());
        }
        [Authorize]
        public IActionResult Main()
        {
            return View();
        }
        //API實作
        [HttpPost]
        public async Task<IActionResult> Index(LoginVM model)
        {
            var result = _accountService.VerifyAccount(model.Username, model.Password);

            if (result == 0)
            {
                var claims = new List<Claim> //Claim 為伺服器認證用class
                  {
                      new Claim(ClaimTypes.Name, model.Username)
                  };
                var identity = new ClaimsIdentity(claims, "Cookies");
                var principal = new ClaimsPrincipal(identity);

                await HttpContext.SignInAsync("Cookies", principal);
                return RedirectToAction("Main");
            }

            model.Errorcode = result;
            return View(model);
        }
    }
}
