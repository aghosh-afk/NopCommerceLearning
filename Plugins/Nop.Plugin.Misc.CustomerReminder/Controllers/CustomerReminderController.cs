using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Nop.Core.Domain.Customers;
using Nop.Plugin.Misc.CustomerReminder.Data;
using Nop.Plugin.Misc.CustomerReminder.Factories;
using Nop.Plugin.Misc.CustomerReminder.Models;
using Nop.Plugin.Misc.CustomerReminder.Services;
using Nop.Services.Customers;
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

    public CustomerReminderController( ICustomerReminderService service, ICustomerService customerService, ICustomerReminderModelFactory modelFactory)
    {
        _service = service;
        _customerService = customerService;
        _modelFactory = modelFactory;
    }

    public async Task<IActionResult> List()
    {
        var model =new CustomerReminderSearchModel();
        model.SetGridPageSize();
        return View("~/Plugins/Misc.CustomerReminder/Views/CustomerReminder/List.cshtml", model);
    }

    [HttpPost]
    public async Task<IActionResult> List(CustomerReminderSearchModel searchModel)
    {
        var model = await _modelFactory
            .PrepareCustomerReminderListModelAsync(searchModel);

        return Json(model);
    }


    [HttpPost]
    [AuthorizeAdmin]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CustomerList(int page, int pageSize, string search)
    {
        var customers = await _customerService.GetAllCustomersAsync(
            email: search,
            pageIndex: page - 1,
            pageSize: pageSize
        );

        var data = customers.Select(c => new
        {
            id = c.Id,
            name = $"{c.FirstName} {c.LastName} ({c.Email})"
        });

        return Json(new
        {
            Data = data,
            Total = customers.TotalCount
        });
    }
    public IActionResult Create()
    {
        var model = new CustomerReminderRecord
        {
            ReminderDate = DateTime.UtcNow
        };

        return View("~/Plugins/Misc.CustomerReminder/Views/CustomerReminder/Create.cshtml", model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CustomerReminderRecord model)
    {
        if (!ModelState.IsValid)
            return View("~/Plugins/Misc.CustomerReminder/Views/CustomerReminder/Create.cshtml", model);

        var customer = await _customerService.GetCustomerByIdAsync(model.CustomerId);
        if (customer == null)
        {
            ModelState.AddModelError("", "Selected customer does not exist.");
            return View("~/Plugins/Misc.CustomerReminder/Views/CustomerReminder/Create.cshtml", model);
        }

        model.ReminderDate = model.ReminderDate.ToUniversalTime();
        model.CreatedOnUtc = DateTime.UtcNow;
        model.IsSent = false;

        await _service.InsertAsync(model);

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

    [HttpGet]
    public async Task<IActionResult> GetCustomers(string text)
    {
        // Fetch customers
        var customers = await _customerService.GetAllCustomersAsync(
            email: null,
            username: null,
            firstName: text,
            lastName: text,
            pageIndex: 0,
            pageSize: 50);

        // Prepare result with "Select Customer..." at top
        var result = new List<object>
    {
        new { Id = 0, FullName = "Select Customer..." } // default option
    };

        result.AddRange(customers.Select(c => new
        {
            Id = c.Id,
            FullName = $"{c.FirstName} {c.LastName}"
        }));

        return Json(result);
    }


}
