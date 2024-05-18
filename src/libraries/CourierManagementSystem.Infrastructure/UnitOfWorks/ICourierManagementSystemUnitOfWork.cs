using CourierManagementSystem.Infrastructure.Repositories;
using DevSkill.Data;

namespace CourierManagementSystem.Infrastructure.UnitOfWorks;

public interface ICourierManagementSystemUnitOfWork : IUnitOfWork
{
    public IBookParcelRepository BookParcels { get; }
    public IItemRepository Items { get; }
}
