using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nop.Plugin.Misc.CustomerReminder.Domain;
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
        else
        {
            // 🔥 IMPORTANT FOR CREATE
            model.ReminderDate = DateTime.Now;
        }

        return model;
    }

    public async Task<CustomerReminderListModel> PrepareCustomerReminderListModelAsync(
    CustomerReminderSearchModel searchModel,
    int sortColumn,
    string sortDirection)
    {
        ArgumentNullException.ThrowIfNull(searchModel);

        // Get paged reminders with sorting
        var reminders = await _reminderService.GetAllPagedAsync(
            pageIndex: searchModel.Page - 1,
            pageSize: searchModel.PageSize,
            customerId: searchModel.CustomerId,
            isSent: searchModel.IsSent,
            fromDate: searchModel.FromDate,
            toDate: searchModel.ToDate,
            sortColumn: sortColumn,
            sortDirection: sortDirection);

        // Get unique customer IDs from page
        var customerIds = reminders
            .Select(x => x.CustomerId)
            .Distinct()
            .ToArray();

        // Load customers in one query (better performance)
        var customers = await _customerService.GetCustomersByIdsAsync(customerIds);

        var customerDictionary = customers.ToDictionary(
            c => c.Id,
            c => $"{c.FirstName} {c.LastName} ({c.Email})"
        );

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
                    : "Deleted Customer",
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
