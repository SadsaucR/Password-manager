using Password_manager.Models;

namespace Password_manager.Services.Interface
{
    public interface IDataService
    {
        List<DataItem> GetAll();
        DataItem? GetById(string id);
        void Add(DataItem item);
        void Update(DataItem item);
        void Delete(string id);
    }
}
