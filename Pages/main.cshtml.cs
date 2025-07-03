using Microsoft.AspNetCore.Mvc.RazorPages;
using Password_manager.Models;

public class PasswordsModel : PageModel
{
    public List<PasswordItem> passwordList { get; set; } = new();

    public void OnGet()
    {
        passwordList = new List<PasswordItem>
        {
            new PasswordItem { Id = "1", Site = "Gmail", Username = "user@gmail.com" },
            new PasswordItem { Id = "2", Site = "GitHub", Username = "dev@gh.com" }
        };
    }
}
