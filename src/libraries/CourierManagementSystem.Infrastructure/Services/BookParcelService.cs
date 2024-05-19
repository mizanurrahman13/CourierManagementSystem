using AutoMapper;
using CourierManagementSystem.Infrastructure.UnitOfWorks;
using DevSkill.Core.Utilities;
using BookParcelEntity = CourierManagementSystem.Infrastructure.Entities.BookParcel;
using BookParcelBO = CourierManagementSystem.Infrastructure.BusinessObjects.BookParcel;
using Org.BouncyCastle.Security;

namespace CourierManagementSystem.Infrastructure.Services;

public class BookParcelService : IBookParcelService
{
    private readonly ICourierManagementSystemUnitOfWork _courierManagementSystemUnitOfWork;
    private readonly IMapper _mapper;
    private readonly ICurrentUserService _currentUserService;

    public BookParcelService(ICourierManagementSystemUnitOfWork courierManagementSystemUnitOfWork,
        IMapper mapper,
        ICurrentUserService currentUserService)
    {
        _courierManagementSystemUnitOfWork = courierManagementSystemUnitOfWork;
        _mapper = mapper;
        _currentUserService = currentUserService;
    }
    public async Task CreateBookParcel(BookParcelBO bookParcel)
    {
        var count = await _courierManagementSystemUnitOfWork.BookParcels
            .GetCountAsync(x => x.BookParcelFromName == bookParcel.BookParcelFromName
            && x.CreatedDate == DateTime.UtcNow);

        if (count == 0)
        {
            var bookParcelEntity = _mapper.Map<BookParcelEntity>(bookParcel);

            var id = IdentityGenerator.NewSequentialGuid();
            bookParcelEntity.Id = id;

            bookParcelEntity.CreatedBy = await _currentUserService.GetUsername();
            bookParcelEntity.UpdatedBy = await _currentUserService.GetUsername();

            await _courierManagementSystemUnitOfWork.BookParcels.AddAsync(bookParcelEntity);
            await _courierManagementSystemUnitOfWork.SaveAsync();
        }
        else
            //throw new DuplicateException("BookParcel with same name already exists");
            throw new Exception("BookParcel with BookingFromName same name already exists");
    }

    public async Task<(int total, int displayTotal, IList<BookParcelBO> records)> GetBookParcelAsync(int pageIndex, int pageSize, string searchText, string orderBy)
    {
        List<BookParcelBO> bookParcels = new List<BookParcelBO>();
        var result = await _courierManagementSystemUnitOfWork.BookParcels.GetDynamicAsync(x => x.BookParcelFromName!.Contains(searchText),
            orderBy, null, pageIndex, pageSize, true);

        foreach (BookParcelEntity entitiy in result.data)
        {
            bookParcels.Add(_mapper.Map<BookParcelBO>(entitiy));
        }

        return (result.total, result.totalDisplay, bookParcels);
    }

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
    public async Task<(int total, int totalDisplay, IList<BookParcelBO> bookParcels)> GetActiveBookParcelsAsync(int pageIndex, int pageSize, string searchText, string orderBy)
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
    {
#pragma warning disable CS8602 // Dereference of a possibly null reference.
        var result = _courierManagementSystemUnitOfWork.BookParcels.GetDynamic(x => x.BookParcelFromName.Contains(searchText) && !x.IsActive,
            orderBy, "Items", pageIndex, pageSize, true);
#pragma warning restore CS8602 // Dereference of a possibly null reference.

        List<BookParcelBO> products = new List<BookParcelBO>();
        foreach (BookParcelEntity product in result.data)
        {
            products.Add(_mapper.Map<BookParcelBO>(product));
        }

        return (result.total, result.totalDisplay, products);
    }

    public (int total, int totalDisplay, IList<BookParcelBO> bookParcels) GetActiveBookParcels(int pageIndex, int pageSize, string searchText, string orderBy)
    {
#pragma warning disable CS8602 // Dereference of a possibly null reference.
        var result = _courierManagementSystemUnitOfWork.BookParcels.GetDynamic(x => x.BookParcelFromName.Contains(searchText) && !x.IsActive,
            orderBy, "Items", pageIndex, pageSize, true);
#pragma warning restore CS8602 // Dereference of a possibly null reference.

        if (result.totalDisplay == 0 && result.total == 0 && result.data == null)
            throw new NullReferenceException("Check filter property.");

        List<BookParcelBO> products = new List<BookParcelBO>();
        foreach (BookParcelEntity product in result.data)
        {
            products.Add(_mapper.Map<BookParcelBO>(product));
        }

        return (result.total, result.totalDisplay, products);
    }

    public async Task<BookParcelBO> GetBookParcelByIdAsync(Guid id)
    {
        var bookParcelEntity = await _courierManagementSystemUnitOfWork.BookParcels.GetByIdAsync(id);

        if (bookParcelEntity is null)
            throw new InvalidOperationException("Booking with this id not found");

        var bookParcel = _mapper.Map<BookParcelBO>(bookParcelEntity);
        return bookParcel;
    }

