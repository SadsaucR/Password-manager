namespace Password_manager.Services.Interface
{
    public interface IAccountService
    {
        public int VerifyAccount(string username, string password); //驗證登入資訊
    }
}
