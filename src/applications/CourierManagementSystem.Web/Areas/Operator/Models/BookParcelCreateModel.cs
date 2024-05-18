using ItemBO = CourierManagementSystem.Infrastructure.BusinessObjects.Item;
using BookParcelBO = CourierManagementSystem.Infrastructure.BusinessObjects.BookParcel;
using Autofac;
using AutoMapper;
using static System.Formats.Asn1.AsnWriter;
using CourierManagementSystem.Infrastructure.Services;
using Org.BouncyCastle.Security;
using CourierManagementSystem.Infrastructure.Enums;

namespace CourierManagementSystem.Web.Areas.Operator.Models;

public class BookParcelCreateModel
{
    public Guid Id { get; set; }

    public string BookParcelFromName { get; set; } = string.Empty;
    public string BookParcelFromPhoneNumber { get; set; } = string.Empty;
    public string BookParcelFromAddress { get; set; } = string.Empty;

    public string BookParcelToName { get; set; } = string.Empty;
    public string BookParcelToPhoneNumber { get; set; } = string.Empty;
    public string BookParcelToAddress { get; set; } = string.Empty;

    public DateTimeOffset? CreatedDate { get; set; }
    public DateTimeOffset? UpdatedDate { get; set; }
    public string? CreatedBy { get; set; }
    public string? UpdatedBy { get; set; }
    public bool IsActive { get; set; }
    public Status Status { get; set; }
    public string? TracId { get; set; }

    public IList<ItemBO>? Items { get; set; }

    private IHttpContextAccessor? _httpContextAccessor;
    private ILifetimeScope? _scope;
    private IMapper? _mapper;

    private IBookParcelService? _bookParcelService;

    public BookParcelCreateModel()
    {

    }

    public BookParcelCreateModel(IHttpContextAccessor httpContextAccessor,
        IBookParcelService? bookParcelService,
        IMapper mapper)
    {
        _bookParcelService = bookParcelService;
        _httpContextAccessor = httpContextAccessor;
        _mapper = mapper;
    }

    public void Resolve(ILifetimeScope scope)
    {
        _scope = scope;
        _bookParcelService = _scope.Resolve<IBookParcelService>();
    }

    public async Task CreateBookParcel(IList<ItemBO> items)
    {
        var bookParcel = _mapper!.Map<BookParcelBO>(this);
        if (this.IsActive == true)
            bookParcel.IsActive = true;
        else
            bookParcel.IsActive = false;

        bookParcel.Items = new List<ItemBO>();

        if (bookParcel.Items == null!)
            throw new InvalidParameterException("Booking Parcel Item is null");

        foreach (var item in items)
        {
            bookParcel.Items?.Add(new ItemBO
            {
                Name = item.Name,
                Description = item.Description,
                Quantity = item.Quantity,
                Total = item.Total,
                CreatedBy = item.CreatedBy,
                CreatedDate = item.CreatedDate,
                UpdatedBy = item.UpdatedBy,
                UpdatedDate = item.UpdatedDate,
                //BookParcelId = item.BookParcelId,
            });
        }
        
        await _bookParcelService!.CreateBookParcel(bookParcel);
    }
}
