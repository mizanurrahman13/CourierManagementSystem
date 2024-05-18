using AutoMapper;
using DevSkill.Core.Utilities;
using Org.BouncyCastle.Security;
using ItemEntity = CourierManagementSystem.Infrastructure.Entities.Item;
using ItemBO = CourierManagementSystem.Infrastructure.BusinessObjects.Item;
using CourierManagementSystem.Infrastructure.UnitOfWorks;
using CourierManagementSystem.Infrastructure.Exceptions;

namespace CourierManagementSystem.Infrastructure.Services;

public class ItemService : IItemService
{
    private readonly ICourierManagementSystemUnitOfWork _courierManagementSystemUnitOfWork;
    private readonly IMapper _mapper;
    private readonly ICurrentUserService _currentUserService;

    public ItemService(ICourierManagementSystemUnitOfWork courierManagementSystemUnitOfWork,
        IMapper mapper,
        ICurrentUserService currentUserService)
    {
        _courierManagementSystemUnitOfWork = courierManagementSystemUnitOfWork;
        _mapper = mapper;
        _currentUserService = currentUserService;
    }
    public async Task CreateItem(ItemBO item)
    {
        var count = await _courierManagementSystemUnitOfWork.Items
            .GetCountAsync(x => x.Name == item.Name);

        if (count == 0)
        {
            var itemEntity = _mapper.Map<ItemEntity>(item);

            var id = IdentityGenerator.NewSequentialGuid();
            itemEntity.Id = id;

            itemEntity.CreatedBy = await _currentUserService.GetUsername();
            itemEntity.UpdatedBy = await _currentUserService.GetUsername();

            await _courierManagementSystemUnitOfWork.Items.AddAsync(itemEntity);
            await _courierManagementSystemUnitOfWork.SaveAsync();
        }
        else
            throw new DuplicateException("Item with same name already exists");
    }

    public async Task<(int total, int displayTotal, IList<ItemBO> records)> GetItemAsync(int pageIndex, int pageSize, string searchText, string orderBy)
    {
        List<ItemBO> items = new List<ItemBO>();
        var result = await _courierManagementSystemUnitOfWork.Items.GetDynamicAsync(x => x.Name!.Contains(searchText),
            orderBy, null, pageIndex, pageSize, true);

        foreach (ItemEntity entitiy in result.data)
        {
            items.Add(_mapper.Map<ItemBO>(entitiy));
        }

        return (result.total, result.totalDisplay, items);
    }

    public async Task<ItemBO> GetItemByIdAsync(Guid id)
    {
        if (id == Guid.Empty)
            throw new InvalidParameterException("Category id can't be null");

        var itemEntity = await _courierManagementSystemUnitOfWork.Items.GetByIdAsync(id);

        if (itemEntity is null)
            throw new InvalidOperationException("Item with this id not found");

        var item = _mapper.Map<ItemBO>(itemEntity);

        return item;
    }

    public ItemBO GetItemById(Guid id)
    {
        if (id == Guid.Empty)
            throw new InvalidParameterException("Item id can't be null");

        var itemEntity = _courierManagementSystemUnitOfWork.Items.GetById(id);

        if (itemEntity is null)
            throw new InvalidOperationException("Item with this id not found");

        var item = _mapper.Map<ItemBO>(itemEntity);

        return item;
    }

    public async Task UpdateItemAsync(ItemBO item)
    {
        var count = await _courierManagementSystemUnitOfWork.Items.GetCountAsync(x => x.Id != item.Id && x.Name == item.Name);

        if (count != 0)
            throw new InvalidOperationException("Item name already exists");

        var itemEntity = await _courierManagementSystemUnitOfWork.Items.GetByIdAsync(item.Id);

        if (itemEntity is null)
            throw new InvalidOperationException("Item with this id not found.");

        itemEntity = _mapper.Map(item, itemEntity);

        itemEntity.CreatedBy = await _currentUserService.GetUsername();
        itemEntity.UpdatedBy = await _currentUserService.GetUsername();

        await _courierManagementSystemUnitOfWork.SaveAsync();
    }

    public async Task DeleteItemAsync(Guid id)
    {
        if (id == Guid.Empty)
            throw new InvalidParameterException("Item id can't be null");

        await _courierManagementSystemUnitOfWork.Items.RemoveAsync(id);
        await _courierManagementSystemUnitOfWork.SaveAsync();
    }

    public async Task<List<ItemBO>> GetItemsAsync(Guid? id)
    {
        var itemsEO = await _courierManagementSystemUnitOfWork.Items.GetAsync(x => x.Id == id, null!);
        var itemsBO = _mapper.Map<List<ItemBO>>(itemsEO);

        return itemsBO;
    }

    public void DeleteItem(Guid id)
    {
        if (id == Guid.Empty)
            throw new InvalidParameterException("Item id can't be null");

        _courierManagementSystemUnitOfWork.Items.Remove(id);
        _courierManagementSystemUnitOfWork.Save();
    }    

    public async Task<IList<ItemBO>> GetAllAsync()
    {
        var itemEntities = await _courierManagementSystemUnitOfWork.Items.GetAllAsync();

        List<ItemBO> items = new List<ItemBO>();

        foreach (ItemEntity entity in itemEntities)
        {
            items.Add(_mapper.Map<ItemBO>(entity));
        }

        return items;
    }

    public IList<ItemBO> GetAll()
    {
        var itemEntities = _courierManagementSystemUnitOfWork.Items.GetAll();

        List<ItemBO> items = new List<ItemBO>();

        foreach (ItemEntity entity in itemEntities)
        {
            items.Add(_mapper.Map<ItemBO>(entity));
        }

        return items;
    }
}
