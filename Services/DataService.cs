using Password_manager.Models;
using Password_manager.Services.Interface;
using System.Text.Json;

namespace Password_manager.Services
{
    /// <summary>
    /// 資料處理業務
    /// </summary>
    public class DataService : IDataService
    {
        private readonly string _filepath;
        private readonly object _lock = new(); //鎖，防多重寫入
        public DataService(IConfiguration configuration)
        {
            _filepath = configuration["Storage:DataPath"]!;
            Directory.CreateDirectory(Path.GetDirectoryName(_filepath)!); //未找到目錄時手動建立
        }

        public List<DataItem> GetAll()
        {
            lock (_lock) return ReadAll();
        }

        public DataItem? GetById(string id)
        {
            lock (_lock) return ReadAll().FirstOrDefault(x => x.Id == id);
        }

        public void Add(DataItem item)
        {
            lock (_lock)
            {
                var items = ReadAll();
                items.Add(item);
                WriteAll(items);
            }
        }

        public void Update(DataItem item)
        {
            lock (_lock)
            {
                var items = ReadAll();
                var index = items.FindIndex(x => x.Id == item.Id);
                if (index >= 0) items[index] = item;
                WriteAll(items);
            }
        }

        public void Delete(string id)
        {
            lock (_lock)
            {
                var items = ReadAll();
                items.RemoveAll(x => x.Id == id);
                WriteAll(items);
            }
        }

        #region 內部方法
        /// <summary>
        /// 讀取方法
        /// </summary>
        /// <returns>資料List</returns>
        private List<DataItem> ReadAll()
        {
            if (!File.Exists(_filepath)) return new List<DataItem>();
            var json = File.ReadAllText(_filepath);
            return JsonSerializer.Deserialize<List<DataItem>>(json) ?? new List<DataItem>();
        }

        /// <summary>
        /// 寫入方法
        /// </summary>
        /// <param name="items"></param>
        private void WriteAll(List<DataItem> items)
        {
            var json = JsonSerializer.Serialize(items, new JsonSerializerOptions
            {
                WriteIndented = true
            });
            File.WriteAllText(_filepath, json);
        }
        #endregion 
    }
}
