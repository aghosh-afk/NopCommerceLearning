using Nop.Core;
using Nop.Core.Domain.Customers;
using Nop.Plugin.Misc.CustomerReminder.Domain;

namespace Nop.Plugin.Misc.CustomerReminder.Services
{
    public interface ICustomerReminderService
    {
        Task<IPagedList<CustomerReminderRecord>> GetAllPagedAsync(int pageIndex,
    int pageSize,
    int? customerId = null,
    bool? isSent = null,
    DateTime? fromDate = null,
    DateTime? toDate = null,
    int sortColumn = 0,
    string sortDirection = "desc");
        Task<CustomerReminderRecord?> GetByIdAsync(int id);
        Task InsertAsync(CustomerReminderRecord record);
        Task UpdateAsync(CustomerReminderRecord record);
        Task DeleteAsync(CustomerReminderRecord record);
        Task<(IList<Customer> Customers, int TotalCount)> SearchCustomersAsync(string search, int page, int pageSize);
    }
}
