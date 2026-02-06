using Nop.Plugin.Misc.CustomerReminder.Data;

namespace Nop.Plugin.Misc.CustomerReminder.Services
{
    public interface ICustomerReminderService
    {
        Task<IList<CustomerReminderRecord>> GetAllAsync();
        Task<CustomerReminderRecord?> GetByIdAsync(int id);
        Task InsertAsync(CustomerReminderRecord record);
        Task UpdateAsync(CustomerReminderRecord record);
        Task DeleteAsync(CustomerReminderRecord record);
    }
}
