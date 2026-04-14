namespace Password_manager.Models
{
    public class LoginVM
    {
        public string Username { get; set; } = "";
        public string Password { get; set; } = "";
        public int? Errorcode { get; set; } 
    }
}
