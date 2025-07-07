using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Password_manager.Models;
using System.Text.Json;
using System.Security.Cryptography;
using System.Text;

namespace Password_manager.Pages.api
{
    [IgnoreAntiforgeryToken]
    public class PasswordApi:PageModel
    {
        private static readonly string DataFile = "App_Data/passwords.json";
        private static readonly string AesKey = "your-32-byte-long-key-123456789012"; // 32 bytes for AES-256
        private static readonly string AesIV = "your-16-byte-iv-1234"; // 16 bytes for AES

        // 取得所有密碼
        public IActionResult OnGet()
        {
            var items = LoadItems();
            return new JsonResult(items);
        }

        // 新增密碼
        public IActionResult OnPost([FromBody] PasswordItem item)
        {
            var items = LoadItems();
            item.Id = Guid.NewGuid().ToString();
            item.Password = Encrypt(item.Password);
            items.Add(item);
            SaveItems(items);
            return new JsonResult(item);
        }

        // 刪除密碼
        public IActionResult OnDelete(string id)
        {
            var items = LoadItems();
            var item = items.FirstOrDefault(x => x.Id == id);
            if (item != null)
            {
                items.Remove(item);
                SaveItems(items);
                return new JsonResult(new { success = true });
            }
            return NotFound();
        }

        // 編輯密碼
        public IActionResult OnPut([FromBody] PasswordItem item)
        {
            var items = LoadItems();
            var existing = items.FirstOrDefault(x => x.Id == item.Id);
            if (existing != null)
            {
                existing.Site = item.Site;
                existing.Username = item.Username;
                existing.Password = Encrypt(item.Password);
                SaveItems(items);
                return new JsonResult(existing);
            }
            return NotFound();
        }

        // 載入資料
        private List<PasswordItem> LoadItems()
        {
            if (!System.IO.File.Exists(DataFile))
                return new List<PasswordItem>();
            var json = System.IO.File.ReadAllText(DataFile);
            var items = JsonSerializer.Deserialize<List<PasswordItem>>(json) ?? new List<PasswordItem>();
            // 解密密碼
            foreach (var item in items)
                item.Password = Decrypt(item.Password);
            return items;
        }

        // 儲存資料
        private void SaveItems(List<PasswordItem> items)
        {
            // 儲存前加密密碼
            var saveList = items.Select(i => new PasswordItem
            {
                Id = i.Id,
                Site = i.Site,
                Username = i.Username,
                Password = Encrypt(i.Password)
            }).ToList();
            var json = JsonSerializer.Serialize(saveList, new JsonSerializerOptions { WriteIndented = true });
            Directory.CreateDirectory(Path.GetDirectoryName(DataFile)!);
            System.IO.File.WriteAllText(DataFile, json);
        }

        // AES 加密
        private string Encrypt(string plainText)
        {
            using var aes = Aes.Create();
            aes.Key = Encoding.UTF8.GetBytes(AesKey);
            aes.IV = Encoding.UTF8.GetBytes(AesIV);
            var encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
            var plainBytes = Encoding.UTF8.GetBytes(plainText);
            var cipherBytes = encryptor.TransformFinalBlock(plainBytes, 0, plainBytes.Length);
            return Convert.ToBase64String(cipherBytes);
        }

        // AES 解密
        private string Decrypt(string cipherText)
        {
            using var aes = Aes.Create();
            aes.Key = Encoding.UTF8.GetBytes(AesKey);
            aes.IV = Encoding.UTF8.GetBytes(AesIV);
            var decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
            var cipherBytes = Convert.FromBase64String(cipherText);
            var plainBytes = decryptor.TransformFinalBlock(cipherBytes, 0, cipherBytes.Length);
            return Encoding.UTF8.GetString(plainBytes);
        }
    }
}
