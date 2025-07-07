using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;

namespace Password_manager.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        [BindProperty]
        public string Username { get; set; } = "";

        [BindProperty]
        public string Password { get; set; } = "";

        public string? ErrorMessage { get; set; }

        public void OnGet()
        {
        }
        public async Task<IActionResult> OnPostAsync()
        {
            // TODO: �o�̽Ч令�A�ۤv���b���K�X�����޿�
            if (Username == "admin" && Password == "123456")
            {
                // �إ� Claims
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, Username)
                };
                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                // �n�J�]�g�J Cookie�^
                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity));

                // �ɦV main ��
                return RedirectToPage("/main");
            }
            else
            {
                ErrorMessage = "�b���αK�X���~";
                return Page();
            }
        }
    }
}
