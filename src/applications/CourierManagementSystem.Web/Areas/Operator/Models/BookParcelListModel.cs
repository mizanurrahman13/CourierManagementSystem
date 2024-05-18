using AutoMapper;
using CourierManagementSystem.Infrastructure.Entities.Users;
using Org.BouncyCastle.Security;
using DevSkill.Http.Utilities;
using CourierManagementSystem.Infrastructure.Services;
using Autofac;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.CodeAnalysis;

namespace CourierManagementSystem.Web.Areas.Operator.Models;

public class BookParcelListModel
{
    private readonly IBookParcelService? _bookParcelService;
    private readonly IItemService? _itemService;

    private IHttpContextAccessor? _httpContextAccessor;
    private ILifetimeScope? _scope;
    private IMapper? _mapper;

    public BookParcelListModel()
    {

    }

    public BookParcelListModel(IHttpContextAccessor httpContextAccessor,
        IBookParcelService? bookParcelService,
        IItemService? itemService,
        IMapper mapper)
    {
        _bookParcelService = bookParcelService;
        _httpContextAccessor = httpContextAccessor;
        _itemService = itemService;
        _mapper = mapper;
    }

    public async Task<object> GetBookParcelAsync(DataTablesAjaxRequestModel model)
    {
        var data = await _bookParcelService!.GetActiveBookParcelsAsync(model.PageIndex, model.PageSize,
            model.SearchText, model.GetSortText(new string[] { "BookParcelFromName", "BookParcelToName" }));

#pragma warning disable CS8602 // Dereference of a possibly null reference.
#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        return new
        {
            recordsTotal = data.total,
            recordsFiltered = data.totalDisplay,
            data = (from bookParcelInfo in data.bookParcels
                    orderby bookParcelInfo.CreatedDate descending
                    select new string[]
                    {
                            (bookParcelInfo.Items!=null)?
                                bookParcelInfo.Items.Select(x => x.Name).FirstOrDefault().ToString():string.Empty,//->0
                            (bookParcelInfo.Items!=null)?
                            String.Join(",", bookParcelInfo.Items.Select(x =>
                            {
                                var res = _itemService!.GetItemById(x.Id);
                                return "[{"+res.Id+"}^{"+res.Name+"}]";
                            }
                            )):string.Empty,//->2
                            bookParcelInfo.BookParcelFromName!,//->1
                            bookParcelInfo.BookParcelFromName!,//->2                            
                            bookParcelInfo.BookParcelFromPhoneNumber.ToString(),//->3
                            bookParcelInfo.BookParcelToPhoneNumber.ToString(),//->4
                            bookParcelInfo.IsActive.ToString(), //5
                            bookParcelInfo.Id.ToString(),//->6
                    }).ToArray()
        };
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
#pragma warning restore CS8602 // Dereference of a possibly null reference.
    }

    public object? GetBookParcels(DataTablesAjaxRequestModel model)
    {
        if (model == null)
            throw new InvalidParameterException("DataTable Request Invalid.");

        var data = _bookParcelService!.GetActiveBookParcels(
            model.PageIndex,
            model.PageSize,
            model.SearchText,
            model.GetSortText(new string[] { "BookParcelFromName", "BookParcelToName" }));

#pragma warning disable CS8602 // Dereference of a possibly null reference.
#pragma warning disable CS8601 // Possible null reference assignment.
        if (data.bookParcels == null)
            throw new InvalidParameterException("Booking Data Not Found.");

        return new
        {
            recordsTotal = data.total,
            recordsFiltered = data.totalDisplay,
            data = (from bookParcelInfo in data.bookParcels
                    orderby bookParcelInfo.CreatedDate descending
                    select new string[]
                    {
                            (bookParcelInfo.Items!=null)?
                                bookParcelInfo.Items.Select(x => x.Name).FirstOrDefault().ToString():string.Empty,//->0
                            (bookParcelInfo.Items!=null)?
                            String.Join(",", bookParcelInfo.Items.Select(x =>
                            {
                                var res = _itemService!.GetItemById(x.Id);
                                return "[{"+res.Id+"}^{"+res.Name+"}]";
                            }
                            )):string.Empty,//->2
                            bookParcelInfo.BookParcelFromName!,//->1
                            bookParcelInfo.BookParcelFromName!,//->2                            
                            bookParcelInfo.BookParcelFromPhoneNumber.ToString(),//->3
                            bookParcelInfo.BookParcelToPhoneNumber.ToString(),//->4
                            bookParcelInfo.IsActive.ToString(), //5
                            bookParcelInfo.Id.ToString(),//->6
                    }).ToArray()
        };
#pragma warning restore CS8601 // Possible null reference assignment.
#pragma warning restore CS8602 // Dereference of a possibly null reference.
    }

    public void Delete(Guid id)
    {
        if (id == Guid.Empty)
            throw new InvalidParameterException("Booking id can not be empty.");

        _bookParcelService?.DeleteBookParcel(id);
    }
}
