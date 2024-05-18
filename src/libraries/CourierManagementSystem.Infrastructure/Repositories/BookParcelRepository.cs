using CourierManagementSystem.Infrastructure.Contexts;
using DevSkill.Data;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using BookParcelBO = CourierManagementSystem.Infrastructure.BusinessObjects.BookParcel;
using CourierManagementSystem.Infrastructure.Entities;
using System.Linq.Dynamic.Core;

namespace CourierManagementSystem.Infrastructure.Repositories;

public class BookParcelRepository : Repository<BookParcel, Guid>, IBookParcelRepository
{
    public BookParcelRepository(ApplicationDbContext context) : base(context)
    {

    }

    public async Task<int> IsBookingAlreadyExists(BookParcelBO bookParcel)
    {
        return await GetCountAsync(x => x.BookParcelFromName == bookParcel.BookParcelFromName && x.Id != bookParcel.Id);
    }

    public virtual IList<BookParcel> Get(Expression<Func<BookParcel, bool>> filter, string includeProperties = "")
    {
        IQueryable<BookParcel> query = _dbSet;

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

    public virtual async Task<IList<BookParcel>> GetAsync(Expression<Func<BookParcel, bool>> filter, string includeProperties = "")
    {
        IQueryable<BookParcel> query = _dbSet;

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

    public virtual (IList<BookParcel> data, int total, int totalDisplay) GetDynamic(
            Expression<Func<BookParcel, bool>> filter = null!,
            string orderBy = null!,
            string includeProperties = "", int pageIndex = 1, int pageSize = 10, bool isTrackingOff = false)
    {
        IQueryable<BookParcel> query = _dbSet;
        var total = query.Count();
        var totalDisplay = query.Count();

        if (filter != null)
        {
            query = query.Where(filter);
            totalDisplay = query.Count();
        }

        foreach (var includeProperty in includeProperties.Split
            (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
        {
            query = query.Include(includeProperty);
        }

        if (orderBy != null)
        {
            var result = query.OrderBy(orderBy).Skip((pageIndex - 1) * pageSize).Take(pageSize);
            if (isTrackingOff)
                return (result.AsNoTracking().ToList(), total, totalDisplay);
            else
                return (result.ToList(), total, totalDisplay);
        }
        else
        {
            var result = query.Skip((pageIndex - 1) * pageSize).Take(pageSize);
            if (isTrackingOff)
                return (result.AsNoTracking().ToList(), total, totalDisplay);
            else
                return (result.ToList(), total, totalDisplay);
        }
    }
}
