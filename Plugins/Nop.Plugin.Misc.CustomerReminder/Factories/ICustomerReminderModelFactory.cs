using System.Threading.Tasks;
using Nop.Plugin.Misc.CustomerReminder.Models;

namespace Nop.Plugin.Misc.CustomerReminder.Factories;

public interface ICustomerReminderModelFactory
{
    Task<CustomerReminderModel> PrepareCustomerReminderModelAsync(
        CustomerReminderModel model,
        int? reminderId);

    Task<CustomerReminderListModel> PrepareCustomerReminderListModelAsync(
    CustomerReminderSearchModel searchModel,
    int sortColumn,
    string sortDirection);
}
