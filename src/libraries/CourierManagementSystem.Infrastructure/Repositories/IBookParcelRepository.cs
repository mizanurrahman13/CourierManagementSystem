using DevSkill.Data;
using BookParcelEntity = CourierManagementSystem.Infrastructure.Entities.BookParcel;
using BookParcelBO = CourierManagementSystem.Infrastructure.BusinessObjects.BookParcel;
using System.Linq.Expressions;

namespace CourierManagementSystem.Infrastructure.Repositories;

public interface IBookParcelRepository : IRepository<BookParcelEntity, Guid>
{
    Task<int> IsBookingAlreadyExists(BookParcelBO bookParcel);
    IList<BookParcelEntity> Get(Expression<Func<BookParcelEntity, bool>> filter, string includeProperties = "");
    Task<IList<BookParcelEntity>> GetAsync(Expression<Func<BookParcelEntity, bool>> filter, string includeProperties = "");
    
    (IList<BookParcelEntity> data, int total, int totalDisplay) GetDynamic(
        Expression<Func<BookParcelEntity, bool>> filter = null!,
        string orderBy = null!,
        string includeProperties = "", int pageIndex = 1, int pageSize = 10, bool isTrackingOff = false);
}
