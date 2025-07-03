namespace Password_manager.Models
{
    public class PasswordItem
    {
        public string Id { get; set; } = Guid.NewGuid().ToString(); //唯一碼
        public string Site { get; set; } = "";                      //網站類型
        public string Username { get; set; } = "";                  //帳號
        public string Password { get; set; } = "";                  //加密後儲存
    }
}
