using DevSkill.Data;
using ItemEntity = CourierManagementSystem.Infrastructure.Entities.Item;
using ItemBO = CourierManagementSystem.Infrastructure.BusinessObjects.Item;
using System.Linq.Expressions;

namespace CourierManagementSystem.Infrastructure.Repositories;

public interface IItemRepository : IRepository<ItemEntity, Guid>
{
    Task<int> IsCategoryAlreadyExists(ItemBO itemBO);
    IList<ItemEntity> Get(Expression<Func<ItemEntity, bool>> filter, string includeProperties = "");
    Task<IList<ItemEntity>> GetAsync(Expression<Func<ItemEntity, bool>> filter, string includeProperties = "");
}
