using CourierManagementSystem.Infrastructure.Contexts;
using CourierManagementSystem.Infrastructure.Repositories;
using DevSkill.Data;

namespace CourierManagementSystem.Infrastructure.UnitOfWorks;

public class CourierManagementSystemUnitOfWork : UnitOfWork, ICourierManagementSystemUnitOfWork
{
    public IBookParcelRepository BookParcels { get; private set; }
    public IItemRepository Items { get; private set; }

    public CourierManagementSystemUnitOfWork(ApplicationDbContext applicationDbContext,
        IBookParcelRepository bookParcelRepository,
        IItemRepository itemRepository)
        : base(applicationDbContext)
    {
        BookParcels = bookParcelRepository;
        Items = itemRepository;
    }
}
