using Microsoft.AspNetCore.Mvc;
using Nop.Plugin.Misc.CustomerReminder.Data;
using Nop.Plugin.Misc.CustomerReminder.Services;
using Nop.Web.Areas.Admin.Controllers;
using Nop.Web.Framework.Mvc.Filters;

[Area("Admin")]
[AuthorizeAdmin]
public class CustomerReminderController : BaseAdminController
{
    private readonly ICustomerReminderService _service;

    public CustomerReminderController(ICustomerReminderService service)
    {
        _service = service;
    }

    public async Task<IActionResult> List()
    {
        var model = await _service.GetAllAsync();
        return View("~/Plugins/Misc.CustomerReminder/Views/CustomerReminder/List.cshtml", model);
    }


    public IActionResult Create()
    {
        return View("~/Plugins/Misc.CustomerReminder/Views/CustomerReminder/Create.cshtml");

    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CustomerReminderRecord model)
    {
        if (!ModelState.IsValid)
        {
            return View("~/Plugins/Misc.CustomerReminder/Views/CustomerReminder/Create.cshtml", model);
        }

        model.ReminderDate = DateTime.SpecifyKind(model.ReminderDate, DateTimeKind.Utc);
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







}
