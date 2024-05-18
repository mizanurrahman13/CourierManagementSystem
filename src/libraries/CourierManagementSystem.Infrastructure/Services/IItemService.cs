using CourierManagementSystem.Infrastructure.BusinessObjects;

namespace CourierManagementSystem.Infrastructure.Services;

public interface IItemService
{
    Task CreateItem(Item item);
    Task<(int total, int displayTotal, IList<Item> records)>
        GetItemAsync(int pageIndex,
        int pageSize,
        string searchText,
        string orderBy);
    Task DeleteItemAsync(Guid id);
    Task<Item> GetItemByIdAsync(Guid id);
    Task UpdateItemAsync(Item item);
    Task<List<Item>> GetItemsAsync(Guid? id);
    void DeleteItem(Guid id);
    Item GetItemById(Guid id);
    Task<IList<Item>> GetAllAsync();
    IList<Item> GetAll();
}
