using Password_manager.Services.Interface;

namespace Password_manager.Services
{
    public class AccountService : IAccountService
    {
        private readonly IConfiguration _configuration;
        public AccountService(IConfiguration configuration) //注入設定檔內容，從appsetting獲取
        {
            _configuration = configuration;
        }

        /// <summary>
        /// 驗證登入資訊
        /// </summary>
        /// <returns> 正確(0),帳號或密碼錯誤(1),未知錯誤(2) </returns>
        public int VerifyAccount(string username, string password)
        {
            int status = 1; 
            try
            {
                string correctUsername = _configuration["DevAccount:Username"]!;
                string correctPassword = _configuration["DevAccount:Password"]!;
                bool exist = (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password) ? false : true);
                if (exist)
                    status = username == correctUsername && password == correctPassword ? 0 : 1;
                else status = 2;
            }
            catch (Exception ex)
            {
                status = 2;
                //logger add
            }
            return status;
        }
    }
}
