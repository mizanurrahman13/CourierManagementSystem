using Autofac;
using CourierManagementSystem.Infrastructure.Exceptions;
using CourierManagementSystem.Web.Areas.Operator.Models;
using DevSkill.Http.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ItemBO = CourierManagementSystem.Infrastructure.BusinessObjects.Item;

namespace CourierManagementSystem.Web.Areas.Operator.Controllers;

[Area("Operator")]
[Authorize(Roles = "Admin")]
public class BookParcelController : Controller
{
    private readonly ILifetimeScope _lifetimeScope;
    private readonly ILogger<BookParcelController> _logger;

    public BookParcelController(ILogger<BookParcelController> logger,
        ILifetimeScope lifetimeScope)
    {
        _lifetimeScope = lifetimeScope;
        _logger = logger;
    }
    //public IActionResult Index()
    //{
    //    return View();
    //}

    public JsonResult GetProducts()
    {
        var model = _lifetimeScope.Resolve<BookParcelListModel>();
        var dataTableModel = new DataTablesAjaxRequestModel(Request);
        var list = model.GetBookParcels(dataTableModel);

        return Json(list);
    }

    [HttpGet]
    public IActionResult Create()
    {
        var model = _lifetimeScope.Resolve<BookParcelCreateModel>();

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(BookParcelCreateModel model)
    {
        var errors = ModelState.Values.SelectMany(v => v.Errors);

        if (ModelState.IsValid)
        {
            model.Resolve(_lifetimeScope);
            try
            {                
                var itemList = model?.Items;

                List<ItemBO> items = new List<ItemBO>();

                if(itemList != null)
                {
                    foreach (var item in itemList)
                    {
                        items?.Add(new ItemBO
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
                }

                await model?.CreateBookParcel(items)!;

                TempData["message"] = $"Booking for {model?.BookParcelToName} is successfully added.";

                return RedirectToAction("Index");
            }
            catch (DuplicateException ioe)
            {
                _logger.LogError(ioe, ioe.Message);

                TempData["message"] = ioe.Message;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);

                TempData["message"] = "Booking could not be added due to some error";
            }
        }

        return View(model);
    }
}
