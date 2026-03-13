using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Nop.Plugin.Misc.CustomerReminder.Domain;
using Nop.Plugin.Misc.CustomerReminder.Factories;
using Nop.Plugin.Misc.CustomerReminder.Models;
using Nop.Plugin.Misc.CustomerReminder.Services;
using Nop.Services.Customers;
using Nop.Services.Helpers;
using Nop.Services.Messages;
using Nop.Services.Security;
using Nop.Web.Areas.Admin.Controllers;

public class CustomerReminderController : BaseAdminController
{

    #region Fields

    private readonly ICustomerReminderService _service;
    private readonly ICustomerService _customerService;
    private readonly ICustomerReminderModelFactory _modelFactory;
    private readonly INotificationService _notificationService;
    private readonly IDateTimeHelper _dateTimeHelper;
    private readonly IPermissionService _permissionService;

    #endregion

    #region Ctor

    public CustomerReminderController(
        ICustomerReminderService service,
        ICustomerService customerService,
        ICustomerReminderModelFactory modelFactory,
        INotificationService notificationService,
        IDateTimeHelper dateTimeHelper,
        IPermissionService permissionService)
    {
        _service = service;
        _customerService = customerService;
        _modelFactory = modelFactory;
        _notificationService = notificationService;
        _dateTimeHelper = dateTimeHelper;
        _permissionService = permissionService;
    }

    #endregion

    #region Methods

    #region LIST

    public async Task<IActionResult> List()
    {
        //if(await _permissionService.AuthorizeAsync())
        //{
        //    return AccessDeniedView();
        //}
        var model = new CustomerReminderSearchModel();
        model.SetGridPageSize();
        return View("~/Plugins/Misc.CustomerReminder/Views/CustomerReminder/List.cshtml", model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> List(CustomerReminderSearchModel searchModel)
    {
        var sortColumn = Convert.ToInt32(Request.Form["order[0][column]"]);
        var sortDirection = Request.Form["order[0][dir]"].ToString();
        var model = await _modelFactory
        .PrepareCustomerReminderListModelAsync(searchModel, sortColumn, sortDirection);

        return Json(new
        {
            draw = Convert.ToInt32(searchModel.Draw),   // must be number
            recordsTotal = model.RecordsTotal,
            recordsFiltered = model.RecordsFiltered,
            data = model.Data
        });
    }

    [HttpPost]
    [ValidateAntiForgeryToken] 
    public async Task<IActionResult> CustomerList(int page, int pageSize, string search) 
    { 
        var (customers, total) = await _service.SearchCustomersAsync(search, page, pageSize);
        var data = customers.Select(c => new { Id = c.Id, FullName = string.IsNullOrWhiteSpace(c.FirstName + c.LastName) ? c.Email : $"{c.FirstName} {c.LastName} ({c.Email})" }); 
        return Json(new { Data = data, Total = total }); 
    }

    #endregion

    #region CREATE

    public async Task<IActionResult> Create()
    {
        var model = await _modelFactory
            .PrepareCustomerReminderModelAsync(new CustomerReminderModel(), null);

        return View("~/Plugins/Misc.CustomerReminder/Views/CustomerReminder/Create.cshtml", model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CustomerReminderModel model)
    {
        if (!ModelState.IsValid)
            return View("~/Plugins/Misc.CustomerReminder/Views/CustomerReminder/Create.cshtml", model);

        var customer = await _customerService.GetCustomerByIdAsync(model.CustomerId);
        if (customer == null)
        {
            ModelState.AddModelError("", "Selected customer does not exist.");
            return View("~/Plugins/Misc.CustomerReminder/Views/CustomerReminder/Create.cshtml", model);
        }

        var entity = new CustomerReminderRecord
        {
            CustomerId = model.CustomerId,
            ReminderTitle = model.ReminderTitle,
            ReminderMessage = model.ReminderMessage,
            ReminderDate = _dateTimeHelper.ConvertToUtcTime(model.ReminderDate),
            CreatedOnUtc = DateTime.UtcNow,
            IsSent = false
        };

        await _service.InsertAsync(entity);

        _notificationService.SuccessNotification("Customer reminder created successfully.");

        return RedirectToAction("List");
    }

    #endregion

    #region EDIT

    public async Task<IActionResult> Edit(int id)
    {
        var entity = await _service.GetByIdAsync(id);

        if (entity == null)
            return RedirectToAction("List");

        var model = await _modelFactory
            .PrepareCustomerReminderModelAsync(null, id);

        return View("~/Plugins/Misc.CustomerReminder/Views/CustomerReminder/Edit.cshtml", model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(CustomerReminderModel model)
    {
        if (!ModelState.IsValid)
            return View("~/Plugins/Misc.CustomerReminder/Views/CustomerReminder/Edit.cshtml", model);

        var entity = await _service.GetByIdAsync(model.Id);

        if (entity == null)
            return RedirectToAction("List");

        entity.CustomerId = model.CustomerId;
        entity.ReminderTitle = model.ReminderTitle;
        entity.ReminderMessage = model.ReminderMessage;
        entity.ReminderDate = _dateTimeHelper.ConvertToUtcTime(model.ReminderDate);
        entity.IsSent = model.IsSent;

        await _service.UpdateAsync(entity);

        _notificationService.SuccessNotification("Customer reminder updated successfully.");

        return RedirectToAction("List");
    }

    #endregion

    #region DELETE

    [HttpPost]
    public async Task<IActionResult> Delete(int id)
    {
        var entity = await _service.GetByIdAsync(id);

        if (entity != null)
        {
            await _service.DeleteAsync(entity);
            _notificationService.SuccessNotification("Customer reminder deleted successfully.");
        }

        return RedirectToAction("List");
    }

    #endregion

    #endregion
}