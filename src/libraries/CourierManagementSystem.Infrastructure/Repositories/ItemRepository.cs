using CourierManagementSystem.Infrastructure.Contexts;
using DevSkill.Data;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using ItemBO = CourierManagementSystem.Infrastructure.BusinessObjects.Item;
using CourierManagementSystem.Infrastructure.Entities;

namespace CourierManagementSystem.Infrastructure.Repositories;

public class ItemRepository : Repository<Item, Guid>, IItemRepository
{
    public ItemRepository(ApplicationDbContext context) : base(context)
    {

    }

    public async Task<int> IsCategoryAlreadyExists(ItemBO item)
    {
        return await GetCountAsync(x => x.Id == item.Id && x.Id != item.Id);
    }

    public virtual IList<Item> Get(Expression<Func<Item, bool>> filter, string includeProperties = "")
    {
        IQueryable<Item> query = _dbSet;

        if (filter != null)
        {
            query = query.Where(filter);
        }

        foreach (var includeProperty in includeProperties.Split
            (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
        {
            query = query.Include(includeProperty);
        }

        return query.ToList();
    }

    public virtual async Task<IList<Item>> GetAsync(Expression<Func<Item, bool>> filter, string includeProperties = "")
    {
        IQueryable<Item> query = _dbSet;

        if (filter != null)
        {
            query = query.Where(filter);
        }

        foreach (var includeProperty in includeProperties.Split
            (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
        {
            query = query.Include(includeProperty);
        }

        //return query.ToList();
        return await EntityFrameworkQueryableExtensions.ToListAsync(query);
    }
}