    public BookParcelBO GetBookParcelById(Guid id)
    {
        var bookParcelEntity = _courierManagementSystemUnitOfWork.BookParcels.GetById(id);

        if (bookParcelEntity is null)
            throw new InvalidOperationException("Booking with this id not found");

        var bookParcel = _mapper.Map<BookParcelBO>(bookParcelEntity);
        return bookParcel;
    }

    public async Task UpdateBookParcelAsync(BookParcelBO bookParcel)
    {
        var today = DateTime.UtcNow;
        var count = await _courierManagementSystemUnitOfWork.BookParcels.GetCountAsync(x => x.Id != bookParcel.Id 
            && x.BookParcelFromName == bookParcel.BookParcelFromName
            &&x.CreatedDate == today);

        if (count > 0)
            throw new InvalidOperationException("Booking with same name already exists.");

        var bookParcelEntity = _courierManagementSystemUnitOfWork.BookParcels.Get(x => x.Id.Equals(bookParcel.Id),
                                "Items").FirstOrDefault();

        bookParcelEntity!.Items = null;
        if (bookParcelEntity is null)
            throw new InvalidOperationException("Booking with this id not found.");

        bookParcelEntity.Items = null;
        _mapper.Map<BookParcelBO>(bookParcelEntity);

        bookParcelEntity.CreatedBy = await _currentUserService.GetUsername();
        bookParcelEntity.UpdatedBy = await _currentUserService.GetUsername();

        await _courierManagementSystemUnitOfWork.SaveAsync();
    }

    public async Task UpdateBookParcelAndItemAsync(BookParcelBO bookParcel)
    {
        var count = await _courierManagementSystemUnitOfWork.BookParcels
            .GetCountAsync(x => x.Id != bookParcel.Id 
                && x.BookParcelFromName == bookParcel.BookParcelFromName
                && x.CreatedDate == DateTime.UtcNow);

        if (count > 0)
            throw new InvalidOperationException("Booking with same name already exists.");

        var bookParcelEntity = _courierManagementSystemUnitOfWork.BookParcels.Get(x => x.Id.Equals(bookParcel.Id),
                                "Items").FirstOrDefault();

        if (bookParcelEntity is null)
            throw new InvalidOperationException("Booking with this id not found.");

        bookParcelEntity = _mapper.Map(bookParcel, bookParcelEntity);

        bookParcelEntity.CreatedBy = await _currentUserService.GetUsername();
        bookParcelEntity.UpdatedBy = await _currentUserService.GetUsername();

        await _courierManagementSystemUnitOfWork.SaveAsync();
    }

    public async void UpdateBookParcel(BookParcelBO bookParcel)
    {
        if (bookParcel is null)
            throw new InvalidOperationException("Booking must be provided to update item");

        var count = await _courierManagementSystemUnitOfWork.BookParcels.IsBookingAlreadyExists(bookParcel);

        if (count > 0)
            throw new InvalidOperationException("Booking name already exists");

        var bookParcelEntity = _courierManagementSystemUnitOfWork.BookParcels.Get(x => x.Id.Equals(bookParcel.Id),
                                string.Empty).FirstOrDefault();

        bookParcelEntity = _mapper.Map(bookParcel, bookParcelEntity);

        bookParcelEntity!.CreatedBy = _currentUserService.GetUsernames();
        bookParcelEntity!.UpdatedBy = _currentUserService.GetUsernames();

        _courierManagementSystemUnitOfWork.Save();
    }

    public BookParcelBO GetProductItemById(Guid Id)
    {
        if (Id == Guid.Empty)
            throw new InvalidParameterException("Id must be provided to get product including images.");

        var result = _courierManagementSystemUnitOfWork.
             BookParcels.Get(x => x.Id.Equals(Id),
             "Items").FirstOrDefault();

        var bookParcel = _mapper.Map<BookParcelBO>(result);

        return bookParcel;
    }

    public (int total, int totalDisplay, IList<BookParcelBO> trashedBookParcels) GetTrashedBookParcels(
                int pageIndex, int pageSize, string searchText, string orderBy)
    {
        var result = _courierManagementSystemUnitOfWork.BookParcels.GetDynamic(x => x.IsActive,
            orderBy, "Items", pageIndex, pageSize, true);

        if (result.totalDisplay == 0 && result.total == 0 && result.data == null)
            throw new NullReferenceException("Check filter property.");

        IList<BookParcelBO> trashedBookParcels = new List<BookParcelBO>();
        foreach (var bookParcel in result.data)
        {
            trashedBookParcels.Add(_mapper.Map<BookParcelBO>(bookParcel));
        }

        return (result.total, result.totalDisplay, trashedBookParcels);
    }

    public void DeleteBookParcel(Guid id)
    {
        var entity = _courierManagementSystemUnitOfWork.BookParcels.GetById(id);

        if (entity is null)
            throw new InvalidOperationException("Booking with this id not found.");

        _courierManagementSystemUnitOfWork.BookParcels.Remove(id);
        _courierManagementSystemUnitOfWork.Save();
    }
}
