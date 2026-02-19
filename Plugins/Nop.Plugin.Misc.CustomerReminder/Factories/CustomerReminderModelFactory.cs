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

        // get paged reminders
        var reminders = await _reminderService.GetAllPagedAsync(
     pageIndex: searchModel.Page - 1,
     pageSize: searchModel.PageSize,
     customerId: searchModel.CustomerId,
     isSent: searchModel.IsSent,
     fromDate: searchModel.FromDate,
     toDate: searchModel.ToDate);


        var model = new CustomerReminderListModel();

        // explicitly specify the generic types
        model.PrepareToGrid<CustomerReminderListModel, CustomerReminderModel, CustomerReminderRecord>(
            searchModel,
            reminders,
            () => reminders.Select(reminder => new CustomerReminderModel
            {
                Id = reminder.Id,
                CustomerId = reminder.CustomerId,
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
