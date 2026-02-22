using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Nop.Core.Domain.Customers;
using Nop.Plugin.Misc.CustomerReminder.Data;
using Nop.Plugin.Misc.CustomerReminder.Factories;
using Nop.Plugin.Misc.CustomerReminder.Models;
using Nop.Plugin.Misc.CustomerReminder.Services;
using Nop.Services.Customers;
using Nop.Services.Helpers;
using Nop.Services.Messages;
using Nop.Web.Areas.Admin.Controllers;
using Nop.Web.Framework.Mvc.Filters;
using Nop.Web.Framework.UI;


//[Area("Admin")]
//[AuthorizeAdmin]Does not need because BaseAdminController is in already have this
public class CustomerReminderController : BaseAdminController
{
    private readonly ICustomerReminderService _service;
    private readonly ICustomerService _customerService;
    private readonly ICustomerReminderModelFactory _modelFactory;
    private readonly INotificationService _notificationService;
    private readonly IDateTimeHelper _dateTimeHelper;

    public CustomerReminderController( ICustomerReminderService service, ICustomerService customerService, ICustomerReminderModelFactory modelFactory, INotificationService notificationService, IDateTimeHelper dateTimeHelper)
    {
        _service = service;
        _customerService = customerService;
        _modelFactory = modelFactory;
        _notificationService = notificationService;
        _dateTimeHelper = dateTimeHelper;
    }

    public async Task<IActionResult> List()
    {
        var model =new CustomerReminderSearchModel();
        model.SetGridPageSize();
        return View("~/Plugins/Misc.CustomerReminder/Views/CustomerReminder/List.cshtml", model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> List(CustomerReminderSearchModel searchModel)
    {
        var model = await _modelFactory
            .PrepareCustomerReminderListModelAsync(searchModel);

        return Json(new
        {
            draw = Convert.ToInt32(model.Draw),
            recordsTotal = model.RecordsTotal,
            recordsFiltered = model.RecordsFiltered,
            data = model.Data
        });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CustomerList(int page, int pageSize, string search)
    {
        var (customers, total) =
            await _service.SearchCustomersAsync(search, page, pageSize);

        var data = customers.Select(c => new
        {
            Id = c.Id,
            FullName = string.IsNullOrWhiteSpace(c.FirstName + c.LastName)
                ? c.Email
                : $"{c.FirstName} {c.LastName} ({c.Email})"
        });

        return Json(new
        {
            Data = data,
            Total = total
        });
    }

    public IActionResult Create()
    {
        var model = new CustomerReminderModel
        {
            ReminderDate = _dateTimeHelper.ConvertToUtcTime(DateTime.UtcNow)
        };

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

        return RedirectToAction("List", "CustomerReminder", new { area = "Admin" });
    }

    public async Task<IActionResult> Edit(int id)
    {
        var record = await _service.GetByIdAsync(id);

        if (record == null)
            return RedirectToAction("List");

        return View("~/Plugins/Misc.CustomerReminder/Views/CustomerReminder/Edit.cshtml", record);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(CustomerReminderRecord model)
    {
        if (!ModelState.IsValid)
            return View("~/Plugins/Misc.CustomerReminder/Views/CustomerReminder/Edit.cshtml", model);

        await _service.UpdateAsync(model);
        return RedirectToAction("List", "CustomerReminder", new { area = "Admin" });
    }

    public async Task<IActionResult> Delete(int id)
    {
        var record = await _service.GetByIdAsync(id);

        if (record != null)
            await _service.DeleteAsync(record);

        return RedirectToAction("List", "CustomerReminder", new { area = "Admin" });
    }

}
