using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nop.Plugin.Misc.CustomerReminder.Data;
using Nop.Plugin.Misc.CustomerReminder.Models;
using Nop.Plugin.Misc.CustomerReminder.Services;
using Nop.Services.Customers;
using Nop.Web.Framework.Models.Extensions;

namespace Nop.Plugin.Misc.CustomerReminder.Factories;

public class CustomerReminderModelFactory : ICustomerReminderModelFactory
{
    private readonly ICustomerReminderService _reminderService;
    private readonly ICustomerService _customerService;

    public CustomerReminderModelFactory(
        ICustomerReminderService reminderService,
        ICustomerService customerService)
    {
        _reminderService = reminderService;
        _customerService = customerService;
    }

    public async Task<CustomerReminderModel> PrepareCustomerReminderModelAsync(
        CustomerReminderModel model,
        int? reminderId)
    {
        if (reminderId.HasValue)
        {
            var entity = await _reminderService.GetByIdAsync(reminderId.Value);

            if (entity != null)
            {
                model ??= new CustomerReminderModel();

                model.Id = entity.Id;
                model.CustomerId = entity.CustomerId;
                model.ReminderTitle = entity.ReminderTitle;
                model.ReminderMessage = entity.ReminderMessage;
                model.ReminderDate = entity.ReminderDate;
                model.IsSent = entity.IsSent;
            }
        }

        return model;
    }

    public async Task<CustomerReminderListModel> PrepareCustomerReminderListModelAsync(CustomerReminderSearchModel searchModel)
    {
        ArgumentNullException.ThrowIfNull(searchModel);

        var reminders = await _reminderService.GetAllPagedAsync(
            pageIndex: searchModel.Page - 1,
            pageSize: searchModel.PageSize,
            customerId: searchModel.CustomerId,
            isSent: searchModel.IsSent,
            fromDate: searchModel.FromDate,
            toDate: searchModel.ToDate);

        // 🔥 Get all distinct customer IDs from this page
        var customerIds = reminders
            .Select(x => x.CustomerId)
            .Distinct()
            .ToList();

        // 🔥 Load customers once (avoid duplicate calls)
        var customerDictionary = new Dictionary<int, string>();

        foreach (var id in customerIds)
        {
            var customer = await _customerService.GetCustomerByIdAsync(id);

            if (customer != null)
            {
                customerDictionary[id] =
                    $"{customer.FirstName} {customer.LastName} ({customer.Email})";
            }
            else
            {
                customerDictionary[id] = "Deleted Customer";
            }
        }

        var model = new CustomerReminderListModel();

        model.PrepareToGrid<CustomerReminderListModel, CustomerReminderModel, CustomerReminderRecord>(
            searchModel,
            reminders,
            () => reminders.Select(reminder => new CustomerReminderModel
            {
                Id = reminder.Id,
                CustomerId = reminder.CustomerId,
                CustomerName = customerDictionary.ContainsKey(reminder.CustomerId)
                    ? customerDictionary[reminder.CustomerId]
                    : "Unknown",
                ReminderTitle = reminder.ReminderTitle,
                ReminderMessage = reminder.ReminderMessage,
                ReminderDate = reminder.ReminderDate,
                IsSent = reminder.IsSent,
                CreatedOnUtc = reminder.CreatedOnUtc
            })
        );

        return model;
    }

}
