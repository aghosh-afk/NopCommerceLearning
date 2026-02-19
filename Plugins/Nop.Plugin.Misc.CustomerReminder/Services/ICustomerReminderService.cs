using Nop.Core;
using Nop.Plugin.Misc.CustomerReminder.Data;

namespace Nop.Plugin.Misc.CustomerReminder.Services
{
    public interface ICustomerReminderService
    {
        Task<IPagedList<CustomerReminderRecord>> GetAllPagedAsync(int pageIndex,
    int pageSize,
    int? customerId = null,
    bool? isSent = null,
    DateTime? fromDate = null,
    DateTime? toDate = null);
        Task<CustomerReminderRecord?> GetByIdAsync(int id);
        Task InsertAsync(CustomerReminderRecord record);
        Task UpdateAsync(CustomerReminderRecord record);
        Task DeleteAsync(CustomerReminderRecord record);
    }
}
